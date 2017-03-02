using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
    public class Api_ClientOwner
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户手机号码（也作为登录账号）
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 用户介绍
        /// </summary>
        public string Intro { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public Nullable<int> Status { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public Nullable<DateTime> RegisterTime { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }
    }

    public class Api_ClientEquipment
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<int> Category_Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Owner_Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Intro { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<int> Sorting { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<bool> IsControl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<bool> IsPublic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<int> Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<System.DateTime> ConectTime { get; set; }
    }

    public class Api_EquipmentData
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int EquipmentId { get; set; }
        /// <summary>
        /// 标识
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<System.DateTime> LastUpdateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<int> State { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NormalValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<int> SortingIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<bool> IsDelete { get; set; }

        public string Unit { get; set; }

        public string Method { get; set; }
        /// <summary>
        /// 只针对于发电量（计算发电收入的金额）
        /// </summary>
        public Nullable<decimal> MoneyMethod { get; set; }
    }
    /// <summary>
    /// 接收参数
    /// </summary>

    public class EquipmentData
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}
