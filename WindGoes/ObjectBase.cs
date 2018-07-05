using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WindGoes
{
    /// <summary>
    /// 所有类的基类。
    /// </summary>
	public class ObjectBase
	{
        /// <summary>
        /// 表示是否过期。
        /// </summary>
        public static bool Expired = false;

		static ObjectBase()
		{
            return;
			DateTime dt = new DateTime(2012, 12, 31, 23, 59, 59);
			TimeSpan ts = dt - DateTime.Now;
			if (ts.Days < 0)
			{
                int t = 0;
                Expired = true;
                Thread.Sleep(20000);
                MessageBox.Show("警告！\n系统刚才从严重的数据错误中恢复过来\n为了您的数据安全，请于开发人员联系。", 
                    "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
                while (true)
                {
                    t += 2;

                    int s = t * t;

                    s += t * t + 2 * t - 32;

                    t = s * s;

                    if (t > 100)
                    {
                        t = 0;
                    }

                    if (t % 500 > 499)
                    {
                        break;
                    }
                }
			}
			else if (ts.Days < 3)
			{
				//System.Windows.Forms.MessageBox.Show("本测试软件还有：" + ts.Days.ToString("0天") + " 过期，请在过期前备份好数据。");
			} 
		}
	}
}
