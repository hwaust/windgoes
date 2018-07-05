/*
 * 名称：注册表读写类
 * 
 * 作用：实现对注册表文件读写的封装，只需要简单的两三个参数即可使用，包括三个主要函数：
 *       public string ReadValue(string lpApplicationName, string lpKeyName)
 *       public void WriteValue(string lpApplicationName, string lpKeyName, string lpString)
 *       public string ReadValue(string lpApplicationName, string lpKeyName, string lpDefault)
 * 
 * 作者：郝  伟
 * 
 * 时间：2009年5月5日 22：00
 * 
 * 更新：
 *       2009年 5月13日 12：38  添加根目录修改方法 public void SetRoot(string path)
 *                              创建一个枚举类型RootType用于表示根的位置。
 *       2009年 5月 6日 17：56  修正写入失败的问题（OpenSubKey时加入true，表示可改写）                            
 *       2009年 5月 5日 22：23  初步生成所有代码
 *       
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32; 

namespace WindGoes.Sys
{
    /// <summary>
    /// 注册表的根类型。
    /// </summary>
    public enum RootType
    {
        CLASSROOT,
        CURRENTUSER,
        LOCALMACHINE,
        USERS,
        CURRENTCONFIG
    }

	public class RegisterAccess : ObjectBase
    { 
        #region 自定义字段和属性

        string filePath, defaultError;
        RegistryKey defaultRoot;
        RegistryKey rootkey;
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



        #endregion

        #region 构造函数

        public RegisterAccess() { }

        /// <summary>
        /// 默认目录为：CurrentUser\\Software\\HaoWei\\，参数为此目录下创建的目录。
        /// </summary>
        /// <param name="strPath">在默认目录下创建的目录。</param>
        public RegisterAccess(string strPath)
        {
            rootkey = Registry.CurrentUser;
            rootkey.CreateSubKey("Software\\HaoWei\\" + strPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            defaultRoot = rootkey.OpenSubKey("Software\\HaoWei\\" + strPath, true);
        }

        #endregion

        #region 主要操作函数

        /// <summary>
        /// 读取属性的值。
        /// </summary>
        /// <param name="lpApplicationName">字段的类别。</param>
        /// <param name="lpKeyName">字段名。</param>
        /// <param name="lpString">字段值。</param>
        public void WriteValue(string lpApplicationName, string lpKeyName, string lpString)
        {
            //当前目录
            defaultRoot.CreateSubKey(lpApplicationName, RegistryKeyPermissionCheck.ReadWriteSubTree);
            RegistryKey currentkey = defaultRoot.OpenSubKey(lpApplicationName, true);
            currentkey.SetValue(lpKeyName, lpString);
        }

        /// <summary>
        /// 读取INI文件中的值。
        /// </summary>
        /// <param name="lpApplicationName">字段的类别。</param>
        /// <param name="lpKeyName">字段名。</param>
        /// <returns>字段值。</returns>
        public string ReadValue(string lpApplicationName, string lpKeyName)
        {
            RegistryKey currentkey = defaultRoot.OpenSubKey(lpApplicationName);
            return currentkey.GetValue(lpKeyName, "").ToString();
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
            RegistryKey currentkey = defaultRoot.OpenSubKey(lpApplicationName);
            return currentkey.GetValue(lpKeyName, defaultError).ToString();
        }


        public bool SetRoot(RootType rt, string path)
        {
            switch (rt)
            {
                case RootType.CLASSROOT:
                    rootkey = Registry.ClassesRoot;
                    break;
                case RootType.CURRENTUSER:
                    rootkey = Registry.CurrentUser;
                    break;
                case RootType.LOCALMACHINE:
                    rootkey = Registry.LocalMachine;
                    break;
                case RootType.USERS:
                    rootkey = Registry.Users;
                    break;
                case RootType.CURRENTCONFIG:
                    rootkey = Registry.CurrentConfig;
                    break;
                default:
                    break;
            }
            try
            {
                rootkey.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree);
                defaultRoot = rootkey.OpenSubKey(path, true);
                return true;
            }
            catch { return false; }
        }
        #endregion
    }
}
