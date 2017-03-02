using Alteon.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Alteon.Propaganda.Areas.Administrator.Helper
{
    public class LoginHelper
    {
        private static DateTime Time;
        private static XElement _root;
        public static XElement root
        {
            get
            {
                if (_root == null || Time < DateTime.Now.AddSeconds(-2))
                {
                    _root = XElement.Load(HttpContext.Current.Request.MapPath("/App_Data/Administrator/Account.xml"));
                    Time = DateTime.Now;
                }
                return _root;
            }

        }



        public static XElement IsExistMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return null;
            return root.Elements("item").Where(p => p.Attribute("mobile").Value == mobile).FirstOrDefault();
        }

        /// <summary>
        /// 新增登录用户的Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cookievalue"></param>
        public static void AddLoginUserCookies(string name, string cookievalue)
        {
            CookieHelper.AddCookies(name, name, cookievalue, DateTime.Now.AddDays(7));
        }

        /// <summary>
        /// 获取登录用户Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetLoginUserCookies(string name)
        {
            return CookieHelper.GetCookies(name);
        }
    }
}