using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyRugby.PlayerStats
{
    /// <summary>
    /// Class to hold the player stats
    /// </summary>
    public class PlayerStats
    {
        public int SruNumber { get; set; }
        public string PlayerName { get; set; }
        public string SkillCategory { get; set; }
        public string Skill { get; set; }
        public int SkillLevel { get; set; }
        public string SkillComments { get; set; }
        public string Position { get; set; }
    }
}
