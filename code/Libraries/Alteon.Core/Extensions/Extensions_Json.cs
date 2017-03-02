namespace Alteon.Core.Extensions
{
    using Alteon.Core.Enum;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Collections;
    using System.Web.Script.Serialization;

    /// <summary>
    /// 扩展 Json
    /// </summary>
    public static partial class Extensions_Json
    {

        /// <summary>
        /// DataTable To Json
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dateTimeFormat">日期格式化</param>
        /// <param name="columnsLowerOrUpper">是否把列名转换大小写，-1：不转换、0：转换小写、1：转换成大写；</param>
        /// <returns></returns>
        public static string ToJsonFromDataTable(this DataTable dt, JsonDateTimeFormat dateTimeFormat = JsonDateTimeFormat.IsoDateTimeLong, int columnsLowerOrUpper = 0)
        {
            if (null == dt) return "[]";

            if (columnsLowerOrUpper > -1)
            {
                if (dt.Columns.Count > 0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dt.Columns[i].ColumnName = 0 == columnsLowerOrUpper ? dt.Columns[i].ColumnName.ToLower() : dt.Columns[i].ColumnName.ToUpper();
                    }
                }
            }

            switch (dateTimeFormat)
            {
                case JsonDateTimeFormat.JavaScriptDateTime:
                    return JsonConvert.SerializeObject(dt, new DataTableConverter(), new JavaScriptDateTimeConverter());

                case JsonDateTimeFormat.IsoDateTime:
                    return JsonConvert.SerializeObject(dt, new DataTableConverter(), new IsoDateTimeConverter());

                case JsonDateTimeFormat.IsoDateTimeShort:
                    IsoDateTimeConverter timeConverter01 = new IsoDateTimeConverter();
                    timeConverter01.DateTimeFormat = "yyyy'-'MM'-'dd'";
                    return JsonConvert.SerializeObject(dt, new DataTableConverter(), timeConverter01);

                case JsonDateTimeFormat.IsoDateTimeLong:
                default:
                    IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
                    timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
                    return JsonConvert.SerializeObject(dt, new DataTableConverter(), timeConverter);
            }
        }

        /// <summary>
        /// DataTable To JqGridJson
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="records">总记录数</param>
        /// <param name="currPage">当前页码</param>
        /// <param name="pageTotal">总页数</param>\
        /// <param name="dateTimeFormat">日期格式化</param>
        /// <param name="columnsLowerOrUpper">是否把列名转换大小写，-1：不转换、0：转换小写、1：转换成大写；</param>
        /// <returns>{page:当前页,total:总页数,records:总记录数,rows:[数据列]}</returns>
        public static string ToJqGridJsonFromDataTable(this DataTable dt, int records = -1, int currPage = 1, int pageTotal = 1, JsonDateTimeFormat dateTimeFormat = JsonDateTimeFormat.IsoDateTimeLong, int columnsLowerOrUpper = 0, string userdata = "")
        {
            if (null == dt)
            {
                return "{page:0,total:0,records:0,rows:[]}";
            }
            if (records < 0)
            {
                records = dt.Rows.Count;
            }
            if (records < 1)
            {
                return "{page:0,total:0,records:0,rows:[]}";
            }
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.Append("\"page\":" + currPage + ",\"total\":" + pageTotal + ",\"records\":" + records + ",\"rows\"");
            jsonBuilder.Append(":");

            if (dt.Rows.Count > 0)
            {
                if (columnsLowerOrUpper > -1)
                {
                    if (dt.Columns.Count > 0)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            dt.Columns[i].ColumnName = 0 == columnsLowerOrUpper ? dt.Columns[i].ColumnName.ToLower() : dt.Columns[i].ColumnName.ToUpper();
                        }
                    }
                }

                switch (dateTimeFormat)
                {
                    case JsonDateTimeFormat.JavaScriptDateTime:
                        jsonBuilder.Append(JsonConvert.SerializeObject(dt, new DataTableConverter(), new JavaScriptDateTimeConverter()));
                        break;

                    case JsonDateTimeFormat.IsoDateTime:
                        jsonBuilder.Append(JsonConvert.SerializeObject(dt, new DataTableConverter(), new IsoDateTimeConverter()));
                        break;

                    case JsonDateTimeFormat.IsoDateTimeShort:
                        IsoDateTimeConverter timeConverter01 = new IsoDateTimeConverter();
                        timeConverter01.DateTimeFormat = "yyyy'-'MM'-'dd'";
                        jsonBuilder.Append(JsonConvert.SerializeObject(dt, new DataTableConverter(), timeConverter01));
                        break;

                    case JsonDateTimeFormat.IsoDateTimeLong:
                    default:
                        IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
                        timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
                        jsonBuilder.Append(JsonConvert.SerializeObject(dt, new DataTableConverter(), timeConverter));
                        break;
                }
            }
            else
            {
                jsonBuilder.Append("[]");
            }

            if (false == userdata.AsNullOrWhiteSpace())
            {
                jsonBuilder.Append(", \"userdata\":").Append(userdata);
            }

            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        #region =============
        /// <summary>
        /// Object To JqGridJson
        /// </summary>
        /// <param name="value"></param>
        /// <param name="records">总记录数</param>
        /// <param name="currPage">当前页码</param>
        /// <param name="pageTotal">总页数</param>\
        /// <param name="dateTimeFormat">日期格式化</param>
        /// <param name="columnsLowerOrUpper">是否把列名转换大小写，-1：不转换、0：转换小写、1：转换成大写；</param>
        /// <returns>{page:当前页,total:总页数,records:总记录数,rows:[数据列]}</returns>
        public static string ToJqGridJsonFromObject(this Object value, int records = -1, int currPage = 1, int pageTotal = 1, JsonDateTimeFormat dateTimeFormat = JsonDateTimeFormat.IsoDateTimeLong, int columnsLowerOrUpper = 0, string userdata = "")
        {
            if (null == value)
            {
                return "{page:0,total:0,records:0,rows:[]}";
            }

            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.Append("\"page\":" + currPage + ",\"total\":" + pageTotal + ",\"records\":" + records + ",\"rows\"");
            jsonBuilder.Append(":");

            switch (dateTimeFormat)
            {
                case JsonDateTimeFormat.JavaScriptDateTime:
                    jsonBuilder.Append(JsonConvert.SerializeObject(value, new JavaScriptDateTimeConverter()));
                    break;

                case JsonDateTimeFormat.IsoDateTime:
                    jsonBuilder.Append(JsonConvert.SerializeObject(value, new IsoDateTimeConverter()));
                    break;

                case JsonDateTimeFormat.IsoDateTimeShort:

                    IsoDateTimeConverter timeConverter01 = new IsoDateTimeConverter();
                    timeConverter01.DateTimeFormat = "yyyy-MM-dd";
                    jsonBuilder.Append(JsonConvert.SerializeObject(value, timeConverter01));
                    break;

                case JsonDateTimeFormat.IsoDateTimeLong:
                default:
                    IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
                    timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
                    jsonBuilder.Append(JsonConvert.SerializeObject(value, timeConverter));
                    break;
            }

            if (false == userdata.AsNullOrWhiteSpace())
            {
                jsonBuilder.AppendFormat(", \"userdata\":\"{0}\"", userdata);
            }

            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
        #endregion

        #region =============
        /// <summary>
        /// Object To JqGridJson
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dateTimeFormat">日期格式化</param>
        /// <returns>{page:当前页,total:总页数,records:总记录数,rows:[数据列]}</returns>
        public static string ToJson<T>(this T value, JsonDateTimeFormat dateTimeFormat = JsonDateTimeFormat.IsoDateTimeLong)
        {
            if (null == value)
            {
                return "{page:0,total:0,records:0,rows:[]}";
            }

            StringBuilder jsonBuilder = new StringBuilder();


            switch (dateTimeFormat)
            {
                case JsonDateTimeFormat.JavaScriptDateTime:
                    jsonBuilder.Append(JsonConvert.SerializeObject(value, new JavaScriptDateTimeConverter()));
                    break;

                case JsonDateTimeFormat.IsoDateTime:
                    jsonBuilder.Append(JsonConvert.SerializeObject(value, new IsoDateTimeConverter()));
                    break;

                case JsonDateTimeFormat.IsoDateTimeShort:
                    IsoDateTimeConverter timeConverter01 = new IsoDateTimeConverter();
                    timeConverter01.DateTimeFormat = "yyyy'-'MM'-'dd'";
                    jsonBuilder.Append(JsonConvert.SerializeObject(value, timeConverter01));
                    break;

                case JsonDateTimeFormat.IsoDateTimeLong:
                default:
                    IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
                    timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
                    jsonBuilder.Append(JsonConvert.SerializeObject(value, timeConverter));
                    break;
            }

            return jsonBuilder.ToString();
        }

        /// <summary>
        /// Object To JqGridJson
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dateTimeFormat">日期格式化</param>
        /// <returns>{page:当前页,total:总页数,records:总记录数,rows:[数据列]}</returns>
        public static string ToObjectJson(this Object value, JsonDateTimeFormat dateTimeFormat = JsonDateTimeFormat.IsoDateTimeLong)
        {
            if (null == value)
            {
                return "{page:0,total:0,records:0,rows:[]}";
            }

            StringBuilder jsonBuilder = new StringBuilder();


            switch (dateTimeFormat)
            {
                case JsonDateTimeFormat.JavaScriptDateTime:
                    jsonBuilder.Append(JsonConvert.SerializeObject(value, new JavaScriptDateTimeConverter()));
                    break;

                case JsonDateTimeFormat.IsoDateTime:
                    jsonBuilder.Append(JsonConvert.SerializeObject(value, new IsoDateTimeConverter()));
                    break;

                case JsonDateTimeFormat.IsoDateTimeShort:
                    IsoDateTimeConverter timeConverter01 = new IsoDateTimeConverter();
                    timeConverter01.DateTimeFormat = "yyyy'-'MM'-'dd'";
                    jsonBuilder.Append(JsonConvert.SerializeObject(value, timeConverter01));
                    break;

                case JsonDateTimeFormat.IsoDateTimeLong:
                default:
                    IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
                    timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
                    jsonBuilder.Append(JsonConvert.SerializeObject(value, timeConverter));
                    break;
            }

            return jsonBuilder.ToString();
        }
        #endregion

        public static T FromJson<T>(this string json)
            where T : new()
        {
            try
            {
                if (string.IsNullOrEmpty(json)) return new T();

                JavaScriptSerializer s = new JavaScriptSerializer();
                s.MaxJsonLength = int.MaxValue;
                T t = s.Deserialize<T>(json);
                if (t == null)
                {
                    return new T();
                }
                else
                {
                    return t;
                }
            }
            catch
            {
                return new T();
            }
        }
    }
}
