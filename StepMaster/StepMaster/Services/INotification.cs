using Android.App;
using System;
using System.Collections.Generic;
using System.Text;

namespace StepMaster.Services
{
    public interface INotification
    {
        Notification ReturnNotif(string title, string description);
    }
}
