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
        public static readonly string normalizedFilePath = folder + "NormalisedSelection.xml";
    }
}
