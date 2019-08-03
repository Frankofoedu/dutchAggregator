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
        static void Main(string[] args)
        {
            ScrapeAndSaveBet9jaToday();

            //Console.WriteLine("TeStInG BEtPawa");
            //var at = new ScrapeBetPawa();
            //var bps = at.ScrapeDaily(client);

            Console.WriteLine(Jobs.SaveToXML(bps, "betPawa.xml"));

            Console.WriteLine("Testing Merrybet");
            var msc = new ScrapeMerryBet();
            var mscrtn = msc.ScrapeDaily(client);
            Console.WriteLine(Jobs.SaveToXML(mscrtn, "merryBet.xml"));

            Console.WriteLine("Finished All Jobs");
            Console.ReadLine();
        }

        public static void ScrapeAndSaveBet9jaToday()
        {
            var action = new ScrapeBet9ja();

            Console.WriteLine("Starting Bet9jaScrape...");
            var rtn = action.ScrapeJsonAsync(client).Result;

            Console.WriteLine("Done with Bet9jaScrape...");

            Console.WriteLine("Displaying data... \n\n\n");
            //foreach (var item in rtn)
            //{
            //    Console.WriteLine(item);
            //    foreach (var i in item.Matches)
            //    {
            //        Console.WriteLine("---" + i.TeamNames);
            //        foreach (var odd in i.Odds)
            //        {
            //            Console.WriteLine("--------" + odd.Selection + " :: " + odd.Value);
            //        }
            //    }
            //}

            Console.WriteLine("\n\n\n Done with displaying data... \n\n\n");

          Console.WriteLine(Jobs.SaveToXML(rtn, "bet9ja" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml"));

        }

        public static void ScrapeAndSaveBetPawaDaily()
        {

            Console.WriteLine("TeStInG BEtPawa");
            var at = new ScrapeBetPawa();
            at.ScrapeDaily(client);

            Console.WriteLine("Done with Betpawa");
            Console.Read();

        }
        public static void ScrapeAndSaveBetPawa()
        {

            Console.WriteLine("Starting BetPawaScrape...");

            var betpawa = new ScrapeBetPawa();

            var betpawartn = betpawa.ScrapeDaily(client);

            Console.WriteLine(Jobs.SaveToXML(betpawartn, "betPawa.xml"));

            Console.WriteLine("Done with BetPawaScrape...");

            Console.WriteLine("Displaying data... \n\n\n");
            foreach (var item in betpawartn)
            {
                Console.WriteLine(item.TeamNames);
                foreach (var odd in item.Odds)
                {
                    Console.WriteLine("--------" + odd.MainType + " - " + odd.Type + " - " + odd.Selection + " :: " + odd.Value);
                }

            }

            Console.WriteLine("\n\n\n Done with displaying data... \n\n\n");

            Console.WriteLine(Jobs.SaveToXML(betpawartn, "betPawa" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml"));

        }
    }
}
