

/*
 * 名称：数据库操作类
 * 简介：主要用于数据的访问操作，封闭了一些常用的操作，可以实现对数据库底层的封装。
 * 作者：郝伟
 * 邮箱：hwaust@126.com
 *   QQ：117511560
 * 
 * v1.0
 * 2013-2-19 Wind
 * - 添加了GetColumnNames函数。
 * 2011-11-22	Wind -做了大量的修改，主要集中在返回数据为不null及2种数据库的整合上。
 * 2011-7-22	Wind	- 追加GetAllDataBase方法，获取服务器中所有数据库表。
 * 2008-10-27	Wind	- 初始版本，支持ACCESS和SQLServer数据库
 *
 * 
数据库中的所有数据的删除操作。
--再关闭所有外键约束  
exec sp_msforeachtable "alter table ? nocheck constraint all"  
--然后删除数据  
exec sp_msforEachTable "TRUNCATE TABLE?"  
--再启用所有外键约束  
exec sp_msforeachtable "alter table ? check constraint all"
 * 
*/


using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Common;

namespace WindGoes.Database
{
    /// <summary>
    /// 封装了数据常用的方法，大大减化了数据库的操作。
    /// </summary>
    public class DBManager
    {
        #region 字段
        static string connectionString = "";
        static DBType defaultDBType = DBType.SqlServer;

        string currentConnection = "";
        DBType currentDBType = DBType.SqlServer;

        string commandText = "";
        DbConnection cn;
        DbCommand cmd;
        DbDataReader reader;
        DbDataAdapter adapter;
        DataSet ds = new DataSet();
        #endregion

        #region 属性
        /// <summary>
        /// 总连接字符串。
        /// </summary>
        public static string ConnectionString
        {
            set { connectionString = value; }
            get { return connectionString; }
        }

        public static DBType DefaultDbType
        {
            get { return DBManager.defaultDBType; }
            set { DBManager.defaultDBType = value; }
        }

        /// <summary>
        /// 当前连接字符串。
        /// </summary>
        public string CurrentConnection
        {
            get { return currentConnection; }
            set { currentConnection = value; }
        }

        /// <summary>
        /// 是否显示出错对话框。
        /// </summary>
        public bool ShowErrorDialog { get; set; }

        /// <summary>
        /// 需要执行的SQL语句。
        /// </summary>
        public string CommandText
        {
            set { commandText = value; }
            get { return commandText; }
        }

        #endregion

        #region 构造函数

        void InitDataBase(DBType db, string con, string command)
        {
            currentDBType = db;
            currentConnection = con;
            switch (db)
            {
                case DBType.Access:
                    cn = new OleDbConnection(con);
                    adapter = new OleDbDataAdapter();
                    break;
                case DBType.SqlServer:
                    cn = new SqlConnection(con);
                    adapter = new SqlDataAdapter();
                    break;
                default:
                    break;
            }
            cmd = cn.CreateCommand();
            cmd.CommandText = command;
        }

        /// <summary>
        /// 用于处理数据库连接的
        /// </summary>
        public DBManager()
        {
            currentConnection = DBManager.ConnectionString;
        }

        /// <summary>
        /// 用于处理数据库连接的
        /// </summary>
        /// <param name="db">表示需要操作的数据库类型</param>
        public DBManager(DBType db)
        {
            InitDataBase(db, connectionString, "");
        }

        /// <summary>
        /// 用于处理数据库连接的
        /// </summary>
        /// <param name="db">数据库类型。</param>
        /// <param name="con">连接字符串。</param>
        public DBManager(DBType db, string con)
        {
            InitDataBase(db, con, "");
        }

        /// <summary>
        /// 用于处理数据库连接的
        /// </summary>
        /// <param name="sql">需要执行的SQL字符串。</param>
        public DBManager(string sql)
        {
            InitDataBase(defaultDBType, connectionString, "");
            commandText = sql;
        }

        /// <summary>
        /// 用于处理数据库连接的类
        /// </summary>
        /// <param name="sql">需要执行的SQL字符串。</param>
        /// <param name="db">数据库类型。</param>
        public DBManager(string sql, DBType db)
        {
            InitDataBase(db, connectionString, sql);
        }

