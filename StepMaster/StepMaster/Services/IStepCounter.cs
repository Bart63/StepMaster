using System;

namespace StepMaster.Services
{
    public interface IStepCounter
    {
        
        void InitSensorService();
        void StopSensorService();
        void SetValueChangedCallbackAction(Action action);
    }
}
