
using StepMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepMaster.Views
{
    public partial class CompetePage : ContentPage
    {
        public CompetePage()
        {
            InitializeComponent();
            BindingContext = new CompeteViewModel();
        }
    }
}