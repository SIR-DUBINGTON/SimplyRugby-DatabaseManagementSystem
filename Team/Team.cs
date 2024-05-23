using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyRugby.Team
{
    /// <summary>
    /// Class to represent a team
    /// </summary>
    public class Team
    {
        public string TeamName { get; set; }
        public string AgeGroup { get; set; }
        public string CoachName { get; set; }
        public int TeamID { get; set; } // Added to identify the team in the database
    }
}
