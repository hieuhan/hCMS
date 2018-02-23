using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibUtils;

namespace hCMS.AppCode.Attribute
{
    public class CmsHandleErrorAttribute : HandleErrorAttribute
    {
        private const string DefaultView = "Error";
        private readonly object _typeId = new object();

        private Type _exceptionType = typeof(Exception);
        private string _master;
        private string _view;

        public Type ExceptionType
        {
            get { return _exceptionType; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (!typeof(Exception).IsAssignableFrom(value))
                {
                    throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
                        "Exception View Attribute Non Exception Type", value.FullName));
                }

                _exceptionType = value;
            }
        }

        public string Master
        {
            get { return _master ?? String.Empty; }
            set { _master = value; }
        }

        public override object TypeId
        {
            get { return _typeId; }
        }

        public string View
        {
            get { return (!String.IsNullOrEmpty(_view)) ? _view : DefaultView; }
            set { _view = value; }
        }
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            // If custom errors are disabled, we need to let the normal ASP.NET exception handler
            // execute so that the user can see useful debugging information.
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }
            var httpException = new HttpException(null, filterContext.Exception);
            //var httpStatusCode = httpException.GetHttpCode();
            //switch ((HttpStatusCode)httpStatusCode)
            //{
            //    case HttpStatusCode.Forbidden:
            //    case HttpStatusCode.NotFound:
            //    case HttpStatusCode.InternalServerError:
            //        break;

            //    default:
            //        return;
            //}

            if (new HttpException(null, httpException).GetHttpCode() != 500)
            {
                return;
            }

            if (!ExceptionType.IsInstanceOfType(httpException))
            {
                return;
            }

            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        error = true,
                        message = filterContext.Exception.Message
                    }
                };
            }
            else
            {
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];
                HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
                filterContext.Result = new ViewResult
                {
                    ViewName = View,
                    MasterName = Master,
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = filterContext.Controller.TempData
                };
            }
            
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;

            // Certain versions of IIS will sometimes use their own error page when
            // they detect a server error. Setting this property indicates that we
            // want it to try to render ASP.NET MVC's error page instead.
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;



            //base.OnException(filterContext);
            //OVERRIDE THE 500 ERROR  
            //filterContext.HttpContext.Response.StatusCode = 200;
        }

        private static void RaiseErrorSignal(Exception e)
        {
            var context = HttpContext.Current;
            // using.Elmah.ErrorSignal.FromContext(context).Raise(e, context);
        }
    }
}