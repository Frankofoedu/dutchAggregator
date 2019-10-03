using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Classes;
using Classes.Constants;
using leagueScraper.Data.MerryBetData;
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
                if(item.CategoryName.ToLower().Contains("outrights") || item.CategoryName.ToLower().Contains("specials") || item.CategoryName.ToLower().Contains("matches") || item.CategoryName.ToLower().Contains("forecast")|| item.CategoryName.ToLower().Contains("goalscorer") || item.CategoryName.ToLower().Contains("winner")|| item.CategoryName.ToLower().Contains("match")) continue;
                
                var country = item.ParentName;
                var countryId = item.ParentCategory;

                var leagueName = item.CategoryName;
                var leagueId = item.CategoryId;

                leagues.Add(new League { Country = country, CountryId = countryId.ToString(), LeagueId = leagueId.ToString(), LeagueName = leagueName, Site = BetConstants.MERRYBETNAME});
            }

            return leagues;
        }

    }
}
