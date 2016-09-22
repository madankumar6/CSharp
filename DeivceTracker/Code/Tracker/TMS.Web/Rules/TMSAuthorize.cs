using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace TMS.Web.Rules
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class TMSAuthorizeAttribute : AuthorizeAttribute
    {
        protected virtual CustomPrincipal CurrentUser
        {
            get
            {
                return HttpContext.Current.User as CustomPrincipal;
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session== null || httpContext.Session["UserData"] == null || !httpContext.Request.IsAuthenticated)
            {
                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                return false;
            }

            CustomPrincipal user = CurrentUser;

            return (user.Identity.IsAuthenticated
                && (String.IsNullOrEmpty(Roles) || Roles.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).AsEnumerable().Select(i => i.Trim()).Any(role => role.Equals(user.Role, StringComparison.OrdinalIgnoreCase)))
                && (String.IsNullOrEmpty(Users) || Users.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).AsEnumerable().Select(i => i.Trim()).Any(u => u.Equals(user.Role, StringComparison.OrdinalIgnoreCase))));
        }

        //Called when access is denied
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {controller = "UserAccount", action = "Login", returnUrl = HttpContext.Current.Request.RawUrl }));
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                
            }
            else
                filterContext.Result =
                    new RedirectToRouteResult(new RouteValueDictionary(new {controller = "Home", action = "Error"}));
        }
    }
}