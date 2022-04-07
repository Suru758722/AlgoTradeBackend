﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Entity
{
    public class OptionOICall : BaseEntity
    {
        public int ExchangeInstrumentId { get; set; }
        public long Time { get; set; }
        public double OI { get; set; }
        public int StrikePrice { get; set; }
        public string TimeString { get; set; }
    }
}
