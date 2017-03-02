using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Model
{
    public class JsonStateResult
    {
        public JsonStateResult() 
        {
            Error = 1;
        }
        /// <summary>
        /// 错误代码:0:成功
        /// </summary>
        public int Error { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        public object Data2 { get; set; }

        public object Data3 { get; set; }
        public object Data4 { get; set; }
    }
}
