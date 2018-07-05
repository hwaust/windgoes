using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TestCenter
{
    static class Program
    {
		public static string AppCfgPath = Application.StartupPath + "\\config.ini";
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
			Console.WriteLine(DateTime.Now.ToString());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
