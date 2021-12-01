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

using Firebase.Auth;
using StepMaster.Models;
using Java.Util;
using Firebase.Firestore;
using StepMaster.Droid.Helpers;

[assembly: Dependency(typeof(FirebaseManager))]
namespace StepMaster.Droid.Managers
{
    class FirebaseManager : IFirebaseManager
    {
        private FirebaseFirestore _database;

        private string _token;
        private string _userUID;

        private FirebaseApp _firebaseApp;

        public FirebaseManager()
        {
            _firebaseApp = FirebaseApp.InitializeApp(Application.Context);

            if (_firebaseApp == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetApplicationId("stepmaster-333414")
                    .SetApiKey("AIzaSyA8lbTADDAoSH4o2UFzXNNB5tkcXwt5sNs")
                    .SetDatabaseUrl("https://stepmaster-333414-default-rtdb.europe-west1.firebasedatabase.app")
                    .SetStorageBucket("stepmaster-333414.appspot.com")
                    .SetProjectId("stepmaster-333414")
                    .Build();

                _firebaseApp = FirebaseApp.InitializeApp(Application.Context, options);

                _database = FirebaseFirestore.GetInstance(_firebaseApp);
            }

        }
        

        public async void Auth(GoogleUser googleUser, Action<bool> callback)
        {
            var credential = GoogleAuthProvider.GetCredential(googleUser.IDToken, googleUser.AccessToken);

            var user = await FirebaseAuth.Instance.SignInWithCredentialAsync(credential);

            if (user.User != null)
            {
                _token = (string)user.User.GetIdToken(true);
                _userUID = FirebaseAuth.Instance.CurrentUser.Uid;

                callback(true);
            }
            else
            {
                callback(false);
            }

        }

        public void GetRankingEntries(Action<List<RankingEntry>> callback)
        {
            Query allRankingEntries = _database.Collection("StepsRanking");

            allRankingEntries.OrderBy("StepsNumber", Query.Direction.Descending);

            allRankingEntries.AddSnapshotListener(new GetRankingEventListener(callback, _userUID));
        }


        public void SaveStepsToRanking(int numberOfSteps, string username)
        {
            
            DocumentReference databaseReference = _database.Collection("StepsRanking").Document(_userUID);

            HashMap entryInfo = new HashMap();
            entryInfo.Put("StepsNumber", numberOfSteps);
            entryInfo.Put("Date", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
            entryInfo.Put("Username", username);

            databaseReference.Set(entryInfo);
            

        }
    }
}