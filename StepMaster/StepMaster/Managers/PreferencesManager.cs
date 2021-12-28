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

        public static string GetValueString(string name)
        {
            return Preferences.Get(name, null);
        }

        public static void SetValueString(string value, string name)
        {
            Preferences.Set(name, value);
        }
        public static bool GetValueBool(string name)
        {
            return Preferences.Get(name, true);
        }

        public static void SetValueBool(bool value, string name)
        {
            Preferences.Set(name, value);
        }

        public static void ClearAll()
        {
            Preferences.Clear();
        }
    }
}
