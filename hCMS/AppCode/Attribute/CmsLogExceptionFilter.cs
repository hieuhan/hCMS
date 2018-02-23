using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibUtils;

namespace hCMS.AppCode.Attribute
{
    public class CmsLogExceptionFilter: IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;
            //var request = filterContext.HttpContext.Request;
            LogFiles.WriteLog(LogFiles.LogLevel.ERROR,exception,filterContext);
        }
    }
}