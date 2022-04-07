using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Entity
{
    public class OptionIVPut : BaseEntity
    {
        public int StrikePrice { get; set; }
        public double IV { get; set; }
    }
}
