/*
 * 名称：基于多线程的，数据库连接测试类。
 * 简介：以住的数据库连接中，单线程会造成窗体卡死，现在做成多线程。
 *			 使用的时候先建立类，传送连接字符串，然后添加事件AfterTest的方法，最后Test即可。
 * 
 * 2011-6-17	建立类
 * 
*/

using System;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Forms;

namespace WindGoes.Data
{
    public delegate void MyEvent(bool result, Exception e);

    /// <summary>
    /// 基于多线程的连接测试类，不会造成窗体卡死。
    /// </summary>
    public class MultiThreadSqlCon : ObjectBase
    {
        int timeout = 20;
        /// <summary>
        /// 连接超时时间。
        /// </summary>
        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        /// <summary>
        /// 需要测试的连接字符串，由于网络延时，会有0.5秒左右的误差。
        /// </summary>
        public string ConnectionString { get; set; }

        bool done = false;
        bool connected = false;
        DateTime dt = DateTime.Now;
        Thread t1 = null;
        Thread t2 = null;
        Exception exception;

        /// <summary>
        /// 在连接测试结束时发生的事件，参数只有一个bool型变量，表示是否连接成功。
        /// </summary>
        public event MyEvent AfterTest = null;

        /// <summary>
        /// 数据库连接测试方法。
        /// </summary>
        private void ConTest()
        {
            try
            {
                SqlConnection sql = new SqlConnection(ConnectionString);
                sql.Open();
                sql.Close();
                connected = true;
            }
            catch (Exception e1)
            {
                connected = false;
                exception = e1;
            }

            done = true;
        }


        /// <summary>
        /// 连接测试时的时间控件方法，超时就会自动退出。
        /// </summary>
        private void ThreadController()
        {
            while (!done)
            {
                TimeSpan ts = DateTime.Now - dt;
                if (ts.TotalSeconds > timeout)
                {
                    connected = false;
                    done = true;
                }
                Thread.Sleep(10);
            }

            if (AfterTest != null)
            {
                bool b = Control.CheckForIllegalCrossThreadCalls;
                System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
                exception = exception ?? new Exception("连接超时，可能是服务器地址不正确。");
                AfterTest(connected, exception);
                Control.CheckForIllegalCrossThreadCalls = b;
            }
            try
            {
                t1.Abort(); //线程的关闭需要时间，尤其是关闭测试数据库连接的线程。
            }
            catch { }
        }

        /// <summary>
        /// 开始测试。
        /// </summary>
        public void StartTest()
        {
            done = false;
            connected = false;
            dt = DateTime.Now;
            t1 = new Thread(new ThreadStart(ConTest));
            t1.IsBackground = true;
            t1.Start();

            t2 = new Thread(new ThreadStart(ThreadController));
            t2.IsBackground = true;
            t2.Start();
        }

        /// <summary>
        /// 强制停止测试。
        /// </summary>
        public void ForceStop()
        {
            try
            {
                t1.Abort();
                t2.Abort();
            }
            catch { }
        }


    }
}
