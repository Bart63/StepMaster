using StepMaster.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using StepMaster.Challenges;
using Xamarin.Forms;

namespace StepMaster.ViewModels
{
    public class AchievementsViewModel : BaseViewModel
    {

        public ObservableCollection<AchievementsEntry> AchievementsEntries { get; set; }

        public AchievementsViewModel()
        {

            AchievementsEntries = new ObservableCollection<AchievementsEntry>(ChallengesManager.GetAchievementEntries());

            ChallengesManager.SetCallbackAchievementViewModel(UpdateView);
        }

        private void UpdateView(List<AchievementsEntry> entries)
        {
            AchievementsEntries.Clear();

            foreach (var e in entries)
            {
                AchievementsEntries.Add(e);
            }

            
            
        }
    }
}
