using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Bet9ja
    {
        public string League { get; set; }
        public List<Bet9jaMatches> Matches { get; set; }
    }

    public class Bet9jaMatches
    {
        public string TeamNames { get; set; }
        public string MatchTime { get; set; }
        public List<Bet9jaOdds> Odds { get; set; }
    }
    public class Bet9jaOdds
    {
        public string Type { get; set; }
        public string Selection { get; set; }
        public string Value { get; set; }

        public string SelectionFull
        {
            get
            {
                return (Type + "-" + Selection).ToLower();
            }
        }
    }
}
