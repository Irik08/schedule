using System.Collections.Generic;
using System.Linq;
using schedule.Models;
using schedule.Extensions;
using System;

namespace schedule.Services
{
    public class MatchesService
    {
        private readonly RefereesService _refereesService;
        private readonly List<Match> _matches; 

        public MatchesService(List<Match> matches, RefereesService refereesService)
        {
            _refereesService = refereesService;
            _matches = matches;
        }

        public void SetupReferees()
        {
            SetupReferees(_matches.Where(x => x.IsAdult()).ToList(), art => art.AddressSlots, ma => ma.Adress);
            SetupReferees(_matches.Where(x => x.IsChild()).ToList(), art => art.AddressSlots, ma => ma.Adress);

            SetupReferees(_matches.Where(x => x.IsAdult()).ToList(), art => art.AddressZones, ma => ma.AdressZone);
            SetupReferees(_matches.Where(x => x.IsChild()).ToList(), art => art.AddressZones, ma => ma.AdressZone);
        }

        private void SetupReferees(List<Match> matches, Func<AddressTimeSlot, List<string>> addressPredicate, Func<Match, string> matchAddressPredicate)
        {
            matches.Shuffle();
            matches = matches
                .OrderByDescending(m => !string.IsNullOrWhiteSpace(m.AppointedRefereeFirst) || !string.IsNullOrWhiteSpace(m.AppointedRefereeSecond) ? 1 : 0)
                .ToList();

            var referees = _refereesService.GetAllReferees();

            foreach (var match in matches)
            {
                SetupAppointedReferees(match);

                if (match.RefereeFirst == null)
                {
                    var firstRef = FindReferee(match, referees, addressPredicate, matchAddressPredicate);
                    _refereesService.AssignRefereeToMatch(firstRef, match, m => m.RefereeFirst);
                }

                if (match.RefereeSecond == null && match.RefereeFirst != null && !match.IsChild())
                {
                    var possibleReferees = referees
                        .Where(r => match.RefereeFirst.Rang != 0 || r.Rang != 0)
                        .Except(new[] {match.RefereeFirst})
                        .ToList();

                    var secondRef = FindReferee(
                        match,
                        possibleReferees,
                        addressPredicate,
                        matchAddressPredicate);

                    _refereesService.AssignRefereeToMatch(secondRef, match, m => m.RefereeSecond);
                }

                SetupSameMatches(match, matches);
            }
        }

        private Referee FindReferee(Match match, List<Referee> referees, Func<AddressTimeSlot, List<string>> addressPredicate, Func<Match, string> matchAddressPredicate)
        {
            Referee bestReferee = null;

            double matchKoef = 0;

            foreach (var referee in referees)
            {
                if (((referee.CurrentQuantityOfAdultWorks >= referee.AdultWorksForRef) && match.IsAdult()) ||
               
                    ((referee.CurrentQuantityOfChildWorks >= referee.ChildWorksForRef) && match.IsChild()) ||
                    
                    (referee.CurrentDates.Contains(match.DateOfMatch.Date)) ||

                    (referee.UncomfortDates.Contains(match.DateOfMatch.Date)) ||

                    (!referee.Groops.Contains(match.MatchType)))
                {
                    continue;
                }

                var refereeKoef = referee.Priority;

                var numberOfTimeSlots = referee.AddressTimeSlots
                    .SelectMany(art => art.TimeSlots)
                    .Distinct().Count();
                var numberOfAddressSlots = referee.AddressTimeSlots
                    .SelectMany(addressPredicate)
                    .Distinct().Count();

                var addressTimeSlotKoef = referee.AddressTimeSlots.Max(art =>
                {
                    double koef = 0;

                    if (art.TimeSlots.Contains(match.Time))
                    {
                        koef += Constants.TimeSlotKoef / numberOfTimeSlots;
                    }
                    else
                    {
                        return -1;
                    }

                    if (addressPredicate(art).Contains(matchAddressPredicate(match)))
                    {
                        koef += Constants.AddressSlotKoef / numberOfAddressSlots;
                    }
                    else
                    {
                        return -1;
                    }

                    return koef;
                });

                if (addressTimeSlotKoef < 0)
                {
                    continue;
                }

                refereeKoef += addressTimeSlotKoef;

                if (refereeKoef >= matchKoef)
                {
                    bestReferee = referee;
                    matchKoef = refereeKoef;
                }
            }

            return bestReferee;
        }

        private void SetupAppointedReferees(Match match)
        {
            if (!string.IsNullOrWhiteSpace(match.AppointedRefereeFirst) && (match.RefereeFirst == null))
            {
                var firstRef = _refereesService.FindOrCreateReferee(match.AppointedRefereeFirst);
                _refereesService.AssignRefereeToMatch(firstRef, match, m => m.RefereeFirst);
            }
            if (!string.IsNullOrWhiteSpace(match.AppointedRefereeSecond) && (match.RefereeSecond == null))
            {
                var secondRef = _refereesService.FindOrCreateReferee(match.AppointedRefereeSecond);
                _refereesService.AssignRefereeToMatch(secondRef, match, m => m.RefereeSecond);
            }
        }

        private void SetupSameMatches(Match match, IEnumerable<Match> matches)
        {
            var sameMatches = matches
                .Where(m => m.IsChild() &&
                            m.DateOfMatch.Date == match.DateOfMatch.Date &&
                            m.Adress == match.Adress &&
                            m != match &&
                            m.RefereeFirst == null);

            foreach (var sameMatch in sameMatches)
            {
                _refereesService.AssignRefereeToMatch(match.RefereeFirst, sameMatch, m => m.RefereeFirst);

                if (match.RefereeFirst != null)
                {
                    _refereesService.AssignRefereeToMatch(match.RefereeSecond, sameMatch, m => m.RefereeSecond);
                }
            }
        }
    }
}
