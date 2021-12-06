using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using StepMaster.Extensions;

namespace StepMaster.Database
{
    public static class StepsDatabase
    {
        private static string dbName = "Steps.db3";
        private static Environment.SpecialFolder specialFolder = Environment.SpecialFolder.ApplicationData;
        private static string folderPath;
        private static string pathDB;
        private static SQLiteConnection db;

        public static void Init ()
        {
            folderPath = Environment.GetFolderPath(specialFolder);
            pathDB = Path.Combine(folderPath, dbName);
            db = new SQLiteConnection(pathDB);
            db.CreateTable<StepsModel>();
            db.CreateTable<AverageWeeklyStepsModel>();
        }

        public static void AddSteps(StepsModel stepsModel)
        {
            db.Insert(stepsModel);
        }

        public static void updateDailySteps(int steps)
        {
            int n = db.Update(new StepsModel()
            {
                Date = DateTime.Now.Date,
                NumberOfSteps = steps
            });
            
            if (n == 0)
            {
                AddSteps(new StepsModel()
                {
                    Date = DateTime.Now.Date,
                    NumberOfSteps = steps
                });
            }

            UpdateAverageStepsPerWeek();
        }

        public static IEnumerable<StepsModel> GetSteps()
        {
            return db.Table<StepsModel>().OrderBy(x => x.Date);
        }

        public static IEnumerable<StepsModel> GetSteps(DateTime dateFrom, DateTime dateTo)
        {
            return db.Table<StepsModel>().Where(x => x.Date >= dateFrom && x.Date <= dateTo); 
        }

        public static int GetSteps(DateTime date)
        {
            
            StepsModel stepsModel = db.Find<StepsModel>(x => x.Date == date);

            if (stepsModel != null)
                return stepsModel.NumberOfSteps;
            else
                return 0;
        }

        public static void RemoveSteps(int numberOfWeeksToLeave)
        {
            DateTime date = DateTime.Now.Date;

            date = date.AddDays(numberOfWeeksToLeave * -7);

            db.Table<StepsModel>().Delete(x => x.Date <= date);
          
        }

        private static void UpdateAverageStepsPerWeek()
        {
            DateTime startDate = DateTime.Now.Date.GetStartDateOfTheWeek();
            DateTime endDate = DateTime.Now.Date.GetEndDateOfTheWeek();

            List<StepsModel> steps = new List<StepsModel>();
            steps.AddRange(GetSteps(startDate, endDate));

            int sum = 0;

            foreach (var element in steps)
            {
                sum += element.NumberOfSteps;
            }

            sum /= 7;

            int n = db.Update(new AverageWeeklyStepsModel()
            {
                NumberOfSteps = sum,
                StartDate = startDate,
                EndDate = endDate
            });

            if (n == 0)
            {
                AddAverageStepsPerWeek(new AverageWeeklyStepsModel
                {
                    NumberOfSteps = sum,
                    StartDate = startDate,
                    EndDate = endDate
                });
            }

            
        }

        public static void DeleteAverageWeeklySteps(int weeksToLeave = 7)
        {
            DateTime date = DateTime.Now.Date;

            date = date.AddDays(weeksToLeave * -7);

            db.Table<AverageWeeklyStepsModel>().Delete(x => x.StartDate < date);
        }

        public static IEnumerable<AverageWeeklyStepsModel> GetAverageWeeklySteps(int numberOfWeeks = 7)
        {
            DateTime startDate = DateTime.Now.Date.AddDays(-7 * numberOfWeeks).GetStartDateOfTheWeek();

            return db.Table<AverageWeeklyStepsModel>().Where(x => x.StartDate >= startDate);
        }

        public static int GetCurrentAverageWeeklySteps()
        {
            DateTime startDate = DateTime.Now.Date.GetStartDateOfTheWeek();

            AverageWeeklyStepsModel steps = db.Find<AverageWeeklyStepsModel>(x => x.StartDate == startDate);

            if (steps != null)
                return steps.NumberOfSteps;
            else
                return 0;
        }

        private static void AddAverageStepsPerWeek(AverageWeeklyStepsModel averageWeeklySteps)
        {
            db.Insert(averageWeeklySteps);
            
        }

        public static void clearDB()
        {
            db.DropTable<StepsModel>();
            db.CreateTable<StepsModel>();
        }
    }
}
