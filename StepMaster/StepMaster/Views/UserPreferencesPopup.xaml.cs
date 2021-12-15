using Rg.Plugins.Popup.Pages;
using StepMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepMaster.Views
{
    
    public partial class UserPreferencesPopup : PopupPage
    {
        public UserPreferencesPopup(Action callback)
        {
            InitializeComponent();
            BindingContext = new UserPreferencesViewModel(callback);
        }
    }
}