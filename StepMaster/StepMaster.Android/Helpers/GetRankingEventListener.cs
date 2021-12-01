using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using StepMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepMaster.Droid.Helpers
{
    public class GetRankingEventListener : Java.Lang.Object, IEventListener
    {

        private Action<List<RankingEntry>> callback;
        private string currentUserUID;

        public GetRankingEventListener(Action<List<RankingEntry>> callback, string userUID)
        {
            this.callback = callback;
            currentUserUID = userUID;
        }

        public void OnEvent(Java.Lang.Object obj, FirebaseFirestoreException error)
        {
            var snapshot = (QuerySnapshot)obj;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                List<RankingEntry> entries = new List<RankingEntry>();

                int i = 1;

                foreach (DocumentSnapshot item in documents)
                {
                    DateTime date = DateTime.ParseExact(item.Get("Date").ToString(), "yyyyMMddHHmmssffff",
                        System.Globalization.CultureInfo.InvariantCulture);
                    
                    if (date.Date == DateTime.Now.Date)
                    {
                        RankingEntry entry = new RankingEntry(item.Get("Username").ToString(), i.ToString() + ".", (int)item.Get("StepsNumber"),
                            item.Id == currentUserUID);

                        entries.Add(entry);

                        i++;
                    }
                }

                callback(entries);
                return;
            }

        }
    }
}