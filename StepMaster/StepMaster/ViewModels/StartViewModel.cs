using StepMaster.Services;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Microcharts;
using SkiaSharp;
using StepMaster.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using StepMaster.Extensions;

namespace StepMaster.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        private int _numberOfSteps;
        private bool _startedCountingSteps = false;
        private string _startStopButtonText = "START!";
        private Chart _stepsChart;
        private int _chartHeight;
        

        private readonly SKColor[] _chartColors = new SKColor[]
        {
            SKColor.Parse("#04C9DB"),
            SKColor.Parse("#3875B3"),
            SKColor.Parse("#119DA4"),
            SKColor.Parse("#567674"),
            SKColor.Parse("#5894d1")
        };


        public int NumberOfSteps
        {
            get => _numberOfSteps;
            set => SetProperty(ref _numberOfSteps, value);
        }

        public Chart StepsChart
        {
            get => _stepsChart;
            set => SetProperty(ref _stepsChart, value);
        }

        public string StartStopButtonText
        {
            get => _startStopButtonText;
            set => SetProperty(ref _startStopButtonText, value);
        }

        public int ChartHeight
        {
            get => _chartHeight;
            set => SetProperty(ref _chartHeight, value);
        }

        public Command CountStepsCommand { get; }
        

        public ObservableCollection<StepsChartInfo> ChartInfos { get; }

        public StartViewModel()
        {
            CountStepsCommand = new Command(StartStopCountingSteps);
            

            ChartInfos = new ObservableCollection<StepsChartInfo>();



            StepsChart = new RadialGaugeChart
            {
                LabelTextSize = 40,
                IsAnimated = true,
                AnimationDuration = TimeSpan.FromSeconds(1.5),
                BackgroundColor = SKColor.Empty,

        };

            float dpi = DependencyService.Get<IDisplayInfo>().GetDisplayDpi();
            ChartHeight =  (int)(420 / dpi * 350);
            

            ChartInfos.Add(new StepsChartInfo("Twoje kroki", 1563, Color.FromRgb(_chartColors[0].Red, _chartColors[0].Green, _chartColors[0].Blue),
                "currentSteps"));
            ChartInfos.Add(new StepsChartInfo("Cel dnia", 5000, Color.FromRgb(_chartColors[1].Red, _chartColors[1].Green, _chartColors[1].Blue),
                "dailyTarget"));

            ChartInfos.Add(new StepsChartInfo("Pobij wynik! (Tomek154)", 17563, Color.FromRgb(_chartColors[2].Red,
                _chartColors[2].Green, _chartColors[2].Blue), "competeWithTomek154"));

            SetStepsChartEntries();

            
        }

        private void UpdateNumberOfSteps()
        {
            NumberOfSteps = DependencyService.Get<IStepCounter>().Steps;

            ChartInfos.Find(x => x.Name == "currentSteps").Value = NumberOfSteps;

            SetStepsChartEntries();

            
        }

        private void StartStopCountingSteps()
        {
            if (!_startedCountingSteps)
            {
                DependencyService.Get<IStepCounter>().InitSensorService();
                DependencyService.Get<IStepCounter>().SetValueChangedCallbackAction(UpdateNumberOfSteps);

                _startedCountingSteps = true;

                StartStopButtonText = "STOP!";

                SetStepsChartEntries();
            }
            else
            {
                DependencyService.Get<IStepCounter>().StopSensorService();

                _startedCountingSteps = false;

                StartStopButtonText = "START!";

                
            }
            
        }

        private void SetStepsChartEntries()
        {
            List<ChartEntry> entries = new List<ChartEntry>();

            foreach (StepsChartInfo c in ChartInfos)
            {
                entries.Add(new ChartEntry(c.Value)
                {
                    Color = SKColor.Parse(c.Color.ToHex())
                }); 
            }

            StepsChart.Entries = entries.ToArray();
        }

        
    }
}