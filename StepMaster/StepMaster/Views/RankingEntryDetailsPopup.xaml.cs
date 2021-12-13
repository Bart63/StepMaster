using Rg.Plugins.Popup.Pages;
using StepMaster.Models;
using StepMaster.Services;
using StepMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepMaster.Views
{
    
    public partial class RankingEntryDetailsPopup : PopupPage
    {
        
        public RankingEntryDetailsPopup(RankingEntry entry, Action<RankingEntry> competeCallback, string UID)
        {

            InitializeComponent();
            BindingContext = new RankingEntryDetailsViewModel(entry, competeCallback, UID);
        }
    }
}