using Android.App;
using Android.Content;
using Android.OS;
using StepMaster.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using StepMaster.ViewModels;


namespace StepMaster.Managers
{
    [Service]
    public class ServiceManager : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public const int ServiceRunningNotifID = 11999;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Notification notif = DependencyService.Get<INotification>().ReturnNotif("Twoje kroki", StartViewModel.Instance.NumberOfSteps.ToString());
            StartForeground(ServiceRunningNotifID, notif);

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override bool StopService(Intent name)
        {
            return base.StopService(name);
        }
    }
}
