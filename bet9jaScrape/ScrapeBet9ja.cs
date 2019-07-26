using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{

    public class Bet9ja
    {
        public string League { get; set; }
        public List<Bet9jaMatches> Matches { get; set; }
    }

    public class Bet9jaMatches
    {
        public string TeamNames { get; set; }
        public string MatchTime { get; set; }
        public List<Bet9jaOdds> Odds { get; set; }
    }
    public class Bet9jaOdds
    {
        public string Type { get; set; }
        public string Selection { get; set; }
        public string Value { get; set; }

        public string SelectionFull
        {
            get
            {
                return (Type + "-" + Selection).ToLower();
            }
        }
    }
    public class ScrapeBet9ja
    {
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
