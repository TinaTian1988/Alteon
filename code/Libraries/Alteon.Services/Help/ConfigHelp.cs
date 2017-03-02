using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Web;
using Alteon.Core.Extensions;

namespace Alteon.Services.Help
{
    public static class ConfigHelp
    {
        private static XmlDocument configDocument;
        private static DateTime lastLoadTime = new DateTime(2000, 1, 1);
        private static bool isNew = false;
        private static readonly string usingnow = ConfigurationManager.AppSettings["UsingNow"];

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

            FileInfo fileInfo = new FileInfo(HttpContext.Current.Server.MapPath("~/Config/" + usingnow + ".config"));
            if (configDocument == null || lastLoadTime < fileInfo.LastWriteTime)
            {
                configDocument = new XmlDocument();
                configDocument.Load(fileInfo.FullName);
                lastLoadTime = DateTime.Now;
                isNew = true;
            }
        }
        public static string Tel
        {
            get
            {
                try
                {
                    Load();
                    return configDocument.DocumentElement.SelectSingleNode("/config/tel").InnerText;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>网站中文名</summary>
        public static string WebSiteName
        {
            get
            {
                try
                {
                    Load();
                    return configDocument.DocumentElement.SelectSingleNode("/config/website").Attributes["name"].Value;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 网站域名
        /// </summary>
        public static string Domain
        {
            get
            {
                try
                {
                    Load();
                    return configDocument.DocumentElement.SelectSingleNode("/config/Domain").InnerText;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        #region ============= 微信配置 =============
        /// <summary>
        /// 微信配置
        /// </summary>
        public static class WeiXin
        {
            public static string grant_type
            {
                get
                {
                    try
                    {
                        Load();
                        return configDocument.DocumentElement.SelectSingleNode("/config/weixin/grant_type").InnerText;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }

            public static string appid
            {
                get
                {
                    try
                    {
                        Load();
                        return configDocument.DocumentElement.SelectSingleNode("/config/weixin/appid").InnerText;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }

            public static string appsecret
            {
                get
                {
                    try
                    {
                        Load();
                        return configDocument.DocumentElement.SelectSingleNode("/config/weixin/appsecret").InnerText;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }

            public static string url
            {
                get
                {
                    try
                    {
                        Load();
                        return configDocument.DocumentElement.SelectSingleNode("/config/weixin/url").InnerText;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }

            public static string token
            {
                get
                {
                    try
                    {
                        Load();
                        return configDocument.DocumentElement.SelectSingleNode("/config/weixin/token").InnerText;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }
            public static string jsApiList
            {
                get
                {
                    try
                    {
                        Load();
                        return configDocument.DocumentElement.SelectSingleNode("/config/weixin/jsApiList").InnerText;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }
            /// <summary>
            /// 没有关注时跳转到的页面地址
            /// </summary>
            public static string WaitSubscribePageUrl
            {
                get
                {
                    try
                    {
                        Load();
                        return configDocument.DocumentElement.SelectSingleNode("/config/weixin/waitsubscribepageurl").InnerText;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }
            /// <summary>
            /// 关注时跳转到的页面地址
            /// </summary>
            public static string SubscribePageUrl
            {
                get
                {
                    try
                    {
                        Load();
                        return configDocument.DocumentElement.SelectSingleNode("/config/weixin/subscribepageurl").InnerText;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }
            /// <summary>
            /// 关注时的欢迎语-标题
            /// </summary>
            public static string SubscribeWelcome
            {
                get
                {
                    try
                    {
                        Load();
                        return configDocument.DocumentElement.SelectSingleNode("/config/weixin/subscribewelcome").InnerText;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }
            /// <summary>
            /// 关注时的欢迎语-详细描述
            /// </summary>
            public static string SubscribeDescription
            {
                get
                {
                    try
                    {
                        Load();
                        return configDocument.DocumentElement.SelectSingleNode("/config/weixin/subscribedescription").InnerText;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }

            /// <summary>
            /// 关注时的欢迎语-显示图片地址
            /// </summary>
            public static string SubscribePicUrl
            {
                get
                {
                    try
                    {
                        Load();
                        return configDocument.DocumentElement.SelectSingleNode("/config/weixin/subscribepicurl").InnerText;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }
            /// <summary>
            /// 关注时的欢迎语-点击时跳转到的界面
            /// </summary>
            public static string SubscribeUrl
            {
                get
                {
                    try
                    {
                        Load();
                        return configDocument.DocumentElement.SelectSingleNode("/config/weixin/subscribeurl").InnerText;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }
            public static bool debug
            {
                get
                {
                    try
                    {
                        Load();
                        return Convert.ToBoolean(configDocument.DocumentElement.SelectSingleNode("/config/weixin/debug").InnerText);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            /// <summary>
            /// 调用微信接口时间间隔
            /// </summary>
            public static int expires
            {
                get
                {
                    try
                    {
                        Load();
                        return Convert.ToInt32(configDocument.DocumentElement.SelectSingleNode("/config/weixin/expires").InnerText);
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
            public static class Huong
            {
                /// <summary>
                /// 点击活动-标题
                /// </summary>
                public static string SubscribeWelcome
                {
                    get
                    {
                        try
                        {
                            Load();
                            return configDocument.DocumentElement.SelectSingleNode("/config/weixin/Huong/subscribewelcome").InnerText;
                        }
                        catch
                        {
                            return string.Empty;
                        }
                    }
                }
                /// <summary>
                /// 点击活动-详细描述
                /// </summary>
                public static string SubscribeDescription
                {
                    get
                    {
                        try
                        {
                            Load();
                            return configDocument.DocumentElement.SelectSingleNode("/config/weixin/Huong/subscribedescription").InnerText;
                        }
                        catch
                        {
                            return string.Empty;
                        }
                    }
                }

                /// <summary>
                /// 点击活动-显示图片地址
                /// </summary>
                public static string SubscribePicUrl
                {
                    get
                    {
                        try
                        {
                            Load();
                            return configDocument.DocumentElement.SelectSingleNode("/config/weixin//Huong/subscribepicurl").InnerText;
                        }
                        catch
                        {
                            return string.Empty;
                        }
                    }
                }
                /// <summary>
                /// 点击活动-点击时跳转到的界面
                /// </summary>
                public static string SubscribeUrl
                {
                    get
                    {
                        try
                        {
                            Load();
                            return configDocument.DocumentElement.SelectSingleNode("/config/weixin//Huong/subscribeurl").InnerText;
                        }
                        catch
                        {
                            return string.Empty;
                        }
                    }
                }
            }
        }

        #endregion

        #region ============= 短信签名 =============
        /// <summary>
        /// 短信签名【顾问易】【安盈汇】之类的
        /// </summary>
        public static class SMS
        {
            public static string Sign
            {
                get
                {
                    Load();
                    return configDocument.DocumentElement.SelectSingleNode("/config/sms").Attributes["sign"].Value;
                }
            }

            public static int channel
            {
                get
                {
                    try
                    {
                        Load();
                        return configDocument.DocumentElement.SelectSingleNode("/config/sms").Attributes["channel"].Value.ToInt32();
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
            public static class SMS_Content
            {
                public static class AppointmentProduct
                {
                    public static string Submit
                    {
                        get
                        {
                            try
                            {
                                Load();
                                return configDocument.DocumentElement.SelectSingleNode("/config/sms/appointmentProduct/submit").InnerText;
                            }
                            catch
                            {
                                return string.Empty;
                            }
                        }
                    }
                    public static string Cancel
                    {
                        get
                        {
                            try
                            {
                                Load();
                                return configDocument.DocumentElement.SelectSingleNode("/config/sms/appointmentProduct/cancel").InnerText;
                            }
                            catch
                            {
                                return string.Empty;
                            }
                        }
                    }
                }

                public static class CPF
                {
                    public static string Audit
                    {
                        get
                        {
                            try
                            {
                                Load();
                                return configDocument.DocumentElement.SelectSingleNode("/config/sms/cpf/audit").InnerText;
                            }
                            catch
                            {
                                return string.Empty;
                            }
                        }
                    }
                    public static string Fail
                    {
                        get
                        {
                            try
                            {
                                Load();
                                return configDocument.DocumentElement.SelectSingleNode("/config/sms/cpf/fail").InnerText;
                            }
                            catch
                            {
                                return string.Empty;
                            }
                        }
                    }
                }

                public static class AuditAppointment
                {
                    public static string AuditPass
                    {
                        get
                        {
                            return GetNodeText("auditPass");
                        }
                    }
                    public static string AuditNoPass
                    {
                        get
                        {
                            return GetNodeText("auditNoPass");
                        }
                    }
                    public static string comfirmRemittancePass
                    {
                        get
                        {
                            return GetNodeText("comfirmRemittancePass");
                        }
                    }
                    public static string comfirmRemittanceNotPass
                    {
                        get
                        {
                            return GetNodeText("comfirmRemittanceNotPass");
                        }
                    }
                    public static string contractEstablished
                    {
                        get
                        {
                            return GetNodeText("contractEstablished");
                        }
                    }
                    /// <summary>
                    /// 现金发放
                    /// </summary>
                    public static string cashGiveOut
                    {
                        get
                        {
                            return GetNodeText("cashGiveOut");
                        }
                    }

                    private static string GetNodeText(string node)
                    {
                        try
                        {
                            Load();
                            return configDocument.DocumentElement.SelectSingleNode("/config/sms/auditAppointment/" + node).InnerText;
                        }
                        catch
                        {
                            return string.Empty;
                        }
                    }
                }
            }
        }

        #endregion

        #region ============= 金币配置 =============
        /// <summary>
        /// 金币配置
        /// </summary>
        public static class Coin
        {
            public static class Get
            {
                private static object obj_sync = new object();
                private static CoinGetConfigModel[] _items;
                public static CoinGetConfigModel[] Items
                {
                    get
                    {
                        try
                        {
                            Load();

                            if (_items == null || isNew)
                            {
                                lock (obj_sync)
                                {
                                    if (_items == null || isNew)
                                    {
                                        var itemNodes = configDocument.DocumentElement.SelectSingleNode("/config/coin/get").ChildNodes;
                                        _items = new CoinGetConfigModel[itemNodes.Count];
                                        int ix = 0;
                                        foreach (XmlNode node in itemNodes)
                                        {
                                            int ax = 1;
                                            _items[ix] = new CoinGetConfigModel();
                                            _items[ix].coin = node.Attributes["coin"].Value;
                                            _items[ix].desc = node.Attributes["desc"].Value;
                                            _items[ix].controller = node.Attributes["controller"].Value;
                                            _items[ix].action = node.Attributes["action"].Value;
                                            _items[ix].method = node.Attributes["method"].Value;
                                            _items[ix].filter = node.Attributes["filter"].Value;
                                            _items[ix].order = node.Attributes["order"].Value;
                                            _items[ix].condition = node.Attributes["condition"].Value;
                                            _items[ix].enable = node.Attributes["enable"].Value.ToBoolean();

                                            var node_field = node.Attributes["field"];
                                            if (node_field != null) _items[ix].field = node_field.Value;
                                            var node_step = node.Attributes["step"];
                                            if (node_step != null) _items[ix].Step = node_step.Value;
                                            var node_maxday = node.Attributes["maxday"];
                                            if (node_maxday != null) _items[ix].MaxDay = node_maxday.Value.ToInt32();
                                            var node_divideparm = node.Attributes["divideparam"];
                                            if (node_divideparm != null) _items[ix].DivideParam = node_divideparm.Value.ToInt32();
                                            var node_isget = node.Attributes["isget"];
                                            if (node_isget != null) _items[ix].IsGet = node_isget.Value.ToBoolean();
                                            ix++;
                                        }
                                    }
                                }
                            }

                            return _items;
                        }
                        catch
                        {
                            return new CoinGetConfigModel[0] { };
                        }
                    }
                }
            }


        }

        #endregion

        #region ============= 现金配置 =============
        /// <summary>
        /// 现金配置
        /// </summary>
        public static class Cash
        {
            public static class Get
            {
                private static object obj_sync = new object();
                private static CashGetConfigModel[] _items;
                public static CashGetConfigModel[] Items
                {
                    get
                    {
                        try
                        {
                            Load();

                            if (_items == null || isNew)
                            {
                                lock (obj_sync)
                                {
                                    if (_items == null || isNew)
                                    {
                                        var itemNodes = configDocument.DocumentElement.SelectSingleNode("/config/cash/get").ChildNodes;
                                        _items = new CashGetConfigModel[itemNodes.Count];
                                        int ix = 0;
                                        foreach (XmlNode node in itemNodes)
                                        {
                                            _items[ix] = new CashGetConfigModel();
                                            _items[ix].Cash = node.Attributes["cash"].Value;
                                            _items[ix].Desc = node.Attributes["desc"].Value;
                                            _items[ix].Controller = node.Attributes["controller"].Value;
                                            _items[ix].Action = node.Attributes["action"].Value;
                                            _items[ix].Method = node.Attributes["method"].Value;
                                            _items[ix].Filter = node.Attributes["filter"].Value;
                                            _items[ix].Order = node.Attributes["order"].Value;
                                            _items[ix].Condition = node.Attributes["condition"].Value;
                                            _items[ix].Effective = node.Attributes["effective"].Value;
                                            _items[ix].Enable = node.Attributes["enable"].Value.ToBoolean();

                                            var node_field = node.Attributes["field"];
                                            if (node_field != null) _items[ix].Field = node_field.Value;
                                            var node_divideparm = node.Attributes["divideparam"];
                                            if (node_divideparm != null) _items[ix].DivideParam = node_divideparm.Value.ToInt32();
                                            var node_isget = node.Attributes["isget"];
                                            if (node_isget != null) _items[ix].IsGet = node_isget.Value.ToInt32();
                                            if (node.ChildNodes.Count > 0)
                                            {
                                                _items[ix].NextAction = new CashHelp();
                                                var nodeNextactionController_field = node.ChildNodes[0].Attributes["controller"];
                                                if (nodeNextactionController_field != null) _items[ix].NextAction.Controller = nodeNextactionController_field.Value;
                                                var nodeNextactionAction_field = node.ChildNodes[0].Attributes["action"];
                                                if (nodeNextactionAction_field != null) _items[ix].NextAction.Action = nodeNextactionAction_field.Value;
                                            }
                                            ix++;
                                        }
                                    }
                                }
                            }

                            return _items;
                        }
                        catch
                        {
                            return new CashGetConfigModel[0] { };
                        }
                    }
                }
            }


        }

        #endregion

        #region ============= 日志配置 =============

        /// <summary>
        /// 日志配置
        /// </summary>
        public static class Log
        {
            public static string path
            {
                get
                {
                    try
                    {
                        Load();
                        return configDocument.DocumentElement.SelectSingleNode("/config/log/directory").InnerText;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }

            public static class debug
            {
                public static bool islog
                {
                    get
                    {
                        try
                        {
                            Load();
                            return configDocument.DocumentElement.SelectSingleNode("/config/log/debug").Attributes["islog"].Value.ToBoolean();
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }

                public static string filename
                {
                    get
                    {
                        try
                        {
                            Load();
                            return configDocument.DocumentElement.SelectSingleNode("/config/log/debug").Attributes["filename"].Value;
                        }
                        catch
                        {
                            return string.Empty;
                        }
                    }
                }
            }

            public static class error
            {
                public static bool islog
                {
                    get
                    {
                        try
                        {
                            Load();
                            return configDocument.DocumentElement.SelectSingleNode("/config/log/error").Attributes["islog"].Value.ToBoolean();
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }

                public static string filename
                {
                    get
                    {
                        try
                        {
                            Load();
                            return configDocument.DocumentElement.SelectSingleNode("/config/log/error").Attributes["filename"].Value;
                        }
                        catch
                        {
                            return string.Empty;
                        }
                    }
                }
            }
        }

        #endregion

        #region ==========================

        public class CoinGetConfigModel
        {
            public string coin { get; set; }
            public string desc { get; set; }
            public string controller { get; set; }
            public string action { get; set; }
            public string filter { get; set; }
            public string order { get; set; }
            public string condition { get; set; }
            public string method { get; set; }
            public bool enable { get; set; }
            public string field { get; set; }
            /// <summary>
            /// 每天递增数量
            /// </summary>
            public string Step { get; set; }
            /// <summary>
            /// 最大递增天数
            /// </summary>
            public int MaxDay { get; set; }
            /// <summary>
            /// 预约金额除以的比例
            /// </summary>
            public int DivideParam { get; set; }
            /// <summary>
            /// 是否直接生效
            /// </summary>
            public bool? IsGet { get; set; }
        }

        #endregion

        #region ============= 现金单条辅助MODE L=============
        /// <summary>
        /// 现金单条辅助MODEL
        /// </summary>
        public class CashGetConfigModel
        {
            public string Cash { get; set; }
            public string Desc { get; set; }
            public string Controller { get; set; }
            public string Action { get; set; }
            public string Method { get; set; }
            public string Filter { get; set; }
            public string Order { get; set; }
            public string Condition { get; set; }
            public string Effective { get; set; }
            public int? IsGet { get; set; }
            public int DivideParam { get; set; }
            public string Field { get; set; }
            public CashHelp NextAction { get; set; }
            public bool Enable { get; set; }
        }
        #endregion

        #region ============= 现金下一步走向 =============
        /// <summary>
        /// 现金下一步走向
        /// </summary>
        public class CashHelp
        {
            public string Controller { get; set; }
            public string Action { get; set; }
        }
        #endregion

        #region ============= 一刮到底(刮刮卡活动) =============

        public static class GuaGuaKa
        {
            /// <summary>
            /// 活动过期时间
            /// </summary>
            public static DateTime ExpiresTime
            {
                get
                {
                    Load();
                    return configDocument.DocumentElement.SelectSingleNode("/config/guaguaka/ExpiresTime").XmlSingleNodeInnerText(defaultVal: DateTime.Now.ToString()).ToDateTime();
                }
            }
        }

        #endregion

        #region 城市经理配置
        public static class CityManager
        {
            public static string TimeSpan
            {
                get
                {
                    try
                    {
                        Load();
                        return configDocument.DocumentElement.SelectSingleNode("/config/cityManager/timeSpan").InnerText;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }
        }
        #endregion
    }
}
