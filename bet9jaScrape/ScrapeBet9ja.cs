using Classes;
using Classes.Bet9jaData;
using Classes.Bet9jaData.Single;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    public class SendData
    {
        public int IDSottoEvento { get; set; }
        public int IDGruppoQuota { get; set; }
    }


    public class ScrapeBet9ja
    {


        public async Task<List<BetMatch>> ScrapeJsonAsync(HttpClient client)
        {
            var listEvents = new List<BetMatch>();
            try
            {
                //request to get all matches

                var payload = "{\"IDSport\":590,\"IDGruppoQuota\":-1}";

                HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");

                var uri = "https://web.bet9ja.com/Controls/ControlsWS.asmx/OddsTodayFullEvent";
                var response = await client.PostAsync(uri, c);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();

                    var t = JsonConvert.DeserializeObject<Bet9jaReceivedData>(data);

                    var soccerData = t.d.Where(x => x.Sport.Trim() == "Soccer").FirstOrDefault();

                    if (soccerData != null)
                    {
                        //get list of ids for todays match
                        var listIds = soccerData.Detail.SottoEventiList.Select(m => m.IDSottoEvento).ToList();

                        foreach (var id in listIds)
                        {

                            var postObject = JsonConvert.SerializeObject(new SendData() { IDSottoEvento = id, IDGruppoQuota = 0 });

                            var httpResponse = client.PostAsync("https://web.bet9ja.com/Controls/ControlsWS.asmx/GetSubEventDetails",
                                           new StringContent(postObject, Encoding.UTF8, "application/json")).Result;

                            if (httpResponse.IsSuccessStatusCode)
                            {
                                var d = await httpResponse.Content.ReadAsStringAsync();

                                //var g = new JObject(d);

                                var singleData = JsonConvert.DeserializeObject<Bet9jaSingleReceivedData>(d);

                                if (singleData != null)
                                {

                                    var y = singleData.d.ClassiQuotaList;
                                    var oddsngames = new List<Bet9jaMatches>();
                                    var listmatch = new List<BetMatch>();


                                    foreach (var item in y)
                                    {
                                        var odds = new List<BetOdds>();
                                        foreach (var m in item.QuoteList)
                                        {
                                            odds.Add(new BetOdds { Value = m.Quota, Selection = m.TipoQuota, Type = item.ClasseQuota });
                                        }
                                        listmatch.Add(new BetMatch { Odds = odds,
                                            DateTimeOfMatch = DateTime.ParseExact(singleData.d.SrtDataInizio, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                                            TeamNames = singleData.d.SottoEvento
                                        });
                                    }

                                    var compOdds = listmatch.GroupBy(x => x.TeamNames).First().SelectMany(m => m.Odds);


                                    listEvents.Add(new BetMatch {
                                        Odds = compOdds.ToList(),
                                        DateTimeOfMatch = DateTime.ParseExact(singleData.d.SrtDataInizio, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                                        TeamNames = singleData.d.SottoEvento,
                                        League = singleData.d.Evento,
                                        Site = "bet9ja"
                                    });

                                }

                            }
                        }

                        //var qbets = listEvents.GroupBy(x => x.League).ToList();
                        //listEvents.Clear();
                        //foreach (var item in qbets)
                        //{
                        //    var tbets = item;
                        //    var bb = tbets.SelectMany(cb => cb.Matches);

                        //    listEvents.Add(new Bet9ja { League = item.Key, Matches = bb.ToList() });
                        //}

                        //.Select(m => new Bet9ja { League = m.Key, Matches = m.ToList() });
                        return listEvents;
                    }
                }
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


        public async Task<List<Bet9ja>> ScrapeJsonAsyncReplaced(HttpClient client)
        {
            var listEvents = new List<Bet9ja>();
            try
            {
                //request to get all matches

                var payload = "{\"IDSport\":590,\"IDGruppoQuota\":-1}";

                HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");

                var uri = "https://web.bet9ja.com/Controls/ControlsWS.asmx/OddsTodayFullEvent";
                var response = await client.PostAsync(uri, c);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();

                    var t = JsonConvert.DeserializeObject<Bet9jaReceivedData>(data);

                    var soccerData = t.d.Where(x => x.Sport.Trim() == "Soccer").FirstOrDefault();

                    if (soccerData != null)
                    {
                        //get list of ids for todays match
                        var listIds = soccerData.Detail.SottoEventiList.Select(m => m.IDSottoEvento).ToList();

                        foreach (var id in listIds)
                        {

                            var postObject = JsonConvert.SerializeObject(new SendData() { IDSottoEvento = id, IDGruppoQuota = 0 });

                            var httpResponse = client.PostAsync("https://web.bet9ja.com/Controls/ControlsWS.asmx/GetSubEventDetails",
                                           new StringContent(postObject, Encoding.UTF8, "application/json")).Result;

                            if (httpResponse.IsSuccessStatusCode)
                            {
                                var d = await httpResponse.Content.ReadAsStringAsync();

                                //var g = new JObject(d);

                                var singleData = JsonConvert.DeserializeObject<Bet9jaSingleReceivedData>(d);

                                if (singleData != null)
                                {

                                    var y = singleData.d.ClassiQuotaList;
                                    var oddsngames = new List<Bet9jaMatches>();
                                    var listmatch = new List<Bet9jaMatches>();
                                    foreach (var item in y)
                                    {
                                        var odds = new List<Bet9jaOdds>();
                                        foreach (var m in item.QuoteList)
                                        {
                                            odds.Add(new Bet9jaOdds { Value = m.Quota, Selection = m.TipoQuota, Type = item.ClasseQuota });
                                        }
                                        listmatch.Add(new Bet9jaMatches { Odds = odds, MatchTime = singleData.d.SrtDataInizio, TeamNames = singleData.d.SottoEvento });
                                    }

                                    var compOdds = listmatch.GroupBy(x => x.TeamNames).First().SelectMany(m => m.Odds);


                                    oddsngames.Add(new Bet9jaMatches { Odds = compOdds.ToList(), MatchTime = singleData.d.SrtDataInizio, TeamNames = singleData.d.SottoEvento });
                                    //var j = new List<Bet9jaMatches>().Add(new Bet9jaMatches{)

                                    listEvents.Add(new Bet9ja { League = singleData.d.Evento, Matches = oddsngames });


                                }

                            }
                        }

                        var qbets = listEvents.GroupBy(x => x.League).ToList();
                        listEvents.Clear();
                        foreach (var item in qbets)
                        {
                            var tbets = item;

                            var bb = tbets.SelectMany(cb => cb.Matches);

                            listEvents.Add(new Bet9ja { League = item.Key, Matches = bb.ToList() });
                        }

                        //.Select(m => new Bet9ja { League = m.Key, Matches = m.ToList() });
                        return listEvents;
                    }
                }
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
        public List<Bet9ja> Scrape()
        {
            ChromeOptions opt = new ChromeOptions();

            //opt.AddArgument("--headless");

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            //service.HideCommandPromptWindow = true;

            var Bet9jaData = new List<Bet9ja>();
            using (var driver = new ChromeDriver())
            {
                //navigate to url for today's matches
                driver.Navigate().GoToUrl("https://web.bet9ja.com/Sport/OddsToday.aspx?IDSport=590");


                var curindex = 0;
                var total = 1;

                var curId = "";
                var nextId = "n";

                var sb = new Bet9ja();

                while (curindex < total)
                {
                    System.Threading.Thread.Sleep(5000);

                    //gets all leagues for today
                    var webElements = driver.FindElements(By.XPath("//div[contains(@class, 'oddsViewPanel')]/div[contains(@class, 'divOdds')]/div[contains(@class, 'SEs')]/div[contains(@class, 'item')]"));

                    //reset total count to count of all the matches in the page from the initial (1)
                    if (total == 1)
                    {
                        total = webElements.Count;
                    }
                    else //what to do if total is no more at the initial value (1)
                    {
                        if (total != webElements.Count) //what to do if total no longer matches with count of all the matches in the page. Most likely due so some matches have started.
                        {
                            total = webElements.Count; // assign the new count to total

                            if (nextId != "") // nextId is empty if the previous was the last
                            {
                                for (int i = 0; i < webElements.Count; i++)
                                {
                                    if (webElements[i].FindElement(By.ClassName("ID")).Text == nextId)
                                    {
                                        curindex = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }

                        }
                    }

                    curId = webElements[curindex].FindElement(By.ClassName("ID")).Text;
                    if ((curindex + 1) < total)
                    {
                        nextId = webElements[curindex + 1].FindElement(By.ClassName("ID")).Text;
                    }
                    else
                    { nextId = ""; }

                    if (webElements[curindex].GetAttribute("class").Contains("firstItemGroup"))
                    {
                        if (sb.Matches != null)
                        {
                            Bet9jaData.Add(sb);
                        }

                        sb = new Bet9ja() { League = GetLeague(webElements[curindex]), Matches = new List<Bet9jaMatches>() };

                    }



                    //total = webElements.Count;

                    var matchdiv = webElements[curindex].FindElement(By.ClassName("Event"));
                    matchdiv.Click();

                    System.Threading.Thread.Sleep(5000);

                    var SElement = driver.FindElement(By.XPath("//*[@id=\"divDett\"]"));
                    var match = GetMatch(SElement);

                    sb.Matches.Add(match);

                    IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                    js.ExecuteScript("history.back()");

                    curindex++;
                }

                if (sb.Matches != null)
                {
                    Bet9jaData.Add(sb);
                }

                return Bet9jaData;
            }
        }

        string GetLeague(IWebElement elem)
        {
            var rtn = "";

            var a = elem.FindElement(By.XPath(".//div[contains(@class, 'EventParent')]"));
            rtn = a.Text;

            return rtn;
        }

        Bet9jaMatches GetMatch(IWebElement element)
        {
            var oddDivs = element.FindElements(By.ClassName("SEItem"));

            var teamName = element.FindElement(By.Id("SEOddsDescSE")).Text;
            var time = element.FindElement(By.Id("SEOddsDataSE")).Text;

            var selectionAndOdds = new List<Bet9jaOdds>();

            foreach (var oddGroup in oddDivs)
            {
                var type = oddGroup.FindElement(By.ClassName("SECQ")).Text;
                var odds = oddGroup.FindElements(By.ClassName("SEOdd"));

                if (odds == null)
                {
                    continue;
                }
                if (odds.Count < 1)
                {
                    continue;
                }

                foreach (var odd in odds)
                {
                    var sNoDivs = odd.FindElements(By.TagName("div"));

                    var SnO = new Bet9jaOdds() { Type = type, Selection = sNoDivs[0].Text.Trim(), Value = sNoDivs[1].Text.Trim() };
                    selectionAndOdds.Add(SnO);
                }
            }

            var m = new Bet9jaMatches() { TeamNames = teamName, MatchTime = time, Odds = selectionAndOdds };
            return m;
        }
    }
}
