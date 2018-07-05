using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace WindGoes.Net
{
    /// <summary>
    /// 网络命令类，用于数据传输。
    /// </summary>
    public class NetOrder
    {
        public static Dictionary<int, string> Orders = new Dictionary<int, string>();

        static NetOrder()
        {
            Orders.Add(0, "None");
            
        }

        int id = 0;
        /// <summary>
        /// 命令编号。
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        string root = null;
        /// <summary>
        /// 服务器的根路径。
        /// </summary>
        public string Root
        {
            get { return root; }
            set { root = value; }
        }


        string orderName = null;
        /// <summary>
        /// 命令名称。
        /// </summary>
        public string OrderName
        {
            get { return orderName; }
            set { orderName = value; }
        }

        List<string> parameter = new List<string>();
        /// <summary>
        /// 参数列表。
        /// </summary>
        public List<string> Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }

        IPEndPoint senderIPE = null;
        /// <summary>
        /// 发送方IP。
        /// </summary>
        public IPEndPoint SenderIPE
        {
            get { return senderIPE; }
            set { senderIPE = value; }
        }

        IPEndPoint localIPE = null;
        /// <summary>
        /// 本地IP和端口。
        /// </summary>
        public IPEndPoint LocalIPE
        {
            get { return localIPE; }
            set { localIPE = value; }
        }


        byte[] data = null;
        /// <summary>
        /// 文件数据。
        /// </summary>
        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }


        /// <summary>
        /// 网络命令类，用于数据传输。
        /// </summary>
        public NetOrder() { }

        /// <summary>
        /// 网络命令类，用于数据传输。
        /// </summary>
        /// <param name="bts">数据字节数组。</param>
        public NetOrder(byte[] bts)
        {
            LoadFromBytes(bts);
        }

        /// <summary>
        /// 由字符串中读取数据。
        /// </summary>
        /// <param name="bts">数据字节数组，默认长度为1024</param>
        public void LoadFromBytes(byte[] bts)
        {
            MemoryStream ms = new MemoryStream(bts);
            BinaryReader br = new BinaryReader(ms);
            string[] ipe = br.ReadString().Split(':');
            senderIPE = new IPEndPoint(IPAddress.Parse(ipe[0]), int.Parse(ipe[1]));
            orderName = br.ReadString();
            switch (orderName)
            {
                case "FileData":
                    parameter.Add(br.ReadString());
                    parameter.Add(br.ReadString());
                    break;

                case "GetFile":
                case "StringData":
                case "ServerTime":
                case "Error":
                    parameter.Add(br.ReadString());
                    break;
                default:
                    break;
            }

            //1024个字节后面都是文件数据。
            if (ms.Length > 1024)
            {
                ms.Position = 1024;
                data = new byte[ms.Length - 1024];
                ms.Read(data, 0, data.Length); 
            }

            br.Close();
            ms.Close();
        }
    }
}