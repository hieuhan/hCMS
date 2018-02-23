using System.Web.Mvc;
using CMSHelperLib;
using CMSLib;
using hCMS.Library;
using hCMS.Models.Roles;

namespace hCMS.Controllers
{
    [CmsAuthorize]
    public class RolesController : Controller
    {
        private readonly int _userId = SessionHelpers.UserId;
        // GET: Roles
        public ActionResult Index()
        {
            var model = new Roles().GetAll();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(short roleId = 0)
        {
            var model = new RolesModel();
            if (roleId > 0)
            {
                var action = new Roles().Get(roleId);
                if (action.RoleId > 0)
                {
                    model.RoleId = action.RoleId;
                    model.RoleName = action.RoleName;
                    model.RoleDesc = action.RoleDesc;
                    model.BuildIn = 0;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RolesModel model)
        {
            if (ModelState.IsValid)
            {
                short systemMessageId = 0;
                var role = new Roles
                {
                    RoleId = model.RoleId,
                    RoleName = model.RoleName,
                    RoleDesc = model.RoleDesc,
                    BuildIn = 0
                };
                if (model.RoleId > 0)
                {
                    role.Update(_userId, ref systemMessageId);
                }
                else
                {
                    role.Insert(_userId, ref systemMessageId);
                }
                if (systemMessageId > 0)
                {
                    var sysMessage = new SystemMessages().Get(systemMessageId);
                    ModelState.AddModelError(string.Empty, sysMessage.SystemMessageDesc);
                }
                else ModelState.AddModelError(string.Empty, "Bạn vui lòng thử lại sau.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MultipleAction(RoleActionsModel model)
        {
            if (model.Delete != null)
            {
                if (model.RolesId != null && model.RolesId.Length > 0)
                {
                    short systemMessageId = 0;
                    foreach (var roleId in model.RolesId)
                    {
                        var role = new Roles
                        {
                            RoleId = roleId
                        };
                        if (model.Delete != null)
                        {
                            role.Delete(0, ref systemMessageId);
                        }
                    }
                }
            }
            return Redirect("/Roles/Index");
        }

        [HttpGet]
        public ActionResult RoleActions(short roleId)
        {
            var model = new RoleActionsModel
            {
                RoleId = roleId,
                ListActionsByRole = new Actions().GetActionsByRole(roleId)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleActions(RoleActionsModel model)
        {
            var roleActions = new RoleActions
            {
                RoleId = model.RoleId
            };
            roleActions.DeleteQuickBy(0);
            if (model.ActionsId != null && model.ActionsId.Length > 0)
            {
                foreach (var item in model.ActionsId)
                {
                    roleActions.ActionId = item;
                    roleActions.InsertQuick(0);
                }
            }
            model.ListActionsByRole = new Actions().GetActionsByRole(model.RoleId);
            ModelState.AddModelError(string.Empty, "Gán chức năng cho quyền thành công !");
            return View(model);
        }
    }
}