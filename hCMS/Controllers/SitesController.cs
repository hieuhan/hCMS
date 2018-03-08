using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSHelperLib;
using CMSLib;
using hCMS.Library;
using hCMS.Models;
using hCMS.Models.Sites;

namespace hCMS.Controllers
{
    [CmsAuthorize]
    public class SitesController : Controller
    {
        private readonly int _userId = SessionHelpers.UserId;

        // GET: Sites
        public ActionResult Index(int page = 1)
        {
            int rowCount = 0;
            var model = new SitesModel
            {
                ListSites = new Sites().GetPage(0, CmsConstants.RowAmount20, page > 0 ? page - 1 : page, "SiteId Desc", "", "", ref rowCount),
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
        public ActionResult Edit(short siteId = 0)
        {
            var model = new SitesEditModel();
            if (siteId > 0)
            {
                var site = new Sites { SiteId = siteId }.Get(0);
                if (site.SiteId > 0)
                {
                    model.SiteId = site.SiteId;
                    model.SiteName = site.SiteName;
                    model.SiteDesc = site.SiteDesc;
                    model.MetaTitle = site.MetaTitle;
                    model.MetaDesc = site.MetaDesc;
                    model.MetaKeyword = site.MetaKeyword;
                    model.MetaTagAll = site.MetaTagAll;
                    model.CanonicalTag = site.CanonicalTag;
                    model.H1Tag = site.H1Tag;
                    model.CrUserId = site.CrUserId;
                    model.CrDateTime = site.CrDateTime;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SitesEditModel model)
        {
            if (ModelState.IsValid)
            {
                short sysMessageId = 0;
                byte sysMessageTypeId = 0;
                model.SystemStatus = SystemStatus.Error;
                var site = new Sites
                {
                    SiteId = model.SiteId,
                    SiteName = model.SiteName,
                    SiteDesc = model.SiteDesc,
                    MetaTitle = model.MetaTitle,
                    MetaDesc = model.MetaDesc,
                    MetaKeyword = model.MetaKeyword,
                    MetaTagAll = model.MetaTagAll,
                    CanonicalTag = model.CanonicalTag,
                    H1Tag = model.H1Tag,
                    CrUserId = model.CrUserId,
                    CrDateTime = model.CrDateTime
                };
                sysMessageTypeId = model.SiteId > 0 ? site.Update(0, _userId, ref sysMessageId) : site.Insert(0, _userId, ref sysMessageId);

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
        public ActionResult Delete(short siteId = 0)
        {
            if (siteId > 0)
            {
                short sysMessageId = 0;
                var site = new Sites { SiteId = siteId }.Get(0);
                if (site.SiteId > 0)
                {
                    new Sites { SiteId = siteId }.Delete(0, 0, ref sysMessageId);
                }
            }
            return Redirect("/Sites/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MultipleAction(SiteMultipleActionModel model)
        {
            if (model.Delete != null)
            {
                if (model.SiteIds != null && model.SiteIds.Length > 0)
                {
                    short sysMessageId = 0;
                    foreach (var siteId in model.SiteIds)
                    {
                        new Sites { SiteId = siteId }.Delete(0, 0, ref sysMessageId);
                    }
                }
            }
            return Redirect("/Sites/Index");
        }

    }
}