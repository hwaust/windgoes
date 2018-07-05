using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindGoes.Data;
using WindGoes.IO;

namespace TestCenter
{
	public class BaseForm:Form
	{
		protected List<PropertyIOManager> pmlist = new List<PropertyIOManager>();

		protected WindGoes.Data.FormDataSaver saver = null;
		public BaseForm()
		{
			InitializeComponent();
			saver = new WindGoes.Data.FormDataSaver(this);
			InitialData();
		}

		public virtual void InitialData()
		{

		}

		public virtual void OnFormLoad()
		{
			IniAccess ia = new IniAccess(Program.AppCfgPath);
			ia.Section = this.Name;
			for (int i = 0; i < pmlist.Count; i++)
			{
				pmlist[i].LoadFromIni(ia);
			}
		}

		public virtual void OnFormClosing()
		{
			IniAccess ia = new IniAccess(Program.AppCfgPath);
			ia.Section = this.Name;
			for (int i = 0; i < pmlist.Count; i++)
			{
				pmlist[i].SaveToIni(ia);
			}
		}

		public void AddProperty(Control c, string p)
		{
			pmlist.Add(new PropertyIOManager(c, p));
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// BaseForm
			// 
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Name = "BaseForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BaseForm_FormClosing);
			this.Load += new System.EventHandler(this.BaseForm_Load);
			this.ResumeLayout(false);

		}

		private void BaseForm_Load(object sender, EventArgs e)
		{ 
			OnFormLoad();
		}

		private void BaseForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			OnFormClosing();
		}


	}
}
