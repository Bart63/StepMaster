using StepMaster.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace StepMaster.ViewModels
{
    public class CompeteViewModel : BaseViewModel
    {

        public ObservableCollection<RankingEntry> RankingEntries { get; private set; }

        public Command UpdateRankingCommand { get; }

        
        public CompeteViewModel()
        {
            UpdateRankingCommand = new Command(async () => await UpdateRanking());

            RankingEntries = new ObservableCollection<RankingEntry>()
            {
                new RankingEntry("domek15", "1.", 16125),
                new RankingEntry("klekot789", "2.", 15478, true),
                new RankingEntry("graczFortnite", "3.", 12482),
                new RankingEntry("gracz36", "4.", 11482),
                new RankingEntry("zielony", "5.", 10256),
                new RankingEntry("bla", "6.", 7896),
                new RankingEntry("skbd", "7.", 156),
                new RankingEntry("abcd", "8.", 56),
                new RankingEntry("qwertY", "9.", 20)
            };
            
        }

        async Task UpdateRanking()
        {
            await Task.Delay(5000);
            //throw new NotImplementedException();
        }

    }
}
