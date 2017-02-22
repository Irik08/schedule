using System.Collections.Generic;
using schedule.Enums;
using schedule.Models;

namespace schedule.Extensions
{
    public static class MatchExtensions
    {
        private static readonly List<MatchType> AdultMatchTypes = new List<MatchType> { MatchType.FirstMen, MatchType.FirstWomen, MatchType.SecondMen, MatchType.SecondWomen };

        private static readonly List<MatchType> ChildMatchTypes = new List<MatchType> { MatchType.Boys, MatchType.Girls };

        public static bool IsAdult(this Match match)
        {
            return AdultMatchTypes.Contains(match.MatchType);
        }

        public static bool IsChild(this Match match)
        {
            return ChildMatchTypes.Contains(match.MatchType);
        }
    }
}
