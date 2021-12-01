using StepMaster.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StepMaster.Services
{
    public interface IFirebaseManager
    {
        void Auth(GoogleUser googleUser, Action<bool> callback);

        void GetRankingEntries(Action<List<RankingEntry>> callback);

        void GetResultToCompeteWith(Action<RankingEntry> callback);

        void SaveStepsToRanking(int numberOfSteps, string username);
        
    }
}
