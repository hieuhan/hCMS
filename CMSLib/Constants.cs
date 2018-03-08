using System;
using System.Configuration;
using System.Web;

namespace CMSLib
{
    public class Constants
    {
        public static string CULTURE_VN = "vi-VN";
        public static string ROOT_PATH = ConfigurationManager.AppSettings["ROOT_PATH"] ?? string.Empty;
        public static string NO_IMAGE_URL = (ConfigurationManager.AppSettings["NO_IMAGE_URL"] == null) ? "" : ConfigurationManager.AppSettings["NO_IMAGE_URL"];
        public static string CONTACT_PRICE = (ConfigurationManager.AppSettings["CONTACT_PRICE"] == null) ? "Liên hệ" : ConfigurationManager.AppSettings["CONTACT_PRICE"];
        public static string CURRENCY = (ConfigurationManager.AppSettings["CURRENCY"] == null) ? " VNĐ" : ConfigurationManager.AppSettings["CURRENCY"];
        public static string CURRENCY_VND = (ConfigurationManager.AppSettings["CURRENCY_VND"] == null) ? " VNĐ" : ConfigurationManager.AppSettings["CURRENCY_VND"];
        public static string CURRENCY_USD = (ConfigurationManager.AppSettings["CURRENCY_USD"] == null) ? " USD" : ConfigurationManager.AppSettings["CURRENCY_USD"];

        public static string WEBSITE_DOMAIN = (ConfigurationManager.AppSettings["WEBSITE_DOMAIN"] == null) ? "" : ConfigurationManager.AppSettings["WEBSITE_DOMAIN"];
        public static string WEBSITE_IMAGEDOMAIN = (ConfigurationManager.AppSettings["WEBSITE_IMAGEDOMAIN"] == null) ? "" : ConfigurationManager.AppSettings["WEBSITE_IMAGEDOMAIN"];
        public static string WEBSITE_MEDIADOMAIN = (ConfigurationManager.AppSettings["WEBSITE_MEDIADOMAIN"] == null) ? "" : ConfigurationManager.AppSettings["WEBSITE_MEDIADOMAIN"];
        public static string LOG_FILE_PATH = (ConfigurationManager.AppSettings["LOG_FILE_PATH"] == null) ? (HttpContext.Current.Request.PhysicalApplicationPath + "\\Logs") : ConfigurationManager.AppSettings["LOG_FILE_PATH"].ToString().Trim();

        public static string MEDIA_PATH = (ConfigurationManager.AppSettings["MEDIA_PATH"] == null) ? "" : ConfigurationManager.AppSettings["MEDIA_PATH"];
        public static string MEDIA_ORIGINAL_PATH = (ConfigurationManager.AppSettings["MEDIA_ORIGINAL_PATH"] == null) ? "" : ConfigurationManager.AppSettings["MEDIA_ORIGINAL_PATH"];
        public static string MEDIA_THUMNAIL_PATH = (ConfigurationManager.AppSettings["MEDIA_THUMNAIL_PATH"] == null) ? "" : ConfigurationManager.AppSettings["MEDIA_THUMNAIL_PATH"];
        public static string MEDIA_ICON_PATH = (ConfigurationManager.AppSettings["MEDIA_ICON_PATH"] == null) ? "" : ConfigurationManager.AppSettings["MEDIA_ICON_PATH"];
        public static int MEDIA_WIDTH = (ConfigurationManager.AppSettings["MEDIA_WIDTH"] == null) ? 0 : Int32.Parse(ConfigurationManager.AppSettings["MEDIA_WIDTH"].ToString());
        public static int MEDIA_HEIGHT = (ConfigurationManager.AppSettings["MEDIA_HEIGHT"] == null) ? 0 : Int32.Parse(ConfigurationManager.AppSettings["MEDIA_HEIGHT"].ToString());
        public static int MEDIA_THUMNAIL_WIDTH = (ConfigurationManager.AppSettings["MEDIA_THUMNAIL_WIDTH"] == null) ? 0 : Int32.Parse(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_WIDTH"].ToString());
        public static int MEDIA_THUMNAIL_HEIGHT = (ConfigurationManager.AppSettings["MEDIA_THUMNAIL_HEIGHT"] == null) ? 0 : Int32.Parse(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_HEIGHT"].ToString());
        public static int MEDIA_ICON_WIDTH = (ConfigurationManager.AppSettings["MEDIA_ICON_WIDTH"] == null) ? 0 : Int32.Parse(ConfigurationManager.AppSettings["MEDIA_ICON_WIDTH"].ToString());
        public static int MEDIA_ICON_HEIGHT = (ConfigurationManager.AppSettings["MEDIA_ICON_HEIGHT"] == null) ? 0 : Int32.Parse(ConfigurationManager.AppSettings["MEDIA_ICON_HEIGHT"].ToString());
        public static string CATEGORIES_ICON_PATH = (ConfigurationManager.AppSettings["CATEGORIES_ICON_PATH"] == null) ? "" : ConfigurationManager.AppSettings["CATEGORIES_ICON_PATH"];
        public static short ARTICLES_ADMIN_ROWSAMOUNT = (ConfigurationManager.AppSettings["ARTICLES_ADMIN_ROWSAMOUNT"] == null) ? (short)0 : Int16.Parse(ConfigurationManager.AppSettings["ARTICLES_ADMIN_ROWSAMOUNT"].ToString());
        public static string FLV_PATH = (ConfigurationManager.AppSettings["FLV_PATH"] == null) ? "" : ConfigurationManager.AppSettings["FLV_PATH"];

    }
}
