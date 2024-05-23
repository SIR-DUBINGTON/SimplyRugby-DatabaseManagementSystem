using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyRugby.MatchAndTraining.TrainingRecord
{
    /// <summary>
    /// Class to represent a training record
    /// </summary>
    public class TrainingRecord
    {
        public int TrainingID { get; set; }
        public int CoachStaffID { get; set; }
        public DateTime TrainingDate { get; set; }
        public string TrainingLocation { get; set; }
        public TimeSpan TrainingTime { get; set; }
        public string SkillsActivities { get; set; }
        public string AccidentsInjuries { get; set; }

        public int TeamID { get; set; }

        public string DisplayInfo
        {
            get
            {
                return $"ID: {TrainingID} - {TrainingDate:yyyy-MM-dd} at {TrainingTime:hh\\:mm}";
            }
        }
    }
}
