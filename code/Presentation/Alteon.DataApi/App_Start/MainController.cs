using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Alteon.Core.Common;
using Alteon.DataApi.Models;
using Alteon.Core.Extensions;
using System.Text;
using Alteon.DataApi.Filters;

namespace Alteon.DataApi.App_Start
{
    public class MainController : Controller
    {
        private readonly string loginUser = "loginUser";
        private readonly string strCategory = "category";
        /// <summary>
        /// 添加登录用户的cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddLoginUserCookie(LoginUser model)
        {
            string value = (model.Id + "|" + model.Name + "|" + model.Mobile + "|" + model.UserIdentity);
            CookieHelper.AddUserCookies(loginUser, value);
        }
        /// <summary>
        /// 获取登录用户cookie
        /// </summary>
        /// <returns></returns>
        public LoginUser GetLoginUserCookie()
        {
            string value = CookieHelper.GetUserCookies(loginUser);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            string[] cookies = value.Split('|');
            LoginUser user = new LoginUser()
            {
                Id = cookies[0],
                Name = cookies[1],
                Mobile = cookies[2],
                UserIdentity = (LoginUserIdentity)Enum.Parse(typeof(LoginUserIdentity), cookies[3], true)
            };
            return user;
        }
        /// <summary>
        /// 清楚登录用户的cookie
        /// </summary>
        public void RemoveLoginUserCookie()
        {
            CookieHelper.RemoveCookies(loginUser);
        }

        ///// <summary>
        ///// 用户筛选的类别
        ///// </summary>
        ///// <param name="id"></param>
        //public void AddCategory(ClientCategory category)
        //{
        //    string cookieValue = HttpUtility.UrlEncode((category.Id + "|" + category.Name), Encoding.GetEncoding("UTF-8"));
        //    CookieHelper.AddCookies(strCategory, strCategory, cookieValue, DateTime.Now.AddDays(7));
        //}

        //public ClientCategory GetCategory()
        //{
        //    string cookie = HttpUtility.UrlDecode(CookieHelper.GetCookies(strCategory, strCategory), Encoding.GetEncoding("UTF-8"));
        //    if (!string.IsNullOrEmpty(cookie))
        //    {
        //        ClientCategory category = new ClientCategory() { Id = int.Parse(cookie.Split('|')[0]), Name = cookie.Split('|')[1] };
        //        return category;
        //    }
        //    return null;
        //}
    }


}