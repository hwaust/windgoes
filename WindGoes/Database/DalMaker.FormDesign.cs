using System;
using System.Collections.Generic;
using System.Text;

namespace WindGoes.Database
{
	//此函数用于生成设计窗体的代码。
	public partial class DalMaker
	{
		/// <summary>
		/// 返回设计窗体代码。
		/// </summary>
		/// <returns></returns>
		public string GetDesignFormString()
		{
			if (sbContent == null) return string.Empty;
			sbContent = new StringBuilder();

			DFormHeader();
			DFormInitializeComponent();
			DFormVarientDefine();

			return sbContent.ToString();
		}

		public void SaveDFormToFile(string filepath)
		{
			SaveToFile(GetDesignFormString(), filepath);
		}

		private void DFormHeader()
		{
			AddLine("namespace " + NameSpace + ".Forms");
			BeginBrace();
			AddLine("partial class " + tableName + "Form");
			BeginBrace();
			
			AddLine("/// <summary>");
			AddLine("/// Required designer variable.");
			AddLine("/// </summary>"); 
			AddLine("private System.ComponentModel.IContainer components = null;");

			AddLine("/// <summary>");
			AddLine("/// Clean up any resources being used.");
			AddLine("/// </summary>");
			AddLine("/// <param name=\"disposing\">true if managed resources should be disposed; otherwise, false.</param>");
			AddLine("protected override void Dispose(bool disposing)");
			BeginBrace();
			AddLine("if (disposing && (components != null))");
			BeginBrace();
			AddLine("components.Dispose();");
			EndBrace();
			AddLine("base.Dispose(disposing);");
			EndBrace(); 
			AddLine();

		}

		private void DFormInitializeComponent()
		{
			AddLine("#region Windows Form Designer generated code");
			AddLine();

			AddLine("/// <summary>");
			AddLine("/// Required method for Designer support - do not modify");
			AddLine("/// the contents of this method with the code editor.");
			AddLine("/// </summary>");
			AddLine("private void InitializeComponent()");
			BeginBrace();
			for (int i = 1; i < upperNames.Length; i++)
			{
				AddLine("this.lbl" + upperNames[i] + " = new System.Windows.Forms.Label();");
				AddLine("this.txt" + upperNames[i] + "  = new System.Windows.Forms.TextBox();");
			}
			AddLine("this.btnSave = new System.Windows.Forms.Button();");
			AddLine("this.SuspendLayout();");

			int ti = 0;
			for (int i = 1; i < upperNames.Length; i++)
			{
				AddLine("//");
				AddLine("// lbl" + upperNames[i]);
				AddLine("//");
				AddLine("this.lbl" + upperNames[i] + ".AutoSize = true;");
				AddLine("this.lbl" + upperNames[i] + ".Location =  new System.Drawing.Point(10, " + (i * 26 - 16) +");");
				AddLine("this.lbl" + upperNames[i] + ".Name = \"lbl" + upperNames[i] + "\";");
				AddLine("this.lbl" + upperNames[i] + ".Size = new System.Drawing.Size(60, 12);");
				AddLine("this.lbl" + upperNames[i] + ".TabIndex = " + ti + 100+ ";");
				AddLine("this.lbl" + upperNames[i] + ".Text = \"" + upperNames[i] + "\";");

				AddLine("//");
				AddLine("// txt" + upperNames[i]);
				AddLine("//");
				AddLine("this.txt" + upperNames[i] + ".Location =  new System.Drawing.Point(80, " + (i * 26 - 18) + ");");
				AddLine("this.txt" + upperNames[i] + ".Name = \"txt" + upperNames[i] + "\";");
				AddLine("this.txt" + upperNames[i] + ".Size = new System.Drawing.Size(400, 12);");
				AddLine("this.txt" + upperNames[i] + ".TabIndex = " + ti + ";");
				AddLine("this.txt" + upperNames[i] + ".Text = \"\";");

				ti++;
			}

			AddLine("//");
			AddLine("// btnSave");
			AddLine("//");
			AddLine("this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));");
			AddLine("this.btnSave.Location = new System.Drawing.Point(420, 280);");
			AddLine("this.btnSave.Name = \"btnSave\";");
			AddLine("this.btnSave.Size = new System.Drawing.Size(68, 28);");
			AddLine("this.btnSave.TabIndex = 20;");
			AddLine("this.btnSave.Text = \"保存\";");
			AddLine("this.btnSave.UseVisualStyleBackColor = true;");
			AddLine("this.btnSave.Click += new System.EventHandler(this.btnSave_Click);");

			AddLine("//");
			AddLine("//" + tableName + "Form");
			AddLine("//");
			AddLine("this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);");
			AddLine("this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;");
			AddLine("this.ClientSize = new System.Drawing.Size(500, 320);");
			for (int i = 1; i < upperNames.Length; i++)
			{
				AddLine("this.Controls.Add(this.lbl" + upperNames[i] + ");");
				AddLine("this.Controls.Add(this.txt" + upperNames[i] + ");"); 
			}
			AddLine("this.Controls.Add(this.btnSave);");
			AddLine("this.Name = \"" + tableName + "Form\";");
			AddLine("this.Text = \"" + tableName + "\";");
			AddLine("this.Load += new System.EventHandler(this." + tableName + "Form_Load);");
			AddLine("this.ResumeLayout(false);");
			AddLine("this.PerformLayout();");
			EndBrace();
			AddLine();

			AddLine("#endregion");
			AddLine();
		}

		private void DFormVarientDefine()
		{
			for (int i = 1; i < upperNames.Length; i++)
			{
				AddLine("private System.Windows.Forms.Label lbl" + upperNames[i] + ";");
				AddLine("private System.Windows.Forms.TextBox txt" + upperNames[i] + ";");
			 }
			AddLine("private System.Windows.Forms.Button btnSave;");
			EndBrace();

			EndBrace();
		}
	}
}
