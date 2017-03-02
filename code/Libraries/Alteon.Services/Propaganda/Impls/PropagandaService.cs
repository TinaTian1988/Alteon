using Alteon.Core.Data;
using Alteon.Core.Infrastructure.DependencyManagement;
using Alteon.Model;
using Alteon.Model.Propaganda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Alteon.Services.Propaganda.Impls
{
    [AutoRegister(ComponentLifeStyle.LifetimeScope, ComponentPlatform.Default)]
    public class PropagandaService : IpropagandaService
    {
        private readonly IDbSession _db;
        private readonly IRepository<Propaganda_User> _userDb;
        private readonly IRepository<Propaganda_Advertising> _adverDb;

        public PropagandaService(IDbSession db, IRepository<Propaganda_User> userDb, IRepository<Propaganda_Advertising> adverDb)
        {
            this._db = db;
            this._userDb = userDb;
            this._adverDb = adverDb;
        }
        

        public Propaganda_User ExistUser(Propaganda_User user)
        {
            StringBuilder sb = new StringBuilder("select * from Propaganda_User where 1=1 ");
            if(user.Id > 0)
            {
                sb.Append(" and Id=@Id");
            }
            if (!string.IsNullOrEmpty(user.Account))
            {
                sb.Append(" and Account=@Account");
            }
            if (!string.IsNullOrEmpty(user.WeiXinOpenId))
            {
                sb.Append(" and WeiXinOpenId=@WeiXinOpenId");
            }
            if(!string.IsNullOrEmpty(user.Code))
            {
                sb.Append(" and Code=@Code");
            }
            if (!string.IsNullOrEmpty(user.Name))
            {
                sb.Append(" and Name=@Name");
            }
            if (!string.IsNullOrEmpty(user.QQ))
            {
                sb.Append(" and QQ=@QQ");
            }
            if (!string.IsNullOrEmpty(user.WeiXin))
            {
                sb.Append(" and WeiXin=@WeiXin");
            }
            if (!string.IsNullOrEmpty(user.Address))
            {
                sb.Append(" and Address=@Address'");
            }
            if (!string.IsNullOrEmpty(user.Telephone))
            {
                sb.Append(" and Telephone=@Telephone");
            }
            if (!string.IsNullOrEmpty(user.Email))
            {
                sb.Append(" and Email=@Email");
            }
            if (user.RegisterTime != null)
            {
                sb.Append(" and RegisterTime=@RegisterTime");
            }
            if (user.State != null)
            {
                sb.Append(" and State=@State");
            }
            return _userDb.GetEntity(sb.ToString(), user);
        }

        public int AddUser(Propaganda_User user)
        {
            var model = _userDb.GetEntity("select * from Propaganda_User where Account=@Account", user);
            if (model != null && model.Id > 0)
            {
                model.Style = 2;
                _userDb.Update(model);//已存在的用户，则更新用户
            }
            else
            {
                _userDb.Insert(user);
            }
            return 0;
        }

        public bool UpdateUser(Propaganda_User user)
        {
            return _userDb.Update(user);
        }

        public int AddAdvertisments(Propaganda_Advertising model, string datas)
        {
            if(string.IsNullOrEmpty(datas))
            {
                return 1;//缺失图片路径
            }
            try
            {
                StringBuilder sb = new StringBuilder();
                //Propaganda_Advertising old = _adverDb.GetEntity("select * from Propaganda_Advertising where DATEDIFF(day,CreateTime,GETDATE())=0 and BusinessUser_Id=@BusinessUser_Id", model);
                model.CreateTime = DateTime.Now;
                //if (old != null)
                //{
                    //model.Id = old.Id;
                //}
                //else
                //{
                    _adverDb.Insert(model);
                //}
                var dataArray = datas.Split(new char[] { ',' });
                for (int i = 0; i < dataArray.Length; i++)
                {
                    sb.AppendFormat("insert Propaganda_AdverContent (Advertising_Id,[Content]) values ({0},'{1}')", model.Id, dataArray[i]);
                }
                _db.ExecuteSqlCommand(sb.ToString());
                return 0;
            }
            catch (Exception)
            {
                return 2;
            }
        }

        public IList<Propaganda_Advertising_Extension> GetAdvers(int bUserId)
        {
            var advers = _db.SqlQuery<Propaganda_AdverContent>(@"select a.*,b.Id CId,b.Advertising_Id,b.Content,b.Type from Propaganda_Advertising a join Propaganda_AdverContent b on a.Id=b.Advertising_id and a.BusinessUser_Id=@BusinessUser_Id order by a.CreateTime desc", new { @BusinessUser_Id = bUserId }).ToList();
            IList<Propaganda_Advertising_Extension> extension = new List<Propaganda_Advertising_Extension>();
            Propaganda_Advertising_Extension model = null;
            for (int i = 0; i < advers.Count; i++)
            {
                if (i > 0)
                {
                    if (advers[i - 1].Id == advers[i].Id)
                    {
                        if (model.Content.Count < 4)
                            model.Content.Add(new Propaganda_AdverContent() { CId = advers[i].CId, Advertising_Id = advers[i].Id, Content = advers[i].Content, Type = advers[i].Type });
                        continue;
                    }
                }
                model = new Propaganda_Advertising_Extension();
                model.Id = advers[i].Id;
                model.BusinessUser_Id = advers[i].BusinessUser_Id;
                model.Intro = advers[i].Intro;
                //model.Type = advers[i].Type;
                model.CreateTime = advers[i].CreateTime;
                if (model.Content == null)
                    model.Content = new List<Propaganda_AdverContent>();
                model.Content.Add(new Propaganda_AdverContent() { CId = advers[i].CId, Advertising_Id = advers[i].Id, Content = advers[i].Content, Type = advers[i].Type });
                extension.Add(model);
            }
            return extension;
        }

        public IList<Propaganda_AdverContent> GetImages(int adverId)
        {
            return _db.SqlQuery<Propaganda_AdverContent>("select a.Id CId,a.Advertising_Id,a.Content from Propaganda_AdverContent a join Propaganda_Advertising b on b.Id=a.Advertising_Id and b.Type=1 and a.Advertising_Id=@id", new { @id = adverId }).ToList();
        }

        public IEnumerable<Propaganda_User> GetAllUser()
        {
            return _userDb.GetList("select * from Propaganda_User where Style=2 and State=0");
        }

        public IEnumerable<Propaganda_User> GetUnvalidUser()
        {
            return _userDb.GetList("select * from Propaganda_User where Style=2 and State=1 and (Logo is not null or Logo != '')");
        }


        #region wifi 广告类别和广告管理
        public IList<Propaganda_ArticleCategory> GetArticleCategory()
        {
            return _db.SqlQuery<Propaganda_ArticleCategory>("select * from Propaganda_ArticleCategory order by Sort").ToList();
        }
        public bool DeleteArticleCategory(int id)
        {
            if (_db.ExecuteSqlCommand("delete Propaganda_ArticleCategory where Id=@id", new { @id = id }) > 0)
                return true;
            return false;
        }
        public bool AddArticleCategory(Propaganda_ArticleCategory model)
        {
            if (model != null && !string.IsNullOrEmpty(model.Name))
            {
                if (_db.ExecuteSqlCommand("insert Propaganda_ArticleCategory (Name,Sort) values (@Name,@Sort)", model) > 0)
                    return true;
            }
            return false;
        }
        public bool UpdateArticleCategory(Propaganda_ArticleCategory model)
        {
            if (model != null && model.Id > 0 && !string.IsNullOrEmpty(model.Name))
            {
                if (_db.ExecuteSqlCommand("update Propaganda_ArticleCategory set Name=@Name,Sort=@Sort where Id=@Id", model) > 0)
                    return true;
            }
            return false;
        }
        public IList<Propaganda_Article> GetArticle(int cid, string keyword, out int count, int pageIndex = 1, int pageSize = 5)
        {
            int startIndex = (pageIndex - 1) * pageSize;
            int endIndex = startIndex + pageSize;
            StringBuilder where = new StringBuilder();
            if (cid > 0)
            {
                where.AppendFormat(" and Category_Id={0}", cid);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                where.AppendFormat(" and (Title like '%{0}%' or SubTitle like '%{0}%' or Author like '%{0}%')", keyword);
            }
            count = _db.SqlQuery<int>(string.Format("select count(*) from Propaganda_Article where 1=1 {0}", where.ToString())).FirstOrDefault();
            return _db.SqlQuery<Propaganda_Article>(string.Format(@"select * from(select row_number() over(order by CreateTime desc) as myindex,Id,Title,SubTitle,Photo,Author,CreateTime from Propaganda_Article where 1=1 {0})as mytable where mytable.myindex>={1} and mytable.myindex<={2}", where.ToString(), startIndex, endIndex)).ToList();
        }

        public bool DeleteArticle(string ids)
        {
            if (_db.ExecuteSqlCommand("delete Propaganda_Article where Id in(@ids)", new { @ids = ids.TrimEnd(',') }) > 0)
                return true;
            return false;
        }

        public Propaganda_Article GetArticleById(int id)
        {
            return _db.SqlQuery<Propaganda_Article>("select * from Propaganda_Article where Id=@id", new { @id = id }).FirstOrDefault();
        }

        public int UpdateArticle(Propaganda_Article model)
        {
            return _db.ExecuteSqlCommand("update article set Category_Id=@Category_Id,Title=@Title,SubTitle=@SubTitle,Photo=@Photo,Author=@Author,Content=@Content where Id=@Id", model);
        }
        public int InsertArticle(Propaganda_Article model)
        {
            return _db.ExecuteSqlCommand("insert Propaganda_Article (Category_Id,Title,SubTitle,Photo,Author,Content,CreateTime) values (@Category_Id,@Title,@SubTitle,@Photo,@Author,@Content,@CreateTime)", model);
        }
        #endregion


        #region 微信小程序接口：ResidentialPlatform
        public Propaganda_User GetUserAndAdverByUserOpenId(string openId)
        {
            return _db.SqlQuery<Propaganda_User>("select * from Propaganda_User where WeiXinOpenId=@openId", new { openId = openId }).FirstOrDefault();
        }
        public IEnumerable<AdverExtension> GetAdverByUserId(int userId, out IList<string> images)
        {
            images = new List<string>();
            var ads = _db.SqlQuery<AdverExtension>("select * from Propaganda_Advertising where BusinessUser_Id=@userId order by CreateTime desc", new { userId = userId });
            var ids = ads.Select(a => a.Id.ToString());
            if (ids != null && ids.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in ids)
                {
                    sb.AppendFormat("{0},", item);
                }
                var contents = _db.SqlQuery<Propaganda_AdverContent>(string.Format("select * from Propaganda_AdverContent where Advertising_Id in ({0}) order by Id desc", sb.ToString().TrimEnd(',')));
                foreach (var a in ads)
                {
                    foreach (var c in contents)
                    {
                        if (c.Type == 1)
                        {
                            images.Add(c.Content);
                        }
                        if (a.Id == c.Advertising_Id)
                        {
                            if (a.Content == null)
                                a.Content = new List<Propaganda_AdverContent>();
                            a.Content.Add(c);
                        }
                    }
                }
            }
            return ads;
        }
        public int DeletAdverByAId(int aId)
        {
            return _db.ExecuteSqlCommand("delete Propaganda_Advertising where Id=@aId delete Propaganda_AdverContent where Advertising_Id=@aId", new { aId = aId });
        }
        public int UpdateLogo(string openId,string fileName)
        {
            return _db.ExecuteSqlCommand("update Propaganda_User set Logo=@logo,State=1,UpdateTime=GETDATE() where WeiXinOpenId=@openId", new { logo = fileName, openId = openId });//每次修改过logo后，状态都更改为待审核
        }
        public int UpdateUserIntroById(int userId, string company, string weixinAccount, string qq, string email, string address)
        {
            return _db.ExecuteSqlCommand("update Propaganda_User set company=@company,weixin=@weixin,qq=@qq,email=@email,address=@address where Id=@id", new { id = userId, company = company, weixin = weixinAccount, qq = qq, email = email, address = address });
        }
        public void AddAdverAndAdverContent(int userId, string desc, int type, string path, string url)
        {
            Propaganda_Advertising adver = _adverDb.GetEntity("select * from Propaganda_Advertising where BusinessUser_Id=@userId and Intro=@desc", new { userId = userId, desc = desc });
            if (adver == null)
            {
                adver = new Propaganda_Advertising() { BusinessUser_Id = userId, Intro = desc, CreateTime = DateTime.Now };
                _adverDb.Insert(adver);
            }
            var content = _db.SqlQuery<Propaganda_AdverContent>("select * from Propaganda_AdverContent where Advertising_Id=@aid and Content=@path", new { aid = adver.Id, path = path });
            if (content == null || content.Count() == 0)
            {
                _db.ExecuteSqlCommand("insert Propaganda_AdverContent (Advertising_Id,Type,Content,Description) values (@Advertising_Id,@Type,@Content,@Description)", new { Advertising_Id = adver.Id, Type = type, Content = path, Description = url });
            }
        }
        public IEnumerable<Propaganda_User> GetUserByAreaId(out int count, int areaId = 0, string keyword = "", int pageIndex = 1, int pageSize = 10)
        {
            int startIndex = (pageIndex - 1) * pageSize;
            int endIndex = startIndex + pageSize;
            StringBuilder where = new StringBuilder();
            if (areaId > 0)
            {
                where.Append(" and Area_Id=@areaId");
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                where.AppendFormat(" and (Account like '%{0}%' or Name like '%{0}%' or WeiXin like '%{0}%' or QQ like '%{0}%' or Address like '%{0}%' or Company like '%{0}%' or Telephone like '%{0}%' or Email like '%{0}%')", keyword);
            }
            count = _db.SqlQuery<int>("select count(*) from Propaganda_User where WeiXinOpenId != '' and State=0 " + where.ToString(), new { areaId = areaId }).FirstOrDefault();
            return _userDb.GetList("select * from(select ROW_NUMBER() over(order by Updatetime desc) myindex, * from Propaganda_User where WeiXinOpenId != '' and State=0 " + where.ToString() + ")t where t.myindex>@s and t.myindex<=@e", new { areaId = areaId, s = startIndex, e = endIndex });
        }
        public int UpdateUserInfo(string openId, string head, string name)
        {
            return _db.ExecuteSqlCommand("update Propaganda_User set Name=@name,HeadPortrait=@head where WeiXinOpenId=@openId", new { name = name, head = head, openId = openId });
        }
        public int ChangeUserState(int userId, int state)
        {
            return _db.ExecuteSqlCommand("update Propaganda_User set State=@state where Id=@userId", new { state = state, userId = userId });
        }
        public IEnumerable<Propaganda_Area> GetArea()
        {
            return _db.SqlQuery<Propaganda_Area>("select * from Propaganda_Area");
        }
        public int DeletePorpagandaUserById(string ids)
        {
            return _db.ExecuteSqlCommand(string.Format("delete Propaganda_User where Id in ({0})", ids));
        }
        public Propaganda_User GetUserById(int id)
        {
            return _userDb.Get(id);
        }

        public int AddPropagandaArea(Propaganda_Area model)
        {
            return _db.ExecuteSqlCommand("insert Propaganda_Area (Name,LatitudeStart,LatitudeEnd,LongitudeStart,LongitudeEnd) values (@Name,@LatitudeStart,@LatitudeEnd,@LongitudeStart,@LongitudeEnd)", model);
        }
        public int DeletePropagandaArea(string ids)
        {
            return _db.ExecuteSqlCommand("delete Propaganda_Area where Id in (@ids)", new { ids = ids });
        }
        public int UpdatePropagandaArea(Propaganda_Area model)
        {
            return _db.ExecuteSqlCommand("update Propaganda_Area set Name=@Name,LatitudeStart=@LatitudeStart,LatitudeEnd=@LatitudeEnd,LongitudeStart=@LongitudeStart,LongitudeEnd=@LongitudeEnd where Id=@Id", model);
        }
        public bool UpdatePropagandaUser(Propaganda_User model)
        {
            return _userDb.Update(model);
        }
        public IEnumerable<string> GetAllAreas()
        {
            return _db.SqlQuery<string>("select Name from Propaganda_Area");
        }
        public int SetUserArea(string area, string openId)
        {
            try
            {
                return _db.ExecuteSqlCommand("update Propaganda_User set Area_Id = (select Id from Propaganda_Area where Name=@area) where WeiXinOpenId=@openId", new { area = area, openId = openId });
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int GetAreaByPosition(decimal latitude, decimal longitude)
        {
            return _db.SqlQuery<int>("select Id from Propaganda_Area where LatitudeStart<=@latitude and LatitudeEnd>=@latitude and LongitudeStart<=@longitude and LongitudeEnd>=@longitude", new { latitude = latitude, longitude = longitude }).FirstOrDefault();
        }
        public IEnumerable<Propaganda_Banner> GetBanner()
        {
            return _db.SqlQuery<Propaganda_Banner>("select * from Propaganda_Banner order by SortIndex");
        }
        public int DeletePropagandaBannerById(int id)
        {
            return _db.ExecuteSqlCommand("delete Propaganda_Banner where Id=@id", new { id = id });
        }
        public int AddBanner(string url, int sort)
        {
            return _db.ExecuteSqlCommand("insert Propaganda_Banner (Url,SortIndex) values (@url,@sort)", new { url = url, sort = sort });
        }
        #endregion
    }
}
