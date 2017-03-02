using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alteon.Propaganda.Models
{

    public class BaseUserVm
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
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
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
        /// 注册时间
        /// </summary>
        public Nullable<System.DateTime> RegisterTime { get; set; }
        /// <summary>
        /// 账号当前状态：0：正常；1：停用
        /// </summary>
        public Nullable<int> State { get; set; }
        public Nullable<int> Level { get; set; }
    }
}