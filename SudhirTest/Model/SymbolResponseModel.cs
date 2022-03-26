using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Model
{
    public class SymbolResponseModel
    {
        public double Price { get; set; }
        public long Time { get; set; }
    }
    public class VolumeModel
    {
        public long Volume { get; set; }
        public long Time { get; set; }
    }
}
