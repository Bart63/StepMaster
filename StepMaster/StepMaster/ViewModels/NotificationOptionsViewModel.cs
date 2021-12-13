using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using StepMaster.Managers;
using System.Globalization;
using Plugin.LocalNotification;
using Rg.Plugins.Popup.Services;

namespace StepMaster.ViewModels
{
    class NotificationOptionsViewModel : BaseViewModel
    {
        private bool _areNotificationsEnabled;
        private TimeSpan _time;
        private string _buttonText;

        public Command ManageNotificationsCommand { get; set; }
        public Command SaveCommand { get; set; }

        public TimeSpan SelectedTime
        {
            get => _time;
            set
            {
                SetProperty(ref _time, value);

            }
        }

        public string ManageNotificationButtonText
        {
            get => _buttonText;
            set => SetProperty(ref _buttonText, value);
        }

        public NotificationOptionsViewModel()
        {
            _areNotificationsEnabled = PreferencesManager.GetValueBool(PreferencesKeysManager.NotificationsEnabled);

            if (_areNotificationsEnabled)
                ManageNotificationButtonText = "Wyłącz przypomnienia";
            else
                ManageNotificationButtonText = "Włącz przypomnienia";

            ManageNotificationsCommand = new Command(ManageNotifications);
            SaveCommand = new Command(Save);

            string t = PreferencesManager.GetValueString(PreferencesKeysManager.NotificationTime);

            if (t != null) 
            {
                SelectedTime = TimeSpan.ParseExact(t, @"hh\.mm", CultureInfo.InvariantCulture);
            }
            else
            {
                SelectedTime = TimeSpan.ParseExact(DateTime.Now.ToString("HH'.'mm"), @"hh\.mm", CultureInfo.InvariantCulture);
            }

                    
        }

        private void ManageNotifications()
        {
            _areNotificationsEnabled = !_areNotificationsEnabled;

            if (_areNotificationsEnabled)
                ManageNotificationButtonText = "Wyłącz przypomnienia";
            else
                ManageNotificationButtonText = "Włącz przypomnienia";
        }

        private void Save()
        {
            PreferencesManager.SetValueBool(_areNotificationsEnabled, PreferencesKeysManager.NotificationsEnabled);
            PreferencesManager.SetValueString(SelectedTime.ToString("hh'.'mm"), PreferencesKeysManager.NotificationTime);

            if (!_areNotificationsEnabled)
            {
                LocalNotificationsManager.CancelNotification(12563);
            }
            else
            {
                DateTime d = DateTime.ParseExact(
                    SelectedTime.ToString("hh'.'mm"), @"HH\.mm",
                    CultureInfo.InvariantCulture);
                LocalNotificationsManager.ShowNotification("Chodzenie", "Czas pójść na spacer", 12563, DateTime.ParseExact(
                    SelectedTime.ToString("hh'.'mm"), @"HH\.mm",
                    CultureInfo.InvariantCulture), NotificationRepeat.Daily);
            }

            PopupNavigation.Instance.PopAllAsync(true);

        }
    }
}
