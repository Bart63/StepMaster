using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace StepMaster.Database
{
    [Table("Achievements")]
    public class AchievementModel
    {
        [PrimaryKey, Unique]
        public string ID { get; set; }

        public bool IsOwned { get; set; }
    }
}
