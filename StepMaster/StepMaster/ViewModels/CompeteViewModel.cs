using StepMaster.Models;
using StepMaster.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StepMaster.ViewModels
{
    public class CompeteViewModel : BaseViewModel
    {
        public ObservableCollection<RankingEntry> RankingEntries { get; private set; }

        public Command UpdateRankingCommand { get; }

        public Command GoogleLoginCommand { get; set; }
        public Command GoogleLogoutCommand { get; set; }

        private GoogleUser _googleUser;

        public GoogleUser GoogleUser
        {
            get { return _googleUser; }
            set { SetProperty(ref _googleUser, value); }
        }
        private bool _isLogedIn;
        private bool _isLogedOut;
        public bool IsLogedIn
        {
            get { return _isLogedIn; }
            set { SetProperty(ref _isLogedIn, value); }
        }

        public bool IsLogedOut
        {
            get { return _isLogedOut; }
            set { SetProperty(ref _isLogedOut, value); }
        }

        public CompeteViewModel()
        {
            UpdateRankingCommand = new Command(async () => await UpdateRanking());

            RankingEntries = new ObservableCollection<RankingEntry>()
            {
                new RankingEntry("domek15", "1.", 16125),
                new RankingEntry("klekot789", "2.", 15478, true),
                new RankingEntry("graczFortnite", "3.", 12482),
                new RankingEntry("gracz36", "4.", 11482),
                new RankingEntry("zielony", "5.", 10256),
                new RankingEntry("bla", "6.", 7896),
                new RankingEntry("skbd", "7.", 156),
                new RankingEntry("abcd", "8.", 56),
                new RankingEntry("qwertY", "9.", 20)
            };

            GoogleLoginCommand = new Command(GoogleLogin);
            GoogleLogoutCommand = new Command(GoogleLogout);

            IsLogedOut = true;

            GoogleLogin();
        }

        async Task UpdateRanking()
        {
            await Task.Delay(5000);
            //throw new NotImplementedException();
        }

        private void GoogleLogout()
        {
            DependencyService.Get<IGoogleManager>().Logout();
            IsLogedIn = false;
            IsLogedOut = true;
        }

        private void GoogleLogin()
        {
            DependencyService.Get<IGoogleManager>().Login(OnLoginComplete);

        }

        private void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {
                GoogleUser = googleUser;
                IsLogedIn = true;
                IsLogedOut = false;
            }
            else
            {
                Log.Warning("log in", message);
            }
        }
    }
}
