using Alteon.Core.Common;
using Alteon.DataApi.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alteon.DataApi.Filters
{
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            MainController main = filterContext.Controller as MainController;
            var loginUser = main.GetLoginUserCookie();
            if (loginUser == null)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
            else
            {
                string agent = filterContext.HttpContext.Request.UserAgent.ToLower();
                string[] keywords = { "Android", "iPhone", "iPod", "iPad", "Windows Phone", "MQQBrowser" };
                if (keywords.Select(p => p.ToLower()).Any(p => agent.Contains(p)))
                {
                    //filterContext.Result = new RedirectResult("/mobileweb/index");
                }
                else
                {
                    filterContext.Result = new RedirectResult("/usercenter/index");
                }
                return;
            }

        }
    }
}