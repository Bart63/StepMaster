using StepMaster.Services;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StepMaster.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        private int numberOfSteps;
        private bool startButtonEnabled;
        private bool stopButtonEnabled;

        public int NumberOfSteps
        {
            get => numberOfSteps;
            set => SetProperty(ref numberOfSteps, value);
        }

        public bool StartButtonEnabled
        {
            get => startButtonEnabled;
            set => SetProperty(ref startButtonEnabled, value);
        }

        public bool StopButtonEnabled
        {
            get => stopButtonEnabled;
            set => SetProperty(ref stopButtonEnabled, value);
        }


        public Command CountStepsCommand { get; }
        public Command StopCountingStepsCommand { get; }

        public StartViewModel()
        {
            Title = "Start";

            CountStepsCommand = new Command(StartCountingSteps);
            StopCountingStepsCommand = new Command(StopCountingSteps);

            StartButtonEnabled = true;
            StopButtonEnabled = false;
        }

        private void UpdateNumberOfSteps()
        {
            NumberOfSteps = DependencyService.Get<IStepCounter>().Steps;
        }

        private void StartCountingSteps()
        {
            DependencyService.Get<IStepCounter>().InitSensorService();
            DependencyService.Get<IStepCounter>().SetValueChangedCallbackAction(UpdateNumberOfSteps);

            StartButtonEnabled = false;
            StopButtonEnabled = true;
        }

        private void StopCountingSteps()
        {
            DependencyService.Get<IStepCounter>().StopSensorService();

            StartButtonEnabled = true;
            StopButtonEnabled = false;
        }

        
    }
}