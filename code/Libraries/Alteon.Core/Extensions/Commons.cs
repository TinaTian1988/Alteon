namespace Alteon.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Drawing.Imaging;
using System.Xml;

    /// <summary>
    /// 常用方法扩展
    /// </summary>
    public static partial class Commons
    {
        /// <summary>
        /// 指示指定的字符串是null、空还是公由空白字符串组成
        /// </summary>
        /// <param name="value">要测试的字符串</param>
        /// <returns></returns>
        public static bool AsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 验证输入字符串是否是有效的Guid
        /// </summary>
        /// <param name="inputText">要测试的字符串</param>
        /// <returns>true|false</returns>
        public static bool AsGuid(this string value)
        {
            if (true == value.AsNullOrWhiteSpace()) return false;

            Guid tmp;
            return Guid.TryParse(value, out tmp);
        }

        /// <summary>
        /// 验证输入字符串是否是有效的System.Decimal 。
        /// </summary>
        /// <param name="value">要验证的字符串</param>
        /// <returns></returns>
        public static bool AsDecimal(this string value)
        {
            if (true == value.AsNullOrWhiteSpace()) return false;

            decimal tmp;
            return decimal.TryParse(value, out tmp);
        }

        /// <summary>
        /// 将字符串转换成等效的System.Decimal表示的形式 。如果转换失败则返回默认值(0)。
        /// </summary>
        /// <param name="value">要验证的字符串</param>
        /// <param name="defValue">转换失败返回的默认值</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value, decimal defValue = 0)
        {
            if (true == value.AsNullOrWhiteSpace()) return defValue;

            decimal tmp;
            if (false == decimal.TryParse(value, out tmp))
            {
                tmp = defValue;
            }
            return tmp;
        }

        /// <summary>
        /// 将字符串转换成等效的32位有符号整数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int ToInt(this string value, int defValue = 0)
        {
            if (true == value.AsNullOrWhiteSpace()) return defValue;

            int tmp;
            if (false == int.TryParse(value, out tmp))
            {
                tmp = defValue;
            }
            return tmp;
        }

        /// <summary>
        /// 将字符串转换成等效的8位有符号整数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static byte ToByte(this string value, byte defValue = 0)
        {
            if (true == value.AsNullOrWhiteSpace()) return defValue;

            byte tmp;
            if (false == byte.TryParse(value, out tmp))
            {
                tmp = defValue;
            }
            return tmp;
        }

        /// <summary>
        /// 将字符串转换成等效的16位有符号整数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static short ToShort(this string value, short defValue = 0)
        {
            if (true == value.AsNullOrWhiteSpace()) return defValue;

            short tmp;
            if (false == short.TryParse(value, out tmp))
            {
                tmp = defValue;
            }
            return tmp;
        }

        /// <summary>
        /// 把DataSet转换成DataTable
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(this DataSet ds)
        {
            if (null == ds) return null;
            return ds.Tables[0];
        }

        /// <summary>
        /// 将字符串转换成等效的布尔值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string value, bool defValue = false)
        {
            if (true == value.AsNullOrWhiteSpace()) return defValue;

            if (value.ToLower() == "true" || value == "1")
            {
                return true;
            }
            else if (value.ToLower() == "false" || value == "0")
            {
                return false;
            }

            bool tmp;
            if (false == bool.TryParse(value, out tmp))
            {
                tmp = defValue;
            }
            return tmp;
        }

        /// <summary>
        /// 验证字符串是否是日期格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AsDateTime(this string value)
        {
            if (true == value.AsNullOrWhiteSpace()) return false;
            DateTime tmp;
            return DateTime.TryParse(value, out tmp);

        }

        /// <summary>
        /// 转换成日期格式 如果转换失败则返回当前日期
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value)
        {
            if (true == value.AsNullOrWhiteSpace()) return DateTime.Now;
            DateTime tmp;
            if (false == DateTime.TryParse(value, out tmp))
            {
                tmp = DateTime.Now;
            }
            return tmp;
        }

        /// <summary>
        /// 虚拟路径转换成物理路径
        /// </summary>
        /// <param name="url">虚拟路径转</param>
        /// <returns></returns>
        public static string ToPhysicalPathFromUrl(this string url)
        {
            return System.Web.HttpContext.Current.Server.MapPath("" + url);
        }

        #region =============分页相关=============

        private static DataPage _dataPages;
        /// <summary>
        /// 分页相关
        /// </summary>
        public static DataPage DataPages
        {
            get
            {
                if (null == _dataPages)
                {
                    _dataPages = new DataPage();
                }
                return _dataPages;
            }
            set { _dataPages = value; }
        }

        #endregion

        /// <summary>
        /// unix时间转换为datetime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ToTimeFromUnixTime(this string timeStamp)
        {
            long lTime;
            if (false == long.TryParse(timeStamp + "0000000", out lTime))
            {
                lTime = 0;
            }
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ToDateTimeInt(this System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 生成13位时间数字
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ToLongDateTime(this System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }

        /// <summary>
        /// 获取当前请求的URL
        /// </summary>
        public static string GetRequestUrl
        {
            get { return System.Web.HttpContext.Current.Request.Url.ToString(); }
        }

        /// <summary>
        /// 把数组排序后返回字符串
        /// </summary>
        /// <param name="strArr">要排序的数组</param>
        /// <returns>排序后的字符串</returns>
        public static string GetArraySortToString(this string[] strArr)
        {
            if (null == strArr || strArr.Length == 0) return "";

            Array.Sort(strArr);
            return string.Join("", strArr);
        }

        /// <summary>
        /// 生成流水号
        /// </summary>
        /// <param name="val"></param>
        /// <param name="nobit">要生成多少位的流水号</param>
        /// <returns>如：00000001</returns>
        public static string SetNo(this int val, int nobit = 8)
        {
            if (nobit < 1) nobit = 8;

            string _bit = "";
            int tmpbit = 10;
            for (int i = 0; i < nobit; i++)
            {
                _bit += "0";
                tmpbit *= 10;
            }

            if (val < tmpbit)
            {
                return val.ToString(_bit);
            }
            return val.ToString();
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string TrimText(string s, int len)
        {
            string str = string.Empty;
            if (s != null)
            {
                if (s.Length > len)
                {
                    str = s.Substring(0, len) + "...";
                }
                else
                {
                    str = s;
                }
            }
            return str;
        }

        public static object IsNull(this object value, string origin, string replace)
        {
            if (replace == null) replace = string.Empty;

            if (value == null)
            {
                return string.Format(replace, value);
            }
            else
            {
                if (value is string)
                {
                    if (string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        return string.Format(replace, value);
                    }
                }
                return string.Format(origin, value);
            }
        }

        public static void CompressImage(Stream stream, Stream saveStream, int toWidth, int toHeight, ImageFormat format)
        {
            if (toWidth == 0 && toHeight == 0)
            {
                return;
            }

            Image img = Image.FromStream(stream);
            //压缩图片
            if (img.Width > toWidth)
            {
                float scale = (float)img.Width / img.Height;

                if (toWidth != 0 && toHeight == 0)
                {
                    toHeight = (int)(toWidth / scale);
                }
                else if (toWidth == 0 && toHeight != 0)
                {
                    toWidth = (int)(toHeight * scale);
                }

                Image newImage = new Bitmap(img, new Size(toWidth, toHeight));
                newImage.Save(saveStream, format);
            }
            else
            {
                img.Save(saveStream, format);
            }
        }

        public static void CompressImage(string filePath, Stream saveStream, int toWidth, int toHeight, ImageFormat format)
        {
            if (toWidth == 0 && toHeight == 0)
            {
                return;
            }

            Image img = Image.FromFile(filePath); //这里读取把文件读取到内存中，马上关闭文件流
            //压缩图片
            if (img.Width > toWidth || img.Height > toHeight)
            {
                float scale = (float)img.Width / img.Height;

                if (toWidth != 0 && toHeight == 0)
                {
                    toHeight = (int)(toWidth / scale);
                }
                else if (toWidth == 0 && toHeight != 0)
                {
                    toWidth = (int)(toHeight * scale);
                }

                Image newImage = new Bitmap(img, new Size(toWidth, toHeight));
                newImage.Save(saveStream, format);
            }
            else
            {
                img.Save(saveStream, format);
            }
        }

        public static void CompressImage(string filePath, string saveFilePath, int toWidth, int toHeight, ImageFormat format)
        {
            if (toWidth == 0 && toHeight == 0)
            {
                return;
            }

            Image img = Image.FromFile(filePath); //这里读取把文件读取到内存中，马上关闭文件流
            //压缩图片
            if (img.Width > toWidth)
            {
                float scale = (float)img.Width / img.Height;

                if (toWidth != 0 && toHeight == 0)
                {
                    toHeight = (int)(toWidth / scale);
                }
                else if (toWidth == 0 && toHeight != 0)
                {
                    toWidth = (int)(toHeight * scale);
                }

                Image newImage = new Bitmap(img, new Size(toWidth, toHeight));
                newImage.Save(saveFilePath, format);
            }
            else
            {
                img.Save(saveFilePath, format);
            }
        }

        public static void CompressImage(Stream stream, string saveFilePath, int toWidth, int toHeight, ImageFormat format)
        {
            if (toWidth == 0 && toHeight == 0)
            {
                return;
            }

            Image img = Image.FromStream(stream);
            //压缩图片
            if (img.Width > toWidth)
            {
                float scale = (float)img.Width / img.Height;

                if (toWidth != 0 && toHeight == 0)
                {
                    toHeight = (int)(toWidth / scale);
                }
                else if (toWidth == 0 && toHeight != 0)
                {
                    toWidth = (int)(toHeight * scale);
                }

                Image newImage = new Bitmap(img, new Size(toWidth, toHeight));
                newImage.Save(saveFilePath, format);
            }
            else
            {
                img.Save(saveFilePath, format);
            }
        }

        /// <summary>
        /// 扩展xml获取单点InnerText
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static string XmlSingleNodeInnerText(this XmlNode xmlNode, string defaultVal = "")
        {
            if (xmlNode == null) return defaultVal;
            return xmlNode.InnerText;
        }
    }
}
