namespace Alteon.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 分页相关
    /// </summary>
    public class DataPage
    {
        #region =============分页相关=============
        private int _DataPage_Total = 0;
        /// <summary>
        /// 分页-数据总数
        /// </summary>
        public int DataPage_Total
        {
            get
            {
                if (_DataPage_Total < 1) _DataPage_Total = 0;

                return _DataPage_Total;
            }
            set
            {
                _DataPage_Total = value;
            }
        }

        private int _DataPage_PageSize = 25;
        /// <summary>
        /// 分页-每页显示数据的项数
        /// </summary>
        public int DataPage_PageSize
        {
            get { if (_DataPage_PageSize < 1) _DataPage_PageSize = 25; return _DataPage_PageSize; }
            set { _DataPage_PageSize = value; }
        }
        /// <summary>
        /// 分页-总页数
        /// </summary>
        public int DataPage_PageTotal
        {
            get
            {
                if (DataPage_Total > 0 && DataPage_Total > DataPage_PageSize)
                {
                    double p = DataPage_Total / DataPage_PageSize;
                    int tmp = Convert.ToInt32(Math.Ceiling(p));
                    if ((DataPage_Total % DataPage_PageSize) >= 1)
                        tmp += 1;
                    return tmp; // Convert.ToInt32(Math.Ceiling(p));
                }

                return 1;
            }
        }

        private int _DataPage_PageIndex = 1;
        /// <summary>
        /// 分页-当前页
        /// </summary>
        public int DataPage_PageIndex
        {
            get { if (_DataPage_PageIndex < 1) _DataPage_PageIndex = 1; return _DataPage_PageIndex; }
            set { _DataPage_PageIndex = value; }
        }
        /// <summary>
        /// 分页-当前页的数据起始索引值
        /// </summary>
        public int DataPage_StartIndex
        {
            get
            {
                if (DataPage_PageIndex > 1)
                {
                    return (DataPage_PageIndex - 1) * DataPage_PageSize;
                }
                return 0;
            }
        }

        /// <summary>
        /// 分页-当前页的数据结束索引值
        /// </summary>
        public int DataPage_EndIndex
        {
            get
            {
                if (DataPage_PageIndex < DataPage_PageTotal)
                {
                    return DataPage_PageIndex * DataPage_PageSize;
                }
                return DataPage_Total;
            }
        }
        #endregion
    }
}
