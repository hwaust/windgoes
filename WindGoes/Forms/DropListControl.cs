using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WindGoes.Forms
{
    /// <summary>
    /// 下拉控件，用于Textbox，能够对数据进行下拉选择。
    /// </summary>
    public class DropListControl
    {
        TextBox txtKey = null;
        ListBox lbData = new ListBox();
        Timer timer = new Timer();
        List<string> list = new List<string>();

 
        /// <summary>
        /// 输入内容后，下拉框弹出的延时，单位为ms，默认值为300
        /// </summary>
        public int Delay
        {
            get { return timer.Interval; }
            set { timer.Interval = value; }
        }


        /// <summary>
        /// 数据集合List
        /// </summary>
        public List<string> DataList
        {
            get { return list; }
            set { list = value; }
        }

        /// <summary>
        /// 下拉框的高度，默认为100
        /// </summary>
        public int Height
        {
            get { return lbData.Height; }
            set { lbData.Height = value; }
        }

        /// <summary>
        /// 下拉控件，主要用于Textbox，能够对数据进行下拉选择。
        /// </summary>
        /// <param name="tb">需要关联的TextBox</param>
        public DropListControl(TextBox tb)
        {
            txtKey = tb;
            lbData.Width = txtKey.Width;
            lbData.Location = new Point(txtKey.Left, txtKey.Top + txtKey.Height + 4);
            lbData.Visible = false;
            lbData.Height = 100;
            lbData.Font = txtKey.Font;
            txtKey.Parent.Controls.Add(lbData);

            timer.Interval = Delay;
            timer.Tick += new EventHandler(timer_Tick);

            txtKey.TextChanged += new EventHandler(textbox_TextChanged);
            txtKey.KeyDown += new KeyEventHandler(txtKey_KeyDown);
            txtKey.LostFocus += new EventHandler(txtKey_LostFocus);

            lbData.Click += new EventHandler(lbData_Click);
        }

        void lbData_Click(object sender, EventArgs e)
        {
            if (lbData.Items.Count > 0 && lbData.SelectedIndex >= 0)
            {
                txtKey.Text = lbData.SelectedItem.ToString();
                timer.Stop();
                lbData.Visible = false;
                txtKey.Select();
                txtKey.SelectionStart = txtKey.TextLength;
            }
        }

        void txtKey_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape: lbData.Visible = false; break;
                case Keys.Up:
                    if (lbData.Items.Count > 0 && lbData.SelectedIndex > 0)
                    {
                        lbData.SelectedIndex -= 1;
                    }
                    e.Handled = true;
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
                        txtKey.Text = lbData.SelectedItem.ToString();
                        timer.Stop();
                        lbData.Visible = false;
                        txtKey.SelectionStart = txtKey.TextLength;
                    }
                    break;
                default:
                    break;
            }
        }

        void textbox_TextChanged(object sender, EventArgs e)
        {
            if (txtKey.TextLength > 0)
            {
                if (lbData.Visible)
                {
                    Search();
                }
                else
                {
                    timer.Stop();
                    timer.Start();
                }

            }
            else
                lbData.Visible = false;
        }

        void txtKey_LostFocus(object sender, EventArgs e)
        {
            if (!lbData.Focused)
            {
                lbData.Visible = false;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            Search();
            lbData.Visible = lbData.Items.Count > 0;
            lbData.BringToFront();
        }


        private void Search()
        {
            //需要查找的关键字。
            string key = txtKey.Text.Trim().ToLower();

            //如果关键字为空则返回。
            if (string.IsNullOrEmpty(txtKey.Text))
            {
                lbData.Items.Clear();
                return;
            }

            //把查询字符串转换成拼音缩写。
            bool py = char.IsLower(key[0]);

            //根据关键字的类型进行查询，满足条件的添加至列表控件。
            lbData.Items.Clear();
            for (int i = 0; i < DataList.Count; i++)
            {
                string source = py ?
                    WindGoes.Data.ChineseSpell.GetChineseSpell(DataList[i]).ToLower() :
                    DataList[i];

                if (source.Contains(key))
                {
                    lbData.Items.Add(DataList[i]);
                }
            }

            //如果结果不为空，那么自动选中第1行数据。
            if (lbData.Items.Count > 0)
            {
                lbData.SelectedIndex = 0;
            }
        }
    }
}
