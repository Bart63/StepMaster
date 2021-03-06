
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
        

        public StatisticsPage()
        {
            InitializeComponent();

            Task.Run(AnimateBackground);
        }

        private async void AnimateBackground()
        {
            Action<double> forward = input => bdGradient.AnchorY = input;
            Action<double> backward = input => bdGradient.AnchorY = input;

            uint durationTime = 5000;

            while (true)
            {
                bdGradient.Animate(name: "forward", callback: forward, start: 0, end: 1, length: durationTime, easing: Easing.SinInOut);
                await Task.Delay(5000);
                bdGradient.Animate(name: "backward", callback: backward, start: 1, end: 0, length: durationTime, easing: Easing.SinInOut);
                await Task.Delay(5000);
            }
        }


    }
}