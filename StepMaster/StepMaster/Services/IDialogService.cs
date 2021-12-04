using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StepMaster.Services
{
    public interface IDialogService
    {
        void ShowErrorAsync(string message, string title, string buttonText);
        void ShowErrorAsync(string message, string title, string buttonText, Action CallBackAferHide);
    }
}
