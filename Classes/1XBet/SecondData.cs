using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes._1XBet
{
    public partial class SecondData
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
        [JsonProperty("CI")]
        public long Ci { get; set; }

        [JsonProperty("CN")]
        public string Cn { get; set; }

        [JsonProperty("COI")]
        public long Coi { get; set; }

        [JsonProperty("E")]
        public E[] E { get; set; }

        [JsonProperty("EC")]
        public long Ec { get; set; }

        [JsonProperty("HS")]
        public long Hs { get; set; }

        [JsonProperty("HSI")]
        public bool Hsi { get; set; }

        [JsonProperty("I")]
        public long I { get; set; }

        [JsonProperty("L")]
        public string L { get; set; }

        [JsonProperty("LI")]
        public long Li { get; set; }

        [JsonProperty("LR")]
        public string Lr { get; set; }

        [JsonProperty("MIO")]
        public Mio Mio { get; set; }

        [JsonProperty("MIS")]
        public Mi[] Mis { get; set; }

        [JsonProperty("MS")]
        public long[] Ms { get; set; }

        [JsonProperty("N")]
        public long N { get; set; }

        [JsonProperty("O1")]
        public string O1 { get; set; }

        [JsonProperty("O1C")]
        public long O1C { get; set; }

        [JsonProperty("O1CT")]
        public string O1Ct { get; set; }

        [JsonProperty("O1I")]
        public long O1I { get; set; }

        [JsonProperty("O1IMG")]
        public string[] O1Img { get; set; }

        [JsonProperty("O1IS")]
        public long[] O1Is { get; set; }

        [JsonProperty("O1R")]
        public string O1R { get; set; }

        [JsonProperty("O2")]
        public string O2 { get; set; }

        [JsonProperty("O2C")]
        public long O2C { get; set; }

        [JsonProperty("O2CT")]
        public string O2Ct { get; set; }

        [JsonProperty("O2I")]
        public long O2I { get; set; }

        [JsonProperty("O2IMG")]
        public string[] O2Img { get; set; }

        [JsonProperty("O2IS")]
        public long[] O2Is { get; set; }

        [JsonProperty("O2R")]
        public string O2R { get; set; }

        [JsonProperty("S")]
        public long UnixTime { get; set; }

        [JsonProperty("SGI")]
        public string Sgi { get; set; }

        [JsonProperty("SI")]
        public long Si { get; set; }

        [JsonProperty("SN")]
        public string Sn { get; set; }

        [JsonProperty("SR")]
        public string Sr { get; set; }

        [JsonProperty("SS")]
        public long Ss { get; set; }

        [JsonProperty("SSI")]
        public long Ssi { get; set; }

        [JsonProperty("SST")]
        public long Sst { get; set; }

        [JsonProperty("STI")]
        public string Sti { get; set; }

        [JsonProperty("T")]
        public long T { get; set; }

        [JsonProperty("TN")]
        public string Tn { get; set; }

        [JsonProperty("B")]
        public long B { get; set; }
    }

    public partial class E
    {
        [JsonProperty("C")]
        public double C { get; set; }

        [JsonProperty("G")]
        public long G { get; set; }

        [JsonProperty("T")]
        public long T { get; set; }

        [JsonProperty("P", NullValueHandling = NullValueHandling.Ignore)]
        public double? P { get; set; }
    }

    public partial class Mio
    {
        [JsonProperty("Loc")]
        public string Loc { get; set; }

        [JsonProperty("TSt")]
        public string TSt { get; set; }
    }

    public partial class Mi
    {
        [JsonProperty("K")]
        public long K { get; set; }

        [JsonProperty("V")]
        public string V { get; set; }
    }


}
