using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Alteon.Core.Caching;
using Alteon.Core.Infrastructure;
using Alteon.Core;
using Alteon.Web.Framework.Mvc;
using Alteon.Core.Enum;

namespace Alteon.Web.Framework.Controllers
{
    /// <summary>
    /// 全局抽象基类Controller
    /// </summary>
    public abstract class WebControllerBase : Controller
    {
        protected readonly ICacheManager _memoryCacheManager;
        protected readonly ICacheManager _perRequestCacheManager;
        protected readonly IWebHelper _webHelper;

        public WebControllerBase()
        {
            _memoryCacheManager = EngineContext.Resolve<ICacheManager>(typeof(MemoryCacheManager));
            _perRequestCacheManager = EngineContext.Resolve<ICacheManager>(typeof(PerRequestCacheManager));
            _webHelper = EngineContext.Resolve<IWebHelper>();
        }

        public AlteonJsonResult AlteonJson(ResultStatus status, string message = "", object data = null)
        {
            return new AlteonJsonResult(status, message, data);
        }

        #region ViewToString

        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }

        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }

        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }

        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <param name="model">Model</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(string viewName, object model)
        {
            //Original source code: http://craftycodeblog.com/2010/05/15/asp-net-mvc-render-partial-view-to-string/
            if (string.IsNullOrEmpty(viewName))
                viewName = this.ControllerContext.RouteData.GetRequiredString("action");

            this.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion
    }
}
