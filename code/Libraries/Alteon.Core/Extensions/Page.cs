using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Text.RegularExpressions;

namespace Alteon.Core.Extensions
{
    public static class Page
    {
        /// <summary>
        /// 获取QueryString中指定参数名的值，当参数不存在或参数值为空时返回string.Empty
        /// </summary>
        /// <param name="key">参数名</param>        
        /// <returns>参数值</returns>
        public static string QueryString(string key)
        {
            return QueryString(key, string.Empty);
        }

        /// <summary>
        /// 获取QueryString中指定参数名的值
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="defaultValue">当参数不存在或参数值为空时要返回的默认值</param>
        /// <returns>参数值</returns>
        public static string QueryString(string key, string defaultValue = "")
        {
            string param = defaultValue;
            string value = HttpContext.Current.Request.QueryString[key];
            if (!string.IsNullOrEmpty(value))
            {
                param = value;
            }
            return param;
        }

        /// <summary>
        /// 获取QueryString中指定参数名的值，当参数不存在或参数值为空时返回0
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns>参数值</returns>
        public static int QueryInt(string key)
        {
            return QueryInt(key, 0);
        }

        /// <summary>
        /// 获取QueryString中指定参数名的值
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="defaultValue">当参数不存在或参数值为空时要返回的默认值</param>
        /// <returns>参数值</returns>
        public static int QueryInt(string key, int defaultValue = 0)
        {
            return QueryString(key, string.Empty).ToInt(defaultValue);
        }

        /// <summary>
        /// 获取Form中指定参数名的值，当参数不存在或参数值为空时返回string.Empty
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns>参数值</returns>
        public static string FormString(string key)
        {
            return FormString(key, string.Empty);
        }

        /// <summary>
        /// 获取Form中指定参数名的值
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="defaultValue">当参数不存在或参数值为空时要返回的默认值</param>
        /// <returns>参数值</returns>
        public static string FormString(string key, string defaultValue = "")
        {
            string param = defaultValue;
            if (HttpContext.Current.Request.Form[key] != null && HttpContext.Current.Request.Form[key] != "")
            {
                param = HttpContext.Current.Request.Form[key];
            }
            return param;
        }

        /// <summary>
        /// 获取Form中指定参数名的值，当参数不存在或参数值为空时返回0
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns>参数值</returns>
        public static int FormInt(string key)
        {
            return FormInt(key, 0);
        }

        /// <summary>
        /// 获取Form中指定参数名的值
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="defaultValue">当参数不存在或参数值为空时要返回的默认值</param>
        /// <returns>参数值</returns>
        public static int FormInt(string key, int defaultValue = 0)
        {
            return FormString(key, string.Empty).ToInt(defaultValue);
        }

        /// <summary>
        /// 获取当前请求中指定参数名的值。优先从QueryString中找，当参数不存在或参数值为空时返回string.Empty
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns>参数值</returns>
        public static string ParamS(string key)
        {
            return ParamS(key, string.Empty);
        }

        /// <summary>
        /// 获取当前请求中指定参数名的值。优先从QueryString中找
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="defaultValue">当参数不存在或参数值为空时要返回的默认值</param>
        /// <returns>参数值</returns>
        public static string ParamS(string key, string defaultValue = "")
        {
            if (HttpContext.Current.Request.QueryString[key] != null && HttpContext.Current.Request.QueryString[key] != "")
            {
                return QueryString(key, defaultValue);
            }
            else if (HttpContext.Current.Request.Form[key] != null && HttpContext.Current.Request.Form[key] != "")
            {
                return FormString(key, defaultValue);
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取当前请求中指定参数名的值。优先从QueryString中找，当参数不存在或参数值为空时返回0
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns>参数值</returns>
        public static int ParamI(string key)
        {
            return ParamI(key, 0);
        }

        /// <summary>
        /// 获取当前请求中指定参数名的值。优先从QueryString中找
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="defaultValue">当参数不存在或参数值为空时要返回的默认值</param>
        /// <returns>参数值</returns>
        public static int ParamI(string key, int defaultValue)
        {
            if (HttpContext.Current.Request.QueryString[key] != null && HttpContext.Current.Request.QueryString[key] != "")
            {
                return QueryInt(key, defaultValue);
            }
            else if (HttpContext.Current.Request.Form[key] != null && HttpContext.Current.Request.Form[key] != "")
            {
                return FormInt(key, defaultValue);
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取当前网站的域名
        /// </summary>
        /// <returns></returns>
        public static string GetDomain()
        {
            var url = HttpContext.Current.Request.Url.ToString();
            if (url.IndexOf("localhost") > -1 || url.IndexOf("127.0.0.1") > -1) return "";

            return GetDomain(url);
        }

        /// <summary>
        /// 获取当前网站的域名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetDomain(string url)
        {

            string host;
            Uri uri;
            try
            {
                uri = new Uri(url);
                host = uri.Host;
                if (true == host.IsIP4()) return "";
            }
            catch
            {
                host = null;
                return "";
            }

            string re = @"(\w+(\.\w+){1}[^\.]+)+";
        AGAIN:
            Match m = Regex.Match(host, re, RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
            Capture c = null;
            while (m.Success)
            {
                Group g = m.Groups[1];
                CaptureCollection cc = g.Captures;
                c = cc[0];
                break;
                m = m.NextMatch();
            }

            if (c == null)
            {
                return "";
            }
            else
            {
                string[] doubleDomain = new string[] { "com.cn", "edu.cn", "net.cn", "org.cn", "co.jp", "gov.cn", "co.uk", "ac.cn" };
                foreach (string strDmn in doubleDomain)
                {
                    if (c.Value.ToLower().IndexOf(strDmn) == 0)
                    {
                        re = @"(\w+(\.\w+){2}[^\.]+)+";
                        goto AGAIN;
                        //break;
                    }
                }

            }

            return c.Value;
        }


    }
}
