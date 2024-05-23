using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyRugby.SkillEntry
{
    /// <summary>
    /// Class to represent a skill entry
    /// </summary>
    public class SkillEntry
    {
        public string SkillCategory { get; set; }
        public string Skill { get; set; }
        public int SkillLevel { get; set; }
        public string SkillComments { get; set; } = string.Empty;
    }
}
