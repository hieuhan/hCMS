using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using hCMS.Controllers;
using LibUtils;

namespace hCMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            Context.Response.Clear();
            Context.ClearError();
            var httpException = exception as HttpException ?? new HttpException((Int32)HttpStatusCode.InternalServerError, "Internal Server Error", exception);
            var httpStatusCode = httpException.GetHttpCode();
            HttpContext httpContext = HttpContext.Current;
            if (httpContext != null)
            {
                RequestContext requestContext = ((MvcHandler)httpContext.CurrentHandler).RequestContext;
                /* When the request is ajax the system can automatically handle a mistake with a JSON response. 
                   Then overwrites the default response */
                if (requestContext.HttpContext.Request.IsAjaxRequest())
                {
                    httpContext.Response.Clear();
                    string controllerName = requestContext.RouteData.GetRequiredString("controller");
                    IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
                    IController controller = factory.CreateController(requestContext, controllerName);
                    ControllerContext controllerContext = new ControllerContext(requestContext, (ControllerBase)controller);

                    JsonResult jsonResult = new JsonResult
                    {
                        Data = new { success = false, serverError = "500" },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    jsonResult.ExecuteResult(controllerContext);
                    httpContext.Response.End();
                }
                else
                {
                    var routeData = new RouteData();
                    routeData.Values["controller"] = "Error";
                    //routeData.Values["action"] = "generalerror";
                    //routeData.Values.Add("url", Context.Request.Url.OriginalString);
                    if (httpException != null)
                    {
                        switch ((HttpStatusCode)httpStatusCode)
                        {
                            case HttpStatusCode.Forbidden:
                            case HttpStatusCode.NotFound:
                            case HttpStatusCode.InternalServerError:
                                routeData.Values.Add("action", $"Error{httpStatusCode}");
                                break;
                            default:
                                routeData.Values.Add("action", "Index");
                                break;
                        }
                    }
                    Server.ClearError();
                    IController controller = new ErrorController();
                    controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                }
            }
        }

    }
}
