﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Model
{
    public class SymbolModel
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DataResult Result { get; set; }

    }
    public class DataResult
    {
        public int ExchangeSegment { get; set; }
        public string ExchangeInstrumentID { get; set; }
        public string DataReponse { get; set; }
    }
}
