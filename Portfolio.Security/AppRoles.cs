using System;
using System.Collections.Generic;
using System.Text;

namespace BoilerPlate.Security
{
   
    public static class AppRoles
    {
        public static string[] appRoles = new string[] { "Admin", "SuperAdmin", "Team", "User" };
        public static string TeamRole = appRoles[2];
        public static string userRole = appRoles[3];
        public static string adminRole = appRoles[0];

    }
}
