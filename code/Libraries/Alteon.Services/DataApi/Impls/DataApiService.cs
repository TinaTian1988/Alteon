using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alteon.Core.Infrastructure.DependencyManagement;
using Alteon.Core.Data;
using Alteon.Model;

using Alteon.Core.Extensions;
using Alteon.Model.Api.Extension;
using System.Transactions;
using Alteon.Core.Caching;

namespace Alteon.Services.DataApi.Impls
{
    [AutoRegister(ComponentLifeStyle.LifetimeScope, ComponentPlatform.Default)]
    public class DataApiService:IDataApiService
    {
        public DataApiService() { }
        private readonly IDbSession _db;
        private readonly IRepository<Api_ClientOwner> _ownerDb;
        private readonly IRepository<Api_ClientEquipment> _equipmentDb;
        private readonly IRepository<Api_EquipmentData> _dataDb;

        public DataApiService(IDbSession db, IRepository<Api_ClientOwner> ownerDb,IRepository<Api_ClientEquipment> equipmentDb, IRepository<Api_EquipmentData> dataDb)
        {
            _db = db;
            _ownerDb = ownerDb;
            _equipmentDb = equipmentDb;
            _dataDb = dataDb;
        }
        #region 基础方法
        public void AddUserGuid(Api_ClientOwner user)
        {
            _db.ExecuteSqlCommand("insert Api_ClientOwner (Id,UserIdentity,RegisterTime) values (@Id,@UserIdentity,@RegisterTime)", new { @Id = user.Id, @UserIdentity = user.UserIdentity, @RegisterTime = user.RegisterTime });
        }


        /// <summary>
        /// 按照指定标识查找用户
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public Api_ClientOwner IsExistOwner(string ownerId = "", string phone = "")
        {
            if (!string.IsNullOrEmpty(ownerId))
                return _ownerDb.GetEntity(@"select * from Api_ClientOwner where Id=@Id", new { @Id = ownerId });
            if (!string.IsNullOrEmpty(phone))
                return _ownerDb.GetEntity(@"select * from Api_ClientOwner where Mobile=@Mobile", new { @Mobile = phone });
            return null;
        }
        /// <summary>
        /// 根据终端设备id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Api_ClientEquipment IsExistEquipment(int id, string name = "")
        {
            if (id > 0)
                return _equipmentDb.Get(id);
            if (!string.IsNullOrEmpty(name))
                return _equipmentDb.GetEntity("select * from Api_ClientEquipment where Name=@Name", new { @Name = name });
            return null;
        }

        public bool ChangeEquipmentDataState(int state, int equipmentDataId)
        {
            return _db.ExecuteSqlCommand("update Api_EquipmentData set State=@state where Id=@id", new { state = state, id = equipmentDataId }) > 0;
        }

        /// <summary>
        /// 增加设备
        /// </summary>
        /// <param name="equipment"></param>
        public void AddEquipment(Api_ClientEquipment equipment)
        {
            _equipmentDb.Insert(equipment);
        }
        public void UpdateEquipment(Api_ClientEquipment equipment)
        {
            _equipmentDb.Update(equipment);
        }
        public void DeleteEquipment(int equipmentId)
        {
            StringBuilder sb = new StringBuilder();
            var dataIds = _db.SqlQuery<int>("select Id from Api_EquipmentData where EquipmentId=@EquipmentId", new { @EquipmentId = equipmentId });
            if (dataIds != null && dataIds.Count() > 0)
            {
                foreach (var item in dataIds)
                {
                    sb.Append(item + ",");
                }
            }
            string dataId = sb.ToString().TrimEnd(',');
            sb = new StringBuilder();
            sb.AppendFormat("update api_clientequipment set IsDelete=1 where id={0}", equipmentId);
            sb.AppendFormat("update Api_EquipmentData set IsDelete=1 where EquipmentId={0}", equipmentId);
            sb.AppendFormat("delete Api_DataValue where EquipmentData_Id in({0})", dataId);
            using(var ts=new TransactionScope())
            {
                _db.ExecuteSqlCommand(sb.ToString());
                ts.Complete();
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data"></param>
        public void UpdateEquimentData(Api_EquipmentData model)
        {
            if (model.EquipmentId > 0)
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(model.Name))
                {
                    sb.Append(" Name=@Name,");
                }
                if(!string.IsNullOrEmpty(model.Unit))
                {
                    sb.Append(" Unit=@Unit,");
                }
                if (model.IsDelete != null)
                {
                    sb.Append(" IsDelete=@IsDelete,");
                }
                if (!string.IsNullOrEmpty(model.Method))
                {
                    sb.Append(" Method=@Method,");
                }
                if (model.MoneyMethod > 0)
                {
                    sb.Append(" MoneyMethod=@MoneyMethod,");
                }
                if (model.SortingIndex != null && model.SortingIndex >= 0)
                {
                    sb.Append(" SortingIndex=@SortingIndex,");
                }
                string updateSql = sb.ToString().TrimEnd(',');
                if (!string.IsNullOrEmpty(updateSql))
                {
                    _db.ExecuteSqlCommand(string.Format("update Api_EquipmentData set {0} where EquipmentId=@EquipmentId and Mark=@Mark", updateSql), new { @Name = model.Name, @Unit = model.Unit, @IsDelete = model.IsDelete, @EquipmentId = model.EquipmentId, @Mark = model.Mark, @Method = model.Method, @MoneyMethod = model.MoneyMethod, @SortingIndex=model.SortingIndex });
                }
            }
        }
        
