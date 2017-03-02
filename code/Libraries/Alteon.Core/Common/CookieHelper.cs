using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Alteon.Core.Extensions;

namespace Alteon.Core.Common
{
    public class CookieHelper
    {

        ///<summary>
        /// 添加cookeis
        ///</summary>
        public static void AddCookies(string name, string mark, string value, DateTime date)
        {
            HttpCookie cookies = HttpContext.Current.Request.Cookies[name];
            if (cookies == null)
                cookies = new HttpCookie(name);
            cookies[mark] = value.EncodeBase64();
            cookies.Expires = date;
            HttpContext.Current.Response.Cookies.Add(cookies);
        }


        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCookies(string name, string mark)
        {
            HttpCookie cookies = HttpContext.Current.Request.Cookies[name];
            if (cookies != null)
            {
                string values = cookies[mark];   //通过key取出对应value，多key同理取
                return values.DecodeBase64();
            }
            return string.Empty;
        }
        public static string GetCookies(string name)
        {
            HttpCookie cookies = HttpContext.Current.Request.Cookies[name];
            if (cookies != null)
                return cookies[name].DecodeBase64();   //通过key取出对应value，多key同理取
            else
                return null;
        }

        /// <summary>
        /// 清除Cookies
        /// </summary>
        public static void RemoveCookies(string name)
        {
            AddCookies(name, name, "", DateTime.Now.AddYears(-1));
        }









        /// <summary>
        /// 新增用户的Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cookievalue"></param>
        public static void AddUserCookies(string name, string cookievalue)
        {
            AddCookies(name, name, cookievalue, DateTime.Now.AddDays(7));
        }
        public static string GetUserCookies(string name)
        {
            HttpCookie cookies = HttpContext.Current.Request.Cookies[name];
            if (cookies != null)
                return cookies[name].DecodeBase64();   //通过key取出对应value，多key同理取
            else
                return null;
        }
    }
}
