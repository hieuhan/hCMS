using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSLib;
using hCMS.Library;
using hCMS.Models;
using hCMS.Models.DataSources;

namespace hCMS.Controllers
{
    [CmsAuthorize]
    public class DataSourcesController : Controller
    {
        // GET: DataSources
        public ActionResult Index(byte dataTypeId = 0, int page = 1)
        {
            int rowCount = 0;
            var model = new DataSourcesModel
            {
                ListDataSources = new DataSources().GetPage(string.Empty,string.Empty,string.Empty,CmsConstants.RowAmount20,page>0?page-1:page,ref rowCount),
                RowCount = rowCount,
                Pagination = new PaginationModel
                {
                    TotalPage = rowCount,
                    PageSize = CmsConstants.RowAmount20,
                    LinkLimit = 5,
                    PageIndex = page
                }
            };
            return View(model);
        }
    }
}