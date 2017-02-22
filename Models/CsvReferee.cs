using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using schedule.Enums;

namespace schedule.Models
{
    public class CsvReferee
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public List<string> Adress { get; set; }

        public List<string> AdressZone { get; set; }

        public List<string> TimeSlot { get; set; }

        public List<DateTime> CurrentDates { get; set; }

        public List<DateTime> UncomfortDates { get; set; }

        public int CurrentQuantityOfAdultMatches { get; set; }

        public int CurrentQuantityOfChildMatches { get; set; }

        public int CurrentQuantityOfAdultWorks { get; set; }

        public int CurrentQuantityOfChildWorks { get; set; }

        public int Rang { get; set; }

        public List<MatchType> Groops { get; set; }

        public int ChildMatchesForRef { get; set; }

        public int ChildWorksForRef { get; set; }

        public int AdultMatchesForRef { get; set; }

        public int AdultWorksForRef { get; set; }

        public double Priority { get; set; }

        public CsvReferee(string line)
        {
            string[] parts = line.Split(';');
            int mat;
            double pat;
            int.TryParse(parts[0], out mat);
            Id = mat;
            Name = parts[2];
            
            Groops = parts[3].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(g => (MatchType)int.Parse(g)).ToList();

            Adress = parts[4].Split(' ').ToList();

            AdressZone = new List<string>();
            foreach (var item in Adress)
            {
                int zal;
                int.TryParse(item, out zal);
                if (zal < 10) { AdressZone.Add("1"); }
                else if (zal < 12) { AdressZone.Add("2"); }
                else if (zal < 16) { AdressZone.Add("3"); }
                else if (zal < 17) { AdressZone.Add("4"); }
                else if (zal < 23) { AdressZone.Add("5"); }
                else if (zal < 24) { AdressZone.Add("6"); }
                else if (zal < 26) { AdressZone.Add("7"); }
                else if (zal < 29) { AdressZone.Add("8"); }
                else if (zal < 31) { AdressZone.Add("9"); }
                else if (zal < 34) { AdressZone.Add("10"); }
                else if (zal < 36) { AdressZone.Add("11"); }
                else if (zal < 39) { AdressZone.Add("12"); }
                else { AdressZone.Add("13"); }
            }
            TimeSlot = parts[5].Split(' ').ToList();
            CurrentDates = new List<DateTime>();

            UncomfortDates = new List<DateTime>();
            List<string> segments = parts[6].Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string elem in segments)
            {
                List<string> segment = elem.Split('-').ToList();
                DateTime dateOfStart = DateTime.ParseExact(segment[0], Constants.DateTimeFormat1, CultureInfo.InvariantCulture);
                DateTime dateOfEnd = DateTime.ParseExact(segment[1], Constants.DateTimeFormat1, CultureInfo.InvariantCulture);
                for (var date = dateOfStart; date <= dateOfEnd; date = date.AddDays(1))
                {
                    UncomfortDates.Add(date);
                }
            }           

            int.TryParse(parts[7], out mat);
            ChildWorksForRef = mat;
            int.TryParse(parts[8], out mat);
            AdultWorksForRef = mat;
            int.TryParse(parts[9], out mat);
            Rang = mat;
            //int.TryParse(parts[10], out mat);
            //CurrentQuantityOfMatches = mat;
            double.TryParse(parts[11], out pat);
            Priority = pat;
        }
        /*public string ToCsvString()
        {
            return $"{Name};{CurrentQuantityOfAdultWorks};{CurrentQuantityOfAdultMatches};{CurrentQuantityOfChildWorks};{CurrentQuantityOfChildMatches}";
        }*/
    }
}
