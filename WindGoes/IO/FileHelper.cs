using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindGoes.IO
{
	/// <summary>
	/// 提供了一系列用于文件操作的方法，主要的目的是扩展系统的相关内容，同时提高安全性。
	/// </summary>
	public class FileHelper
	{
		static string success = "OK";
		/// <summary>
		/// 将文件复制到指定位置，返回 "OK", 如果出现错误，则返回相关错误信息。
		/// </summary>
		/// <param name="source"></param>
		/// <param name="dest"></param>
		/// <returns></returns>
		public static string CopyFile(string source, string dest)
		{

			return success;
		}
	}
}
