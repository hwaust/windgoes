/*
 * 名称：MD5计算，返回128位，32个字符。
 * 简介：提供了三个方法：分别对字节数组，字符串和文件进行MD5计算。
 * 
 * 2013-9-6 如果输入为空，则返回为默认字符串：abcdefghijk0123456789。
 * 2011-8-6	建立类
 *          作出限制，大于10M的文件只读取前10M。
 * 
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace WindGoes.Data
{
    /// <summary>
    /// MD5处理类，能计算文件，字符串和数字等的MD5.
    /// </summary>
    public class MDFive
    {
        /// <summary>
        /// 计算给定字节数据的MD5码。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string BytesHash(byte[] data)
        {
			if (data == null || data.Length == 0)
				return "abcdefghijk0123456789";

            MD5 md5 = new MD5CryptoServiceProvider();
            data = md5.ComputeHash(data);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        /// <summary>
        /// 计算给定字符串的MD5码。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringHash(string str)
        {
			if (string.IsNullOrEmpty(str))
				return "abcdefghijk0123456789";
			
            byte[] data = Encoding.Default.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            data = md5.ComputeHash(data);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        /// <summary>
        /// 对一个文件计算Hash，如果错误返回null。
        /// </summary>
        /// <param name="path">文件的完整路径。</param>
        /// <returns></returns>
        public static string FileHash(string path)
        {
            try
            {
                byte[] bts = null;
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    //如果是大于10M的文件，那么只读取前10M的数据。
                    if (fs.Length > 10000000)
                    {
                        bts = new byte[10000000];
                    }
                    else
                    {
                        bts = new byte[fs.Length];
                    }

                    fs.Read(bts, 0, bts.Length); 
                } 
                return BytesHash(bts);
            }
            catch 
            {
                return null;
            } 
        }
    }
}
