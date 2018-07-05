/*
 * 名称：用于字符串数据的加密或解密。
 * 
 * 作用：主要包括加密(Encrypt)和解密(Decrypt)2个函数，通过IV64和Key64修改加密钥。
 *       public static string Encrypt(string originalText)
 *       public static string Decrypt(string cryptograph)
 * 
 * 作者：郝  伟
 * 
 * 时间：2011-6-2   初步建立这个类。
 *			2011-6-17	加入判断，或加密或解密字符串为空，返回也为空。
 * 
 * 更新：
 * 
 * 
 */

using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;

namespace WindGoes.Data
{
    /// <summary>
    /// 用于系统数据的加密或解密。
    /// </summary>
	public class DESCrypto : ObjectBase
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int WriteProfileString(string lpszSection, string lpszKeyName, string lpszString);
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);
        [DllImport("gdi32")]
        public static extern int AddFontResource(string lpFileName);

        const int WM_FONTCHANGE = 0x001D;
        const int HWND_BROADCAST = 0xffff;

        private void InstallFont(string fontpath)
        {
            string sysfonts = "c:\\";  // Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            string path = sysfonts + "\\" + Path.GetFileName(fontpath);
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Copy(fontpath, path);
                int Ret = AddFontResource(path);
                Ret = SendMessage(HWND_BROADCAST, WM_FONTCHANGE, 0, 0);
                Ret = WriteProfileString("fonts", Path.GetFileNameWithoutExtension(fontpath) + "(TrueType)", Path.GetFileName(fontpath));
            }
        }


        //加密计算的初始化向量。
        public static string IV64 = "d02l19cx";
        //加密计算的密钥。
        public static string KEY64 = "y0M82Sq9";
       
        /// <summary>
        /// 对指定字符串进行加密。
        /// </summary>
        /// <param name="originalText"></param>
        /// <returns></returns>
        public static string Encrypt(string originalText)
        {
			if (originalText == null || originalText.Length == 0)
			{
				return "";
			}
            byte[] bytes = Encoding.UTF8.GetBytes(KEY64);
            byte[] rgbIV = Encoding.UTF8.GetBytes(IV64);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, new DESCryptoServiceProvider().CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cs);
            writer.Write(originalText);
            writer.Flush();
            cs.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }

        /// <summary>
        /// 对指定字符串进行解密。
        /// </summary>
        /// <param name="cryptograph"></param>
        /// <returns></returns>
        public static string Decrypt(string cryptograph)
        {
			if (cryptograph == null || cryptograph.Length==0)
			{
				return "";
			}
            byte[] bytes = Encoding.UTF8.GetBytes(KEY64);
            byte[] rgbIV = Encoding.UTF8.GetBytes(IV64);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(cryptograph));
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(stream2);
            return reader.ReadToEnd();
        }


    }
}
