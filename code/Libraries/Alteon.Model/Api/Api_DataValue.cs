//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Alteon.Model
{
     
    using System;
    using System.Collections.Generic;
    
    using Alteon.Core;
    
    /// <summary>
    /// 设备数据
    /// </summary>
    public partial class Api_DataValue : EntityBase
    {
        /// <summary>
        /// 数据id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 数据属性id
        /// </summary>
        public Nullable<int> EquipmentData_Id { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public Nullable<decimal> Value { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public Nullable<System.DateTime> CreateTime { get; set; }
    }
}
