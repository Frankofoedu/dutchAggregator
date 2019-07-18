using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dutchBet.Models
{
    public class Match
    {
        public string Teams { get; set; }

        public string Odd1Name { get; set; }
        public List<MatchOdds> MatchOdds1 { get; set; }

        public string Odd2Name { get; set; }
        public List<MatchOdds> MatchOdds2 { get; set; }
    }
    public class MatchOdds
    {
        public string Site { get; set; }
        public double Odd { get; set; }
    }
}