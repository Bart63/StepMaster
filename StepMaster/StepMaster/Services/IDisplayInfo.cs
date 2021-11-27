using System;
using System.Collections.Generic;
using System.Text;

namespace StepMaster.Services
{
    public interface IDisplayInfo
    {
        int GetDisplayWidth();
        int GetDisplayHeight();
        int GetDisplayDpi();
    }
}
