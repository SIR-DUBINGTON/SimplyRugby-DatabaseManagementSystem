using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyRugby.PlayerDevCentreAdmin.StaffMember
{
    /// <summary>
    /// Class to represent a staff member
    /// </summary>
    public class StaffMember
    {
        public int StaffID { get; set; }
        public string StaffName { get; set; }

        public string username { get; set; }

        public DateTime DOB { get; set; }

        public string Address { get; set; }

        public string Postcode { get; set; }

        public string ContactNo { get; set; }
        public string StaffRole { get; set; }
        public string Email { get; set; }
    }
}
