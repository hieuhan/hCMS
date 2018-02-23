using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSHelperLib;
using CMSLib;
using hCMS.Library;
using hCMS.Models;
using hCMS.Models.Shared;

namespace hCMS.Controllers
{
    public class SharedController : Controller
    {
        private readonly int _userId = SessionHelpers.UserId;
        // GET: Shared
        [ChildActionOnly]
        public ActionResult PartialHeader()
        {
            var model = new HeaderModel
            {
                ListActionLevel1 = new Actions().GetActionsByUser(_userId),
                ListActionForUser = new Actions().GetNotRootForUser(_userId)
            };
            return PartialView(model);
        }

        public ActionResult PartialPagination(PaginationModel model)
        {
            return PartialView(model);
        }
    }
}