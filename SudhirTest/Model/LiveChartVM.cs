using DotNetify;
using Microsoft.EntityFrameworkCore;
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

        public LiveChartVM(ILiveChartService liveChartService)
        {
            _liveChartService = liveChartService;
            var timer = Observable.Interval(TimeSpan.FromSeconds(2));
            timer.Subscribe(x =>
            {
                x += 31;
                var temp = _liveChartService.GetSymbolCurrentPrice(TimeFrame);
                Chart = temp[0].Price;
                Time = temp[0].Time;
                PushUpdates();
            });
        }

        public void UpdateData(string name)
        {
            var temp = 53;
        }


    }
    }
