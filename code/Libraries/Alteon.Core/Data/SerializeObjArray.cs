using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Alteon.Core
{
    [Serializable]
    /// <summary>
    /// 序列化对象数组
    /// </summary>
    public class SerializeObjArray :IEnumerable<SerializeObj>
    {
        private IEnumerable<SerializeObj> _serializArray;

        /// <summary>
        /// 反序列化对象数组
        /// </summary>
        /// <param name="serializeData"></param>
        public SerializeObjArray(object serializeData)
        {
            List<SerializeObj> list = new List<SerializeObj>();
            try
            {
                string t = serializeData.GetType().Name;
                JArray _list = new JArray();

                if (t.Contains("String"))
                {
                    _list = Core.Common.JsonConverter.DeserializeObject<JArray>((string)serializeData);
                }
                else if (t.Contains("JArray"))
                {
                    _list = (JArray)serializeData;
                }

                if (_list != null)
                {
                    foreach (var jt in _list)
                    {
                        list.Add(new SerializeObj((JObject)jt));
                    }
                }
            }
            catch { }
            _serializArray = list;

        }

        public IEnumerator<SerializeObj> GetEnumerator()
        {
            return _serializArray.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _serializArray.GetEnumerator();
        } 
        

    }
}
