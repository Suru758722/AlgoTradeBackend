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

        public string ExchangeTimeStamp { get; set; }

    }
}
