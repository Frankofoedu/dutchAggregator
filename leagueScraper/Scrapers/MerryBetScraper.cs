using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using leagueScraper.Data.MerryBetData;
using leagueScraper.Model;
using Newtonsoft.Json;

namespace leagueScraper.Scrapers
{
    public class MerryBetScraper
    {
        const string URL = "https://merrybet.com/rest/market/categories";
        const string CATEGORY_SOCCER = "Soccer";
        public static async Task<List<League>> ScrapeAsync(HttpClient client)
        {
            var json = await ApiUtility.GetAsync(client, URL);

            if (json == null) return null;


            var data = JsonConvert.DeserializeObject<MerrybetData>(json, Converter.Settings).Data.Where(x => x.SportName == CATEGORY_SOCCER);

            var leagues = new List<League>();

            foreach (var item in data)
            {
                var country = item.ParentName;
                var countryId = item.ParentCategory;

                var leagueName = item.CategoryName;
                var leagueId = item.CategoryId;

                leagues.Add(new League { Country = country, CountryId = countryId.ToString(), LeagueId = leagueId.ToString(), LeagueName = leagueName, Site = "MerryBet" });
            }

            return leagues;
        }

    }
}
