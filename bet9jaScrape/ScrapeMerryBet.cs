using Newtonsoft.Json;
using Classes.MerryBet;
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
        public List<MerrybetData> ScrapeDaily(HttpClient client)
        {
            var currdate = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            var tomodate = DateTime.Now.AddDays(1).ToString("yyyy'-'MM'-'dd");
            var listEvents = new List<MerrybetData>();
            var jsonSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };


            try
            {

                //get todays data
                string FirstresponseBody = client.GetStringAsync("https://merrybet.com/rest/search/events/search-by-date/" + currdate).Result;

                //get next day data
                string SecondresponseBody = client.GetStringAsync("https://merrybet.com/rest/search/events/search-by-date/" + tomodate).Result;

                //get first day list of events id
                var t = JsonConvert.DeserializeObject<SearchData>(FirstresponseBody).data.Where(m => m.category1Name == "Soccer").Select(x => x.eventId).ToList();

                //get second day list of events id and add to previous list
                t.AddRange(JsonConvert.DeserializeObject<SearchData>(SecondresponseBody).data.Where(m => m.category1Name == "Soccer").Select(x => x.eventId).ToList());

                for (int i = 0; i < t.Count; i++)
                {
                    //get id of event
                    var eventId = t[i];

                    //get data for event
                    var singleresponse = client.GetStringAsync("https://merrybet.com/rest/market/events/" + eventId).Result;
                    var singleEventData = JsonConvert.DeserializeObject<MBData.ReceivedDataMerryBet>(singleresponse);

                    if (singleEventData.data != null)
                    {

                        var mbOdds = singleEventData.data.eventGames.
                                                        SelectMany(x => x.outcomes.
                                                        Select(m => new DailyMerrybetOdds()
                                                        { MainType = x.gameName, Selection = m.outcomeName, Value = m.outcomeOdds.ToString() }))
                                                        .ToList();

                        var teamNames = singleEventData.data.eventName.Split('-');

                        try
                        {
                            mbOdds.ForEach(
                                m => m.Selection = m.Selection.Replace(teamNames[0].Trim(), "1").Replace(teamNames[1].Trim(), "2"));

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(mbOdds.Count + e.Message);
                            continue;
                        }

                        var mbOddsnGames = new MerrybetData()
                        {
                            DateOfMatch = DateTimeOffset.FromUnixTimeMilliseconds(singleEventData.data.eventStart).DateTime.Date.ToString(),
                            League = singleEventData.data.category3Name,
                            TeamNames = singleEventData.data.eventName,
                            Odds = mbOdds,
                            TimeOfMatch = DateTimeOffset.FromUnixTimeMilliseconds(singleEventData.data.eventStart).DateTime.ToLocalTime().TimeOfDay.ToString()
                        };

                        listEvents.Add(mbOddsnGames);

                    }
                    else
                    {
                        Console.WriteLine("No data returned");
                    }
                }

                return listEvents;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\n Http Exception Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;

        }
    }
}
