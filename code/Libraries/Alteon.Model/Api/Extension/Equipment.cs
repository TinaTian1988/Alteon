using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Model.Api.Extension
{
    public class Equipment
    {
        /// <summary>
        /// 设备id(tcp接收到的gatewayNo字段)
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 网关标识
        /// </summary>
        public int? GateNo { get; set; }
        /// <summary>
        /// 设备主人id
        /// </summary>
        public string Owner_Id { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 设备描述
        /// </summary>
        public string Intro { get; set; }
        /// <summary>
        /// 设备地理位置
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 设备排序
        /// </summary>
        public Nullable<int> Sorting { get; set; }
        /// <summary>
        /// 设备是否可控
        /// </summary>
        public bool IsControl { get; set; }
        /// <summary>
        /// 设备是否公开
        /// </summary>
        public bool IsPublic { get; set; }
        /// <summary>
        /// 设备当前状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 设备最后连接时间
        /// </summary>
        public Nullable<System.DateTime> ConectTime { get; set; }
    }
}
