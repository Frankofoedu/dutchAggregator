using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classes.CalcClasses
{
    public class TwoOddsReturn
    {
        public string Team { get; set; }

        public string Game1 { get; set; }
        public string Game2 { get; set; }

        public string Site1 { get; set; }
        public string Site2 { get; set; }

        public double Odd1 { get; set; }
        public double Odd2 { get; set; }

        public double PercentageToPlay1 { get; set; }
        public double PercentageToPlay2 { get; set; }

        public double PercentageReturns { get; set; }
    }
}