using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StepMaster.Models
{
    public abstract class AchievementsEntry
    {
        public AchievementsEntry(string name, string description, bool isOwned, string iconName,
            string groupName, string typeName, string iD)
        {
            Name = name;
            Description = description;
            this.isOwned = isOwned;
            IconName = iconName;
            GroupName = groupName;
            TypeName = typeName;
            ID = iD;
            
        }

        private bool _isOwned;
        public string Name { get; set; }
        public string Description { get; set; }
        public bool isOwned { get => _isOwned;
            set 
            {
                _isOwned = value;
                Opacity = isOwned ? 1.0f : 0.5f;
            } }
        public float Opacity { get; set; }
        public string IconName { get; set; }
        
        public string GroupName { get; set; }
        public string TypeName { get; set; }

        public string ID { get; set; }

        public virtual bool Check(int steps)
        {
            return false;
        }

        public virtual bool Check(List<int> steps)
        {
            return false;
        }
    }
}
