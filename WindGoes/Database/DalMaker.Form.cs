using System;
using System.Collections.Generic;
using System.Text;

namespace WindGoes.Database
{
	//此函数用于生成窗体的代码。
	public partial class DalMaker
	{
		/// <summary>
		/// 将窗体代码保存在指定文件中。
		/// </summary>
		/// <param name="filepath">指定的文件路径。</param>
		public void SaveFormToFile(string filepath)
		{
			SaveToFile(GetFormString(), filepath);
		}

		private string GetFormString()
		{
			if (sbContent == null)
				return string.Empty;
			sbContent = new StringBuilder();

			FromHeader();
			FormProperty();
			FormAttachUI();
			FormConstructor();
			FromSaveButtonClick();
			FormLoad();
			FormEnd();
			return sbContent.ToString();
		}


		private void FromHeader()
		{

			AddLine("using System;");
			AddLine("using System.Collections.Generic;");
			AddLine("using System.ComponentModel;");
			AddLine("using System.Data;");
			AddLine("using System.Drawing;");
			AddLine("using System.Text;");
			AddLine("using System.Windows.Forms;");
			AddLine();

			AddLine("namespace " + NameSpace + ".Forms");
			BeginBrace();

			AddLine("public partial class " + tableName + "Form : Form");
			BeginBrace();

			AddLine();
		}

		private void FormProperty()
		{
			AddLine(tableName + " _" + tableName + " = new " + tableName + "();");
			AddLine("/// <summary>");
			AddLine("/// ");
			AddLine("/// </summary>");
			AddLine("public " + " " + tableName + " " + tableName);
			BeginBrace();
			AddLine("get { return _" + tableName + "; }");
			AddLine("set { _" + tableName + " = value; }");
			EndBrace();

			AddLine();
		}

		private void FormAttachUI()
		{
			AddLine("public void AttachUI() ");
			BeginBrace();

			AddLine("if (" + tableName + "." + upperNames[0] + " > 0) ");
			BeginBrace();
			for (int i = 1; i < upperNames.Length; i++)
			{
				if (dataTypes[i].IndexOf("String") >= 0) 
					AddLine("txt" + upperNames[i] + ".Text = " +tableName+ "." + upperNames[i] + ";");
				else
					AddLine("txt" + upperNames[i] + ".Text = " + tableName + "." + upperNames[i] + ".ToString();");
			}
			EndBrace();

			EndBrace();

			AddLine();
		}

		private void FromSaveButtonClick()
		{
			AddLine("private void btnSave_Click(object sender, EventArgs e)");
			BeginBrace();
			for (int i = 1; i < upperNames.Length; i++)
			{
				if (dataTypes[i].IndexOf("String") >= 0) 
					AddLine(tableName + "." + upperNames[i] + " = txt" + upperNames[i] + ".Text;");
				else
					AddLine(tableName + "." + upperNames[i] + " = " + dataTypes[i] + ".Parse(txt" + upperNames[i] + ".Text);");
			}

			AddLine("bool b = btnSave.Text == \"保存\" ? " + tableName + ".Update() : " + tableName + ".Insert();");
			AddLine("string s = b ? btnSave.Text + \"成功\" : btnSave.Text + \"失败\";");
			AddLine("MessageBox.Show(s);");

			AddLine("btnSave.Text = btnSave.Text == \"新建\" ? \"保存\" : btnSave.Text;");
			AddLine("Close();");
			EndBrace();
			AddLine();
		}

		private void FormConstructor()
		{
			AddLine("public " + tableName + "Form()");
			BeginBrace();
			AddLine("InitializeComponent();");
			EndBrace();

			AddLine();
		}

		private void FormLoad()
		{
			AddLine("private void " + tableName + "Form_Load(object sender, EventArgs e)");
			BeginBrace();

			AddLine("if (" + tableName +"." + upperNames[0] + " > 0)");
			BeginBrace();
			AddLine("btnSave.Text = \"保存\";");
			AddLine("AttachUI();");
			EndBrace();
			AddLine("else");
			BeginBrace();
			AddLine("btnSave.Text = \"新建\";");
			EndBrace();
			EndBrace();

			AddLine();
		}

		private void FormEnd()
		{
			EndBrace();
			EndBrace();
		}
	}
}
