using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace StepMaster.Managers
{
    public static class PreferencesManager
    {
        
        public static int GetValueInt(string name)
        {
            return Preferences.Get(name, -1);
        }

        public static void SetValueInt(int value, string name)
        {
            Preferences.Set(name, value);
        }
    }
}
