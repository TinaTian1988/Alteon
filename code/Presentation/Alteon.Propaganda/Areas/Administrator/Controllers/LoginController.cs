using Alteon.Core.Common;
using Alteon.Core.Extensions;
using Alteon.Propaganda.Areas.Administrator.Helper;
using Alteon.Propaganda.Areas.Administrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alteon.Propaganda.Areas.Administrator.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AjaxLogin(string inputPhone, string inputPassword)
        {
            SimpleJsonResult result = new SimpleJsonResult();
            var accountElement = LoginHelper.IsExistMobile(inputPhone.Trim());
            if (accountElement == null)
            {
                result.message = "不存在的用户";
                return Json(result);
            }
            string sha1Psd = StrExt.GetSHA1Str(inputPassword.Trim());
            if (string.Compare(accountElement.Attribute("password").Value, sha1Psd, true) > 0)
            {
                result.message = "密码错误";
                return Json(result);
            }
            LoginUser user = new LoginUser();
            user.Mobile = inputPhone.Trim();
            user.Password = sha1Psd;
            LoginHelper.AddLoginUserCookies(CookieName.loginUserCookieName, JsonConverter.SerializeObject(user));
            result.status = 1;
            return Json(result);
        }
	}
}