using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using schedule.Models;

namespace schedule.Helpers
{
    public static class FileHelper
    {
        public static List<CsvReferee> ReadReferees(string filename)
        {
            return ReadItems(filename, s => new CsvReferee(s));
        }

        public static List<Match> ReadMatches(string filename)
        {
            return ReadItems(filename, s => new Match(s));
        }

        public static void WriteMatchesToCsv(string filename, List<Match> matches)
        {
            using (var csv = new StreamWriter(filename, false, Encoding.Default))
            {
                foreach (var match in matches)
                {
                    csv.WriteLine(match.ToCsvString());
                }
            }
        }

        public static void WriteRefereesToCsv(string filename, List<Referee> referees)
        {
            using (var csv = new StreamWriter(filename, false, Encoding.Default))
            {
                foreach (var referee in referees)
                {
                    csv.WriteLine(referee.ToCsvString());
                }
            }
        }

        private static List<T> ReadItems<T>(string filename, Func<string, T> creator)
        {
            var res = new List<T>();
            using (var sr = new StreamReader(filename, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var item = creator(line);
                    res.Add(item);
                }
            }
            return res;
        } 
    }
}
