using dutchBet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dutchBet.Controllers
{
    public class OddsController : Controller
    {
        public MatchOddsBySite MatchData { get; set; }

        // GET: Odds
        public ActionResult Index()
        {
            MatchData = new MatchOddsBySite();
            MatchData.SiteAndOdds = new List<SiteOdds>();

            for (int i = 0; i < 6; i++)
            {
                MatchData.SiteAndOdds.Add(new SiteOdds());
            }

            return View(MatchData);
        }

        public List<TwoOddsReturn> ProfitableReturns { get; set; }
        [HttpPost]
        public ActionResult Result(MatchOddsBySite match)
        {
            var actions = new Actions();
            ProfitableReturns = new List<TwoOddsReturn>();

            var Odd_1 = new List<MatchOdds>();
            var Odd_X = new List<MatchOdds>();
            var Odd_2 = new List<MatchOdds>();
            var Odd_1X = new List<MatchOdds>();
            var Odd_12 = new List<MatchOdds>();
            var Odd_X2 = new List<MatchOdds>();
            var Odd_U1 = new List<MatchOdds>();
            var Odd_U2 = new List<MatchOdds>();
            var Odd_U3 = new List<MatchOdds>();
            var Odd_U4 = new List<MatchOdds>();
            var Odd_U5 = new List<MatchOdds>();
            var Odd_O1 = new List<MatchOdds>();
            var Odd_O2 = new List<MatchOdds>();
            var Odd_O3 = new List<MatchOdds>();
            var Odd_O4 = new List<MatchOdds>();
            var Odd_O5 = new List<MatchOdds>();

            foreach (var a in match.SiteAndOdds)
            {
                Odd_1.Add(new MatchOdds() { Odd = a.Odd_1, Site = a.Site });
                Odd_X.Add(new MatchOdds() { Odd = a.Odd_X, Site = a.Site });
                Odd_2.Add(new MatchOdds() { Odd = a.Odd_2, Site = a.Site });
                Odd_1X.Add(new MatchOdds() { Odd = a.Odd_1X, Site = a.Site });
                Odd_12.Add(new MatchOdds() { Odd = a.Odd_12, Site = a.Site });
                Odd_X2.Add(new MatchOdds() { Odd = a.Odd_X2, Site = a.Site });
                Odd_U1.Add(new MatchOdds() { Odd = a.Odd_Under1_5, Site = a.Site });
                Odd_U2.Add(new MatchOdds() { Odd = a.Odd_Under2_5, Site = a.Site });
                Odd_U3.Add(new MatchOdds() { Odd = a.Odd_Under3_5, Site = a.Site });
                Odd_U4.Add(new MatchOdds() { Odd = a.Odd_Under4_5, Site = a.Site });
                Odd_U5.Add(new MatchOdds() { Odd = a.Odd_Under5_5, Site = a.Site });
                Odd_O1.Add(new MatchOdds() { Odd = a.Odd_Over1_5, Site = a.Site });
                Odd_O2.Add(new MatchOdds() { Odd = a.Odd_Over2_5, Site = a.Site });
                Odd_O3.Add(new MatchOdds() { Odd = a.Odd_Over3_5, Site = a.Site });
                Odd_O4.Add(new MatchOdds() { Odd = a.Odd_Over4_5, Site = a.Site });
                Odd_O5.Add(new MatchOdds() { Odd = a.Odd_Over5_5, Site = a.Site });
            }

            for (int i = 0; i < Odd_1.Count; i++)
            {
                var odd1 = Odd_1[i];

                if (odd1.Odd == 0)
                {
                    continue;
                }

                for (int j = 0; j < Odd_X2.Count; j++)
                {
                    var odd2 = Odd_X2[j];

                    if (odd2.Odd == 0)
                    {
                        continue;
                    }

                    var rtn = actions.calculateForTwoOdds(odd1.Odd, odd2.Odd);

                    rtn.Site1 = odd1.Site;
                    rtn.Site2 = odd2.Site;

                    rtn.Game1 = "1";
                    rtn.Game2 = "X2";

                    rtn.Team = match.Match;

                    ProfitableReturns.Add(rtn);
                }

            }


            for (int i = 0; i < Odd_2.Count; i++)
            {
                var odd1 = Odd_2[i];

                if (odd1.Odd == 0)
                {
                    continue;
                }

                for (int j = 0; j < Odd_1X.Count; j++)
                {
                    var odd2 = Odd_1X[j];

                    if (odd2.Odd == 0)
                    {
                        continue;
                    }

                    var rtn = actions.calculateForTwoOdds(odd1.Odd, odd2.Odd);

                    rtn.Site1 = odd1.Site;
                    rtn.Site2 = odd2.Site;

                    rtn.Game1 = "2";
                    rtn.Game2 = "1X";

                    rtn.Team = match.Match;

                    ProfitableReturns.Add(rtn);
                }

            }


            for (int i = 0; i < Odd_X.Count; i++)
            {
                var odd1 = Odd_X[i];

                if (odd1.Odd == 0)
                {
                    continue;
                }

                for (int j = 0; j < Odd_12.Count; j++)
                {
                    var odd2 = Odd_12[j];

                    if (odd2.Odd == 0)
                    {
                        continue;
                    }

                    var rtn = actions.calculateForTwoOdds(odd1.Odd, odd2.Odd);

                    rtn.Site1 = odd1.Site;
                    rtn.Site2 = odd2.Site;

                    rtn.Game1 = "X";
                    rtn.Game2 = "12";

                    rtn.Team = match.Match;

                    ProfitableReturns.Add(rtn);
                }

            }


            for (int i = 0; i < Odd_U1.Count; i++)
            {
                var odd1 = Odd_U1[i];

                if (odd1.Odd == 0)
                {
                    continue;
                }

                for (int j = 0; j < Odd_O1.Count; j++)
                {
                    var odd2 = Odd_O1[j];

                    if (odd2.Odd == 0)
                    {
                        continue;
                    }

                    var rtn = actions.calculateForTwoOdds(odd1.Odd, odd2.Odd);

                    rtn.Site1 = odd1.Site;
                    rtn.Site2 = odd2.Site;

                    rtn.Game1 = "Under 1.5";
                    rtn.Game2 = "Over 1.5";

                    rtn.Team = match.Match;

                    ProfitableReturns.Add(rtn);
                }

            }


            for (int i = 0; i < Odd_U2.Count; i++)
            {
                var odd1 = Odd_U2[i];

                if (odd1.Odd == 0)
                {
                    continue;
                }

                for (int j = 0; j < Odd_O2.Count; j++)
                {
                    var odd2 = Odd_O2[j];

                    if (odd2.Odd == 0)
                    {
                        continue;
                    }

                    var rtn = actions.calculateForTwoOdds(odd1.Odd, odd2.Odd);

                    rtn.Site1 = odd1.Site;
                    rtn.Site2 = odd2.Site;

                    rtn.Game1 = "Under 2.5";
                    rtn.Game2 = "Over 2.5";

                    rtn.Team = match.Match;

                    ProfitableReturns.Add(rtn);
                }

            }

            for (int i = 0; i < Odd_U3.Count; i++)
            {
                var odd1 = Odd_U3[i];

                if (odd1.Odd == 0)
                {
                    continue;
                }

                for (int j = 0; j < Odd_O3.Count; j++)
                {
                    var odd2 = Odd_O3[j];

                    if (odd2.Odd == 0)
                    {
                        continue;
                    }

                    var rtn = actions.calculateForTwoOdds(odd1.Odd, odd2.Odd);

                    rtn.Site1 = odd1.Site;
                    rtn.Site2 = odd2.Site;

                    rtn.Game1 = "Under 3.5";
                    rtn.Game2 = "Over 3.5";

                    rtn.Team = match.Match;

                    ProfitableReturns.Add(rtn);
                }

            }


            for (int i = 0; i < Odd_U4.Count; i++)
            {
                var odd1 = Odd_U4[i];

                if (odd1.Odd == 0)
                {
                    continue;
                }

                for (int j = 0; j < Odd_O4.Count; j++)
                {
                    var odd2 = Odd_O4[j];

                    if (odd2.Odd == 0)
                    {
                        continue;
                    }

                    var rtn = actions.calculateForTwoOdds(odd1.Odd, odd2.Odd);

                    rtn.Site1 = odd1.Site;
                    rtn.Site2 = odd2.Site;

                    rtn.Game1 = "Under 4.5";
                    rtn.Game2 = "Over 4.5";

                    rtn.Team = match.Match;

                    ProfitableReturns.Add(rtn);
                }

            }


            for (int i = 0; i < Odd_U5.Count; i++)
            {
                var odd1 = Odd_U5[i];

                if (odd1.Odd == 0)
                {
                    continue;
                }

                for (int j = 0; j < Odd_O5.Count; j++)
                {
                    var odd2 = Odd_O5[j];

                    if (odd2.Odd == 0)
                    {
                        continue;
                    }

                    var rtn = actions.calculateForTwoOdds(odd1.Odd, odd2.Odd);

                    rtn.Site1 = odd1.Site;
                    rtn.Site2 = odd2.Site;

                    rtn.Game1 = "Under 5.5";
                    rtn.Game2 = "Over 5.5";

                    rtn.Team = match.Match;

                    ProfitableReturns.Add(rtn);
                }

            }


            ViewBag.Title = "Profitable Returns";

            return View(ProfitableReturns);
        }
    }
}