/*
 * 名称：可以在指定的时间内对某算法进行测试，如果超时则停止。
 * 简介：本类中有两个事件：Test 事件即测试事件，AfterTest即测试后执行的事件。
 *  
 * 2011-06-17       建立类
 * 2018-07-06       大幅修改此类，将测试方法由写死改为方法， 同时对事件参数(TestEventArgs) 进行扩展。
 * 
*/

using System;
using System.Threading;
using WindGoes.Sys;

namespace WindGoes.Data
{
    /// <summary>
    /// 基于多线程的连接测试类，不会造成窗体卡死。
    /// </summary>
    public class TimedMutipleThreadTester : ObjectBase
    {
        /// <summary>
        /// 超过此时间后，测试事件将停止，默认值为20秒。
        /// </summary>
        public int Timeout { get; set; } = 20;
         
        bool isRunning = false;
        Thread testThread = null;
        Thread controllerThread = null;
        Exception exception; 

        /// <summary>
        /// Initialize with default timeout 20 second.
        /// </summary>
        public TimedMutipleThreadTester() { }

        /// <summary>
        /// Initialize with timeout (in second)
        /// </summary>
        /// <param name="timeout">Timeout</param>
        public TimedMutipleThreadTester(int timeout)
        {
            Timeout = timeout;
        }

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
        private void test()
        {
            isRunning = true;
            try
            {
                Test?.Invoke(null, null);
            }
            catch (Exception e)
            {
                exception = e;
            }
            isRunning = false;
        }

        /// <summary>
        /// 连接测试时的时间控件方法，超时就会自动退出。
        /// </summary>
        private void threadController()
        {
            DateTime startingTime = DateTime.Now;
            while (isRunning)
            {
                isRunning = (DateTime.Now - startingTime).TotalSeconds < Timeout;
                Thread.Sleep(1);
            }

            // about test thread if still running.
            if (isRunning)
                testThread.Abort();

            // invoke AfterTest
            AfterTest?.Invoke(this, new TestEventArgs(true, exception));
        }

        /// <summary>
        /// 开始运行测试。
        /// </summary>
        public void StartTest()
        {
            testThread = new Thread(new ThreadStart(test));
            testThread.IsBackground = true;
            testThread.Start();

            controllerThread = new Thread(new ThreadStart(threadController));
            controllerThread.IsBackground = true;
            controllerThread.Start();
        }

        /// <summary>
        /// 停止测试。
        /// </summary>
        public void Stop()
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
