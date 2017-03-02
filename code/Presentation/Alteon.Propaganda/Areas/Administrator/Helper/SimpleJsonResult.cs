using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alteon.Propaganda.Areas.Administrator.Helper
{
    public class SimpleJsonResult
    {
        public SimpleJsonResult()
        {
            this.status = 0;
        }
        /// <summary>
        /// 状态：0失败；1成功
        /// </summary>
        public int status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
        public int dataCount { get; set; }
    }
}