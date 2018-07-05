using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WindGoes.Net;
using System.Net;

namespace WindGoes.Forms
{
    public partial class NetSendForm : Form
    {
        IPEndPoint remote = null;
        IPEndPoint local = null;
        FileDownloader down = new FileDownloader();
        DataSender ds = null;

        public NetSendForm()
        {
            InitializeComponent();
            down.DownloadComplete += new HRecievedNetData(down_DownloadComplete);
        }

        void down_DownloadComplete(object sender, DataDownloadEvenetArgs e)
        {
            MessageBox.Show(e.Message);
        }

        bool updateIP()
        {
            try
            {
                remote = new IPEndPoint(IPAddress.Parse(txtServerIP.Text), 56789);
                local = new IPEndPoint(IPAddress.Parse(txtLocalIP.Text), 30000);
                down.RemoteIpe = remote;
                down.LocalIpe = local;
                ds = new DataSender(local, remote);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            updateIP(); 
            down.DownLoad(txtFilePath.Text);
        }

        private void btnExcuteOrder_Click(object sender, EventArgs e)
        {
            updateIP();
            ds.SendOrder(txtOrder.Text);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            updateIP();

            if (ds.SendFile(txtFilePath.Text))
            {
                MessageBox.Show("发送成功。");
            }
            else
            {
                MessageBox.Show("发送失败。");
            }
        }

        private void NetSendForm_Load(object sender, EventArgs e)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.1.100"), 30000);
            NetServer ns = new NetServer(ip);
            ns.RecievedOrder += new RecievedNetOrderHandler(ns_RecievedOrder);
            ns.Run(); 
        }

        void ns_RecievedOrder(object sender, NetOrder e)
        {
            Console.WriteLine();
        }
    }
}
