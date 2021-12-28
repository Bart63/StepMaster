using Android.Content.Res;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Text;


namespace StepMaster.Managers
{
    public static class LocalNotificationsManager
    {
        public static void ShowNotification(string title, string description, int id,
             DateTime time, NotificationRepeat repeat)     
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
                    RepeatType = repeat,
                },
                Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions()
                {
                    ChannelId = "default",
                    VisibilityType = Plugin.LocalNotification.AndroidOption.AndroidVisibilityType.Public,
                    IconLargeName = new Plugin.LocalNotification.AndroidOption.AndroidIcon("icon"),
                    IconSmallName = new Plugin.LocalNotification.AndroidOption.AndroidIcon("icon")
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
                    IsProgressBarIndeterminate = false,
                    IconLargeName = new Plugin.LocalNotification.AndroidOption.AndroidIcon("icon"),
                    IconSmallName = new Plugin.LocalNotification.AndroidOption.AndroidIcon("icon")
                }

            };

            NotificationCenter.Current.Show(notification);
        }

        public static void ShowNotification(string title, string description, int id, string icon)
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
                    IsProgressBarIndeterminate = false,
                    IconLargeName = new Plugin.LocalNotification.AndroidOption.AndroidIcon(icon),
                    IconSmallName = new Plugin.LocalNotification.AndroidOption.AndroidIcon(icon)
                }

            };

            NotificationCenter.Current.Show(notification);
        }

        public static void CancelNotification(int id)
        {
            NotificationCenter.Current.Cancel(new int[] { id });
        }
    }
}
