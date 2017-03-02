using Alteon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alteon.Propaganda.Models
{
    public class BusinessUserVm : BaseUserVm
    {
        public BusinessUserVm(Propaganda_User model)
        {
            this.Id = model.Id;
            this.Account = model.Account;
            this.Password = model.Password;
            this.Name = model.Name;
            this.QQ = model.QQ;
            this.WeiXin = model.WeiXin;
            this.Address = model.Address;
            this.Telephone = model.Telephone;
            this.Email = model.Email;
            this.RegisterTime = model.RegisterTime;
            this.State = model.State;
            this.Level = model.Level;
        }

        
        /// <summary>
        /// 缴费开始时间
        /// </summary>
        public Nullable<System.DateTime> PayTimeStart { get; set; }
        /// <summary>
        /// 缴费结束时间
        /// </summary>
        public Nullable<System.DateTime> PayTimeEnd { get; set; }
        /// <summary>
        /// 缴费金额
        /// </summary>
        public Nullable<decimal> Amount { get; set; }
        
    }
}