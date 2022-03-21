using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SudhirTest.Data;
using SudhirTest.Entity;
using SudhirTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Services
{
    public interface IAnalysisService
    {
        dynamic LoadBhavCopy(object data);
        List<string> GetInstrument();
        List<TimeFrame> GetTimeFrame();
        bool DeleteOldData();

    }
    public class AnalysisService : IAnalysisService
    {
       // private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AnalysisService(IConfiguration config)
        {
           // _context = context;
            _config = config;

        }
        public dynamic LoadBhavCopy(object data)
        {
            throw new NotImplementedException();
        }
        public List<string> GetInstrument()
        {
            List<string> list = new List<string>();
            foreach (InstrumentNumberEnum val in Enum.GetValues(typeof(InstrumentNumberEnum)))
            {
                list.Add(val.ToString());
            }
                return list;
        }
        public List<TimeFrame> GetTimeFrame()
        {
            List<TimeFrame> frame = new List<TimeFrame>();
            frame.Add(new TimeFrame { Frame = "min" });
            frame.Add(new TimeFrame { Frame = "hr" });
            frame.Add(new TimeFrame { Frame = "day" });

            return frame;
        }

        public bool DeleteOldData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_config["ConnectionStrings:connection"]))
                {
                    conn.Open();
                    foreach (InstrumentNumberEnum val in Enum.GetValues(typeof(InstrumentNumberEnum)))
                     {
                        string sql = "Delete from " + val.ToString() + "Where LastTradedTime < " + DateTime.Now.AddDays(-30);
                        using (SqlCommand command = new SqlCommand(sql, conn))
                        {
                            command.ExecuteNonQuery();
                        }
                    }           
                    conn.Close();
                }
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }
    }
}
