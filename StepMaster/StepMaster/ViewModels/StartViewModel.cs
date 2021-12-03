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
using Xamarin.Forms.Internals;
using StepMaster.Database;

namespace StepMaster.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        private int _numberOfSteps;
        private bool _startedCountingSteps = false;
        private string _startStopButtonText = "START!";
        private Chart _stepsChart;
        private int _chartHeight;

        private IGoogleManager _googleManager;
        private IFirebaseManager _firebaseManager;
        private int _currentColorIndex = 2;
        private string _lastCompetitionName;
        

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

        public StartViewModel(IGoogleManager googleManager, IFirebaseManager firebaseManager)
        {
            _googleManager = googleManager;
            _firebaseManager = firebaseManager;

            googleManager.Login(OnLoginComplete);

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

            StepsDatabase.Init();

            StepsDatabase.RemoveSteps(2);

            NumberOfSteps = StepsDatabase.GetSteps(DateTime.Now.Date);


            ChartInfos.Add(new StepsChartInfo("Twoje kroki", NumberOfSteps,
                Color.FromRgb(_chartColors[0].Red, _chartColors[0].Green, _chartColors[0].Blue), "currentSteps"));
            ChartInfos.Add(new StepsChartInfo("Cel dnia", 5000, Color.FromRgb(_chartColors[1].Red, _chartColors[1].Green, _chartColors[1].Blue),
                "dailyTarget"));


            SetStepsChartEntries();

        }

        public StartViewModel()
        {

        }
            

        private void UpdateNumberOfSteps()
        {
            NumberOfSteps = DependencyService.Get<IStepCounter>().Steps;

            ChartInfos.Find(x => x.Name == "currentSteps").Value = NumberOfSteps;

            SetStepsChartEntries();

            StepsDatabase.updateDailySteps(NumberOfSteps);
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

        private void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {
                _firebaseManager.Auth(googleUser, OnFirebaseAuthCompleted);


            }
            else
            {
                Log.Warning("log in", message);
            }
        }

        private void OnFirebaseAuthCompleted(bool success)
        {
            if (success)
            {
                //_firebaseManager.SaveStepsToRanking(NumberOfSteps, _googleManager.User.Name);
                _firebaseManager.SaveStepsToRanking(NumberOfSteps, _googleManager.User.Name);

                Device.StartTimer(TimeSpan.FromMinutes(5), () =>
                {
                    _firebaseManager.GetResultToCompeteWith(OnSelectedEntryToCompete);

                    return true; 
                });

                Device.StartTimer(TimeSpan.FromMinutes(5), () =>
                {
                    _firebaseManager.SaveStepsToRanking(NumberOfSteps, DependencyService.Get<IGoogleManager>().User.Name);

                    return true;
                });
            }
            else
            {
                //TODO: some message box warning
            }
        }

        private void OnSelectedEntryToCompete(RankingEntry rankingEntry)
        {
            if (rankingEntry == null)
                return;

            if (_lastCompetitionName != "" && _lastCompetitionName != "competeWith" + rankingEntry.Username)
            {
                int i = ChartInfos.FindIndex(x => x.Name == _lastCompetitionName);

                if (i != -1)
                {
                    ChartInfos.RemoveAt(i);
                }
            }


            int index = ChartInfos.FindIndex(x => x.Name == "competeWith" + rankingEntry.Username);

            if (index != -1)
            {
                ChartInfos[index].Value = rankingEntry.Steps;
            }
            else
            {

                ChartInfos.Add(new StepsChartInfo("Rywalizuj (" + rankingEntry.Username + " )", rankingEntry.Steps,
                    Color.FromRgb(_chartColors[_currentColorIndex].Red, _chartColors[_currentColorIndex].Green, _chartColors[_currentColorIndex].Blue),
                    "competeWith" + rankingEntry.Username));

                _lastCompetitionName = "competeWith" + rankingEntry.Username;
            }

            SetStepsChartEntries();
        }
    }
}