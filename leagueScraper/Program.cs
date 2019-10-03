using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Classes;
using leagueScraper.Scrapers;

namespace leagueScraper
{
    class Program
    {
       static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var leagueList = new List<List<League>>();


            leagueList.Add(await Bet9jaScraper.ScrapeAsync(client));

            leagueList.Add( await MerryBetScraper.ScrapeAsync(client));

            leagueList.Add(await _1XBetScraper.ScrapeAsync(client));

            leagueList.Add( await SportyBetScraper.ScrapeAsync(client));


            foreach (var listLeague in leagueList)
            {
                var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "xml/");
                Jobs.SaveToXML(listLeague, folder + $"{listLeague[0].Site}-leagueFile");
            }

            Console.WriteLine("Done");

            Console.Read();

        }

    }
}
