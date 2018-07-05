using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace WindGoes.Net
{
    /// <summary>
    /// 网络服务器类，用于向目标地址提供数据服务。
    /// </summary>
    public class NetServer 
    {
        Thread listenThread;//监听线程

        /// <summary>
        /// 接收到网络参数后的命令。
        /// </summary>
        public event RecievedNetOrderHandler RecievedOrder = null;

        private IPEndPoint localIpe = null;    
        /// <summary>
        /// 本机服务器IP地址及端口
        /// </summary>
        public IPEndPoint LocalIpe
        {
            get { return localIpe; }
            set { localIpe = value; }
        }


        string destFolder = "D:\\DongChenOrder";
        /// <summary>
        /// 需要更新的目录。
        /// </summary>
        public string Root
        {
            get { return destFolder; }
            set { destFolder = value; }
        }


        /// <summary>
        /// 网络服务器类，用于向目标地址提供数据服务。
        /// </summary>
        /// <param name="local">本机的地址和端口。</param>
        public NetServer(IPEndPoint local)
        {
            localIpe = local;
        }

        /// <summary>
        /// 循环监听 
        /// </summary>
        void listen()
        {
            Socket sockets;
            byte[] data;

            try
            {
                sockets = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sockets.Bind(localIpe);
            }
            catch { return; }

            while (true)
            {
                try
                {
                    sockets.Listen(200);
                    Socket newSocket = sockets.Accept();  
                    using (MemoryStream ms = new MemoryStream())
                    {
                        byte[] btMsg = new byte[10240];  
                        int t = 1;
                        while (t > 0)
                        {
                            t = newSocket.Receive(btMsg);
                            ms.Write(btMsg, 0, t);
                        }
                        data = ms.ToArray();
                    } 
                    newSocket.Close();

                    NetOrder no = new NetOrder(data);
                    no.Root = Root; 
                    no.LocalIPE = localIpe;
 
 
                    new NetOrderDealer(no).Run();

                    if (RecievedOrder != null)
                    {
                        RecievedOrder(this, no);                       
                    }

                    Thread.Sleep(1);
                }
                catch (SocketException ex)  { Console.WriteLine(ex.Message); }
            }
        }

        /// <summary>
        /// 启动线程
        /// </summary>
        public void Run()
        {
            try
            {
                listenThread = new Thread(new ThreadStart(listen));
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch { }
        } 
    }
}
