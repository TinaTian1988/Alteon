using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alteon.Core.Common
{

    /// <summary>
    /// Json操作
    /// </summary>
    public sealed class JsonConverter
    {
        private static JsonSerializerSettings _jsonSettings;
        private static void initSettings()
        {
            IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverter();
            datetimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            _jsonSettings = new JsonSerializerSettings();
            _jsonSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            _jsonSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            _jsonSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            _jsonSettings.Converters.Add(datetimeConverter);

        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject(object obj)
        {

            if (_jsonSettings == null) initSettings();
            string json = JsonConvert.SerializeObject(obj, _jsonSettings);
            return (string.IsNullOrEmpty(json) ? "null" : json);
        }
        /// <summary>
        /// 反序列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string jsonStr)
        {
            if (_jsonSettings == null) initSettings();
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonStr, _jsonSettings);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
