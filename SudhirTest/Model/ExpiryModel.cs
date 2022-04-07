using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Model
{
    public class ExpiryModel
    {
        public List<string> Result { get; set; }
    }
    public class ExpiryOptionModel
    {
        public List<ResultExchangeInstrumentModel> Result { get; set; }
    }
    public class ResultExchangeInstrumentModel
    {
        public int ExchangeInstrumentId { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
    }
}
