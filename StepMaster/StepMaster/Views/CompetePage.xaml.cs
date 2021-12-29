using StepMaster.Services;
using StepMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StepMaster.Extensions;
using StepMaster.Models;

namespace StepMaster.Views
{
    public partial class CompetePage : ContentPage
    {
        public static CompetePage Instance;
        public CompetePage()
        {
            Instance = this;
            InitializeComponent();
            BindingContext = new CompeteViewModel(DependencyService.Get<IGoogleManager>(), DependencyService.Get<IFirebaseManager>());
        }

        public void ScrollToCurrentUser(RankingEntry entry)
        {
            if (entry != null)
                RankingListView.ScrollTo(entry, null, ScrollToPosition.Start, true);
        }
    }
}