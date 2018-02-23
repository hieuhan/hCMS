using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LibUtils
{
    public class UtilConstants
    {
        public static string LOG_FILE_PATH = ConfigurationManager.AppSettings["LOG_FILE_PATH"] ?? string.Concat(HttpContext.Current.Request.PhysicalApplicationPath , "\\Logs");
    }
}
