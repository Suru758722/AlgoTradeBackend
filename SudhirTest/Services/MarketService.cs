using Newtonsoft.Json;
using SudhirTest.Data;
using SudhirTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SudhirTest.Services
{
    public interface IMarketService
    {
        Task<dynamic> SaveMarketDataAsync();
    }
    public class MarketService : IMarketService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        public MarketService(ApplicationDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<dynamic> SaveMarketDataAsync()
        {
            try
            {
                _httpClient.BaseAddress = new Uri("https://api.binance.com");
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                CancellationToken cancellationToken = new CancellationToken();
                await PeriodicFooAsync(_httpClient, cancellationToken);
                return true;
            }catch(Exception ex)
            {
                return ex;
            }

        }
        private async Task PeriodicFooAsync(HttpClient _httpClient,CancellationToken cancellationToken)
        {
            while (true)
            {
                await Task.Delay(5000, cancellationToken);
                await GetBinanceApi(_httpClient);
            }
        }
        private async Task GetBinanceApi(HttpClient _httpClient)
        {
            try
            {
                HttpResponseMessage Res = await _httpClient.GetAsync("https://api.binance.com/api/v3/avgPrice?symbol=BNBBTC");
                if (Res.IsSuccessStatusCode)
                {
                    var response = Res.Content.ReadAsStringAsync().Result;
                    SymbolModel info = JsonConvert.DeserializeObject<SymbolModel>(response);
                    SymbolData symbol = new SymbolData();
                    symbol.SymbolId = 1;
                    symbol.Price = info.Price;
                    symbol.Time = DateTime.Now;
                    await _context.SymbolData.AddAsync(symbol);
                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception ex)
            {
            }
            
        }
       
    }
}
