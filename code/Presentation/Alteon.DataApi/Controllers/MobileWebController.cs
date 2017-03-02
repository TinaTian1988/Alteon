using Alteon.DataApi.App_Start;
using Alteon.DataApi.Filters;
using Alteon.DataApi.Models;
using Alteon.Model;
using Alteon.Services.DataApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Alteon.Core.Extensions;

namespace Alteon.DataApi.Controllers
{
    [LoginFilter]
    public class MobileWebController : MainController
    {
        private LoginUser loginUser;
        private readonly IDataApiService _dataApiService;
        public MobileWebController(IDataApiService dataApiService) 
        {
            loginUser = base.GetLoginUserCookie();
            _dataApiService = dataApiService;
        }


        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 设备列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Equipment()
        {
            string loginUserId = loginUser.Id;
            ViewBag.userKey = loginUserId;
            return View();
        }
        /// <summary>
        /// 实时数据
        /// </summary>
        /// <returns></returns>
        public ActionResult DataList()
        {
            string loginUserId = loginUser.Id;
            ViewBag.ownerId = loginUserId;
            return View();
        }

        public ActionResult Chart(int equipmentId, string mark,string markName, string unit)
        {
            return View();
        }
        /// <summary>
        /// 历史图表
        /// </summary>
        /// <returns></returns>
        public ActionResult HistoryChart()
        {
            string userId = loginUser.Id;
            ViewData["equipments"] = _dataApiService.GetEquipmentAndDataByUserId(userId, 1, 9999);

            return View();
        }
        /// <summary>
        /// 历史数据
        /// </summary>
        /// <returns></returns>
        public ActionResult HistoryData()
        {
            string userId = loginUser.Id;
            ViewData["equipments"] = _dataApiService.GetEquipmentAndDataByUserId(userId, 1, 9999);
            return View();
        }
        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        public ActionResult UserCenter()
        {
            return View();
        }

        /// <summary>
        /// 太阳能发电专属页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Electricity()
        {
            return View();
        }

        [HttpPost]
        public string GetDataListByEquipmentIdAndMark(int euipmentId, string mark)
        {
            JsonStateResult result = new JsonStateResult();
            if (loginUser.Id != "cbba3ba376ad42a7abb3e36294f87086")
            {
                result.Error = 1;
                result.Msg = "没有发电设备监控权限";
                return Alteon.Core.Common.JsonConverter.SerializeObject(result);
            }
            
            var data = _dataApiService.GetEquipmentDataByEquipmentIdAndMark(euipmentId, mark);
            if (data != null )
            {
                data.CarbonDioxideReduce = Convert.ToDecimal(0.0007979M * data.Value).ToString("0.00");
                data.InComeMoney = Convert.ToDecimal(data.Value * data.MoneyMethod).ToString("0.00");
                data.TodayElectricity = (decimal)data.Value - (_dataApiService.GetLastDayElectricity(euipmentId, mark));

                result.Error = 0;
                result.Data = data;
                return Alteon.Core.Common.JsonConverter.SerializeObject(result);
            }
            result.Error = 1;
            result.Msg = "没有数据";
            return Alteon.Core.Common.JsonConverter.SerializeObject(result);
        }

        public ActionResult ElectricityChart(int equipmentId, string mark, string markName, string unit)
        {
            return View();
        }

        public ActionResult SubTabs()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetElectDataChart(int equipmentId, string mark, string time, int flag)
        {
            //StringBuilder sb = new StringBuilder();
            //DateTime begin;
            //DateTime end;
            //IList<Api_EquipmentData> list = _dataApiService.GetElecDataByEquipmentIdAndMark(equipmentId, mark, time, flag);
            //switch (flag)
            //{ 
            //    case 1://天
            //        if (list.Count > 96)//数据过多，显示不完，按900s间隔取最高值
            //        {
            //            List<ChartData> resultData = new List<ChartData>();
            //            TimeSpan timeSpan = list[list.Count - 1].LastUpdateTime.ToDateTime() - list[0].LastUpdateTime.ToDateTime();
            //            int span = (int)timeSpan.TotalSeconds / 900;
            //            begin = list[0].LastUpdateTime.ToDateTime();
            //            end = begin.AddSeconds(900);
            //            for (int s = 0; s < span; s++)
            //            {
            //                //decimal sum = 0;
            //                //int num = 0;

            //                decimal highest = 0;

            //                for (int i = 0; i < list.Count; i++)
            //                {
            //                    if (list[i].LastUpdateTime >= begin && list[i].LastUpdateTime < end)
            //                    {
            //                        //sum +=  (decimal)list[i].Value;
            //                        //num++;

            //                        if (list[i].Value >= list[i + 1].Value)
            //                            highest = (decimal)list[i].Value;
            //                        else
            //                            highest = (decimal)list[i + 1].Value;
            //                    }
            //                }
            //                //resultData.Add(new ChartData() { time = end.ToString("yyyy-MM-dd HH:mm:ss"), value = num == 0 ? "0" : (sum / num).ToString("0.00") });

            //                resultData.Add(new ChartData() { time = end.ToString("yy-M-d h:m:s"), value = highest.ToString("0.00") });

            //                begin = end;
            //                end = begin.AddSeconds(900);
            //            }
            //            ChartData[] array = resultData.ToArray();
            //            return Json(array);
            //        }
            //        else//直接显示
            //        {
            //            ChartData[] array = new ChartData[list.Count];
            //            for (int i = 0; i < list.Count; i++)
            //            {
            //                array[i] = new ChartData() { time = ((DateTime)list[i].LastUpdateTime).ToString("yy-M-d h:m:s"), value = list[i].Value.ToString() };
            //            }
            //            return Json(array);
            //        }
                    
            //    case 2://月
            //    case 3://年
            //        ChartData[] array2 = new ChartData[list.Count];
            //        for (int i = 0; i < list.Count; i++)
            //        {
            //            array2[i] = new ChartData() { time = ((DateTime)list[i].LastUpdateTime).ToString("yy-M-d"), value = list[i].Value.ToString() };
            //        }
            //        return Json(array2);
            //}
            return Json(null);
        }

        [HttpPost]
        public string GetDate(string now, int flag, int value)
        {
            DateTime t = Convert.ToDateTime(now);
            switch (flag)
            {
                case 1:
                    t = t.AddDays(value);
                    break;
                case 2:
                    t = t.AddMonths(value);
                    break;
                case 3:
                    t = t.AddYears(value);
                    break;
            }
            return t.Year + "-" + t.Month + "-" + t.Day;
        }
	}
}