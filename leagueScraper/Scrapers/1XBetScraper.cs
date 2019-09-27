using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using leagueScraper.Data._1XBet;
using leagueScraper.Model;
using Newtonsoft.Json;

namespace leagueScraper.Scrapers
{
    public static class _1XBetScraper
    {
        const string URL = "https://1xbet.ng/LineFeed/GetSportsShortZip?sports=1&lng=en&tf=2200000&tz=1&antisports=198&withCountries=true&country=132&partner=159&virtualSports=true";
        const string CATEGORY_SOCCER = "Football";
        public static async Task<List<League>> ScrapeAsync(HttpClient client)
        {
            var json = await ApiUtility.GetAsync(client, URL);

            var data = JsonConvert.DeserializeObject<_1XBetData>(json, Converter.Settings);

            if (data == null) return null;

            var filteredData = data.Value.FirstOrDefault(x => x.N == CATEGORY_SOCCER);

            if (filteredData == null) throw new Exception("No soccer category");

            var leagues = new List<League>();
            foreach (var lg in filteredData.L)
            {
                (var country, var countryId, var leagueName, var leagueId) = (lg.Cn, lg.Ci, lg.LL, lg.Li);

                leagues.Add(new League { Country = country, CountryId = countryId.ToString(), LeagueId = leagueId.ToString(), LeagueName = leagueName, Site = "1XBet" });
                
            }

            return leagues;
        }
    }
}
