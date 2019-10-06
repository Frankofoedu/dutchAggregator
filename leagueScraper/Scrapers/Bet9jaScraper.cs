using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Classes;
using leagueScraper.Data;
using Newtonsoft.Json;
using Utilities;

namespace leagueScraper.Scrapers
{
    public static class Bet9jaScraper
    {
        const string SOCCER_ID = "590";
        public static async Task<List<League>> ScrapeAsync(HttpClient client)
        {

            var payload = "{\"IDPalinsesto\":1,\"IDLingua\":2,\"TipoVisualizzazione\":1,\"StartDate\":637049664000000000,\"EndDate\":637055712000000000}";

            HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");

            var uri = "https://web.bet9ja.com/Controls/ControlsWS.asmx/GetPalimpsest";
            
            var json = await ApiUtility.PostAsync(client,uri, c);

            if (json == null) return null;

            var t = JsonConvert.DeserializeObject<Bet9jaData>(json, Converter.Settings).D.SportList.Where(x => x.IdSport.ToString() == SOCCER_ID).FirstOrDefault();

            var leagues = new List<League>();

            foreach (var group in t.GroupList)
            {
                var country = group.Gruppo;
                var countryId = group.IdGruppo;
                foreach (var league in group.EventList)
                {
                    var leagueName = league.Evento;
                    var leagueId = league.IdEvento;
                    leagues.Add(new League { Country = country, CountryId = countryId.ToString(), LeagueId = leagueId.ToString(), LeagueName = leagueName, Site = "Bet9ja" });

                }
            }

            return leagues;
        }
    }
}
