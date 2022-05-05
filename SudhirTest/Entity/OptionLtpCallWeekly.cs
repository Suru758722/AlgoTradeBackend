using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Entity
{
    public class OptionLtpCallWeekly : BaseEntity
    {
        public long Time { get; set; }
        public int ExchangeInstrumentId { get; set; }
        public double Ltp { get; set; }
        public int StrikePrice { get; set; }
        public string TimeString { get; set; }
    }
}
