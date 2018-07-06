using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WindGoes.Data;
using System.Threading;
using WindGoes.IO;
using WindGoes.Database;
using WindGoes.Sys;

namespace WindGoes.Forms
{
    /// <summary>
    /// 用于数据库连接测试，及数据库连接字符字符串管理。
    /// </summary>
    public partial class ConTestForm : BaseForm
    {
        /// <summary>
        /// Used to measure the connecting time.
        /// </summary>
        DateTime connectionTiming;

        /// <summary>
        /// 用于数据库连接测试，及数据库连接字符字符串管理。
        /// </summary>
        public ConTestForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 保存的文件路径。
        /// </summary>
        public string FilePath { get; set; } = ".\\data.bin";

        /// <summary> 
        /// 连接字符串对象。
        /// </summary>
        public SQLConnection Connection { get; set; }

        /// <summary>
        /// 当前窗体是否已经连接成功。
        /// </summary>
        public bool Connected { get; set; }


        // 用于保存已经存在的数据库连接字符串信息。
        List<SQLConnection> cms = new List<SQLConnection>();


        private void ConTestForm_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            IniAccess.CreateFile(FilePath);

            cbConnectionType.SelectedIndex = 0;

            string[] constrs = File.ReadAllLines(FilePath, Encoding.Default);
            foreach (string constr in constrs)
            {
                SQLConnection sm = SQLConnection.FromDesString(constr, true);
                if (sm != null)
                {
                    cms.Add(sm);
                    cbServer.Items.Add(sm.DataSource);

                }
            }

 

            // init UI
            if (cms.Count > 0)
            {
                Connection = cms[0]; 
                cbDatabase.Items.Add(Connection.InitialCatalog);
                cbDatabase.SelectedIndex = 0;
                btnSave.Enabled = true;
            }

            UpdateUI();
        }

        private void btnConnection_Click(object sender, EventArgs e)
        {
            UpdateConnection();

            btnConnection.Enabled = false;
            connectionTiming = DateTime.Now;
            timer1.Start();


            DBManager.ConnectionString = Connection.ConnectionString;
            MultiThreadSqlCon mt = new MultiThreadSqlCon((int)numTimeoutInSecond.Value);
            mt.Test += Mt_Test;
            mt.AfterTest += mt_AfterTest;
            mt.StartTest();
        }


        private void Mt_Test(object sender, EventArgs e)
        {
            DBManager dm = new DBManager();
            dm.CurrentConnection = Connection.ConnectionString;
            dm.OpenCn();
            dm.CloseCn();
            Connected = true;
        }

        void mt_AfterTest(object sender, EventArgs e)
        {
            btnConnection.Enabled = true;
            if (Connected)
            {
                cbDatabase.Enabled = true;
                cbDatabase.Items.Clear();
                cbDatabase.Items.AddRange(new DBManager().GetAllDatabaseNames());
            }
            timer1.Stop();

            UpdateConnection();
            if (Connected)
            {
                MessageBox.Show("数据库连接成功。\n请选择需要连接的数据库名称。", "连接成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                TestEventArgs args = e as TestEventArgs;
                MessageBox.Show("数据库连接失败，原因如下：\n" + args.Exception?.Message, "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        // 
        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - connectionTiming;
            lblTotalTime.Text = ts.TotalSeconds.ToString("总时间：0.0s");
            string s1 = "连接测试中";
            int t = 0;
            for (int i = 0; i < t; i++)
                s1 += ".";
            t = t > 3 ? 0 : t += 1;
            lblResult.Text = s1;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ConTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timer1.Enabled)
            {
                DialogResult dr = MessageBox.Show("连接测试中，确定要退出?", "退出确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

            Control.CheckForIllegalCrossThreadCalls = true;
        }


        private void cbConnectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateConnection();
        }

        private void UpdateConnection()
        {
            txtUserName.Enabled = cbConnectionType.SelectedIndex == 1;
            txtPassword.Enabled = cbConnectionType.SelectedIndex == 1;
            string db = cbDatabase.Text.Length > 0 ? cbDatabase.Text : "master";
            Connection = cbConnectionType.SelectedIndex == 0 ?
                new SQLConnection(cbServer.Text, db) :
                new SQLConnection(cbServer.Text, db, txtUserName.Text, txtPassword.Text);
        }


        private void cbServer_TextChanged(object sender, EventArgs e)
        {
            btnConnection.Enabled = cbServer.Text.Length > 0;
            Connected = false;
            UpdateConnection();

            for (int i = 0; i < cms.Count; i++)
            {
                if (cms[i].DataSource.ToLower().Trim() == cbServer.Text.ToLower().Trim())
                {
                    Connection = cms[i];
                    UpdateUI();
                    return;
                }
            }

            Connection = null;
            UpdateUI();

        }


        private void UpdateUI()
        {
            if (Connection == null)
            {
                txtUserName.Clear();
                txtPassword.Clear();
                cbDatabase.Text = "";
            }
            else
            {
                cbConnectionType.SelectedIndex = Connection.TrustedConnection ? 0 : 1;
                if (!Connection.TrustedConnection)
                {
                    txtUserName.Text = Connection.UserID;
                    txtPassword.Text = Connection.Password;
                }
                cbDatabase.Text = Connection.InitialCatalog;
            }
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            Connected = false;
            UpdateConnection();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            Connected = false;
            UpdateConnection();
        }

        private void btGetConnectionString_Click(object sender, EventArgs e)
        {
            try
            {
                string tempfile = Application.StartupPath + "\\tmp.txt";
                File.WriteAllText(tempfile, Connection.ConnectionString, Encoding.Default);
                System.Diagnostics.Process.Start("NotePad.exe", tempfile);
                Application.DoEvents();
                Thread.Sleep(500); // if not wait, the file will be deleted before displayed.
                File.Delete(tempfile);
            }
            catch (Exception e1) { Console.WriteLine(e1.Message); }
        }

        private void cbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateConnection();
            if (cbDatabase.SelectedIndex >= 0)
            {
                btnSave.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //1.若为空，表示为新连接。
            if (Connection == null)
            {
                Connection = new SQLConnection();
                Connection.DataSource = cbServer.Text.Trim();
                Connection.TrustedConnection = cbConnectionType.SelectedIndex == 0;
                if (!Connection.TrustedConnection)
                {
                    Connection.UserID = txtUserName.Text;
                    Connection.Password = txtPassword.Text;
                }
                Connection.InitialCatalog = cbDatabase.Text;
                cms.Add(Connection);
            }
            else
            {
                Connection.DataSource = cbServer.Text.Trim();
                Connection.TrustedConnection = cbConnectionType.SelectedIndex == 0;
                if (!Connection.TrustedConnection)
                {
                    Connection.UserID = txtUserName.Text;
                    Connection.Password = txtPassword.Text;
                }
                Connection.InitialCatalog = cbDatabase.Text;
            }

            //2.移除原来队伍中已经存在的conManager
            for (int i = 0; i < cms.Count; i++)
            {
                if (cms[i].DataSource == Connection.DataSource)
                {
                    cms.RemoveAt(i);
                    break;
                }
            }

            //3.写文件，其中conManager先写入，方便下次读取时自动关联。
            using (StreamWriter sw = new StreamWriter(FilePath, false, Encoding.Default))
            {
                sw.WriteLine(Connection.ToDESString(true));
                for (int i = 0; i < cms.Count; i++)
                {
                    sw.WriteLine(cms[i].ToDESString(true));
                }
            }

            //4.关闭窗体
            Close();
        }
    }
}
