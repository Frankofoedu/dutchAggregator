using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.BetPawaData
{
    public  class SingleReceivedData
    {
        public class Ptp
        {
            public string N { get; set; }
            public object E { get; set; }
        }

        public class Price
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public int Type { get; set; }

            [JsonProperty(PropertyName = "Price")]
            public string Cost { get; set; }
            public double PriceRaw { get; set; }
            public bool Hot { get; set; }
            public string Hcp { get; set; }
        }

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
            public int? HandicapInt { get; set; }
            public string HandicapType { get; set; }
            public string PriceSorting { get; set; }
            public string PricePresentationType { get; set; }
            public object PresentationColumns { get; set; }
            public List<Price> Prices { get; set; }
            public bool Boosted { get; set; }
        }

        public class Data
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
            public string StartsRaw { get; set; }
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
            public Data Data { get; set; }
        }


    }
}
