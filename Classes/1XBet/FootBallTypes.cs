using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes._1XBet
{
    public class FootBallTypes
    {

        public string Error { get; set; }
        public int ErrorCode { get; set; }
        public string Guid { get; set; }
        public int Id { get; set; }
        public bool Success { get; set; }
        public List<Data> Value { get; set; }

    }
    public class Data
    {
        public int Id { get { return LI; } }
        public int CI { get; set; }
        public int GC { get; set; }
        public string L { get; set; }
        public int LI { get; set; }
        public string LR { get; set; }
        public int SI { get; set; }
        public string SN { get; set; }
        public string SR { get; set; }
        public int? T { get; set; }
    }
}
