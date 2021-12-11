using StepMaster.Database;
using StepMaster.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using StepMaster.Extensions;

namespace StepMaster.Challenges
{
    public static class ChallengesManager
    {
        private static string name = "ChallengesList.xml";
        private static string resourcePath;
        private static Action<List<AchievementsEntry>> _callbackAchievementViewModel;
        
        private static List<DailyStepsAchievement> _dailyStepsChallenges;
        private static List<MultidayStepsAchievement> _multidayStepsChallenges;

        public enum AchievementType
        {
            dailySteps,
            multidaySteps
        }

        public static void Init()
        {
            var assembly = Assembly.GetExecutingAssembly();
            resourcePath = assembly.GetManifestResourceNames()
            .Single(str => str.EndsWith(name));

            
            _dailyStepsChallenges = new List<DailyStepsAchievement>();
            _multidayStepsChallenges = new List<MultidayStepsAchievement>();

            string result;

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            AddDailyStepsAchievements("stepsChallenges", "dailyChallenges", ref result);
            AddMultidayStepsAchievements("stepsChallenges", "multidayChallenges", ref result);
            
        }

        public static void SetCallbackAchievementViewModel(Action<List<AchievementsEntry>> callback)
        {
            _callbackAchievementViewModel = callback;
        }

        public static List<AchievementsEntry> GetAchievementEntries()
        {
            List<AchievementsEntry> achievements = new List<AchievementsEntry>();

            achievements.AddRange(_dailyStepsChallenges);
            achievements.AddRange(_multidayStepsChallenges);

            return achievements;
        }

        private static void AddDailyStepsAchievements(string groupName, string typeName, ref string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            var nodes = xmlDoc.SelectNodes("challenges/" + groupName + "/" + typeName + "/challenge");

            foreach (XmlNode childrenNode in nodes)
            {
                int steps = int.Parse((childrenNode.SelectSingleNode("numberOfSteps") != null) ?
                    childrenNode.SelectSingleNode("numberOfSteps").InnerText.ToString() : "0");


                _dailyStepsChallenges.Add(new DailyStepsAchievement(childrenNode.SelectSingleNode("name").InnerText.ToString(),
                    childrenNode.SelectSingleNode("description").InnerText.ToString(),
                    AchievementsDatabase.IsAchievementOwned(childrenNode.SelectSingleNode("@id").Value.ToString()),
                    childrenNode.SelectSingleNode("icon").InnerText.ToString(), groupName, typeName,
                    childrenNode.SelectSingleNode("@id").Value.ToString(), steps));
            }
        }

        
        private static void AddMultidayStepsAchievements(string groupName, string typeName, ref string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            var nodes = xmlDoc.SelectNodes("challenges/" + groupName + "/" + typeName + "/challenge");

            foreach (XmlNode childrenNode in nodes)
            {
                int days = int.Parse((childrenNode.SelectSingleNode("numberOfDays") != null) ?
                    childrenNode.SelectSingleNode("numberOfDays").InnerText.ToString() : "0");

                int steps = int.Parse((childrenNode.SelectSingleNode("numberOfSteps") != null) ?
                    childrenNode.SelectSingleNode("numberOfSteps").InnerText.ToString() : "0");


                _multidayStepsChallenges.Add(new MultidayStepsAchievement(childrenNode.SelectSingleNode("name").InnerText.ToString(),
                    childrenNode.SelectSingleNode("description").InnerText.ToString(),
                    AchievementsDatabase.IsAchievementOwned(childrenNode.SelectSingleNode("@id").Value.ToString()),
                    childrenNode.SelectSingleNode("icon").InnerText.ToString(), groupName, typeName,
                    childrenNode.SelectSingleNode("@id").Value.ToString(), steps, days));
            }
        }

        public static void Check(AchievementType achievementType)
        {
            switch(achievementType)
            {
                case AchievementType.dailySteps:

                    int steps = StepsDatabase.GetSteps(DateTime.Now.Date);

                    foreach (var challenge in _dailyStepsChallenges)
                    {
                        if (challenge.Check(steps))
                        {
                            AchievementsDatabase.SetAchievementOwned(challenge.ID);

                            if (_callbackAchievementViewModel != null)
                                _callbackAchievementViewModel(GetAchievementEntries());

                            //TODO: some nice popup window with gratulations
                        }
                    }

                    break;

                case AchievementType.multidaySteps:

                    List<int> weeklySteps = new List<int>();

                    foreach (var element in StepsDatabase.GetSteps(DateTime.Now.GetStartDateOfTheWeek().Date, DateTime.Now.GetEndDateOfTheWeek().Date))
                    {
                        weeklySteps.Add(element.NumberOfSteps);
                    }

                    foreach (var challenge in _multidayStepsChallenges)
                    {
                        if (challenge.Check(weeklySteps))
                        {
                            AchievementsDatabase.SetAchievementOwned(challenge.ID);

                            if (_callbackAchievementViewModel != null)
                                _callbackAchievementViewModel(GetAchievementEntries());

                            //TODO: some nice popup window with gratulations
                        }
                    }

                    break;
            }
        }
    }
}
