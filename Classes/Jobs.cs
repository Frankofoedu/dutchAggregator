using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Classes
{
    public static class Jobs
    {

        /// <summary>
        /// Loads file to XML
        /// </summary>
        /// <param name="FileName">File path of the XML file</param>
        /// <returns>Bet9ja object from XML file</returns>
        public static List<T> LoadFromXML<T>(string FileName)
        {
            using (var stream = System.IO.File.OpenRead(FileName))
            {
                var obj = new List<T>();
                var xmlSerializer = new XmlSerializer(obj.GetType());
                obj = xmlSerializer.Deserialize(stream) as List<T>;
                return obj;
            }
        }

        /// <summary>
        /// Saves file to XML
        /// </summary>
        /// <param name="obj">Object to be saved</param>
        /// <param name="FileName">File path of the new XML file</param>
        ///// <returns>Confirmation that file was saved.</returns>
        public static string SaveToXML<T>(List<T> obj, string FileName)
        {
            try
            {
                using (var writer = new System.IO.StreamWriter(FileName))
                {
                    var xmlSerializer = new XmlSerializer(obj.GetType());
                    xmlSerializer.Serialize(writer, obj);
                    writer.Flush();
                }
                return "Object saved to XML file.";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// Checks if two matches team names from different site are the same
        /// </summary>
        /// <param name="M1">First match team name</param>
        /// <param name="M2">Second match team name</param>
        /// <returns></returns>
        public static bool SameMatch(string M1, string M2)
        {
            var homeNawayM1 = new List<string>();
            var homeNawayM2 = new List<string>();
            var M1homeArr = new List<string>();
            var M2homeArr = new List<string>();
            var M1awayArr = new List<string>();
            var M2awayArr = new List<string>();

            M1.Split('-').ToList().ForEach(m => homeNawayM1.Add(m.Trim().ToLower()));
            M2.Split('-').ToList().ForEach(m => homeNawayM2.Add(m.Trim().ToLower()));

            homeNawayM1[0].Split(' ').ToList().ForEach(m => M1homeArr.Add(m.Trim()));
            homeNawayM1[1].Split(' ').ToList().ForEach(m => M1awayArr.Add(m.Trim()));

            homeNawayM2[0].Split(' ').ToList().ForEach(m => M2homeArr.Add(m.Trim()));
            homeNawayM2[1].Split(' ').ToList().ForEach(m => M2awayArr.Add(m.Trim()));

            M1homeArr.RemoveAll(m => m.Count() <= 3);
            M2homeArr.RemoveAll(m => m.Count() <= 3);
            M1awayArr.RemoveAll(m => m.Count() <= 3);
            M2awayArr.RemoveAll(m => m.Count() <= 3);

            return M1homeArr.Intersect(M2homeArr).Any() && M1awayArr.Intersect(M2awayArr).Any();
        }

        public static Bet9jaMatches SameMatch(DailyPawaMatches M1, List<Bet9jaMatches> Ms2)
        {
            var bet9jaMatches = Ms2.Where(m => Jobs.SameMatch(m.TeamNames, M1.TeamNames)).ToList();

            if (bet9jaMatches.Count <= 0)
            {
                return null;
            }
            else if (bet9jaMatches.Count == 1)
            {
                return bet9jaMatches[0];
            }
            else
            {
                var timeB9Matches = bet9jaMatches.Where(n=> DateTime.Parse(n.MatchTime).TimeOfDay.ToString() == M1.TimeOfMatch).ToList();

                if (timeB9Matches.Count == 1)
                {
                    return timeB9Matches[0];
                }
                else
                {
                    var homeNawayM1 = new List<string>();
                    var homeNawayM2 = new List<string>();

                    M1.TeamNames.Split('-').ToList().ForEach(m => homeNawayM1.Add(m.Trim().ToLower().Replace(" ", "")));

                    List<int> LevenshteinScores = new List<int>();
                    foreach (var M2 in Ms2)
                    {
                        M2.TeamNames.Split('-').ToList().ForEach(m => homeNawayM2.Add(m.Trim().ToLower().Replace(" ", "")));

                        LevenshteinScores.Add(ComputeLevenshteinDistance(homeNawayM1[0], homeNawayM2[0]) + ComputeLevenshteinDistance(homeNawayM1[1], homeNawayM2[1]));
                    }

                    var i = LevenshteinScores.IndexOf(LevenshteinScores.Min());
                    return Ms2[i];
                }
            }
        }

        public static MerryBet.MerrybetData SameMatch(DailyPawaMatches M1, List<MerryBet.MerrybetData> Ms2)
        {
            var merrybetMatches = Ms2.Where(m => Jobs.SameMatch(m.TeamNames, M1.TeamNames)).ToList();

            if (merrybetMatches.Count <= 0)
            {
                return null;
            }
            else if (merrybetMatches.Count == 1)
            {
                return merrybetMatches[0];
            }
            else
            {
                var timeMBMatches = merrybetMatches.Where(n => n.TimeOfMatch == M1.TimeOfMatch).ToList();

                if (timeMBMatches.Count == 1)
                {
                    return timeMBMatches[0];
                }
                else
                {
                    var homeNawayM1 = new List<string>();
                    var homeNawayM2 = new List<string>();

                    M1.TeamNames.Split('-').ToList().ForEach(m => homeNawayM1.Add(m.Trim().ToLower().Replace(" ", "")));

                    List<int> LevenshteinScores = new List<int>();
                    foreach (var M2 in Ms2)
                    {
                        M2.TeamNames.Split('-').ToList().ForEach(m => homeNawayM2.Add(m.Trim().ToLower().Replace(" ", "")));

                        LevenshteinScores.Add(ComputeLevenshteinDistance(homeNawayM1[0], homeNawayM2[0]) + ComputeLevenshteinDistance(homeNawayM1[1], homeNawayM2[1]));
                    }

                    var i = LevenshteinScores.IndexOf(LevenshteinScores.Min());
                    return Ms2[i];
                }
            }
        }


        /// <summary>
        /// Compute the Levenshtein Distance between two strings.
        /// </summary>
        public static int ComputeLevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        public static CalcClasses.TwoOddsReturn calculateForTwoOdds(double odd1, double odd2)
        {
            var x = 1.0 / (odd1 + odd2) * odd2;
            var y = 1.0 / (odd1 + odd2) * odd1;
            double x2 = x * 100;
            double y2 = y * 100;
            double rtn = x2 * odd1;

            return new CalcClasses.TwoOddsReturn()
            {
                Odd1 = odd1,
                Odd2 = odd2,
                PercentageToPlay1 = x2,
                PercentageToPlay2 = y2,
                PercentageReturns = rtn
            };
        }
    }
}
