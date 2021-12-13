using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Text;

namespace StepMaster.Managers
{
    public static class LocalNotificationsManager
    {
        public static void ShowNotification(string title, string description, int id,
            int value, int maxValue, DateTime time)
        {
            var notification = new NotificationRequest
            {
                BadgeNumber = 1,
                Description = description,
                Title = title,
                NotificationId = id,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = time,

                },
                Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions()
                {
                    ChannelId = "default",
                    ProgressBarMax = maxValue,
                    ProgressBarProgress = value,
                    VisibilityType = Plugin.LocalNotification.AndroidOption.AndroidVisibilityType.Public,
                    IsProgressBarIndeterminate = false
                }

            };

            NotificationCenter.Current.Show(notification);
        }

        public static void ShowNotification(string title, string description, int id)
        {
            var notification = new NotificationRequest
            {
                BadgeNumber = 1,
                Description = description,
                Title = title,
                NotificationId = id,
                Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions()
                {
                    ChannelId = "default",
                    VisibilityType = Plugin.LocalNotification.AndroidOption.AndroidVisibilityType.Public,
                    IsProgressBarIndeterminate = false
                }

            };

            NotificationCenter.Current.Show(notification);
        }
    }
}
