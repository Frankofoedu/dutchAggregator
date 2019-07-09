using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dutchBet.Models
{
    public class Actions
    {
        public class twoOddsReturn
        {
            public double Odd1 { get; set; }
            public double Odd2 { get; set; }
            
            public double PercentageToPlay1 { get; set; }
            public double PercentageToPlay2 { get; set; }

            public double PercentageReturns { get; set; }
        }

        public twoOddsReturn calculateForTwoOdds(double odd1, double odd2)
        {
            var x = 1.0 / (odd1 + odd2) * odd2;
            var y = 1.0 / (odd1 + odd2) * odd1;
            double x2 = x * 100;
            double y2 = y * 100;
            double rtn = x2 * odd1;

            return new twoOddsReturn() {
                Odd1 = odd1,
                Odd2=odd2,
                PercentageToPlay1 = x2,
                PercentageToPlay2 = y2,
                PercentageReturns = rtn
            };
        }
    }
}