using Newtonsoft.Json;
using Scraper.Models.MerryBet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{

    public class ScrapeMerryBet
    {
        string x = DateTime.Now.ToString();
        public List<MerrybetData> ScrapeDaily(HttpClient client)
        {
            var currdate = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            var tomodate = DateTime.Now.AddDays(1).ToString("yyyy'-'MM'-'dd");
            var listEvents = new List<MerrybetData>();


            try
            {

                //get todays data
                string FirstresponseBody = client.GetStringAsync("https://merrybet.com/rest/search/events/search-by-date/" + currdate).Result;

                //get next day data
                string SecondresponseBody = client.GetStringAsync("https://merrybet.com/rest/search/events/search-by-date/" + currdate).Result;
                //get first day list of events
                var t = JsonConvert.DeserializeObject<SearchData>(FirstresponseBody).data.Select(x => x.eventId).ToList();

                //get second day list of events and add to previous list
                t.AddRange(JsonConvert.DeserializeObject<SearchData>(SecondresponseBody).data.Select(x => x.eventId).ToList());

                for (int i = 0; i < t.Count; i++)
                {
                    //get id of event
                    var eventId = t[i];

                    //get data for event
                    var singleresponse = client.GetStringAsync("https://merrybet.com/rest/market/events/" + eventId);
                    var singleEventData = JsonConvert.DeserializeObject<MBData.ReceivedDataMerryBet>(FirstresponseBody);

                    var events = singleEventData.data.eventGames[i];

                    var mbOdds = new List<DailyMerrybetOdds>();
                    for (int j = 0; j < events.outcomes.Count; j++)
                    {
                        var outcome = events.outcomes[j];

                        mbOdds.Add(new DailyMerrybetOdds() { MainType = events.gameName, Selection = outcome.outcomeName, Value = outcome.outcomeOdds.ToString() });

                    }

                    var mbOddsnGames = new MerrybetData()
                    {
                        DateOfMatch = DateTimeOffset.FromUnixTimeMilliseconds(1564234200000).DateTime.Date.ToString(),
                        League = singleEventData.data.category3Name,
                        TeamNames = singleEventData.data.eventName,
                        Odds = mbOdds,
                        TimeOfMatch = DateTimeOffset.FromUnixTimeMilliseconds(1564234200000).DateTime.TimeOfDay.ToString()
                    };

                    listEvents.Add(mbOddsnGames);
                }

                return listEvents;

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }
    }
}
