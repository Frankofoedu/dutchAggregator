using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class BetMatch
    {
        public int Id { get; set; }
        public string Site { get; set; }
        public string Country { get; set; }
        public string League { get; set; }
        public string TeamNames { get; set; }
        public DateTime DateTimeOfMatch { get; set; }
        public List<BetOdds> Odds { get; set; }
    }

    public class BetOdds
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
                return string.IsNullOrWhiteSpace(MainType) ? (Type + "-" + Selection).ToLower() : (MainType + "-" + Type + "-" + Selection).ToLower();
            }
        }

    }
}
