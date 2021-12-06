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
        public static StartViewModel Instance;

        private int _numberOfSteps;
        private bool _startedCountingSteps = false;
        private string _startStopButtonText = "START!";
        private Chart _stepsChart;
        private int _chartHeight;

        private IGoogleManager _googleManager;
        private IFirebaseManager _firebaseManager;
        private int _currentColorIndex = 2;
        private string _lastCompetition;
        

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
            Instance = this;

            _googleManager = googleManager;
            _firebaseManager = firebaseManager;

            googleManager.Login(OnLoginComplete);

            CountStepsCommand = new Command(StartStopCountingSteps);
            

            ChartInfos = new ObservableCollection<StepsChartInfo>();



            StepsChart = new RadialGaugeChart
            {
                LabelTextSize = 40,
                IsAnimated = false,
                BackgroundColor = SKColor.Empty,

        };

            float dpi = DependencyService.Get<IDisplayInfo>().GetDisplayDpi();
            ChartHeight =  (int)(420 / dpi * 350);

            StepsDatabase.Init();

            StepsDatabase.RemoveSteps(2);

            NumberOfSteps = StepsDatabase.GetSteps(DateTime.Now.Date);

            StepsDatabase.updateDailySteps(NumberOfSteps);

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
            NumberOfSteps += 1;

            int index = ChartInfos.FindIndex(x => x.Name == "currentSteps");

            ChartInfos[index] = new StepsChartInfo("Twoje kroki", NumberOfSteps,
                Color.FromRgb(_chartColors[0].Red, _chartColors[0].Green, _chartColors[0].Blue), "currentSteps");
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

                if (_googleManager.IsLoggedIn)
                {
                    _firebaseManager.Auth(_googleManager.User, OnFirebaseAuthCompleted);
                }

                Device.StartTimer(TimeSpan.FromSeconds(2), () =>
                {
                    SetStepsChartEntries();

                    StepsDatabase.updateDailySteps(NumberOfSteps);

                    return _startedCountingSteps;
                });
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
                DependencyService.Get<IDialogService>().ShowErrorAsync(message, "Błąd", "Zamknij");
            }
        }

        private void OnFirebaseAuthCompleted(bool success, string message)
        {
            if (success)
            {
                //_firebaseManager.SaveStepsToRanking(NumberOfSteps, _googleManager.User.Name);
                _firebaseManager.SaveStepsToRanking(NumberOfSteps, _googleManager.User.Name);
                _firebaseManager.GetResultToCompeteWith(OnSelectedEntryToCompete);

                Device.StartTimer(TimeSpan.FromMinutes(1), () =>
                {

                    _firebaseManager.GetResultToCompeteWith(OnSelectedEntryToCompete);

                    return true;
                });

                Device.StartTimer(TimeSpan.FromSeconds(30), () =>
                {

                    _firebaseManager.SaveStepsToRanking(NumberOfSteps, DependencyService.Get<IGoogleManager>().User.Name);

                    return _startedCountingSteps;
                });
            }
            else
            {
                DependencyService.Get<IDialogService>().ShowErrorAsync(message, "Błąd", "Zamknij");
            }
        }

        private void OnSelectedEntryToCompete(RankingEntry rankingEntry)
        {
            if (rankingEntry == null)
                return;

            
            int i = ChartInfos.FindIndex(x => x.Name == "competeWith" + rankingEntry.Username);

            if (i != -1)
            {
                ChartInfos.RemoveAt(i);
            }


            i = ChartInfos.FindIndex(x => x.Name == "competeWith" + _lastCompetition);

            if (i != -1)
            {
                ChartInfos.RemoveAt(i);
            }

            ChartInfos.Add(new StepsChartInfo("Rywalizuj (" + rankingEntry.Username + ")", rankingEntry.Steps,
                Color.FromRgb(_chartColors[_currentColorIndex].Red, _chartColors[_currentColorIndex].Green, _chartColors[_currentColorIndex].Blue),
                "competeWith" + rankingEntry.Username));

            ChartInfos.Sort(delegate (StepsChartInfo x1, StepsChartInfo x2)
            {
                if (x1.Value > x2.Value) return 1;
                if (x1.Value < x2.Value) return -1; else return 0;
            });

            _lastCompetition = "competeWith" + rankingEntry.Username;

            SetStepsChartEntries();
        }
    }
}