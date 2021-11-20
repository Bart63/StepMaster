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

        public StepsChartInfo(string infoText, int value, Color color, string name)
        {
            InfoText = infoText;
            Value = value;
            Color = color;
            Name = name;
        }
    }
}
