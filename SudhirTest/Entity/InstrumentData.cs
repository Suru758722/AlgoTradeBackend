using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Entity
{
    public class InstrumentData: BaseEntity
    {
        public double LastTradedPrice { get; set; }
        public DateTime Time { get; set; }
        public int? InstrumentId { get; set; }
        [ForeignKey("InstrumentId")]
        public virtual Instrument Instrument { get; set; }
        public int? ExchangeSegmentId { get; set; }
        [ForeignKey("ExchangeSegmentId")]
        public virtual ExchangeSegment ExchangeSegment { get; set; }
    }
}
