using System;
using System.Collections.Generic;
using System.Text;
using StepMaster.Database;
using Xunit;
using StepMaster.Extensions;
using StepMaster.Challenges;
using StepMaster.Managers;
using StepMaster.Models;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace StepMaster.Tests
{
    public class UnitTests
    {
        public UnitTests()
        {
            StepsDatabase.Init();
            AchievementsDatabase.Init();

            StepsDatabase.clearDB();
            AchievementsDatabase.clearDB();
        }

        [Fact]
        public void TestStepsDatabase()
        {
            Assert.Equal(0, StepsDatabase.GetCurrentAverageWeeklySteps());

            StepsDatabase.UpdateDailySteps(14);
            Assert.True(StepsDatabase.GetSteps(DateTime.Now.Date) == 14);

            DateTime startDate = DateTime.Now.GetStartDateOfTheWeek();
            
            StepsDatabase.UpdateDailySteps(0, startDate);
            StepsDatabase.UpdateDailySteps(10, startDate.AddDays(1));
            StepsDatabase.UpdateDailySteps(0, startDate.AddDays(2));
            StepsDatabase.UpdateDailySteps(15, startDate.AddDays(3));
            StepsDatabase.UpdateDailySteps(0, startDate.AddDays(4));
            StepsDatabase.UpdateDailySteps(0, startDate.AddDays(5));
            StepsDatabase.UpdateDailySteps(785, startDate.AddDays(6));

            Assert.Equal(115, StepsDatabase.GetCurrentAverageWeeklySteps());

            List<StepsModel> steps = new List<StepsModel>();
            steps.AddRange(StepsDatabase.GetSteps(startDate, startDate.AddDays(6)));
            Assert.True(steps[1].NumberOfSteps == 10);

            List<AverageWeeklyStepsModel> averages = new List<AverageWeeklyStepsModel>();
            averages.AddRange(StepsDatabase.GetAverageWeeklySteps(0));

            Assert.Single(averages);

            Assert.Equal(115, averages[0].NumberOfSteps);

            startDate = DateTime.Now.GetStartDateOfTheWeek();
            startDate = startDate.AddDays(-7);

            StepsDatabase.UpdateDailySteps(0, startDate);
            StepsDatabase.UpdateDailySteps(10, startDate.AddDays(1));
            StepsDatabase.UpdateDailySteps(0, startDate.AddDays(2));
            StepsDatabase.UpdateDailySteps(15, startDate.AddDays(3));
            StepsDatabase.UpdateDailySteps(0, startDate.AddDays(4));
            StepsDatabase.UpdateDailySteps(0, startDate.AddDays(5));
            StepsDatabase.UpdateDailySteps(100, startDate.AddDays(6));

            averages.Clear();
            averages.AddRange(StepsDatabase.GetAverageWeeklySteps(2));

            Assert.Equal(2, averages.Count);

            Assert.Equal(17, averages[0].NumberOfSteps);
        }

        [Fact]
        public void TestAchievementsDatabase()
        {
            AchievementsDatabase.clearDB();

            Assert.False(AchievementsDatabase.IsAchievementOwned("sd1"));

            AchievementsDatabase.SetAchievementOwned("sd1");

            Assert.True(AchievementsDatabase.IsAchievementOwned("sd1"));
        }

        [Fact]
        public void TestChallengesManager()
        {

            StepsDatabase.clearDB();
            AchievementsDatabase.clearDB();

            ChallengesManager.Init();

            Assert.Equal(7, ChallengesManager.GetAchievementEntries().Count);

            Assert.Equal("Dobre nawyki", ChallengesManager.GetAchievementEntries()[5].Name);

            DateTime startDate = DateTime.Now.GetStartDateOfTheWeek();

            StepsDatabase.UpdateDailySteps(105, startDate);
            StepsDatabase.UpdateDailySteps(10, startDate.AddDays(1));
            StepsDatabase.UpdateDailySteps(0, startDate.AddDays(2));
            StepsDatabase.UpdateDailySteps(15, startDate.AddDays(3));
            StepsDatabase.UpdateDailySteps(0, startDate.AddDays(4));
            StepsDatabase.UpdateDailySteps(0, startDate.AddDays(5));
            StepsDatabase.UpdateDailySteps(785, startDate.AddDays(6));

            StepsDatabase.UpdateDailySteps(104);

            Assert.True(ChallengesManager.Check(ChallengesManager.AchievementType.dailySteps));

            Assert.False(ChallengesManager.Check(ChallengesManager.AchievementType.multidaySteps));

            startDate = DateTime.Now.Date;

            StepsDatabase.UpdateDailySteps(1005, startDate);
            StepsDatabase.UpdateDailySteps(1000, startDate.AddDays(-1));
            StepsDatabase.UpdateDailySteps(5236, startDate.AddDays(-2));
            StepsDatabase.UpdateDailySteps(1286, startDate.AddDays(-3));
            StepsDatabase.UpdateDailySteps(15236, startDate.AddDays(-4));

            Assert.True(ChallengesManager.Check(ChallengesManager.AchievementType.multidaySteps));

            StepsDatabase.UpdateDailySteps(1005, startDate);
            StepsDatabase.UpdateDailySteps(1000, startDate.AddDays(-1));
            StepsDatabase.UpdateDailySteps(0, startDate.AddDays(-2));
            StepsDatabase.UpdateDailySteps(1286, startDate.AddDays(-3));
            StepsDatabase.UpdateDailySteps(15236, startDate.AddDays(-4));

            Assert.False(ChallengesManager.Check(ChallengesManager.AchievementType.multidaySteps));
        }

        [Fact]
        public void TestModels()
        {
            RankingEntry entry = new RankingEntry("test", "5.", 452, false, null);
            RankingEntry entry2 = new RankingEntry("test", "4.", 425, true, null);

            Assert.Equal(452, entry.Steps);
            Assert.Equal("test", entry2.Username);

            StepsChartInfo chartInfo = new StepsChartInfo("test", 45, Color.Green, "name");

            Assert.Equal("test", chartInfo.InfoText);
            Assert.Equal("name", chartInfo.Name);

            GoogleUser googleUser = new GoogleUser(){ Name = "test"};

            Assert.Equal("test", googleUser.Name);
        }

        [Fact]
        public void TestExtensions()
        {
            DateTime dateTime = DateTime.Now.GetEndDateOfTheWeek();

            Assert.Equal(DayOfWeek.Sunday, dateTime.Date.DayOfWeek);

            ObservableCollection<RankingEntry> entries = new ObservableCollection<RankingEntry>();

            entries.Add(new RankingEntry("test3", "3.", 452));
            entries.Add(new RankingEntry("test1", "1.", 1000));
            entries.Add(new RankingEntry("test2", "2.", 458));

            int index = entries.FindIndex(x => x.Steps == 1000);

            Assert.Equal(1, index);

            index = entries.FindIndex(x => x.Steps == 4586);

            Assert.Equal(-1, index);

            RankingEntry entry = entries.Find(x => x.Username == "test2");

            Assert.Equal(458, entry.Steps);

            entry = entries.Find(x => x.Username == "test562");

            Assert.Null(entry);

            entries.Sort(delegate (RankingEntry x1, RankingEntry x2)
            {
                if (x1.Steps < x2.Steps) return 1;
                if (x1.Steps > x2.Steps) return -1;
                else
                    return 0;

            });

            Assert.Equal(1000, entries[0].Steps);
        }

    }
}
