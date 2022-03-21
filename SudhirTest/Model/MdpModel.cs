using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Model
{
    public class MdpModel
    {
        public string id
        {
            get
            {
                return ExchangeTimeStamp;
            }
        }

        public string SymbolName { get; set; }

        public ToucheLineModel Touchline { get; set; }
        public double LastTradedPrice { get; set; }
        public long LastTradedTime { get; set; }
        public string ExchangeTimeStamp { get; set; }
        public int ExchangeSegment { get; set; }
        public int ExchangeInstrumentID { get; set; }
        public long LastTradedQunatity { get; set; }
    }
}
