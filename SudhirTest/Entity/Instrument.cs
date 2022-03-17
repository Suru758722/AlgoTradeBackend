using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Entity
{
    public class Instrument: BaseEntity
    {
        public int ExchangeInstrumentID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Series { get; set; }
        public long InstrumentID { get; set; }
        public int? ExchangeSegmentId { get; set; }
        [ForeignKey("ExchangeSegmentId")]
        public virtual ExchangeSegment ExchangeSegment { get; set; }
    }
}
