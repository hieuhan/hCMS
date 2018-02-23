using System.Web.Mvc;
using CMSHelperLib;
using CMSLib;
using CMSViewLib;
using hCMS.Library;
using hCMS.Models;
using hCMS.Models.Users;

namespace hCMS.Controllers
{
    public class UsersController : Controller
    {
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
                var result = UserHelpers.Login(model.UserName, model.Password);
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
                    model.Password = user.Password;
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
                    Password = model.Password,
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
        public ActionResult MultipleAction(UsersModel model)
        {
            if (model.Delete != null)
            {
                if (model.UsersId != null && model.UsersId.Length > 0)
                {
                    short sysMessageId = 0;
                    foreach (var userId in model.UsersId)
                    {
                        new Users { UserId = userId }.Delete(0, ref sysMessageId);
                    }
                }
            }
            return Redirect("/Users/Index");
        }

    }
}