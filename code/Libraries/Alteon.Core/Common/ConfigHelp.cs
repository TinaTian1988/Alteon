using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Reflection;
using Alteon.Core.Extensions;

namespace Alteon.Core.Common
{
    public static class ConfigHelp
    {
        private static XmlDocument configDocument;
        private static DateTime lastLoadTime = new DateTime(2000, 1, 1);
        private static bool isNew = false;
        //private static readonly string usingnow = ConfigurationManager.AppSettings["UsingNow"];

        static ConfigHelp()
        {
            Load();
        }

        /// <summary>
        /// 当配置文件被修改时重新读取，而不用重启iis
        /// </summary>
        public static void Load()
        {
            isNew = false;

            FileInfo fileInfo = new FileInfo(HttpContext.Current.Server.MapPath("~/Config/Customer.config"));
            if (lastLoadTime < fileInfo.LastWriteTime)
            {
                configDocument = new XmlDocument();
                configDocument.Load(fileInfo.FullName);
                lastLoadTime = DateTime.Now;
                isNew = true;
            }
        }

        /// <summary>
        /// 获取节点中的文本
        /// </summary>
        /// <param name="path">指定路径，例如："/config/website"</param>
        /// <returns></returns>
        public static string GetNodeText(string path)
        {
            try
            {
                Load();
                return configDocument.DocumentElement.SelectSingleNode(path).InnerText;
            }
            catch
            {
                return string.Empty;
            }
        }
       
        /// <summary>
        /// 获取指定路径中属性值
        /// </summary>
        /// <param name="path">指定路径，例如："/config/website"</param>
        /// <param name="attribute">属性名称</param>
        /// <returns></returns>
        public static string GetAttributesValue(string path, string attribute)
        {
            try
            {
                Load();
                return configDocument.DocumentElement.SelectSingleNode(path).Attributes[attribute].Value;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}