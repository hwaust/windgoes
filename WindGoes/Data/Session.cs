using System;
using System.Collections.Generic;
using System.Text;

namespace WindGoes.Data
{
    /// <summary>
    /// 用于保存全局和局部的数据。
    /// </summary>
    public class Session
    {
        //使用字典来保存数据。
        Dictionary<string, object> data = new Dictionary<string, object>();

	

        /// <summary>
        /// 读取或设置Session的值。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                return data.ContainsKey(key) ? data[key] : null;
            }
            set
            {
                if (data.ContainsKey(key))
                {
                    data[key] = value;
                }
                else
                {
                    data.Add(key, value);
                }
            }
        }

        /// <summary>
        /// 获得Session中的数据总量。
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return data.Count;
        }

        /// <summary>
        /// 是否包含某值。
        /// </summary>
        /// <param name="obj">待检测的值。</param>
        /// <returns></returns>
        public bool ContainsValue(object obj)
        {
            return data.ContainsValue(obj);
        }

        /// <summary>
        /// 清空所有数据。
        /// </summary>
        public void Clear()
        {
            data.Clear(); 
        }

        /// <summary>
        /// 获得所有的键值。
        /// </summary>
        /// <returns></returns>
        public string[] GetKeys()
        {
            List<string> keys = new List<string>();
            foreach (string key in data.Keys)
            {
                keys.Add(key);
            }
            return keys.ToArray();
        }

        /// <summary>
        /// 获得所有的数据。
        /// </summary>
        /// <returns></returns>
        public object[] GetValues()
        {
            List<object> values = new List<object>();
            foreach (object value in data.Values)
            {
                values.Add(value);
            }
            return values.ToArray();
        }

		public void Save(string path)
		{

		}
    }
}
