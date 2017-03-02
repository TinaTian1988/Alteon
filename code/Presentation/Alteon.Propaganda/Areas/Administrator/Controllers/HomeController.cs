using Alteon.Core.Common;
using Alteon.Model;
using Alteon.Model.Propaganda;
using Alteon.Propaganda.Areas.Administrator.Filters;
using Alteon.Propaganda.Areas.Administrator.Helper;
using Alteon.Services.Propaganda;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alteon.Propaganda.Areas.Administrator.Controllers
{
    [NeedLogin]
    public class HomeController : Controller
    {
        private readonly IpropagandaService _propagandaUserService;
        public HomeController(IpropagandaService propagandaUserService)
        {
            this._propagandaUserService = propagandaUserService;
        }

        /// <summary>
        /// 母版页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region 商户管理
        public ActionResult PropagandaUser()
        {
            return View();
        }
        
        [HttpPost]
        public string AjaxGetPropagandaUser(int aid, string keyword, int pageIndex, int pageSize)
        {
            SimpleJsonResult jr = new SimpleJsonResult();
            int count = 0;
            var list = _propagandaUserService.GetUserByAreaId(out count, aid, keyword, pageIndex, pageSize);
            if (list != null && list.Count() > 0)
            {
                jr.status = 1;
                jr.data = list;
                jr.dataCount = count;
            }
            return JsonConverter.SerializeObject(jr);
        }
        [HttpPost]
        public JsonResult AjaxValidPropagandaUser(int userId, int state)
        {
            SimpleJsonResult jr = new SimpleJsonResult();
            if (_propagandaUserService.ChangeUserState(userId, state) > 0)
            {
                jr.status = 1;
            }
            return Json(jr);
        }
        [HttpPost]
        public JsonResult AjaxDeletePropogandaUser(string ids)
        {
            SimpleJsonResult jr = new SimpleJsonResult();
            if (_propagandaUserService.DeletePorpagandaUserById(ids.TrimEnd(',')) > 0)
            {
                jr.status = 1;
            }
            return Json(jr);
        }
        public ActionResult EditePropagandaUser(int id)
        {
            return View(_propagandaUserService.GetUserById(id));
        }
        [HttpPost]
        public ActionResult AjaxEditPropagandaUser(Propaganda_User model)
        {
            SimpleJsonResult r = new SimpleJsonResult();
            if (_propagandaUserService.UpdatePropagandaUser(model))
                return RedirectToAction("PropagandaUser");
            return Redirect("EditePropagandaUser?id="+model.Id);
        }
        #endregion

        #region 区域管理
        public ActionResult PropagandaArea()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AjaxGetPropagandaArea()
        {
            SimpleJsonResult jr = new SimpleJsonResult();
            var list = _propagandaUserService.GetArea();
            if (list != null && list.Count() > 0)
            {
                jr.status = 1;
                jr.data = list;
            }
            return Json(jr);
        }
        [HttpPost]
        public JsonResult AjaxEditPropagandaArea(Propaganda_Area model)
        {
            SimpleJsonResult r = new SimpleJsonResult();
            if ((model.Id > 0 && _propagandaUserService.UpdatePropagandaArea(model) > 0) || (model.Id <= 0 && _propagandaUserService.AddPropagandaArea(model) > 0))
            {
                r.status = 1;
            }
            return Json(r);
        }
        [HttpPost]
        public JsonResult DeletePropagandaAreaById(string ids)
        {
            SimpleJsonResult r = new SimpleJsonResult();
            if (_propagandaUserService.DeletePropagandaArea(ids.TrimEnd(',')) > 0)
            {
                r.status = 1;
            }
            return Json(r);
        }
        
        #endregion

        #region Banner管理
        public ActionResult PropagandaBanner()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AjaxGetPropagandaBanner()
        {
            JsonStateResult j = new JsonStateResult();
            var list = _propagandaUserService.GetBanner();
            if (list != null && list.Count() > 0)
            {
                j.Error = 0;
                j.Data = list;
            }
            return Json(j);
        }
        
        [HttpPost]
        public JsonResult AjaxDeletePropagandaBannerById(int id)
        {
            JsonStateResult j = new JsonStateResult();
            if (_propagandaUserService.DeletePropagandaBannerById(id) > 0)
            {
                j.Error = 0;
            }
            return Json(j);
        }
        #endregion
    }
}