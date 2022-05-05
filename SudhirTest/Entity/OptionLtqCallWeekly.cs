using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Entity
{
    public class OptionLtqCallWeekly : BaseEntity
    {
        public int ExchangeInstrumentId { get; set; }
        public long Time { get; set; }
        public double Ltq { get; set; }
        public int StrikePrice { get; set; }
        public string TimeString { get; set; }
    }
}
