using System;
using schedule.Helpers;
using schedule.Services;
using System.Linq;

namespace schedule
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var csvReferees = FileHelper.ReadReferees("files/судьи1.csv");
            var referees = RefereesHelper.GroupReferees(csvReferees);
            var matches = FileHelper.ReadMatches("files/расписание.csv");

            var refereesService = new RefereesService(referees);
            var matchesService = new MatchesService(matches, refereesService);

            matchesService.SetupReferees();

            matches = matches.OrderBy(m => m.DateOfMatch).ToList();

            FileHelper.WriteMatchesToCsv("готовое.csv", matches);
            FileHelper.WriteRefereesToCsv("готовые судьи.csv", referees);

            Console.WriteLine("Referees have been assigned");
            Console.ReadLine();
        }
    }
}

