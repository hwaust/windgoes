using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TestCenter
{
	public partial class Form1 : BaseForm
	{
		public Person Admin { get; set; }

		public Form1()
		{
			InitializeComponent();
		
			saver.AttachProperty(this, "Text");
			AddProperty(this, "Location");
			AddProperty(this, "Size");
			AddProperty(listView1, null);
		}
		public override void InitialData()
		{
			base.InitialData();
			Admin = new Person();
		}

		private void button1_Click(object sender, EventArgs e)
		{
            new WindGoes.Forms.ConTestForm().ShowDialog();
		
		}

		void test()
		{
		}

		private void button2_Click(object sender, EventArgs e)
		{

		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Admin.Name = Admin.Name + Admin.Name.Length.ToString();
			textBox1.Text = Admin.Name;
		}


	}
}
