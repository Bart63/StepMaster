using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StepMaster.Models
{
    public class AchievementsEntry
    {
        public AchievementsEntry(string name, string description, bool isOwned, string iconName,
            string groupName, string typeName, string iD, int numberOfSteps = 0, int numberOfDays = 0, int numberOfCompetitions = 0)
        {
            Name = name;
            Description = description;
            this.isOwned = isOwned;
            Opacity = isOwned ? 1.0f : 0.5f;
            IconName = iconName;
            GroupName = groupName;
            TypeName = typeName;
            ID = iD;
            NumberOfSteps = numberOfSteps;
            NumberOfDays = numberOfDays;
            NumberOfCompetitions = numberOfCompetitions;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool isOwned { get; set; }
        public float Opacity { get; set; }
        public string IconName { get; set; }
        
        public string GroupName { get; set; }
        public string TypeName { get; set; }

        public string ID { get; set; }

        public int NumberOfSteps { get; set; }

        public int NumberOfDays { get; set; }

        public int NumberOfCompetitions { get; set; }
    }
}
