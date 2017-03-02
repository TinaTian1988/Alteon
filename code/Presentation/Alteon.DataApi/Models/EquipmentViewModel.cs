using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alteon.Model.Api.Extension;

namespace Alteon.DataApi.Models
{
    public class EquipmentViewModel
    {
        public EquipmentViewModel(Equipment model)
        {
            this.Id = model.Id;
            this.GateNo = model.GateNo;
            this.Owner_Id = model.Owner_Id;
            this.Name = model.Name;
            this.Intro = model.Intro;
            this.Address = model.Address;
            this.Sorting = model.Sorting;
            this._isControl = model.IsControl;
            this._isPublic = model.IsPublic;
            this.Status = model.Status;
            this.ConectTime = model.ConectTime;
        }

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
        private Nullable<bool> _isControl;
        public string IsControl
        {
            get
            {
                if (_isControl != null && _isControl == true)
                    return "是";
                if (_isControl != null && _isControl == false)
                    return "否";
                else
                    return string.Empty;
            }
            set
            {
                _isControl = bool.Parse(value);
            }
        }
        /// <summary>
        /// 设备是否公开
        /// </summary>
        private Nullable<bool> _isPublic;
        public string IsPublic
        {
            get
            {
                if (_isPublic != null && _isPublic == true)
                    return "是";
                if (_isPublic != null && _isPublic == false)
                    return "否";
                else
                    return string.Empty;
            }
            set { _isPublic = bool.Parse(value); }
        }
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