using Scraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScrapeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing scrapper...");
            var action = new ScrapeBet9ja();

            Console.WriteLine("Starting Bet9jaScrape...");
            var rtn = action.Scrape();

            Console.WriteLine("Done with Bet9jaScrape...");

            Console.WriteLine("Displaying data... \n\n\n");
            foreach (var item in rtn)
            {
                Console.WriteLine(item);
                foreach (var i in item.Matches)
                {
                    Console.WriteLine("---" + i.TeamNames);
                    foreach (var odd in i.Odds)
                    {
                        Console.WriteLine("--------" + odd.Selection + " :: " + odd.Value);
                    }
                }
            }

            Console.WriteLine("\n\n\n Done with displaying data... \n\n\n");

            Console.WriteLine(SaveToXML(rtn, "bet9ja.xml"));


            Console.WriteLine("Starting BetPawaScrape...");

            var betpawa = new ScrapeBetPawa();

            var betpawartn = betpawa.Scrape();

            Console.WriteLine("Done with BetPawaScrape...");

            Console.WriteLine("Displaying data... \n\n\n");
            foreach (var item in betpawartn)
            {
                Console.WriteLine(item);
                foreach (var i in item.Matches)
                {
                    Console.WriteLine("---" + i.TeamNames);
                    foreach (var odd in i.Odds)
                    {
                        Console.WriteLine("--------" + odd.MainType + " - " + odd.Type + " - " + odd.Selection + " :: " + odd.Value);
                    }
                }
            }

            Console.WriteLine("\n\n\n Done with displaying data... \n\n\n");

            Console.WriteLine(SaveToXML(betpawartn, "betPawa.xml"));

            Console.ReadLine();
        }

        /// <summary>
        /// Saves file to XML
        /// </summary>
        /// <param name="obj">Object to be saved</param>
        /// <param name="FileName">File path of the new XML file</param>
        ///// <returns>Confirmation that file was saved.</returns>
        public static string SaveToXML<T>(List<T> obj, string FileName)
        {
            try
            {
                using (var writer = new System.IO.StreamWriter(FileName))
                {
                    var xmlSerializer = new XmlSerializer(obj.GetType());
                    xmlSerializer.Serialize(writer, obj);
                    writer.Flush();
                }
                return "Object saved to XML file.";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        /// <summary>
        /// Loads file to XML
        /// </summary>
        /// <param name="FileName">File path of the XML file</param>
        /// <returns>Bet9ja object from XML file</returns>
        public static Bet9ja LoadFromXML(string FileName)
        {
            using (var stream = System.IO.File.OpenRead(FileName))
            {
                var xmlSerializer = new XmlSerializer(typeof(Bet9ja));
                return xmlSerializer.Deserialize(stream) as Bet9ja;
            }
        }
    }
}
