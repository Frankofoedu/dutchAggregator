﻿using OpenQA.Selenium;
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
        public List<Odds> Odds { get; set; }
    }
    public class Odds
    {
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
            service.HideCommandPromptWindow = true;

            var Bet9jaData = new List<Bet9ja>();
            using (var driver = new ChromeDriver())
            {
                //navigate to url for today's matches
                driver.Navigate().GoToUrl("https://web.bet9ja.com/Sport/OddsToday.aspx?IDSport=590");

                driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 10);
                //gets all leagues for today
                var webElements = driver.FindElements(By.XPath("//div[contains(@class, 'match-league-wrap')]/div[contains(@class, 'match-league')]"));


                //foreach (var webelement in webElements)
                //{
                //    var sb = new Bet9ja { League = GetLeague(webelement) };

                //    sb.Matches = GetMatches(webelement);

                //    Bet9jaData.Add(sb);
                //}

                return Bet9jaData;
            }
        }

        //string GetLeague(IWebElement elem)
        //{
        //    var rtn = "";

        //    var a = elem.FindElement(By.XPath(".//div[contains(@class, 'league-title')]/span[contains(@class, 'text')]"));
        //    rtn = a.Text;

        //    return rtn;
        //}



        //List<Matches> GetMatches(IWebElement element)
        //{
        //    var bpMatches = new List<Matches>();

        //    var matchDivs = element.FindElements(By.XPath(".//div[contains(@class, 'match-table')]/div[contains(@class, 'match-row')]"));

        //    foreach (var item in matchDivs)
        //    {
        //        var selectionAndOdds = new List<Odds>();
        //        var teamNames = item.FindElement(By.XPath(".//div[contains(@class, 'left-team-cell')]/div/div[contains(@class, 'teams')]")).GetAttribute("title");
        //        var _1X2 = item.FindElements(By.XPath(".//div[contains(@class, 'market-cell')]/div[contains(@class, 'm-market')]"))[0];

        //        var _1 = _1X2.FindElements(By.ClassName("m-outcome"))[0];
        //        if (_1.GetAttribute("class").Contains("m-outcome--disabled"))
        //        {
        //            var _1odd = new Odds() { Selection = "1", Value = "0" };
        //            selectionAndOdds.Add(_1odd);
        //        }
        //        else
        //        {
        //            var _1odd = new Odds()
        //            {
        //                Selection = "1",
        //                Value = _1.FindElement(By.ClassName("m-outcome-odds")).Text
        //            };
        //            selectionAndOdds.Add(_1odd);
        //        }

        //        var _X = _1X2.FindElements(By.ClassName("m-outcome"))[1];
        //        if (_X.GetAttribute("class").Contains("m-outcome--disabled"))
        //        {
        //            var _Xodd = new Odds() { Selection = "X", Value = "0" };
        //            selectionAndOdds.Add(_Xodd);
        //        }
        //        else
        //        {
        //            var _Xodd = new Odds()
        //            {
        //                Selection = "X",
        //                Value = _X.FindElement(By.ClassName("m-outcome-odds")).Text
        //            };
        //            selectionAndOdds.Add(_Xodd);
        //        }

        //        var _2 = _1X2.FindElements(By.ClassName("m-outcome"))[2];
        //        if (_2.GetAttribute("class").Contains("m-outcome--disabled"))
        //        {
        //            var _2odd = new Odds() { Selection = "2", Value = "0" };
        //            selectionAndOdds.Add(_2odd);
        //        }
        //        else
        //        {
        //            var _2odd = new Odds()
        //            {
        //                Selection = "2",
        //                Value = _2.FindElement(By.ClassName("m-outcome-odds")).Text
        //            };
        //            selectionAndOdds.Add(_2odd);
        //        }

        //        bpMatches.Add(new Matches { TeamNames = teamNames, Odds = selectionAndOdds });
        //    }

        //    return bpMatches;
        //}
    }
}