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
    /// Propaganda_User
    /// </summary>
    public partial class Propaganda_User : EntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 登录账号（手机号码）
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 微信openId
        /// </summary>
        public string WeiXinOpenId { get; set; }
        /// <summary>
        /// 国家代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 所属区域
        /// </summary>
        public int Area_Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 微信账号
        /// </summary>
        public string WeiXin { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 身份标识：1：用户；2：商家；3：管理员
        /// </summary>
        public Nullable<int> Style { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        public Nullable<DateTime> UpdateTime { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public Nullable<System.DateTime> RegisterTime { get; set; }
        /// <summary>
        /// 当前状态：0：正常；1：审核中；2：禁用
        /// </summary>
        public Nullable<int> State { get; set; }
        /// <summary>
        /// 名片
        /// </summary>
        public string BusinessCard { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPortrait { get; set; }
        /// <summary>
        /// 公司/介绍
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// Logo
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// 商户等级
        /// </summary>
        public Nullable<int> Level { get; set; }
    }
}