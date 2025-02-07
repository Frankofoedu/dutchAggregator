﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Classes;
using Classes.Constants;
using Classes.CalcClasses;
using Classes.MerryBet;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Newtonsoft.Json;
using Utilities;

namespace DutchBetTest
{
    class Program
    {

        static void Main(string[] args)
        {


            Console.WriteLine("Started...");

            newAnalyse();


            Console.WriteLine("Done...");
            Console.ReadLine();
        }

        public static void newAnalyse()
        {
            var NormalisedSelections = new List<NormalisedSelection>();
            var TwoWayCompares = new List<TwoWayCompare>();
            var ProfitableReturns = new List<TwoOddsReturn>();


            if (System.IO.File.Exists(BetConstants.folder + "NormalisedSelection.xml"))
            {
                NormalisedSelections = FileUtility.LoadFromXML<NormalisedSelection>(BetConstants.folder + "NormalisedSelection.xml");
            }
            if (System.IO.File.Exists(BetConstants.folder + "TwoWayComparism.xml"))
            {
                TwoWayCompares = FileUtility.LoadFromXML<TwoWayCompare>(BetConstants.folder + "TwoWayComparism.xml");
            }

            var bet9jaMatches = FileUtility.LoadFromXML<BetMatch>(BetConstants.bet9jaFilePath);
            var sportyBetMatches = FileUtility.LoadFromXML<BetMatch>(BetConstants.sportyBetFilePath);
            var betPawaMatches = FileUtility.LoadFromXML<BetMatch>(BetConstants.betPawaFilePath);
            var merryBetMatches = FileUtility.LoadFromXML<BetMatch>(BetConstants.merryBetFilePath);

            //loops through all bet9ja matches
            for (int i = 0; i < bet9jaMatches.Count; i++)
            {
                var b9match = bet9jaMatches[i];


                #region Get all Matches that match the league and time of current match
                var sbLeagueNTimeTrimmed = sportyBetMatches.Where(m => (b9match.League.ToLower().Contains(m.League.ToLower()) || m.League.ToLower().Contains(b9match.League.ToLower())) && m.DateTimeOfMatch.ToUniversalTime().Equals(b9match.DateTimeOfMatch.ToUniversalTime())).ToList();
                var bpLeagueNTimeTrimmed = betPawaMatches.Where(m => (b9match.League.ToLower().Contains(m.League.ToLower()) || m.League.ToLower().Contains(b9match.League.ToLower())) && m.DateTimeOfMatch.ToUniversalTime().Equals(b9match.DateTimeOfMatch.ToUniversalTime())).ToList();
                var mbLeagueNTimeTrimmed = merryBetMatches.Where(m => (b9match.League.ToLower().Contains(m.League.ToLower()) || m.League.ToLower().Contains(b9match.League.ToLower())) && m.DateTimeOfMatch.ToUniversalTime().Equals(b9match.DateTimeOfMatch.ToUniversalTime())).ToList();
                #endregion

                var bpMatch = new BetMatch();
                var mbMatch = new BetMatch();
                var sbMatch = new BetMatch();

                if (bpLeagueNTimeTrimmed.Count() < 1) { bpMatch = null; }
                else { bpMatch = Jobs.SameMatch(b9match, bpLeagueNTimeTrimmed); }

                if (mbLeagueNTimeTrimmed.Count() < 1) { mbMatch = null; }
                else { mbMatch = Jobs.SameMatch(b9match, mbLeagueNTimeTrimmed); }

                if (sbLeagueNTimeTrimmed.Count() < 1) { sbMatch = null; }
                else { sbMatch = Jobs.SameMatch(b9match, sbLeagueNTimeTrimmed); }

                Console.WriteLine("\n\nBet9ja Match : " + b9match.TeamNames +
                    " ---sportybets found : " + (sbMatch != null ? sbMatch.TeamNames : "null") +
                    " ---Betpawas found : " + (bpMatch != null ? bpMatch.TeamNames : "null") +
                    " ---Merrybets found : " + (mbMatch != null ? mbMatch.TeamNames : "null")
                    );


                //calculates the percentage return on the set of matches
                ProfitableReturns.AddRange(TwoWayCompare(b9match, bpMatch, mbMatch, sbMatch));

            }

            int profitable = 0;
            foreach (var item in ProfitableReturns)
            {
                if (item.PercentageReturns > 100)
                {
                    profitable++;
                    Console.WriteLine(item.Team + "__" + item.Site1 + "__" + item.Game1 + "__" + item.Odd1 + "__" + item.Site2 + "__" + item.Game2 + "__" + item.Odd2 + "__ (" + item.PercentageReturns + ")");
                }
            }

            Console.WriteLine("Profitable = " + profitable);
        }

