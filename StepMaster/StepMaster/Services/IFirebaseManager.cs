using StepMaster.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StepMaster.Services
{
    public interface IFirebaseManager
    {
        
        void Auth(GoogleUser googleUser, Action<bool, string> callback);

        void GetRankingEntries(Action<List<RankingEntry>> callback);

        void GetResultToCompeteWith(Action<RankingEntry> callback, string UID = null);

        void SaveStepsToRanking(int numberOfSteps, string username);
        
    }
}
