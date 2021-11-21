using StepMaster.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StepMaster.ViewModels
{
    public class AchievementsViewModel : BaseViewModel
    {

        public ObservableCollection<AchievementsEntry> AchievementsEntries { get; private set; }

        public AchievementsViewModel()
        {

            AchievementsEntries = new ObservableCollection<AchievementsEntry>()
            {
                new AchievementsEntry("Pierwsze kroki", "Pokonaj pierwsze 5000 kroków.", true, "achievement_icon_3"),
                new AchievementsEntry("Systematyczny", "Przez 5 dni pod rząd pokonaj min. 3000 kroków.", false, "achievement_icon_1"),
                new AchievementsEntry("Zapaleniec", "W ciągu dnia pokonaj co najmniej 25000 kroków.", false, "achievement_icon_2"),
                new AchievementsEntry("Mini zapaleniec", "W ciągu dnia pokonaj co najmniej 15000 kroków.", true, "achievement_icon_6"),
                new AchievementsEntry("Niepokonany", "Przez 5 dni pod rząd wygrywaj rywalizację ze znajomym.", false, "achievement_icon_5"),
                new AchievementsEntry("Gotowy do starcia", "Pokonaj znajomego w rywalizacji.", false, "achievement_icon_4")
            };

        }
    }
}
