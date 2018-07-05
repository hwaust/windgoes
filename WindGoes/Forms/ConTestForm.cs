using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WindGoes.Data;
using System.Threading;
using WindGoes.IO;
using WindGoes.Database;

namespace WindGoes.Forms
{
    /// <summary>
    /// 用于数据库连接测试，及数据库连接字符字符串管理。
    /// </summary>
    public partial class ConTestForm : BaseForm
    {
        /// <summary>
        /// 用于数据库连接测试，及数据库连接字符字符串管理。
        /// </summary>
        public ConTestForm()
        {
            InitializeComponent();
            FilePath = ".\\data.bin";
        }

        /// <summary>
        /// 保存的文件路径。
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 是否连接成功。
        /// </summary>  
        SQLConnection conManager = new SQLConnection();

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
            IniAccess.CreateFile(FilePath);

            cbConnectionType.SelectedIndex = 0;

          string[] constrs=   File.ReadAllLines(FilePath, Encoding.Default);
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
                cbServer.Text = cms[0].DataSource;

            AdjustUI();
        }

        private void btnConnection_Click(object sender, EventArgs e)
        {
            AdjustUI();

            btnConnection.Enabled = false;
            dt = DateTime.Now;
            timer1.Start();


            MultiThreadSqlCon mt = new MultiThreadSqlCon();
            DBManager.ConnectionString = Connection.ConnectionString;
            mt.ConnectionString = Connection.ConnectionString;
            mt.Timeout = (int)numTimeoutInSecond.Value;
            mt.AfterTest += new MyEvent(mt_AfterTest);
            mt.StartTest();
        }

        void mt_AfterTest(bool result, Exception e)
        {
            btnConnection.Enabled = true;
            Connected = result;
            if (Connected)
            {
                cbDatabase.Enabled = true;
                DBManager dm = new DBManager();

                cbDatabase.Items.Clear();
                cbDatabase.Items.AddRange(dm.GetAllDatabaseNames());
            }
            timer1.Stop();
            AdjustUI();
            if (result)
            {
                MessageBox.Show("数据库连接成功。\n     >> 请选择需要连接的数据库名称 <<", "连接成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("数据库连接失败，原因如下：\n" + e.Message, "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        DateTime dt; 

        // 
        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - dt;
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
        }
         

        private void cbConnectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdjustUI();
        }

        private void AdjustUI()
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
            btnConnection.Enabled = cbServer.Text.Length == 0 ? false : true;
            Connected = false;
            AdjustUI();

            for (int i = 0; i < cms.Count; i++)
            {
                if (cms[i].DataSource.ToLower().Trim() == cbServer.Text.ToLower().Trim())
                {
                    Connection = cms[i];
                    AttachUI(Connection);
                    return;
                }
            }

            Connection = null;
            AttachUI(Connection);

        }


        private void AttachUI(SQLConnection cm)
        {
            if (cm == null)
            {
                txtUserName.Clear();
                txtPassword.Clear();
                cbDatabase.Text = "";
            }
            else
            {
                cbConnectionType.SelectedIndex = cm.TrustedConnection ? 0 : 1;
                if (!cm.TrustedConnection)
                {
                    txtUserName.Text = cm.UserID;
                    txtPassword.Text = cm.Password;
                }
                cbDatabase.Text = cm.InitialCatalog;
            }
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            Connected = false;
            AdjustUI();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            Connected = false;
            AdjustUI();
        }

        private void btGetConnectionString_Click(object sender, EventArgs e)
        {
            try
            {
                string path1 = @"C:\tmp.dat";
                using (StreamWriter sw = new StreamWriter(path1, false, Encoding.Default))
                {
                    sw.WriteLine("");
                }
                System.Diagnostics.Process.Start("NotePad.exe", path1);
                Application.DoEvents();
                Thread.Sleep(1000);
                File.Delete(path1);
            }
            catch (Exception e1) { Console.WriteLine(e1.Message); }
        }

        private void cbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdjustUI();
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
                if (cms[i].DataSource == conManager.DataSource)
                {
                    cms.RemoveAt(i);
                    break;
                }
            }

            //3.写文件，其中conManager先写入，方便下次读取时自动关联。
            using (StreamWriter sw = new StreamWriter(FilePath, false, Encoding.Default))
            {
                sw.WriteLine(conManager.ToDESString(true));
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
