using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dutchBet.Models
{
    public class MatchOddsBySite
    {
        public string Match { get; set; }
        public List<SiteOdds> SiteAndOdds { get; set; }
    }
    public class SiteOdds
    {
        public string Site { get; set; }
        public double Odd_1 { get; set; }
        public double Odd_X { get; set; }
        public double Odd_2 { get; set; }
        public double Odd_1X { get; set; }
        public double Odd_12 { get; set; }
        public double Odd_X2 { get; set; }
        public double Odd_Over1_5 { get; set; }
        public double Odd_Under1_5 { get; set; }
        public double Odd_Over2_5 { get; set; }
        public double Odd_Under2_5 { get; set; }
        public double Odd_Over3_5 { get; set; }
        public double Odd_Under3_5 { get; set; }
        public double Odd_Over4_5 { get; set; }
        public double Odd_Under4_5 { get; set; }
        public double Odd_Over5_5 { get; set; }
        public double Odd_Under5_5 { get; set; }
    }
}