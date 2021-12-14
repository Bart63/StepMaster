using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StepMaster.Database
{
    public static class AchievementsDatabase
    {
        private static string dbName = "Achievements.db3";
        private static Environment.SpecialFolder specialFolder = Environment.SpecialFolder.ApplicationData;
        private static string folderPath;
        private static string pathDB;
        private static SQLiteConnection db;

        public static void Init()
        {
            folderPath = Environment.GetFolderPath(specialFolder);
            pathDB = Path.Combine(folderPath, dbName);
            db = new SQLiteConnection(pathDB);
            db.CreateTable<AchievementModel>();

           
        }

        public static bool IsAchievementOwned(string ID)
        {
            AchievementModel achievementModel = db.Find<AchievementModel>(x => x.ID == ID);

            if (achievementModel != null)
                return achievementModel.IsOwned;
            else
            {
                db.Insert(new AchievementModel()
                {
                    ID = ID,
                    IsOwned = false
                });


                return false;
            }
        }

        public static void SetAchievementOwned(string ID)
        {
            db.Update(new AchievementModel()
            {
                ID = ID,
                IsOwned = true
            }); 
        }

        public static void clearDB()
        {
            db.DropTable<AchievementModel>();
            db.CreateTable<AchievementModel>();
        }
    }
}
