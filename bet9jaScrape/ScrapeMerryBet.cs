﻿using Newtonsoft.Json;
using Classes.MerryBet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Classes;

namespace Scraper
{

    public class ScrapeMerryBet
    {
        public List<BetMatch> ScrapeDaily(HttpClient client)
        {
            var currdate = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            var tomodate = DateTime.Now.AddDays(1).ToString("yyyy'-'MM'-'dd");
            var listEvents = new List<BetMatch>();
            var jsonSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

            try
            {
                //get todays data
                string FirstresponseBody = client.GetStringAsync("https://merrybet.com/rest/search/events/search-by-date/" + currdate).Result;

                //get next day data
                //string SecondresponseBody = client.GetStringAsync("https://merrybet.com/rest/search/events/search-by-date/" + tomodate).Result;

                //get first day list of events id
                var t = JsonConvert.DeserializeObject<SearchData>(FirstresponseBody).data.Where(m => m.category1Name == "Soccer" && !m.category3Name.ToLower().Contains("special") && m.eventType == 1).Select(x => x.eventId).ToList();

                //get second day list of events id and add to previous list
                //t.AddRange(JsonConvert.DeserializeObject<SearchData>(SecondresponseBody).data.Where(m => m.category1Name == "Soccer" && !m.category3Name.ToLower().Contains("special") && m.eventType == 1).Select(x => x.eventId).ToList());

                Parallel.ForEach(t, (eventId) =>
                {

                    //get data for event
                    var singleresponse = client.GetStringAsync("https://merrybet.com/rest/market/events/" + eventId).Result;
                    var singleEventData = JsonConvert.DeserializeObject<MBData.ReceivedDataMerryBet>(singleresponse);

                    if (singleEventData.data != null)
                    {
                        var mbOdds = singleEventData.data.eventGames.
                                                        SelectMany(x => x.outcomes.
                                                        Select(m => new BetOdds()
                                                        { Type = x.gameName, Selection = m.outcomeName, Value = m.outcomeOdds.ToString() }))
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
                            return;
                        }

                        var mbOddsnGames = new BetMatch();

                        if (singleEventData.data.category3Name.ToLower() == "group stage")
                        {
                            mbOddsnGames = new BetMatch()
                            {
                                DateTimeOfMatch = DateTimeOffset.FromUnixTimeMilliseconds(singleEventData.data.eventStart).DateTime.AddHours(1),
                                League = singleEventData.data.category2Name,
                                TeamNames = singleEventData.data.eventName,
                                Odds = mbOdds,
                                Country = singleEventData.data.category3Name,
                                Site = "merryBet"
                            };
                        }
                        else
                        {
                            mbOddsnGames = new BetMatch()
                            {
                                DateTimeOfMatch = DateTimeOffset.FromUnixTimeMilliseconds(singleEventData.data.eventStart).DateTime.AddHours(1),
                                League = singleEventData.data.category3Name,
                                TeamNames = singleEventData.data.eventName,
                                Odds = mbOdds,
                                Country = singleEventData.data.category2Name,
                                Site = "merryBet"
                            };
                        }

                        listEvents.Add(mbOddsnGames);

                    }
                    else
                    {
                        Console.WriteLine("No data returned");
                    }
                });

                Console.WriteLine("--Merrybet Done");
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
