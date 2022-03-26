using DotNetify;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SudhirTest.Data;
using SudhirTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Model
{
    public class LiveChartVM : MulticastVM
    {
        private readonly ILiveChartService _liveChartService;
        private readonly IAnalysisService _analysisService;

        public double Chart
        {
            get => Get<double>();
            set => Set(value);
        }
        public long Time
        {
            get => Get<long>();
            set => Set(value);
        }
        public string TimeFrame
        {
            get => Get<string>();
            set => Set(value);
        }
        public string Instrument
        {
            get => Get<string>();
            set => Set(value);
        }
        public class SymbolVmModel
        {
            public long time { get; set; }
            public double value { get; set; }

        }
        public LiveChartVM(ILiveChartService liveChartService, IAnalysisService analysisService)
        {
            _liveChartService = liveChartService;
            _analysisService = analysisService;


            TimeFrame = _analysisService.GetTimeFrame().FirstOrDefault().Frame;
            Instrument = Convert.ToString(_analysisService.GetInstrument().FirstOrDefault());
        
            var timer = Observable.Interval(TimeSpan.FromSeconds(60));
            timer.Subscribe(x =>
            {
                var t = x;
                var temp = _liveChartService.GetSymbolCurrentPrice(Instrument);
                Chart = temp[0].Price;
                Time = temp[0].Time;
                PushUpdates();
            });
        }

       
        public List<SymbolVmModel> chartList
        {
            get => _liveChartService.GetChartList(TimeFrame, Instrument).Select(x => new SymbolVmModel { time = x.Time, value = x.Price }).ToList();
            set => Set(value);
        }
        public void UpdateTime(string key)
        {
            TimeFrame = key;
            chartList = _liveChartService.GetChartList(TimeFrame, Instrument).Select(x => new SymbolVmModel { time = x.Time, value = x.Price }).ToList();

        }
        public void UpdateInstrument(string key)
        {
            Instrument = key;
            chartList = _liveChartService.GetChartList(TimeFrame, Instrument).Select(x => new SymbolVmModel { time = x.Time, value = x.Price }).ToList();

        }

    }
}
