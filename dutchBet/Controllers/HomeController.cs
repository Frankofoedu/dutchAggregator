using Classes;
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

        
        public List<NormalisedSelection> NormalisedSelections { get; set; }
        public ActionResult NormaliseOddSelection()
        {
            var folder =  System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "xml/");
            if (System.IO.File.Exists( folder + "NormalisedSelection.xml"))
            {
                NormalisedSelections = Jobs.LoadFromXML<NormalisedSelection>(folder + "NormalisedSelection.xml");
            }
            var bet9jaData = Jobs.LoadFromXML<Bet9ja>(folder + "bet9ja7-26-2019.xml");
            var bet9jaMatches = new List<Bet9jaMatches>();
            bet9jaData.ForEach(n => bet9jaMatches.AddRange(n.Matches));
            bet9jaMatches.OrderByDescending(m => m.Odds.Count()).ToList();

            var betPawaMatches = Jobs.LoadFromXML<DailyPawaMatches>(folder + "betPawa7-26-2019.xml").OrderByDescending(m => m.Odds.Count());

            var largestSelectionMatchBet9ja = bet9jaMatches.First();
            var largestSelectionMatchBetPawa = betPawaMatches.First();

            ViewBag.Bet9jaOdds = largestSelectionMatchBet9ja.Odds.OrderBy(m=>m.SelectionFull).ToList();
            ViewBag.BetPawaOdds = largestSelectionMatchBetPawa.Odds.OrderBy(m => m.SelectionFull).ToList();

            var max = largestSelectionMatchBet9ja.Odds.Count > largestSelectionMatchBetPawa.Odds.Count ? 
                largestSelectionMatchBet9ja.Odds.Count : largestSelectionMatchBetPawa.Odds.Count;

            ViewBag.Max = max;

            return View(NormalisedSelections);
        }

        [HttpPost]
        public ActionResult NormaliseOddSelection(List<NormalisedSelection> NS)
        {
            var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "xml/");
            ViewBag.Msg= Jobs.SaveToXML(NS, folder + "NormalisedSelection.xml");

            var bet9jaData = Jobs.LoadFromXML<Bet9ja>(folder + "bet9ja7-26-2019.xml");
            var bet9jaMatches = new List<Bet9jaMatches>();
            bet9jaData.ForEach(n => bet9jaMatches.AddRange(n.Matches));
            bet9jaMatches.OrderByDescending(m => m.Odds.Count()).ToList();

            var betPawaMatches = Jobs.LoadFromXML<DailyPawaMatches>(folder + "betPawa7-26-2019.xml").OrderByDescending(m => m.Odds.Count());

            var largestSelectionMatchBet9ja = bet9jaMatches.First();
            var largestSelectionMatchBetPawa = betPawaMatches.First();

            ViewBag.Bet9jaOdds = largestSelectionMatchBet9ja.Odds.OrderBy(m => m.SelectionFull).ToList();
            ViewBag.BetPawaOdds = largestSelectionMatchBetPawa.Odds.OrderBy(m => m.SelectionFull).ToList();

            var max = largestSelectionMatchBet9ja.Odds.Count > largestSelectionMatchBetPawa.Odds.Count ?
                largestSelectionMatchBet9ja.Odds.Count : largestSelectionMatchBetPawa.Odds.Count;

            ViewBag.Max = max;

            return View(NormalisedSelections);
        }
    }
}
