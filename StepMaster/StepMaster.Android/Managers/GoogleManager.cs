using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Plus;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using StepMaster.Droid.Managers;
using StepMaster.Models;
using StepMaster.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Auth;
using Android.Gms.Auth;

[assembly: Dependency(typeof(GoogleManager))]
namespace StepMaster.Droid.Managers
{
    public class GoogleManager : Java.Lang.Object, IGoogleManager, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        public Action<GoogleUser, string> _onLoginComplete;
        public static GoogleApiClient _googleApiClient { get; set; }
        public static GoogleManager Instance { get; private set; }

        private bool _isLoggedIn;
        public bool IsLoggedIn { get => _isLoggedIn; set => _isLoggedIn = value; }

        private GoogleUser _currentUser;
        public GoogleUser User { get => _currentUser ; set => _currentUser = value; }

        public GoogleManager()
        {
            Instance = this;
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                                                             .RequestEmail()
                                                             .RequestIdToken("695961368940-0s5v5ejrdr1pfrq2kdmqrvjsfh2s1o2d.apps.googleusercontent.com")
                                                             .Build();

            _googleApiClient = new GoogleApiClient.Builder(((MainActivity)Forms.Context).ApplicationContext)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                .AddScope(new Scope(Scopes.Profile))
                .Build();
        }

        public void Login(Action<GoogleUser, string> onLoginComplete)
        {
            _onLoginComplete = onLoginComplete;
            Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(_googleApiClient);
            ((MainActivity)Forms.Context).StartActivityForResult(signInIntent, 1);
            _googleApiClient.Connect();
        }

        public void Logout()
        {
           
            _googleApiClient.ClearDefaultAccountAndReconnect();
        }

        public void OnAuthCompleted(GoogleSignInResult result)
        {
            if (result.IsSuccess)
            {

                IsLoggedIn = true;

                GoogleSignInAccount account = result.SignInAccount;


                Task.Factory.StartNew(() => {
                    
                    var accessToken = GoogleAuthUtil.GetToken(Android.App.Application.Context, result.SignInAccount.Account,
                        $"oauth2:{Scopes.Email} {Scopes.Profile}");


                    CreateNewGoogleUser(account.IdToken, accessToken, account);
                });

                
            }
            else
            {
                IsLoggedIn = false;

                _onLoginComplete?.Invoke(null, "An error occured!");

            }
        }

        private void CreateNewGoogleUser(string Idtoken, string accessToken, GoogleSignInAccount account)
        {
            
            User = new GoogleUser()
            {
                Name = account.DisplayName,
                Email = account.Email,
                Picture = new Uri((account.PhotoUrl != null ? $"{account.PhotoUrl}" : $"https://autisticdating.net/imgs/profile-placeholder.jpg")),
                IDToken = Idtoken,
                AccessToken = accessToken
            };

            _onLoginComplete?.Invoke(User, string.Empty);
        }

        public void OnConnected(Bundle connectionHint)
        {

        }

        public void OnConnectionSuspended(int cause)
        {
            _onLoginComplete?.Invoke(null, "Canceled!");
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            _onLoginComplete?.Invoke(null, result.ErrorMessage);
        }
    }
}