using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Core.Data
{
    public class DbSort
    {
        public string PropertyName { get; set; }

        public bool Ascending { get; set; }

        /// <summary>
        /// 排序数组转SQL
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static string markSortScript(IList<DbSort> sort)
        {
            if (sort == null) return "";
            StringBuilder sort_sql = new StringBuilder();
            if (sort != null && sort.Count > 0)
            {
                sort_sql.Append(" order by ");
                var i = 0;
                foreach (var s in sort)
                {
                    sort_sql.Append((i > 0 ? "," : "") + "[" + s.PropertyName + "]" + (s.Ascending ? " asc" : " desc"));
                    i++;
                }
            }
            return sort_sql.ToString();
        }
    }
}
