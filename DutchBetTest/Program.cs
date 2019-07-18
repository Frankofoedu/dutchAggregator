using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace DutchBetTest
{
    class BetPawaOverview
    {
        public string League { get; set; }
        public List<BetPawaMatches> Matches { get; set; }
    }

    class BetPawaMatches
    {
        public string TeamNames { get; set; }
        public List<BetPawaOdds> Odds { get; set; }
    }
    class BetPawaOdds
    {
        public string Selection { get; set; }
        public string Value { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //double odd1 = Convert.ToDouble(Console.ReadLine());
            //double odd2 = Convert.ToDouble(Console.ReadLine());


            //var chromeOptions = new ChromeOptions();

            //var x = 1.0 / (odd1 + odd2) * odd2;
            //var y = 1.0 / (odd1 + odd2) * odd1;
            //double x2 = x * 100;
            //double y2 = y * 100;
            //double rtn = x2 * odd1;

            //Console.WriteLine(rtn + " - " + x2 + " - " + y2);
            //var v = Console.ReadLine();
            ChromeOptions opt = new ChromeOptions();

            opt.AddArgument("--headless");

           

            var betOverview = new List<BetPawaOverview>();
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
                    betOverview.Add(new BetPawaOverview { League = a.Text });
                }

                for (int i = 0; i < links.Count; i++)
                {
                    //go to sidebar link
                    driver.Navigate().GoToUrl(links[i]);

                    //list of match showing win, draw and lose
                    var matches = driver.FindElements(By.CssSelector("div[class='events-container prematch']"));
                    // var t = matches[0].FindElement(By.TagName("h3"));
                    betOverview[i].Matches = GetMatches(matches);

                }


                //*[@id="Popular-League-List"]/li[1]/a
                Console.Write(x);
                Console.Read();
            }

            List<BetPawaMatches> GetMatches(ReadOnlyCollection<IWebElement> x)
            {
                var bpMatches = new List<BetPawaMatches>();
                for (int i = 0; i < x.Count; i++)
                {
                    var teamNames = x[i].FindElement(By.TagName("h3"));

                    var selections = x[i].FindElements(By.ClassName("event-selection"));
                    var odds = x[i].FindElements(By.ClassName("event-odds"));

                    var selectionAndOdds = GetOdds(selections, odds);
                    bpMatches.Add(new BetPawaMatches { TeamNames = teamNames.Text, Odds = selectionAndOdds });
                    //  GetOdds(x[0]);
                }

                return bpMatches; 
            }
            
           List<BetPawaOdds> GetOdds(ReadOnlyCollection<IWebElement> selects, ReadOnlyCollection<IWebElement> odds)
            {
                var lstBets = new List<BetPawaOdds>();
                for (int i = 0; i < selects.Count; i++)
                {
                    lstBets.Add(new BetPawaOdds { Selection = selects[i].Text, Value = odds[i].Text });
                }

                return lstBets;
            }
        }
    }
}
