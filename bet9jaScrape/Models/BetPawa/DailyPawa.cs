using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper.Models
{
    public class DailyPawaMatches
    {
        public string League { get; set; }
        public int Id { get; set; }
        public string TeamNames { get; set; }
        public string TimeOfMatch { get; set; }
        public string DateOfMatch { get; set; }
        public List<BetPawaOdds> Odds { get; set; }
    }

    public class DailyPawaOdds
    {
        public int Id { get; set; }
        public string MainType { get; set; }
        public string Type { get; set; }
        public string Selection { get; set; }
        public string Value { get; set; }
        [NotMapped]
        public string MapType { get { return MainType + Type; } }

    }

}
