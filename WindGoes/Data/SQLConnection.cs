/*
 * 名称：连接字符串自动生成类
 * 
 * 作用：根据传入参数生成连接字符串。
 *
 * 
 * 时间：2010-3-21  初步建立这个类。
 *       2011-6-3   添加向Ini文件读写的方法。
 *       2011-8-7   添加TestConnection方法。
 * 更新：无
 * 
 * 
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using WindGoes.IO;
using System.Windows.Forms;

namespace WindGoes.Data
{
    /// <summary>
    /// Connections 的摘要说明
    /// </summary>
	public class SQLConnection : ObjectBase
    {
        #region 变量和属性

        //Data Source = newbjb\SqlExpress;Initial Catalog = Tickets;User ID =sa; Password =317659;
        string dataSource, initialCatalog, userID, password;
        bool trustedConnection = true;

        /// <summary>
        /// 数据源
        /// </summary>
        public string DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public string InitialCatalog
        {
            get { return initialCatalog; }
            set { initialCatalog = value; }
        }

        /// <summary>
        /// 用户帐号
        /// </summary>
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        /// <summary>
        /// 用户密码。
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// 用户密码。
        /// </summary>
        public bool TrustedConnection
        {
            get { return trustedConnection; }
            set { trustedConnection = value; }
        }

        /// <summary>
        /// 最终生成的数据库连接字符串。
        /// </summary>
        public string ConnectionString
        {
            get
            {
                //如果是本地连接
                if (trustedConnection)
                    return string.Format("Data Source = {0};Initial Catalog = {1};Trusted_Connection = true;",
                        dataSource, initialCatalog);

                //如果是远程连接
                return string.Format("Data Source = {0};Initial Catalog = {1};User ID = {2}; Password = {3};",
                    dataSource, initialCatalog, userID, password);
            }
        }
        #endregion
        private void InitData(string ds, string ic, string ui, string pw, bool tc)
        {
            dataSource = ds;
            initialCatalog = ic;
            userID = ui;
            password = pw;
            trustedConnection = tc;
        }

        /// <summary>
        /// 用于生成连接字符串的类。
        /// </summary>
        /// <param name="ic">设置需要连接的数据库名称</param>
        public SQLConnection(string ic)
        {
            //InitData("192.168.1.25", ic, "hwaust", "abc123", false);
            InitData(Dns.GetHostName() + "\\sqlexpress", ic, "", "", true);
        }

        /// <summary>
        /// 用于生成连接字符串的类。
        /// </summary>
        /// <param name="datasource">需要用于连接的服务器的名称</param>
        /// <param name="initialcatalog">设置需要连接的数据库名称</param>
        public SQLConnection(string datasource, string initialcatalog)
        {
            InitData(datasource, initialcatalog, "", "", true);
        }

        /// <summary>
        /// 用于生成连接字符串的类。
        /// </summary>
        public SQLConnection() { }

        /// <summary>
        /// 用于生成连接字符串的类。
        /// </summary>
        /// <param name="ds">需要用于连接的服务器的名称</param>
        /// <param name="ic">设置需要连接的数据库名称</param>
        /// <param name="ui">连接数据库所需要的帐号</param>
        /// <param name="pw">连接数据库所需要的帐号的密码。</param>
        public SQLConnection(string ds, string ic, string ui, string pw)
        {
            InitData(ds, ic, ui, pw, false);

        }


        /// <summary>
        /// 将连接字符串保存到INI文件中。用户名密码会自动加密。
        /// </summary>
        /// <param name="iniPath"></param>
        /// <returns></returns>
        public bool SaveToIni(string iniPath)
        {
            try
            {
                IniAccess.CreateFile(iniPath);
                IniAccess ia = new IniAccess(iniPath);
                ia.Section = "System";
                ia.WriteValue("DataSource", DataSource);
                ia.WriteValue("InitialCatalog", InitialCatalog);
                ia.WriteValue("UserID", DESCrypto.Encrypt(UserID));
                ia.WriteValue("Password", DESCrypto.Encrypt(Password));
                ia.WriteValue("TrustedConnection", TrustedConnection.ToString());
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 从给定的INI文件中读取连接字符串。用户名密码会自动加密。
        /// </summary>
        /// <param name="iniPath"></param>
        /// <returns></returns>
        public bool LoadFromIni(string iniPath)
        {
            try
            {
                IniAccess ia = new IniAccess(iniPath);
                ia.Section = "System";
                DataSource = ia.ReadValue("DataSource");
                InitialCatalog = ia.ReadValue("InitialCatalog");
                UserID = ia.ReadValue("UserID");
                Password = ia.ReadValue("Password");
                UserID = DESCrypto.Decrypt(UserID);
                Password = DESCrypto.Decrypt(Password);

                string s = ia.ReadValue("TrustedConnection");
                if (s != null && s.Length > 0)
                {
                    trustedConnection = bool.Parse(s);
                }

            }
            catch { return false; }


            return true;
        }

        /// <summary>
        /// 转换成加密字符串。
        /// </summary>
        /// <param name="des">表示是否需要加密。</param>
        /// <returns></returns>
        public string ToDESString(bool des)
        {
            string s = string.Format("{0},{1},{2},{3},{4}", dataSource, initialCatalog, userID, password, trustedConnection);
            //des为真则加密。 
            return des ? DESCrypto.Encrypt(s) : s;
        }

        /// <summary>
        /// 对字符串进行解密。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="des">表示是否需要解密。</param>
        public static SQLConnection FromDesString(string s, bool des)
        {
            SQLConnection cn = new SQLConnection();
            try
            {
                s = des ? DESCrypto.Decrypt(s) : s;
                string[] data = s.Split(',');
                cn.dataSource = data[0];
                cn.initialCatalog = data[1];
                cn.userID = data[2];
                cn.password = data[3];
                cn.trustedConnection = bool.Parse(data[4]);
            }
            catch { cn = null; }


            return cn;
        }


        /// <summary>
        /// 测试当前连接是否可以连接成功。
        /// </summary>
        /// <returns></returns>
        public bool TestConnection()
        {
            try
            {
                SqlConnection sql = new SqlConnection(this.ConnectionString + "Connect Timeout=3;");
                Console.WriteLine(sql.ConnectionTimeout);
                sql.Open();
                sql.Close();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
