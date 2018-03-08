using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSHelperLib;
using CMSLib;
using hCMS.Library;
using hCMS.Models;
using hCMS.Models.Countries;

namespace hCMS.Controllers
{
    [CmsAuthorize]
    public class CountriesController : Controller
    {
        private readonly int _userId = SessionHelpers.UserId;

        // GET: Countries
        public ActionResult Index(string countryName = "",int page = 1)
        {
            int rowCount = 0;
            var model = new CountriesModel
            {
                ListCountries = new Countries{CountryName = countryName }.GetPage(string.Empty,string.Empty,string.Empty,CmsConstants.RowAmount20,page>0?page-1:page,ref rowCount),
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
        public ActionResult Edit(short countryId = 0)
        {
            var model = new CountriesEditModel();
            if (countryId > 0)
            {
                var country = new Countries { CountryId = countryId }.Get();
                if (country.CountryId > 0)
                {
                    model.CountryId = country.CountryId;
                    model.CountryName = country.CountryName;
                    model.CountryDesc = country.CountryDesc;
                    model.ImagePath = country.IconPath;
                    model.DisplayOrder = country.DisplayOrder;
                    model.CrUserId = country.CrUserId;
                    model.CrDateTime = country.CrDateTime;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CountriesEditModel model)
        {
            if (ModelState.IsValid)
            {
                short sysMessageId = 0;
                byte sysMessageTypeId = 0;
                model.SystemStatus = SystemStatus.Error;
                var country = new Countries
                {
                    CountryId = model.CountryId,
                    CountryName = model.CountryName,
                    CountryDesc = model.CountryDesc,
                    IconPath = model.ImagePath,
                    DisplayOrder = model.DisplayOrder,
                    CrUserId = model.CrUserId,
                    CrDateTime = model.CrDateTime
                };
                sysMessageTypeId = model.CountryId > 0 ? country.Update(0, _userId, ref sysMessageId) : country.Insert(0, _userId, ref sysMessageId);

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
        public ActionResult Delete(short countryId = 0)
        {
            if (countryId > 0)
            {
                short sysMessageId = 0;
                var country = new Countries { CountryId = countryId }.Get();
                if (country.CountryId > 0)
                {
                    new Countries { CountryId = countryId }.Delete(0, 0, ref sysMessageId);
                }
            }
            return Redirect("/Countries/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MultipleAction(CountriesMultipleActionModel model)
        {
            if (model.Delete != null)
            {
                if (model.CountryIds != null && model.CountryIds.Length > 0)
                {
                    short sysMessageId = 0;
                    foreach (var countryId in model.CountryIds)
                    {
                        new Countries { CountryId = countryId }.Delete(0, 0, ref sysMessageId);
                    }
                }
            }
            return Redirect("/Countries/Index");
        }
    }
}