using Classes;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scraper
{


    public class ScrapeBetPawa
    {

        public class SendData
        {
            public string MarketTypeGrouping { get; set; }
            public int[] EventIds { get; set; }
        }

        public List<BetPawa> Scrape()
        {
            ChromeOptions opt = new ChromeOptions();

            opt.AddArgument("--headless");



            var betOverview = new List<BetPawa>();
            using (var driver = new ChromeDriver())//opt))
            {


                driver.Navigate().GoToUrl("https://www.betpawa.ng");


                //gets all sidebar leagues
                var x = driver.FindElements(By.XPath("//*[@id=\"Popular-League-List\"]/li/a"));



                var links = new List<string>();
                foreach (var a in x)
                {
                    //gets sidebar links
                    links.Add(a.GetAttribute("href"));
                    betOverview.Add(new BetPawa { League = a.Text });
                }

                for (int i = 0; i < links.Count; i++)
                {
                    //go to sidebar link
                    driver.Navigate().GoToUrl(links[i]);

                    //list of match showing win, draw and lose
                    var matches = driver.FindElements(By.CssSelector("div[class='events-container prematch']"));
                    // var t = matches[0].FindElement(By.TagName("h3"));
                    betOverview[i].Matches = GetMatches(matches, driver);

                }



                //*[@id="Popular-League-List"]/li[1]/a
                Console.Write("Finished Running BetPawa Scraper");
            }

            return betOverview;
        }

        public List<DailyPawaMatches> ScrapeDaily(HttpClient client)
        {
            var betOverview = new List<DailyPawaMatches>();
          
            try
            {
                //get all initial page
                string responseBody = client.GetStringAsync("https://www.betpawa.ng/events/ws/getUpcomingEvents2/_1X2/2").Result;

                var paths = new List<string>();

                

                var t = JsonConvert.DeserializeObject<Rootobject>(responseBody);

                //get all paths for initial matches that showed up
                paths.AddRange(t.Data.Events.Where(x => x.StartsRaw.Date <= DateTime.Today.AddDays(1)).Select(p => p.Path));


                var listEventsIds = GetRemainingEvents(t.Data.RemainingEventIds, 1);

                foreach (var ids in listEventsIds)
                {
                    var postObject = JsonConvert.SerializeObject(new SendData() { MarketTypeGrouping = "_1X2", EventIds = ids });

                    Console.WriteLine("-----Query sent");
                    var response = client.PostAsync("https://www.betpawa.ng/events/ws/getEventsByIds",
                                   new StringContent(postObject, Encoding.UTF8, "application/json")).Result;

                   
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("-----Response received");
                        var m =  response.Content.ReadAsStringAsync().Result;

                        //add path for event id
                       var data = JsonConvert.DeserializeObject<RootObject>(m);

                        foreach (var item in data.Data)
                        {
                            if (!(item.StartsRaw.Date <= DateTime.Today.AddDays(1)))
                                break;

                            paths.Add(item.Path);
                            
                        }                        
                    }
                }

                var Mlinks = new List<string>();
                var bpMatches = new List<BetPawaMatches>();

                Console.WriteLine("-----Scraping started");
                using (var driver = new ChromeDriver())
                {
                    for (int i = 0; i < paths.Count; i++)
                    {

                        var url = "https://www.betpawa.ng/" + paths[i];

                        driver.Navigate().GoToUrl(url);

                        // driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 10);

                        Thread.Sleep(5000);

                        var leagueName = driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div[1]/div[4]/div/div[1]/div/div/span[1]/a")).Text;
                        var teamNames = driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div[1]/div[4]/div/div[1]/div/h2")).Text;

                        var matchTime = driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div[1]/div[4]/div/div[1]/div/span/span[2]")).Text;

                        var matchDate = driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div[1]/div[4]/div/div[1]/div/span/span[1]")).Text;

                        var SelectAndOddsHtml = driver.FindElements(By.CssSelector("div[class='events-container']"));

                        var SelectAndOdds = GetOdds(SelectAndOddsHtml);


                        betOverview.Add(new DailyPawaMatches() { League = leagueName, DateOfMatch = matchDate, TeamNames = teamNames, TimeOfMatch = matchTime, Odds = SelectAndOdds });
                     
                    }



                }

                Console.WriteLine("-----Scraping Done");

                return betOverview;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }


       List<int[]> GetRemainingEvents(int[] events, int currentGroup)
        {
            bool isRemaining = true;
            var data = new List<int[]>();
            var totalEvents = events.Length;
            var amountToSend = 50;

            
            

            while (isRemaining)
            {
                var skip = amountToSend * (currentGroup - 1);
                isRemaining = skip < totalEvents;
                data.Add(events.Skip(skip).Take(amountToSend).ToArray());

                currentGroup++;

                if ((amountToSend * (currentGroup - 1) ) > totalEvents)
                {
                    isRemaining = false;
                }
            }
            return data;
        }

        List<BetPawaMatches> GetMatches(ReadOnlyCollection<IWebElement> x, ChromeDriver driver)
        {
            var Mlinks = new List<string>();
            var bpMatches = new List<BetPawaMatches>();

            string teamNames;
            string matchTime;
            string matchDate;
            for (int i = 0; i < x.Count; i++)
            {

                //get matches links
                Mlinks.Add(x[i].FindElement(By.ClassName("event-match")).GetAttribute("href"));

            }


            //go to individual match page

            for (int i = 0; i < Mlinks.Count; i++)
            {



                driver.Navigate().GoToUrl(Mlinks[i]);

                // driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 10);

                Thread.Sleep(5000);

                teamNames = driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div[1]/div[4]/div/div[1]/div/h2")).Text;

                matchTime = driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div[1]/div[4]/div/div[1]/div/span/span[2]")).Text;

                matchDate = driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div[1]/div[4]/div/div[1]/div/span/span[1]")).Text;

                var SelectAndOddsHtml = driver.FindElements(By.CssSelector("div[class='events-container']"));

                var SelectAndOdds = GetOdds(SelectAndOddsHtml);

                var newMatch = new BetPawaMatches { DateOfMatch = matchDate, TeamNames = teamNames, TimeOfMatch = matchTime, Odds = SelectAndOdds };
                bpMatches.Add(newMatch);

                //using (var db = new BetPawaContext())
                //{
                //    db.BetPawaMatches.Add(newMatch);

                //    db.SaveChanges();
                //}
            }


            return bpMatches;
        }

        List<BetPawaOdds> GetOdds(ReadOnlyCollection<IWebElement> selectsnodds)
        {
            var lstBets = new List<BetPawaOdds>();

            for (int i = 0; i < selectsnodds.Count; i++)
            {
                var container = selectsnodds[i];

                var mainType = container.FindElement(By.TagName("h3")).Text;
                var subcontainers = container.FindElements(By.ClassName("events-sub-container"));

                string selectionType = "";
                for (int j = 0; j < subcontainers.Count; j++)
                {
                    var bsubCont = subcontainers[j];
                    string sType;

                    try
                    {

                        sType = bsubCont.FindElement(By.TagName("h4")).Text;
                    }
                    catch (NoSuchElementException)
                    {
                        sType = null;
                    }


                    if (string.IsNullOrEmpty(sType))
                    {
                        sType = selectionType;
                    }
                    else
                    {
                        selectionType = sType;
                    }

                    var listBetHtml = bsubCont.FindElements(By.CssSelector(".event-bet"));

                    for (int k = 0; k < listBetHtml.Count; k++)
                    {
                        var html = listBetHtml[k];
                        string slct = null;
                        string odd = null;

                        try
                        {

                            slct = html.FindElement(By.ClassName("event-selection")).Text;
                            odd = html.FindElement(By.ClassName("event-odds")).Text;
                        }
                        catch (Exception)
                        {

                            continue;
                        }

                        if (!(string.IsNullOrEmpty(slct) || string.IsNullOrEmpty(odd)))
                        {
                            lstBets.Add(new BetPawaOdds { MainType = mainType, Type = sType, Selection = slct, Value = odd });
                        }


                    }
                }
            }
            //for (int i = 0; i < selects.Count; i++)
            //{
            //    lstBets.Add(new BetPawaOdds { Selection = selects[i].Text, Value = odds[i].Text });
            //}

            return lstBets;
        }
    }
}
