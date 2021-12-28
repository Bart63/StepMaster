using Rg.Plugins.Popup.Services;
using StepMaster.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StepMaster.ViewModels
{
    public class RankingEntryDetailsViewModel : BaseViewModel
    {
        private RankingEntry _entry;
        private Action<RankingEntry> _competeCallback;
        private string _currentUID;
        private bool _isCompetingWithSelectedUser;

        public RankingEntry Entry
        {
            get => _entry;
            set => SetProperty(ref _entry, value);
        }

        public string CompeteButtonText
        {
            get
            {
                return !_isCompetingWithSelectedUser ? "Rywalizuj" : "Zakończ rywalizację";
            }
        }

        public Command CompeteCommand { get; set; }


        public RankingEntryDetailsViewModel()
        {
            
        }

        public RankingEntryDetailsViewModel(RankingEntry entry, Action<RankingEntry> competeCallback, string currentUID)
        {
            CompeteCommand = new Command(Compete);
            Entry = entry;
            _competeCallback = competeCallback;
            _currentUID = currentUID;

            _isCompetingWithSelectedUser = _currentUID == entry.UID;
        }

        private void Compete()
        {
            if (!_isCompetingWithSelectedUser)
            {
                PopupNavigation.Instance.PopAllAsync(true);
                _competeCallback(Entry);
            }
            else
            {
                PopupNavigation.Instance.PopAllAsync(true);
                _competeCallback(null);
            }
        }
    }
}
