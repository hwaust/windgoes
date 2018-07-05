using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindGoes.Data
{
	/// <summary>
	/// 窗体数据保存类，用于保存窗体的数据。
	/// </summary>
	public class FormDataSaver
	{
		Form form = null;
		List<PropertyIOManager> pim = new List<PropertyIOManager>();

		/// <summary>
		/// 用于记录的Ini文件，默认位置为 config.ini
		/// </summary>
		public string IniPath { get; set; }

		public FormDataSaver(Form f)
		{
			form = f;
			f.Load += new EventHandler(f_Load);
			f.FormClosing += new FormClosingEventHandler(f_FormClosing);
			IniPath = "config.ini";
		}

		void f_Load(object sender, EventArgs e)
		{
			
			for (int i = 0; i < pim.Count; i++)
			{
				pim[i].LoadFromIni(IniPath);
			}
		}
		void f_FormClosing(object sender, FormClosingEventArgs e)
		{
			for (int i = 0; i < pim.Count; i++)
			{
				pim[i].SaveToIni(IniPath);
			}
		}


		public void AttachProperty(Control c, string propertyName)
		{
			PropertyIOManager pm = new PropertyIOManager(c, propertyName);
			pim.Add(pm);
		}
	}
}
