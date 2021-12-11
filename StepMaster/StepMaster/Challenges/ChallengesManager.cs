using StepMaster.Database;
using StepMaster.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace StepMaster.Challenges
{
    public static class ChallengesManager
    {
        private static string name = "ChallengesList.xml";
        private static string resourcePath;

        
        private static List<AchievementsEntry> _challenges;

        public static void Init()
        {
            var assembly = Assembly.GetExecutingAssembly();
            resourcePath = assembly.GetManifestResourceNames()
            .Single(str => str.EndsWith(name));

            
            _challenges = new List<AchievementsEntry>();

            string result;

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            AddAchievements("stepsChallenges", "dailyChallenges", ref result);
            AddAchievements("stepsChallenges", "periodicChallenges", ref result);
            AddAchievements("competitionChallenges", "dailyChallenges", ref result);
            
        }

        public static List<AchievementsEntry> GetAchievementEntries()
        {
            return _challenges;
        }

        private static void AddAchievements(string groupName, string typeName, ref string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            var nodes = xmlDoc.SelectNodes("challenges/" + groupName + "/" + typeName + "/challenge");

            foreach (XmlNode childrenNode in nodes)
            {
                int steps = int.Parse((childrenNode.SelectSingleNode("numberOfSteps") != null) ?
                    childrenNode.SelectSingleNode("numberOfSteps").InnerText.ToString() : "0");

                int days = int.Parse((childrenNode.SelectSingleNode("numberOfDays") != null) ?
                    childrenNode.SelectSingleNode("numberOfDays").InnerText.ToString() : "0");

                int competitions = int.Parse((childrenNode.SelectSingleNode("numberOfCompetitions") != null) ?
                    childrenNode.SelectSingleNode("numberOfCompetitions").InnerText.ToString() : "0");

                _challenges.Add(new AchievementsEntry(childrenNode.SelectSingleNode("name").InnerText.ToString(),
                    childrenNode.SelectSingleNode("description").InnerText.ToString(),
                    AchievementsDatabase.IsAchievementOwned(childrenNode.SelectSingleNode("@id").Value.ToString()),
                    childrenNode.SelectSingleNode("icon").InnerText.ToString(), groupName, typeName,
                    childrenNode.SelectSingleNode("@id").Value.ToString(), steps, days, competitions));
            }
        }
    }
}
