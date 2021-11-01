using Android.Content;
using Android.Hardware;
using Android.Runtime;
using StepMaster.Droid.StepDetection;
using StepMaster.Services;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(StepCounter))]
namespace StepMaster.Droid.StepDetection
{
    public class StepCounter : Java.Lang.Object, IStepCounter, ISensorEventListener
    {
        private int StepsCounter = 0;
        private SensorManager sManager;

        private Action ValueChangedCallback;


        public int Steps
        {
            get { return StepsCounter; }
            set { StepsCounter = value; }
        }

        public new void Dispose()
        {
            sManager.UnregisterListener(this);
            sManager.Dispose();
        }

        public void InitSensorService()
        {

            sManager = Android.App.Application.Context.GetSystemService(Context.SensorService) as SensorManager;
            sManager.RegisterListener(this, sManager.GetDefaultSensor(SensorType.StepCounter), SensorDelay.Normal);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            
        }

        public void OnSensorChanged(SensorEvent e)
        {
            Steps = (int)e.Values[0];

            ValueChangedCallback();
        }

        public void SetValueChangedCallbackAction(Action action)
        {
            ValueChangedCallback = action;
        }

        public void StopSensorService()
        {
            sManager.UnregisterListener(this);
        }

    }
}