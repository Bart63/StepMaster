using StepMaster.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using StepMaster.Challenges;

namespace StepMaster.ViewModels
{
    public class AchievementsViewModel : BaseViewModel
    {

        public ObservableCollection<AchievementsEntry> AchievementsEntries { get; private set; }

        public AchievementsViewModel()
        {

            AchievementsEntries = new ObservableCollection<AchievementsEntry>(ChallengesManager.GetAchievementEntries());

            
        }
    }
}
