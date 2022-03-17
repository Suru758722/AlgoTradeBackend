using Microsoft.EntityFrameworkCore;
using SudhirTest.Data;
using SudhirTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Services
{
    public interface ILiveChartService
    {
       List<SymbolResponseModel> GetSymbolCurrentPrice(string frame,string Instrument);

    }
    public class LiveChartService: ILiveChartService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;


        public LiveChartService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

        }
    public  List<SymbolResponseModel> GetSymbolCurrentPrice(string frame,string instrument)
        {
           
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var data = context.InstrumentData.OrderByDescending(x => x.Id).FirstOrDefault();
                    List<SymbolResponseModel> model = new List<SymbolResponseModel>();
                    model.Add(new SymbolResponseModel { Price = data.LastTradedPrice, Time = data.Time });

                    return model;
                }
            }catch(Exception ex)
            {
                List<SymbolResponseModel> model = new List<SymbolResponseModel>();
                model.Add(new SymbolResponseModel { Price = 0, Time = DateTime.Now });
                return model;
            } 
        }
    }
}
