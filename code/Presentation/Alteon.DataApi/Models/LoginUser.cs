using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alteon.DataApi.Models
{
    public class LoginUser
    {
        /// <summary>
        /// UserKey
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Head { get; set; }
        /// <summary>
        /// 用户身份
        /// </summary>
        public LoginUserIdentity UserIdentity { get; set; }
    }

    public enum LoginUserIdentity
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        SuperManager = 1,
        /// <summary>
        /// 普通用户
        /// </summary>
        NormalUser = 2
    }

    
}