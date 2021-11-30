using StepMaster.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StepMaster.Services
{
    public interface IFirebaseManager
    {
        void Auth(GoogleUser googleUser, Action<bool> callback);

        List<RankingEntry> GetRankingEntries();

        void SaveStepsToRanking(int numberOfSteps);
        
    }
}
