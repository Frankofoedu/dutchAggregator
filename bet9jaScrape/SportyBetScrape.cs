using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    public class SportyBet
    {
        public string League { get; set; }
        public List<SportyBetMatches> Matches { get; set; }
    }

    public class SportyBetMatches
    {
        public string TeamNames { get; set; }
        public string MatchTime { get; set; }
        public List<SportyBetOdds> Odds { get; set; }
    }
    public class SportyBetOdds
    {
        public string Selection { get; set; }
        public string Value { get; set; }
    }
    /// <summary>
    /// scrape sportybet url- https://www.sportybet.com/ng/sport/football/today
    /// </summary>
    public class SportyBetScrape
    {
        public List<SportyBet> Scrape()
        {
            ChromeOptions opt = new ChromeOptions();

            //opt.AddArgument("headless");

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            //service.HideCommandPromptWindow = true;

            var SportyBetData = new List<SportyBet>();
            using (var driver = new ChromeDriver())
            {
                //navigate to url for today's matches
                driver.Navigate().GoToUrl("https://www.sportybet.com/ng/sport/football/today");

                driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 10);
                //gets all leagues for today
                var webElements = driver.FindElements(By.XPath("//div[contains(@class, 'match-league-wrap')]/div[contains(@class, 'match-league')]"));


                foreach (var webelement in webElements)
                {
                    var sb = new SportyBet { League = GetLeague(webelement) };

                    sb.Matches = GetMatches(webelement);

                    SportyBetData.Add(sb);
                }

                return SportyBetData;
            }
        }

        string GetLeague(IWebElement elem)
        {
            var rtn = "";

            var a = elem.FindElement(By.XPath(".//div[contains(@class, 'league-title')]/span[contains(@class, 'text')]"));
            rtn = a.Text;

            return rtn;
        }



        List<SportyBetMatches> GetMatches(IWebElement element)
        {
            var bpMatches = new List<SportyBetMatches>();

            var matchDivs = element.FindElements(By.XPath(".//div[contains(@class, 'match-table')]/div[contains(@class, 'match-row')]"));

            foreach (var item in matchDivs)
            {
                var selectionAndOdds = new List<SportyBetOdds>();
                var teamNames = item.FindElement(By.XPath(".//div[contains(@class, 'left-team-cell')]/div/div[contains(@class, 'teams')]")).GetAttribute("title");

                var time = item.FindElement(By.XPath(".//div[contains(@class, 'left-team-cell')]/div/div[contains(@class, 'clock-time')]")).Text.Replace("&nbsp;", "");

                var _1X2 = item.FindElements(By.XPath(".//div[contains(@class, 'market-cell')]/div[contains(@class, 'm-market')]"))[0];

                var _1 = _1X2.FindElements(By.ClassName("m-outcome"))[0];
                if (_1.GetAttribute("class").Contains("m-outcome--disabled"))
                {
                    var _1odd = new SportyBetOdds() { Selection = "1", Value = "0" };
                    selectionAndOdds.Add(_1odd);
                }
                else
                {
                    var _1odd = new SportyBetOdds()
                    {
                        Selection = "1",
                        Value = _1.FindElement(By.ClassName("m-outcome-odds")).Text
                    };
                    selectionAndOdds.Add(_1odd);
                }

                var _X = _1X2.FindElements(By.ClassName("m-outcome"))[1];
                if (_X.GetAttribute("class").Contains("m-outcome--disabled"))
                {
                    var _Xodd = new SportyBetOdds() { Selection = "X", Value = "0" };
                    selectionAndOdds.Add(_Xodd);
                }
                else
                {
                    var _Xodd = new SportyBetOdds()
                    {
                        Selection = "X",
                        Value = _X.FindElement(By.ClassName("m-outcome-odds")).Text
                    };
                    selectionAndOdds.Add(_Xodd);
                }

                var _2 = _1X2.FindElements(By.ClassName("m-outcome"))[2];
                if (_2.GetAttribute("class").Contains("m-outcome--disabled"))
                {
                    var _2odd = new SportyBetOdds() { Selection = "2", Value = "0" };
                    selectionAndOdds.Add(_2odd);
                }
                else
                {
                    var _2odd = new SportyBetOdds()
                    {
                        Selection = "2",
                        Value = _2.FindElement(By.ClassName("m-outcome-odds")).Text
                    };
                    selectionAndOdds.Add(_2odd);
                }

                bpMatches.Add(new SportyBetMatches { TeamNames = teamNames, MatchTime = time, Odds = selectionAndOdds });
            }

            return bpMatches;
        }

    }
}
