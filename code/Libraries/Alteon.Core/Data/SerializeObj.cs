using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Reflection;

namespace Alteon.Core
{
    [Serializable]
    public class SerializeObj
    {
        private JObject _serializ;

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="SerializeData">序列化数据（JSon、Xml)</param>
        public SerializeObj(object serializeData)
        {
            string t = serializeData.GetType().Name;
            if (t.Contains("JObject"))
                _serializ = (JObject)serializeData;
            else if (t.Contains("String"))
            {
                _serializ = Core.Common.JsonConverter.DeserializeObject<JObject>((string)serializeData);
            }
            else
            {
                _serializ = new JObject();
            }
        }
        /// <summary>
        /// 属性数量
        /// </summary>
        public int Count
        {
            get
            {
                return _serializ.Count;
            }
        }

        /// <summary>
        /// 获取字符串值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string Get(string propertyName)
        {
            try
            {
                string t = "";
                JToken data;
                bool isext = _serializ.TryGetValue(propertyName, out data);
                if (isext) t = (string)data;
                return (t == null ? "" : t);
            }
            catch { }
            return "";
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public T Get<T>(string propertyName)
        {
            try
            {
                if (typeof(T) == typeof(SerializeObjArray))
                {
                    return (T)Convert.ChangeType(GetSerializeObjArray(propertyName), typeof(T), CultureInfo.InvariantCulture);
                }
                else
                {
                    JToken data;
                    bool isext = _serializ.TryGetValue(propertyName, out data);
                    if (isext)
                    {
                        return data.Value<T>();
                    }
                }
            }
            catch
            {

            }
            return default(T);
        }

        /// <summary>
        /// 转换为指定实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public T To<T>() where T : new()
        {
            try
            {

                Type t = typeof(T).GetType();
                T obj = new T();
                var mems = obj.GetType().GetProperties();
                foreach (var item in mems)
                {
                    try
                    {
                        Type _type = item.PropertyType.IsGenericType ? Nullable.GetUnderlyingType(item.PropertyType) : item.PropertyType;
                        item.SetValue(obj, Convert.ChangeType(Get<string>(item.Name), _type));
                    }
                    catch { }
                }
                return (T)obj;
            }
            catch
            {

            }
            return default(T);
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object GetValue(string propertyName)
        {
            JToken data;
            bool isext = _serializ.TryGetValue(propertyName, out data);
            if (isext) return data;
            return null;
        }

        /// <summary>
        /// 获取一组序列化对象
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public SerializeObjArray GetSerializeObjArray(string Name)
        {
            SerializeObjArray soa = new SerializeObjArray(_serializ[Name]);
            return soa;
        }
    }
}
