﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Model
{
    public class ToucheLineModel
    {
        public BasicModel BidInfo { get; set; }
        public BasicModel AskInfo { get; set; }
        public double LastTradedPrice { get; set; }
        public double ltp { get; set; }
        public long LastTradedTime { get; set; }
        public long ltt { get; set; }
        public long LastTradedQunatity { get; set; }
        public long ltq { get; set; }
        public int ExchangeInstrumentID { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double PercentChange { get; set; }
        public double? TotalValueTraded { get; set; }
        public double TotalTradedQuantity { get; set; }
    }
}
