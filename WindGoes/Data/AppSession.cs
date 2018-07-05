/*
 * 名称：通用内存共享类
 * 
 * 作用：通过对立一个字典，将所有的数据以键和值的形式进行存储，方便使用。
 * 作者：郝伟
 * 
 * 时间：2013年3月26日 21：20
 * 更新：无 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindGoes.Data
{
	/// <summary>
	/// 用于程序的数据存储。
	/// </summary>
	  class AppSession
	{
		
		/// <summary>
		/// 所有的键。
		/// </summary>
		public List<string> Keys { get; set; }

		/// <summary>
		/// 所有的值。
		/// </summary>
		public List<object> Values { get; set; }


		public AppSession()
		{
			Keys = new List<string>();
			Values = new List<object>();
		}

		public T GetValue<T>(string key)
		{
			int p = Keys.IndexOf(key);
			if(p >= 0)
				return (T)Values[p];

			return default(T);
		}
	}
}
