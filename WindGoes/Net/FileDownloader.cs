using System; 
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace WindGoes.Net
{
    /// <summary>
    /// 数据接收。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="data"></param>
    public delegate void HGetData(object sender, byte[] data);




    /// <summary>
    /// 文件下载器。
    /// </summary>
    public class FileDownloader
    {
        #region 属性
        IPEndPoint localIpe;
        /// <summary>
        /// 本机IP。
        /// </summary>
        public IPEndPoint LocalIpe
        {
            get { return localIpe; }
            set { localIpe = value; }
        }

        IPEndPoint remoteIpe;
        /// <summary>
        /// 远程地址。
        /// </summary>
        public IPEndPoint RemoteIpe
        {
            get { return remoteIpe; }
            set { remoteIpe = value; }
        }

        string filePath = null;
        /// <summary>
        /// 需要下载的文件的路径和名称。
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        string pathToSave = null;
        /// <summary>
        /// 待保存的文件路径。
        /// </summary>
        public string PathToSave
        {
            get { return pathToSave; }
            set { pathToSave = value; }
        }

        int blockSize = 102400;
        /// <summary>
        /// 读取或设置接收文件时的块大小，默认为102400.
        /// </summary>
        public int BlockSize
        {
            get { return blockSize; }
            set { blockSize = value; }
        }


        static int currentPort = 30000;
        /// <summary>
        /// 当前的端口号。
        /// </summary>
        public static int CurrentPort
        {
            get
            {
                currentPort = currentPort > 65500 ? 30000 : currentPort + 1;
                return currentPort;
            }
            set { currentPort = value; }
        }


        bool completeDownload = false;
        /// <summary>
        /// 下载是否已经完成。
        /// </summary>
        public bool Complete
        {
            get { return completeDownload; }
            set { completeDownload = value; }
        }

        bool downloading = false;
        /// <summary>
        /// 是否正在下载。
        /// </summary>
        public bool Downloading
        {
            get { return downloading; }
            set { downloading = value; }
        }

        Thread listenThread;
        int fileSize = 1;
        /// <summary>
        /// 总文件大小。
        /// </summary>
        public int FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        int downloadedSize = 0;
        /// <summary>
        /// 已经下载的数据量。
        /// </summary>
        public int DownloadedSize
        {
            get { return downloadedSize; }
            set { downloadedSize = value; }
        }

        #endregion

        #region 构造函数
        /// <summary>
        /// 文件下载器。
        /// </summary>
        public FileDownloader()
        {
            localIpe = new IPEndPoint(GetLocalIP(), CurrentPort);
        }

        /// <summary>
        /// 文件下载器。
        /// </summary>
        /// <param name="remoteIP">远程Ip地址。</param>
        /// <param name="remotePort">远程的端口。</param>
        public FileDownloader(string remoteIP, int remotePort)
        {
            localIpe = new IPEndPoint(GetLocalIP(), CurrentPort);
            remoteIpe = new IPEndPoint(IPAddress.Parse(remoteIP), remotePort);
        }

        /// <summary>
        /// 文件下载器。
        /// </summary>
        /// <param name="localIP">本地Ip地址。</param>
        /// <param name="localPort">本地的端口。</param>
        /// <param name="remoteIP">远程Ip地址。</param>
        /// <param name="remotePort">远程的端口。</param>
        public FileDownloader(string localIP, int localPort, string remoteIP, int remotePort)
        {
            localIpe = new IPEndPoint(IPAddress.Parse(localIP), localPort);
            remoteIpe = new IPEndPoint(IPAddress.Parse(remoteIP), remotePort);
        }
        #endregion

        /// <summary>
        /// 当文件下载完成以后触发。
        /// </summary>
        public event HRecievedNetData DownloadComplete = null;



        /// <summary>
        /// 下载指定路径的文件。当名称为"XML"时，为下载XML命令。
        /// </summary>
        /// <param name="fp">文件的路径和名称。</param>
        public void DownLoad(string fp)
        {
            if (downloading)
                return;

            try
            {
                localIpe.Port = CurrentPort;
                filePath = fp;
                downloadedSize = 0;
                completeDownload = false;
                listenThread = new Thread(new ThreadStart(download));
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch { }
        }

        /// <summary>
        /// 不指定文件的路径和名称，只表示等待下载状态。
        /// </summary>
        public void DownLoad()
        {
            DownLoad("");
        }

        void download()
        {
            Socket sockets = null;		//用于传输的Socket类。
            byte[] fileData = null;		//文件数据。
            byte[] tempBytes = null;	//临时缓冲数据。

            DataDownloadEvenetArgs e = new DataDownloadEvenetArgs();
            e.RecieverIp = localIpe;
            e.SenderIp = remoteIpe;

            try
            {
                sockets = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sockets.Bind(localIpe);
                sockets.Listen(5);

                //发送连接请求, 某路径没数据，那么变为等待状态。
                if (filePath != null && filePath.Length > 0)
                {
                    DataSender ds = new DataSender(localIpe, remoteIpe);
                    if (filePath.ToLower() == "xml")
                    {
                        ds.SendOrder("XML");
                    }
                    else
                    {
                        ds.SendOrder("GetFile", filePath);
                    }
                }

                //等待接收数据。
                Socket newSocket = sockets.Accept();

                //先读取头文件。
                tempBytes = new byte[1024];
                newSocket.Receive(tempBytes);
                NetOrder no = new NetOrder(tempBytes);



                e.StartTime = DateTime.Now;
                if (no.OrderName == "FileData")
                {
                    //开始接收数据。
                    downloading = true;

                    filePath = no.Parameter[0];
                    fileSize = int.Parse(no.Parameter[1]);

                    //再读取后面的数据。t用于表示每次读取到的数据长度。
                    MemoryStream ms = new MemoryStream();
                    tempBytes = new byte[blockSize];
                    int t = 1;
                    while (t > 0)
                    {
                        t = newSocket.Receive(tempBytes);
                        downloadedSize += t;
                        ms.Write(tempBytes, 0, t);
                    }

                    //接收数据到fileData
                    fileData = ms.ToArray();
                    e.EndTime = DateTime.Now;

                    //关闭所有相关资源。
                    newSocket.Close();
                    sockets.Close();
                    ms.Close();
                    downloading = false;

                    //保存文件。
                    SaveFile(fileData);
                    completeDownload = true;

                    e.Data = fileData;
                    e.Message = "文件接收成功。";
                }
                else if (no.OrderName == "Error")
                {
                    e.Message = no.Parameter[0];
                    e.Data = null;
                }
            }
            catch (Exception e1)
            {
                e.Data = null;
                e.Message = e1.Message;
                e.EndTime = DateTime.Now;
            }

            
            if (DownloadComplete != null)
                DownloadComplete(this, e);
        }

        /// <summary>
        /// 将数据直接保存在本地指定路径下。
        /// </summary>
        /// <param name="data">文件数据。</param>
        public void SaveFile(byte[] data)
        {
            if (data != null)
            {
                string path = pathToSave == null ? filePath : pathToSave + "\\" + filePath;
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
            }
        }

        /// <summary>
        /// 已经完成的下载百分比。
        /// </summary>
        /// <returns></returns>
        public float DownloadPasentage()
        {
            return 1.0f * downloadedSize / fileSize;
        }

   
        /// <summary>
        /// 获取本机Ip地址。
        /// </summary>
        /// <returns></returns>
        public IPAddress GetLocalIP()
        {
            IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            {
                for (int i = 0; i < ips.Length; i++)
                {
                    if (ips[i].ToString().Contains("."))//(!ips[i].IsIPv6LinkLocal)&& 
                    {
                        return ips[i];
                    }
                }
            }
            return ips[0];
        }
    }
}



  

