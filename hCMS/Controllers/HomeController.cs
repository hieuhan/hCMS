using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSHelperLib;
using CMSLib;
using hCMS.Library;
using LibUtils;

namespace hCMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            int userId = SessionHelpers.UserId;
            short defaultActionId = SessionHelpers.DefaultAction;
            string redirect = "/Users/Login";
            if (userId > 0)
            {
                var action = new Actions().Get(defaultActionId);
                if (action.ActionId > 0 && !string.IsNullOrEmpty(action.Url))
                {
                    redirect = action.Url.StartsWith(CmsConstants.ROOT_PATH)
                        ? action.Url
                        : string.Concat(CmsConstants.ROOT_PATH, action.Url);
                }
                else redirect = "Users/ChangePassword";
            }
            return Redirect(redirect);
        }
    }
}