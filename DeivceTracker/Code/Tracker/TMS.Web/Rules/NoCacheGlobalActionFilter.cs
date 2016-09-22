using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMS.Web.Rules
{
    public class NoCacheGlobalActionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
            cache.SetCacheability(HttpCacheability.NoCache);

            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            cache.SetNoStore();

            base.OnResultExecuted(filterContext);
        }
    }
}