using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace hCMS.Library
{
    public static class CmsConstants
    {
        public static string ROOT_PATH = ConfigurationManager.AppSettings["ROOT_PATH"] ?? string.Empty;

        public static readonly byte SystemMessageIdError = byte.Parse(ConfigurationManager.AppSettings["SystemMessageIdError"] ?? "1");
        public static readonly byte SystemMessageIdSuccess = byte.Parse(ConfigurationManager.AppSettings["SystemMessageIdSuccess"] ?? "2");

        public static int RowAmount20 = int.Parse(ConfigurationManager.AppSettings["RowAmount20"] ?? "20");

        public static string CONNECTION_STRING = ConfigurationManager.AppSettings["CONNECTION_STRING"] ?? ConfigurationManager.AppSettings["CONNECTION_STRING"];
    }
}