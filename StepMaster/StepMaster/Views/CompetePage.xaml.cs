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
        public CompetePage()
        {
            InitializeComponent();
            BindingContext = new CompeteViewModel(DependencyService.Get<IGoogleManager>(), DependencyService.Get<IFirebaseManager>());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            RankingEntry entry = CompeteViewModel.Instance.RankingEntries.Find(x => x.IsCurrentUser);

            if (entry != null)
                RankingListView.ScrollTo(entry, ScrollToPosition.Center);
        }

    }
}