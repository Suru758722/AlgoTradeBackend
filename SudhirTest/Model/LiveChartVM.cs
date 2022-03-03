using DotNetify;
using SudhirTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Model
{
    public class LiveChartVM : MulticastVM
    {
       // readonly ApplicationDbContext _context;
       
        //public string[][] Waveform
        //{
        //    get => Get<string[][]>();
        //    set => Set(value);
        //}

        public int[] Chart
        {
            get => Get<int[]>();
            set => Set(value);
        }
        public int[] Chart2
        {
            get => Get<int[]>();
            set => Set(value);
        }


        //public double[] Pie
        //{
        //    get => Get<double[]>();
        //    set => Set(value);
        //}

        public LiveChartVM()
        {
            //_context = context;
            var timer = Observable.Interval(TimeSpan.FromSeconds(1));
            var random = new Random();

            Chart = Enumerable.Range(1, 1).Select(_ => random.Next(17100, 17150)).ToArray();
            Chart2 = Enumerable.Range(1, 1).Select(_ => random.Next(37200, 37210)).ToArray();

            timer.Subscribe(x =>
            {
                x += 31;


                Chart = Chart.Skip(1).ToArray();
                var barTemp = Chart.ToList();
                barTemp.Add(Enumerable.Range(1, 1).Select(_ => random.Next(17100, 17110)).FirstOrDefault());
                Chart = barTemp.ToArray();
                Chart2 = Chart2.Skip(1).ToArray();
                var barTemp2 = Chart2.ToList();
                barTemp2.Add(Enumerable.Range(1, 1).Select(_ => random.Next(37200, 37210)).FirstOrDefault());
                Chart2 = barTemp2.ToArray();

                PushUpdates();
            });
        }
    }
    }
