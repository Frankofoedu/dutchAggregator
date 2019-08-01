using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.MerryBet
{

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
        public List<object> marketTypes { get; set; }
        public int periodId { get; set; }
        public int gameLayout { get; set; }
        public int eventLayout { get; set; }
        public List<Outcome> outcomes { get; set; }
        public List<int?> combinationTypes { get; set; }
    }

    public class Datum
    {
        public int eventId { get; set; }
        public int remoteId { get; set; }
        public string eventName { get; set; }
        public object eventStart { get; set; }
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
        public int? category3AggregatedId { get; set; }
    }

    public class SearchData
    {
        public int code { get; set; }
        public string description { get; set; }
        public List<Datum> data { get; set; }
    }
}

