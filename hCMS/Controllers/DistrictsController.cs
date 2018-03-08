using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSHelperLib;
using CMSLib;
using hCMS.Library;
using hCMS.Models;
using hCMS.Models.Districts;

namespace hCMS.Controllers
{
    [CmsAuthorize]
    public class DistrictsController : Controller
    {
        private readonly int _userId = SessionHelpers.UserId;
        // GET: Districts
        public ActionResult Index(short countryId = 0, short provinceId=0,string districtName="", int page=1)
        {
            int rowCount = 0;
            var model = new DistrictsModel
            {
                ListDistricts = new Districts{CountryId = countryId,ProvinceId = provinceId,DistrictName = districtName}.GetPage(_userId,CmsConstants.RowAmount20,page>0?page-1:page,string.Empty,ref rowCount),
                ListProvinces = countryId > 0 ? new Provinces().GetList(countryId) : new List<Provinces>(),
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
        public ActionResult Edit(short districtId = 0)
        {
            var model = new DistrictEditModel();
            if (districtId > 0)
            {
                var district = new Districts().Get(districtId);
                if (district.DistrictId > 0)
                {
                    model.DistrictId = district.DistrictId;
                    model.CountryId = district.CountryId;
                    model.ProvinceId = district.ProvinceId;
                    model.DistrictName = district.DistrictName;
                    model.DistrictDesc = district.DistrictDesc;
                    model.DisplayOrder = district.DisplayOrder;
                    model.CrUserId = district.CrUserId;
                    model.CrDateTime = district.CrDateTime;
                    model.ListProvinces= new Provinces().GetList(district.CountryId);
                }
            }else model.ListProvinces= new List<Provinces>();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DistrictEditModel model)
        {
            if (ModelState.IsValid)
            {
                short sysMessageId = 0;
                byte sysMessageTypeId = 0;
                model.SystemStatus = SystemStatus.Error;
                var district = new Districts
                {
                    CountryId = model.CountryId,
                    ProvinceId = model.ProvinceId,
                    DistrictId = model.DistrictId,
                    DistrictName = model.DistrictName,
                    DistrictDesc = model.DistrictDesc,
                    DisplayOrder = model.DisplayOrder,
                    CrUserId = model.CrUserId,
                    CrDateTime = model.CrDateTime
                };
                sysMessageTypeId = model.CountryId > 0 ? district.Update(0, _userId, ref sysMessageId) : district.Insert(0, _userId, ref sysMessageId);

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
        public ActionResult Delete(short districtId = 0)
        {
            if (districtId > 0)
            {
                short sysMessageId = 0;
                var district = new Districts { DistrictId = districtId }.Get();
                if (district.ProvinceId > 0)
                {
                    new Districts { DistrictId = districtId }.Delete(0, 0, ref sysMessageId);
                }
            }
            return Redirect("/Districts/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MultipleAction(DistrictsMultipleActionModel model)
        {
            if (model.Delete != null)
            {
                if (model.DistrictIds != null && model.DistrictIds.Length > 0)
                {
                    short sysMessageId = 0;
                    foreach (var districtId in model.DistrictIds)
                    {
                        new Districts { DistrictId = districtId }.Delete(0, 0, ref sysMessageId);
                    }
                }
            }
            return Redirect("/Districts/Index");
        }
    }
}