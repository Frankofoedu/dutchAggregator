using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class SportyBet
    {
        public int Id { get; set; }
        public string League { get; set; }
        public List<SportyBetMatches> Matches { get; set; }
    }

    public class SportyBetMatches
    {
        public int Id { get; set; }
        public string TeamNames { get; set; }
        public string TimeOfMatch { get; set; }
        public string DateOfMatch { get; set; }
        public List<SportyBetOdds> Odds { get; set; }
    }
    public class SportyBetOdds
    {
        public int Id { get; set; }
        public string MainType { get; set; }
        public string Type { get; set; }
        public string Selection { get; set; }
        public string Value { get; set; }

        public string SelectionFull
        {
            get
            {
                return (MainType + "-" + Type + "-" + Selection).ToLower();
            }
        }

    }

}
