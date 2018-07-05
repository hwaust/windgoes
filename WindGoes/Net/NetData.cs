using System;
using System.Collections.Generic;
using System.Text;

namespace WindGoes.Net
{
    /// <summary>
    /// 数据下载代理。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void HRecievedNetData(object sender, DataDownloadEvenetArgs e);

    /// <summary>
    /// 命令收到代理。
    /// </summary>
    /// <param name="sender">命令接收方。</param>
    /// <param name="e">命令参数。</param>
    public delegate void RecievedNetOrderHandler(object sender, NetOrder e);
}
