using System;
using System.Collections.Generic; 
using System.Text;
using System.IO;

namespace WindGoes.Data
{
    public class ErrorManager
    {
        public static string FilePath = "Errors.log";

        /// <summary>
        /// 向文件中写入错误信息，默认文件路径为 "Errors.log"， 可以通过ErrorManager.FilePath设置。
        /// </summary>
        /// <param name="msg"></param>
        public static void AddLog(string msg)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath, true, Encoding.Default))
                {
                    sw.WriteLine(DateTime.Now.ToString() + '\t' + msg);
                }
            }
            catch { }
        }
    }
}
