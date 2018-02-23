using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibDatabase
{
    public class DbConstants
    {
        public static string CONNECTION_STRING = ConfigurationManager.AppSettings["CONNECTION_STRING"] ?? string.Empty;
        public static string ROLENAME_ADMIN = ConfigurationManager.AppSettings["ROLENAME_ADMIN"] ?? "";
    }
}
