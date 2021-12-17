using Rg.Plugins.Popup.Services;
using StepMaster.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StepMaster.ViewModels
{
    class UserPreferencesViewModel : BaseViewModel
    {
        private int _dailyStepsTarget;
        private bool _isSavingEnabled;
        private Action _callback;
        public Command SaveCommand { get; set; }
        public int DailyStepsTarget
        {
            get => _dailyStepsTarget;
            set
            {
                IsSavingEnabled = value > 0;
                SetProperty(ref _dailyStepsTarget, value);
            }
        }

        public bool IsSavingEnabled
        {
            get => _isSavingEnabled;
            set => SetProperty(ref _isSavingEnabled, value);
        }

        public UserPreferencesViewModel()
        {
        }

        public UserPreferencesViewModel(Action callback)
        {
            DailyStepsTarget = PreferencesManager.GetValueInt(PreferencesKeysManager.DailyStepsTarget);

            SaveCommand = new Command(Save);
            _callback = callback;
        }

        private void Save()
        {
            PreferencesManager.SetValueInt(DailyStepsTarget, PreferencesKeysManager.DailyStepsTarget);

            _callback();

            PopupNavigation.Instance.PopAllAsync(true);

        }
    }
}
