using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using Dapper;
using DapperExtensions;

using Alteon.Core.Data;

namespace Alteon.Data
{
    public sealed class DbSession : IDbSession
    {
        private readonly string _connectionString = string.Empty;

        public DbSession(string connName)
        {
            if (ConfigurationManager.ConnectionStrings[connName]!=null)
                _connectionString = ConfigurationManager.ConnectionStrings[connName].ConnectionString;
        }

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public IDbSession ResetDbSession(string connName)
        {
            DbSession dbs = new DbSession(connName);
            if (string.IsNullOrEmpty(dbs._connectionString)) dbs = this;
            return dbs;
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
}
