using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WindGoes.Data;

namespace WindGoes
{
	/// <summary>
	/// 提供了一些对文件操作的函数。
	/// </summary>
	public class FileHelper
	{

		/// <summary>
		/// 将文件中的所有数据一次性读取。
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string LoadFromFile(string path)
		{
			string data = null;
			try
			{
				using (StreamReader sr = new StreamReader(path, Encoding.Default))
				{
					data = sr.ReadToEnd();
				}
			}
			catch (Exception e1)
			{
				Console.WriteLine(e1.Message);
			}
			return data;
		}

		/// <summary>
		/// 将所有内容加密后，保存至指定文件夹。
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		//public bool SaveDesString(string path)
		//{
		//    return FileHelper.SaveToFile( DESCrypto.Encrypt(ToFileString()), path);
		//}

		/// <summary>
		/// 将指定的文本内容保存在指定的文件中。
		/// </summary>
		/// <param name="content">数据内容。</param>
		/// <param name="path">待保存的路径。</param>
		/// <returns></returns>
		public static bool SaveToFile(string content, string path)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter(path, false, Encoding.Default))
				{
					sw.Write(content);
				}
				return true;
			}
			catch (Exception e1)
			{
				Console.WriteLine(e1.Message);
			}
			return false;
		}

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
