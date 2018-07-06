using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WindGoes.Data;

namespace WindGoes.Forms
{
    public partial class ConTestFormOld : Form
    {
        private string str = "连接测试中";
        private string configPath = "config.ini";
        private SQLConnection conManager = new SQLConnection();
        private DateTime dt;
        private int t = 0;
        bool connected = false;

        // Properties
        public string ConfigPath
        {
            get
            {
                return this.configPath;
            }
            set
            {
                this.configPath = value;
            }
        }

        public SQLConnection ConManager
        {
            get
            {
                return this.conManager;
            }
            set
            {
                this.conManager = value;
            }
        }

        public bool Connected
        {
            get
            {
                return connected;
            }
            set
            {
                connected = value;
            }
        }

        public ConTestFormOld()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnConnection_Click(object sender, EventArgs e)
        {
            this.conManager = this.rbRemote.Checked ? new SQLConnection(this.txtServer.Text, this.txtDataBase.Text, this.txtUserName.Text, this.txtPassword.Text) : (this.conManager = new SQLConnection(this.txtServer.Text, this.txtDataBase.Text));
            this.btnConnection.Enabled = false;
            this.dt = DateTime.Now;
            this.timer1.Start();
            TimedMutipleThreadTester con = new TimedMutipleThreadTester();
            con.Timeout = (int)this.numericUpDown1.Value;
            //con.ConnectionString = this.conManager.ConnectionString;
           // con.AfterTest += new EventHandler(mt_AfterTest);
            con.StartTest();
        }

        private void ConTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.timer1.Enabled && (MessageBox.Show("连接测试中，确定要退出?", "退出确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
            {
                e.Cancel = true;
            }
            if (this.Connected)
            {
                this.conManager.SaveToIni(this.configPath);
            }
        }

        private void ConTestForm_Load(object sender, EventArgs e)
        {
            if (this.conManager.LoadFromIni(this.ConfigPath))
            {
                this.txtServer.Text = this.conManager.DataSource;
                this.txtDataBase.Text = this.conManager.InitialCatalog;
                this.txtUserName.Text = this.conManager.UserID;
                this.txtPassword.Text = this.conManager.Password;
                if (this.conManager.TrustedConnection)
                {
                    this.rbLocal.Checked = true;
                }
                else
                {
                    this.rbRemote.Checked = true;
                }
            }
        }


        private void mt_AfterTest(object result, Exception e)
        {
            this.btnConnection.Enabled = true;
            this.Connected = (bool) result;
            this.timer1.Stop();
            this.lblResult.Text = Connected ? "连接成功！" : e.Message;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan span = (TimeSpan)(DateTime.Now - this.dt);
            this.lblTotalTime.Text = span.TotalSeconds.ToString("总时间：0.0s");
            this.str = "连接测试中";
            for (int i = 0; i < this.t; i++)
            {
                this.str = this.str + ".";
            }
            this.t = (this.t > 3) ? 0 : ++this.t;
            this.lblResult.Text = this.str;
        }



    }
}
