using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Model.Api.Extension
{
    public class EquipmentDataExt : Api_EquipmentData
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        public decimal? Value { get; set; }
        #region 只针对发电量
        /// <summary>
        /// 今日发电量
        /// </summary>
        public decimal TodayElectricity { get; set; }
        /// <summary>
        /// 总二氧化碳减排量
        /// </summary>
        public string CarbonDioxideReduce { get; set; }
        /// <summary>
        /// 总收入
        /// </summary>
        public string InComeMoney { get; set; }
        #endregion
    }

    public class EquipmentData : Api_EquipmentData
    {
        private decimal? _value;
        public decimal? Value 
        {
            get 
            {
                if (!string.IsNullOrEmpty(Method))
                {
                    string[] methods = Method.Split('+');
                    return _value * decimal.Parse(string.IsNullOrEmpty(methods[0])?"0":methods[0]) + decimal.Parse(methods[1]);
                }
                return _value;
            }
            set
            { _value = value; }
        }
        public DateTime? CreateTime { get; set; }
        public string EquipmentName { get; set; }
    }
}
