using Classes;
using Classes._1XBet;
using Classes._1XBet._1xBetData;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Scraper.ScrapeBetPawa;

namespace Scraper
{
    public class Scrape1XBet
    {
        public static async Task<List<BetMatch>> Scrape(HttpClient client)
        {
            //url to get all football markets
            var url = "https://1xbet.ng/LineFeed/GetChampsZip?sport=1&lng=en&tf=2200000&tz=1&country=132&partner=159&virtualSports=true";
            var betMatches = new List<BetMatch>();
            try
            {
                Console.WriteLine("Started scraping 1xBet");
                //get todays data
                var response = await client.GetStringAsync(url);


                //var response = responseTask.Result;
                var todayData = JsonConvert.DeserializeObject<FootBallTypes>(response);

                if (todayData != null)
                {
                    //loop through leagues/countries
                    foreach (var market in todayData.Value)
                    {
                        //removes unneccesary leagues
                        if (!market.L.ToLower().Trim().Contains("statistics round") && !market.L.ToLower().Trim().Contains("special"))
                        {
                            //get country and league
                            var arr = market.L.Split('.');
                            string country = "";
                            string league = "";
                            if (arr.Length > 1)
                            {
                                country = arr[0].Trim();
                                league = arr[1].Trim();
                            }
                            else
                            {
                                //sets the league if no seperator exists
                                /*
                                 e.g "L": "Copa Libertadores",
                                     "L": "South America Cup",
                                 */
                                league = market.L;
                            }

                            var s = market.L.Trim().Replace(" ", "-");
                            //get matches in league

                            var mUrl = $"https://1xbet.ng/LineFeed/Get1x2_VZip?champs={market.Id}&count=10&lng=en&mode=4&top=true&partner=159";

                            var marketResponse = await client.GetAsync(mUrl);

                            if (marketResponse.IsSuccessStatusCode)
                            {
                                var dt = await marketResponse.Content.ReadAsStringAsync();

                                var data = JsonConvert.DeserializeObject<SecondData>(dt);

                                //create list of match tasks
                                var allMatchTasks = data.Value.Select(x => client.GetAsync($"https://1xbet.ng/LineFeed/GetGameZip?id={x.Ci}&lng=en&cfview=0&isSubGames=true&GroupEvents=true&allEventsGroupSubGames=true&countevents=250&partner=159"));

                                var allMatches = await Task.WhenAll(allMatchTasks);
                                

                                foreach (var item in allMatches)
                                {
                                    if (item.IsSuccessStatusCode)
                                    {
                                        var singleMatchdata = await item.Content.ReadAsStringAsync();

                                        var singleMatch = JsonConvert.DeserializeObject<_1xBetMatchData>(singleMatchdata);

                                        //compare dates to get matches for today only.

                                        var date1 = DateTimeOffset.FromUnixTimeSeconds (singleMatch.Value.S).DateTime.AddHours(1);
                                        var date2 = DateTime.Now;

                                        //to get only todays data
                                        if (DateTime.Compare(date1, date2) > 0)
                                        {
                                            var betMatch = new BetMatch
                                            {
                                                Country = country,
                                                League = league,
                                                DateTimeOfMatch = date1,
                                                Site = "1xBet",
                                                TeamNames = $"{singleMatch.Value.O1} - {singleMatch.Value.O2}"
                                            };

                                            var betsNamesUrl = "https://xafr-a.akamaihd.net/genfiles/cms/betstemplates/betsNames_full_en.js";

                                            var betsNameResponse = await client.GetAsync(betsNamesUrl);

                                            var betsJson = await betsNameResponse.Content.ReadAsStringAsync();




                                            //get bets name
                                            var betsNameJson = GetBetsName(betsJson);

                                            //get bets name Model 
                                           var betsNameModelJson = GetBetsNameModel(betsJson);

                                            var betsName = JsonConvert.DeserializeObject<Dictionary<string, BetsName>>(betsNameJson, Converter.Settings);

                                            var betsNameModel =  JsonConvert.DeserializeObject<Dictionary<string, BetsNameModel>>(betsNameModelJson, BetsNameModel.Converter.Settings);

                                            var betList = new List< BetOdds>();
                                            foreach (var ge in singleMatch.Value.Ge)
                                            {
                                                

                                                foreach (var ee in ge.E)
                                                {
                                                    foreach (var eee in ee)
                                                    {
                                                        var g = eee.T;
                                                        var betType = betsNameModel[g.ToString()].N.Replace("^", string.Empty);

                                                        var odd = eee.C;
                                                        if (eee.P != null)
                                                        {
                                                            betType.Replace("()", eee.P.ToString());
                                                        }


                                                        betList.Add(new BetOdds { MainType = betType, Selection = betType, Value = odd.ToString() });
                                                    }
                                                }

                                                


                                            }

                                            betMatch.Odds = betList;

                                            betMatches.Add(betMatch);
                                        }




                                    }
                                }

                                return betMatches;
                            }                                                                                

                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return null;
        }

        private static string GetBetsNameModel(string betsJson)
        {
            var firstIndex = betsJson.IndexOf("var betsModel =");

            var g = ";\nvar betsModelGroup";
            var secondIndex = betsJson.IndexOf(g);

            var t = betsJson.Remove(0, secondIndex);



            var f = t.Remove(0, 23);

            //removes trailing ";"
            f = f.Remove(f.Length - 1);

            return f;
        }

        private static string GetBetsName(string betsNameJson)
        {

            var firstIndex = betsNameJson.IndexOf($";\nvar betsModelGroup");
            var t = betsNameJson.Remove(firstIndex);


            var g = "var betsModel =";
            var secondIndex = betsNameJson.IndexOf(g);

            var f  = t.Remove(secondIndex, 15);

            return f;
        }

        private static List<HtmlNode> GetScriptTag(IEnumerable<HtmlNode> ft)
        {

            return (from item in ft
                    from atttr in item.Attributes
                    where atttr.Value == "application/ld+json" && atttr.Name == "type"
                    select item).ToList();

        }
    }
}
