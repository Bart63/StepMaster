using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StepMaster.Models
{
    public class AchievementsEntry
    {
        public AchievementsEntry(string name, string description, bool isOwned, string iconName)
        {
            Name = name;
            Description = description;
            this.isOwned = isOwned;
            Opacity = isOwned ? 1.0f : 0.5f;
            IconName = iconName;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool isOwned { get; set; }
        public float Opacity { get; set; }
        public string IconName { get; set; }
        

    }
}
