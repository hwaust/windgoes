using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WindGoes
{
    /// <summary>
    /// 一些对控件操作所用到的方法。
    /// </summary>
    public class ControlHelper
    {
        /// <summary>
        /// 根据字符串数组，初始化一个ListView。
        /// </summary>
        /// <param name="lv">需要初始化的ListView</param>
        /// <param name="heads">字符串数组。</param>
        public static void InitListView(ListView lv, string[] hs)
        {
            if (lv == null)
            {
                lv = new ListView();
                lv.Width = 100;
                lv.Height = 100;
            }

            lv.GridLines = true;
            lv.MultiSelect = false;
            lv.FullRowSelect = true;

            lv.View = View.Details;

            lv.Columns.Clear();

			for (int i = 0; i < hs.Length; i++)
            {
				lv.Columns.Add(hs[i]);
            }
        }

        /// <summary>
        /// 等宽调整ListView
        /// </summary>
        /// <param name="lv">需要等宽调整的ListView</param>
        public static void AdjustListViewWidth(ListView lv)
        {
            int wid = (lv.Width - 50) / lv.Columns.Count;
            for (int i = 0; i < lv.Columns.Count; i++)
            {
                lv.Columns[i].Width = wid;
            }
        } 
    }
}
