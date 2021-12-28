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
using Rg.Plugins.Popup.Services;
using StepMaster.Views;
using StepMaster.Managers;
using StepMaster.Challenges;
using Plugin.LocalNotification;
using System.Globalization;

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
        private int _dailyStepsTarget;
        private bool _setUserToCompeteWith = true;
        private bool _showDailyTargetNotification = true;
        private DateTime _currentDate;
        
        private readonly SKColor[] _chartColors = new SKColor[]
        {
            SKColor.Parse("#04C9DB"),
            SKColor.Parse("#3875B3"),
            SKColor.Parse("#119DA4"),
            SKColor.Parse("#567674"),
            SKColor.Parse("#5894d1")
        };

        public string UIDToCompeteWith = null;
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
        public Command ShowNotificationOptionsCommand { get; }

        public Command ShowUserPreferencesOptionsCommand { get; }

        public ObservableCollection<StepsChartInfo> ChartInfos { get; }

        public StartViewModel(IGoogleManager googleManager, IFirebaseManager firebaseManager)
        {
            StepsDatabase.Init();
            AchievementsDatabase.Init();
            ChallengesManager.Init();

            Instance = this;

            _googleManager = googleManager;
            _firebaseManager = firebaseManager;

            UIDToCompeteWith = PreferencesManager.GetValueString(PreferencesKeysManager.UIDToCompeteWith);

            googleManager.Login(OnLoginComplete);

            CountStepsCommand = new Command(StartStopCountingSteps);
            ShowNotificationOptionsCommand = new Command(ShowNotificationOptions);
            ShowUserPreferencesOptionsCommand = new Command(ShowPreferencesOptions);
            
            ChartInfos = new ObservableCollection<StepsChartInfo>();

            StepsChart = new RadialGaugeChart
            {
                LabelTextSize = 40,
                IsAnimated = false,
                BackgroundColor = SKColor.Empty,

        };

            float dpi = DependencyService.Get<IDisplayInfo>().GetDisplayDpi();
            ChartHeight =  (int)(420 / dpi * 350);

            _currentDate = DateTime.Now;

            StepsDatabase.RemoveSteps(2);

            NumberOfSteps = StepsDatabase.GetSteps(DateTime.Now.Date);

            StepsDatabase.UpdateDailySteps(NumberOfSteps);

            _dailyStepsTarget = PreferencesManager.GetValueInt(PreferencesKeysManager.DailyStepsTarget);

            if (_dailyStepsTarget == -1)
            {
                _dailyStepsTarget = 5000;
                PreferencesManager.SetValueInt(_dailyStepsTarget, PreferencesKeysManager.DailyStepsTarget);
            }

            ChartInfos.Add(new StepsChartInfo("Twoje kroki", NumberOfSteps,
                Color.FromRgb(_chartColors[0].Red, _chartColors[0].Green, _chartColors[0].Blue), "currentSteps"));
            ChartInfos.Add(new StepsChartInfo("Cel dnia", _dailyStepsTarget,
                Color.FromRgb(_chartColors[1].Red, _chartColors[1].Green, _chartColors[1].Blue),
                "dailyTarget"));


            SetStepsChartEntries();

            string notificationTime = PreferencesManager.GetValueString(PreferencesKeysManager.NotificationTime);

            if (notificationTime == null)
            {
                PreferencesManager.SetValueString("14.00", PreferencesKeysManager.NotificationTime);

                DateTime d = DateTime.ParseExact("14.00", @"HH\.mm",
                    CultureInfo.InvariantCulture);

                if (d.TimeOfDay < DateTime.Now.TimeOfDay)
                    d.AddDays(1);

                LocalNotificationsManager.ShowNotification("Chodzenie", "Czas pójść na spacer", 12563, d, NotificationRepeat.Daily);
            }

            Device.StartTimer(TimeSpan.FromSeconds(0.5f), () =>
            {
                if (_currentDate.Date < DateTime.Now.Date)
                {
                    StepsDatabase.UpdateDailySteps(NumberOfSteps, _currentDate);

                    NumberOfSteps = 0;
                    StepsDatabase.UpdateDailySteps(NumberOfSteps);

                    _currentDate = DateTime.Now;
                }

                return true;
            });

        }

        private void ShowPreferencesOptions(object obj)
        {
            PopupNavigation.Instance.PushAsync(new UserPreferencesPopup(OnUserPreferencesChange));
        }

        private void OnUserPreferencesChange()
        {
            _dailyStepsTarget = PreferencesManager.GetValueInt(PreferencesKeysManager.DailyStepsTarget);
            _showDailyTargetNotification = true;

            int index = ChartInfos.FindIndex(x => x.Name == "dailyTarget");

            ChartInfos[index] = new StepsChartInfo("Cel dnia", _dailyStepsTarget,
                Color.FromRgb(_chartColors[1].Red, _chartColors[1].Green, _chartColors[1].Blue),
                "dailyTarget");

            ChartInfos.Sort(delegate (StepsChartInfo x1, StepsChartInfo x2)
            {
                if (x1.Value > x2.Value) return 1;
                if (x1.Value < x2.Value) return -1; else return 0;
            });

            SetStepsChartEntries();
        }

        private void ShowNotificationOptions()
        {
            PopupNavigation.Instance.PushAsync(new NotificationSettingsPopup());
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

            DependencyService.Get<IAndroidService>().StartService();

            if (NumberOfSteps >= _dailyStepsTarget && _showDailyTargetNotification)
            {
                LocalNotificationsManager.ShowNotification("Zadanie osiągnięte", "Dzienny cel: " + _dailyStepsTarget, 1288965);

                _showDailyTargetNotification = false;
            }
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

                DependencyService.Get<IAndroidService>().StartService();

                Device.StartTimer(TimeSpan.FromSeconds(2), () =>
                {
                    SetStepsChartEntries();

                    StepsDatabase.UpdateDailySteps(NumberOfSteps);

                    ChallengesManager.Check(ChallengesManager.AchievementType.dailySteps);
                    ChallengesManager.Check(ChallengesManager.AchievementType.multidaySteps);

                    return _startedCountingSteps;
                });
            }
            else
            {
                DependencyService.Get<IStepCounter>().StopSensorService();

                _startedCountingSteps = false;

                StartStopButtonText = "START!";

                DependencyService.Get<IAndroidService>().StopService();

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

                if (UIDToCompeteWith != null || _setUserToCompeteWith)
                    _firebaseManager.GetResultToCompeteWith(OnSelectedEntryToCompete, UIDToCompeteWith);

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    
                    if (UIDToCompeteWith != null || _setUserToCompeteWith)
                        _firebaseManager.GetResultToCompeteWith(OnSelectedEntryToCompete, UIDToCompeteWith);

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
            
            int i = ChartInfos.FindIndex(x => x.Name == _lastCompetition);

            if (i != -1)
            {
                ChartInfos.RemoveAt(i);
            }
            
            if (rankingEntry == null)
            {
                SetStepsChartEntries();
                return;
            }
                

            i = ChartInfos.FindIndex(x => x.Name == "competeWith" + rankingEntry.Username);

            if (i != -1)
            {
                ChartInfos.RemoveAt(i);
            }


            ChartInfos.Add(new StepsChartInfo("Rywalizuj (" + rankingEntry.Username + ")", rankingEntry.Steps,
                Color.FromRgb(_chartColors[_currentColorIndex].Red, _chartColors[_currentColorIndex].Green, _chartColors[_currentColorIndex].Blue),
                "competeWith" + rankingEntry.Username, rankingEntry.UID, ShowCompeteOptions));

            ChartInfos.Sort(delegate (StepsChartInfo x1, StepsChartInfo x2)
            {
                if (x1.Value > x2.Value) return 1;
                if (x1.Value < x2.Value) return -1; else return 0;
            });

            _lastCompetition = "competeWith" + rankingEntry.Username;
            UIDToCompeteWith = rankingEntry.UID;
            _setUserToCompeteWith = false;

            SetStepsChartEntries();
        }

        public void SetUIDToCompeteWith(string UID)
        {
            UIDToCompeteWith = UID;

            if (UIDToCompeteWith == null)
            {
                OnSelectedEntryToCompete(null);
            }
            else
            {
                PreferencesManager.SetValueString(UID, PreferencesKeysManager.UIDToCompeteWith);

                _firebaseManager.GetResultToCompeteWith(OnSelectedEntryToCompete, UIDToCompeteWith);
            }

            

        }

        private void ShowCompeteOptions(StepsChartInfo stepsChartInfo)
        {
            PopupNavigation.Instance.PushAsync(new RankingEntryDetailsPopup(_firebaseManager.GetRankingEntry(stepsChartInfo.UID),
                OnShowCompeteOptions, UIDToCompeteWith));
        }

        private void OnShowCompeteOptions(RankingEntry entry)
        {
            SetUIDToCompeteWith(entry?.UID);
        }
    }
}