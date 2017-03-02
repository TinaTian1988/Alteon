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
    /// Hqlk_Article
    /// </summary>
    [Serializable]
    public partial class Hqlk_Article : EntityBase
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 类型，1为文章；2为政策
        /// </summary>
        public Nullable<int> Type { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 排序索引
        /// </summary>
        public Nullable<int> SortingIndex { get; set; }
        /// <summary>
        /// 首页展示标志，1为展示；0为不展示
        /// </summary>
        public Nullable<int> IsIndexPageShow { get; set; }
        /// <summary>
        /// 使用标志，1为正在使用；0为已删除
        /// </summary>
        public Nullable<int> IsUse { get; set; }
    }
}
