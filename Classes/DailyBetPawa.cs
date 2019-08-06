using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class DailyPawaMatches
    {
        public string League { get; set; }
        public int Id { get; set; }
        public string TeamNames { get; set; }
        public string TimeOfMatch { get; set; }
        public DateTime DateOfMatch { get; set; }
        public List<BetPawaOdds> Odds { get; set; }
    }




}
