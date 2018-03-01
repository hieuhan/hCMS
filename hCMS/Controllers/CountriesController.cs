using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hCMS.Controllers
{
    public class CountriesController : Controller
    {
        // GET: Countries
        public ActionResult Index(string keyword="",int page = 1)
        {
            return View();
        }
    }
}