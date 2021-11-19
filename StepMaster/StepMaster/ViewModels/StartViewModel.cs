using StepMaster.Services;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Microcharts;
using SkiaSharp;

namespace StepMaster.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        private int _numberOfSteps;
        private bool _startedCountingSteps = false;
        private string _startStopButtonText = "START!";
        private ChartEntry[] chartEntries = new[]
        {
            new ChartEntry(1256)
            {
                
                Color = SKColor.Parse("#4585fa"),
                
            },
            new ChartEntry(5000)
            {
                
                Color = SKColor.Parse("#4cc5fa"),
                
            }
        };
        private Chart stepsChart;

        public int NumberOfSteps
        {
            get => _numberOfSteps;
            set => SetProperty(ref _numberOfSteps, value);
        }

        public Chart StepsChart
        {
            get => stepsChart;
            set => SetProperty(ref stepsChart, value);
        }

        public string StartStopButtonText
        {
            get => _startStopButtonText;
            set => SetProperty(ref _startStopButtonText, value);
        }

        public Command CountStepsCommand { get; }
       

        public StartViewModel()
        {
            
            CountStepsCommand = new Command(StartStopCountingSteps);
            

            StepsChart = new RadialGaugeChart
            {
                Entries = chartEntries,
                LabelTextSize = 40,
                IsAnimated = true,
                AnimationDuration = TimeSpan.FromSeconds(1.5),
                BackgroundColor = SKColor.Parse("#D4ECDD")
            };
            
        }

        private void UpdateNumberOfSteps()
        {
            NumberOfSteps = DependencyService.Get<IStepCounter>().Steps;

            setStepsChartEntries(NumberOfSteps);
        }

        private void StartStopCountingSteps()
        {
            if (!_startedCountingSteps)
            {
                DependencyService.Get<IStepCounter>().InitSensorService();
                DependencyService.Get<IStepCounter>().SetValueChangedCallbackAction(UpdateNumberOfSteps);

                _startedCountingSteps = true;

                StartStopButtonText = "STOP!";

                setStepsChartEntries(1356);
            }
            else
            {
                DependencyService.Get<IStepCounter>().StopSensorService();

                _startedCountingSteps = false;

                StartStopButtonText = "START!";

                
            }
            
        }

        private void setStepsChartEntries(int steps)
        {
            chartEntries = new[]
            {
                new ChartEntry(steps)
                {

                    Color = SKColor.Parse("#4585fa"),

                },
                new ChartEntry(5000)
                {

                    Color = SKColor.Parse("#4cc5fa"),

                }
            };

            StepsChart.Entries = chartEntries;
        }


    }
}