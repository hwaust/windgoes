/*
 * 名称： 用于对指定数据库所有表结构的Excel和TxT格式输出。
 * 
 * 作用： 主要包括：输出到Excel（ExportTablesToExcel）和输出到TxT(ExportTablesToTxt)两个函数，
 *        同过给定输出路径和实例化该类时提供的连接字符串输出指定数据库表结构。
 *        public void ExportTablesToExcel(string path)
 *        public void ExportTablesToTxt(string path)
 *        
 *作者：叶飞
 *
 * 时间：2011年7月20日 初步建立这个类。
 *       2011年7月23日 重大修改
 *       2011年7月27日 减小耦合，加入指定路径输出。
 *       
 * 更新：
 *
 * 
 */

using System;
using System.Collections.Generic;
using WindGoes.Data;
using System.Data;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.IO;

namespace DBStructureOutPut
{
    class DBStructOutput
    {
        DBManager dm = new DBManager();
        string selectstr = "select  * from information_schema.columns where table_name<>'dtproperties'";
        string connectionString = null;
        /// <summary>
        /// 类所使用的连接字符串。
        /// </summary>
        public string ConnectionString
        {
            get { return connectionString; }
            set
            {
                connectionString = value;
                dm.CurrentConnection = connectionString;
            }
        }

