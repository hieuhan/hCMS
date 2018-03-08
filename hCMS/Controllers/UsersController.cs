using System.Web.Mvc;
using CMSHelperLib;
using CMSLib;
using CMSViewLib;
using hCMS.Library;
using hCMS.Models;
using hCMS.Models.Users;
using LibUtils;

namespace hCMS.Controllers
{
    public class UsersController : Controller
    {
        private int _userId = SessionHelpers.UserId;
        // GET: Users
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = "")
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new UserLoginModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                string urlRedirect = "/Users/ChangePassword";
                var result = UserHelpers.Login(model.UserName, md5.MD5Hash(model.Password));
                if (result.ActionStatus.Equals("OK"))
                {
                    SessionHelpers.UserId = result.User.UserId;
                    SessionHelpers.UserName = result.User.UserName;
                    SessionHelpers.DefaultAction = result.User.DefaultActionId;

                    //ToDo ghi logs
                    new UserLogs { UserName = result.User.UserName, StatusId = result.User.UserStatusId, CrDateTime = result.User.CrDateTime, IPAddress = Request.UserHostAddress, UserAgent = Request.UserAgent }.InsertQuick();

                    if (Url.IsLocalUrl(model.ReturnUrl) && model.ReturnUrl.Length > 1 &&
                        model.ReturnUrl.StartsWith("/") && !model.ReturnUrl.StartsWith("//") &&
                        !model.ReturnUrl.StartsWith("/\\"))
                    {
                        urlRedirect = model.ReturnUrl;
                    }
                    else
                    {
                        if (result.User.DefaultActionId > 0)
                        {
                            var action = new Actions().Get(result.User.DefaultActionId);
                            if (action.ActionId > 0 && !string.IsNullOrEmpty(action.Url))
                            {
                                urlRedirect = action.Url.StartsWith(CmsConstants.ROOT_PATH)
                                    ? action.Url
                                    : string.Concat(CmsConstants.ROOT_PATH, action.Url);
                            }
                        }
                    }

                    return Redirect(urlRedirect);
                }
                ModelState.AddModelError(string.Empty, result.ActionMessage);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return Redirect("/Users/Login");
        }

        [CmsAuthorize]
        public ActionResult Index(byte userTypeId = 0, byte userStatusId = 0, string userName = "", int page = 1)
        {
            int rowCount = 0;
            string fullName = string.Empty,
                address = string.Empty,
                email = string.Empty,
                mobile = string.Empty, dateFrom = string.Empty, dateTo = string.Empty, orderBy = string.Empty;
            byte genderId = 0;

            var model = new UsersModel
            {
                ListUsers = new Users().GetPage(userName, fullName, address, email, mobile, genderId, userStatusId, userTypeId, dateFrom, dateTo, orderBy, CmsConstants.RowAmount20, page > 0 ? page - 1 : page, ref rowCount),
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
        [CmsAuthorize]
        public ActionResult Edit(int userId = 0)
        {
            var model = new UserEditModel();
            if (userId > 0)
            {
                var user = new Users().Get(userId);
                if (user.UserId > 0)
                {
                    model.UserId = user.UserId;
                    model.UserName = user.UserName;
                    //model.Password = user.Password;
                    model.FullName = user.Fullname;
                    model.Mobile = user.Mobile;
                    model.Email = user.Email;
                    model.Address = user.Address;
                    model.GenderId = user.GenderId;
                    model.UserTypeId = user.UserTypeId;
                    model.UserStatusId = user.UserStatusId;
                    model.DefaultActionId = user.DefaultActionId;
                    model.BirthDay = user.Birthday;
                    model.Comments = user.Comments;
                }
            }
            return View(model);
        }

        [HttpPost]
        [CmsAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditModel model)
        {
            if (ModelState.IsValid)
            {
                short sysMessageId = 0;
                var user = new Users
                {
                    UserId = model.UserId,
                    UserName = model.UserName,
                    //Password = md5.MD5Hash(model.Password),
                    Fullname = model.FullName,
                    Email = model.Email,
                    Address = model.Address,
                    Mobile = model.Mobile,
                    Birthday = model.BirthDay,
                    GenderId = model.GenderId,
                    UserTypeId = model.UserTypeId,
                    UserStatusId = model.UserStatusId,
                    DefaultActionId = model.DefaultActionId,
                    Comments = model.Comments
                };
                if (model.UserId > 0)
                {
                    user.Update(0, ref sysMessageId);
                }
                else
                {
                    user.Insert(0, ref sysMessageId);
                }

                if (sysMessageId > 0)
                {
                    var sysMessage = new SystemMessages().Get(sysMessageId);
                    ModelState.AddModelError(string.Empty, sysMessage.SystemMessageDesc);
                }
                else ModelState.AddModelError(string.Empty, "Bạn vui lòng thử lại sau.");
            }
            return View(model);
        }

        [HttpGet]
        [CmsAuthorize]
        public ActionResult Create()
        {
            return View(new UserCreateModel());
        }

        [HttpPost]
        [CmsAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                short sysMessageId = 0;
                byte sysMessageTypeId = 0;
                model.SystemStatus = SystemStatus.Error;
                var user = new Users
                {
                    UserId = model.UserId,
                    UserName = model.UserName,
                    Password = md5.MD5Hash(model.Password),
                    Fullname = model.FullName,
                    Email = model.Email,
                    Address = model.Address,
                    Mobile = model.Mobile,
                    Birthday = model.BirthDay,
                    GenderId = model.GenderId,
                    UserTypeId = model.UserTypeId,
                    UserStatusId = model.UserStatusId,
                    DefaultActionId = model.DefaultActionId,
                    Comments = model.Comments
                };
                sysMessageTypeId = user.Insert(0, ref sysMessageId);
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
        [CmsAuthorize]
        public ActionResult Delete(int userId = 0)
        {
            if (userId > 0)
            {
                short cmsMessageId = 0;
                new Users { UserId = userId }.Delete(0, ref cmsMessageId);
            }
            return RedirectToAction("Index", "Users");
        }

        [HttpPost]
        [CmsAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult MultipleAction(UserMultipleActionModel model)
        {
            if (model.Delete != null)
            {
                if (model.UserIds != null && model.UserIds.Length > 0)
                {
                    short sysMessageId = 0;
                    foreach (var userId in model.UserIds)
                    {
                        new Users { UserId = userId }.Delete(0, ref sysMessageId);
                    }
                }
            }
            return Redirect("/Users/Index");
        }

        [CmsAuthorize]
        public ActionResult ChangePassword()
        {
            var model = new ChangePasswordModel
            {
                UserName = SessionHelpers.UserName
            };
            return View(model);
        }

        [HttpPost]
        [CmsAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                short sysMessageId = 0;
                byte sysMessageTypeId = 0;
                model.SystemStatus = SystemStatus.Error;
                Users user = new Users().Get(_userId);
                if (user.UserId > 0)
                {
                    if (user.Password != md5.MD5Hash(model.CurrentPassword))
                    {
                        ModelState.AddModelError("SystemMessages", "Mật khẩu cũ không chính xác.");
                    }
                    else
                    {
                        user.Password = md5.MD5Hash(model.Password);
                        sysMessageTypeId = user.Update(_userId, ref sysMessageId);
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
                }
                else ModelState.AddModelError("SystemMessages", "User không tồn tại.");
            }
            return View(model);
        }

        [CmsAuthorize]
        public ActionResult Profile()
        {
            var model = new UserProfileModel();
            var user = new Users().Get(_userId);
            if (user.UserId > 0)
            {
                model.UserId = user.UserId;
                model.UserName = user.UserName;
                //model.Password = user.Password;
                model.FullName = user.Fullname;
                model.Mobile = user.Mobile;
                model.Email = user.Email;
                model.Address = user.Address;
                model.GenderId = user.GenderId;
                model.UserTypeId = user.UserTypeId;
                model.UserStatusId = user.UserStatusId;
                model.DefaultActionId = user.DefaultActionId;
                model.BirthDay = user.Birthday;
                model.Comments = user.Comments;
            }
            return View(model);
        }

        [HttpPost]
        [CmsAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Profile(UserProfileModel model)
        {
            if (ModelState.IsValid)
            {
                short sysMessageId = 0;
                byte sysMessageTypeId = 0;
                model.SystemStatus = SystemStatus.Error;
                Users user = new Users().Get(_userId);
                if (user.UserId > 0)
                {
                    user.Fullname = model.FullName;
                    user.Email = model.Email;
                    user.Mobile = model.Mobile;
                    user.GenderId = model.GenderId;
                    user.Birthday = model.BirthDay;
                    user.Address = model.Address;
                    user.UserTypeId = model.UserTypeId;
                    user.UserStatusId = model.UserStatusId;
                    user.DefaultActionId = model.DefaultActionId;
                    user.Comments = model.Comments;
                    sysMessageTypeId = user.Update(_userId, ref sysMessageId);
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
                else ModelState.AddModelError("SystemMessages", "User không tồn tại.");
            }
            return View(model);
        }

        [CmsAuthorize]
        public ActionResult UserRoles(int userId)
        {
            var model = new UserRolesModel
            {
                UserId = userId,
                ListUserRoles = new UserRoles().GetListByUserId(userId)
            };
            return View(model);
        }

        [HttpPost]
        [CmsAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult UserRoles(UserRolesModel model)
        {
            var userRoles = new UserRoles
            {
                UserId = model.UserId
            };
            userRoles.DeleteQuickBy(_userId);
            if (model.RolesId != null && model.RolesId.Length > 0)
            {
                foreach (var item in model.RolesId)
                {
                    userRoles.RoleId = item;
                    userRoles.InsertQuick(_userId);
                }
            }
            model.ListUserRoles = new UserRoles().GetListByUserId(model.UserId);
            model.SystemStatus = SystemStatus.Success;
            ModelState.AddModelError("SystemMessages", "Gán nhóm chức năng cho User thành công !");
            return View(model);
        }

        [CmsAuthorize]
        public ActionResult UserActions(int userId)
        {
            var model = new UserActionsModel
            {
                UserId = userId,
                ListUserActions = new UserActions().GetByUser(userId)
            };
            return View(model);
        }

        [HttpPost]
        [CmsAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult UserActions(UserActionsModel model)
        {
            var userAction = new UserActions
            {
                UserId = model.UserId
            };
            userAction.DeleteQuickBy();
            if (model.ActionsId != null && model.ActionsId.Length > 0)
            {
                foreach (var actionId in model.ActionsId)
                {
                    userAction.ActionId = actionId;
                    userAction.InsertQuick();
                }
            }
            model.ListUserActions = new UserActions().GetByUser(model.UserId);
            model.SystemStatus = SystemStatus.Success;
            ModelState.AddModelError("SystemMessages", "Gán Chức năng cho User thành công !");
            return View(model);
        }

        [CmsAuthorize]
        public ActionResult UserSites(int userId)
        {
            var model = new UserSitesModel
            {
                UserId = userId,
                ListSites = Sites.Static_GetList(0),
                ListUserSites = new UserSites().GetByUser(userId)
            };
            return View(model);
        }

        [HttpPost]
        [CmsAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult UserSites(UserSitesModel model)
        {
            short sysMessageId = 0;
            var userSite = new UserSites
            {
                UserId = model.UserId
            };
            userSite.DeleteByUserId(0, _userId, ref sysMessageId);
            if (model.SitesId != null && model.SitesId.Length > 0)
            {
                foreach (var siteId in model.SitesId)
                {
                    userSite.SiteId = siteId;
                    userSite.Insert(1, _userId, ref sysMessageId);
                }
            }
            model.ListSites = Sites.Static_GetList(0);
            model.ListUserSites = new UserSites().GetByUser(model.UserId);
            model.SystemStatus = SystemStatus.Success;
            ModelState.AddModelError("SystemMessages", "Gán Site cho User thành công !");
            return View(model);
        }

    }
}