        public static List<TwoOddsReturn> TwoWayCompare(BetMatch b9Match, BetMatch bpMatch, BetMatch mbMatch, BetMatch sbMatch)
        {

            var NormalisedSelections = new List<NormalisedSelection>();
            var TwoWayCompares = new List<TwoWayCompare>();
            var ProfitableReturns = new List<TwoOddsReturn>();


            if (System.IO.File.Exists(BetConstants.folder + "NormalisedSelection.xml"))
            {
                NormalisedSelections = FileUtility.LoadFromXML<NormalisedSelection>(BetConstants.folder + "NormalisedSelection.xml");
            }
            if (System.IO.File.Exists(BetConstants.folder + "TwoWayComparism.xml"))
            {
                TwoWayCompares = FileUtility.LoadFromXML<TwoWayCompare>(BetConstants.folder + "TwoWayComparism.xml");
            }

            foreach (var item in TwoWayCompares)
            {
                var match = new Match();
                match.Teams = b9Match.TeamNames;

                match.Odd1Name = item.Selection1;
                match.Odd2Name = item.Selection2;

                match.MatchOdds1 = new List<MatchOdds>();
                match.MatchOdds2 = new List<MatchOdds>();

                var ItemNS1 = NormalisedSelections.First(m => m.Normal == item.Selection1);

                if (!string.IsNullOrWhiteSpace(ItemNS1.Bet9ja))
                {
                    if (b9Match != null)
                    {
                        var bet9jaMatchOdd = b9Match.Odds.FirstOrDefault(m => m.SelectionFull.Replace(" ", "") == ItemNS1.Bet9ja.Replace(" ", ""));
                        if (bet9jaMatchOdd != null)
                        {
                            double odd = 0f;
                            if (double.TryParse(bet9jaMatchOdd.Value, out odd))
                            {
                                match.MatchOdds1.Add(new MatchOdds() { Odd = odd, Site = "bet9ja" });
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(ItemNS1.BetPawa))
                {
                    if (bpMatch != null)
                    {
                        var betPawaMatchOdd = bpMatch.Odds.FirstOrDefault(m => m.SelectionFull.Replace(" ", "") == ItemNS1.BetPawa.Replace(" ", ""));
                        if (betPawaMatchOdd != null)
                        {
                            double odd = 0f;
                            if (double.TryParse(betPawaMatchOdd.Value, out odd))
                            {
                                match.MatchOdds1.Add(new MatchOdds() { Odd = odd, Site = "betpawa" });
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(ItemNS1.MerryBet))
                {
                    if (mbMatch != null)
                    {
                        var merrybetMatchOdd = mbMatch.Odds.FirstOrDefault(m => m.SelectionFull.Replace(" ", "") == ItemNS1.MerryBet.Replace(" ", ""));
                        if (merrybetMatchOdd != null)
                        {
                            double odd = 0f;
                            if (double.TryParse(merrybetMatchOdd.Value, out odd))
                            {
                                match.MatchOdds1.Add(new MatchOdds() { Odd = odd, Site = "merrybet" });
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(ItemNS1.SportyBet))
                {
                    if (sbMatch != null)
                    {
                        var sportybetMatchOdd = sbMatch.Odds.FirstOrDefault(m => m.SelectionFull.Replace(" ", "") == ItemNS1.SportyBet.Replace(" ", ""));
                        if (sportybetMatchOdd != null)
                        {
                            double odd = 0f;
                            if (double.TryParse(sportybetMatchOdd.Value, out odd))
                            {
                                match.MatchOdds1.Add(new Classes.CalcClasses.MatchOdds() { Odd = odd, Site = "sportybet" });
                            }
                        }
                    }
                }



                var ItemNS2 = NormalisedSelections.First(m => m.Normal == item.Selection2);

                if (!string.IsNullOrWhiteSpace(ItemNS2.Bet9ja))
                {
                    if (b9Match != null)
                    {
                        var bet9jaMatchOdd = b9Match.Odds.FirstOrDefault(m => m.SelectionFull.Replace(" ", "") == ItemNS2.Bet9ja.Replace(" ", ""));
                        if (bet9jaMatchOdd != null)
                        {
                            double odd = 0f;
                            if (double.TryParse(bet9jaMatchOdd.Value, out odd))
                            {
                                match.MatchOdds2.Add(new Classes.CalcClasses.MatchOdds() { Odd = odd, Site = "bet9ja" });
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(ItemNS2.BetPawa))
                {
                    if (bpMatch != null)
                    {
                        var betPawaMatchOdd = bpMatch.Odds.FirstOrDefault(m => m.SelectionFull.Replace(" ", "") == ItemNS2.BetPawa.Replace(" ", ""));
                        if (betPawaMatchOdd != null)
                        {
                            double odd = 0f;
                            if (double.TryParse(betPawaMatchOdd.Value, out odd))
                            {
                                match.MatchOdds2.Add(new MatchOdds() { Odd = odd, Site = "betpawa" });
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(ItemNS2.MerryBet))
                {
                    if (mbMatch != null)
                    {
                        var merrybetMatchOdd = mbMatch.Odds.FirstOrDefault(m => m.SelectionFull.Replace(" ", "") == ItemNS2.MerryBet.Replace(" ", ""));
                        if (merrybetMatchOdd != null)
                        {
                            double odd = 0f;
                            if (double.TryParse(merrybetMatchOdd.Value, out odd))
                            {
                                match.MatchOdds2.Add(new MatchOdds() { Odd = odd, Site = "merrybet" });
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(ItemNS2.SportyBet))
                {
                    if (sbMatch != null)
                    {
                        var sportybetMatchOdd = sbMatch.Odds.FirstOrDefault(m => m.SelectionFull.Replace(" ", "") == ItemNS2.SportyBet.Replace(" ", ""));
                        if (sportybetMatchOdd != null)
                        {
                            double odd = 0f;
                            if (double.TryParse(sportybetMatchOdd.Value, out odd))
                            {
                                match.MatchOdds2.Add(new Classes.CalcClasses.MatchOdds() { Odd = odd, Site = "sportybet" });
                            }
                        }
                    }
                }

                foreach (var odd1 in match.MatchOdds1)
                {
                    if (odd1.Odd == 0)
                    {
                        continue;
                    }

                    for (int j = 0; j < match.MatchOdds2.Count; j++)
                    {
                        var odd2 = match.MatchOdds2[j];

                        if (odd2.Odd == 0)
                        {
                            continue;
                        }

                        var rtn = Jobs.calculateForTwoOdds(odd1.Odd, odd2.Odd);

                        rtn.Site1 = odd1.Site;
                        rtn.Site2 = odd2.Site;

                        rtn.Game1 = match.Odd1Name;
                        rtn.Game2 = match.Odd2Name;

                        rtn.Team = match.Teams;

                        ProfitableReturns.Add(rtn);
                    }
                }
            }

            return ProfitableReturns;
        }

        public static void TestNameComparison()
        {

            Console.WriteLine("Loading bet9ja data from file");

            var bet9jaData = FileUtility.LoadFromXML<Bet9ja>(BetConstants.bet9jaFilePath);
            var bet9jaMatches = new List<Bet9jaMatches>();
            bet9jaData.ForEach(n => bet9jaMatches.AddRange(n.Matches));
            bet9jaMatches = bet9jaMatches.OrderByDescending(m => m.Odds.Count()).ToList();


            Console.WriteLine("Loading betPawa data from file");
            var betPawaMatches = FileUtility.LoadFromXML<DailyPawaMatches>(BetConstants.betPawaFilePath).OrderByDescending(m => m.Odds.Count()).ToList();

            Console.WriteLine("Loading merrybet data from file");
            var merrybetMatches = FileUtility.LoadFromXML<MerrybetData>(BetConstants.merryBetFilePath).OrderByDescending(m => m.Odds.Count()).ToList();




            Console.WriteLine("\nbetpawa matches\n");
            betPawaMatches.ForEach(m => Console.WriteLine(m.TeamNames));

            Console.WriteLine("\n\nbet9ja matches\n");
            bet9jaMatches.ForEach(m => Console.WriteLine(m.TeamNames));

            Console.WriteLine("\n\nmerrybet matches\n");
            merrybetMatches.ForEach(m => Console.WriteLine(m.TeamNames));


            var totalFound = 0;

            foreach (var item in betPawaMatches)
            {
                var bet9jaMatch = Jobs.SameMatch(item, bet9jaMatches);

                if (bet9jaMatch == null)
                {
                    Console.WriteLine(item.TeamNames + " ----- Not Found");
                }
                else
                {
                    totalFound++;
                    Console.WriteLine(item.TeamNames + " ----- " + bet9jaMatch.TeamNames + "--- Found");
                }
            }

            Console.WriteLine(totalFound + "/" + betPawaMatches.Count + " betpawa teams were found in Bet9ja");
            totalFound = 0;
            foreach (var item in betPawaMatches)
            {
                var merrybetMatch = Jobs.SameMatch(item, merrybetMatches);

                if (merrybetMatch == null)
                {
                    Console.WriteLine(item.TeamNames + " ----- Not Found");
                }
                else
                {
                    totalFound++;
                    Console.WriteLine(item.TeamNames + " ----- " + merrybetMatch.TeamNames + "--- Found");
                }
            }

            Console.WriteLine(totalFound + "/" + betPawaMatches.Count + " betpawa teams were found in Merrybet");
        }


        public static void TestNameComparisonBetPawaMatches()
        {

            Console.WriteLine("Loading bet9ja data from file");
            var bet9jaData = FileUtility.LoadFromXML<Bet9ja>("bet9ja7-26-2019.xml");

            Console.WriteLine("Loading betPawa data from file");
            var betPawaData = FileUtility.LoadFromXML<BetPawa>("betPawa7-26-2019.xml");

            var betpawatodaymatches = new List<BetPawaMatches>();
            var bet9jatodaymatches = new List<Bet9jaMatches>();

            Console.WriteLine("Fetching Matches");
            bet9jaData.ForEach(n => bet9jatodaymatches.AddRange(n.Matches));
            betPawaData.ForEach(n => betpawatodaymatches.AddRange(n.Matches.Where(m => m.DateOfMatch == "Fri 26/07")));

            var totalFound = 0;

            Console.WriteLine("Checking for matching teams");

            var max = betpawatodaymatches.Max(m => DateTime.Parse(m.TimeOfMatch).TimeOfDay);
            var min = betpawatodaymatches.Min(m => DateTime.Parse(m.TimeOfMatch).TimeOfDay);

            var bet9jaTimeFiltered = bet9jatodaymatches.Where(m => DateTime.ParseExact(m.MatchTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).TimeOfDay >= min &&
            DateTime.ParseExact(m.MatchTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).TimeOfDay <= max).ToList();

            Console.WriteLine("\nbetpawa matches\n");
            betpawatodaymatches.ForEach(m => Console.WriteLine(m.TeamNames));

            Console.WriteLine("\n\nbet9ja matches\n");

            bet9jaTimeFiltered.ForEach(m => Console.WriteLine(m.TeamNames));

            foreach (var item in betpawatodaymatches)
            {
                var bet9jaMatch = bet9jatodaymatches.FirstOrDefault(m => Jobs.SameMatch(m.TeamNames, item.TeamNames));

                if (bet9jaMatch == null)
                {
                    Console.WriteLine(item.TeamNames + " ----- Not Found");
                }
                else
                {
                    totalFound++;
                    Console.WriteLine(item.TeamNames + " ----- Found");
                }
            }

            Console.WriteLine(totalFound + "/" + betpawatodaymatches.Count + " betpawa teams were found in Bet9ja");

        }


        public static string toPrettyString<T>(List<T> Obj)
        {
            var str = JsonConvert.SerializeObject(Obj, Formatting.Indented);

            return str;
        }

        public static string toPrettyString<T>(T Obj)
        {
            var str = JsonConvert.SerializeObject(Obj, Formatting.Indented);

            return str;
        }
    }
}
