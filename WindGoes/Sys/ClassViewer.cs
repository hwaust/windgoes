/*
 * 名称：类内容查看工具。
 * 
 * 作用：用于对任意一个类的输出显示和查看。如生成一个类有5个属性，查输出查看时，可以使用此类。
 * 
 * 作者：郝  伟
 * 
 * 时间：
 * 2013-09-07   初步建立这个类。 
 * 
 * 更新：
 * 2013-10-15 已经用于吴焱论文。
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace WindGoes.Sys
{
	/// <summary>
	/// 用于查看任意类的对象。
	/// </summary>
	public class ClassViewer
	{
		object obj = "ClassView";

		/// <summary>
		/// 用于查看任意类的对象。
		/// </summary>
		/// <param name="o">需要查看的对象。</param>
		public ClassViewer(object o) { obj = o; }

		/// <summary>
		/// 返回所有的属性。
		/// <example>
		/// 如果一个类有2个属性 ID和Name，值分别为10001和"Jack"，那么返回字符为  "ID = 5; Name = Jack; "
		/// </example>
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public  string GetAllAttributes(object obj)
		{
			try
			{
				Type t = obj.GetType();
				PropertyInfo[] pis = t.GetProperties();
				StringBuilder builder = new StringBuilder();
				foreach (PropertyInfo pi in pis)
				{
					builder.Append(pi.Name + " = " + pi.GetValue(obj, null) + "; ");
				}
				return builder.ToString();
			}
			catch { }
			return null;
		}


	}
}
