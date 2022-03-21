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
    public class LiveVolumeVM : MulticastVM
    {
        private readonly ILiveChartService _liveChartService;
        private readonly IAnalysisService _analysisService;

        public double Chart
        {
            get => Get<double>();
            set => Set(value);
        }
        public DateTime Time
        {
            get => Get<DateTime>();
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

        public LiveVolumeVM(ILiveChartService liveChartService, IAnalysisService analysisService)
        {
            _liveChartService = liveChartService;
            _analysisService = analysisService;


            TimeFrame = _analysisService.GetTimeFrame().FirstOrDefault().Frame;
            Instrument = Convert.ToString(_analysisService.GetInstrument().FirstOrDefault().Id);

            var timer = Observable.Interval(TimeSpan.FromSeconds(3));
            timer.Subscribe(x =>
            {
                var t = x;
                var temp = _liveChartService.GetSymbolCurrentPrice((int)x, Instrument);
                Chart = temp[0].Price;
                Time = temp[0].Time;
                PushUpdates();
            });
        }

        public void UpdateTime(string key)
        {
            TimeFrame = key;
        }
        public void UpdateInstrument(string key)
        {
            Instrument = key;
        }

    }
    }
