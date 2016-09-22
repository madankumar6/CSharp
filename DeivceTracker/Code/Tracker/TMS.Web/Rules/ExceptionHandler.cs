using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using TMS.BusinessRule.Interfaces;
using TMS.Model;
using Utils;

namespace TMS.Web.Rules
{
    public class ExceptionHandler
    {
    }
    public class ExceptionHandlingAttribute : HandleErrorAttribute
    {
        #region Properties
        private static readonly string FileName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType?.ToString();
        internal static Log4NetWrap Logger = new Log4NetWrap(FileName);
        #endregion

        
        public override void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            context.ExceptionHandled = true;

            //Log Critical errors
            Logger.ErrorFormat("Controller : {0} - Action : {1}, Message : {2} Trace : \n {3}", context.RouteData.Values["controller"].ToString(), context.RouteData.Values["action"].ToString(), exception.Message, exception.StackTrace);

            var model = new HandleErrorInfo(context.Exception, "Home", "Error");

            HttpContext.Current.Response.StatusCode = 500;

            context.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };
        }
    }
}