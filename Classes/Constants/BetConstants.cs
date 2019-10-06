using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Constants
{
    public class BetConstants
    {

        public static readonly string folder =  System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "xml/");
        public static readonly string bet9jaFilePath = folder + "bet9ja" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml";
        public static readonly string betPawaFilePath =folder + "betPawa" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml";
        public static readonly string merryBetFilePath = folder + "merryBet" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml";
        public static readonly string sportyBetFilePath = folder + "sportyBet" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml";
        public static readonly string oneXBetFilePath = folder + "1xBet" + DateTime.Now.ToShortDateString().Replace('/', '-').Replace('.', '_') + ".xml";
        public static readonly string normalizedFilePath = folder + "NormalisedSelection.xml";




        #region SiteNames

        public const string BET9JANAME = "bet9ja";
        public const string ONEXBETNAME = "1xbet";
        public const string MERRYBETNAME = "merrybet";
        public const string SPORTYBETNAME = "sportybet";
        public const string BETPAWANAME = "betpawa";

        #endregion


        public static readonly string bet9jaLeagueFilePath = folder + $"{BET9JANAME}league.xml";
        public static readonly string betPawaLeagueFilePath = folder + $"{BETPAWANAME}league.xml";
        public static readonly string merryBetLeagueFilePath = folder + $"{MERRYBETNAME}league.xml";
        public static readonly string sportyBetLeagueFilePath = folder + $"{SPORTYBETNAME}league.xml";
        public static readonly string oneXBetLeagueFilePath = folder + $"{ONEXBETNAME}league.xml";
        public static readonly string normalizedLeagueFilePath = folder + "NormalizedLeague.xml";
    }
}
