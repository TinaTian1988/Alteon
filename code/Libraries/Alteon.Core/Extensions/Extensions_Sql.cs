using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alteon.Core.Extensions
{
    /// <summary>
    /// 扩展 Dictionary
    /// </summary>
    public static partial class Extensions_Dictionary
    {
        /// <summary>
        /// 返回条件表达式 如: and colName = 'colValue'
        /// </summary>
        /// <param name="strSql">sql</param>
        /// <param name="colName">列</param>
        /// <param name="colValue">列</param>
        /// <returns></returns>
        public static string SqlWhereEquals(this string strSql, string colName, string colValue)
        {
            if (true == colName.AsNullOrWhiteSpace() || true == colValue.AsNullOrWhiteSpace()) return strSql;
            if (false == strSql.AsNullOrWhiteSpace())
            {
                if (false == strSql.TrimEnd().EndsWith("and", StringComparison.CurrentCultureIgnoreCase))
                {
                    strSql += " AND ";
                }
                
            }

            return string.Format(strSql + "{0} = '{1}'", colName, colValue);
        }

        /// <summary>
        /// 返回条件表达式 如: and colName = 'colValue'
        /// </summary>
        /// <param name="strSql">sql</param>
        /// <param name="colName">列</param>
        /// <param name="colValue">列</param>
        /// <returns></returns>
        public static string SqlWhereEquals(this string strSql, string colName, int colValue)
        {
            if (true == colName.AsNullOrWhiteSpace()) return strSql;
            if (false == strSql.AsNullOrWhiteSpace())
            {
                if (false == strSql.TrimEnd().EndsWith("and", StringComparison.CurrentCultureIgnoreCase))
                {
                    strSql += " AND ";
                }

            }

            return string.Format(strSql + "{0} = {1}", colName, colValue);
        }

        /// <summary>
        /// 返回条件表达式 如: and colName like '%colValue%'
        /// </summary>
        /// <param name="strSql">sql</param>
        /// <param name="colName">列</param>
        /// <param name="colValue">列</param>
        /// <returns></returns>
        public static string SqlWhereLike(this string strSql, string colName, string colValue)
        {
            if (true == colName.AsNullOrWhiteSpace() || true == colValue.AsNullOrWhiteSpace()) return strSql;
            if (false == strSql.AsNullOrWhiteSpace())
            {
                if (false == strSql.TrimEnd().EndsWith("and", StringComparison.CurrentCultureIgnoreCase))
                {
                    strSql += " AND ";
                }
            }

            return string.Format(strSql + "{0} like '%{1}%'", colName, colValue);
        }

    }
}
