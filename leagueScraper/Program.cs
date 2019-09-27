using System;
using System.Net.Http;
using System.Threading.Tasks;
using leagueScraper.Scrapers;

namespace leagueScraper
{
    class Program
    {
       static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
           var bet9JaLeagues = await Bet9jaScraper.ScrapeAsync(client);

            var merrybetLeagues = await MerryBetScraper.ScrapeAsync(client);

            var _1xbet = await _1XBetScraper.ScrapeAsync(client);

            var sportybet = await SportyBetScraper.ScrapeAsync(client);

        }

    }
}
