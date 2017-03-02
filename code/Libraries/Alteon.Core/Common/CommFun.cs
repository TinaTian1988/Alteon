using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;

namespace Alteon.Core.Common
{
    public class CommFun
    {

        /// <summary>
        /// 替换Sql敏感字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string repSqlVal(string str)
        {
            return (str == null ? null : str.Replace("%", "").Replace("--", " ").Replace("'", "''"));
        }

        /// <summary>
        /// 读取应用设置
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string getAppSettings(string configName)
        {
            try
            {
                return ConfigurationManager.AppSettings[configName];
            }
            catch
            {
                return "";
            }
        }

        public static T getAppSettings<T>(string configName)
        {
            try
            {
                var v = ConfigurationManager.AppSettings[configName].ToString();
                return (T)Convert.ChangeType(v, typeof(T), CultureInfo.InvariantCulture);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 将此实体中成员变量转化为对象数组，便于Wcf等动态传参
        /// </summary>
        /// <returns></returns>
        public static object[] GetObjectMemberArray(object obj)
        {
            PropertyInfo[] members = obj.GetType().GetProperties();
            object[] objs = new object[members.Count()];
            for (int i = 0; i < members.Count(); i++)
            {
                objs[i] = members[i].GetValue(obj);
            }
            return objs;
        }

        /// <summary>
        /// 对字符串进行SHA1加密
        /// </summary>
        /// <param name="strIN">需要加密的字符串</param>
        /// <returns>密文</returns>
        public static string SHA1_Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();


        }


        public static string ToJson(object obj)
        {
            return JsonConverter.SerializeObject(obj);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static long GetTimeStamp(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetStampTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// base64编码的文本转存图片
        /// </summary>
        /// <param name="data">base64</param>
        /// <param name="fileName">转存文件完整路径</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string Base64StringToBitmap(string data, string fileName)
        {
            string filePath = string.Empty;
            try
            {
                string str = data.Substring(0, data.IndexOf(',')).Split('/')[1];
                string ext = str.Substring(0, str.IndexOf(';'));
                if (!ValidateImg(ext))
                {
                    return "请上传gif, jpg, png, bmp格式的图片";
                }
                data = data.Substring(data.IndexOf(',') + 1);
                byte[] arr = Convert.FromBase64String(data);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                bmp.Save(fileName, ImageFormat.Png);
                /*
                switch (ext)
                {
                    case "gif":
                        bmp.Save(fileName + ".gif", ImageFormat.Gif);
                        filePath = fileName + ".gif";
                        break;
                    case "png":
                        bmp.Save(fileName + ".png", ImageFormat.Png);
                        filePath = fileName + ".png";
                        break;
                    case "bmp":
                        bmp.Save(fileName + ".bmp", ImageFormat.Bmp);
                        filePath = fileName + ".bmp";
                        break;
                    case "jpg":
                    case "jpeg":
                        bmp.Save(fileName + ".jpg", ImageFormat.Jpeg);
                        filePath = fileName + ".jpg";
                        break;

                }
                */

                ms.Close();
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "图片转换失败";
            }
        }

        /// <summary>
        /// 查检图片格式
        /// </summary>
        /// <param name="extName"></param>
        /// <returns></returns>
        private static bool ValidateImg(string extName)
        {
            bool blean = false;
            string[] imgType = new string[] { "gif", "jpeg", "jpg", "png", "bmp" };
            for (int i = 0; i < imgType.Length; i++)
            {
                if (extName.ToLower().Equals(imgType[i]))
                {
                    blean = true;
                    break;
                }
                else
                {
                    continue;
                }
            }
            return blean;
        }

    }

}
