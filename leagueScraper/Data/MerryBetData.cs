using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace leagueScraper.Data.MerryBetData
{

    public partial class MerrybetData
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("data")]
        public Datum[] Data { get; set; }
    }

    public partial class Datum
    {
        [JsonProperty("categoryId")]
        public long CategoryId { get; set; }

        [JsonProperty("remoteId")]
        public long RemoteId { get; set; }

        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("parentCategory")]
        public long ParentCategory { get; set; }

        [JsonProperty("sportId")]
        public long SportId { get; set; }

        [JsonProperty("eventsCount")]
        public long EventsCount { get; set; }

        [JsonProperty("sortOrder")]
        public long SortOrder { get; set; }

        [JsonProperty("treatAsSport")]
        public long TreatAsSport { get; set; }

        [JsonProperty("categoryFlag")]
        public CategoryFlag CategoryFlag { get; set; }

        [JsonProperty("parentName")]
        public string ParentName { get; set; }

        [JsonProperty("sportName")]
        public string SportName { get; set; }
    }

    public enum CategoryFlag { Null };


    public static class Serialize
    {
        public static string ToJson(this MerrybetData self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                CategoryFlagConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class CategoryFlagConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(CategoryFlag) || t == typeof(CategoryFlag?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "null")
            {
                return CategoryFlag.Null;
            }
            throw new Exception("Cannot unmarshal type CategoryFlag");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (CategoryFlag)untypedValue;
            if (value == CategoryFlag.Null)
            {
                serializer.Serialize(writer, "null");
                return;
            }
            throw new Exception("Cannot marshal type CategoryFlag");
        }

        public static readonly CategoryFlagConverter Singleton = new CategoryFlagConverter();
    }

}
