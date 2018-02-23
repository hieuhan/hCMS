using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using CMSHelperLib;

namespace LibUtils
{
    public class LogFiles
    {
        public enum LogLevel
        {
            WARNING,
            ERROR,
            INFO
        }

        private static string _logExtension = ".log";
        private static readonly int UserId = SessionHelpers.UserId;

        public static void WriteLog(LogLevel logLevel, Exception ex, ExceptionContext filterContext)
        {
            //string logMessage = ex.ToString();
            string logDirectory = UtilConstants.LOG_FILE_PATH;
            DateTime currentDateTime = DateTime.Now;
            //string currentDateTimeString = currentDateTime.ToString();
            CheckCreateLogDirectory(logDirectory);
            string logLine = BuildLogLine(logLevel, currentDateTime, filterContext);
            logDirectory = $"{logDirectory}\\{logLevel}_{LogFileName(DateTime.Now)}{_logExtension}";// logDirectory + "\\Log_" + LogFileName(DateTime.Now) + _logExtension;

            //StreamWriter streamWriter = null;
            //try
            //{
            //    streamWriter = new StreamWriter(logDirectory, true);
            //    streamWriter.WriteLine(logLine);
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    if (streamWriter != null)
            //    {
            //        streamWriter.Close();
            //    }
            //}

            using (StreamWriter writer = File.AppendText(logDirectory))
            {
                writer.WriteLine(logLine);
                writer.Flush();
            }
        }

        private static bool CheckCreateLogDirectory(string logPath)
        {
            bool loggingDirectoryExists = false;
            DirectoryInfo directoryInfo = new DirectoryInfo(logPath);
            if (directoryInfo.Exists)
            {
                loggingDirectoryExists = true;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(logPath);
                    loggingDirectoryExists = true;
                }
                catch
                {
                }
            }
            return loggingDirectoryExists;
        }

        /// <summary>
        /// Builds the log line.
        /// </summary>
        /// <param name="currentDateTime">The current date time.</param>
        /// <param name="logLevel">The Log Level.</param>
        /// <param name="filterContext">The filter context.</param>       
        private static string BuildLogLine(LogLevel logLevel, DateTime currentDateTime, ExceptionContext filterContext)
        {
            string controllerName = filterContext.RouteData.Values["Controller"].ToString();
            string actionName = filterContext.RouteData.Values["Action"].ToString();

            RouteValueDictionary paramList = ((System.Web.Routing.Route)(filterContext.RouteData.Route)).Defaults;
            if (paramList != null)
            {
                paramList.Remove("Controller");
                paramList.Remove("Action");
            }

            StringBuilder loglineStringBuilder = new StringBuilder();

            loglineStringBuilder.Append("Log Time : ");
            loglineStringBuilder.Append(LogFileEntryDateTime(currentDateTime));
            loglineStringBuilder.Append(System.Environment.NewLine);

            loglineStringBuilder.Append("Log Level : ");
            loglineStringBuilder.Append(logLevel);
            loglineStringBuilder.Append(System.Environment.NewLine);

            if (UserId > 0)
            {
                loglineStringBuilder.Append("UserName : ");
                loglineStringBuilder.Append(SessionHelpers.UserName);
                loglineStringBuilder.Append(System.Environment.NewLine);
            }

            loglineStringBuilder.Append("ControllerName : ");
            loglineStringBuilder.Append(controllerName);
            loglineStringBuilder.Append(System.Environment.NewLine);

            loglineStringBuilder.Append("ActionName : ");
            loglineStringBuilder.Append(actionName);
            loglineStringBuilder.Append(System.Environment.NewLine);

            loglineStringBuilder.Append("----------------------------------------------------------------------------------------------------------");
            loglineStringBuilder.Append(System.Environment.NewLine);

            loglineStringBuilder.AppendFormat("Source:\t{0}", filterContext.Exception.Source);
            loglineStringBuilder.Append(System.Environment.NewLine);

            loglineStringBuilder.AppendFormat("Target:\t{0}", filterContext.Exception.TargetSite);
            loglineStringBuilder.Append(System.Environment.NewLine);

            loglineStringBuilder.AppendFormat("Type:\t{0}", filterContext.Exception.GetType().Name);
            loglineStringBuilder.Append(System.Environment.NewLine);

            loglineStringBuilder.AppendFormat("Message:\t{0}", filterContext.Exception.Message);
            loglineStringBuilder.Append(System.Environment.NewLine);
            if (filterContext.Exception.StackTrace != null)
            {
                loglineStringBuilder.AppendFormat("Stack:\t{0}", filterContext.Exception.StackTrace);
            }
            
            //loglineStringBuilder.Append(logMessage);
            loglineStringBuilder.Append(System.Environment.NewLine);
            loglineStringBuilder.Append("==========================================================================================================");

            return loglineStringBuilder.ToString();
        }

        /// <summary>
        /// Logs the file entry date time.
        /// </summary>
        /// <param name="currentDateTime">The current date time.</param>
        private static string LogFileEntryDateTime(DateTime currentDateTime)
        {
            return currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
        }

        /// <summary>
        /// Logs the name of the file.
        /// </summary>
        /// <param name="currentDateTime">The current date time.</param>
        private static string LogFileName(DateTime currentDateTime)
        {
            return currentDateTime.ToString("ddMMyyyyHHmmss");
        }

    }
}
