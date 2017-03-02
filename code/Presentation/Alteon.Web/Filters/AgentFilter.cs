using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace Alteon.Web.Filters
{
    public class AgentFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            //根据用户终端不同设备类型，分配到不同域
            string agent = actionContext.HttpContext.Request.UserAgent.ToLower();
            string[] keywords = { "Android", "iPhone", "iPod", "iPad", "Windows Phone", "MQQBrowser" };
            if (keywords.Select(p => p.ToLower()).Any(p => agent.Contains(p)))
            {

            }
            else
            {
                //actionContext.Result = new RedirectToRouteResult("PC_default", null);
                return;
            }
            base.OnActionExecuting(actionContext);
        }
    }
}