        /// <summary>
		/// 构造累，获取连接字符串
		/// </summary>
		/// <param name="con">数据库的连接字符串。</param>
		public DBStructOutput(string con)
		{
			ConnectionString = con;
		}
        /// <summary>
        /// 导出表的结构至Excel文件。
        /// </summary>
        /// <param name="path">Excel文件的路径。</param>
        public void ExportTablesToExcel(string path)
        {
            List<string> list = GetTables();
            dm.CommandText = selectstr;
            DataSet myds = dm.GetDataSet();

            try
            {
                List<string> TableName = new List<string>();
                List<int> TableNo = new List<int>();
                TableName.Add(myds.Tables[0].Rows[0][2].ToString());
                int tableno = -1;
                for (int i = 0; i < myds.Tables[0].Rows.Count; i++)
                {
                    tableno++;
                    if ((i != 0) && (myds.Tables[0].Rows[i][2].ToString() != myds.Tables[0].Rows[i - 1][2].ToString()))
                    {
                        TableName.Add(myds.Tables[0].Rows[i][2].ToString());
                        TableNo.Add(tableno);
                        tableno = 0;
                    }
                }
                TableNo.Add(tableno + 1);

                Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application();
                Workbook wBook = Excel.Workbooks.Add(true);
                Worksheet wSheet;
                Excel.Visible = false;
                int rowcount = 0;
                for (int i = 0; i < TableName.Count; i++)
                {
                    if (i == 0)
                    {
                        wSheet = wBook.Worksheets[1] as Worksheet;
                    }
                    else
                    {
                        wSheet = wBook.Worksheets.Add() as Worksheet;
                    }
                    wSheet.Name = TableName[i];

                    for (int j = 3; j < myds.Tables[0].Columns.Count; j++)
                    {
                        wSheet.Cells[1, j - 2] = myds.Tables[0].Columns[j].ColumnName;
                    }
                    #region 中文命名
                    wSheet.Cells[2, 1] = "列名称";
                    wSheet.Cells[2, 2] = "列标识号";
                    wSheet.Cells[2, 3] = "列默认值";
                    wSheet.Cells[2, 4] = "是否接受空值";
                    wSheet.Cells[2, 5] = "数据类型";
                    wSheet.Cells[2, 6] = "部分数据最大长度（字符数）";
                    wSheet.Cells[2, 7] = "部分数据最大长度（字节数）";
                    wSheet.Cells[2, 8] = "部分数据的精度";
                    wSheet.Cells[2, 9] = "部分数据的精度基数";
                    wSheet.Cells[2, 10] = "部分数据的小数位数";
                    wSheet.Cells[2, 11] = "datetime和SQL-92interval数据类型的子类型代码";
                    wSheet.Cells[2, 12] = "字符数据或文本数据类型，则返回 master";
                    wSheet.Cells[2, 13] = "（始终返回 NULL）";
                    wSheet.Cells[2, 14] = "返回回字符集的唯一名称";
                    wSheet.Cells[2, 15] = "列为字符数据或文本数据类型返回 master";
                    wSheet.Cells[2, 16] = "（始终返回 NULL）";
                    wSheet.Cells[2, 17] = "参数分页的名称";
                    wSheet.Cells[2, 18] = "域日志";
                    wSheet.Cells[2, 19] = "域结构";
                    wSheet.Cells[2, 20] = "域名称";
                    #endregion
                    for (int j = 0; j < TableNo[i]; j++)
                    {
                        for (int k = 3; k < myds.Tables[0].Columns.Count; k++)
                        {
                            wSheet.Cells[j + 3, k - 2] = myds.Tables[0].Rows[rowcount][k];
                        }
                        rowcount++;
                    }
                }
                //设置禁止弹出保存和覆盖的询问提示框   
                Excel.DisplayAlerts = false;
                Excel.AlertBeforeOverwriting = false;
                //保存工作簿   
                wBook.Save();
                //保存excel文件   
                Excel.Save(path);
                Excel.SaveWorkspace(path);
                Excel.Quit();
                Excel = null;
                killgc();
                MessageBox.Show("已将文件导入： " + path + " 路径下", "提示信息",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                MessageBox.Show("导出Excel出错！错误原因：" + err.Message, "提示信息",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                killgc();
                return;

            }
        }
        /// <summary>
        /// 导出表结构到TxT文件
        /// </summary>
        /// <param name="path">TxT文件的保存路径</param>
        public void ExportTablesToTxt(string path)
        {
            List<string> list = GetTables();
            dm.CommandText = selectstr;
            DataSet myds = dm.GetDataSet();


            try
            {
                StreamWriter sr = new StreamWriter(path);



                int ii = 0;
                for (int i = 0; i < myds.Tables[0].Rows.Count; i++, ii++)
                {

                    if ((i != 0) && (myds.Tables[0].Rows[i][2].ToString() != myds.Tables[0].Rows[i - 1][2].ToString()))
                    {
                        ii = 0;
                        sr.WriteLine();
                        sr.WriteLine(myds.Tables[0].Rows[i][2]);
                    }
                    if (i == 0)
                    {

                        sr.WriteLine(myds.Tables[0].Rows[0][2]);

                    }
                    sr.WriteLine("----------------------------第{0}列----------------------------", ii + 1);
                    for (int j = 0; j < myds.Tables[0].Columns.Count - 3; j++)
                    {
                        sr.Write(myds.Tables[0].Columns[j + 3].ColumnName + ":");
                        sr.WriteLine(myds.Tables[0].Rows[i][j + 3]);

                    }
                }
                sr.Close();
                killgc();
                MessageBox.Show("已将文件导入： " + path + " 路径下", "提示信息",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                MessageBox.Show("导出Excel出错！错误原因：" + err.Message, "提示信息",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                killgc();
                return;

            }
            
        }
        /// <summary>
        /// 获取指定数据库中的所有表
        /// </summary>
        /// <returns></returns>
        public List<string> GetTables()
        {
            List<string> result = new List<string>();
            dm.CommandText = "select name from sysobjects where xtype='U'and name<>'dtproperties' order by name";
            DataSet myds = dm.GetDataSet();
            try
            {
                for (int i = 0; i < myds.Tables[0].Rows.Count; i++)
                {


                    result.Add(myds.Tables[0].Rows[i][0].ToString());
                }

                return result;
            }
            catch (Exception)
            {
                MessageBox.Show("连接字符串有错或数据库无表！");
                result.Clear();
                return result;
                
            }
            
            
        }
        /// <summary>
        /// 清除废弃的两个进程
        /// </summary>
        private void killgc()
        {
            Process[] processes;

            processes = System.Diagnostics.Process.GetProcesses();

            foreach (Process p in processes)
            {

                if (p.ProcessName == "EXCEL"||p.ProcessName =="IntelliTrace")
                {
                    try
                    {
                        p.Kill();
                        //MessageBox.Show("杀死" + p.ProcessName + "成功");
                        // break;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("关闭Excel.EXE进程时出现异常，进程可能未关闭，请注意手动关闭！");
                    }
                }
            }

        }
    }
}
