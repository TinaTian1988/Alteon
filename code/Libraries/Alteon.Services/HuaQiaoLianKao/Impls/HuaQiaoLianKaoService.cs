using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alteon.Core.Infrastructure.DependencyManagement;
using Alteon.Core.Caching;
using Alteon.Core.Data;
using Alteon.Core.Extensions;
using Alteon.Model;


using Newtonsoft.Json;

namespace Alteon.Services.HuaQiaoLianKao.Impls
{
    [AutoRegister(ComponentLifeStyle.LifetimeScope, ComponentPlatform.Default)]
    public class HuaQiaoLianKaoService : IHuaQiaoLianKaoService
    {
        
        private readonly IDbSession _db;
        private readonly IRepository<Hqlk_Article> _db_hqlk_article;
        private readonly IRepository<Hqlk_Banner> _db_hqlk_banner;

        public HuaQiaoLianKaoService(IDbSession db, IRepository<Hqlk_Article> db_hqlk_article, IRepository<Hqlk_Banner> db_hqlk_banner)
        {
            _db = db;
            _db_hqlk_article = db_hqlk_article;
            _db_hqlk_banner = db_hqlk_banner;
        }

        /// <summary>
        /// 获取Banner图列表
        /// </summary>
        /// <param name="isUse">是否可用：1为可用，0为不可用，如果为空则默认为1</param>
        /// <returns>Json字符串</returns>
        public string GetBanners(int isUse = 1)
        {
            JsonStateResult jsonResult = new JsonStateResult();
            jsonResult.Error = -1001;
            jsonResult.Msg = "null";
            string sql = "select * from Hqlk_Banner where IsUse=@IsUse";
            Hqlk_Banner param = new Hqlk_Banner() { IsUse = isUse };
            var list = _db_hqlk_banner.GetList(sql,param);
            if (list != null && list.Count() > 0)
            {
                jsonResult.Error = 1000;
                jsonResult.Msg = "ok";
                list = list.OrderBy(a => a.SortingIndex);
                jsonResult.Data = list;
            }
            return JsonConvert.SerializeObject(jsonResult);;
        }

        /// <summary>
        /// 获取Article列表
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        /// <param name="isUse">是否可用：1为可用，0为不可用，如果为空则默认为1</param>
        /// <returns>Json字符串</returns>
        public string GetArticles(string keyword, int type = 0, int isUse = 1)
        {
            JsonStateResult jsonResult = new JsonStateResult();
            jsonResult.Error = -1001;
            jsonResult.Msg = "null";
            string sql = "select * from Hqlk_Article where 1=1 ";
            StringBuilder sbSql = new StringBuilder();
            Hqlk_Article model = new Hqlk_Article();
            model.IsUse = isUse;
            if (type == 1 || type == 2)
            {
                sbSql.Append("and Type=@Type ");
                model.Type = type;
            }
            if (keyword.AsNullOrWhiteSpace() == false)
            {
                sbSql.Append("and IsUse=@IsUse and Title like @Title or Description like @Description or Content like @Content ");
                model.Title = string.Format("%{0}%", keyword);
                model.Description = string.Format("%{0}%", keyword);
                model.Content = string.Format("%{0}%", keyword);
            }
            sbSql.Insert(0, sql);
            var list = _db_hqlk_article.GetList(sbSql.ToStr(), model);
            if (list != null && list.Count() > 0)
            {
                jsonResult.Error = 1000;
                jsonResult.Msg = "ok";
                list = list.OrderBy(a => a.SortingIndex);
                jsonResult.Data = list;
            }
            return JsonConvert.SerializeObject(jsonResult);
        }

        /// <summary>
        /// 根据id获取文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetArticleById(int id)
        {
            JsonStateResult jsonResult = new JsonStateResult();
            jsonResult.Error = -1001;
            jsonResult.Msg = "null";
            Hqlk_Article model = _db_hqlk_article.Get(id);
            if (model != null)
            {
                jsonResult.Error = 1000;
                jsonResult.Msg = "ok";
                jsonResult.Data = model;
            }

            return JsonConvert.SerializeObject(jsonResult);
        }
    }
}
