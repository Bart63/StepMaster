using StepMaster.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microcharts;
using SkiaSharp;
using StepMaster.Services;
using StepMaster.Extensions;
using StepMaster.Database;
using System.Collections.Generic;

namespace StepMaster.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        private Chart _weeklyStepsChart;
        private Chart _previousWeekStepsChart;

        private readonly SKColor[] _chartColors = new SKColor[]
        {
            SKColor.Parse("#d9ed92"),
            SKColor.Parse("#b5e48c"),
            SKColor.Parse("#99d98c"),
            SKColor.Parse("#52b69a"),
            SKColor.Parse("#34a0a4"),
            SKColor.Parse("#1a759f"),
            SKColor.Parse("#3875B3"),
        };

        public Chart WeeklyStepsChart
        {
            get => _weeklyStepsChart;
            set => SetProperty(ref _weeklyStepsChart, value);
        }

        public Chart PreviousWeekStepsChart
        {
            get => _previousWeekStepsChart;
            set => SetProperty(ref _previousWeekStepsChart, value);
        }


        public StatisticsViewModel()
        {

            WeeklyStepsChart = new PointChart
            {
                
                PointSize = 60,
                PointMode = PointMode.Circle,
                LabelOrientation = Orientation.Vertical,
                LabelTextSize = 40,
                ValueLabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Empty
                
            };

            PreviousWeekStepsChart = new PointChart
            {

                PointSize = 60,
                PointMode = PointMode.Circle,
                LabelOrientation = Orientation.Vertical,
                LabelTextSize = 40,
                ValueLabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Empty

            };

            UpdateCharts(DateTime.Now, ref _weeklyStepsChart);
            UpdateCharts(DateTime.Now.AddDays(-7), ref _previousWeekStepsChart);

        }

        private void UpdateCharts(DateTime currentDate, ref Chart chart)
        {
            DateTime startDate = currentDate.GetStartDateOfTheWeek();
            DateTime endDate = currentDate.GetEndDateOfTheWeek();

            List<StepsModel> steps = new List<StepsModel>();
            steps.AddRange(StepsDatabase.GetSteps(startDate, endDate));

            List<ChartEntry> chartEntries = new List<ChartEntry>();

            
            DateTime d = startDate;

            List<DateTime> dates = new List<DateTime>();

            for (int i = 0; i < 7; i++)
            {
                dates.Add(d);
                d = d.AddDays(1);
            }

            for (int i = 0; i < 7; i++)
            {
                StepsModel step = steps.Find(x => x.Date.Date == dates[i].Date);
                int value;

                if (step != null)
                    value = step.NumberOfSteps;
                else
                    value = 0;

                chartEntries.Add(new ChartEntry(value)
                {
                    Label = dates[i].Date.ToString("MM/dd/yyyy"),
                    ValueLabel = value.ToString(),
                    Color = (dates[i].Date.Date <= DateTime.Now.Date) ? _chartColors[4] : _chartColors[6],
                });
            }
            

            chart.Entries = chartEntries.ToArray();
            
        }

    }
}