using schedule.Enums;
using System;
using System.Globalization;
using schedule.Extensions;

namespace schedule.Models
{
    public class Match
    {
        public string MatchName { get; set; }

        public string Hosts { get; set; }

        public string Guests { get; set; }

        public DateTime DateOfMatch { get; set; }

        public Referee RefereeFirst { get; set; }

        public Referee RefereeSecond { get; set; }

        public string AppointedRefereeFirst { get; set; }

        public string AppointedRefereeSecond { get; set; }

        public string Adress { get; set; }

        public string AdressZone { get; set; }

        public string Time { get; set; }

        public int QuantityOfGames { get; set; }

        public MatchType MatchType { get; set; }

        public Match(string line)
        {
            string[] parts = line.Split(';');
            DateOfMatch = DateTime.ParseExact(parts[1]+'.'+parts[3], Constants.DateTimeFormat, CultureInfo.InvariantCulture);

            int mat;
            switch(DateOfMatch.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    if (DateOfMatch.TimeOfDay < Constants.time19)
                    {
                        Time = "1";
                    }
                    else
                    {
                        Time = "2";
                    }
                    break;
                case DayOfWeek.Tuesday:
                    if (DateOfMatch.TimeOfDay < Constants.time19)
                    {
                        Time = "3";
                    }
                    else
                    {
                        Time = "4";
                    }
                    break;
                case DayOfWeek.Wednesday:
                    if (DateOfMatch.TimeOfDay < Constants.time19)
                    {
                        Time = "5";
                    }
                    else
                    {
                        Time = "6";
                    }
                    break;
                case DayOfWeek.Thursday:
                    if (DateOfMatch.TimeOfDay < Constants.time19)
                    {
                        Time = "7";
                    }
                    else
                    {
                        Time = "8";
                    }
                    break;
                case DayOfWeek.Friday:
                    if (DateOfMatch.TimeOfDay < Constants.time19)
                    {
                        Time = "9";
                    }
                    else
                    {
                        Time = "10";
                    }
                    break;
                case DayOfWeek.Saturday:
                    if (DateOfMatch.TimeOfDay < Constants.time13)
                    {
                        Time = "11";
                    }
                    else
                    {
                        if (DateOfMatch.TimeOfDay < Constants.time18)
                        {
                            Time = "12";
                        }
                        else
                        {
                            Time = "13";
                        }
                    }
                    break;
                case DayOfWeek.Sunday:
                    if (DateOfMatch.TimeOfDay < Constants.time13)
                    {
                        Time = "14";
                    }
                    else
                    {
                        if (DateOfMatch.TimeOfDay < Constants.time18)
                        {
                            Time = "15";
                        }
                        else
                        {
                            Time = "16";
                        }
                    }
                    break;
            }

            switch (parts[4])
            {
                case "1 муж":
                    MatchType = MatchType.FirstMen;
                    break;
                case "жен":
                    MatchType = MatchType.FirstWomen;
                    break;
                case "2 муж":
                    MatchType = MatchType.SecondMen;
                    break;
                case "2 жен":
                    MatchType = MatchType.SecondWomen;
                    break;
                case "юн":
                    MatchType = MatchType.Boys;
                    break;
                case "дев":
                    MatchType = MatchType.Girls;
                    break;
                case "мал":
                    MatchType = MatchType.Boys;
                    break;
                case "двч":
                    MatchType = MatchType.Girls;
                    break;
            }

            Hosts = parts[5];
            Guests = parts[6];
            MatchName =$"{Hosts} {Guests}";
            Adress = parts[7];
            
            int zal;
            int.TryParse(Adress, out zal);
            if (zal < 10) { AdressZone = "1"; }
            else if (zal < 12) { AdressZone = "2"; }
            else if (zal < 16) { AdressZone = "3"; }
            else if (zal < 17) { AdressZone = "4"; }
            else if (zal < 23) { AdressZone = "5"; }
            else if (zal < 24) { AdressZone = "6"; }
            else if (zal < 26) { AdressZone = "7"; }
            else if (zal < 29) { AdressZone = "8"; }
            else if (zal < 31) { AdressZone = "9"; }
            else if (zal < 34) { AdressZone = "10"; }
            else if (zal < 36) { AdressZone = "11"; }
            else if (zal < 39) { AdressZone = "12"; }
            else { AdressZone = "13"; }

            int.TryParse(parts[8], out mat);
            QuantityOfGames = mat;

            AppointedRefereeFirst = parts[9];
            AppointedRefereeSecond = parts[10];
            
        }

        public string ToCsvString()
        {
            var secondRef = this.IsAdult() ?  RefereeSecond?.Name ?? "No second ref" : string.Empty;
            return $"{DateOfMatch};{DateOfMatch.DayOfWeek};{Hosts};{Guests};{Adress};{RefereeFirst?.Name ?? "No first ref"};{secondRef}";
        }
    }
}
