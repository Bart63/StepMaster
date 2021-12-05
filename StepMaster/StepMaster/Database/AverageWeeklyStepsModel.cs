using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace StepMaster.Database
{
    [Table("AverageWeeklySteps")]
    public class AverageWeeklyStepsModel
    {
        public int NumberOfSteps { get; set; }

        [PrimaryKey, Unique]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
