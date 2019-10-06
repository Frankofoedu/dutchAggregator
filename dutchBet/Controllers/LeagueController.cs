﻿using Classes;
using Classes.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace dutchBet.Controllers
{
    public class LeagueController : Controller
    {
        public List<NormalisedLeague> NormalisedLeagues { get; set; }
        public NormalisedLeague SubmittedNormal { get; set; }

        // GET: League
        public ActionResult Index(string normal)
        {
            if (System.IO.File.Exists(BetConstants.normalizedFilePath))
            {
                NormalisedLeagues = FileUtility.LoadFromXML<NormalisedLeague>(BetConstants.normalizedLeagueFilePath);
            }
            if (!string.IsNullOrWhiteSpace(normal) & NormalisedLeagues != null)
            {
                SubmittedNormal = NormalisedLeagues.FirstOrDefault(m => m.Normal == normal);
            }

            var bet9jaLeagues = FileUtility.LoadFromXML<League>(BetConstants.bet9jaLeagueFilePath).OrderByDescending(m => m.Country).ThenBy(n=>n.LeagueName).ToList();

            var sportyBetLeagues = FileUtility.LoadFromXML<League>(BetConstants.sportyBetLeagueFilePath).OrderByDescending(m => m.Country).ThenBy(n => n.LeagueName).ToList();

            var betPawaLeagues = FileUtility.LoadFromXML<League>(BetConstants.bet9jaLeagueFilePath).OrderByDescending(m => m.Country).ThenBy(n => n.LeagueName).ToList();

            var merryBetLeagues = FileUtility.LoadFromXML<League>(BetConstants.merryBetFilePath).OrderByDescending(m => m.Country).ThenBy(n => n.LeagueName).ToList();

            var oneXBetLeagues = FileUtility.LoadFromXML<League>(BetConstants.oneXBetLeagueFilePath).OrderByDescending(m => m.Country).ThenBy(n => n.LeagueName).ToList();

            if (NormalisedLeagues != null)
            {
                bet9jaLeagues.RemoveAll(x => NormalisedLeagues.Any(m => m.Bet9ja == null ? false : m.Bet9ja == x.LeagueId));
                sportyBetLeagues.RemoveAll(x => NormalisedLeagues.Any(m => m.BetPawa == null ? false : m.BetPawa == x.LeagueId));
                betPawaLeagues.RemoveAll(x => NormalisedLeagues.Any(m => m.MerryBet == null ? false : m.MerryBet == x.LeagueId));
                merryBetLeagues.RemoveAll(x => NormalisedLeagues.Any(m => m.SportyBet == null ? false : m.SportyBet == x.LeagueId));
                oneXBetLeagues.RemoveAll(x => NormalisedLeagues.Any(m => m.OneXBet == null ? false : m.SportyBet == x.LeagueId));
            }

            ViewBag.Bet9jaLeagues = bet9jaLeagues;
            ViewBag.SportyBetLeagues = sportyBetLeagues;
            ViewBag.BetPawaLeagues = betPawaLeagues;
            ViewBag.MerryBetLeagues = merryBetLeagues;
            ViewBag.OneXBetLeagues = oneXBetLeagues;

            return View(SubmittedNormal);
        }


        [HttpPost]
        public ActionResult Index(string nval, NormalisedLeague NL)
        {

            if (System.IO.File.Exists(BetConstants.normalizedLeagueFilePath))
            {
                NormalisedLeagues = FileUtility.LoadFromXML<NormalisedLeague>(BetConstants.normalizedLeagueFilePath);
            }
            else
            {
                NormalisedLeagues = new List<NormalisedLeague>();
            }

            if (string.IsNullOrWhiteSpace(NL.Normal))
            {
                ViewBag.Msg = "Error! The Normal can not be empty.";
            }
            else
            {

                if (!string.IsNullOrWhiteSpace(nval) && NormalisedLeagues != null && NL.Normal == nval)
                {
                    var editNormal = NormalisedLeagues.FirstOrDefault(n => n.Normal == nval);
                    if (editNormal == null)
                    {
                        ViewBag.Msg = "Error! The Normal to edit does not exist.";
                    }
                    else
                    {
                        editNormal.NairaBet = NL.NairaBet;
                        editNormal.MerryBet = NL.MerryBet;
                        editNormal.Bet9ja = NL.Bet9ja;
                        editNormal.BetPawa = NL.BetPawa;
                        editNormal.SportyBet = NL.SportyBet;
                        editNormal.OneXBet = NL.OneXBet;

                        ViewBag.Msg = FileUtility.SaveToXML(NormalisedLeagues, BetConstants.normalizedLeagueFilePath);
                    }
                }
                else if (NormalisedLeagues != null && NormalisedLeagues.Any(m => m.Normal == NL.Normal))
                {
                    ViewBag.Msg = "Error! The Normal already exists.";
                }
                else
                {
                    NormalisedLeagues.Add(NL);
                    ViewBag.Msg = FileUtility.SaveToXML(NormalisedLeagues, BetConstants.normalizedLeagueFilePath);
                }
            }

            var bet9jaLeagues = FileUtility.LoadFromXML<League>(BetConstants.bet9jaLeagueFilePath).OrderByDescending(m => m.Country).ThenBy(n => n.LeagueName).ToList();

            var sportyBetLeagues = FileUtility.LoadFromXML<League>(BetConstants.sportyBetLeagueFilePath).OrderByDescending(m => m.Country).ThenBy(n => n.LeagueName).ToList();

            var betPawaLeagues = FileUtility.LoadFromXML<League>(BetConstants.bet9jaLeagueFilePath).OrderByDescending(m => m.Country).ThenBy(n => n.LeagueName).ToList();

            var merryBetLeagues = FileUtility.LoadFromXML<League>(BetConstants.merryBetFilePath).OrderByDescending(m => m.Country).ThenBy(n => n.LeagueName).ToList();

            var oneXBetLeagues = FileUtility.LoadFromXML<League>(BetConstants.oneXBetLeagueFilePath).OrderByDescending(m => m.Country).ThenBy(n => n.LeagueName).ToList();

            if (NormalisedLeagues != null)
            {
                bet9jaLeagues.RemoveAll(x => NormalisedLeagues.Any(m => m.Bet9ja == null ? false : m.Bet9ja == x.LeagueId));
                sportyBetLeagues.RemoveAll(x => NormalisedLeagues.Any(m => m.BetPawa == null ? false : m.BetPawa == x.LeagueId));
                betPawaLeagues.RemoveAll(x => NormalisedLeagues.Any(m => m.MerryBet == null ? false : m.MerryBet == x.LeagueId));
                merryBetLeagues.RemoveAll(x => NormalisedLeagues.Any(m => m.SportyBet == null ? false : m.SportyBet == x.LeagueId));
                oneXBetLeagues.RemoveAll(x => NormalisedLeagues.Any(m => m.OneXBet == null ? false : m.SportyBet == x.LeagueId));
            }

            ViewBag.Bet9jaLeagues = bet9jaLeagues;
            ViewBag.SportyBetLeagues = sportyBetLeagues;
            ViewBag.BetPawaLeagues = betPawaLeagues;
            ViewBag.MerryBetLeagues = merryBetLeagues;
            ViewBag.OneXBetLeagues = oneXBetLeagues;

            return View(NL);
        }

        public ActionResult ViewNormal()
        {
            if (System.IO.File.Exists(BetConstants.normalizedLeagueFilePath))
            {
                NormalisedLeagues = FileUtility.LoadFromXML<NormalisedLeague>(BetConstants.normalizedLeagueFilePath);
            }

            return View(NormalisedLeagues);
        }

        [HttpPost]
        public ActionResult ViewNormal(string normal)
        {
            if (System.IO.File.Exists(BetConstants.normalizedLeagueFilePath))
            {
                NormalisedLeagues = FileUtility.LoadFromXML<NormalisedLeague>(BetConstants.normalizedLeagueFilePath);
            }

            NormalisedLeagues.RemoveAll(n => n.Normal == normal);

            FileUtility.SaveToXML(NormalisedLeagues, BetConstants.normalizedLeagueFilePath);

            return View(NormalisedLeagues);
        }
    }
}