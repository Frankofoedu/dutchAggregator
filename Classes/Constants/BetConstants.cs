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


        public static readonly string bet9jaLeagueFilePath = folder + "bet9jaLeague.xml";
        public static readonly string betPawaLeagueFilePath = folder + "betPawaLeague.xml";
        public static readonly string merryBetLeagueFilePath = folder + "merryBetLeague.xml";
        public static readonly string sportyBetLeagueFilePath = folder + "sportyBetLeague.xml";
        public static readonly string oneXBetLeagueFilePath = folder + "1xBetLeague.xml";
        public static readonly string normalizedLeagueFilePath = folder + "NormalizedLeague.xml";
    }
}
