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

        public List<RankingEntry> RankingEntries { get; private set; }

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
        

        public async void Auth(GoogleUser googleUser, Action<bool, string> callback)
        {
            try
            {
                var credential = GoogleAuthProvider.GetCredential(googleUser.IDToken, googleUser.AccessToken);

                var user = await FirebaseAuth.Instance.SignInWithCredentialAsync(credential);

                if (user.User != null)
                {
                    _token = (string)user.User.GetIdToken(true);
                    _userUID = FirebaseAuth.Instance.CurrentUser.Uid;

                    GetRankingEntries();

                    callback(true, null);

                }
                else
                {
                    callback(false, "Unable to authenticate user");
                }
            }
            catch (FirebaseNetworkException e)
            {
                callback(false, e.LocalizedMessage);
            }
            

        }

        public void GetRankingEntries(Action<List<RankingEntry>> callback)
        {
            Query allRankingEntries = _database.Collection("StepsRanking");

            allRankingEntries.AddSnapshotListener(new GetRankingEventListener(callback, SetRankingEntries, _userUID));
        }

        public void GetRankingEntries()
        {
            Query allRankingEntries = _database.Collection("StepsRanking");

            allRankingEntries.AddSnapshotListener(new GetRankingEventListener(null, SetRankingEntries, _userUID));
        }

        private void SetRankingEntries(List<RankingEntry> rankingEntries)
        {
            RankingEntries = rankingEntries;
        }

        public void GetResultToCompeteWith(Action<RankingEntry> callback, string UID = null)
        {
            if (UID != null)
            {
                int index = RankingEntries.FindIndex(x => x.UID == UID);

                if (index > 0)
                    callback(RankingEntries[index]);
            }
            else
            {
                if (RankingEntries != null)
                {
                    if (RankingEntries.Count > 1)
                    {
                        RankingEntries.Sort(delegate (RankingEntry x1, RankingEntry x2)
                        {
                            if (x1.Steps < x2.Steps) return 1;
                            if (x1.Steps > x2.Steps) return -1;
                            else
                                return 0;

                        });

                        int index = RankingEntries.FindIndex(x => x.IsCurrentUser);

                        if (index > 0)
                        {
                            if (RankingEntries[index].Steps < RankingEntries[index - 1].Steps)
                                callback(RankingEntries[index - 1]);
                            else
                                callback(null);
                        }
                        else
                            callback(null);
                    }
                }
            }
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