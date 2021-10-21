using StepMaster.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace StepMaster.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}