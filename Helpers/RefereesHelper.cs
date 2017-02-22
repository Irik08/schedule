using System.Collections.Generic;
using System.Linq;
using schedule.Models;

namespace schedule.Helpers
{
    public static class RefereesHelper
    {
        public static List<Referee> GroupReferees(List<CsvReferee> csvReferees)
        {
            var result = new List<Referee>();

            var groupedReferees = csvReferees.GroupBy(r => r.Name);

            foreach (var groupedReferee in groupedReferees)
            {
                var referee = new Referee(groupedReferee.Select(r => r).First());

                foreach (var csvReferee in groupedReferee.Skip(1))
                {
                    referee.AddressTimeSlots.Add(new AddressTimeSlot(csvReferee.Adress, csvReferee.AdressZone, csvReferee.TimeSlot));
                }

                result.Add(referee);
            }

            return result;
        }
    }
}
