using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Core.Data
{
    public interface IDbSession
    {
        string ConnectionString { get; }

        /// <summary>
        /// 获取一个"Opening"的数据库连接
        /// </summary>
        IDbConnection GetOpenConnection();

        IDbSession ResetDbSession(string connName);

        IEnumerable<TElement> SqlQuery<TElement>(string sql, object parameters = null, CommandType cmdType = CommandType.Text);

        int ExecuteSqlCommand(string sql, object parameters = null, CommandType cmdType = CommandType.Text);
    }
}
