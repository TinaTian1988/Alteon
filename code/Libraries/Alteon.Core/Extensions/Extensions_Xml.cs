using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Alteon.Core.Extensions
{
    /// <summary>
    /// 扩展 Xml
    /// </summary>
    public static class Extensions_Xml
    {
        /// <summary>
        /// xml保存的主目录
        /// </summary>
        private const string XmlRoot = "~/App_Data/Xml/";

        #region ====== 序列化 ======
        /// <summary>
        /// 把对象序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string XmlToSerializer<T>(this T t) where T : class
        {
            string message;
            return t.XmlToSerializer<T>(out message);
        }
        /// <summary>
        /// 把对象序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string XmlToSerializer<T>(this T t, out string message) where T : class
        {
            if (null == t)
            {
                message = "要序列化的对象为NULL";
                return string.Empty;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                try
                {
                    serializer.Serialize(ms, t);
                    message = "序列化成功";
                    return Encoding.Default.GetString(ms.ToArray());
                }
                catch (XmlException ex)
                {
                    message = ex.Message;
                    return string.Empty;
                }
                finally
                {
                    ms.Close();
                }
            }
        }
        #endregion

        #region ====== Load ======
        /// <summary>
        /// 读取xml
        /// </summary>
        /// <param name="filePath">xml文件物理路径</param>
        /// <returns></returns>
        public static string XmlToLoad(this string filePath)
        {
            string messages;
            return filePath.XmlToLoad(out messages);
        }

        /// <summary>
        /// 读取xml
        /// </summary>
        /// <param name="filePath">xml文件物理路径</param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static string XmlToLoad(this string filePath, out string messages)
        {
            if (true == filePath.AsNullOrWhiteSpace())
            {
                messages = "文件物理路径为空";
                return string.Empty;
            }

            try
            {
                if (false == File.Exists(filePath))
                {
                    messages = "文件不存在";
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                messages = ex.Message;
                return string.Empty;
            }

            try
            {
                var doc = new XmlDocument();
                doc.Load(filePath);
                messages = "读取成功";
                return doc.InnerXml;
            }
            catch (XmlException ex)
            {
                messages = ex.Message;
                return string.Empty;
            }
            
        }

        /// <summary>
        /// 从 XML 字符串中还原对象 </summary>
        /// <param name="xmlString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T XmlToDeserialize<T>(this string xmlString) where T : class
        {
            string messages;
            return xmlString.XmlToDeserialize<T>(out messages);
        }
        /// <summary>
        /// 从 XML 字符串中还原对象 </summary>
        /// <param name="xmlString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T XmlToDeserialize<T>(this string xmlString, out string messages) where T : class
        {
            if (true == xmlString.AsNullOrWhiteSpace())
            {
                messages = "要反序列化的字符串为空";
                return null;
            }
            using (MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(xmlString)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                try
                {
                    messages = "反序列化成功";
                    return (T)serializer.Deserialize(ms);
                }
                catch(XmlException ex)
                {
                    messages = ex.Message;
                    return null;
                }
                finally
                {
                    ms.Close();
                }
            }
        }
        #endregion

        #region ====== 读数据 ======

        #endregion
    }
}
