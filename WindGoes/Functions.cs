/*
 * 名称：生成随机字符串。
 * 
 * 作用：生成随机字符串，用于随机测试数据。
 * 2007-2-19 用于吴焱的论文。
 * 
 * 时间：2011-7-2   建立 public void IsSingleton(string appId) 方法用于判断程序的唯一性。
 *       2008.2.1   初步建立这几个方法。 
 *       
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using WindGoes.Sys;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WindGoes
{
    /// <summary>
    /// 一些常用的函数。
    /// </summary>
    public class Functions
    {
        /// <summary>
		/// 日志文件所在的路径。默认为Errors.log
        /// </summary>
        public static string FilePath = "Errors.log";

        //用于随机生成数字
        static Random rnd = new Random(Environment.TickCount);
        /// <summary>
        /// 随机类的对象。
        /// </summary>
        public static Random Rnd
        {
            get { return Functions.rnd; }
            set { Functions.rnd = value; }
        }


        /// <summary>
        /// 用于测试一个程序是否是唯一实例，通过字符串进行限制。
        /// </summary>
        /// <param name="appId"></param>
        public static bool IsSingleton(string appId)
        {
            MemoryShare ms = new MemoryShare();
            ms.Init(appId);
            byte[] bts = new byte[ms.MaxLength];
            ms.Read(ref bts, 0, bts.Length);

            bool b = true ;
            for (int i = 0; i < bts.Length; i++)
            {
                if (bts[i] > 0)
                {
                    b = false ;
                    break;
                }
            }

            if (b)
            {
                for (int i = 0; i < bts.Length; i++)
                {
                    bts[i] = 1;
                } 
                ms.Write(bts, 0, bts.Length);
            }

            return b;
        }

        /// <summary>
        /// 向文件中写入字符串信息，默认文件路径为 "Errors.log"， 可以通过Functions.FilePath设置。
        /// </summary>
        /// <param name="msg"></param>
        public static void AddLogToFile(string msg)
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

        /// <summary>
        /// 根据进程名称，关闭指定进程。
        /// </summary>
        /// <param name="name">进程名称。</param>
        /// <returns></returns>
        public static bool KillProcessByName(string name)
        {
            try
            {
                Process[] processes = System.Diagnostics.Process.GetProcesses();
                foreach (Process p in processes)
                {
                    if (p.ProcessName == name)
                    {
                        p.Kill();
                    }
                }
                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 获得指定名称的进程的ID。
        /// </summary>
        /// <param name="name">进程名称。</param>
        /// <returns></returns>
        public static int GetProcessIDByName(string name)
        {
            try
            {
                Process[] processes = System.Diagnostics.Process.GetProcesses();
                foreach (Process p in processes)
                {
                    if (p.ProcessName == name)
                    {
                        return p.Id;
                    }
                }
            }
            catch { }

            return -1;
        }


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

        /// <summary>
        /// 发送消息，只有在Debug模式下才会生效。
        /// </summary>
        /// <param name="traceMessage">待发消息。</param>
        [Conditional("DEBUG")]
        public static void Trace(string traceMessage)
        {
            Console.WriteLine("[TRACE] - " + traceMessage);
        }

    }
}