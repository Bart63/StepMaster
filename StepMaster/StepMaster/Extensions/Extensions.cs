using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StepMaster.Extensions
{
    public static class Extensions
    {
        public static T Find<T>(this ObservableCollection<T> collection, Func<T, bool> condition)
        {
            foreach(T element in collection)
            {
                if (condition(element))
                    return element;
            }

            return default(T);
        }

        public static int FindIndex<T>(this ObservableCollection<T> collection, Func<T, bool> condition)
        {
            int i = 0;
            foreach (T element in collection)
            {
                if (condition(element))
                    return i;

                i++;
            }

            return -1;
        }
    }
}
