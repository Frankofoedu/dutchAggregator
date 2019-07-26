using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Scraper;

namespace DutchBetTest
{
    public class Selection2Compare {
        public string Site1_Site2 { get; set; }
        public string Selection { get; set; }
        public string Opposite { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started...");

            Console.WriteLine("Loading bet9ja data from file");
            var bet9jaData = LoadFromXML<Bet9ja>("bet9ja7-26-2019.xml");

            Console.WriteLine("Loading betPawa data from file");
            var betPawaData = LoadFromXML<BetPawa>("betPawa7-26-2019.xml");

            var betpawatodaymatches = new List<BetPawaMatches>();
            var bet9jatodaymatches = new List<Bet9jaMatches>();

            Console.WriteLine("Fetching Matches");
            bet9jaData.ForEach(n => bet9jatodaymatches.AddRange(n.Matches));
            betPawaData.ForEach(n => betpawatodaymatches.AddRange(n.Matches.Where(m => m.DateOfMatch == "Fri 26/07")));

            var totalFound = 0;

            Console.WriteLine("Checking for matching teams");

            var max = betpawatodaymatches.Max(m => DateTime.Parse(m.TimeOfMatch).TimeOfDay);
            var min = betpawatodaymatches.Min(m => DateTime.Parse(m.TimeOfMatch).TimeOfDay);

            var bet9jaTimeFiltered = bet9jatodaymatches.Where(m => DateTime.ParseExact(m.MatchTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).TimeOfDay >= min &&
            DateTime.ParseExact(m.MatchTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).TimeOfDay <= max).ToList();

            Console.WriteLine("\nbetpawa matches\n");
            betpawatodaymatches.ForEach(m => Console.WriteLine(m.TeamNames));

            Console.WriteLine("\n\nbet9ja matches\n");

            bet9jaTimeFiltered.ForEach(m => Console.WriteLine(m.TeamNames));

            foreach (var item in betpawatodaymatches)
            {
                var bet9jaMatch = bet9jatodaymatches.FirstOrDefault(m => SameMatch(m.TeamNames, item.TeamNames));

                if (bet9jaMatch == null)
                {
                    Console.WriteLine(item.TeamNames + " ----- Not Found");
                }
                else
                {
                    totalFound++;
                    Console.WriteLine(item.TeamNames + " ----- Found");
                }
            }

            Console.WriteLine(totalFound + "/" + betpawatodaymatches.Count + " betpawa teams were found in Bet9ja");

            Console.ReadLine();
        }

        public static bool SameMatch(string M1, string M2)
        {
            var homeNawayM1 = new List<string>();
            var homeNawayM2 = new List<string>();
            var M1homeArr = new List<string>();
            var M2homeArr = new List<string>();
            var M1awayArr = new List<string>();
            var M2awayArr = new List<string>();

            M1.Split('-').ToList().ForEach(m => homeNawayM1.Add(m.Trim().ToLower()));
            M2.Split('-').ToList().ForEach(m => homeNawayM2.Add(m.Trim().ToLower()));

            homeNawayM1[0].Split(' ').ToList().ForEach(m => M1homeArr.Add(m.Trim()));
            homeNawayM1[1].Split(' ').ToList().ForEach(m => M1awayArr.Add(m.Trim()));

            homeNawayM2[0].Split(' ').ToList().ForEach(m => M2homeArr.Add(m.Trim()));
            homeNawayM2[1].Split(' ').ToList().ForEach(m => M2awayArr.Add(m.Trim()));

            M1homeArr.RemoveAll(m => m.Count() <= 3);
            M2homeArr.RemoveAll(m => m.Count() <= 3);
            M1awayArr.RemoveAll(m => m.Count() <= 3);
            M2awayArr.RemoveAll(m => m.Count() <= 3);

            return M1homeArr.Intersect(M2homeArr).Any() && M1awayArr.Intersect(M2awayArr).Any();
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

    }
}
