/*
 * 名称：INI读写类
 * 
 * 作用：向系统申请一块内存，可以向其中读写数据，几个程序可以共享此内存块。
 *  
 * 作者：郝  伟
 * 
 * 时间：2008.2.1   初步建立这几个方法。     
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WindGoes.Sys
{ 
    /// <summary>
    /// 用于共享内存操作的类。
    /// </summary>
	public class MemoryShare : ObjectBase
    {
        #region Dlls
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFileMapping(int hFile, IntPtr lpAttributes, uint flProtect, uint dwMaxSizeHi, uint dwMaxSizeLow, string lpName);
        
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr OpenFileMapping(int dwDesiredAccess,[MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,string lpName);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr MapViewOfFile(IntPtr hFileMapping,uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow,uint dwNumberOfBytesToMap);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool UnmapViewOfFile(IntPtr pvBaseAddress);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32", EntryPoint="GetLastError")]
        public static extern int GetLastError ();
        #endregion

        #region Const
        const int ERROR_ALREADY_EXISTS = 183;

        const int FILE_MAP_COPY = 0x0001;
        const int FILE_MAP_WRITE = 0x0002;
        const int FILE_MAP_READ = 0x0004;
        const int FILE_MAP_ALL_ACCESS = 0x0002 | 0x0004;

        const int PAGE_READONLY = 0x02;
        const int PAGE_READWRITE = 0x04;
        const int PAGE_WRITECOPY = 0x08;
        const int PAGE_EXECUTE = 0x10;
        const int PAGE_EXECUTE_READ = 0x20;
        const int PAGE_EXECUTE_READWRITE = 0x40;

        const int SEC_COMMIT = 0x8000000;
        const int SEC_IMAGE = 0x1000000;
        const int SEC_NOCACHE = 0x10000000;
        const int SEC_RESERVE = 0x4000000; 
        const int INVALID_HANDLE_VALUE = -1;

        IntPtr m_hSharedMemoryFile = IntPtr.Zero;
        IntPtr m_pwData = IntPtr.Zero;
        #endregion 
 

        bool m_bAlreadyExist = false;
        bool m_bInit = false;
        long m_MemSize = 0;

        int maxLength = 1024;
        /// <summary>
        /// 最大长度。
        /// </summary>
        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }

        ~MemoryShare()
        {
            if (m_bInit)
            {
                UnmapViewOfFile(m_pwData);
                CloseHandle(m_hSharedMemoryFile);
            }
        }

        /// <summary>
        /// 初始化共享内存
        /// </summary>
        /// <param name="strName">共享内存名称</param>
        /// <returns></returns>
        public int Init(string strName)
        {
            return Init(strName, maxLength);
        }

        /// <summary>
        /// 初始化共享内存
        /// </summary>
        /// <param name="strName">共享内存名称</param>
        /// <param name="lngSize">共享内存大小</param>
        /// <returns></returns>
        public int Init(string strName, long lngSize)
        {
            //验证并初始化数据。
            if (lngSize <= 0 || lngSize > 0x00800000)   lngSize = 0x00800000;   //长度最大为800K
            if (strName.Length <= 0)  return 1;   //共享区名称不能为空。    
            m_MemSize = lngSize; 

            //创建内存共享体(INVALID_HANDLE_VALUE)
            m_hSharedMemoryFile = CreateFileMapping(INVALID_HANDLE_VALUE, IntPtr.Zero,
                (uint)PAGE_READWRITE, 0, (uint)lngSize, strName);
            if (m_hSharedMemoryFile == IntPtr.Zero)
            {
                m_bAlreadyExist = false;
                m_bInit = false;
                return 2; //创建共享体失败
            }
            else
                m_bAlreadyExist = GetLastError() == ERROR_ALREADY_EXISTS; //判断是否为新创建。
  
            // 创建内存映射
            m_pwData = MapViewOfFile(m_hSharedMemoryFile, FILE_MAP_WRITE, 0, 0, (uint)lngSize);
            if (m_pwData == IntPtr.Zero)
            {
                m_bInit = false;
                CloseHandle(m_hSharedMemoryFile);
                return 3; //创建内存映射失败
            }
            else 
                m_bInit = true; 
 
            return 0;     //创建成功
        }
        
        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="bytData">数据</param>
        /// <param name="lngAddr">起始地址</param>
        /// <param name="lngSize">个数</param>
        /// <returns></returns>
        public int Read(ref byte[] bytData, int lngAddr, int lngSize)
        {
            if (lngAddr + lngSize > m_MemSize) return 2; //超出数据区
            if (m_bInit)
            {               
                Marshal.Copy(m_pwData, bytData, lngAddr, lngSize);
            }
            else
            {
                return 1; //共享内存未初始化
            }
            return 0;     //读成功
        }

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="bytData">数据</param>
        /// <param name="lngAddr">起始地址</param>
        /// <param name="lngSize">个数</param>
        /// <returns></returns>
        public int Write(byte[] bytData, int lngAddr, int lngSize)
        {
            if (lngAddr + lngSize > m_MemSize) return 2; //超出数据区
            if (m_bInit)
            {
                Marshal.Copy(bytData, lngAddr, m_pwData, lngSize);
            }
            else
            {
                return 1; //共享内存未初始化
            }
            return 0;     //写成功
        }

        /// <summary>
        /// 从共享内存中读取所有数据，返回字节数组。
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            byte[] bts = new byte[maxLength];
            Read(ref bts, 0, bts.Length);
            return bts;
        }

        /// <summary>
        /// 将当前数据写入共享内存。
        /// </summary>
        /// <param name="bts"></param>
        /// <returns></returns>
        public int SetBytes(byte[] bts)
        {
            return Write(bts, 0, bts.Length);
        }
    }
}
