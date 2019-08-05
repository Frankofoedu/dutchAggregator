﻿using Classes;
using Classes.Constants;
using Classes.MerryBet;
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
        
        public Match MatchToCalculate { get; set; }
        public List<NormalisedSelection> NormalisedSelections { get; set; }
        public NormalisedSelection SubmittedNormal { get; set; }
        public List<TwoWayCompare> TwoWayCompares { get; set; }
        public TwoWayCompare TwoWayCompare { get; set; }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

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

        public ActionResult NormaliseOddSelection()
        {
           // var folder = Server.MapPath("~/xml/");
                //System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "xml/");


            if (System.IO.File.Exists(BetConstants.normalizedFilePath))
            {
                NormalisedSelections = Jobs.LoadFromXML<NormalisedSelection>(BetConstants.normalizedFilePath);
            }

            var bet9jaData = Jobs.LoadFromXML<Bet9ja>(BetConstants.bet9jaFilePath);
            var bet9jaMatches = new List<Bet9jaMatches>();
            bet9jaData.ForEach(n => bet9jaMatches.AddRange(n.Matches));
              bet9jaMatches = bet9jaMatches.OrderByDescending(m => m.Odds.Count()).ToList();

            var betPawaMatches = Jobs.LoadFromXML<DailyPawaMatches>(BetConstants.betPawaFilePath).OrderByDescending(m => m.Odds.Count());

            var merrybetMatches = Jobs.LoadFromXML<MerrybetData>(BetConstants.merryBetFilePath).OrderByDescending(m => m.Odds.Count());

            var largestSelectionMatchBet9ja = bet9jaMatches.First();
            var largestSelectionMatchBetPawa = betPawaMatches.First();
            var largestSelectionMatchMerryBet = merrybetMatches.First();

            if (NormalisedSelections != null)
            {
                largestSelectionMatchBet9ja.Odds.RemoveAll(x => NormalisedSelections.Any(m => m.Bet9ja == x.SelectionFull));
                largestSelectionMatchBetPawa.Odds.RemoveAll(x => NormalisedSelections.Any(m => m.BetPawa == x.SelectionFull));
                largestSelectionMatchMerryBet.Odds.RemoveAll(x => NormalisedSelections.Any(m => m.MerryBet == x.SelectionFull));
            }

            ViewBag.Bet9jaOdds = largestSelectionMatchBet9ja.Odds.OrderBy(m=>m.SelectionFull).ToList();
            ViewBag.BetPawaOdds = largestSelectionMatchBetPawa.Odds.OrderBy(m => m.SelectionFull).ToList();
            ViewBag.MerryBetOdds = largestSelectionMatchMerryBet.Odds.OrderBy(m => m.SelectionFull).ToList();

            return View(SubmittedNormal);
        }

        [HttpPost]
        public ActionResult NormaliseOddSelection(NormalisedSelection NS)
        {
            var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "xml/");

            if (System.IO.File.Exists(BetConstants.normalizedFilePath))
            {
                NormalisedSelections = Jobs.LoadFromXML<NormalisedSelection>(BetConstants.normalizedFilePath);
            }
            else
            {
                NormalisedSelections = new List<NormalisedSelection>();
            }

            if (NormalisedSelections!=null && NormalisedSelections.Any(m=>m.Normal == NS.Normal))
            {
                ViewBag.Msg = "Error! The Normal already exists.";
            }
            else
            {
                NormalisedSelections.Add(NS);
                ViewBag.Msg = Jobs.SaveToXML(NormalisedSelections, BetConstants.normalizedFilePath);
            }



            var bet9jaData = Jobs.LoadFromXML<Bet9ja>(BetConstants.bet9jaFilePath);
            var bet9jaMatches = new List<Bet9jaMatches>();
            bet9jaData.ForEach(n => bet9jaMatches.AddRange(n.Matches));
            bet9jaMatches.OrderByDescending(m => m.Odds.Count()).ToList();

            var betPawaMatches = Jobs.LoadFromXML<DailyPawaMatches>(BetConstants.betPawaFilePath).OrderByDescending(m => m.Odds.Count());

            var merrybetMatches = Jobs.LoadFromXML<MerrybetData>(BetConstants.merryBetFilePath).OrderByDescending(m => m.Odds.Count());

            var largestSelectionMatchBet9ja = bet9jaMatches.First();
            var largestSelectionMatchBetPawa = betPawaMatches.First();
            var largestSelectionMatchMerryBet = merrybetMatches.First();

            if (NormalisedSelections != null)
            {
                largestSelectionMatchBet9ja.Odds.RemoveAll(x => NormalisedSelections.Any(m => m.Bet9ja == x.SelectionFull));
                largestSelectionMatchBetPawa.Odds.RemoveAll(x => NormalisedSelections.Any(m => m.BetPawa == x.SelectionFull));
                largestSelectionMatchMerryBet.Odds.RemoveAll(x => NormalisedSelections.Any(m => m.MerryBet == x.SelectionFull));
            }

            ViewBag.Bet9jaOdds = largestSelectionMatchBet9ja.Odds.OrderBy(m => m.SelectionFull).ToList();
            ViewBag.BetPawaOdds = largestSelectionMatchBetPawa.Odds.OrderBy(m => m.SelectionFull).ToList();
            ViewBag.MerryBetOdds = largestSelectionMatchMerryBet.Odds.OrderBy(m => m.SelectionFull).ToList();

            return View(NS);
        }
        public ActionResult ViewNormal()
        {
            var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "xml/");
            if (System.IO.File.Exists(BetConstants.normalizedFilePath))
            {
                NormalisedSelections = Jobs.LoadFromXML<NormalisedSelection>(BetConstants.normalizedFilePath);
            }

            return View(NormalisedSelections);
        }

        [HttpPost]
        public ActionResult ViewNormal( string normal)
        {
            var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "xml/");
            if (System.IO.File.Exists(BetConstants.normalizedFilePath))
            {
                NormalisedSelections = Jobs.LoadFromXML<NormalisedSelection>(BetConstants.normalizedFilePath);
            }

            NormalisedSelections.RemoveAll( n=>  n.Normal == normal);

            Jobs.SaveToXML(NormalisedSelections, BetConstants.normalizedFilePath);

            return View(NormalisedSelections);
        }


        public ActionResult AddTwoWayComparism()
        {
            var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "xml/");
            if (System.IO.File.Exists(BetConstants.normalizedFilePath))
            {
                
                NormalisedSelections = Jobs.LoadFromXML<NormalisedSelection>(BetConstants.normalizedFilePath);
            }
            if (System.IO.File.Exists(folder + "TwoWayComparism.xml"))
            {
                TwoWayCompares = Jobs.LoadFromXML<TwoWayCompare>(folder + "TwoWayComparism.xml");
            }

            ViewBag.NS = NormalisedSelections;
            ViewBag.TwoWay = TwoWayCompares;

            return View(TwoWayCompare);
        }

        [HttpPost]
        public ActionResult AddTwoWayComparism(TwoWayCompare TWC)
        {
            var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "xml/");
            if (System.IO.File.Exists(BetConstants.normalizedFilePath))
            {
                NormalisedSelections = Jobs.LoadFromXML<NormalisedSelection>(BetConstants.normalizedFilePath);
            }
            if (System.IO.File.Exists(folder + "TwoWayComparism.xml"))
            {
                TwoWayCompares = Jobs.LoadFromXML<TwoWayCompare>(folder + "TwoWayComparism.xml");
            }
            else { TwoWayCompares = new List<TwoWayCompare>(); }

            if (TwoWayCompares.Any(m=>(m.Selection1 == TWC.Selection1 && m.Selection2== TWC.Selection2) || 
                (m.Selection1 == TWC.Selection2 && m.Selection2 == TWC.Selection1)))
            {
                ViewBag.Msg = "Error! Current Selections already exists";
            }
            else {
                TwoWayCompares.Add(TWC);
                ViewBag.Msg = Jobs.SaveToXML(TwoWayCompares, folder + "TwoWayComparism.xml");
            }

            ViewBag.NS = NormalisedSelections;
            ViewBag.TwoWay = TwoWayCompares;

            return View(TwoWayCompare);
        }
    }
}
