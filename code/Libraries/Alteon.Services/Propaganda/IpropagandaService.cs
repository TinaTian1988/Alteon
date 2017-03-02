using Alteon.Model;
using Alteon.Model.Propaganda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Services.Propaganda
{
    public interface IpropagandaService
    {
        Propaganda_User ExistUser(Propaganda_User user);

        int AddUser(Propaganda_User user);

        bool UpdateUser(Propaganda_User user);

        int AddAdvertisments(Propaganda_Advertising model, string datas);

        IList<Propaganda_Advertising_Extension> GetAdvers(int bUserId);

        IList<Propaganda_AdverContent> GetImages(int adverId);

        IEnumerable<Propaganda_User> GetAllUser();

        IEnumerable<Propaganda_User> GetUnvalidUser();

        #region wifi 广告类别和广告管理
        IList<Propaganda_ArticleCategory> GetArticleCategory();
        bool DeleteArticleCategory(int id);
        bool AddArticleCategory(Propaganda_ArticleCategory model);
        bool UpdateArticleCategory(Propaganda_ArticleCategory model);
        IList<Propaganda_Article> GetArticle(int cid, string keyword, out int count, int pageIndex = 1, int pageSize = 5);
        bool DeleteArticle(string ids);
        Propaganda_Article GetArticleById(int id);
        int UpdateArticle(Propaganda_Article model);
        int InsertArticle(Propaganda_Article model);
        #endregion

        #region 微信小程序接口：ResidentialPlatform
        Propaganda_User GetUserAndAdverByUserOpenId(string openId);
        IEnumerable<AdverExtension> GetAdverByUserId(int userId, out IList<string> images);
        int DeletAdverByAId(int aId);
        int UpdateLogo(string openId, string fileName);
        int UpdateUserIntroById(int userId, string company, string weixinAccount, string qq, string email, string address);
        void AddAdverAndAdverContent(int userId, string desc, int type, string path, string url);
        IEnumerable<Propaganda_User> GetUserByAreaId(out int count, int areaId = 0, string keyword = "", int pageIndex = 1, int pageSize = 10);
        int UpdateUserInfo(string openId, string head, string name);
        int ChangeUserState(int userId, int state);
        IEnumerable<Propaganda_Area> GetArea();
        int DeletePorpagandaUserById(string ids);
        Propaganda_User GetUserById(int id);
        int AddPropagandaArea(Propaganda_Area model);
        int DeletePropagandaArea(string ids);
        int UpdatePropagandaArea(Propaganda_Area model);
        bool UpdatePropagandaUser(Propaganda_User model);
        IEnumerable<string> GetAllAreas();
        int SetUserArea(string area, string openId);
        int GetAreaByPosition(decimal latitude, decimal longitude);
        IEnumerable<Propaganda_Banner> GetBanner();
        int DeletePropagandaBannerById(int id);
        int AddBanner(string url, int sort);
        #endregion
    }
}
