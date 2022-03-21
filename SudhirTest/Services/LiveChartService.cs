using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SudhirTest.Data;
using SudhirTest.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Services
{
    public interface ILiveChartService
    {
       List<SymbolResponseModel> GetSymbolCurrentPrice(int frame,string Instrument);

    }
    public class LiveChartService: ILiveChartService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly IConfiguration _config;


        public LiveChartService(IConfiguration config, IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _config = config;

        }
        public  List<SymbolResponseModel> GetSymbolCurrentPrice(int frame,string instrument)
        {
           
            try
            {
                string instrumentName = Enum.GetName(typeof(InstrumentNumberEnum), Convert.ToInt32(instrument));
                string sql = "Select * top 1 From" + instrumentName + "order by Id Desc";
                using (SqlConnection conn = new SqlConnection(_config["ConnectionStrings:connection"]))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {

                        DataTable data = new DataTable();
                        data.Load(command.ExecuteReader());

                    }
                    
                }
                List<SymbolResponseModel> model = new List<SymbolResponseModel>();
                model.Add(new SymbolResponseModel { Price = 0, Time = DateTime.Now });
                return model;
                }catch(Exception ex)
            {
                List<SymbolResponseModel> model = new List<SymbolResponseModel>();
                model.Add(new SymbolResponseModel { Price = 0, Time = DateTime.Now });
                return model;
            } 
        }
        public List<SymbolResponseModel> GetVolumeList(int frame, string instrument)
        {

            try
            {
                string instrumentName = Enum.GetName(typeof(InstrumentNumberEnum),Convert.ToInt32(instrument));
                string sql = "Select * top 1 From"+ instrumentName + "order by Id Desc";
                using (SqlConnection conn = new SqlConnection(_config["ConnectionStrings:connection"]))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                       
                            DataTable data = new DataTable();
                            data.Load(command.ExecuteReader());
                       
                }
                //using (var context = _contextFactory.CreateDbContext())
                //{
                //    var data = context.InstrumentData.Where(x => x.InstrumentId == Convert.ToInt32(instrument)).OrderByDescending(x => x.Id).Skip(frame).FirstOrDefault();
                //    List<SymbolResponseModel> model = new List<SymbolResponseModel>();
                //    model.Add(new SymbolResponseModel { Price = data.LastTradedPrice, Time = data.Time });

                //    return model;
                }
                List<SymbolResponseModel> model = new List<SymbolResponseModel>();
                model.Add(new SymbolResponseModel { Price = 0, Time = DateTime.Now });
                return model;
            }
            catch (Exception ex)
            {
                List<SymbolResponseModel> model = new List<SymbolResponseModel>();
                model.Add(new SymbolResponseModel { Price = 0, Time = DateTime.Now });
                return model;
            }
        }
    }
}
