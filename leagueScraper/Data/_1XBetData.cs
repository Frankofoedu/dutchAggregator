using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace leagueScraper.Data._1XBet
{
    public partial class _1XBetData
    {
        [JsonProperty("Error")]
        public string Error { get; set; }

        [JsonProperty("ErrorCode")]
        public long ErrorCode { get; set; }

        [JsonProperty("Guid")]
        public string Guid { get; set; }

        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Success")]
        public bool Success { get; set; }

        [JsonProperty("Value")]
        public Value[] Value { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("C")]
        public long C { get; set; }

        [JsonProperty("I")]
        public long I { get; set; }

        [JsonProperty("N")]
        public string N { get; set; }

        [JsonProperty("R")]
        public string R { get; set; }

        [JsonProperty("L", NullValueHandling = NullValueHandling.Ignore)]
        public L[] L { get; set; }

        [JsonProperty("MS", NullValueHandling = NullValueHandling.Ignore)]
        public long? Ms { get; set; }
    }

    public partial class L
    {
        [JsonProperty("CI")]
        public long Ci { get; set; }

        [JsonProperty("CN")]
        public string Cn { get; set; }

        [JsonProperty("CR")]
        public string Cr { get; set; }

        [JsonProperty("GC")]
        public long Gc { get; set; }

        [JsonProperty("L")]
        public string LL { get; set; }

        [JsonProperty("LI")]
        public long Li { get; set; }

        [JsonProperty("LR")]
        public string Lr { get; set; }

        [JsonProperty("T", NullValueHandling = NullValueHandling.Ignore)]
        public long? T { get; set; }

        [JsonProperty("CHIMG", NullValueHandling = NullValueHandling.Ignore)]
        public string Chimg { get; set; }
    }


    public static class Serialize
    {
        public static string ToJson(this _1XBetData self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
