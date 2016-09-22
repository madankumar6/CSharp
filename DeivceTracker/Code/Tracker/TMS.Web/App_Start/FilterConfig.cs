using System.Web;
using System.Web.Mvc;
using TMS.Web.Rules;

namespace TMS.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new TMSAuthorizeAttribute());
            filters.Add(new ExceptionHandlingAttribute());
            filters.Add(new NoCacheGlobalActionFilter());
        }
    }
}
