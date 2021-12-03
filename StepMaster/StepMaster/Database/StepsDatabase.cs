using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        }

        public static IEnumerable<StepsModel> GetSteps()
        {
            return db.Table<StepsModel>().OrderBy(x => x.Date);
        }

        public static IEnumerable<StepsModel> GetSteps(DateTime dateFrom, DateTime dateTo)
        {
            return db.Table<StepsModel>().Where(x => x.Date >= dateFrom && x.Date <= dateTo).OrderBy(x => x.Date);
        }

        public static int GetSteps(DateTime date)
        {
            
            StepsModel stepsModel = db.Find<StepsModel>(x => x.Date == date);

            if (stepsModel != null)
                return stepsModel.NumberOfSteps;
            else
                return 0;
        }

        public static void RemoveSteps(int numberOfWeeks)
        {
            DateTime date = DateTime.Now.Date;

            date = date.AddDays(numberOfWeeks * -7);

            db.Table<StepsModel>().Delete(x => x.Date <= date);
          
        }

        public static void clearDB()
        {
            db.DropTable<StepsModel>();
            db.CreateTable<StepsModel>();
        }
    }
}
