using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using DapperExtensions;
using System.Transactions;
using Alteon.Model;

namespace Alteon.Monitor
{
    public class DbSession
    {
        private readonly string _connectionString = string.Empty;
        public DbSession()
        {
            _connectionString = "Data Source=119.29.116.201;Initial Catalog=Alteon;Persist Security Info=True;User ID=sa;Password=Mmatt5250;MultipleActiveResultSets=True";
        }


        public IDbConnection GetOpenConnection()
        {
            var result = new SqlConnection(_connectionString);
            result.Open();

            return result;
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, object parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var conn = this.GetOpenConnection())
            {
                return conn.Query<TElement>(sql, parameters, commandType: cmdType);
            }
        }

        public int ExecuteSqlCommand(string sql, object parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var conn = this.GetOpenConnection())
            {
                return conn.Execute(sql, parameters, commandType: cmdType);
            }
        }
    }
    public sealed class DataService
    {
        private static ObjectCache Cache = MemoryCache.Default;

        private static DbSession db = new DbSession();


        #region cache
        public static T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        public static void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public static bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }
        #endregion

        /// <summary>
        /// 根据指定条件查找是否有存在的用户
        /// </summary>
        /// <param name="ownerId">用户userkey</param>
        /// <param name="phone">用户手机号码</param>
        /// <returns></returns>
        public static Api_ClientOwner IsExistUser(string ownerId = "", string phone = "")
        {
            if (!string.IsNullOrEmpty(ownerId))
                return db.SqlQuery<Api_ClientOwner>(@"select * from Api_ClientOwner where Id=@Id", new { @Id = ownerId }).FirstOrDefault();
            if (!string.IsNullOrEmpty(phone))
                return db.SqlQuery<Api_ClientOwner>(@"select * from Api_ClientOwner where Mobile=@Mobile", new { @Mobile = phone }).FirstOrDefault();
            return null;
        }


        /// <summary>
        /// 根据设备Id和数据标识查找数据
        /// </summary>
        /// <param name="mark">标识</param>
        /// <returns></returns>
        public static Api_EquipmentData IsExistEquimentData(int equipmentId, string mark)
        {
            if (!string.IsNullOrEmpty(mark))
            {
                return db.SqlQuery<Api_EquipmentData>("select top 1 * from Api_EquipmentData where EquipmentId=@EquipmentId and Mark=@Mark", new { @EquipmentId = equipmentId, @Mark = mark }).FirstOrDefault();
            }
            return null;
        }
        /// <summary>
        /// 根据用户id和网关标识获取设备id
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="gateNo"></param>
        /// <returns></returns>
        public static int GetEquipmentId(string ownerId, int gateNo)
        {
            return db.SqlQuery<int>("select Id from Api_ClientEquipment where GateNo=@GateNo and Owner_Id=@Owner_Id", new { @GateNo = gateNo, @Owner_Id = ownerId }).FirstOrDefault();
        }

        static object mylock = new object();

        public static void UploadData(int equipmentId, List<EquipmentData> dataList)
        {
            lock (mylock)
            {
                DateTime dt = DateTime.Now;
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < dataList.Count; i++)
                {
                    var model = IsExistEquimentData(equipmentId, dataList[i].Name);
                    if (model == null)
                    {
                        continue;
                    }
                    sb.AppendFormat(" insert Api_DataValue (EquipmentData_Id,Value,CreateTime) values ({0},{1},'{2}')", model.Id, dataList[i].Value, dt);
                }
                sb.AppendFormat(" update Api_ClientEquipment set ConectTime='{0}' where Id={1}", dt, equipmentId);
                using (var ts = new TransactionScope())
                {
                    db.ExecuteSqlCommand(sb.ToString());
                    ts.Complete();
                }

            }

        }

    }
}
