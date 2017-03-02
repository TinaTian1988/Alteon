using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebPages.Html;

namespace Alteon.Web.Framework
{
    /// <summary>
    /// *.cshtml -> HtmlHelper扩展
    /// </summary>
    public static class HtmlHelperExtension
    {
        public static IHtmlString Demo(this HtmlHelper that, string demo)
        {
            return new HtmlString(demo);
        }
    }
}
