﻿using Classes;
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


            var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "xml/");
            var spt = new ScrapeSportyBet();
            var sptData = spt.ScrapeSportyBetDailyAsync(client).Result;

            Console.WriteLine(Jobs.SaveToXML(sptData, folder + "sportybet" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml"));

            ScrapeAndSaveBet9jaToday();

            Console.WriteLine("TestInG BEtPawa");
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

        public static void ScrapeAndSaveBet9jaToday()
        {
            var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "xml/");
            var action = new ScrapeBet9ja();

            Console.WriteLine("Starting Bet9jaScrape...");
            var rtn = action.ScrapeJsonAsync(client).Result;

            Console.WriteLine("Done with Bet9jaScrape...");

            Console.WriteLine(Jobs.SaveToXML(rtn, folder + "bet9ja" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml"));

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

            Console.WriteLine("Starting BetPawaScrape... at :" + DateTime.Now.ToString());

            var betpawa = new ScrapeBetPawa();

            var betpawartn = betpawa.ScrapeDaily(client);

            Console.WriteLine(Jobs.SaveToXML(betpawartn, "betPawa.xml"));

            Console.WriteLine("Done with BetPawaScrape... at: " + DateTime.Now.ToString());

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
