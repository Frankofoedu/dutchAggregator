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
        static string folder;
        static string bet9jaFilePath;
        static string betPawaFilePath;
        static string merryBetFilePath;

        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static async Task Main(string[] args)
        {
            folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "xml/");
            bet9jaFilePath = folder + "bet9ja" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml";
            betPawaFilePath = folder + "betPawa" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml";
            merryBetFilePath = folder + "merryBet" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml";


            Console.WriteLine("TeStInG BEtPawa");
            var at = new ScrapeBetPawa();
            var bps = at.ScrapeDaily();


            Console.WriteLine("Starting Bet9jaScrape...");
            var action = new ScrapeBet9ja();
            var rtn = await action.ScrapeJsonAsync();



            Console.WriteLine("Testing Merrybet");
            var msc = new ScrapeMerryBet();
            var mscrtn = msc.ScrapeDaily();


            Console.WriteLine(Jobs.SaveToXML( bps,  betPawaFilePath));
            Console.WriteLine(Jobs.SaveToXML( mscrtn, merryBetFilePath));
            Console.WriteLine(Jobs.SaveToXML( rtn, bet9jaFilePath));

            Console.WriteLine("Finished All Jobs");
            Console.ReadLine();
        }

    }
}
