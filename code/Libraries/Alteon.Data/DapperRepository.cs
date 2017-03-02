using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

using Dapper;
using DapperExtensions;

using Alteon.Core.Data;
using Alteon.Core;

namespace Alteon.Data
{
    public class DapperRepository<TEntity> : IRepository<TEntity>
        where TEntity : EntityBase
    {
        private readonly IDbSession _dbSession;


        public DapperRepository(IDbSession dbSession)
        {
            _dbSession = dbSession;

//#if DEBUG
//            Debug.WriteLine("DbSession:{0} ", _dbSession.GetHashCode());
//#endif
        }

        public TEntity Get(int id)
        {
            using (var conn = _dbSession.GetOpenConnection())
                return conn.Get<TEntity>(id);
        }

        public TEntity GetEntity(string sql, object parameters = null)
        {
            using (var conn = _dbSession.GetOpenConnection())
                return conn.Query<TEntity>(sql, parameters).FirstOrDefault();
        }

        public void Insert(TEntity entity)
        {
            using (var conn = _dbSession.GetOpenConnection())
                conn.Insert(entity);
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            using (var conn = _dbSession.GetOpenConnection())
                conn.Insert(entities);
        }

        public bool Update(TEntity entity)
        {
            using (var conn = _dbSession.GetOpenConnection())
                return conn.Update(entity);
        }

        public bool Update(string sql, object parameters)
        {
            using (var conn = _dbSession.GetOpenConnection())
            {
                var effectCount = conn.Execute(sql, parameters);

                return effectCount > 0;
            }
        }

        public bool Delete(TEntity entity)
        {
            using (var conn = _dbSession.GetOpenConnection())
                return conn.Delete(entity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            using (var conn = _dbSession.GetOpenConnection())
                return conn.GetList<TEntity>(buffered: true);
        }

        public IEnumerable<TEntity> GetList(string sql, object parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var conn = _dbSession.GetOpenConnection())
                return conn.Query<TEntity>(sql, parameters, commandType: cmdType);
        }
    }
}
