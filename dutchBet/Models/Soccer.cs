
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dutchBet.Models.Soccer
{
    public class Rootobject
    {
        public bool success { get; set; }
        public string site { get; set; }
        public string league { get; set; }
        public int marketCount { get; set; }
        public Market[] market { get; set; }


    }
    public class Market
    {
        public string date { get; set; }
        public string time { get; set; }
        public string _event { get; set; }
        public Odds odds { get; set; }
    }

    public class Odds
    {
        public string _1 { get; set; }
        public string _2 { get; set; }
        public string _12 { get; set; }
        public string X { get; set; }
        public string _1X { get; set; }
        public string _2X { get; set; }
        public string GG { get; set; }
        public string NG { get; set; }
        public string _1HT { get; set; }
        public string XHT { get; set; }
        public string _2HT { get; set; }
        public string O25 { get; set; }
        public string U25 { get; set; }
    }

}