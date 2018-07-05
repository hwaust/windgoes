/*
 * 名称：INI读写类
 * 
 * 作用：实现对INI文件读写的封装，只需要简单的两三个参数即可使用，包括三个主要函数：
 *       public string ReadValue(string lpApplicationName, string lpKeyName)
 *       public bool WriteValue(string lpApplicationName, string lpKeyName, string lpString)
 *       public string ReadValue(string lpApplicationName, string lpKeyName, string lpDefault)
 * 
 * 作者：郝  伟
 * 
 * 时间：2010-5-5   初步建立这几个方法。   
 *       2011-6-3   添加Section属性，这样就可以直接写值。 
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;

namespace WindGoes.IO
{
    /// <summary>
    /// 此类用于实现对INI文件的读写，包括了读，写两个方法，同时还有文件位置属性及默认输出属性。
    /// </summary>
	public class IniAccess : ObjectBase
    {
        #region DLL声明模块
        [DllImport("kernel32")]
        private static extern bool GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);

        #endregion

        #region 自定义字段和属性

        string filePath, defaultError;

        /// <summary>
        /// 读取或设置所操作的INI文件的位置。
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        /// <summary>
        /// 读取或设置读取失败的信息。
        /// </summary>
        public string DefaultError
        {
            get { return defaultError; }
            set { defaultError = value; }
        }

		string section = "Others";
		/// <summary>
		/// 默认的段。
		/// </summary>
		public string Section
		{
			get { return section; }
			set { section = value; }
		}

        #endregion

        #region 构造函数

        /// <summary>
        /// 此类用于实现对INI文件的读写，包括了读，写两个方法，同时还有文件位置属性及默认输出属性。
        /// </summary>
        public IniAccess() { }

        /// <summary>
        /// 此类用于实现对INI文件的读写，包括了读，写两个方法，同时还有文件位置属性及默认输出属性。
        /// </summary>
        /// <param name="strPath">要操作的INI文件位置。</param>
        public IniAccess(string strPath)
        {
            if (strPath.IndexOf(':') < 0)
            {
                strPath = Path.Combine(Application.StartupPath, strPath);
            }
            FilePath = strPath;

            CreateFile(filePath);
        }
        #endregion

        #region 主要操作函数

        /// <summary>
        /// 读取属性的值。
        /// </summary>
        /// <param name="lpApplicationName">字段的类别。</param>
        /// <param name="lpKeyName">字段名。</param>
        /// <param name="lpString">字段值。</param>
        /// <returns>返回值表示是否执行成功。</returns>
        public bool WriteValue(string lpApplicationName, string lpKeyName, string lpString)
        {
            return WritePrivateProfileString(lpApplicationName, lpKeyName, lpString, filePath);
        }

		/// <summary>
		/// 读取属性的值。
		/// </summary>
		/// <param name="lpKeyName">字段名。</param>
		/// <param name="lpString">字段值。</param>
		/// <returns>返回值表示是否执行成功。</returns>
		public bool WriteValue(string lpKeyName, string lpString)
		{
			return WritePrivateProfileString(section, lpKeyName, lpString, filePath);
		}


        /// <summary>
        /// 读取INI文件中的值。
        /// </summary>
        /// <param name="lpApplicationName">字段的类别。</param>
        /// <param name="lpKeyName">字段名。</param>
        /// <param name="lpDefault">读取失败的返回值。</param>
        /// <returns>字段值.</returns>
        public string ReadValue(string lpApplicationName, string lpKeyName, string lpDefault)
        {
            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString(lpApplicationName, lpKeyName, lpDefault, sb, sb.Capacity, filePath);
            return sb.ToString();
        }

        /// <summary>
        /// 读取INI文件中的值。
        /// </summary>
        /// <param name="lpApplicationName">字段的类别。</param>
        /// <param name="lpKeyName">字段名。</param>
        /// <returns>字段值。</returns>
        public string ReadValue(string lpApplicationName, string lpKeyName)
        {
            StringBuilder sb = new StringBuilder(255);
            bool b = GetPrivateProfileString(lpApplicationName, lpKeyName, defaultError, sb, sb.Capacity, filePath);
            return sb.ToString();
        }

		/// <summary>
		/// 读取INI文件中的值。
		/// </summary> 
		/// <param name="lpKeyName">字段名。</param>
		/// <returns>字段值。</returns>
		public string ReadValue(string lpKeyName)
		{
			StringBuilder sb = new StringBuilder(255);
			bool b = GetPrivateProfileString(section, lpKeyName, defaultError, sb, sb.Capacity, filePath);
			return sb.ToString();
		}


        #endregion

        /// <summary>
        /// 创建文件，如果路径不存在自动创建路径，然后再创建文件。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool CreateFile(string fileName)
        {
            try
            {
                if (fileName.IndexOf(':') < 0)
                    fileName = Path.Combine(Application.StartupPath, fileName); 

                string dir = Path.GetDirectoryName(fileName);
                string name = Path.GetFileName(fileName);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                if (!File.Exists(fileName))
                {
                    FileStream fs = File.Create(fileName);
                    fs.Close();
                }
            }
            catch 
            {
                return false;
            }
      
            return true;
        }
    }
}
