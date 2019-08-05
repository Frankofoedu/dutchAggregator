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
        public async Task<List<SportyBet>> ScrapeSportyBetDailyAsync(HttpClient client)
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

                var returnData = new List<SportyBet>();
                var oddsng = new List<SportyBetMatches>();
                foreach (var item in responses.ToList())
                {
                    var matchStart = DateTimeOffset.FromUnixTimeMilliseconds(item.data.estimateStartTime).DateTime;
                                    

                    returnData.Add(new SportyBet
                    {
                        League = item.data.sport.category.tournament.name,
                        Matches = item.data.markets.Select(v => new SportyBetMatches
                        {
                            TimeOfMatch = matchStart.TimeOfDay.ToString(),
                            DateOfMatch = matchStart.Date.ToString(),
                            TeamNames = item.data.homeTeamName + " " + item.data.awayTeamName,
                            Odds = v.outcomes.Select(l => new SportyBetOdds
                            {
                                Selection = l.desc,
                                MainType = v.desc,
                                Type = l.desc,
                                Value = l.odds
                            }).ToList()
                        }).ToList()
                    });
                   

                }

                return returnData.GroupBy(q => q.League).Select(w => new SportyBet { League = w.Key, Matches = w.SelectMany(e => e.Matches).ToList() })
                    .SelectMany(u => u.Matches.GroupBy(o=> o.TeamNames).Select(p=> new SportyBet { League = u.League, Matches= p.ToList() })).ToList();
                

                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }
    }
}
