using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSHelperLib;
using CMSLib;
using hCMS.Library;
using hCMS.Models;
using hCMS.Models.DataSources;

namespace hCMS.Controllers
{
    [CmsAuthorize]
    public class DataSourcesController : Controller
    {
        private readonly int _userId = SessionHelpers.UserId;
        // GET: DataSources
        public ActionResult Index(string dataSourceName = "",string dateFrom = "", string dateTo = "", string orderBy = "", byte dataTypeId = 0, int page = 1)
        {
            int rowCount = 0;
            var model = new DataSourcesModel
            {
                ListDataSources = new DataSources{DataSourceName = dataSourceName, DataTypeId = dataTypeId }.GetPage(dateFrom, dateTo, orderBy, CmsConstants.RowAmount20,page>0?page-1:page,ref rowCount),
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

        [HttpGet]
        public ActionResult Edit(short dataSourceId = 0)
        {
            var model = new DataSourcesEditModel();
            if (dataSourceId > 0)
            {
                var dataSource = new DataSources { DataSourceId = dataSourceId }.Get(dataSourceId);
                if (dataSource.DataSourceId > 0)
                {
                    model.DataSourceId = dataSource.DataSourceId;
                    model.DataTypeId = dataSource.DataTypeId;
                    model.DataSourceName = dataSource.DataSourceName;
                    model.DataSourceDesc = dataSource.DataSourceDesc;
                    model.DisplayOrder = dataSource.DisplayOrder;
                    model.CrUserId = dataSource.CrUserId;
                    model.CrDateTime = dataSource.CrDateTime;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DataSourcesEditModel model)
        {
            if (ModelState.IsValid)
            {
                short sysMessageId = 0;
                byte sysMessageTypeId = 0;
                model.SystemStatus = SystemStatus.Error;
                var dataSource = new DataSources()
                {
                    DataSourceId = model.DataSourceId,
                    DataSourceName = model.DataSourceName,
                    DataSourceDesc = model.DataSourceDesc,
                    DataTypeId = model.DataTypeId,
                    DisplayOrder = model.DisplayOrder,
                    CrUserId = model.CrUserId,
                    CrDateTime = model.CrDateTime
                };
                sysMessageTypeId = model.DataSourceId > 0 ? dataSource.Update(0, _userId, ref sysMessageId) : dataSource.Insert(0, _userId, ref sysMessageId);

                if (sysMessageId > 0)
                {
                    var sysMessage = new SystemMessages().Get(sysMessageId);
                    if (sysMessageTypeId == CmsConstants.SystemMessageIdSuccess)
                    {
                        model.SystemStatus = SystemStatus.Success;
                    }
                    ModelState.AddModelError("SystemMessages", sysMessage.SystemMessageDesc);
                }
                else ModelState.AddModelError("SystemMessages", "Bạn vui lòng thử lại sau.");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(short dataSourceId = 0)
        {
            if (dataSourceId > 0)
            {
                short sysMessageId = 0;
                var dataSource = new DataSources { DataSourceId = dataSourceId }.Get();
                if (dataSource.DataSourceId > 0)
                {
                    new DataSources { DataSourceId = dataSourceId }.Delete(0, 0, ref sysMessageId);
                }
            }
            return Redirect("/DataSources/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MultipleAction(DataSourceMultipleActionModel model)
        {
            if (model.Delete != null)
            {
                if (model.DataSourceIds != null && model.DataSourceIds.Length > 0)
                {
                    short sysMessageId = 0;
                    foreach (var dataSourceId in model.DataSourceIds)
                    {
                        new DataSources { DataSourceId = dataSourceId }.Delete(0, 0, ref sysMessageId);
                    }
                }
            }
            return Redirect("/DataSources/Index");
        }
    }
}