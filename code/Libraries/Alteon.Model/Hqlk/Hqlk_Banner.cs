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

    using Alteon.Core;
    using System;
    using System.Collections.Generic;
    
    
    
    /// <summary>
    /// Hqlk_Banner
    /// </summary>
    [Serializable]
    public partial class Hqlk_Banner : EntityBase
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 排序索引
        /// </summary>
        public Nullable<int> SortingIndex { get; set; }
        /// <summary>
        /// 使用标志，1为正在使用；0为已删除
        /// </summary>
        public Nullable<int> IsUse { get; set; }
    }
}
