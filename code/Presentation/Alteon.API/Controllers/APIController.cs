using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Alteon.Model;
using Alteon.Services.Help;
using Alteon.Core.Common;
using Alteon.Core.Extensions;
using Alteon.API.Models;
using Alteon.Core.Caching;
using Alteon.Services.Propaganda;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Alteon.Core.Extensions.Ext;

namespace Alteon.API.Controllers
{
    public class APIController : Controller
    {
        private readonly IpropagandaService _service;

        public APIController(IpropagandaService service)
        {
            _service = service;
        }

        #region 小程序用户登录维护
        public ActionResult WeiXinLogin(string code, string rawData, string signature, string encryptedData, string iv)
        {
            JsonStateResult j = new JsonStateResult();
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(rawData) || string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(encryptedData) || string.IsNullOrEmpty(iv)) 
            {
                j.Msg = "缺失参数";
                return Json(j, JsonRequestBehavior.AllowGet);
            }
            string result = CommonVariable.HttpGet(string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", CommonVariable.appid, CommonVariable.secret,code));
            string openId = string.Empty;
            string sessionKey = string.Empty;
            Dictionary<string, string> dicResult = JsonConverter.DeserializeObject<Dictionary<string, string>>(result);
            if (!dicResult.ContainsKey("openid") || !dicResult.ContainsKey("session_key"))
            {
                return Json(j, JsonRequestBehavior.AllowGet);
            }
            openId = dicResult["openid"];
            sessionKey = dicResult["session_key"];
            string signature2 = (rawData + sessionKey).ToSHA1();
            if (string.Compare(signature, signature2, true) != 0)
            {
                j.Msg = "非法请求，签名校验失败";
                return Json(j, JsonRequestBehavior.AllowGet);
            }
            string data = Cryptography.AESDecrypt(encryptedData, sessionKey, iv);
            if (string.IsNullOrEmpty(data))
            {
                j.Msg = "AES解密出错";
                return Json(j, JsonRequestBehavior.AllowGet);
            }
            WeiXinLoginUser user = JsonConverter.DeserializeObject<WeiXinLoginUser>(data);
            if (string.IsNullOrEmpty(user.openId) || string.IsNullOrEmpty(user.avatarUrl) || string.IsNullOrEmpty(user.nickName)) 
            {
                j.Msg = "json反序列化出错";
                return Json(j, JsonRequestBehavior.AllowGet);
            }
            j.Error = 0;
            j.Data = user;
            return Json(j, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 发送短信（腾讯云）
        /// </summary>
        /// <param name="sendType">1：单条；2：群发</param>
        /// <param name="jsonMobile">json格式的接收者手机号码</param>
        /// <param name="tempId">短信模板ID</param>
        /// <param name="jsonParam">json格式的模板参数</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendSmsCode(int sendType, string mobiles, int tempId, string parameters)
        {
            try
            {
                var appId = CommonVariable.SmsAppId;
                var appKey = CommonVariable.SmsAppKey;

                List<string> templParams = parameters.Split(',').ToList();
                List<string> phoneNumbers = mobiles.Split(',').ToList();
                if (sendType == 1)
                {
                    MemoryCacheManager cache = new MemoryCacheManager();
                    LoginUser loginUser = cache.Get<LoginUser>(phoneNumbers[0]);
                    if (loginUser == null)
                        loginUser = new LoginUser(phoneNumbers[0], 0);
                    if (loginUser.smsSendTimes >= 5)
                    {
                        return Json(new { result = 1 });
                    }
                    SmsSingleSender singleSender = new SmsSingleSender(appId, appKey);
                    SmsSingleSenderResult singleResult = singleSender.SendWithParam("86", phoneNumbers[0], tempId, templParams, "", "", "");
                    loginUser.smsSendTimes += 1;
                    //记录该用户今日发短信验证码的次数
                    cache.Set(phoneNumbers[0], loginUser, 60 * 24);
                    return Json(singleResult);
                }
                else if (sendType == 2)
                {
                    SmsMultiSender multiSender = new SmsMultiSender(appId, appKey);
                    SmsMultiSenderResult multiResult = multiSender.SendWithParam("86", phoneNumbers, tempId, templParams, "", "", "");
                    return Json(multiResult);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { result = -1 });
        }

        /// <summary>
        /// 商户注册
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Regist(string openId, string nickName, string headImg, string mobile, string password, int style)
        {
            JsonStateResult r = new JsonStateResult();
            Propaganda_User user = new Propaganda_User()
            {
                WeiXinOpenId = openId,
                HeadPortrait = headImg,
                Name = nickName,
                Code = "86",
                Account = mobile,
                Password = password.ToSHA1(),
                State = 0,
                Style = style,
                RegisterTime = DateTime.Now
            };
            //新增用户，如果用户已存在则不新增
            int result = _service.AddUser(user);
            if (result == 0)
            {
                r.Error = 0; //注册成功
            }
            return Json(r);
        }

        [HttpPost]
        public JsonResult Login(string mobile, string password)
        {
            JsonStateResult r = new JsonStateResult();
            r.Error = 0;
            var user = _service.ExistUser(new Propaganda_User() { Account = mobile.Trim() });
            if (user == null)
            {
                r.Error = 1;
            }
            else if (string.Compare(user.Password, password.ToSHA1(), true) != 0)
            {
                r.Error = 2;
            }
            else if (user.Style != 2)
            {
                r.Error = 3;
            }
            return Json(r);
        }

        /// <summary>
        /// 根据微信openid获取用户资料，用于显示在个人中心页
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetUserByOpenId(string openId)
        {
            JsonStateResult r = new JsonStateResult();
            var model = _service.GetUserAndAdverByUserOpenId(openId);
            if (model != null) 
            {
                r.Error = 0;
                r.Data = model;
            }
            return Json(r);
        }

        [HttpPost]
        public string GetAdverByUserId(int userId)
        {
            JsonStateResult r = new JsonStateResult();
            IList<string> images = null;
            var list = _service.GetAdverByUserId(userId,out images);
            if (list != null && list.Count() > 0)
            {
                r.Error = 0;
                r.Data = list;
                r.Data2 = images;
            }
            return JsonConverter.SerializeObject(r);
        }

        [HttpPost]
        public JsonResult DeletAdverByAId(int aId)
        {
            JsonStateResult r = new JsonStateResult();
            if (_service.DeletAdverByAId(aId) > 0)
            {
                r.Error = 0;
            }
            return Json(r);
        }

        [HttpPost]
        public JsonResult UpLoadLogo(string openId, HttpPostedFileBase logo)
        {
            JsonStateResult r = new JsonStateResult();
            if (string.IsNullOrEmpty(openId))
            {
                return Json(r);
            }
            try
            {
                string fileName = string.Format("{0}.jpg", DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_hhh"));
                string dir = CommonVariable.baseUrl + openId + "/images/logo/";
                if (!Directory.Exists(Server.MapPath(dir)))
                {
                    Directory.CreateDirectory(Server.MapPath(dir));
                }
                logo.SaveAs(Server.MapPath(dir + fileName));
                if (_service.UpdateLogo(openId, dir + fileName) > 0)
                    r.Error = 0;
            }
            catch (Exception){}
            return Json(r);
        }

        [HttpPost]
        public JsonResult UpdateMobile(string openId, string mobile)
        {
            JsonStateResult r = new JsonStateResult();
            var model = _service.ExistUser(new Propaganda_User { WeiXinOpenId = openId.Trim() });
            if (model != null)
            {
                model.Account = mobile.Trim();
                if(_service.UpdateUser(model))
                {
                    r.Data = model.Id;
                    r.Error = 0;
                }
            }
            return Json(r);
        }

        [HttpPost]
        public JsonResult UpdateUserIntro(int userId, string company, string weixinAccount, string qq, string email, string address)
        {
            JsonStateResult r = new JsonStateResult();
            if (_service.UpdateUserIntroById(userId, company, weixinAccount, qq, email, address) > 0)
            {
                r.Error = 0;
            }
            return Json(r);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="desc"></param>
        /// <param name="type"></param>
        /// <param name="path">图片路径</param>
        /// <param name="url">视频文件的路径</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PublishAdver(int userId,string openId, HttpPostedFileBase adver, string desc, int type, string url)
        {
            JsonStateResult r = new JsonStateResult();
            try
            {
                string fileName = string.Format("{0}.jpg", DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_hhh"));
                string dir = CommonVariable.baseUrl + openId + "/images/adver/";
                if (!Directory.Exists(Server.MapPath(dir)))
                {
                    Directory.CreateDirectory(Server.MapPath(dir));
                }
                adver.SaveAs(Server.MapPath(dir + fileName));
                _service.AddAdverAndAdverContent(userId, desc, type, dir + fileName, url);
                r.Error = 0;
            }
            catch (Exception e) { }
            return Json(r);
        }

        [HttpPost]
        public JsonResult GetAdverLogo(decimal latitude, decimal longitude, int pageIndex, int pageSize)
        {
            JsonStateResult r = new JsonStateResult();
            int areaId = _service.GetAreaByPosition(latitude, longitude);
            int count = 0;
            var list = _service.GetUserByAreaId(out count,areaId: areaId, pageIndex: pageIndex, pageSize: pageSize);
            if (list != null && list.Count() > 0)
            {
                r.Data = list.Where(l => l.Level == 0);
                r.Data2 = list.Where(l => l.Level == 1);
                if(pageSize > 50)
                {
                    r.Data4 = list.Where(t=>string.IsNullOrEmpty(t.Company)==false).Select(t => t.Company );
                }
                else
                {
                    r.Data4 = _service.GetUserByAreaId(out count, areaId: areaId, pageIndex: 1, pageSize: 999).Where(t => string.IsNullOrEmpty(t.Company) == false).Select(t => t.Company );
                }
                r.Msg = areaId.ToString();
                r.Error = 0;
            }
            r.Data3 = _service.GetBanner().Select(t=>t.Url);
            
            return Json(r);
        }
        [HttpPost]
        public JsonResult GetBusinessByKeyword(string keyword, int areaId)
        {
            JsonStateResult j = new JsonStateResult();
            int count = 0;
            var list = _service.GetUserByAreaId(out count, areaId: areaId, keyword:keyword, pageSize: 999);
            if (list != null && list.Count() > 0)
            {
                j.Error = 0;
                j.Data = list;
            }
            return Json(j);
        }

        [HttpPost]
        public JsonResult UpdateUserInfo(string openId, string head, string name)
        {
            JsonStateResult r = new JsonStateResult();
            if (_service.UpdateUserInfo(openId, head, name) > 0)
            {
                r.Error = 0;
            }
            return Json(r);
        }

        [HttpPost]
        public JsonResult GetArea()
        {
            JsonStateResult j = new JsonStateResult();
            var list = _service.GetAllAreas();
            if (list != null && list.Count() > 0)
            {
                j.Error = 0;
                j.Data = list;
            }
            return Json(j);
        }

        [HttpPost]
        public JsonResult SetUserArea(string area,string openId)
        {
            JsonStateResult j = new JsonStateResult();
            if (_service.SetUserArea(area, openId) > 0)
                j.Error = 0;
            return Json(j);
        }

        [HttpPost]
        public JsonResult GetAreaByPosition(decimal latitude, decimal longitude)
        {
            JsonStateResult j = new JsonStateResult();
            var areaId = _service.GetAreaByPosition(latitude, longitude);
            if (areaId > 0)
            {
                j.Error = 0;
                j.Data = areaId;
            }
            return Json(j);
        }

        [HttpPost]
        public void AjaxUploadPropagandaBanner(HttpPostedFileBase image, int sortIndex)
        {
            try
            {
                string fileName = string.Format("{0}.jpg", DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_hhh"));
                string dir = CommonVariable.baseUrl + "Administrator/Banner/";
                if (!Directory.Exists(Server.MapPath(dir)))
                {
                    Directory.CreateDirectory(Server.MapPath(dir));
                }
                image.SaveAs(Server.MapPath(dir + fileName));
                _service.AddBanner(dir + fileName, sortIndex);
            }
            catch (Exception e) { }
        }
	}
}