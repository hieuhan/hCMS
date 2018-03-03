using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSHelperLib;
using CMSLib;
using hCMS.Library;
using hCMS.Models;
using hCMS.Models.Actions;

namespace hCMS.Controllers
{
    [CmsAuthorize]
    public class ActionsController : Controller
    {
        private readonly int _userId = SessionHelpers.UserId;
        // GET: Actions
        public ActionResult Index()
        {
            var model = new ActionsModel
            {
                ListActions = new Actions().GetAllHierachy2($"\xA0\xA0\xA0"),
                ListActionStatus = ActionStatus.Static_GetList()
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(short actionId = 0)
        {
            var model = new ActionEditModel();
            if (actionId > 0)
            {
                var action = new Actions().Get(actionId);
                if (action.ActionId > 0)
                {
                    model.ActionId = action.ActionId;
                    model.ActionName = action.ActionName;
                    model.ActionDesc = action.ActionDesc;
                    model.Url = action.Url;
                    model.ParentActionId = action.ParentActionId;
                    model.ActionStatusId = action.ActionStatusId;
                    model.ActionOrder = action.ActionOrder;
                    model.Display = action.Display == 1;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ActionEditModel model)
        {
            if (ModelState.IsValid)
            {
                short systemMessageId = 0, levelId = 0;
                byte sysMessageTypeId = 0;
                model.SystemStatus = SystemStatus.Error;
                if (model.ParentActionId > 0)
                {
                    Actions parentAction = new Actions().Get(model.ParentActionId);
                    if (parentAction.ActionId > 0)
                    {
                        levelId = (short)(parentAction.LevelId + 1);
                    }
                }
                var action = new Actions
                {
                    ActionId = model.ActionId,
                    ActionName = model.ActionName,
                    ActionDesc = model.ActionDesc,
                    Url = model.Url.TrimmedOrDefault(string.Empty),
                    ParentActionId = model.ParentActionId,
                    ActionOrder = model.ActionOrder,
                    Display = byte.Parse(model.Display ? "1" : "0"),
                    ActionStatusId = model.ActionStatusId,
                    LevelId = levelId
                };
                sysMessageTypeId = model.ActionId > 0 ? action.Update(0, _userId, ref systemMessageId) : action.Insert(0, _userId, ref systemMessageId);

                if (systemMessageId > 0)
                {
                    var systemMessage = new SystemMessages().Get(systemMessageId);
                    if (sysMessageTypeId == CmsConstants.SystemMessageIdSuccess)
                    {
                        model.SystemStatus = SystemStatus.Success;
                    }
                    ModelState.AddModelError("SystemMessages", systemMessage.SystemMessageDesc);
                }
                else
                {
                    ModelState.AddModelError("SystemMessages", "Bạn vui lòng thử lại sau.");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MultipleAction(ActionsModel model)
        {
            if (model.ActionsId != null && model.ActionsId.Length > 0)
            {
                short systemMessageId = 0;
                foreach (var actionId in model.ActionsId)
                {
                    var action = new Actions
                    {
                        ActionId = actionId
                    };
                    if (model.Delete != null)
                    {
                        action.Delete(0, 0, ref systemMessageId);
                    }
                }
            }
            return Redirect("/Actions/Index");
        }

        public ActionResult ActionRoles(short actionId = 0)
        {
            var model = new ActionRolesModel
            {
                ActionId = actionId,
                ListRoles = new Roles().GetAll(),
                ListRoleActions = new RoleActions().GetByActionId(actionId)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActionRoles(ActionRolesModel model)
        {
            var roleAction = new RoleActions
            {
                ActionId = model.ActionId
            };
            roleAction.DeleteQuickBy(_userId);
            if (model.RolesId != null && model.RolesId.Length > 0)
            {
                foreach (var roleId in model.RolesId)
                {
                    roleAction.RoleId = roleId;
                    roleAction.InsertQuick(_userId);
                }
            }
            model.ListRoles = new Roles().GetAll();
            model.ListRoleActions = new RoleActions().GetByActionId(model.ActionId);
            model.SystemStatus = SystemStatus.Success;
            ModelState.AddModelError("SystemMessages", "Gán quyền cho Chức năng thành công !");
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(short actionId = 0)
        {
            if (actionId > 0)
            {
                short systemMessageId = 0;
                new Actions
                {
                    ActionId = actionId
                }.Delete(0, 0, ref systemMessageId);
            }
            return Redirect("/Actions/Index");
        }

    }
}