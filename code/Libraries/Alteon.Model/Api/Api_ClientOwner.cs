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
    /// Api_ClientOwner
    /// </summary>
    public partial class Api_ClientOwner : EntityBase
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
        /// 用户状态，1：正常；2：禁用
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
        /// <summary>
        /// 用户身份，1：超级管理员；2：普通用户
        /// </summary>
        public int UserIdentity { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Head { get; set; }
    }
}
