using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alteon.DataApi.Filters
{
    public class UserAgentFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            string agent = actionContext.HttpContext.Request.UserAgent.ToLower();
            string[] keywords = { "Android", "iPhone", "iPod", "iPad", "Windows Phone", "MQQBrowser" };
            if (keywords.Select(p => p.ToLower()).Any(p => agent.Contains(p)))
            {

            }
            else
            {
                //actionContext.Result = new RedirectToRouteResult("PC_default", null);
                actionContext.Result = new RedirectResult("/mobileweb/index");
                return;
            }
            base.OnActionExecuting(actionContext);
        }
    }
}