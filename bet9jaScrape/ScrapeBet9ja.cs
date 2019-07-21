using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bet9jaScrape
{

    public class Bet9ja
    {
        public string League { get; set; }
        public List<Matches> Matches { get; set; }
    }

    public class Matches
    {
        public string TeamNames { get; set; }
        public string MatchTime { get; set; }
        public List<Odds> Odds { get; set; }
    }
    public class Odds
    {
        public string Type { get; set; }
        public string Selection { get; set; }
        public string Value { get; set; }
    }

    public class ScrapeBet9ja
    {
        public List<Bet9ja> Scrape()
        {
            ChromeOptions opt = new ChromeOptions();

            opt.AddArgument("headless");

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            //service.HideCommandPromptWindow = true;

            var Bet9jaData = new List<Bet9ja>();
            using (var driver = new ChromeDriver())
            {
                //navigate to url for today's matches
                driver.Navigate().GoToUrl("https://web.bet9ja.com/Sport/OddsToday.aspx?IDSport=590");

                System.Threading.Thread.Sleep(7000);
                //gets all leagues for today
                var webElements = driver.FindElements(By.XPath("//div[contains(@class, 'oddsViewPanel')]/div[contains(@class, 'divOdds')]/div[contains(@class, 'SEs')]/div[contains(@class, 'item')]"));

                var sb = new Bet9ja();

                foreach (var webelement in webElements)
                {
                    if (webelement.GetAttribute("class").Contains("firstItemGroup") )
                    {
                        if (sb.Matches != null)
                        {
                            Bet9jaData.Add(sb);
                        }

                        sb = new Bet9ja { League = GetLeague(webelement), Matches=new List<Matches>() };

                        sb.Matches.Add(GetMatches(webelement));
                    }
                    else
                    {
                        sb.Matches.Add(GetMatches(webelement));
                    }
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

        Matches GetMatches(IWebElement element)
        {
            var oddDivs = element.FindElements(By.XPath(".//div[contains(@class, 'odds')]/div/div/div[contains(@class, 'cq')]"));

            var teamName = element.FindElement(By.ClassName("Event")).Text;
            var time = element.FindElement(By.ClassName("Time")).Text;

            var selectionAndOdds = new List<Odds>();

            foreach (var oddGroup in oddDivs)
            {
                var odds = oddGroup.FindElements(By.ClassName("odd"));

                foreach (var odd in odds)
                {
                    var sNoDivs = odd.FindElements(By.TagName("div"));
                    if (odd.GetAttribute("class").Contains("empty"))
                    {
                        var SnO = new Odds() { Selection = sNoDivs[0].Text, Value = "0" };
                        selectionAndOdds.Add(SnO);
                    }else
                    {
                        var SnO = new Odds() { Selection = sNoDivs[0].Text, Value = sNoDivs[1].Text };
                        selectionAndOdds.Add(SnO);
                    }
                }
            }

            var m = new Matches() { TeamNames = teamName, MatchTime = time, Odds = selectionAndOdds };
            return m;
        }

    }
}
