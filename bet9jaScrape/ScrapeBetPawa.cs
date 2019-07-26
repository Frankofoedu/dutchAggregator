using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scraper
{
    public class BetPawa
    {
        public int Id { get; set; }
        public string League { get; set; }
        public List<BetPawaMatches> Matches { get; set; }
    }

    public class BetPawaMatches
    {
        public int Id { get; set; }
        public string TeamNames { get; set; }
        public string TimeOfMatch { get; set; }
        public string DateOfMatch { get; set; }
        public List<BetPawaOdds> Odds { get; set; }
    }
    public class BetPawaOdds
    {
        public int Id { get; set; }
        public string MainType { get; set; }
        public string Type { get; set; }
        public string Selection { get; set; }
        public string Value { get; set; }

        public string SelectionFull { get {
                return (MainType + "-" + Type + "-" + Selection).ToLower();
            } }
    }

    public class ScrapeBetPawa
    {

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
