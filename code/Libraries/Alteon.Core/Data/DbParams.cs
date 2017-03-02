using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alteon.Core.Data
{
    public class DbParams
    {


        /// <summary>
        /// 关键字
        /// </summary>
        public string kwd
        {
            get { return this._kwd; }
            set { this._kwd = Common.CommFun.repSqlVal(value); }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string type
        {
            get { return this._type; }
            set { this._type = Common.CommFun.repSqlVal(value); }
        }

        /// <summary>
        /// 类别
        /// </summary>
        public int Class { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 访问级别
        /// </summary>
        public int grade { get; set; }

        /// <summary>
        /// 推荐
        /// </summary>
        public int ishot { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int pc { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public int ps { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public string sort
        {
            get { return _sort; }
            set { this._sort = Common.CommFun.repSqlVal(value); } 
        }

        /// <summary>
        /// 扩展搜索条件
        /// </summary>
        public string extname
        {
            get { return this._ext; }
            set { this._ext = Common.CommFun.repSqlVal(value); }
        }

        /// <summary>
        /// 扩展搜索值
        /// </summary>
        public string extvalue
        {
            get { return this._extval; }
            set { this._extval = Common.CommFun.repSqlVal(value); }
        }

        private string _kwd;
        private string _type;
        private string _sort;
        private string _ext;
        private string _extval;

        #region 增加选项by田甜2015-9-15
        private string _publicOption;
        private string _carefullyChosenArea;
        private string _typeName;
        /// <summary>
        /// 发布选项
        /// </summary>
        public string PublicOption 
        {
            get { return this._publicOption; }
            set { this._publicOption = Common.CommFun.repSqlVal(value); }
        }
        public string CarefullyChosenArea
        {
            get { return this._carefullyChosenArea; }
            set { this._carefullyChosenArea = Common.CommFun.repSqlVal(value); }
        }
        public string TypeName
        {
            get { return this._typeName; }
            set { this._typeName = Common.CommFun.repSqlVal(value); }
        }
        #endregion
    }
}