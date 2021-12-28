using System;
using System.Collections.Generic;
using System.Text;

namespace StepMaster.Models
{
    class MultidayStepsAchievement : AchievementsEntry
    {
        public int NumberOfSteps { get; set; }
        public int NumberOfDays { get; set; }
        public override bool Check(List<int> steps)
        {
            for (int i = 0; i < NumberOfDays; i++)
            {
                if (steps[steps.Count - 1 - i] < NumberOfSteps)
                    return false;
            }

            return true;
        }

        

        public MultidayStepsAchievement(string name, string description, bool isOwned, string iconName, string groupName, string typeName, string iD, int numberOfSteps, int numberOfDays)
            : base(name, description, isOwned, iconName, groupName, typeName, iD)
        {
            NumberOfSteps = numberOfSteps;
            NumberOfDays = numberOfDays;
        }
    }
}