        /// <summary>
        /// 用于处理数据库连接的类
        /// </summary>
        /// <param name="sql">需要执行的SQL字符串。</param>
        /// <param name="db">数据库类型。</param>
        /// <param name="con">连接字符串。</param>
        public DBManager(string sql, DBType db, string con)
        {
            InitDataBase(db, con, sql);
        }

        #endregion


        #region 数据库的打开的关闭操作
        /// <summary>
        /// 打开数据库连接.
        /// </summary>
        /// <returns>打开成功返回true,否则返回false</returns>
        public bool OpenCn()
        {
            if (cn == null)
            {
                InitDataBase(currentDBType, currentConnection, commandText);
            }
            if (cn.State == ConnectionState.Open)
                return true;

            try
            {
                cn.Open();
            }
            catch (Exception e1)
            {
                if (ShowErrorDialog)
                    MessageBox.Show("连接数据库失败，请检查连接后重新连接！错误信息：\n" + e1.Message, "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 关闭数据库连接.
        /// </summary>
        /// <returns>关闭成功返回true,否则返回false</returns>
        public bool CloseCn()
        {
            try
            {
                cn.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

        #region 对数据库的操作



        /// <summary>
        /// 一般Update和Insert可以用这个方法来实现.
        /// </summary>
        /// <returns>用bool型表示执行结果, true表示成功,否则失败</returns>
        public bool NonQuery()
        {
            if (!OpenCn())
                return false;
            try
            {
                cmd = cn.CreateCommand();
                cmd.CommandText = commandText;
                if (currentDBType == DBType.Access)
                {
                    int t = commandText.IndexOf("delete", StringComparison.OrdinalIgnoreCase);
                    if (t >= 0 && t <= 6)
                    {
                        commandText = "delete * from " + commandText.Substring(t + 6);
                    }
                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                if (ShowErrorDialog)
                    MessageBox.Show("操作失败，原因：" + exp.Message);
                CloseCn();
                return false;
            }

            return CloseCn();
        }

        /// <summary>
        /// 与NonQuery的区别在于，这个函数返回的是字符串，用于表示的操作的结果。
        /// </summary>
        /// <returns>用bool型表示执行结果, true表示成功,否则失败</returns>
        public string NonQueryString()
        {
            string str = "S";
            if (!OpenCn())
                return "Open Database Failed";
            try
            {
                if (currentDBType == DBType.Access)
                {
                    int t = commandText.IndexOf("delete", StringComparison.OrdinalIgnoreCase);
                    if (t >= 0 && t <= 6)
                    {
                        commandText = "delete from " + commandText.Substring(t + 6);
                    }
                }
                cmd.CommandText = commandText;
                cmd.ExecuteNonQuery();
                str = "操作成功。";
            }
            catch (Exception exp)
            {
                str = exp.Message;
            }
            if (!CloseCn())
                return "Close Database Failed";
            return str;
        }


        /// <summary>
        /// 快速执行SQL语句，去除了打开和关闭数据库的操作，同时去除了所有验证，从而大大提高了数据操作速度。
        /// 但是执行时务必自行保证验证各细节正确，否则会造成程序出错。
        /// </summary>
        public void FastExecuteNonQuery()
        {
            cmd.CommandText = commandText;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 一般Select语句用这个方法来实现, 返回结果为字符串二维数组.
        /// </summary>
        /// <returns>若读取失败，则返回空数据。</returns>
        public string[][] GetStrings()
        {
            List<string[]> data = new List<string[]>();
            if (!OpenCn())
                return data.ToArray();
            try
            {
                cmd.Connection = cn;
                cmd.CommandText = commandText;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string[] strs = new string[reader.FieldCount];
                    for (int i = 0; i < strs.Length; i++)
                        strs[i] = reader[i].ToString();
                    data.Add(strs);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                if (ShowErrorDialog)
                    MessageBox.Show("读取失败，错误原因：\n" + ex.Message);
                return data.ToArray();
            }
            if (!CloseCn())
                return data.ToArray();
            return data.ToArray();
        }


        /// <summary>
        /// Return all database names in a DB server.
        /// </summary>
        /// <returns></returns>
        public  string[] GetAllDatabaseNames()
        {
            if (!OpenCn())
                return new string[0];

            CommandText = "select name from master..sysdatabases";

            string[] data = GetColumn();

            List<string> list = new List<string>();
            List<string> excludedNames = new List<string> { "master", "tempdb", "model", "msdb" };
            for (int i = 0; i < data.Length; i++)
                if (!excludedNames.Contains(data[i].ToLower()))
                    list.Add(data[i]); 

            return list.ToArray();
        }

        /// <summary>
        /// 一般Select语句用这个方法来实现, 返回结果为object的二维数组.
        /// </summary>
        /// <returns>如果返回为null表示读取失败</returns>
        public object[][] GetObjects()
        {
            List<object[]> data = new List<object[]>();
            if (!OpenCn())
                return data.ToArray();
            try
            {
                cmd.Connection = cn;
                cmd.CommandText = commandText;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    object[] objs = new object[reader.FieldCount];
                    for (int i = 0; i < objs.Length; i++)
                        objs[i] = reader[i];
                    data.Add(objs);
                }
                reader.Close();
            }
            catch
            {
                return data.ToArray();
            }
            if (!CloseCn())
                return data.ToArray();
            return data.ToArray();
        }



        /// <summary>
        /// 只返回第一条记录的数据，如果没有数据，返回长度为0的数据（不为null)。
        /// </summary>
        /// <returns>string[]</returns>
        public string[] GetRow()
        {
            string[][] data = GetStrings();
            return data.Length > 0 ? data[0] : new string[0];
        }

        /// <summary>
        /// 返回第一列的数据，适合只有一列的操作。
        /// </summary>
        /// <returns></returns>
        public string[] GetColumn()
        {
            string[][] data = GetStrings();

            if (data.Length == 0)
                return new string[0];

            string[] str = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
                str[i] = data[i][0];
            return str;
        }

        /// <summary>
        /// 对于只要查询一个数据的查询可以用这个方法
        /// </summary>
        /// <returns>如果没有读取成功，返回为null，否则返回对象。</returns>
        public object GetObject()
        {
            object obj;
            if (!OpenCn())
                return null;
            try
            {
                cmd = cn.CreateCommand();
                cmd.CommandText = commandText;
                obj = cmd.ExecuteScalar();
            }
            catch
            {
                return null;
            }
            if (!CloseCn())
                return null;
            return obj;
        }

        /// <summary>
        /// 获得表中所有列的信息
        /// </summary>
        /// <param name="tableName">要得到信息的表名</param>
        /// <returns></returns>
        public FieldInfo[] GetFieldInfo(string tableName)
        {
            cmd.CommandText = "select * from " + tableName;
            if (!OpenCn())
            {
                return null;
            }
            reader = cmd.ExecuteReader();
            FieldInfo[] fis = new FieldInfo[reader.FieldCount];
            for (int i = 0; i < fis.Length; i++)
            {
                fis[i] = new FieldInfo(i, reader.GetName(i), reader.GetFieldType(i));
            }
            reader.Close();
            CloseCn();
            return fis;
        }

        /// <summary>
        /// 返回数据集。
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public DataSet GetDataSet(DBType db)
        {
            if (OpenCn())
            {
                switch (db)
                {
                    case DBType.Access:
                        adapter = new OleDbDataAdapter(commandText, connectionString);
                        break;
                    case DBType.SqlServer:
                        adapter = new SqlDataAdapter(commandText, connectionString);
                        break;
                    default:
                        break;
                }
                adapter.Fill(ds);
                return ds;
            }
            return null;
        }

        /// <summary>
        /// 返回数据集。
        /// </summary> 
        /// <returns></returns>
        public DataSet GetDataSet()
        {
            if (OpenCn())
            {
                switch (currentDBType)
                {
                    case DBType.Access:
                        adapter = new OleDbDataAdapter(commandText, connectionString);
                        break;
                    case DBType.SqlServer:
                        adapter = new SqlDataAdapter(commandText, connectionString);
                        break;
                    default:
                        break;
                }
                ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
            return null;
        }

        /// <summary>
        /// 生成插入字符串.
        /// </summary>
        /// <param name="cols">列名</param>
        /// <param name="values">每列对应的值</param>
        /// <returns>真表示成功,否则表示失败.</returns>
        public bool InsertSql(string cols, object[] values)
        {
            return false;
        }
        #endregion

        #region SQL Operation Strings
        public static string Select(string tableName)
        {
            return "select * from " + tableName;
        }

        public static string Select(string tableName, string sectors)
        {
            return "select " + sectors + " from " + tableName;
        }

        public static string Select(string tableName, string sectors, string conditions)
        {
            return "select " + sectors + " from " + tableName + " where " + conditions;
        }

        public static string Insert(string tableName, string content)
        {
            return "insert into " + tableName + " values(" + content + ")";
        }

        public static string InsertAll(params object[] data)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("insert into " + data[0] + " values(");

            for (int i = 0; i < data.Length - 1; i++)
            {
                sb.Append(string.Format("'{0}',", data[i + 1].ToString()));
            }
            sb[sb.Length - 1] = ')';
            return sb.ToString();
        }

        /// <summary>
        /// 例句： DBManager.Update("Table", "PhoneNumber,Address,UserType;`0|`1&China", 11, 22,33, "Mobile", "139554636", 20)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Update(params object[] data)
        {
            try
            {
                //生成前面的UPDATE主句：UPDATE Table SET 
                StringBuilder sb = new StringBuilder();
                sb.Append("UPDATE " + data[0] + " SET ");
                string[] alldata = data[1].ToString().Split(';');
                string[] cols1 = alldata[0].Split(',');

                //UPDATE要更新的内容部分：PhoneNumber = '11',Address = '22',UserType = '33' 
                for (int i = 0; i < cols1.Length; i++)
                    sb.Append(string.Format("{0} = '{1}',", cols1[i], data[i + 2]));
                sb[sb.Length - 1] = ' ';

                //如果没有条件直接返回当前句
                if (alldata.Length == 1) return sb.ToString();

                //加入WHERE，然后根据‘|’和‘&’把条件部分断开，同时把条件符放在sb1里
                sb.Append("WHERE");
                string[] cols2 = alldata[1].Split('|', '&'); if (data.Length < 2 + cols1.Length + cols2.Length) return "参数数量不够";
                StringBuilder sb1 = new StringBuilder();
                for (int i = 0; i < alldata[1].Length; i++)
                    if (alldata[1][i] == '|' || alldata[1][i] == '&')
                        sb1.Append(alldata[1][i]);

                //将分段条件和条件符连接在一起
                for (int i = 0; i < cols2.Length - 1; i++)
                    sb.Append(string.Format(" {0} = '{1}' {2}", cols2[i], data[i + cols1.Length + 2].ToString(), sb1[i]));
                sb.Append(string.Format(" {0} = '{1}'", cols2[cols2.Length - 1], data[cols2.Length + cols1.Length + 1].ToString()));

                //把条件符更换成原关键字，然后把省略部分，即‘`0’这样的内容，更换为原文
                sb.Replace("&", "AND");
                sb.Replace("|", "OR");
                for (int i = sb.Length - 1; i >= 0; i--)
                    if (sb[i] == '`') sb.Replace(sb.ToString().Substring(i, 2), cols1[int.Parse(sb[i + 1].ToString())]);

                //返回结果
                return sb.ToString();
            }
            catch { }
            return "1";
        }

        public static string Update1(params object[] data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("update " + data[0] + " set ");
            string[] cols = data[1].ToString().Split(',');

            for (int i = 0; i < cols.Length; i++)
            {
                sb.Append(string.Format("{0} = '{1}',", cols[i], data[i + 2]));
            }
            sb[sb.Length - 1] = ' ';

            if (data.Length > cols.Length + 2)
            {
                sb.Append("where " + data[data.Length - 1].ToString());
            }
            return sb.ToString();
        }

        public static string Update2(params object[] data)
        {
            //DBManager.Update(Tables[0], "PhoneNumber,Address,UserType;`0|`1&China", data[0], data[1], data[2], "Mobile", "139554636", 20)
            StringBuilder sb = new StringBuilder();
            sb.Append("update " + data[0] + " set ");
            string[] alldata = data[1].ToString().Split(';');
            string[] cols1 = alldata[0].Split(',');

            for (int i = 0; i < cols1.Length; i++)
            {
                sb.Append(string.Format("{0} = '{1}',", cols1[i], data[i + 2]));
            }
            sb[sb.Length - 1] = ' ';

            sb.Append(" where ");
            int t = 0;
            int t1 = 0;
            while (t < alldata[1].Length)
            {
                switch (alldata[1][t])
                {
                    case '`':
                        t++;
                        sb.Append(cols1[int.Parse(alldata[1].Substring(t, 1))] + " = '" + data[t1 + cols1.Length + 2] + "'");
                        t1++;
                        break;
                    case '|':
                        sb.Append(" or ");
                        break;
                    case '&':
                        sb.Append(" and ");
                        break;
                    default:
                        if (char.IsLetter(alldata[1][t]))
                        {
                            if (t >= alldata[1].Length - 1 || !char.IsLetter(alldata[1][t + 1]))
                            {
                                for (int i = t; i >= 0; i--)
                                {
                                    if (!char.IsLetter(alldata[1][i]))
                                    {
                                        sb.Append(alldata[1].Substring(i + 1, t - i) + " = '" + data[t1 + cols1.Length + 2] + "'");
                                        t1++;
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                }
                t++;
            }


            return sb.ToString();
        }

        public static string Insert(params object[] data)
        {
            //insert1(表名,字段(格式:"a,b,c"),值a1,值b1,值c1);
            string cols = data[0].ToString();
            string[] cols1 = data[1].ToString().Split(',');
            string[] cols2 = new string[data.Length - 2];
            for (int i = 2, j = 0; i < data.Length; i++, j++)
            {
                cols2[j] = data[i].ToString();
            }
            if (cols1.Length != cols2.Length)
            {
                return "字段数量和值数量不对应,请检查输入";
            }
            else
            {
                string Field = string.Join(",", cols1);
                string Value = string.Join("','", cols2);
                string rt = "INSERT INTO " + cols + "(" + Field + ") VALUES('" + Value + "')";
                //Console.WriteLine(cols);
                return rt;
            }
        }

        public static string Insert1(params object[] data)
        {
            //insert(表名,字段(格式:"a,b,c"),值(对应格式:"a1,b1,c1"));
            string cols = data[0].ToString();
            string[] cols1 = data[1].ToString().Split(',');
            string[] cols2 = data[2].ToString().Split(',');
            if (cols1.Length != cols2.Length)
            {
                return "字段数量和值数量不对应,请检查输入";
            }
            else
            {
                string Field = string.Join(",", cols1);
                string Value = string.Join("','", cols2);
                string rt = "INSERT INTO " + cols + "(" + Field + ") VALUES('" + Value + "')";
                return rt;
            }
        }

        //解析UPDATE语句
        object[] GetParams(string str)
        {
            try
            {
                int start = 0, end = 0;
                bool begin = false;
                List<object> objects = new List<object>();

                for (int i = 0; i < str.Length;)
                    if (str[i++] == '(')
                    {
                        start = i;
                        break;
                    }

                for (int i = 0; i < str.Length;)
                    if (str[i++] == ')')
                    {
                        end = i;
                        break;
                    }

                StringBuilder sb = new StringBuilder();
                for (int i = start; i < end; i++)
                {
                    switch (str[i])
                    {

                        case ' ':
                            if (begin)
                                sb.Append(str[i]);
                            break;
                        case ',':
                            if (!begin)
                            {
                                if (sb.Length > 0)
                                    objects.Add(sb.ToString());
                                sb = new StringBuilder();
                            }
                            else
                                sb.Append(str[i]);
                            break;
                        case '"':
                            begin = !begin;
                            break;
                        case ')':
                            objects.Add(sb.ToString());
                            break;
                        default:
                            if (!begin && !(char.IsLetter(str[i - 1]) || char.IsDigit(str[i - 1])))
                                sb = new StringBuilder();
                            sb.Append(str[i]);
                            break;
                    }
                }
                return objects.ToArray();
            }
            catch (Exception)
            {

                throw;
            }
        }

        void Test(string sql)//(object sender, KeyEventArgs e)
        {
            try
            {
                //string str = (String)GetType().InvokeMember("Update", BindingFlags.Default | BindingFlags.InvokeMethod, null, this, GetParams(sql));
            }
            catch (Exception e1) { Console.WriteLine(e1.Message); }

        }


        #endregion

        /// <summary>
        /// 返回服务器中所有数据库的名称，只支持Sql数据库。
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllDataBaseNames()
        {
            List<string> list = new List<string>();
            if (currentDBType == DBType.Access)
            {
                return list;
            }
            CommandText = "select name from master..sysdatabases";
            string[] data = GetColumn();

            for (int i = 0; i < data.Length; i++)
            {
                string name = data[i].ToLower();
                if (name != "master" && name != "tempdb" && name != "model" && name != "msdb")
                    list.Add(data[i]);
            }

            return list;
        }

        /// <summary>
        /// 返回当前数据库中所有的表名。
        /// </summary>
        /// <returns></returns>
        public string[] GetAllTableNames()
        {
            List<string> tableNames = new List<string>();
            if (currentDBType == DBType.SqlServer)
            {
                //"SELECT name FROM SYSOBJECTS WHERE XTYPE = 'U' 
                if (OpenCn())
                {
                    DataTable dt = cn.GetSchema("Tables");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Rows[i][j] == null || dt.Rows[i][j].ToString().Length == 0)
                            {
                                Console.Write("null" + "\t");
                            }
                            else
                            {
                                Console.Write(dt.Rows[i][j] + "\t");
                            }
                        }
                        Console.WriteLine();
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][3].ToString().ToLower() == "base table")
                        {
                            tableNames.Add(dt.Rows[i][2].ToString());
                        }
                    }
                    CloseCn();
                }
            }
            else
            {
                if (OpenCn())
                {
                    DataTable dt = cn.GetSchema("Tables");
                    //dt = cn.GetSchema("Columns"); 
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    for (int j = 0; j < dt.Columns.Count; j++)
                    //    {
                    //        if (dt.Rows[i][j] == null || dt.Rows[i][j].ToString().Length == 0)
                    //        {
                    //            Console.Write("null" + "\t");
                    //        }
                    //        else
                    //        {
                    //            Console.Write(dt.Rows[i][j] + "\t");
                    //        }
                    //    }
                    //    Console.WriteLine();
                    //}

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][3].ToString().ToLower() == "table")
                        {
                            tableNames.Add(dt.Rows[i][2].ToString());
                        }
                    }
                    CloseCn();
                }
            }

            //commandText =  currentDBType == DBType.SqlServer ?
            //    "SELECT name FROM SYSOBJECTS WHERE XTYPE = 'U' " :
            //     " select name from MSysObjects where type=1 and  flags=0 ";
            return tableNames.ToArray();
        }

        /// <summary>
        /// 返回指定表的所有列的列名。
        /// </summary>
        /// <param name="tableName">指定表的表名。</param>
        /// <returns></returns>
        public string[] GetColumnNames(string tableName)
        {
            string[] names = null;
            try
            {
                DBManager dm = new DBManager();
                dm.CommandText = "select top 1 * from " + tableName;
                DataTable dt = dm.GetDataSet().Tables[0];
                names = new string[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    names[i] = dt.Columns[i].ColumnName;
                }
            }
            catch { }
            return names;
        }

        public List<string> GetColumnInfo(string tableName)
        {
            List<string> list = new List<string>();

            if (currentDBType == DBType.SqlServer)
            {

            }
            else
            {
                if (OpenCn())
                {
                    DataTable dt = cn.GetSchema("Columns");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][2].ToString().ToLower() == tableName.ToLower())
                            list.Add(dt.Rows[i][3].ToString() + dt.Rows[i][27]);
                    }
                    CloseCn();
                }
            }

            return list;
        }
    }
}

//public static string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:Database Password= ql_54T30c4_342dmou ;User ID=Admin;Data Source= data\\account.hdt;Persist Security Info=False;";

