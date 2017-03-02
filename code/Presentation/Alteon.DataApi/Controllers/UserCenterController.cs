using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Alteon.Services.DataApi;
using Alteon.Model;
using NLog;
using Alteon.Core.Caching;
using Alteon.Core.Common;
using Alteon.Core.Extensions;
using Alteon.DataApi.Filters;
using Alteon.DataApi.App_Start;
using System.Text;
using Alteon.DataApi.Models;

using System.Web.Security;
using System.Threading.Tasks;
using Alteon.Model.Api.Extension;
using System.Net;
using System.Net.Sockets;


namespace Alteon.DataApi.Controllers
{
    public class UserCenterController : MainController
    {
        private readonly IDataApiService _dataApiService;
        private LoginUser loginUser;
        public UserCenterController() { }
        public UserCenterController(IDataApiService dataApiService)
        {
            _dataApiService = dataApiService;
            loginUser = base.GetLoginUserCookie();
        }

        /// <summary>
        /// 用户中心，操控设备
        /// </summary>
        /// <returns></returns>

        public ActionResult Index()
        {
            return View(loginUser);
        }



        public ActionResult IndexContent()
        {
            return View();
        }

        #region 设备
        public ActionResult EquipmentList()
        {
            string loginUserId = loginUser.Id;
            ViewBag.userKey = loginUserId;
            return View();
        }
        [HttpPost]
        public ActionResult AjaxGetEquipmentList(string userKey, int pageIndex, int pageSize)
        {
            JsonStateResult result = new JsonStateResult();
            var list = _dataApiService.GetEquipmentAndDataByUserId(userKey, pageIndex, pageSize);
            if (list != null && list.Count > 0)
            {
                List<EquipmentViewModel> vmList = new List<EquipmentViewModel>();
                for (int i = 0; i < list.Count; i++)
                {
                    vmList.Add(new EquipmentViewModel(list[i]));
                }
                result.Error = 0;
                result.Msg = _dataApiService.GetEquipmentAndDataByUserIdCount(userKey).ToString();
                result.Data = vmList;
                return Content(JsonConverter.SerializeObject(result));
            }
            result.Error = 1;
            result.Msg = "没有数据";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EquipmentDataList()
        {
            string loginUserId = loginUser.Id;
            ViewBag.ownerId = loginUserId;
            return View();
        }
        [HttpPost]
        public ActionResult GetDataList(string ownerId)
        {
            JsonStateResult result = new JsonStateResult();
            var list = _dataApiService.GetEquipmentData(ownerId);
            if (list != null && list.Count > 0)
            {
                result.Error = 0;
                result.Msg = list.Count.ToString();
                result.Data = list.OrderBy(d => d.SortingIndex);
                return Content(JsonConverter.SerializeObject(result));
            }
            result.Error = 1;
            result.Msg = "没有数据";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AjaxChangeEquipmentDataState(int state, int eId, string mark)
        {
            JsonStateResult r = new JsonStateResult();
            r.Error = 1;
            r.Msg = "操作失败";
            if (_dataApiService.ChangeEquipmentDataState(state, eId) && TcpClient(string.Format("key{0}={1}", mark, state)))
            {
                r.Error = 0;
            }
            return Json(r);
        }

        [HttpPost]
        public JsonResult AjaxAddEquipment(Api_ClientEquipment model, int flag)
        {
            JsonStateResult result = new JsonStateResult();
            string loginUserId = loginUser.Id;
            var equipment = _dataApiService.IsExistEquipment(0, model.Name);
            if (flag == 0)//添加
            {
                if (equipment != null)
                {
                    result.Error = 1;
                    result.Msg = "已经添加过此设备";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                model.IsDelete = false;
                _dataApiService.AddEquipment(model);
                if (model.Id > 0)
                {
                    result.Error = 0;
                    result.Msg = "添加成功";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                result.Error = 2;
                result.Msg = "添加失败，请重试";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (flag == 1)//修改
            {
                equipment.Sorting = model.Sorting;
                equipment.Address = model.Address;
                equipment.Intro = model.Intro;
                equipment.IsControl = model.IsControl;
                equipment.IsPublic = model.IsPublic;
                equipment.Status = model.Status;
                try
                {
                    _dataApiService.UpdateEquipment(equipment);
                    result.Error = 0;
                    result.Msg = "修改成功";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    result.Error = 2;
                    result.Msg = "修改失败，请重试";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            result.Error = 3;
            result.Msg = "未知错误";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetEquipmentById(int id)
        {
            JsonStateResult result = new JsonStateResult();
            var equipment = _dataApiService.GetEquipmentById(id);
            if (equipment != null)
            {
                result.Error = 0;
                result.Data = equipment;
                return Content(JsonConverter.SerializeObject(result));
            }
            result.Error = 1;
            result.Msg = "不存在的设备";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDataById(int id)
        {
            JsonStateResult result = new JsonStateResult();
            var data = _dataApiService.GetDataById(id);
            if (data != null)
            {
                result.Error = 0;
                result.Data = data;
                return Content(JsonConverter.SerializeObject(result));
            }
            result.Error = 1;
            result.Msg = "不存在的设备";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditDataById(Api_EquipmentData model, int flag=0)
        {
            JsonStateResult result = new JsonStateResult();
            try
            {
                if (flag > 0)//新增
                {
                    if (_dataApiService.IsExistEquipmentData(model.EquipmentId, model.Mark.ToUpper()) != null)
                    {
                        result.Error = 1;
                        result.Msg = "已存在标识为“" + model.Mark + "”的数据了，请勿重复添加";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    _dataApiService.InsertEquipmentData(model);
                    result.Error = 0;
                    result.Msg = "添加成功";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                _dataApiService.UpdateEquimentData(model);
                result.Error = 0;
                result.Msg = "修改成功";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                result.Error = 1;
                result.Msg = "修改失败";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteDataById(int id)
        {
            JsonStateResult result = new JsonStateResult();
            try
            {
                var model = _dataApiService.GetDataById(id);
                model.IsDelete = true;
                _dataApiService.UpdateEquimentData(model);
                result.Error = 0;
                result.Msg = "删除成功";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                result.Error = 1;
                result.Msg = "删除失败";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteDataByEquipmentId(int equipmentId, string mark)
        {
            JsonStateResult result = new JsonStateResult();
            try
            {
                _dataApiService.DeleteDataByEquipmentIdAndMark(equipmentId, mark);
                result.Error = 0;
                result.Msg = "删除成功";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                result.Error = 1;
                result.Msg = "删除失败";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[JsonException]
        public JsonResult DeleteEquipmentById(int id)
        {
            JsonStateResult result = new JsonStateResult();
            try
            {
                _dataApiService.DeleteEquipment(id);
                result.Error = 0;
                result.Msg = "删除成功";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                result.Error = 1;
                result.Msg = "删除失败";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DataChart(int equipmentId, string mark, string unit)
        {
            return View();
        }

        /// <summary>
        /// 今天数据走势图表（以900秒为间隔，取最高值）
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="mark"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDataChart(int equipmentId, string mark)
        {
            StringBuilder sb = new StringBuilder();
            DateTime begin;
            DateTime end;
            IList<Alteon.Model.Api.Extension.EquipmentData> list = _dataApiService.GetDataByEquipmentIdAndMark(equipmentId, mark);
            if (list.Count > 96)//数据过多，显示不完，按900s间隔取最高值
            {
                List<ChartData> resultData = new List<ChartData>();
                TimeSpan timeSpan = list[list.Count - 1].CreateTime.ToDateTime() - list[0].CreateTime.ToDateTime();
                int span = (int)timeSpan.TotalSeconds / 900;
                begin = list[0].CreateTime.ToDateTime();
                end = begin.AddSeconds(900);
                for (int s = 0; s < span; s++)
                {


                    decimal highest = 0;

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].CreateTime >= begin && list[i].CreateTime < end)
                        {
                            if (list[i].Value >= list[i + 1].Value)
                                highest = (decimal)list[i].Value;
                            else
                                highest = (decimal)list[i + 1].Value;
                        }
                    }
                    resultData.Add(new ChartData() { time = end.ToString("yyyy-MM-dd HH:mm:ss"), value = highest.ToString("0.00") });

                    begin = end;
                    end = begin.AddSeconds(900);
                }
                ChartData[] array = resultData.ToArray();
                return Json(array);
            }
            else//直接显示
            {
                ChartData[] array = new ChartData[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    array[i] = new ChartData() { time = ((DateTime)list[i].CreateTime).ToString("yyyy-MM-dd HH:mm:ss"), value = list[i].Value.ToString() };
                }
                return Json(array);
            }
        }




        public ActionResult HistoryChart()
        {
            string userId = loginUser.Id;
            ViewData["equipments"] = _dataApiService.GetEquipmentAndDataByUserId(userId, 1, 9999);
            return View();
        }

        [HttpPost]
        public JsonResult GetDataListByEquipmentId(int euipmentId)
        {
            JsonStateResult result = new JsonStateResult();
            var list = _dataApiService.GetEquipmentDataByEquipmentId(euipmentId, 1, 9999);
            if (list != null && list.Count > 0)
            {
                result.Error = 0;
                result.Msg = list.Count.ToString();
                result.Data = list;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result.Error = 1;
            result.Msg = "没有数据";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllDataChart(int equipmentId, string mark, ChartSearchParam param)
        {
            StringBuilder sb = new StringBuilder();
            DateTime begin;
            DateTime end;
            IList<Alteon.Model.Api.Extension.EquipmentData> list = _dataApiService.GetAllDataByEquipmentIdAndMark(equipmentId, mark, param);
            if (list.Count > 96)//数据过多，显示不完，按900s间隔取平均值
            {
                List<ChartData> resultData = new List<ChartData>();
                TimeSpan timeSpan = list[list.Count - 1].CreateTime.ToDateTime() - list[0].CreateTime.ToDateTime();
                int span = (int)timeSpan.TotalSeconds / 900;
                begin = list[0].CreateTime.ToDateTime();
                end = begin.AddSeconds(900);
                for (int s = 0; s < span; s++)
                {
                    decimal highest = 0;

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].CreateTime >= begin && list[i].CreateTime < end)
                        {
                            if (list[i].Value >= list[i + 1].Value)
                                highest = (decimal)list[i].Value;
                            else
                                highest = (decimal)list[i + 1].Value;
                        }
                    }
                    resultData.Add(new ChartData() { time = end.ToString("yyyy-MM-dd HH:mm:ss"), value = highest.ToString("0.00") });


                    begin = end;
                    end = begin.AddSeconds(900);
                }
                ChartData[] array = resultData.ToArray();
                return Json(array);
            }
            else//直接显示
            {
                ChartData[] array = new ChartData[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    array[i] = new ChartData() { time = ((DateTime)list[i].CreateTime).ToString("yyyy-MM-dd HH:mm:ss"), value = list[i].Value.ToString() };
                }
                return Json(array);
            }
        }


        public ActionResult HistoryData()
        {
            string userId = loginUser.Id;
            ViewData["equipments"] = _dataApiService.GetEquipmentAndDataByUserId(userId, 1, 9999);
            return View();
        }

        [HttpPost]
        //[JsonException]
        public ActionResult GetAllDataByEquipmentIdAndMark(int equipmentId, string mark, ChartSearchParam param, int pageIndex, int pageSize)
        {
            JsonStateResult result = new JsonStateResult();
            var list = _dataApiService.GetAllDataByEquipmentIdAndMark(equipmentId, mark, param, pageIndex, pageSize);
            if (list != null && list.Count() > 0)
            {
                result.Error = 0;
                result.Msg = _dataApiService.GetAllDataByEquipmentIdAndMarkCount(equipmentId, mark, param).ToString();
                result.Data = list;
                return Content(JsonConverter.SerializeObject(result));
            }
            result.Error = 1;
            result.Msg = "没有数据";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 用户

        public ActionResult UserInfo(string flag = "")
        {
            ViewBag.flag = flag;
            return View(loginUser);
        }
        [HttpPost]
        public JsonResult CompleteInfo(string id, string userName, int cid, string cname)
        {
            JsonStateResult result = new JsonStateResult();
            try
            {
                var user = _dataApiService.IsExistOwner(id);
                user.Name = userName;
                _dataApiService.UpdateOwner(user);
                result.Error = 0;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                result.Error = 1;
                result.Msg = "出错了";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ChangePsd()
        {
            return View(loginUser);
        }

        [HttpPost]
        public JsonResult CheckOldPassword(string id, string password)
        {
            JsonStateResult result = new JsonStateResult();
            try
            {
                var user = _dataApiService.IsExistOwner(id);
                if ((password + id).ToSHA1() == user.Password)
                {
                    result.Error = 0;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                result.Error = 1;
                result.Msg = "原密码验证不通过";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                result.Error = 1;
                result.Msg = "出错了";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ChangePassword(string id, string password)
        {
            JsonStateResult result = new JsonStateResult();
            try
            {
                var user = _dataApiService.IsExistOwner(id);
                if (user != null)
                {
                    user.Password = (password + id).ToSHA1();
                    _dataApiService.UpdateOwner(user);
                    result.Error = 0;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                result.Error = 1;
                result.Msg = "非法用户";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                result.Error = 1;
                result.Msg = "出错了";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddUser()
        {
            if (loginUser.UserIdentity != LoginUserIdentity.SuperManager)
            {
                return Redirect("/Home/Login");
            }


            return View();
        }
        [HttpGet]
        public JsonResult CreateGuid()
        {
            JsonStateResult result = new JsonStateResult();
            string id = Guid.NewGuid().ToString().Replace("-", "");
            if (!string.IsNullOrEmpty(id))
            {
                Api_ClientOwner user = new Api_ClientOwner()
                {
                    Id = id,
                    UserIdentity = 2,
                    RegisterTime = DateTime.Now
                };
                _dataApiService.AddUserGuid(user);
                result.Data = id;
                result.Error = 0;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result.Error = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 太阳能发电专属页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Electricity()
        {
            return View();
        }


        #region tcp
        private bool TcpClient(string command)
        {
            try
            {
                TcpClient client = new TcpClient("182.254.230.155", 5000);
                IPEndPoint ipendpoint = client.Client.RemoteEndPoint as IPEndPoint;
                NetworkStream stream = client.GetStream();
                byte[] messages = Encoding.Default.GetBytes(command);
                stream.Write(messages, 0, messages.Length);
                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
            
        }
        #endregion


        #region 短信测试
        public string CreateSign(string app_key, string smsFreeSignName, string app_secret)
        {
            string data = app_key + smsFreeSignName + app_secret;
            string createsign = SHA1(data).ToUpper();
            return createsign;
        }
        public string SHA1(string source)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(source, "SHA1");
        }
        public ActionResult AliDaYuSmsTest()
        {
            //string url = "http://gw.api.taobao.com/router/rest";
            //string appkey = "23363034";
            //string secret = "b096d66daf8f778457d6d5f755746c2d";
            //ITopClient client = new DefaultTopClient(url, appkey, secret);
            //AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            ////req.Extend = "123456";
            //req.SmsType = "norjjmal";
            //req.SmsFreeSignName = "大鱼ddd测试";
            //req.SmsParam = "{\"code\":\"007007\",\"product\":\"顾问易\"}";
            //req.RecNum = "18620614886";
            //req.SmsTemplateCode = "SMS_8971296";
            //AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            //string result = rsp.Body;
            //return Content(rsp.Body);

            var ser = new ServiceReference1.SentNoteClient();
            //string app_key = "anyi";
            //string app_secret = "3571592486";
            //string content = "testmessage001";
            //string sign = CreateSign(app_key, content, app_secret);
            //Task<bool> resultTask = ser.SentCustomExtAsync(app_key, sign, content, new string[] { "18620614886" }, "【顾问易】", 2);
            ////bool result = ser.SentCustomExt(app_key, sign, content, new string[] { "18620614886" }, "【顾问易】", 2);

            string app_key = "anyi";
            string app_secret = "3571592486";
            string smsFreeSignName = "大鱼测试";
            string sign = CreateSign(app_key, smsFreeSignName, app_secret);
            string smsParam = "{\"code\":\"007007\",\"product\":\"顾问易\"}";
            string smsTemplateCode = "SMS_8971296";
            string[] phones = new string[] { "18620614886" };
            //bool result = ser.AliSmsSent(app_key, sign, smsFreeSignName, smsTemplateCode, smsParam, phones);
            var result = ser.AliSmsSentAsync(app_key, sign, smsFreeSignName, smsTemplateCode, smsParam, phones);
            return Content(result.ToStr());



        }
        #endregion
    }


}