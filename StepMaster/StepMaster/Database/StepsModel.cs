using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace StepMaster.Database
{
    [Table("Steps")]
    public class StepsModel
    {
        public int NumberOfSteps { get; set; }

        [PrimaryKey, Unique]
        public DateTime Date { get; set; }
    }
}
