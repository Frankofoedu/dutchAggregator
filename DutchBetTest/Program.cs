using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Scraper;
using Scraper.Models;

namespace DutchBetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started...");

            Console.WriteLine("Loading bet9ja data from file");
            var bet9jaData = LoadFromXML<Bet9ja>("bet9ja.xml");

            Console.WriteLine("Loading betPawa data from file");
            var betPawaData = LoadFromXML<BetPawa>("betPawa.xml");

            var betpawatodaymatches = new List<BetPawaMatches>();
            var bet9jatodaymatches = new List<Bet9jaMatches>();

            Console.WriteLine("Fetching Matches");
            bet9jaData.ForEach(n => bet9jatodaymatches.AddRange(n.Matches));
            betPawaData.ForEach(n => betpawatodaymatches.AddRange(n.Matches.Where(m => m.DateOfMatch == "Wed 24/07")));

            var totalFound = 0;

            Console.WriteLine("Checking for matching teams");
            foreach (var item in betpawatodaymatches)
            {
                var bet9jaMatch = bet9jatodaymatches.FirstOrDefault(m => m.TeamNames == item.TeamNames);

                if (bet9jaMatch == null)
                {
                    Console.WriteLine(item.TeamNames + " ----- Not Found");
                }
                else {
                    totalFound++;
                    Console.WriteLine(item.TeamNames + " ----- Found");
                }
            }

            Console.WriteLine(totalFound + "/" + betpawatodaymatches.Count + " betpawa teams were found in Bet9ja");

            Console.ReadLine();
        }


        /// <summary>
        /// Loads file to XML
        /// </summary>
        /// <param name="FileName">File path of the XML file</param>
        /// <returns>Bet9ja object from XML file</returns>
        public static List<T> LoadFromXML<T>(string FileName)
        {
            using (var stream = System.IO.File.OpenRead(FileName))
            {
                var obj = new List<T>();
                var xmlSerializer = new XmlSerializer(obj.GetType());
                obj = xmlSerializer.Deserialize(stream) as List<T>;
                return obj;
            }
        }
    }
}