        public void UpdateOwner(Api_ClientOwner owner)
        {
            _ownerDb.Update(owner);
        }
        public void InsertEquipmentData(Api_EquipmentData model)
        {
            model.IsDelete = false;
            _dataDb.Insert(model);
        }
        public Api_EquipmentData IsExistEquipmentData(int equipmentId, string mark)
        {
            return _db.SqlQuery<Api_EquipmentData>("select * from Api_EquipmentData where EquipmentId=@equipmentId and Mark=@mark", new { @equipmentId = equipmentId, @mark = mark }).FirstOrDefault();
        }
        #endregion

        #region 扩展方法

        /// <summary>
        /// 根据用户id，分页获取设备列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<Equipment> GetEquipmentAndDataByUserId(string userId, int pageIndex, int pageSize)
        {
            int startIndex = (pageIndex - 1) * pageSize;
            int endIndex = startIndex + pageSize;
            
            var equipmentList = _db.SqlQuery<Equipment>(@"select * from (select ROW_NUMBER() over (order by a.Sorting) as myindex,a.* from Api_ClientEquipment a where a.Owner_Id=@Owner_Id and a.IsDelete=0) as mytable where mytable.myindex>@indexStart and mytable.myindex<=@indexEnd", new { @Owner_Id = userId, @indexStart = startIndex, @indexEnd = endIndex });
            return equipmentList.ToList();
        }
        /// <summary>
        /// 根据用户id，获取设备列表的总数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetEquipmentAndDataByUserIdCount(string userId)
        {
            return _db.SqlQuery<int>("select count(*)from Api_ClientEquipment a where a.Owner_Id=@Owner_Id and a.IsDelete=0", new { @Owner_Id = userId }).FirstOrDefault();
        }
        /// <summary>
        /// 根据设备id获取设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Equipment GetEquipmentById(int id)
        {
            return _db.SqlQuery<Equipment>("select a.* from Api_ClientEquipment a where a.Id=@id", new { @id = id }).FirstOrDefault();
        }
        /// <summary>
        /// 根据设备id分页获取该设备数据
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<Api_EquipmentData> GetEquipmentDataByEquipmentId(int equipmentId,int pageIndex,int pageSize)
        {
            var list = _db.SqlQuery<Api_EquipmentData>(@"select a.Mark,a.Name,a.Unit,b.* from Api_EquipmentData a join 
(select * from (  
 select Id,EquipmentData_Id,Value,CreateTime,row_number() 
 over(partition by EquipmentData_Id order by CreateTime desc) rn  
 from Api_DataValue
) t where t.rn <=1) b
on a.Id=b.EquipmentData_Id 
and a.EquipmentId=@equipmentId
and a.IsDelete=0", new { @equipmentId = equipmentId });
            return list.ToList();
        }

        public Api_EquipmentData GetDataById(int id)
        {
            return _db.SqlQuery<Api_EquipmentData>("select * from Api_EquipmentData where Id=@Id", new { @Id = id }).FirstOrDefault();
        }

        /// <summary>
        /// 获取指定设备id指定标识的数据
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public IList<Alteon.Model.Api.Extension.EquipmentData> GetDataByEquipmentIdAndMark(int equipmentId, string mark)
        {
            return _db.SqlQuery<Alteon.Model.Api.Extension.EquipmentData>(@"select b.Id,a.EquipmentId,a.Mark,a.Name,a.State,a.NormalValue,a.Type,a.SortingIndex,a.Method,a.MoneyMethod,a.Unit,b.Value,b.CreateTime from Api_EquipmentData a join Api_DataValue b on a.Id=b.EquipmentData_Id and a.EquipmentId=@EquipmentId and a.Mark=@Mark and DATEDIFF(DD,b.CreateTime,GETDATE())=0", new { @EquipmentId = equipmentId, @Mark = mark }).ToList();
        }


        /// <summary>
        /// 获取设备数据
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public IList<Alteon.Model.Api.Extension.EquipmentData> GetEquipmentData(string ownerId)
        {
            IList<Alteon.Model.Api.Extension.EquipmentData> list = new List<Alteon.Model.Api.Extension.EquipmentData>();
            var ids = _db.SqlQuery<int>("select Id from Api_ClientEquipment where Owner_Id=@Owner_Id and IsDelete=0", new { @Owner_Id = ownerId });
            if (ids != null && ids.Count() > 0)
            {
                StringBuilder id = new StringBuilder();
                foreach (var item in ids)
                {
                    id.Append(item + ",");
                }
                list = _db.SqlQuery<Alteon.Model.Api.Extension.EquipmentData>(string.Format(@"select a.Id,a.EquipmentId, a.SortingIndex,a.State, c.Name EquipmentName,a.Mark,a.Name,b.Value,a.Unit,b.CreateTime,a.Method,a.MoneyMethod 
from Api_EquipmentData a join api_clientequipment c on a.EquipmentId=c.Id and a.EquipmentId in({0}) and a.IsDelete=0
left join (select *, ROW_NUMBER() over(partition by EquipmentData_Id order by CreateTime desc)rn  from Api_DataValue) b 
on a.Id=b.EquipmentData_Id and b.rn=1 and a.IsDelete=0", id.ToString().TrimEnd(','))).ToList();
            }
            return list;
        }

        public IList<Alteon.Model.Api.Extension.EquipmentData> GetAllDataByEquipmentIdAndMark(int equipmentId, string mark, ChartSearchParam param)
        {
            StringBuilder sb = new StringBuilder();
            if (param.currentOption == 1)//今天
            {
                switch (param.timeSpan)
                {
                    case FixedTimeSpan.CurrentHour:
                        sb.Append(" and DateDiff(dd,b.CreateTime,getdate())=0 and datepart(hh,b.CreateTime) = datepart(hh,getdate())");
                        break;
                    case FixedTimeSpan.LastHour:
                        sb.Append(" and DateDiff(dd,b.CreateTime,getdate())=0 and datepart(hh,b.CreateTime) = datepart(hh,getdate())-1");
                        break;
                    case FixedTimeSpan.Today:
                        sb.Append(" and DateDiff(dd,b.CreateTime,getdate())=0");
                        break;
                    default:
                        break;
                }
            }
            if (param.currentOption == 2)//日期筛选
            {
                sb.AppendFormat("and b.CreateTime>='{0}' and b.CreateTime <='{1}'", param.beginTime, param.endTime);
            }
            return _db.SqlQuery<Alteon.Model.Api.Extension.EquipmentData>(string.Format(@"select b.Id,a.EquipmentId,a.Mark,a.Name,a.State,a.NormalValue,a.Type,a.SortingIndex,a.Method,a.MoneyMethod,a.Unit,b.Value,b.CreateTime from Api_EquipmentData a join Api_DataValue b on a.Id=b.EquipmentData_Id and a.EquipmentId=@EquipmentId and a.Mark=@Mark {0}", sb.ToString()), new { @EquipmentId = equipmentId, @Mark = mark }).ToList();
        }


        public IEnumerable<Alteon.Model.Api.Extension.EquipmentData> GetAllDataByEquipmentIdAndMark(int equipmentId, string mark, ChartSearchParam param, int pageIndex, int pageSize)
        {
            int startIndex = (pageIndex - 1) * pageSize;
            int endIndex = startIndex + pageSize;
            string where = string.Empty;
            if (param.beginTime != null && param.endTime != null)
            {
                where += string.Format("and b.CreateTime>='{0}' and b.CreateTime<='{1}'", param.beginTime, param.endTime);
            }
            if (param.beginValue > 0 && param.endValue > 0)
            {
                where += string.Format(" and b.Value>={0} and b.Value<={1}", param.beginValue, param.endValue);
            }
            return _db.SqlQuery<Alteon.Model.Api.Extension.EquipmentData>(string.Format(@"select * from 
(select ROW_NUMBER() over (order by b.CreateTime desc) as myindex,b.Id,a.EquipmentId,a.Mark,a.Name,a.State,a.NormalValue,a.Type,a.SortingIndex,a.Method,a.MoneyMethod,a.Unit,b.Value,b.CreateTime  
from Api_EquipmentData a join Api_DataValue b
on a.Id=b.EquipmentData_Id
and a.EquipmentId=@equipmentId and a.Mark=@mark and a.IsDelete=0 {0}
) as mytable 
where mytable.myindex>@start and mytable.myindex<=@end", where), new { @equipmentId = equipmentId, @mark = mark, @start = startIndex, @end = endIndex });
        }

        public int GetAllDataByEquipmentIdAndMarkCount(int equipmentId, string mark, ChartSearchParam param)
        {
            string where = string.Empty;
            if (param.beginTime != null && param.endTime != null)
            {
                where += string.Format("and b.CreateTime>='{0}' and b.CreateTime<='{1}'", param.beginTime, param.endTime);
            }
            if (param.beginValue > 0 && param.endValue > 0)
            {
                where += string.Format(" and b.Value>={0} and b.Value<={1}", param.beginValue, param.endValue);
            }
            return _db.SqlQuery<int>(string.Format(@"select count(*)from Api_EquipmentData a left join Api_DataValue b
on a.Id=b.EquipmentData_Id
and a.EquipmentId=@equipmentId and a.Mark=@mark and a.IsDelete=0 {0}", where), new { @equipmentId = equipmentId, @mark = mark }).FirstOrDefault();
        }

        public EquipmentDataExt GetEquipmentDataByEquipmentIdAndMark(int equipmentId, string mark)
        {

            var data = _db.SqlQuery<EquipmentDataExt>(@"select c.Name EquipmentName, a.*,b.Value,b.CreateTime from Api_EquipmentData a join 
(select top 1 * from Api_DataValue where EquipmentData_Id=62 order by CreateTime desc) b on a.Id=b.EquipmentData_Id
and a.EquipmentId=5 and a.Mark='T1'
join Api_ClientEquipment c on a.EquipmentId=c.Id").FirstOrDefault();
            return data;
        }

        public decimal GetLastDayElectricity(int equipmentId, string mark)
        {
            return _db.SqlQuery<decimal>(@"select Value from Api_DataValue 
 where CreateTime=
(select max(b.CreateTime) from Api_EquipmentData a left join Api_DataValue b on a.Id=b.EquipmentData_Id
 and a.EquipmentId=5 and a.Mark='T1' 
and DATEDIFF(day,b.CreateTime,GETDATE())=1)").FirstOrDefault();
        }


        public IList<Api_EquipmentData> GetElecDataByEquipmentIdAndMark(int equipmentId, string mark, string strDatetime, int flag)
        {
            DateTime datetime = Convert.ToDateTime(strDatetime);
            DateTime begin = datetime;
            DateTime end = new DateTime(2099, 12, 31, 23, 59, 59);

            IList<Api_EquipmentData> result = new List<Api_EquipmentData>();

            switch (flag)
            {
                case 1://天
                    end = begin.AddDays(1);
                    return _db.SqlQuery<Api_EquipmentData>("select * from Api_EquipmentData where EquipmentId=@EquipmentId and Mark=@Mark and LastUpdateTime>=@begin and LastUpdateTime<@end", new { @EquipmentId = equipmentId, @Mark = mark, @begin = begin, @end = end }).ToList();

                case 2://月
                    end = begin;
                    begin = begin.AddMonths(-1);
                    TimeSpan span = end - begin;
                    int days = (int)span.TotalDays;
                    for (int i = 1; i <= days; i++)
                    {
                        end = begin.AddDays(1);
                        decimal? value = _db.SqlQuery<decimal?>("select MAX(value) from Api_EquipmentData where EquipmentId=58 and Mark='T1' and LastUpdateTime >= @begin and LastUpdateTime<@end", new { @EquipmentId = equipmentId, @Mark = mark, @begin = begin, @end = end }).FirstOrDefault();
                        if (value != null)
                        {
                            result.Add(new Api_EquipmentData()
                            {
                                //Value = value,
                                //LastUpdateTime = begin
                            });
                        }
                        begin = end;
                    }
                    return result;
                case 3://年
                    end = begin;
                    begin = begin.AddYears(-1);
                    for (int i = 1; i <= 12; i++)
                    {
                        end = begin.AddMonths(1);
                        decimal? value = _db.SqlQuery<decimal?>("select MAX(value) from Api_EquipmentData where EquipmentId=58 and Mark='T1' and LastUpdateTime >= @begin and LastUpdateTime<@end", new { @EquipmentId = equipmentId, @Mark = mark, @begin = begin, @end = end }).FirstOrDefault();
                        if (value != null)
                        {
                            //result.Add(new Api_EquipmentData()
                            //{
                            //    Value = value,
                            //    LastUpdateTime = begin
                            //});
                        }
                        begin = end;
                    }
                    return result;
            }
            return null;
        }

        public int DeleteDataByEquipmentIdAndMark(int eid,string mark)
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            int id = _db.SqlQuery<int>("select Id from Api_EquipmentData where EquipmentId=@eid and Mark=@mark", new { @eid = eid, @mark = mark.ToUpper() }).FirstOrDefault();
            sb.AppendFormat("update Api_EquipmentData set IsDelete=1 where EquipmentId={0} and Mark='{1}'", eid, mark.ToUpper());
            sb.AppendFormat("delete Api_DataValue where EquipmentData_Id={0}", id);
            using (var ts = new TransactionScope())
            {
                result = _db.ExecuteSqlCommand(sb.ToString());
                ts.Complete();
            }
            return result;
        }
        #endregion
    }
}
