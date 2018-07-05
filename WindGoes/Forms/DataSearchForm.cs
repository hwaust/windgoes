using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing; 
using System.Text;
using System.Windows.Forms;

namespace WindGoes.Forms
{
    /// <summary>
    /// 数据搜索窗体。
    /// </summary>
    public partial class DataSearchForm : BaseForm
    {
        bool showAllWhenBlank = true;
        /// <summary>
        /// 当显示为空时，是否显示所有可选项。
        /// </summary>
        public bool ShowAllWhenBlank
        {
            get { return showAllWhenBlank; }
            set { showAllWhenBlank = value; }
        }


		List<string> list = new List<string>();
        /// <summary>
        /// 数据集合List
        /// </summary>
        public List<string> DataList
        {
            get { return list; }
            set { list = value; }
        } 

        List<string> resultList = new List<string>();
        /// <summary>
        /// 返回的数据集结果
        /// </summary>
        public List<string> ResultList
        {
            get { return resultList; }
            set { resultList = value; }
        }

		/// <summary>
		/// 查询的结果。
		/// </summary>
		public string Result
		{
			get
			{
                return lbData.SelectedItem != null ? lbData.SelectedItem.ToString() : string.Empty;
			}
		}

        /// <summary>
        /// 数据搜索窗体。
        /// </summary>
        public DataSearchForm()
        {
            InitializeComponent();
        }

		private void Search()
		{
            try
            {
                //需要查找的关键字。
                string key = txtKey.Text.Trim().ToLower();

                //如果关键字为空则返回。
                if (string.IsNullOrEmpty(txtKey.Text))
                {
                    lbData.Items.Clear();
                    if (showAllWhenBlank)
                    {
                        foreach (string s in list)
                        {
                            lbData.Items.Add(s);
                        }
                    }
                    return;
                }

                //把查询字符串转换成拼音缩写。
                bool py = char.IsLower(key[0]);

                //根据关键字的类型进行查询，满足条件的添加至列表控件。
                lbData.Items.Clear();

                //用于计数，如果太长则只显示前100条。
                int count = 0;
                for (int i = 0; i < DataList.Count; i++)
                {
                    string source = py ?
                        WindGoes.Data.ChineseSpell.GetChineseSpell(DataList[i]).ToLower() :
                        DataList[i];

                    if (source.Contains(key))
                    {
                        lbData.Items.Add(DataList[i]);
                        count++;
                        if (count > 100)
                        {
                            break;
                        }
                    }
                }

                //如果结果不为空，那么自动选中第1行数据。
                if (lbData.Items.Count > 0)
                {
                    lbData.SelectedIndex = 0;
                }

                lbCount.Text = lbData.Items.Count.ToString("0条");
            }
            catch { }
		} 

		private void txtSearchByContent_TextChanged(object sender, EventArgs e)
		{
 			Search();
		}

		private void listBox1_DoubleClick(object sender, EventArgs e)
		{
			ResultList.Clear();
            if (lbData.SelectedIndex >= 0)
    			ResultList.Add(lbData.Items[lbData.SelectedIndex].ToString()); 
			this.Close();
		} 

		private void Form2_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Escape:
					Close();
					break;

				case Keys.Up:
					if (lbData.Items.Count > 0 && lbData.SelectedIndex > 0)
					{
						lbData.SelectedIndex -= 1;
					}

					break;

				case Keys.Down:
					if (lbData.Items.Count > 0 && lbData.SelectedIndex < lbData.Items.Count - 1)
					{
						lbData.SelectedIndex += 1;
					}
					break;

				case Keys.Enter:
					if (lbData.Items.Count > 0 && lbData.SelectedIndex >= 0)
					{
						resultList.Add(lbData.SelectedItem.ToString());
						Close();
					}
					break;
				default:
					break;
			} 
		}

		private void txtKey_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
			{
				e.Handled = true;
			}
		}

        private void DataSearchForm_Load(object sender, EventArgs e)
        {
            if (showAllWhenBlank)
            {
                foreach (string s in list)
                {
                    lbData.Items.Add(s);
                }
            }
            lbCount.Text = lbData.Items.Count.ToString("0条");
        }
	}
}
