using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.Common;
using System.Text.RegularExpressions;

using Alteon.Core.Enum;

namespace Alteon.Core.Extensions.LinqToSqlExtensions
{
    public static class TableExtensions
    {
        public static int Delete<TEntity>(this Table<TEntity> table, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            string tableName = table.Context.Mapping.GetTable(typeof(TEntity)).TableName;
            string command = String.Format("DELETE FROM {0}", tableName);

            ConditionBuilder conditionBuilder = new ConditionBuilder();
            conditionBuilder.Build(predicate.Body);

            if (false == conditionBuilder.Condition.AsNullOrWhiteSpace())
            {
                command += " WHERE " + conditionBuilder.Condition;
            }

            return table.Context.ExecuteCommand(command, conditionBuilder.Arguments);
        }

        #region ==========批量更新===========
        /*
        public static int Update<TEntity>(this Table<TEntity> table,
            Expression<Func<TEntity, TEntity>> evaluator, Expression<Func<TEntity, bool>> predicate, out string msg)
            where TEntity : class
        {
            //获取表名
            string tableName = table.Context.Mapping.GetTable(typeof(TEntity)).TableName;

            //查询条件表达式转换成SQL的条件语句
            ConditionBuilder builder = new ConditionBuilder();
            builder.Build(predicate.Body);
            string sqlCondition = builder.Condition;

            //获取Update的赋值语句
            var updateMemberExpr = (MemberInitExpression)evaluator.Body;
            var updateMemberCollection = updateMemberExpr.Bindings.Cast<MemberAssignment>().Select(c => new
            {
                Name = c.Member.Name,
                Value = ((ConstantExpression)c.Expression).Value
            });

            int i = builder.Arguments.Length;
            string sqlUpdateBlock = string.Join(", ", updateMemberCollection.Select(c => string.Format("[{0}]={1}", c.Name, "{" + (i++) + "}")).ToArray());

            //SQL命令
            string commandText = string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, sqlUpdateBlock, sqlCondition);

            

            //获取SQL参数数组 (包括查询参数和赋值参数)
            var args = builder.Arguments.Union(updateMemberCollection.Select(c => c.Value)).ToArray();

            msg = sqlCondition + "<br/>" + sqlUpdateBlock + "<br/>" + commandText+"<br/>";
            foreach (var m in args)
            {
                msg += m + "; ";
            }
            msg += "<br/>";
            foreach (var m in builder.Arguments)
            {
                msg += m + "; ";
            }

            return 0;
            //执行
            //return table.Context.ExecuteCommand(commandText, args);

        }
        */

