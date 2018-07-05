using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace WindGoes.Net
{
    /// <summary>
    /// 用于数据的发送类。
    /// </summary>
    public class DataSender
    {
        IPEndPoint localIP = null;
        /// <summary>
        /// 本机IP.
        /// </summary>
        public IPEndPoint LocalIP
        {
            get { return localIP; }
            set { localIP = value; }
        }

        IPEndPoint remoteIpe;
        /// <summary>
        /// 远程地址
        /// </summary>
        public IPEndPoint RemoteIpe
        {
            get { return remoteIpe; }
            set { remoteIpe = value; }
        }

        /// <summary>
        /// 数据发送
        /// </summary>
        public DataSender()  { }

        /// <summary>
        /// 数据发送
        /// </summary>
        /// <param name="local"></param>
        /// <param name="remote"></param>
        public DataSender(IPEndPoint local, IPEndPoint remote)
        {
            localIP = local;
            remoteIpe = remote;
        } 
        
        
        /// <summary>
        /// 发送文件。
        /// </summary>
        /// <param name="filePath">文件的路径。</param>
        /// <returns></returns>
        public bool SendFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return false;
                }

                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] bts = new byte[fs.Length];
                fs.Read(bts, 0, bts.Length);
                fs.Close();

                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        bw.Write(localIP.ToString());
                        bw.Write("FileData");
                        bw.Write(filePath);
                        bw.Write(bts.Length.ToString());
                        string Addon = new string('0', (int)(1024 - 2 - ms.Position));
                        bw.Write(Addon);
                        bw.Write(bts);
                    }
                    SendBytes(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); return false;
            }
            return true;
        }

        /// <summary>
        /// 由绝对路径+相对路径组成。如 D:\DongChenOrder  和 bin\MerterialManager.exe 组成。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool SendFile(string path, string file)
        {
            try
            {
                FileStream fs = new FileStream(path + "\\" + file, FileMode.Open, FileAccess.Read);
                byte[] bts = new byte[fs.Length];
                fs.Read(bts, 0, bts.Length);
                fs.Close();

                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        bw.Write(localIP.ToString());
                        bw.Write("FileData");
                        bw.Write(file);
                        bw.Write(bts.Length.ToString());
                        string Addon = new string('0', (int)(1024 - 2 - ms.Position));
                        bw.Write(Addon);
                        bw.Write(bts);
                    }
                    SendBytes(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); return false;
            }
            return true;
        }

        /// <summary>
        /// 向客户端发送一个字符串。
        /// </summary>
        /// <param name="str"></param>
        public void SendString(string str)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(localIP.ToString());
                    bw.Write(str);
                    string Addon = new string('0', (int)(1024 - ms.Position - 2));
                    bw.Write(Addon);
                }
                SendBytes(ms.ToArray());
            }
        }


        /// <summary>
        /// 发送字节数组。
        /// </summary>
        /// <param name="bts"></param>
        /// <returns></returns>
        public bool SendBytes(byte[] bts)
        {
            try
            {
                Socket socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(remoteIpe);
                socket.Send(bts);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 发送命令至服务器，只要加入命令和参数即可，如 SendOrder("XML"); SendOrder("GetFile", "Bin\\Update.xml");
        /// </summary>
        /// <param name="orders"></param> 
        public void SendOrder(params string[] orders)
        {
            //byte[] bts = new byte[1024];
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(localIP.ToString());
                    for (int i = 0; i < orders.Length; i++)
                    {
                        bw.Write(orders[i]);
                    }
                    string Addon = new string('0', (int)(1024 - ms.Position - 2));
                    bw.Write(Addon);
                }
                SendBytes(ms.ToArray());
            }
        }

        //public void SendFile(object no)
        //{

        //}

        //public void SendOrder(NetOrder no)
        //{

        //    switch (no.OrderName)
        //    {
        //        case "GetFile":
        //            string filePath = no.Parameter[0];
        //            if (File.Exists(Root + "\\" + filePath))
        //                ds.SendFile(Root, filePath);
        //            else
        //                ds.SendOrder("Error", "文件 '" + filePath + "' 不存在。");
        //            break;
        //        case "XML": ds.SendFile("Update.Xml"); break;
        //        case "GetServerTime": ds.SendOrder("ServerTime", DateTime.Now.ToString()); break;
        //        case "ServerTime": Console.WriteLine(no.Parameter[0]); break;
        //        default: break;
        //    }
        //}
    }
}
