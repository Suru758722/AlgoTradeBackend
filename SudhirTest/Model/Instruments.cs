using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SudhirTest.Model
{
    [DataContract]
    public class Instruments
    {
        /// <summary>
        /// Gets or sets the exchange
        /// <see cref="ExchangeSegment"/>
        /// </summary>
        [DataMember(Name = "exchangeSegment")]
        public int exchangeSegment { get; set; }

        /// <summary>
        /// Gets or sets the exchange instrument id
        /// </summary>
        [DataMember(Name = "exchangeInstrumentID")]
        public long exchangeInstrumentID { get; set; }
    }
}
