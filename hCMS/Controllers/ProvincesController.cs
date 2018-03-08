using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSHelperLib;
using CMSLib;
using hCMS.Library;
using hCMS.Models;
using hCMS.Models.Provinces;

namespace hCMS.Controllers
{
    [CmsAuthorize]
    public class ProvincesController : Controller
    {
        private readonly int _userId = SessionHelpers.UserId;
        // GET: Provinces
        public ActionResult Index(short countryId=0,string provinceName = "", int page = 1)
        {
            int rowCount = 0;
            var model = new ProvincesModel
            {
                ListProvinces = new Provinces { CountryId = countryId, ProvinceName = provinceName }.GetPage(string.Empty, string.Empty, string.Empty, CmsConstants.RowAmount20, page > 0 ? page - 1 : page, ref rowCount),
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
        public ActionResult Edit(short provinceId = 0)
        {
            var model = new ProvinceEditModel();
            if (provinceId > 0)
            {
                var province = new Provinces().Get(provinceId);
                if (province.ProvinceId > 0)
                {
                    model.ProvinceId = province.ProvinceId;
                    model.ProvinceName = province.ProvinceName;
                    model.ProvinceDesc = province.ProvinceDesc;
                    model.CountryId = province.CountryId;
                    model.DisplayOrder = province.DisplayOrder;
                    model.CrUserId = province.CrUserId;
                    model.CrDateTime = province.CrDateTime;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProvinceEditModel model)
        {
            if (ModelState.IsValid)
            {
                short sysMessageId = 0;
                byte sysMessageTypeId = 0;
                model.SystemStatus = SystemStatus.Error;
                var province = new Provinces
                {
                    CountryId = model.CountryId,
                    ProvinceId = model.ProvinceId,
                    ProvinceName = model.ProvinceName,
                    ProvinceDesc = model.ProvinceDesc,
                    DisplayOrder = model.DisplayOrder,
                    CrUserId = model.CrUserId,
                    CrDateTime = model.CrDateTime
                };
                sysMessageTypeId = model.CountryId > 0 ? province.Update(0, _userId, ref sysMessageId) : province.Insert(0, _userId, ref sysMessageId);

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
        public ActionResult Delete(short provinceId = 0)
        {
            if (provinceId > 0)
            {
                short sysMessageId = 0;
                var province = new Provinces { ProvinceId = provinceId }.Get();
                if (province.ProvinceId > 0)
                {
                    new Provinces { ProvinceId = provinceId }.Delete(0, 0, ref sysMessageId);
                }
            }
            return Redirect("/Provinces/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MultipleAction(ProvincesMultipleActionModel model)
        {
            if (model.Delete != null)
            {
                if (model.ProvinceIds != null && model.ProvinceIds.Length > 0)
                {
                    short sysMessageId = 0;
                    foreach (var provinceId in model.ProvinceIds)
                    {
                        new Provinces { ProvinceId = provinceId }.Delete(0, 0, ref sysMessageId);
                    }
                }
            }
            return Redirect("/Provinces/Index");
        }

    }
}