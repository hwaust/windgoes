using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace WindGoes.Net
{
    /// <summary>
    /// 收到下载数据的参数类。
    /// </summary>
    public class DataDownloadEvenetArgs : EventArgs
    {
        /// <summary>
        /// 事件附带的信息。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 文件数据。
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 开始接收数据时间。
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 完成接收数据时间。
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 发送数据方IP地址和端口。
        /// </summary>
        public IPEndPoint SenderIp { get; set; }

        /// <summary>
        /// 数据接收方IP地址和端口。
        /// </summary>
        public IPEndPoint RecieverIp { get; set; }
    }
}
