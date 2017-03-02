using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Text;
using Alteon.Core.Common;
using Alteon.Services.DataApi;
using NLog;
using System.Net;
using Alteon.Core.Caching;
using Alteon.Core.Extensions;
using Alteon.Model;
using System.Net.Sockets;
using Alteon.DataApi.Models;
using Alteon.DataApi.App_Start;
using Alteon.DataApi.Filters;

namespace Alteon.DataApi.Controllers
{
    
    public class HomeController : MainController
    {
        private static Logger log = LogManager.GetCurrentClassLogger(typeof(HomeController));
        private readonly IDataApiService _dataApiService;
        private ICacheManager cache;
        public HomeController() { }
        public HomeController(IDataApiService dataApiService)
        {
            _dataApiService = dataApiService;
            cache = new MemoryCacheManager();
        }




        #region 注册登录
        /// <summary>
        /// 注册页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }
        public JsonResult CheckUserKey(string userkey)
        {
            JsonStateResult result = new JsonStateResult();
            var owner = _dataApiService.IsExistOwner(userkey);
            if (owner != null)
            {
                result.Error = 0;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result.Error = 1;
            result.Msg = "非法UserKey";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[JsonException]
        public JsonResult RegisterPost(string userKey, string phone, string password)
        {
            JsonStateResult result = new JsonStateResult();
            var owner = _dataApiService.IsExistOwner(null, phone);
            if (owner != null)
            {
                result.Error = 1;
                result.Msg = "已经注册过啦，直接去登录吧";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            owner = _dataApiService.IsExistOwner(userKey);
            if (owner != null)
            {
                owner.Mobile = phone.Trim();
                owner.Password = (password.Trim() + userKey).ToSHA1();
                owner.Status = 1;
                owner.RegisterTime = DateTime.Now;
                _dataApiService.UpdateOwner(owner);

                LoginUser loginUser = new LoginUser()
                {
                    Id = owner.Id,
                    Name = owner.Name,
                    Mobile = owner.Mobile,
                    UserIdentity = (LoginUserIdentity)owner.UserIdentity
                };
                base.AddLoginUserCookie(loginUser);
                result.Error = 0;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result.Error = 2;
                result.Msg = "注册失败，请重试";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            base.RemoveLoginUserCookie();
            return View();
        }
        [HttpPost]
        //[JsonException]
        public JsonResult LoginPost(string phone, string password)
        {
            JsonStateResult result = new JsonStateResult();
            var owner = _dataApiService.IsExistOwner(null, phone);
            if (owner == null)
            {
                result.Error = 1;
                result.Msg = "还没有账号，快去注册一个吧";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (string.Compare(owner.Password, (password + owner.Id).ToSHA1(), true) != 0)
                {
                    result.Error = 2;
                    result.Msg = "密码不正确";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else if (owner.Status != 1)
                {
                    result.Error = 3;
                    result.Msg = "账号已禁用，请联系管理员";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LoginUser loginUser = new LoginUser()
                    {
                        Id = owner.Id,
                        Name = owner.Name,
                        Mobile = owner.Mobile,
                        UserIdentity = (LoginUserIdentity)owner.UserIdentity
                    };
                    base.AddLoginUserCookie(loginUser);
                    result.Error = 0;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion
	}
}