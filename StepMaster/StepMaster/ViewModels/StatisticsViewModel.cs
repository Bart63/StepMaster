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
using XF.Material.Forms.UI.Dialogs;

namespace StepMaster.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        private Chart _weeklyStepsChart;
        private Chart _previousWeekStepsChart;
        private Chart _averageStepsPerWeekChart;
        private float _averageStepsPerDay;
        private string _unitsChoiceButtonText;
        private string _infoTextPerDay;
        private string _infoTextChart1;
        private string _infoTextChart2;
        private string _infoTextChart3;
        private const float _stepsToKcalCoefficient = 0.05f;

        private enum UnitType
        {
            kroki, kcal
        }

        private UnitType _unitType = UnitType.kroki;

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

        public Command ShowUnitsOptionsCommand { get; set; }

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

        public float AverageStepsPerDay
        {
            get => _averageStepsPerDay;
            set => SetProperty(ref _averageStepsPerDay, value);
        }

        public string UnitsChoiceButtonText
        {
            get => _unitsChoiceButtonText;
            set => SetProperty(ref _unitsChoiceButtonText, value + " (zmień)");
        }

        public string InfoTextPerDay
        {
            get => _infoTextPerDay;
            set => SetProperty(ref _infoTextPerDay, value);
        }

        public string InfoTextChart1
        {
            get => _infoTextChart1;
            set => SetProperty(ref _infoTextChart1, value);
        }
        public string InfoTextChart2
        {
            get => _infoTextChart2;
            set => SetProperty(ref _infoTextChart2, value);
        }

        public string InfoTextChart3
        {
            get => _infoTextChart3;
            set => SetProperty(ref _infoTextChart3, value);
        }

        public StatisticsViewModel()
        {
            float dpi = DependencyService.Get<IDisplayInfo>().GetDisplayDpi();

            float pointSize = 60 * (dpi / 420);
            float lineSize = 5 * (dpi / 420);
            float textSize = 40 * (dpi / 420);

            pointSize = (pointSize > 60) ? 60 : pointSize;
            lineSize = (lineSize > 5) ? 5 : lineSize;
            textSize = (textSize > 40) ? 40 : textSize;

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


            UpdateCharts();

            ShowUnitsOptionsCommand = new Command(await => ShowUnitsOptions());
            UnitsChoiceButtonText = "Kroki";
            InfoTextPerDay = "Średnio kroki/dzień";
            InfoTextChart1 = "Twoje kroki w tym tygodniu";
            InfoTextChart2 = "Twoje kroki w poprzednim tygodniu";
            InfoTextChart3 = "Twoje kroki średnio tygodniowo";

            Device.StartTimer(TimeSpan.FromSeconds(30), () =>
            {
                UpdateCharts();

                return true;
            });
        }

        private void UpdateCharts()
        {
            UpdateCharts(DateTime.Now, ref _weeklyStepsChart);
            UpdateCharts(DateTime.Now.AddDays(-7), ref _previousWeekStepsChart);

            float n = StepsDatabase.GetCurrentAverageWeeklySteps();

            if (_unitType == UnitType.kcal)
            {
                n *= _stepsToKcalCoefficient;
                n = (float)Math.Round(n, 2);
            }

            AverageStepsPerDay = n;


            UpdateAverageWeeklyChart(ref _averageStepsPerWeekChart);
        }

        private async void ShowUnitsOptions()
        {
            var choices = new string[] { "Kroki", "kcal" };

            var result = await MaterialDialog.Instance.SelectActionAsync(title: "Wybierz jednostkę",
                                                                         actions: choices);

            if (result == 0)
            {
                if (_unitType != UnitType.kroki)
                {
                    UnitsChoiceButtonText = choices[0];
                    InfoTextPerDay = "Średnio kroki/dzień";

                    InfoTextChart1 = "Twoje kroki w tym tygodniu";
                    InfoTextChart2 = "Twoje kroki w poprzednim tygodniu";
                    InfoTextChart3 = "Twoje kroki średnio tygodniowo";

                    _unitType = UnitType.kroki;

                    UpdateCharts();
                }
            }
            if (result == 1)
            {
                if (_unitType != UnitType.kcal)
                {
                    UnitsChoiceButtonText = choices[1];
                    InfoTextPerDay = "Średnio kcal/dzień";

                    InfoTextChart1 = "Spalone kcal w tym tygodniu";
                    InfoTextChart2 = "Spalone kcal w poprzednim tygodniu";
                    InfoTextChart3 = "Spalone kcal średnio tygodniowo";

                    _unitType = UnitType.kcal;

                    UpdateCharts();
                }
            }
        }

        private void UpdateCharts(DateTime currentDate, ref Chart chart)
        {
            DateTime startDate = currentDate.GetStartDateOfTheWeek();
            DateTime endDate = currentDate.GetEndDateOfTheWeek();

            List<StepsModel> steps = new List<StepsModel>();
            steps.AddRange(StepsDatabase.GetSteps(startDate.Date, endDate.Date));


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
                float value;

                if (step != null)
                {
                    value = step.NumberOfSteps;

                    if (_unitType == UnitType.kcal)
                    {
                        value *= _stepsToKcalCoefficient;
                        value = (float)Math.Round(value, 2);
                    }
                }
                else
                    value = 0f;

                chartEntries.Add(new ChartEntry(value)
                {
                    Label = dates[i].Date.ToString("MM/dd/yyyy"),
                    ValueLabel = value.ToString().Replace(",", "."),
                    Color = (dates[i].Date.Date <= DateTime.Now.Date) ? (dates[i].Date.Date == DateTime.Now.Date) ? SKColor.Parse("#eb4034") : _chartColors[4] : _chartColors[6],

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
                float value;

                if (step != null)
                {
                    value = step.NumberOfSteps;

                    if (_unitType == UnitType.kcal)
                    {
                        value *= _stepsToKcalCoefficient;
                        value = (float)Math.Round(value, 2);
                    }
                }
                else
                    value = 0;

                chartEntries.Add(new ChartEntry(value)
                {
                    Label = startDates[i].Date.ToString("MM/dd") + "-" + endDates[i].Date.ToString("MM/dd"),
                    ValueLabel = value.ToString().Replace(",", "."),
                    Color = _chartColors[i]
                });
            }

            chart.Entries = chartEntries;
        }

    }
}