﻿using Android.App;
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

                entries.Sort(delegate (RankingEntry x1, RankingEntry x2) {
                if (x1.Steps < x2.Steps) return 1;
                    if (x1.Steps > x2.Steps) return -1;
                    else
                        return 0;
               
                });

                i = 1;
                foreach (var item in entries)
                {
                    item.PositionNumber = i + ".";
                    i++;
                }

                callback(entries);
                return;
            }

        }
    }
}