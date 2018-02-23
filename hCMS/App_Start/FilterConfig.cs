using System.Web;
using System.Web.Mvc;
using hCMS.AppCode.Attribute;

namespace hCMS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CmsLogExceptionFilter());
            filters.Add(new CmsHandleErrorAttribute());
        }
    }
}
