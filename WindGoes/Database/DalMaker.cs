using System; 
using System.Text;
using System.Data;
using System.IO;
using System.Collections.Generic;

//此文件为DalMaker的基本属性、构造函数和一些基本方法。
namespace WindGoes.Database
{
	/// <summary>
	/// 用于将数据库中的表导出成DAL的CS文件。
	/// </summary>
	public partial class DalMaker
	{
		#region 变量和属性。
		string tableName = "";
		string[] lowerNames;	//即字段的名称，加前下划线
		/// <summary>
		/// 字段的名称，由属性名加下划线组成。
		/// </summary>
		public string[] LowerNames
		{
			get { return lowerNames; }
			set { lowerNames = value; }
		}
		string[] dataTypes;		//字段的数据类型。
		/// <summary>
		/// 每个字段的数据类型。
		/// </summary>
		public string[] DataTypes
		{
			get { return dataTypes; }
			set { dataTypes = value; }
		}
		string[] upperNames;	//封闭后的属性名。
		/// <summary>
		/// 封装后的属性的名称。
		/// </summary>
		public string[] UpperNames
		{
			get { return upperNames; }
			set { upperNames = value; }
		}
		string[] addons;			//备注名称。
		/// <summary>
		/// 从数据库中读取到的各个字段的备注内容。
		/// </summary>
		public string[] Addons
		{
			get { return addons; }
			set { addons = value; }
		}


		DBManager dm = new DBManager();
		StringBuilder sbContent = null;
		int depth = 0;

		/// <summary>
		/// 默认的命名空间的名称。
		/// </summary>
		public static string NameSpace { get; set; }

		#endregion 

		/// <summary>
		/// CS文件生成类。
		/// </summary>
		/// <param name="table">需要生成的CS文件的表名。</param>
		public DalMaker(string table)
		{
			tableName = table;
			InitProperty();
		}
		static DalMaker()
		{
			NameSpace = "Unknown";
		}


		/// <summary>
		/// 将指定字符串保存至文件。
		/// </summary>
		/// <param name="content">需要保存的内容。</param>
		/// <param name="filePath">指定的文件。</param>
		public void SaveToFile(string content, string filePath)
		{
			StreamWriter sw = new StreamWriter(filePath, false, Encoding.Default);
			sw.Write(content);
			sw.Close();
		}


		#region 基本方法。
		void Add(string content)
		{
			sbContent.Append(content);
		}
		void AddLine()
		{
			sbContent.Append("\r\n");
		} 
		void AddLine(string content)
		{
			sbContent.Append(new string('\t', depth) + content + "\r\n");
		}
		void AddLine(string content, int t)
		{
			if (t > 0)
			{
				sbContent.Append(new string('\t', depth) + content + "\r\n");
				depth += t;
			}
			else
			{
				depth += t;
				sbContent.Append(new string('\t', depth) + content + "\r\n");
			} 
		}
		void BeginBrace()
		{
			AddLine("{", 1);
		}
		void EndBrace()
		{
			AddLine("}", -1);
		}
		#endregion


	}
}
 