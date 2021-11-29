using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StepMaster.Services;
using StepMaster.Droid.Managers;
using Xamarin.Forms;
using Firebase;


using Application = Android.App.Application;
using Firebase.Database;
using Firebase.Auth;
using StepMaster.Models;

[assembly: Dependency(typeof(FirebaseManager))]
namespace StepMaster.Droid.Managers
{
    class FirebaseManager : IFirebaseManager
    {
        private FirebaseDatabase _database;

        private string _token;

        private FirebaseApp _firebaseApp;

        public FirebaseManager()
        {
            _firebaseApp = FirebaseApp.InitializeApp(Application.Context);

            if (_firebaseApp == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetApplicationId("stepmaster-333414")
                    .SetApiKey("AIzaSyA8lbTADDAoSH4o2UFzXNNB5tkcXwt5sNs")
                    .SetDatabaseUrl("https://stepmaster-333414-default-rtdb.europe-west1.firebasedatabase.app/")
                    .SetStorageBucket("stepmaster-333414.appspot.com")
                    .Build();

                _firebaseApp = FirebaseApp.InitializeApp(Application.Context, options);

                _database = FirebaseDatabase.GetInstance(_firebaseApp);
            }

        }
        

        public async void Auth(GoogleUser googleUser, Action<bool> callback)
        {
            var credential = GoogleAuthProvider.GetCredential(googleUser.IDToken, googleUser.AccessToken);

            var user = await FirebaseAuth.Instance.SignInWithCredentialAsync(credential);

            if (user.User != null)
            {
                _token = (string)user.User.GetIdToken(true);

                callback(true);
            }
            else
            {
                callback(false);
            }

        }
    }
}