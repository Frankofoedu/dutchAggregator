using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dutchBet.Models.Tennis
{
    
    public class Rootobject
    {
        public bool success { get; set; }
        public string site { get; set; }
        public string sport { get; set; }
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
        public string XTU { get; set; }
        public string XTO { get; set; }
        public string ES3 { get; set; }
        public string ES4 { get; set; }
        public string ES5 { get; set; }
        public string ODD { get; set; }
        public string EVEN { get; set; }
        public string GH1 { get; set; }
        public string GH2 { get; set; }
        public string HWY { get; set; }
        public string HWN { get; set; }
        public string AWY { get; set; }
    }

}