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
            if (steps.Count < 7 - NumberOfDays)
                return false;

            steps.RemoveRange(0, 7 - NumberOfDays);

            foreach (int s in steps)
            {
                if (s < NumberOfSteps)
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
