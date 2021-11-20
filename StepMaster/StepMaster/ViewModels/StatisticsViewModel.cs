using StepMaster.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microcharts;
using SkiaSharp;

namespace StepMaster.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        private Chart _weeklyStepsChart;
        

        private readonly SKColor[] _chartColors = new SKColor[]
        {
            SKColor.Parse("#d9ed92"),
            SKColor.Parse("#b5e48c"),
            SKColor.Parse("#99d98c"),
            SKColor.Parse("#52b69a"),
            SKColor.Parse("#34a0a4"),
            SKColor.Parse("#1a759f"),
            SKColor.Parse("#184e77"),
        };

        public Chart WeeklyStepsChart
        {
            get => _weeklyStepsChart;
            set => SetProperty(ref _weeklyStepsChart, value);
        }

        public StatisticsViewModel()
        {
            var entries = new[]
            {
                new ChartEntry(1256)
                {
                    Color = _chartColors[0],
                    Label = "Pon",
                    ValueLabel = "1256"
                },
                new ChartEntry(5693)
                {
                    Color = _chartColors[1],
                    Label = "Wt",
                    ValueLabel = "5693"
                },
                new ChartEntry(10025)
                {
                    Color = _chartColors[2],
                    Label = "Śr",
                    ValueLabel = "10025"
                },
                new ChartEntry(512)
                {
                    Color = _chartColors[3],
                    Label = "Czw",
                    ValueLabel = "512"
                },
                new ChartEntry(1250)
                {
                    Color = _chartColors[4],
                    Label = "Pt",
                    ValueLabel = "1250"
                },
                new ChartEntry(11254)
                {
                    Color = _chartColors[5],
                    Label = "Sob",
                    ValueLabel = "11254"
                },
                new ChartEntry(963)
                {
                    Color = _chartColors[6],
                    Label = "Niedz",
                    ValueLabel = "963"
                }

            };

            WeeklyStepsChart = new PointChart
            {
                Entries = entries,
                PointSize = 50,
                PointMode = PointMode.Circle,
                LabelOrientation = Orientation.Horizontal,
                LabelTextSize = 45,
                ValueLabelOrientation = Orientation.Horizontal,
                
            };
            
        }

    }
}