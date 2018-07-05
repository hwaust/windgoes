using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace WindGoes.Net
{
    /// <summary>
    /// NetOrder处理类，专门用于命令类的处理。
    /// </summary>
    class NetOrderDealer
    {
        Thread t;

        NetOrder order = null;
        /// <summary>
        /// 命令。
        /// </summary>
        public NetOrder Order
        {
            get { return order; }
            set { order = value; }
        }

        /// <summary>
        /// NetOrder处理类，专门用于命令类的处理。
        /// </summary>
        /// <param name="no"></param>
        public NetOrderDealer(NetOrder no)
        {
            order = no;
        }

        public void Run()
        {
            try
            {
                t = new Thread(new ThreadStart(dealer));
                t.IsBackground = true;
                t.Start();
            }
            catch (Exception)
            {

                throw;
            }
        }

        void dealer()
        {
            DataSender ds = new DataSender(order.LocalIPE, order.SenderIPE);

            switch (order.OrderName)
            {
                case "GetFile":
                    ds.SendFile(order.Root, order.Parameter[0]);
                    break;

                case "XML": 
                    ds.SendFile("Update.Xml"); 
                    break;

                case "GetServerTime": 
                    ds.SendOrder("ServerTime", DateTime.Now.ToString()); 
                    break;

                case "ServerTime":
                    Console.WriteLine(order.Parameter[0]); 
                    break;
                case "FileData": SaveFile();  break;

 
            }
        }


        private void SaveFile()
        {
            try
            {
                string path = Path.Combine(order.Root, order.Parameter[0]);
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(order.Data, 0, order.Data.Length);
                fs.Close();
            }
            catch { }
        }


    }
}
