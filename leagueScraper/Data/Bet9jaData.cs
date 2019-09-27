using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace leagueScraper.Data
{
    public partial class Bet9jaData
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }

    public partial class D
    {
        [JsonProperty("__type")]
        public string Type { get; set; }

        [JsonProperty("VisualizationType")]
        public long VisualizationType { get; set; }

        [JsonProperty("VisualizationTypeStartDate")]
        public double VisualizationTypeStartDate { get; set; }

        [JsonProperty("VisualizationTypeEndDate")]
        public double VisualizationTypeEndDate { get; set; }

        [JsonProperty("SportList")]
        public SportList[] SportList { get; set; }
    }

    public partial class SportList
    {
        [JsonProperty("Antepost")]
        public bool Antepost { get; set; }

        [JsonProperty("IDSport")]
        public long IdSport { get; set; }

        [JsonProperty("IDTipoSport")]
        public long IdTipoSport { get; set; }

        [JsonProperty("Sport")]
        public string Sport { get; set; }

        [JsonProperty("NumQuote")]
        public long NumQuote { get; set; }

        [JsonProperty("NumSottoeventi")]
        public long NumSottoeventi { get; set; }

        [JsonProperty("GroupList")]
        public GroupList[] GroupList { get; set; }
    }

    public partial class GroupList
    {
        [JsonProperty("Antepost")]
        public bool Antepost { get; set; }

        [JsonProperty("IDSport")]
        public long IdSport { get; set; }

        [JsonProperty("IDGruppo")]
        public long IdGruppo { get; set; }

        [JsonProperty("Gruppo")]
        public string Gruppo { get; set; }

        [JsonProperty("NumQuote")]
        public long NumQuote { get; set; }

        [JsonProperty("NumSottoeventi")]
        public long NumSottoeventi { get; set; }

        [JsonProperty("EventList")]
        public EventList[] EventList { get; set; }
    }

    public partial class EventList
    {
        [JsonProperty("Antepost")]
        public bool Antepost { get; set; }

        [JsonProperty("IDGruppo")]
        public long IdGruppo { get; set; }

        [JsonProperty("IDEvento")]
        public long IdEvento { get; set; }

        [JsonProperty("Evento")]
        public string Evento { get; set; }

        [JsonProperty("NumQuote")]
        public long NumQuote { get; set; }

        [JsonProperty("NumSottoeventi")]
        public long NumSottoeventi { get; set; }
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
