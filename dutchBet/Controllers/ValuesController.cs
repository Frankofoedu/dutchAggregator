using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace dutchBet.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public async System.Threading.Tasks.Task<IEnumerable<string>> GetAsync()
        {
            var MerryBetPL = await loadMerryBetPremierLeagueAsync();

            if (MerryBetPL.Count>0)
            {
                var NairaBetPL = new List<Models.Soccer.Market>();

                foreach (var item in MerryBetPL)
                {
                    NairaBetPL.Add(new Models.Soccer.Market() {
                        date = item.date,
                        odds = item.odds,
                        time = item.time,
                        _event = item._event
                    });
                }




            }



            return new string[] { "value1", "value2" };
        }

        private async System.Threading.Tasks.Task<List<Models.Soccer.Market>> loadMerryBetPremierLeagueAsync()
        {
            using (WebClient Wc = new WebClient())
            {
                try
                {
                    var json = await Wc.DownloadStringTaskAsync("https://odds-scraper.herokuapp.com/api/merrybet/soccer/pl");
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Soccer.Rootobject>(json);
                    if (obj != null)
                    {
                        if (obj.success)
                        {
                            return obj.market.ToList();
                        }
                    }

                    return null;
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {

        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
