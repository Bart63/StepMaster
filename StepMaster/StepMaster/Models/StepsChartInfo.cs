using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StepMaster.Models
{
    public class StepsChartInfo
    {
        public string InfoText { get; set; }
        public int Value { get; set; }
        public Color Color { get; set; }
        public string Name { get; set; }
        public string UID { get; set; }

        public Command<StepsChartInfo> ClickCommand { get; set; }

        public StepsChartInfo(string infoText, int value, Color color, string name, string UID = null, Action<StepsChartInfo> clickAction = null)
        {
            InfoText = infoText;
            Value = value;
            Color = color;
            Name = name;
            this.UID = UID;

            if (clickAction != null)
                ClickCommand = new Command<StepsChartInfo>(clickAction);
            
        }
    }
}
