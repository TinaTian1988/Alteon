using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Model.Propaganda
{
    public class Propaganda_Area
    {
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 纬度范围开始值
        /// </summary>
        public decimal LatitudeStart { get; set; }
        /// <summary>
        /// 纬度范围结束值
        /// </summary>
        public decimal LatitudeEnd { get; set; }
        /// <summary>
        /// 经度范围开始值
        /// </summary>
        public decimal LongitudeStart { get; set; }
        /// <summary>
        /// 经度范围结束值
        /// </summary>
        public decimal LongitudeEnd { get; set; }
    }
}
