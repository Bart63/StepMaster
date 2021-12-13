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
    
    public partial class NotificationSettingsPopup : PopupPage
    {
        public NotificationSettingsPopup()
        {
            InitializeComponent();
            BindingContext = new NotificationOptionsViewModel();
        }
    }
}