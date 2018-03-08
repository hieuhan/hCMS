using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSHelperLib;
using CMSLib;
using hCMS.Library;
using hCMS.Models;
using hCMS.Models.MediaGroups;

namespace hCMS.Controllers
{
    [CmsAuthorize]
    public class MediaGroupsController : Controller
    {
        private readonly int _userId = SessionHelpers.UserId;
        // GET: MediaGroups
        public ActionResult Index(short siteId = 1)
        {
            var model = new MediaGroupsModel
            {
                ListUsers = new Users().GetAll(),
                ListMediaGroups = new MediaGroups().GetAllHierachy(_userId, siteId, 0, $"\xA0\xA0\xA0")
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(short mediaGroupId=0)
        {
            var model = new MediaGroupsEditModel();
            if (mediaGroupId > 0)
            {
                var mediaGroup = new MediaGroups().Get(mediaGroupId);
                if (mediaGroup.MediaGroupId > 0)
                {
                    model.SiteId = mediaGroup.SiteId;
                    model.MediaGroupId = mediaGroup.MediaGroupId;
                    model.MediaGroupName = mediaGroup.MediaGroupName;
                    model.MediaGroupDesc = mediaGroup.MediaGroupDesc;
                    model.ParentGroupId = mediaGroup.ParentGroupId;
                    model.CrUserId = mediaGroup.CrUserId;
                    model.CrDateTime = mediaGroup.CrDateTime;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MediaGroupsEditModel model)
        {
            if (ModelState.IsValid)
            {
                short sysMessageId = 0;
                byte sysMessageTypeId = 0;
                model.SystemStatus = SystemStatus.Error;
                var mediaGroup = new MediaGroups
                {
                    SiteId = model.SiteId,
                    MediaGroupId = model.MediaGroupId,
                    MediaGroupName = model.MediaGroupName,
                    MediaGroupDesc = model.MediaGroupDesc,
                    ParentGroupId = model.ParentGroupId,
                    CrUserId = model.CrUserId,
                    CrDateTime = model.CrDateTime
                };
                sysMessageTypeId = model.MediaGroupId > 0 ? mediaGroup.Update(0, _userId, ref sysMessageId) : mediaGroup.Insert(0, _userId, ref sysMessageId);

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
        public ActionResult Delete(short mediaGroupId = 0)
        {
            if (mediaGroupId > 0)
            {
                short sysMessageId = 0;
                var mediaGroup = new MediaGroups { MediaGroupId = mediaGroupId }.Get();
                if (mediaGroup.MediaGroupId > 0)
                {
                    new MediaGroups { MediaGroupId = mediaGroupId }.Delete(0, 0, ref sysMessageId);
                }
            }
            return Redirect("/MediaGroups/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MultipleAction(MediaGroupMultipleActionModel model)
        {
            if (model.Delete != null)
            {
                if (model.MediaGroupIds != null && model.MediaGroupIds.Length > 0)
                {
                    short sysMessageId = 0;
                    foreach (var mediaGroupId in model.MediaGroupIds)
                    {
                        new MediaGroups{ MediaGroupId = mediaGroupId }.Delete(0, 0, ref sysMessageId);
                    }
                }
            }
            return Redirect("/MediaGroups/Index");
        }
    }
}