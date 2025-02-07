﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Classes;
using Classes.Constants;
using leagueScraper.Data;
using leagueScraper.Data.SportyBet;
using Newtonsoft.Json;
using Utilities;
using Converter = leagueScraper.Data.SportyBet.Converter;

namespace leagueScraper.Scrapers
{

    public static class SportyBetScraper
    {
        const string URL = "https://www.sportybet.com/api/ng/factsCenter/popularAndSportList?sportId=sr%3Asport%3A1&timeline=&productId=3&_t=1569432045200";

        public static async Task<List<League>> ScrapeAsync(HttpClient client)
        {
            try
            {
                var json = await ApiUtility.GetAsync(client, URL);

                var data = JsonConvert.DeserializeObject<SportyBetData>(json, Converter.Settings);

                if (data == null) return null;


                var leagues = new List<League>();
                foreach (var cat in data.Data.SportList[0].Categories)
                {
                    (var country, var countryId) = (cat.Name, cat.Id);

                    if (cat.Tournaments == null) continue;


                    foreach (var lg in cat.Tournaments)
                    {
                        (var leagueName, var leagueId) = (lg.Name, lg.Id);

                        leagues.Add(new League { Country = country, CountryId = countryId.ToString(), LeagueId = leagueId.ToString(), LeagueName = leagueName, Site = BetConstants.SPORTYBETNAME });
                    }

                }

                return leagues;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
           
        }
    }
}

