/*
 * 名称：用于文件和文件夹类的一些操作。
 * 作用：生成随机字符串，用于随机测试数据。
 * 时间：
 * 2013-7-31   建立2个用于打开文件和文件夹的函数。
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindGoes
{
	/// <summary>
	/// 用于文件和文件夹类的一些操作函数。
	/// </summary>
	public class IOHelper
	{
		/// <summary>
		/// 开指定的文件或文件夹。如果是文件则尝试打开，如果是文件夹则使用IE打开。
		/// </summary>
		/// <param name="path">文件或文件夹路径。</param>
		public static void OpenPath(string path)
		{
			System.Diagnostics.Process.Start("explorer.exe", path);
		}

		/// <summary>
		/// 打开并选中文件或文件夹。
		/// </summary>
		/// <param name="path">指定的文件或文件夹。</param>
		public static void OpenAndSelectPath(string path)
		{
			System.Diagnostics.Process.Start("explorer.exe", "/select, " + path + ", /n");
		}
	}
}
