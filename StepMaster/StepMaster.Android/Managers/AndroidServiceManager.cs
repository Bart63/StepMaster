using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using StepMaster.Droid.Managers;
using StepMaster.Managers;
using StepMaster.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidServiceManager))]
namespace StepMaster.Droid.Managers
{
    class AndroidServiceManager : IAndroidService
    {
        private static Context context = Android.App.Application.Context;
        public void StartService()
        {
            var intent = new Intent(context, typeof(ServiceManager));

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                context.StartForegroundService(intent);
            }
            else
            {
                context.StartService(intent);
            }
        }

        public void StopService()
        {
            var intent = new Intent(context, typeof(ServiceManager));
            context.StopService(intent);
        }
    }
}