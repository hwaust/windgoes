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
using WindGoes.Sys;

namespace WindGoes.Data
{
    /// <summary>
    /// 基于多线程的连接测试类，不会造成窗体卡死。
    /// </summary>
    public class MultiThreadSqlCon : ObjectBase
    {
        /// <summary>
        /// 连接超时时间，根据网络延时，会有一定误差。
        /// </summary>
        public int Timeout { get; set; } = 20;

        bool done = false;

        Thread testThread = null;
        Thread controllerThread = null;

        Exception exception;

        /// <summary>
        /// 在连接测试结束时发生的事件，参数只有一个bool型变量，表示是否连接成功。
        /// </summary>
        public event EventHandler AfterTest = null;

        /// <summary>
        /// Event to be tested.
        /// </summary>
        public event EventHandler Test = null;

        /// <summary>
        /// 数据库连接测试方法。
        /// </summary>
        private void ConTest()
        {
            done = false;
            try
            {
                Test?.Invoke(null, null);
            }
            catch (Exception e)
            {
                exception = e;
            }
            done = true;
        }


        /// <summary>
        /// 连接测试时的时间控件方法，超时就会自动退出。
        /// </summary>
        private void ThreadController()
        {
            // waiting
            DateTime startingTime = DateTime.Now;
            while (!done)
            {
                TimeSpan ts = DateTime.Now - startingTime;
                done = ts.TotalSeconds > Timeout;
                Thread.Sleep(1);
            }

            // invoke
            AfterTest?.Invoke(this, new TestEventArgs(null, exception ?? new Exception("连接超时，可能是服务器地址不正确。")));

            //线程的关闭需要时间，尤其是关闭测试数据库连接的线程。
            testThread.Abort();
        }

        /// <summary>
        /// 开始测试。
        /// </summary>
        public void StartTest()
        {
            testThread = new Thread(new ThreadStart(ConTest));
            testThread.IsBackground = true;
            testThread.Start();

            controllerThread = new Thread(new ThreadStart(ThreadController));
            controllerThread.IsBackground = true;
            controllerThread.Start();
        }

        /// <summary>
        /// 强制停止测试。
        /// </summary>
        public void ForceStop()
        {
            try
            {
                testThread.Abort();
                controllerThread.Abort();
            }
            catch { }
        }


    }
}
