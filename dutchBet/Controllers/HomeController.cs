using dutchBet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dutchBet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public Match MatchToCalculate { get; set; }
        public ActionResult Calculate()
        {
            ViewBag.Title = "Bet Calculate";

            return View(MatchToCalculate);
        }

        public List<TwoOddsReturn> ProfitableReturns { get; set; }
        [HttpPost]
        public ActionResult PostCalculate(Match match)
        {
            var actions = new Actions();
            ProfitableReturns = new List<TwoOddsReturn>();

            for (int i = 0; i < match.MatchOdds1.Count; i++)
            {
                var odd1 = match.MatchOdds1[i];

                if (odd1.Odd== 0)
                {
                    continue;
                }

                for (int j = 0; j < match.MatchOdds2.Count; j++)
                {
                    var odd2 = match.MatchOdds2[j];

                    if (odd2.Odd== 0)
                    {
                        continue;
                    }

                    var rtn = actions.calculateForTwoOdds(odd1.Odd, odd2.Odd);

                        rtn.Site1 = odd1.Site;
                        rtn.Site2 = odd2.Site;

                        rtn.Game1 = match.Odd1Name;
                        rtn.Game2 = match.Odd2Name;

                        rtn.Team = match.Teams;

                        ProfitableReturns.Add(rtn);
                }

            }




            ViewBag.Title = "Profitable Returns";

            return View(ProfitableReturns);
        }

    }
}
