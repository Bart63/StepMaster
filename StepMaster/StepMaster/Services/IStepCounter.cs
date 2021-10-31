using System;

namespace StepMaster.Services
{
    public interface IStepCounter
    {
        int Steps { get; set; }
        void InitSensorService();
        void StopSensorService();
        void SetValueChangedCallbackAction(Action action);
    }
}
