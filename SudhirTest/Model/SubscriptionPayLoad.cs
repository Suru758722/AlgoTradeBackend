using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SudhirTest.Model
{
    [DataContract]
    public class SubscriptionPayload : Payload
    {

        /// <summary>
        /// Gets or sets the instruments
        /// </summary>
        [DataMember(Name = "instruments")]
        public List<Instruments> instruments { get; set; }

        /// <summary>
        /// Gets or sets the message code
        /// </summary>
        [DataMember(Name = "xtsMessageCode")]
        public int xtsMessageCode { get; set; }

    }
   
}
