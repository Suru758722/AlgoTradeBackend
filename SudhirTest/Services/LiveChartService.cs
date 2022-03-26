using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
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
        List<SymbolResponseModel> GetSymbolCurrentPrice(string Instrument);
        List<SymbolResponseModel> GetChartList(string frame, string Instrument);
        List<VolumeModel> GetSymbolCurrentVolume(string instrument);
        List<VolumeModel> GetVolumeList(string frame, string instrument);
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
        public  List<SymbolResponseModel> GetSymbolCurrentPrice(string instrument)
        {
           
            try
            {
                string sql = "select * from " + instrument.ToLower() + " order by id desc fetch first 1 rows only";
                List<SymbolResponseModel> model = new List<SymbolResponseModel>();

                using (NpgsqlConnection conn = new NpgsqlConnection(_config["ConnectionStrings:connection"]))
                {
                    DataTable dataTable = new DataTable();

                    conn.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
                    {

                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
                        da.Fill(dataTable);
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            model.Add(new SymbolResponseModel { Price = (double)dr.ItemArray[1], Time = (long)dr.ItemArray[2] });

                        }
                       
                    }
                    conn.Close();
                }
                
                return model;
                }catch(Exception ex)
            {
                List<SymbolResponseModel> model = new List<SymbolResponseModel>();
                model.Add(new SymbolResponseModel { Price = 0, Time = 56464 });
                return model;
            } 
        }
     
        public  List<VolumeModel> GetSymbolCurrentVolume(string instrument)
        {
           
            try
            {
                string sql = "select * from " + instrument.ToLower() + " order by id desc fetch first 1 rows only";
                List<VolumeModel> model = new List<VolumeModel>();

                using (NpgsqlConnection conn = new NpgsqlConnection(_config["ConnectionStrings:connection"]))
                {
                    DataTable dataTable = new DataTable();

                    conn.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
                    {

                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
                        da.Fill(dataTable);
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            model.Add(new VolumeModel { Volume = (long)dr.ItemArray[4], Time = (long)dr.ItemArray[2] });

                        }
                       
                    }
                    conn.Close();
                }
                
                return model;
                }catch(Exception ex)
            {
                List<VolumeModel> model = new List<VolumeModel>();
                model.Add(new VolumeModel { Volume = 0, Time = 5646432387 });
                return model;
            } 
        }


        public List<VolumeModel> GetVolumeList(string frame, string instrument)
        {

            try
            {
                string sql = "select * from " + instrument.ToLower();
                List<VolumeModel> model = new List<VolumeModel>();

                using (NpgsqlConnection conn = new NpgsqlConnection(_config["ConnectionStrings:connection"]))
                {
                    DataTable dataTable = new DataTable();

                    conn.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
                    {

                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
                        da.Fill(dataTable);
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            model.Add(new VolumeModel { Volume = (long)dr.ItemArray[4], Time = (long)dr.ItemArray[2] });

                        }

                    }
                    conn.Close();
                }

                return model;
            }
            catch (Exception ex)
            {
                List<VolumeModel> model = new List<VolumeModel>();
                model.Add(new VolumeModel { Volume = 0, Time = 3355223895});
                return model;
            }
        }
        public List<SymbolResponseModel> GetChartList(string frame, string instrument)
        {

            try
            {
                string sql = "select * from " + instrument.ToLower();
                List<SymbolResponseModel> model = new List<SymbolResponseModel>();
                List<SymbolResponseModel> hrModel = new List<SymbolResponseModel>();
                List<SymbolResponseModel> dailyModel = new List<SymbolResponseModel>();

                using (NpgsqlConnection conn = new NpgsqlConnection(_config["ConnectionStrings:connection"]))
                {
                    DataTable dataTable = new DataTable();

                    conn.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
                    {

                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
                        da.Fill(dataTable);
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            model.Add(new SymbolResponseModel { Price = (double)dr.ItemArray[1], Time = (long)dr.ItemArray[2] });

                        }
                        
                    }
                    conn.Close();
                }

                if(frame == "day")
                {
                    foreach (var element in model)
                    {
                        if (dailyModel.Count() == 0)
                        {
                            dailyModel.Add(new SymbolResponseModel { Time = element.Time, Price = element.Price });
                        }
                        else if (element.Time >= (dailyModel.LastOrDefault().Time + 86400))
                        {
                            dailyModel.Add(new SymbolResponseModel { Time = element.Time, Price = element.Price });
                        }
                    }
                    return dailyModel;
                }
                else if(frame == "hr")
                {
                    foreach (var element in model)
                    {
                        if (hrModel.Count() == 0)
                        {
                            hrModel.Add(new SymbolResponseModel { Time = element.Time, Price = element.Price });
                        }
                        else if (element.Time >= (hrModel.LastOrDefault().Time + 3600))
                        {
                            hrModel.Add(new SymbolResponseModel { Time = element.Time, Price = element.Price });
                        }
                    }
                    return hrModel;

                }
                else 
                {
                    return model;
                }

            }
            catch (Exception ex)
            {
                List<SymbolResponseModel> model = new List<SymbolResponseModel>();
                model.Add(new SymbolResponseModel { Price = 0, Time = 5454 });
                return model;
            }
        }
        public DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime.AddYears(10);
        }
    }
}
