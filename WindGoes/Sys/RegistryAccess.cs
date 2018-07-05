/*
 * ���ƣ�ע����д��
 * 
 * ���ã�ʵ�ֶ�ע����ļ���д�ķ�װ��ֻ��Ҫ�򵥵���������������ʹ�ã�����������Ҫ������
 *       public string ReadValue(string lpApplicationName, string lpKeyName)
 *       public void WriteValue(string lpApplicationName, string lpKeyName, string lpString)
 *       public string ReadValue(string lpApplicationName, string lpKeyName, string lpDefault)
 * 
 * ���ߣ���  ΰ
 * 
 * ʱ�䣺2009��5��5�� 22��00
 * 
 * ���£�
 *       2009�� 5��13�� 12��38  ��Ӹ�Ŀ¼�޸ķ��� public void SetRoot(string path)
 *                              ����һ��ö������RootType���ڱ�ʾ����λ�á�
 *       2009�� 5�� 6�� 17��56  ����д��ʧ�ܵ����⣨OpenSubKeyʱ����true����ʾ�ɸ�д��                            
 *       2009�� 5�� 5�� 22��23  �����������д���
 *       
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32; 

namespace WindGoes.Sys
{
    /// <summary>
    /// ע���ĸ����͡�
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
        #region �Զ����ֶκ�����

        string filePath, defaultError;
        RegistryKey defaultRoot;
        RegistryKey rootkey;
        /// <summary>
        /// ��ȡ��������������INI�ļ���λ�á�
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        /// <summary>
        /// ��ȡ�����ö�ȡʧ�ܵ���Ϣ��
        /// </summary>
        public string DefaultError
        {
            get { return defaultError; }
            set { defaultError = value; }
        }



        #endregion

        #region ���캯��

        public RegisterAccess() { }

        /// <summary>
        /// Ĭ��Ŀ¼Ϊ��CurrentUser\\Software\\HaoWei\\������Ϊ��Ŀ¼�´�����Ŀ¼��
        /// </summary>
        /// <param name="strPath">��Ĭ��Ŀ¼�´�����Ŀ¼��</param>
        public RegisterAccess(string strPath)
        {
            rootkey = Registry.CurrentUser;
            rootkey.CreateSubKey("Software\\HaoWei\\" + strPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            defaultRoot = rootkey.OpenSubKey("Software\\HaoWei\\" + strPath, true);
        }

        #endregion

        #region ��Ҫ��������

        /// <summary>
        /// ��ȡ���Ե�ֵ��
        /// </summary>
        /// <param name="lpApplicationName">�ֶε����</param>
        /// <param name="lpKeyName">�ֶ�����</param>
        /// <param name="lpString">�ֶ�ֵ��</param>
        public void WriteValue(string lpApplicationName, string lpKeyName, string lpString)
        {
            //��ǰĿ¼
            defaultRoot.CreateSubKey(lpApplicationName, RegistryKeyPermissionCheck.ReadWriteSubTree);
            RegistryKey currentkey = defaultRoot.OpenSubKey(lpApplicationName, true);
            currentkey.SetValue(lpKeyName, lpString);
        }

        /// <summary>
        /// ��ȡINI�ļ��е�ֵ��
        /// </summary>
        /// <param name="lpApplicationName">�ֶε����</param>
        /// <param name="lpKeyName">�ֶ�����</param>
        /// <returns>�ֶ�ֵ��</returns>
        public string ReadValue(string lpApplicationName, string lpKeyName)
        {
            RegistryKey currentkey = defaultRoot.OpenSubKey(lpApplicationName);
            return currentkey.GetValue(lpKeyName, "").ToString();
        }

        /// <summary>
        /// ��ȡINI�ļ��е�ֵ��
        /// </summary>
        /// <param name="lpApplicationName">�ֶε����</param>
        /// <param name="lpKeyName">�ֶ�����</param>
        /// <param name="lpDefault">��ȡʧ�ܵķ���ֵ��</param>
        /// <returns>�ֶ�ֵ.</returns>
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
