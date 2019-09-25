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



                //get structure of odds
                var ddt = await GetBetModelJsonAsync(client);
                
                var betsName =  GetBetsName(ddt);
                var betsNameModelGroup = GetBetsNameModel(ddt);

                //var response = responseTask.Result;
                var todayData = JsonConvert.DeserializeObject<FootBallTypes>(response);

                if (todayData == null) return null;



                //loop through leagues/countries
                foreach (var market in todayData.Value)
                {

                    //removes unneccesary leagues
                    if (market.L.ToLower().Trim().Contains("statistics round") || market.L.ToLower().Trim().Contains("special"))
                        continue;
                    
                   
                    
                    //get country and league
                    var arr = market.L.Split('.');


                    string country = "";
                    string league = "";
                    league = GetLeagueAndOrCountryNames(market, arr, ref country);

                    //get matches in league
                  var mUrl = $"https://1xbet.ng/LineFeed/Get1x2_VZip?champs={market.Id}&count=10&lng=en&mode=4&top=true&partner=159";

                    var marketResponse = await client.GetAsync(mUrl);

                    
                    if (!marketResponse.IsSuccessStatusCode)
                        continue;
                    

                    var dt = await marketResponse.Content.ReadAsStringAsync();

                    var matchData = JsonConvert.DeserializeObject<SecondData>(dt);

                    //create list of match tasks
                    var allMatchTasks = matchData.Value.Select(x => client.GetAsync($"https://1xbet.ng/LineFeed/GetGameZip?id={x.Ci}&lng=en&cfview=0&isSubGames=true&GroupEvents=true&allEventsGroupSubGames=true&countevents=250&partner=159"));

                    //get list of matche and thier data
                    var allMatches = await Task.WhenAll(allMatchTasks);


                    foreach (var item in allMatches)
                    {
                        if (!item.IsSuccessStatusCode)
                        {
                            continue;
                        }
                        var singleMatchdata = await item.Content.ReadAsStringAsync();

                        var singleMatch = JsonConvert.DeserializeObject<_1xBetMatchData>(singleMatchdata);

                        //compare dates to get matches for today only.

                        var date1 = DateTimeOffset.FromUnixTimeSeconds(singleMatch.Value.S).DateTime.AddHours(1);
                        var date2 = DateTime.Now;

                        //to get matches for only today
                        if (DateTime.Compare(date1, date2) <= 0)
                        {
                            continue;
                        }

                        var betMatch = new BetMatch
                        {
                            Country = country,
                            League = league,
                            DateTimeOfMatch = date1,
                            Site = "1xBet",
                            TeamNames = $"{singleMatch.Value.O1} - {singleMatch.Value.O2}"
                        };


                        var betList = new List<BetOdds>();

                        foreach (var e in singleMatch.Value.Ge)
                        {
                            var bG = e.G;

                            var betGroup = betsNameModelGroup[bG.ToString()].N;

                            foreach (var eee in e.E.SelectMany(ee => ee.Select(eee => eee)))
                            {
                                var betType = eee.T.ToString();
                                var value = eee.C.ToString();
                                var selection = betsName[betType].N;
                                betList.Add(new BetOdds { MainType = betGroup, Selection = selection, Type = betGroup, Value = value });
                            }
                        }

                        betMatch.Odds = betList;

                        betMatches.Add(betMatch);
                    }

                    return betMatches;
                }

                return null;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// gets the file that holds id and names of the odds
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static async Task<string> GetBetModelJsonAsync(HttpClient client)
        {
            var url = "https://xafr-a.akamaihd.net/genfiles/cms/betstemplates/betsNames_full_en.js";

            var response = await client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();

            

        }
        private static Dictionary<string, BetsNameModel> GetBetsNameModel(string betsJson)
        {
            /*
             betjson => 
             "var betsModel = {...};
              var betsModelGroup = {...};"
        
             */
            
            var firstIndex = betsJson.IndexOf("var betsModel =");            
            var secondIndex = betsJson.IndexOf(";\nvar betsModelGroup");

            var t = betsJson.Remove(0, secondIndex);
            var f = t.Remove(0, 23);

            //removes trailing ";"
            f = f.Remove(f.Length - 1);

            var betsNameModel = JsonConvert.DeserializeObject<Dictionary<string, BetsNameModel>>(f, BetsNameModel.Converter.Settings);

            return betsNameModel;
        }
        private static Dictionary<string, BetsName> GetBetsName(string betsNameJson)
        {

            var firstIndex = betsNameJson.IndexOf($";\nvar betsModelGroup");
            var secondIndex = betsNameJson.IndexOf("var betsModel =");

            var t = betsNameJson.Remove(firstIndex);            
            var f = t.Remove(secondIndex, 15);            

            return  JsonConvert.DeserializeObject<Dictionary<string, BetsName>>(f, Converter.Settings);

        }
        private static string GetLeagueAndOrCountryNames(Classes._1XBet.Data market, string[] arr, ref string country)
        {
            string league;
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

            return league;
        }
               
    }
}
