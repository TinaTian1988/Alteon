using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Alteon.Web.Framework.Aop
{
    /// <summary>
    /// 授权认证拦截器
    /// </summary>
    public sealed class OnAuthorizeAttribute : AuthorizeAttribute
    {
        private bool allowVisit;

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //获取无需认证特性标识
            var nonAuthorization = filterContext.ActionDescriptor.GetCustomAttributes(typeof(NonAuthorizeAttribute), true).Any();

            if (filterContext.HttpContext.User.Identity.IsAuthenticated || nonAuthorization)
                allowVisit = true;
            else
                allowVisit = false;

            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return allowVisit;
        }
    }
}
