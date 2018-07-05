using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing; 
using System.Text;
using System.Windows.Forms;

namespace WindGoes.Forms
{
    public partial class MultiDataSearchForm : Form
    {
        string[,] dataArray;
        /// <summary>
        /// 外部数据源
        /// </summary>
        public string[,] DataArray
        {
            get { return dataArray; }
            set { dataArray = value; }
        }
        string[] resultArray = new string[4];
        /// <summary>
        /// 返回所得到的结果
        /// </summary>
        public string[] ResultArray
        {
            get { return resultArray; }
            set { resultArray = value; }
        }

        List<string> itemList = new List<string>();
        List<List<string>> itemsList = new List<List<string>>();
        /// <summary>
        /// 需要被查询和匹配的数据源
        /// </summary>
        /// <param name="data">数据资源，即列表视图中的一项</param>
        public void AddItems(string[] data)
        {
            itemList.Clear();
            for (int i = 0; i < data.Length; i++)
            {
                itemList.Add(data[i]);
            }
            itemsList.Add(itemList);
        }
        /// <summary>
        /// 初始化列表视图的列标题
        /// </summary>
        /// <param name="names">列标题字符串数组</param>
        public void AddTableNames(string[] names)
        {
            ColumnHeader colHead;
            if(names==null)
            {
                MessageBox.Show("str为进行初始化！");
            }
            if (names.Length == 0)
            {
                MessageBox.Show("请设置正确的列标题格式！");
            }
            //string[] columnStr = colHeadStr.Split(',');
            for (int i = 0; i < names.Length; i++)
            {
                colHead = new ColumnHeader();
                colHead.Text = names[i];
                lvwResult.Columns.Add(colHead);
            }

        }

        //对传入的数据源按str进行搜索，并对listview进行绘制
        private void Search(string str)
        {
            lvwResult.Items.Clear();
            ListViewItem lt;
            ListViewItem.ListViewSubItem lvsi;
            bool flag = false;
            string cstr = WindGoes.Data.ChineseSpell.GetChineseSpell(str);
            string ckey="";
            for (int i = 0; i < itemsList.Count; i++)
            {
                foreach(string strVar in itemsList[i])
                {
                    if (char.IsLetter(str[0]))
                    {
                        ckey = WindGoes.Data.ChineseSpell.GetChineseSpell(strVar);
                        if (ckey.Contains(cstr))
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        if (strVar.Contains(str))
                        {
                            flag = true;
                        }
                    }      
                }
                if (flag == true)
                {
                    lt = new ListViewItem();
                    int count = 0;
                    foreach (string item in itemsList[i])
                    {
                        count++;
                        if (count<2)
                        { 
                            lt.Text =item;
                        }
                        else
                        {
                            
                            lvsi = new ListViewItem.ListViewSubItem();
                            lvsi.Text = item;
                            lt.SubItems.Add(lvsi);
                            
                        }
                    }
                    lvwResult.Items.Add(lt);
                    flag = false;
                }
            }
        }

        //将要查询的字符串给中间的空格给去掉
        private string DeleteVoid(string strData)
        {
            string[] keyTemp = strData.Split(' ');
            strData = "";
            for (int i = 0; i < keyTemp.Length; i++)
            {
                strData += keyTemp[i];
            }
            return strData;
        }

        public MultiDataSearchForm()
        {
            InitializeComponent();
            lvwResult.Columns.Clear();
            //AddTableNames();
        }

        private void txtKey_TextChanged(object sender, EventArgs e)
        {
            
            string key = DeleteVoid(txtKey.Text.Trim());
            if (key == "")
            {
                lvwResult.Items.Clear();
                lblNum.Text = "0条";
                return;
            }
            Search(key);
            if (lvwResult.Items.Count > 0)
            {
                lvwResult.SelectedIndices.Add(0);
                //lvwResult.Select();
            }
            lblNum.Text = lvwResult.Items.Count.ToString("0条");
        }

        private void txtKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        private void lvwResult_DoubleClick(object sender, EventArgs e)
        {
            resultArray[0] = lvwResult.SelectedItems[0].Text;
            for (int i = 1; i < 4; i++)
            {
                resultArray[i] = lvwResult.SelectedItems[0].SubItems[i].Text;
            }
            Close();
        }

        private void DataSearchForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
                case Keys.Enter:
                    if(lvwResult.Items.Count > 0 && lvwResult.SelectedIndices[0]>= 0)
                    {
                        resultArray[0] = lvwResult.SelectedItems[0].Text;
                        for (int i = 1; i < 4; i++)
                        {
                            resultArray[i] = lvwResult.SelectedItems[0].SubItems[i].Text;
                        }
                        Close();
                    }
                    break;
                case Keys.Up:
                    if (lvwResult.Items.Count > 0 && lvwResult.SelectedIndices[0] > 0)
                    {
                        int temp = lvwResult.SelectedIndices[0];
                        temp -= 1;
                        lvwResult.SelectedIndices.Clear();
                        lvwResult.SelectedIndices.Add(temp);
                    }
                    break;
                case Keys.Down:
                    if (lvwResult.Items.Count > 0&&lvwResult.SelectedIndices[0]<lvwResult.Items.Count-1)
                    {
                        int temp = lvwResult.SelectedIndices[0];
                        temp += 1;
                        lvwResult.SelectedIndices.Clear();
                        lvwResult.SelectedIndices.Add(temp);
                    }
                    break;
                default:
                    break;
            } 
        }
    }
}
