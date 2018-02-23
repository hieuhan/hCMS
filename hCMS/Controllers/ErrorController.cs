using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hCMS.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // GET: Error
        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }
    }
}