using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Web.Framework.ViewEngines.Razor
{
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        /// <summary>
        /// 静态资源处理。(CSS/JS)
        /// </summary>
        /// <param name="orginPath">原始路径</param>
        public string Cnt(string orginPath)
        {
            //TODO:加个版本号管理
            return string.Format("{0}?v={1}", Url.Content(orginPath), "1.0");
        }
    }
}
