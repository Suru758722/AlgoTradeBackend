using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Entity
{
    public class OptionInstrument: BaseEntity
    {
        public long ExchangeInstrumentId { get; set; }
        public int StrikePrice { get; set; }
        public string Symbol { get; set; }
        public string  Expiry { get; set; }
        public string OptionType { get; set; }
        public string Discription { get; set; }
        public string DisplayName { get; set; }

    }
}
