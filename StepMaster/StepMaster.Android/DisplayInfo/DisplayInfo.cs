
using StepMaster.Droid.DisplayInfo;
using Xamarin.Forms;
using StepMaster.Services;

[assembly: Dependency(typeof(DisplayInfo))]
namespace StepMaster.Droid.DisplayInfo
{
    public class DisplayInfo : IDisplayInfo
    {
        public int GetDisplayWidth()
        {
            return (int)Android.App.Application.Context.Resources.DisplayMetrics.WidthPixels;
        }

        public int GetDisplayHeight()
        {
            return (int)Android.App.Application.Context.Resources.DisplayMetrics.HeightPixels;
        }

        public int GetDisplayDpi()
        {
            return (int)Android.App.Application.Context.Resources.DisplayMetrics.DensityDpi;
        }
        
    }
}