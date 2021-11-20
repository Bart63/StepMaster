using StepMaster.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepMaster
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            XF.Material.Forms.Material.Init(this);

            Device.SetFlags(new[] { "Brush_Experimental" });

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            //login();
        }

        private async void login()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }


        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
