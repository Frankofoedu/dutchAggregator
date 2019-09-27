using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace leagueScraper.Data.SportyBet
{
    public partial class SportyBetData
    {
        [JsonProperty("bizCode")]
        public long BizCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("sportList")]
        public PopularEvent[] SportList { get; set; }

        [JsonProperty("popularEvents")]
        public PopularEvent[] PopularEvents { get; set; }
    }

    public partial class PopularEvent
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("eventSize")]
        public long EventSize { get; set; }

        [JsonProperty("categories", NullValueHandling = NullValueHandling.Ignore)]
        public PopularEvent[] Categories { get; set; }

        [JsonProperty("tournaments", NullValueHandling = NullValueHandling.Ignore)]
        public Tournament[] Tournaments { get; set; }
    }

    public partial class Tournament
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("eventSize")]
        public long EventSize { get; set; }

        [JsonProperty("categoryName", NullValueHandling = NullValueHandling.Ignore)]
        public string CategoryName { get; set; }

        [JsonProperty("categoryId", NullValueHandling = NullValueHandling.Ignore)]
        public string CategoryId { get; set; }
    }

    public partial class SportyBetData
    {
        public static SportyBetData FromJson(string json) => JsonConvert.DeserializeObject<SportyBetData>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this SportyBetData self) => JsonConvert.SerializeObject(self, Converter.Settings);
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
