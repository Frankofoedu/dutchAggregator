using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.MerryBet
{


    public class MerrybetData
    {
        public string League { get; set; }
        public int Id { get; set; }
        public string TeamNames { get; set; }
        public string TimeOfMatch { get; set; }
        public string DateOfMatch { get; set; }
        public List<DailyMerrybetOdds> Odds { get; set; }
    }

    public class DailyMerrybetOdds
    {
        public int Id { get; set; }
        public string MainType { get; set; }
        public string Type { get; set; }
        public string Selection { get; set; }
        public string Value { get; set; }
        [NotMapped]
        public string SelectionFull
        {
            get
            {
                return (MainType + "-" + Type + "-" + Selection).ToLower();
            }
        }

        
    }

    public class MBData
    {


        public class ReceivedDataMerryBet
        {
            public int code { get; set; }
            public string description { get; set; }

            [JsonConverter(typeof(IgnoreUnexpectedArraysConverter<Data>))]
            public Data data { get; set; }
        }

        public class Outcome
        {
            public int outcomeId { get; set; }
            public string outcomeName { get; set; }
            public double outcomeOdds { get; set; }
        }

        public class EventGame
        {
            public int gameId { get; set; }
            public string gameName { get; set; }
            public int gameType { get; set; }
            public int gameCode { get; set; }
            public double argument { get; set; }
            public int combinationType { get; set; }
            public List<int> marketTypes { get; set; }
            public int gameLayout { get; set; }
            public int eventLayout { get; set; }
            public List<Outcome> outcomes { get; set; }
            public string gameTypeDefaultName { get; set; }
            public string gameTypePattern { get; set; }
            public List<int?> combinationTypes { get; set; }
        }

        public class EventExtendedData
        {
            public string remoteCategoryId { get; set; }
        }

        public class Data
        {
            public int eventId { get; set; }
            public int remoteId { get; set; }
            public string eventName { get; set; }
            public long eventStart { get; set; }
            public int eventType { get; set; }
            public int category3Id { get; set; }
            public int category2Id { get; set; }
            public int category1Id { get; set; }
            public string category3Name { get; set; }
            public string category2Name { get; set; }
            public string category1Name { get; set; }
            public int eventCodeId { get; set; }
            public int gamesCount { get; set; }
            public List<EventGame> eventGames { get; set; }
            public int treatAsSport { get; set; }
            public EventExtendedData eventExtendedData { get; set; }
        }

    }
}
