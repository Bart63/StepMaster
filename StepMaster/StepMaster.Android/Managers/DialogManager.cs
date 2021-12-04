using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using StepMaster.Droid.Managers;
using StepMaster.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(DialogManager))]
namespace StepMaster.Droid.Managers
{
    class DialogManager : IDialogService
    {
        public async void ShowErrorAsync(string message, string title, string buttonText)
        {
            await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, message, buttonText);
        }

        public async void ShowErrorAsync(string message, string title, string buttonText, Action CallBackAferHide)
        {
            await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, message, buttonText);
            CallBackAferHide?.Invoke();
        }
    }
}