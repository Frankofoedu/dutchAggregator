using Classes;
using Scraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScrapeTest
{
    class Program
    {
        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            var startTime = DateTime.Now;

            var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "xml/");

            var Scrapetasks = new List<Task<List<BetMatch>>>();

            Console.WriteLine("Fetching Betpawa");
            var at = new ScrapeBetPawa();
            Scrapetasks.Add(Task.Run(() => at.ScrapeDaily(client)));

            Console.WriteLine("Starting Bet9jaScrape...");
            var action = new ScrapeBet9ja();
            Scrapetasks.Add(action.ScrapeJsonAsync(client));

            Console.WriteLine("Testing Merrybet");
            var msc = new ScrapeMerryBet();
            Scrapetasks.Add(Task.Run(() => msc.ScrapeDaily(client)));

            Console.WriteLine("Testing Sportybet");
            var ssb = new ScrapeSportyBet();
            Scrapetasks.Add(ssb.ScrapeSportyBetDailyAsync(client));

            await Task.WhenAll(Scrapetasks);

            Console.WriteLine("Scraping Done. Saving...");

            foreach (var task in Scrapetasks)
            {
                if (task.Result != null)
                {
                    Console.WriteLine(Jobs.SaveToXML(task.Result, folder + task.Result.First().Site + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml"));
                }
            }

            Console.WriteLine("Finished All Jobs. Time spent = " + DateTime.Now.Subtract(startTime).ToString()) ;


            Console.ReadLine();



        }

    }
}
