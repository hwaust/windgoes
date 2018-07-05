using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindGoes.Data
{
	/// <summary>
	/// 属性序列化的接口，包括对象转为字符串和字符串转为对象2个过程。
	/// </summary>
	public interface IPropertyManager
	{
		/// <summary>
		/// 将当前对象转换为字符串，注：在字符串中不能有回车。
		/// </summary>
		/// <returns></returns>
		string ToPString();
		/// <summary>
		/// 给定字符串，解释后，给当前对象赋值，返回this指针。
		/// </summary>
		/// <param name="s">给定格式字符串。</param>
		/// <returns></returns>
		void FromPString(string s);
	}
}
