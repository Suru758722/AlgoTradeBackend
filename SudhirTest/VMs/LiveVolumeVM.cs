﻿using DotNetify;
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

        public double Volume
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
        public class VolumeVmModel
        {
            public long time { get; set; }
            public long value { get; set; }

        }
        public LiveVolumeVM(ILiveChartService liveChartService, IAnalysisService analysisService)
        {
            _liveChartService = liveChartService;
            _analysisService = analysisService;


            TimeFrame = _analysisService.GetTimeFrame().FirstOrDefault().Frame;
            Instrument = Convert.ToString(_analysisService.GetInstrument().FirstOrDefault());

            var timer = Observable.Interval(TimeSpan.FromSeconds(60));
            timer.Subscribe(x =>
            {
                var t = x;
                var temp = _liveChartService.GetSymbolCurrentVolume( Instrument);
                Volume = temp[0].Volume;
                Time = temp[0].Time;
                PushUpdates();
            });
        }
        public List<VolumeVmModel> volumeList => _liveChartService.GetVolumeList(TimeFrame, Instrument).Select(x => new VolumeVmModel { time = x.Time, value = x.Volume }).ToList();

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
