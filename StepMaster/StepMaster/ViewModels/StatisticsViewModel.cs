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
        private Chart _averageStepsPerWeekChart;
        private int _averageStepsPerDay;

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

        public Chart AverageStepsPerWeekChart
        {
            get => _averageStepsPerWeekChart;
            set => SetProperty(ref _averageStepsPerWeekChart, value);
        }

        public int AverageStepsPerDay
        {
            get => _averageStepsPerDay;
            set => SetProperty(ref _averageStepsPerDay, value);
        }

        public StatisticsViewModel()
        {
            int dpi = DependencyService.Get<IDisplayInfo>().GetDisplayDpi();

            float pointSize = 60 * (dpi / 420);
            float lineSize = 5 * (dpi / 420);
            float textSize = 40 * (dpi / 420);

            WeeklyStepsChart = new LineChart
            {
                
                PointSize = pointSize,
                PointMode = PointMode.Circle,
                LabelOrientation = Orientation.Vertical,
                LabelTextSize = textSize,
                ValueLabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Empty,
                LineSize = lineSize
                
            };

            PreviousWeekStepsChart = new LineChart
            {

                PointSize = pointSize,
                PointMode = PointMode.Circle,
                LabelOrientation = Orientation.Vertical,
                LabelTextSize = textSize,
                ValueLabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Empty,
                LineSize = lineSize

            };

            AverageStepsPerWeekChart = new LineChart
            {

                PointSize = pointSize,
                PointMode = PointMode.Circle,
                LabelOrientation = Orientation.Vertical,
                LabelTextSize = textSize,
                ValueLabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Empty,
                LineSize = lineSize
          
            };


            UpdateCharts(DateTime.Now, ref _weeklyStepsChart);
            UpdateCharts(DateTime.Now.AddDays(-7), ref _previousWeekStepsChart);

            AverageStepsPerDay = StepsDatabase.GetCurrentAverageWeeklySteps();
            UpdateAverageWeeklyChart(ref _averageStepsPerWeekChart);


            Device.StartTimer(TimeSpan.FromSeconds(30), () =>
            {
                UpdateCharts(DateTime.Now, ref _weeklyStepsChart);
                UpdateCharts(DateTime.Now.AddDays(-7), ref _previousWeekStepsChart);

                AverageStepsPerDay = StepsDatabase.GetCurrentAverageWeeklySteps();

                UpdateAverageWeeklyChart(ref _averageStepsPerWeekChart);

                return true;
            });
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

            StepsDatabase.DeleteAverageWeeklySteps();
        }

        private void UpdateAverageWeeklyChart(ref Chart chart, int numberOfWeeks = 7)
        {
            List<AverageWeeklyStepsModel> averages = new List<AverageWeeklyStepsModel>();

            averages.AddRange(StepsDatabase.GetAverageWeeklySteps(numberOfWeeks));

            DateTime startDate = DateTime.Now.Date.GetStartDateOfTheWeek();
            DateTime endDate = DateTime.Now.Date.GetEndDateOfTheWeek();

            List<DateTime> startDates = new List<DateTime>();
            List<DateTime> endDates = new List<DateTime>();

            

            List<ChartEntry> chartEntries = new List<ChartEntry>();

            for (int i = 0; i < numberOfWeeks; i++)
            {
                startDates.Insert(0, startDate);
                endDates.Insert(0, endDate);
                startDate = startDate.AddDays(-7);
                endDate = endDate.AddDays(-7);
            }

            Random r = new Random();

            for (int i = 0; i < numberOfWeeks; i++)
            {
                AverageWeeklyStepsModel step = averages.Find(x => x.StartDate.Date == startDates[i].Date);
                int value;

                if (step != null)
                    value = step.NumberOfSteps;
                else
                    value = 0;

                chartEntries.Add(new ChartEntry(value)
                {
                    Label = startDates[i].Date.ToString("MM/dd") + "-" + endDates[i].Date.ToString("MM/dd"),
                    ValueLabel = value.ToString(),
                    Color = _chartColors[i]
                });
            }

            chart.Entries = chartEntries;
        }

    }
}