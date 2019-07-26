using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper.Models
{
    public class BetPawa
    {
        public int Id { get; set; }
        public string League { get; set; }
        public List<BetPawaMatches> Matches { get; set; }
    }

    public class BetPawaMatches
    {
        public int Id { get; set; }
        public string TeamNames { get; set; }
        public string TimeOfMatch { get; set; }
        public string DateOfMatch { get; set; }
        public List<BetPawaOdds> Odds { get; set; }
    }
    public class BetPawaOdds
    {
        public int Id { get; set; }
        public string MainType { get; set; }
        public string Type { get; set; }
        public string Selection { get; set; }
        public string Value { get; set; }

        public string MapType { get { return MainType + Type; } set { _ = MainType + Type; } }

    }

  
    [NotMapped]
    public class Rootobject
    {
        public bool Success { get; set; }
        public object[] Errors { get; set; }
        public Data Data { get; set; }
    }
    [NotMapped]
    public class Data
    {
        public Event[] Events { get; set; }
        public int[] RemainingEventIds { get; set; }
    }
    [NotMapped]
    public class Event
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int ExId { get; set; }
        public bool Live { get; set; }
        public object HomeScore { get; set; }
        public object AwayScore { get; set; }
        public object GameMinute { get; set; }
        public string Path { get; set; }
        public Ptp[] Ptps { get; set; }
        public string Starts { get; set; }
        public DateTime StartsRaw { get; set; }
        public Market[] Markets { get; set; }
        public int TotalMarketCount { get; set; }
        public string Region { get; set; }
        public string League { get; set; }
        public string Sport { get; set; }
        public int SportId { get; set; }
        public string CountryPath { get; set; }
        public string GroupPath { get; set; }
        public int GroupId { get; set; }
        public bool Boosted { get; set; }
    }
    [NotMapped]
    public class Ptp
    {
        public string N { get; set; }
        public int E { get; set; }
    }
    [NotMapped]
    public class Market
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string GroupedName { get; set; }
        public int Priority { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int GroupPriority { get; set; }
        public object HandicapInt { get; set; }
        public string HandicapType { get; set; }
        public string PriceSorting { get; set; }
        public string PricePresentationType { get; set; }
        public object PresentationColumns { get; set; }
        public Price[] Prices { get; set; }
        public bool Boosted { get; set; }
    }
    [NotMapped]
    public class Price
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Type { get; set; }

        [JsonProperty(PropertyName = "Price")]
        public string Cost { get; set; }
        public float PriceRaw { get; set; }
        public bool Hot { get; set; }
        public object Hcp { get; set; }
    }


    #region Returned data
 
    public class Datum
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int ExId { get; set; }
        public bool Live { get; set; }
        public object HomeScore { get; set; }
        public object AwayScore { get; set; }
        public object GameMinute { get; set; }
        public string Path { get; set; }
        public List<Ptp> Ptps { get; set; }
        public string Starts { get; set; }
        public DateTime StartsRaw { get; set; }
        public List<Market> Markets { get; set; }
        public int TotalMarketCount { get; set; }
        public string Region { get; set; }
        public string League { get; set; }
        public string Sport { get; set; }
        public int SportId { get; set; }
        public string CountryPath { get; set; }
        public string GroupPath { get; set; }
        public int GroupId { get; set; }
        public bool Boosted { get; set; }
    }

    public class RootObject
    {
        public bool Success { get; set; }
        public List<object> Errors { get; set; }
        public List<Datum> Data { get; set; }
    }
    #endregion
}
