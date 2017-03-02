using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alteon.Model;

namespace Alteon.Services.HuaQiaoLianKao
{
    public interface IHuaQiaoLianKaoService
    {
        /// <summary>
        /// 获取Banner图列表
        /// </summary>
        /// <param name="isUse">是否可用：1为可用，0为不可用，如果为空则默认为1</param>
        /// <returns>Json字符串</returns>
        string GetBanners(int isUse = 1);

        /// <summary>
        /// 获取Article列表
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        /// <param name="isUse">是否可用：1为可用，0为不可用，如果为空则默认为1</param>
        /// <returns>Json字符串</returns>
        string GetArticles(string keyword, int type = 0, int isUse = 1);

        /// <summary>
        /// 根据id获取文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetArticleById(int id);
    }
}
