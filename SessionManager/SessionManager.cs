using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyRugby.SessionManager
{
    /// <summary>
    /// Class to manage the session of the user
    /// </summary>
    public static class SessionManager
    {
        public static string Username { get;  set; }
        public static string UserRole { get; set; }
        public static int StaffID { get; set; }

        public static void ClearSession()
        {
            Username = null;
            UserRole = null;
            StaffID = 0;
        }

    }

}
