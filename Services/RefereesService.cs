using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using schedule.Models;
using schedule.Extensions;

namespace schedule.Services
{
    public class RefereesService
    {
        private readonly List<Referee> _referees;

        public RefereesService(List<Referee> referees)
        {
            _referees = referees;
        }

        public List<Referee> GetAllReferees()
        {
            return _referees;
        }
        
        public Referee FindOrCreateReferee(string name)
        {
            var referee = _referees.FirstOrDefault(r => r.Name == name) ?? new Referee(name);

            return referee;
        } 

        public void AssignRefereeToMatch(Referee referee, Match match, Expression<Func<Match, Referee>> refereeExpression)
        {
            var currentReferee = refereeExpression.Compile().Invoke(match);

            if (currentReferee != null)
            {
                DetachRefereeFromMatch(referee, match, refereeExpression);
            }

            if (referee == null)
            {
                return;
            }

            refereeExpression.SetValue(match, referee);
            AddMatchForReferee(referee, match);
        }

        public void DetachRefereeFromMatch(Referee referee, Match match, Expression<Func<Match, Referee>> refereeExpression)
        {
            var currentReferee = refereeExpression.Compile().Invoke(match);

            if (currentReferee == null)
            {
                return;
            }

            RemoveMatchForReferee(referee, match);
            refereeExpression.SetValue(match, null);
        }

        private static void AddMatchForReferee(Referee referee, Match match)
        {
            referee.CurrentDates.Add(match.DateOfMatch.Date);
            if (match.IsAdult())
            {
                referee.CurrentQuantityOfAdultWorks ++;
                referee.CurrentQuantityOfAdultMatches += match.QuantityOfGames;
            }
            if (match.IsChild())
            {
                referee.CurrentQuantityOfChildWorks ++;
                referee.CurrentQuantityOfChildMatches += match.QuantityOfGames;
            }

        }

        private static void RemoveMatchForReferee(Referee referee, Match match)
        {
            referee.CurrentDates.Remove(match.DateOfMatch.Date);
            if (match.IsAdult())
            {
                referee.CurrentQuantityOfAdultWorks --;
                referee.CurrentQuantityOfAdultMatches -= match.QuantityOfGames;
            }
            if (match.IsChild())
            {
                referee.CurrentQuantityOfChildWorks --;
                referee.CurrentQuantityOfChildMatches -= match.QuantityOfGames;
            }
        }
    }
}
