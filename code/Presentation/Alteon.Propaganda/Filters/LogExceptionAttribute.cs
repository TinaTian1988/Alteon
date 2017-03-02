using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alteon.Propaganda.Filters
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class LogExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                string area = (string)filterContext.RouteData.DataTokens["area"];
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];
                string msgTemplate = "在执行{0} controller[{1}] 的 action[{2}] 时产生异常：{3}";
                LogManager.GetLogger("LogExceptionAttribute").Error(string.Format(msgTemplate, area, controllerName, actionName, filterContext.Exception));
            }
            base.OnException(filterContext);
        }
    }
}