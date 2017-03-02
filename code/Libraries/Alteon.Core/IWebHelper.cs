using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Alteon.Core
{
    /// <summary>
    /// 常见WebHelper
    /// </summary>
    public interface IWebHelper
    {
        /// <summary>
        /// 获取Server变量的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string ServerVariables(string name);

        /// <summary>
        /// 获取当前主机地址，如:http://www.xxx.com/
        /// </summary>
        /// <returns></returns>
        string GetCurrentHost();

        /// <summary>
        /// Get context IP address
        /// </summary>
        /// <returns>URL referrer</returns>
        string GetCurrentIpAddress();

        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        string MapPath(string path);

        /// <summary>
        /// 判断当前请求是否静态资源
        /// </summary>
        /// <remarks>
        /// These are the file extensions considered to be static resources:
        /// .css
        ///	.gif
        /// .png 
        /// .jpg
        /// .jpeg
        /// .js
        /// .axd
        /// .ashx
        /// </remarks>
        bool IsStaticResource(HttpRequest request);
    }
}
