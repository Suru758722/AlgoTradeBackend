﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Entity
{
    public class NiftyFut: BaseEntity
    {
        public double LastTradedPrice { get; set; }
        public long LastTradedTime { get; set; }
        public int ExchangeInstrumentID { get; set; }
        public long LastTradedQunatity { get; set; }
        public long  OI { get; set; }
        public string TimeString { get; set; }
    }
}
