using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Classes.SportyBetData;
using Classes;
using Newtonsoft.Json;

namespace Scraper
{
    public class ScrapeSportyBet
    {
        public async Task<List<BetMatch>> ScrapeSportyBetDailyAsync(HttpClient client)
        {
            var currdate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            try
            {
                Console.WriteLine("Started scraping sportybet");
                //get todays data
                string FirstresponseBody = client.GetStringAsync("https://www.sportybet.com/api/ng/factsCenter/pcUpcomingEvents?sportId=sr%3Asport%3A1&marketId=1%2C18%2C10%2C29%2C11%2C26%2C36%2C14&pageSize=100&pageNum=1&timeline=24&_t=" + currdate).Result;

                Console.WriteLine("today's data received");

                //serialize data
                var t = JsonConvert.DeserializeObject<SearchData>(FirstresponseBody);

                var eventsId = t.data.tournaments.SelectMany(x => x.events).Select(m => m.eventId);


                currdate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var tasks = eventsId.Select(ids => client.GetStringAsync("https://www.sportybet.com/api/ng/factsCenter/event?eventId=" + ids + "&productId=3&_t=" + currdate));

                Console.WriteLine("Query Sent");

                await Task.WhenAll(tasks);

                Console.WriteLine("All Matches received");

                var responses = tasks.Select(task => JsonConvert.DeserializeObject<ReceievedData.SportyBetData>(task.Result));


                Console.WriteLine("All Matches parsed");

                var returnData = new List<BetMatch>();
                //loop through each match
                foreach (var item in responses.ToList())
                {
                    var matchStart = DateTimeOffset.FromUnixTimeMilliseconds(item.data.estimateStartTime).DateTime;

                    foreach (var market in item.data.markets)
                    {
                        var Odds = market.outcomes.Select(l => new BetOdds
                        {
                            Selection = l.desc,
                            MainType = market.desc,
                            Type = l.desc,
                            Value = l.odds
                        }).ToList();

                        returnData.Add(new BetMatch
                        {
                            League = item.data.sport.category.tournament.name,
                            DateTimeOfMatch = matchStart,
                            TeamNames = item.data.homeTeamName + " - " + item.data.awayTeamName,
                            Odds = Odds,
                            Country = item.data.sport.category.name,
                        });
                    }
                }
                var leagues = new List<string>();

                foreach (var item in returnData)
                {
                    if(! leagues.Any(m=> m == item.League))
                    {
                        leagues.Add(item.League);
                    }
                }

                var matchGrouped = new List<BetMatch>();

                foreach (var item in leagues)
                {
                    var sbg = returnData.Where(m => m.League == item);
                    var matchgroup = sbg.GroupBy(m => m.TeamNames);

                    foreach (var i in matchgroup)
                    {
                        var sb = new BetMatch()
                        {
                            Country = i.First().Country,
                            TeamNames = i.First().TeamNames,
                            DateTimeOfMatch = i.First().DateTimeOfMatch,
                            Odds = i.SelectMany(n => n.Odds).ToList(),
                            League = i.First().League,
                            Site = i.First().Site,
                        };

                        matchGrouped.Add(sb);
                    }
                }
                return matchGrouped;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }
    }
}
