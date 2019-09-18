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


            var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "xml/");

            Console.WriteLine("Testing Sportybet");
            var ssb = new ScrapeSportyBet();
            var ssbrtn = await ssb.ScrapeSportyBetDailyAsync(client);
            Console.WriteLine(Jobs.SaveToXML(ssbrtn, folder + "sportyBet" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml"));

            Console.WriteLine("Starting Bet9jaScrape...");
            var action = new ScrapeBet9ja();
            var rtn = action.ScrapeJsonAsync(client).Result;
            if (rtn == null)
            {
                rtn = new List<BetMatch>();
            }
            Console.WriteLine(Jobs.SaveToXML(rtn, folder + "bet9ja" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml"));

            Console.WriteLine("Fetchig Betpawa");
            var at = new ScrapeBetPawa();
            var bps = at.ScrapeDaily(client);
            Console.WriteLine(Jobs.SaveToXML(bps, folder + "betPawa" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml"));

            Console.WriteLine("Testing Merrybet");
            var msc = new ScrapeMerryBet();
            var mscrtn = msc.ScrapeDaily(client);
            Console.WriteLine(Jobs.SaveToXML(mscrtn, folder + "merryBet" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml"));

            Console.WriteLine("Finished All Jobs");
            Console.ReadLine();



        }

    }
}
