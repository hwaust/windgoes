using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WindGoes
{
	/// <summary>
	/// 关于日期的一些处理函数。
	/// </summary>
	public class DateHelper
	{
		/// <summary>
		/// 返回指定格式的时间字符串，格式为： 2011-11-08
		/// </summary>
		/// <param name="dt">需要转换的时间。</param>
		/// <returns></returns>
		public static string ToStringYYYY_MM_DD(DateTime dt)
		{
			return string.Format("{0}-{1}-{2}",
				dt.Year, dt.Month.ToString("00"), dt.Day.ToString("00"));
		}

		/// <summary>
		/// 返回指定格式的时间字符串，格式为： 17:07:32
		/// </summary>
		/// <param name="dt">需要转换的时间。</param>
		/// <returns></returns>
		public static string ToStringhh_mm_ss(DateTime dt)
		{
			return string.Format("{0}-{1}-{2}",
				dt.Hour.ToString("00"), dt.Minute.ToString("00"), dt.Second.ToString("00"));

		}


		/// <summary>
		/// 返回指定格式的日期时间字符串，格式为： 2011-10-08 17:07:32
		/// </summary>
		/// <param name="dt">需要转换的时间。</param>
		/// <returns></returns>
		public static string ToStringYYYY_MM_DD_hh_mm_ss(DateTime dt)
		{
			return string.Format("{0}-{1}-{2} {3}:{4}:{5}",
				dt.Year, dt.Month.ToString("00"), dt.Day.ToString("00"),
				dt.Hour.ToString("00"), dt.Minute.ToString("00"), dt.Second.ToString("00"));
		}

		/// <summary>
		/// 给定日期，返回字符串，格式为：2013-04-09 22:52:35 -> 20130409225235
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string ToStringYYYYMMDDhhmmss(DateTime dt)
		{
			return string.Format("{0}{1}{2}{3}{4}{5}",
				dt.Year,
				dt.Month.ToString("00"),
				dt.Day.ToString("00"),
				dt.Hour.ToString("00"),
				dt.Minute.ToString("00"),
				dt.Second.ToString("00"));
		}

		/// <summary>
		/// 指定的格式为：20121105143005 -> 2012-11-05 14:30:05
		/// </summary>
		/// <param name="dts"></param>
		/// <returns></returns>
		public static DateTime GetDateFromYYYYMMDDhhmmss(string dts)
		{
			DateTime dt = new DateTime(2012, 1, 1, 1, 0, 0);
			try
			{
				dt = new DateTime(
					int.Parse(dts.Substring(0, 4)),
					int.Parse(dts.Substring(4, 2)),
					int.Parse(dts.Substring(6, 2)),
					int.Parse(dts.Substring(8, 2)),
					int.Parse(dts.Substring(10, 2)),
					int.Parse(dts.Substring(12, 2))
					);
			}
			catch { }
			return dt;
		} 

		/// <summary>
		/// 给指定文件名加时间戳。
		/// </summary>
		/// <param name="dst">目标地址。</param>
		/// <returns></returns>
		public static string AppendTimeToFileName(string dst)
		{
			try
			{
				string path = Path.GetDirectoryName(dst);
				string filename = Path.GetFileNameWithoutExtension(dst);
				string ext = Path.GetExtension(dst);
				return path + "\\" + filename + "_" + ToStringYYYYMMDDhhmmss(DateTime.Now) + ext;
			}
			catch { } 
			return dst;
		}

	}
}
