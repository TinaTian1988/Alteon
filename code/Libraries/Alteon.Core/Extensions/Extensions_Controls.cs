using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Alteon.Core.Extensions
{
    /// <summary>
    /// 扩展 控件类
    /// </summary>
    public static partial class Extensions_Controls
    {
        /// <summary>
        /// 把textarea内的换行转换成html表示的代码
        /// </summary>
        /// <param name="value">textarea内容</param>
        /// <returns></returns>
        public static string ToHtmlFromTextareaString(this string value)
        {
            if (true == value.AsNullOrWhiteSpace()) return "";
            return value.Replace("\r\n", "<br/>").Replace("\n\r", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>").Replace("\t", "　　");
        }

        /// <summary>
        /// 把html代码转换成textarea表示的内容
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStringFromTextareaHtml(this string value)
        {
            if (true == value.AsNullOrWhiteSpace()) return "";
            return value.Replace("<br/>", "\r\n").Replace("<br/>", "\n\r").Replace("<br/>", "\r").Replace("<br/>", "\n").Replace("　　", "\t");
        }

        /// <summary>
        /// 下拉列表绑定数据
        /// </summary>
        /// <param name="dropDownList">下列表框</param>
        /// <param name="dt">数据源</param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        public static void SetDropDownListDataBind(this System.Web.UI.WebControls.DropDownList dropDownList, DataTable dt, string dataTextField, string dataValueField)
        {
            if (null == dropDownList) return;

            if (null == dt) return;
            if (true == dataTextField.AsNullOrWhiteSpace()) return;
            if (true == dataValueField.AsNullOrWhiteSpace()) return;
            if (0 == dt.Rows.Count) return;

            dropDownList.DataSource = dt;
            dropDownList.DataTextField = dataTextField;
            dropDownList.DataValueField = dataValueField;
            dropDownList.DataBind();
        }

    }
}
