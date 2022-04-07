﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Model
{
    public class InsertDataModel
    {
        public double LastTradedPrice { get; set; }
        public long LastTradedTime { get; set; }
    }
    public class OptionInsertModel
    {
        public string OptionType { get; set; }
        public long LastTradedTime { get; set; }
        public int StrikePrice { get; set; }

    }
}
