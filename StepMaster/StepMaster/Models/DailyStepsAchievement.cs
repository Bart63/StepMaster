using System;
using System.Collections.Generic;
using System.Text;

namespace StepMaster.Models
{
    public class DailyStepsAchievement : AchievementsEntry
    {
        public int NumberOfSteps { get; set; }

        public DailyStepsAchievement(string name, string description, bool isOwned, string iconName, string groupName, string typeName, string iD, int numberOfSteps)
            : base(name, description, isOwned, iconName, groupName, typeName, iD)
        {
            NumberOfSteps = numberOfSteps;
        }

        public override bool Check(int steps)
        {
            return steps >= NumberOfSteps;
        }
    }
}
