using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyRugby.Player
{
    /// <summary>
    /// Class to store the player details
    /// </summary>
    public class Player
    {
        public int SruNumber { get; set; }
        public string PlayerName { get; set; }
        public string Address { get; set; }
        public DateTime Dob { get; set; }
        public string ContactNo { get; set; }
        public string Postcode { get; set; }
        public string Email { get; set; }
        public string NextOfKin { get; set; }
        public string NextOfKinContactNo { get; set; }
        public string KnownHealthIssues { get; set; }

        public string TeamName { get; set; } // Add a property to store the team name

        public string GuardianName { get; set; }
        public string GuardianContactNo { get; set; }

        public string GuardianAddress { get; set; }
        public string GuardianRelationship { get; set; }

        public string CoachName { get; set; }

        public bool IsSelected { get; set; }

    }
}
