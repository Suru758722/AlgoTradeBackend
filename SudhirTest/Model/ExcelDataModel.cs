using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Model
{
    public class ExcelDataModel
    {
        public string Time { get; set; }
        public int Strike_Price { get; set; }
        public double Ltp_Call { get; set; }
        public double Ltp_Put { get; set; }
        public double Ltq_Call { get; set; }
        public double Ltq_Put { get; set; }
        public double Oi_Call { get; set; }
        public double Oi_Put { get; set; }

    }
}
