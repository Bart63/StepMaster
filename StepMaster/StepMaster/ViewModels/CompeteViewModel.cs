using Rg.Plugins.Popup.Services;
using StepMaster.Models;
using StepMaster.Services;
using StepMaster.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using StepMaster.Extensions;

namespace StepMaster.ViewModels
{
    public class CompeteViewModel : BaseViewModel
    {

        public static CompeteViewModel Instance;
        public ObservableCollection<RankingEntry> RankingEntries { get; private set; }

        public Command UpdateRankingCommand { get; }

        public Command GoogleLoginCommand { get; set; }
        public Command GoogleLogoutCommand { get; set; }

        public Command RankingTapped { get; set; }

        private GoogleUser _googleUser;

        private IGoogleManager _googleManager;
        private IFirebaseManager _firebaseManager;
        
        private bool _isLogedIn;
        private bool _isLogedOut;

        private bool _isRefreshing;

        public GoogleUser GoogleUser
        {
            get { return _googleUser; }
            set { SetProperty(ref _googleUser, value); }
        }
        
        public bool IsLoggedIn
        {
            get { return _isLogedIn; }
            set { SetProperty(ref _isLogedIn, value); }
        }

        public bool IsLoggedOut
        {
            get { return _isLogedOut; }
            set { SetProperty(ref _isLogedOut, value); }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public CompeteViewModel(IGoogleManager googleManager, IFirebaseManager firebaseManager)
        {
            Instance = this;

            IsRefreshing = false;

            _googleManager = googleManager;
            _firebaseManager = firebaseManager;

            UpdateRankingCommand = new Command(async () => await UpdateRanking());
            RankingTapped = new Command<RankingEntry>(RankingEntryTapped);

            RankingEntries = new ObservableCollection<RankingEntry>();
            

            GoogleLoginCommand = new Command(GoogleLogin);
            GoogleLogoutCommand = new Command(GoogleLogout);

            IsLoggedIn = _googleManager.IsLoggedIn;
            IsLoggedOut = !_googleManager.IsLoggedIn;

            if (IsLoggedIn)
            {
                GoogleUser = _googleManager.User;

                _firebaseManager.Auth(GoogleUser, OnFirebaseAuthCompleted);
            }
            
            
        }

        public CompeteViewModel()
        {
            RankingEntries = new ObservableCollection<RankingEntry>();
        }

        async Task UpdateRanking()
        {
            if (DependencyService.Get<IGoogleManager>().IsLoggedIn)
            {
                IsRefreshing = true;

                _firebaseManager.GetRankingEntries(OnFirebaseRankingLoaded);
            }

            await Task.Delay(500);
            
        }

        public void GoogleLogout()
        {
            IsLoggedIn = false;
            IsLoggedOut = true;

            RankingEntries.Clear();

            RankingEntries.Add(new RankingEntry("Zaloguj się!", "0.", 0, false));
            
            _googleManager.Logout();
        }

        public void GoogleLogin()
        {
            _googleManager.Login(OnLoginComplete);

        }

        public void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {
                GoogleUser = googleUser;
                
                IsLoggedIn = _googleManager.IsLoggedIn;
                IsLoggedOut = !_googleManager.IsLoggedIn;

                
                GoogleUser = _googleManager.User;

                _firebaseManager.Auth(GoogleUser, OnFirebaseAuthCompleted);
                
            }
            else
            {
                DependencyService.Get<IDialogService>().ShowErrorAsync(message, "Błąd", "Zamknij");
            }
        }

        private void OnFirebaseAuthCompleted(bool success, string errors)
        {
            if (success)
            {
                _firebaseManager.SaveStepsToRanking(StartViewModel.Instance.NumberOfSteps, GoogleUser.Name);

                _firebaseManager.GetRankingEntries(OnFirebaseRankingLoaded);

                Device.StartTimer(TimeSpan.FromMinutes(1), () =>
                {
                    _firebaseManager.GetRankingEntries(OnFirebaseRankingLoaded);

                    return DependencyService.Get<IGoogleManager>().IsLoggedIn;
                });

            }
            else
            {
                RankingEntries.Clear();
                RankingEntries.Add(new RankingEntry("Błąd", "0.", 0, false));

                DependencyService.Get<IDialogService>().ShowErrorAsync(errors, "Błąd", "Zamknij");
            }
        }

        public void OnFirebaseRankingLoaded(List<RankingEntry> entries)
        {
            RankingEntries.Clear();

            foreach (RankingEntry entry in entries)
            {
                RankingEntries.Add(entry);
            }

            IsRefreshing = false;

            int a = 5;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;
            a = 8;

        }

        private void RankingEntryTapped (RankingEntry entry)
        {
            if (!entry.IsCurrentUser)
                PopupNavigation.Instance.PushAsync(new RankingEntryDetailsPopup(entry, OnRankingEntryCompete,
                    StartViewModel.Instance.UIDToCompeteWith));
        }

        private async void OnRankingEntryCompete(RankingEntry entry)
        {
            StartViewModel.Instance.SetUIDToCompeteWith(entry?.UID);

            await Shell.Current.GoToAsync("//StartPage");
        }
        
       
    }
}
