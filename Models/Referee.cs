using schedule.Enums;
using System;
using System.Collections.Generic;

namespace schedule.Models
{
    public class Referee
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public List<AddressTimeSlot> AddressTimeSlots { get; set; } = new List<AddressTimeSlot>();

        public List<DateTime> CurrentDates { get; set; } = new List<DateTime>();

        public List<DateTime> UncomfortDates { get; set; } = new List<DateTime>();

        public int CurrentQuantityOfAdultMatches { get; set; }

        public int CurrentQuantityOfChildMatches { get; set; }

        public int CurrentQuantityOfAdultWorks { get; set; }

        public int CurrentQuantityOfChildWorks { get; set; }

        public int Rang { get; set; }

        public List<MatchType> Groops { get; set; } = new List<MatchType>();

        public int ChildMatchesForRef { get; set; }

        public int ChildWorksForRef { get; set; }

        public int AdultMatchesForRef { get; set; }

        public int AdultWorksForRef { get; set; }

        public double Priority { get; set; }

        public Referee(string name)
        {
            Name = name;
        }

        public Referee(CsvReferee csvReferee)
        {
            Name = csvReferee.Name;
            AdultMatchesForRef = csvReferee.AdultMatchesForRef;
            AdultWorksForRef = csvReferee.AdultWorksForRef;
            ChildMatchesForRef = csvReferee.ChildMatchesForRef;
            ChildWorksForRef = csvReferee.ChildWorksForRef;
            CurrentDates = csvReferee.CurrentDates;
            CurrentQuantityOfAdultMatches = csvReferee.CurrentQuantityOfAdultMatches;
            CurrentQuantityOfAdultWorks = csvReferee.CurrentQuantityOfAdultWorks;
            CurrentQuantityOfChildMatches = csvReferee.CurrentQuantityOfChildMatches;
            CurrentQuantityOfChildWorks = csvReferee.CurrentQuantityOfChildWorks;
            Groops = csvReferee.Groops;
            Id = csvReferee.Id;
            Priority = csvReferee.Priority;
            Rang = csvReferee.Rang;
            UncomfortDates = csvReferee.UncomfortDates;
            AddressTimeSlots.Add(new AddressTimeSlot(csvReferee.Adress, csvReferee.AdressZone, csvReferee.TimeSlot));
        }
        public string ToCsvString()
        {
            return $"{Name};{CurrentQuantityOfAdultWorks};{CurrentQuantityOfAdultMatches};{CurrentQuantityOfChildWorks};{CurrentQuantityOfChildMatches}";
        }
    }
}
