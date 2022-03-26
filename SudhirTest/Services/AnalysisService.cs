using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SudhirTest.Data;
using SudhirTest.Entity;
using SudhirTest.Model;
using System;
using System.Collections.Generic;
using System.Data;
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
                using (NpgsqlConnection conn = new NpgsqlConnection(_config["ConnectionStrings:connection"]))
                {
                    conn.Open();


                    List<InsertDataModel> list = new List<InsertDataModel>();
                    foreach (InstrumentNumberEnum val in Enum.GetValues(typeof(InstrumentNumberEnum)))
                     {
                        string sql = "select * from " + val.ToString().ToLower() + " order by id desc fetch first 1 rows only";
                        DataTable dataTable = new DataTable();

                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {

                            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                            da.Fill(dataTable);
                            foreach (DataRow dr in dataTable.Rows)
                            {
                                list.Add(new InsertDataModel { LastTradedPrice = (double)dr.ItemArray[1], LastTradedTime = (long)dr.ItemArray[2] });
                            }

                        }
                        DateTime latestDate = UnixTimeStampToDateTime(list.FirstOrDefault().LastTradedTime);
                        DateTime foo = latestDate.AddDays(-30);
                        long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
                        string sql1 = "delete from " + val.ToString().ToLower() + " where lasttradedtime < " + unixTime;
                        using (NpgsqlCommand command = new NpgsqlCommand(sql1, conn))
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
        public DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