        /// <summary>
        /// 更新全部
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="table"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static int Update<TEntity>(this Table<TEntity> table, Expression<Func<TEntity, TEntity>> evaluator) where TEntity : class
        {
            return Update(table, evaluator, o => true);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="TEntity">要操作的实体类</typeparam>
        /// <param name="table">源</param>
        /// <param name="evaluator">更新表达式</param>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public static int Update<TEntity>(this Table<TEntity> table, Expression<Func<TEntity, TEntity>> evaluator, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var bindings = ((System.Linq.Expressions.MemberInitExpression)(((System.Linq.Expressions.LambdaExpression)(evaluator)).Body)).Bindings;
            var custs = table.AsQueryable();
           ParameterExpression param = evaluator.Parameters[0];

           Expression allFilter = null;
           PropertyInfo firstMember = null;
           foreach (var b in bindings)
            {
                PropertyInfo member = (PropertyInfo)b.Member;
                var right = ((System.Linq.Expressions.MemberAssignment)(b)).Expression;
                if (right is System.Linq.Expressions.ConstantExpression)
                {
                    right = Expression.Constant(((ConstantExpression)right).Value, member.PropertyType);
                }

                if (firstMember == null)
               {
                    firstMember = member;
                }

                var left = Expression.Property(param, member);
                var filter = Expression.Equal(left, right);

                if (allFilter == null)
                {
                    allFilter = filter;
                }
                else
                {
                    var spilthFilter = Expression.Equal(Expression.Property(param, firstMember), Expression.Property(param, firstMember));
                    allFilter = Expression.And(allFilter, spilthFilter);
                    allFilter = Expression.And(allFilter, filter);
                }
            }

            List<DbParameter> parameters = new List<DbParameter>();
            var alias = "";
            var setStr = "";

            if (allFilter != null)
            {
                Expression pred = Expression.Lambda(allFilter, param);
                Expression expr = Expression.Call(typeof(Queryable), "Where", new Type[] { param.Type }, Expression.Constant(custs), pred);
                var query = table.AsQueryable().Provider.CreateQuery(expr);
                var cmd = table.Context.GetCommand(query);
                string sql = cmd.CommandText.Replace('\n', ' ').Replace('\r', ' ');
                var whereIndex = sql.ToUpper().IndexOf("WHERE ");
                setStr = sql.Substring(whereIndex + 6);
                alias = System.Text.RegularExpressions.Regex.Replace(sql.Substring(0, whereIndex).ToUpper(), ".* AS ", "");
                alias = System.Text.RegularExpressions.Regex.Replace(alias, @" |\[|\]", "");

                var pattern = string.Format(@"\)\s*AND\s*\(\s*\[{0}\]\s*.\s*\[{1}\]\s*=\s*\[{0}\]\s*.\s*\[{1}\]\s*\)\s*AND\s*\(", alias, firstMember.Name);
                setStr = System.Text.RegularExpressions.Regex.Replace(setStr, pattern, " , ", RegexOptions.IgnoreCase);
                setStr = System.Text.RegularExpressions.Regex.Replace(setStr, string.Format(@"\[{0}\].", alias), "", RegexOptions.IgnoreCase).Trim();
                if (setStr.StartsWith("("))
                {
                    setStr = setStr.Substring(1, setStr.Length - 2);
                }
                int pi = 0;
                foreach (DbParameter p in cmd.Parameters)
                {
                    var newName = "@setParam_" + pi++;
                    setStr = setStr.Replace(p.ParameterName, newName);
                    p.ParameterName = newName;
                    parameters.Add(p);
                }
               cmd.Parameters.Clear();
           }

            setStr = setStr.Trim(new char[] { ' ', ',' });

            var tableMapping = table.Context.Mapping.GetTable(param.Type);

            var whereCmd = table.Context.GetCommand(table.Where(predicate));
            var whereIndex2 = whereCmd.CommandText.ToUpper().IndexOf("WHERE");
            var whereStr = whereIndex2 > -1 ? whereCmd.CommandText.Replace('\r', ' ').Replace('\n', ' ').Substring(whereIndex2) : "";
            var updateStr = string.Format("UPDATE {0} SET {1} FROM {0} AS {2} {3}", tableMapping.TableName, setStr, alias, whereStr);

            whereCmd.CommandText = updateStr;
            whereCmd.Parameters.AddRange(parameters.ToArray());

            try
            {
                if (whereCmd.Connection.State != System.Data.ConnectionState.Open)
                {
                    whereCmd.Connection.Open();
                }
                return whereCmd.ExecuteNonQuery();
            }
            finally
            {
                whereCmd.Connection.Close();
                whereCmd.Dispose();
            }
        }

        #endregion

        /// <summary>
        /// Linq排序扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName">属性的字符串名称</param>
        /// <param name="sort">排序方式：ASC升序、DESC降序</param>
        /// <returns></returns>
        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string propertyName, string sort = "asc")
        {
            SortDirectionEnum tmpSort = sort.Equals("asc", StringComparison.CurrentCultureIgnoreCase) ? SortDirectionEnum.Ascending : SortDirectionEnum.Descending;
            return source.SortBy<T>(propertyName, tmpSort);
        }

        /// <summary>
        /// Linq排序扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName">属性的字符串名称</param>
        /// <param name="sort">方向</param>
        /// <returns></returns>
        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string propertyName, SortDirectionEnum sort)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (true == propertyName.AsNullOrWhiteSpace())
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, String.Empty);
            MemberExpression property = Expression.Property(parameter, propertyName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = (sort == SortDirectionEnum.Ascending) ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                                new Type[] { source.ElementType, property.Type },
                                                source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

    }
}
