﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.SportyBetData
{

    public class SearchData
    {
        public int bizCode { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
    public class Tournament2
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Category
    {
        public string id { get; set; }
        public string name { get; set; }
        public Tournament2 tournament { get; set; }
    }

    public class Sport
    {
        public string id { get; set; }
        public string name { get; set; }
        public Category category { get; set; }
    }

    public class Outcome
    {
        public string id { get; set; }
        public string odds { get; set; }
        public string probability { get; set; }
        public int isActive { get; set; }
        public string desc { get; set; }
    }

    public class Market
    {
        public string id { get; set; }
        public int product { get; set; }
        public string desc { get; set; }
        public int status { get; set; }
        public string group { get; set; }
        public string marketGuide { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public int favourite { get; set; }
        public List<Outcome> outcomes { get; set; }
        public string specifier { get; set; }
    }

    public class Event
    {
        public string eventId { get; set; }
        public string gameId { get; set; }
        public string productStatus { get; set; }
        public object estimateStartTime { get; set; }
        public int status { get; set; }
        public string matchStatus { get; set; }
        public string homeTeamName { get; set; }
        public string awayTeamName { get; set; }
        public Sport sport { get; set; }
        public int totalMarketSize { get; set; }
        public List<Market> markets { get; set; }
        public string bookingStatus { get; set; }
        public int commentsNum { get; set; }
        public int topicId { get; set; }
    }

    public class Tournament
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<Event> events { get; set; }
        public string categoryName { get; set; }
        public string categoryId { get; set; }
    }

    public class Data
    {
        public int totalNum { get; set; }
        public List<Tournament> tournaments { get; set; }
    }



}
