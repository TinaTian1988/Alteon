namespace Alteon.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;
    using System.Web.UI;

    /// <summary>
    /// 对象级别扩展类
    /// </summary>
    public static class ControlExt
    {
        /// <summary>
        /// 查所页面同一类别控件
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static IEnumerable<Control> FindAllControls(this Control ctrl)
        {
            if (ctrl == null)
                yield break;
            foreach (Control c in ctrl.Controls)
                yield return c;
            foreach (Control c in ctrl.Controls)
                foreach (Control cc in FindAllControls(c))
                    yield return cc;
        }
        
    }
}