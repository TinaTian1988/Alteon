using Alteon.Propaganda.Areas.Administrator.Helper;
using Alteon.Propaganda.Areas.Administrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alteon.Propaganda.Areas.Administrator.Filters
{
    public class NeedLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (string.IsNullOrEmpty(LoginHelper.GetLoginUserCookies(CookieName.loginUserCookieName)))
            {
                filterContext.Result = new RedirectResult("/Administrator/Login/Index");
            }
        }
    }
}