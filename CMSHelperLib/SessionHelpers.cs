using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMSHelperLib
{
    public class SessionHelpers
    {
        private static string _sessionUserId = "hCMSUserId";
        private static string _sessionUserName = "hCMSUserName";
        private static string _sessionRoles = "hCMSRoles";
        private static string _sessionDefaultAction = "hCMSDefaultAction";
        public static int UserId
        {
            get
            {
                int userId = 0;
                if (HttpContext.Current == null)
                    return 0;
                var sessionUserId = HttpContext.Current.Session[_sessionUserId];
                if (sessionUserId != null)
                {
                    Int32.TryParse(sessionUserId.ToString(), out userId);
                }
                return userId;
            }
            set => HttpContext.Current.Session[_sessionUserId] = value;
        }

        public static string UserName
        {
            get
            {
                if (HttpContext.Current == null)
                    return string.Empty;
                var sessionUserName = HttpContext.Current.Session[_sessionUserName];
                if (sessionUserName != null) return sessionUserName.ToString();
                return null;
            }
            set => HttpContext.Current.Session[_sessionUserName] = value;
        }

        public static string Roles
        {
            get
            {
                if (HttpContext.Current == null)
                    return string.Empty;
                var sessionRoles = HttpContext.Current.Session[_sessionRoles];
                if (sessionRoles != null) return sessionRoles.ToString();
                return null;
            }
            set => HttpContext.Current.Session[_sessionRoles] = value;
        }

        public static short DefaultAction
        {
            get
            {
                if (HttpContext.Current == null)
                    return 0;
                short defaultAction = 0;
                var sessionDefaultAction = HttpContext.Current.Session[_sessionDefaultAction];
                if (sessionDefaultAction != null)
                {
                    short.TryParse(sessionDefaultAction.ToString(),out defaultAction);
                }
                return defaultAction;
            }
            set => HttpContext.Current.Session[_sessionDefaultAction] = value;
        }

    }
}
