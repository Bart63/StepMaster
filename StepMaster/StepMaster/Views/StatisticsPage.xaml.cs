
using StepMaster.ViewModels;
using StepMaster.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepMaster.Views
{
    public partial class StatisticsPage : ContentPage
    {
        StatisticsViewModel _viewModel;

        public StatisticsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new StatisticsViewModel();
        }

        
    }
}