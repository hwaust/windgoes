using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace WindGoes.Database
{
	//用于创建对象的类。
	public partial class DalMaker
	{
		/// <summary>
		/// 返回指定表所对应的CS文件的全部文本。
		/// </summary>
		/// <returns></returns>
		public string GetClassString()
		{
			if (sbContent == null) return string.Empty;
			sbContent = new StringBuilder();
			AddHead();
			AddDefinitions();
			AddProperties();
			AddLoadByID();
			AddLoadFromStrings();
			AddLoadFromSql();
			AddInsert();
			AddInsertString();
			AddUpdate();
			AddUpdateString();
			AddDelete();
			AddDeleteString();
			AddStaticDelete();
			AddGetLastID();
			AddCheckZero();
			AddToStrings();
			AddEnd();

			AddOthers();
			return sbContent.ToString();
		}

		/// <summary>
		/// 将转换的Cs文件直接全部保存至文件。
		/// </summary>
		/// <param name="filePath"></param>
		public void SaveCSToFile(string filePath)
		{
			SaveToFile(GetClassString(), filePath); 
		} 

		#region 添加类的各个方法。
		private bool InitProperty()
		{
			dm.CommandText = "select * from " + tableName;
			DataSet ds = dm.GetDataSet();
			try
			{
				DataTable dt = ds.Tables[0];
				lowerNames = new string[dt.Columns.Count];
				upperNames = new string[dt.Columns.Count];
				dataTypes = new string[dt.Columns.Count];
				addons = new string[dt.Columns.Count];
				List<string> list = dm.GetColumnInfo(tableName);

				for (int i = 0; i < dt.Columns.Count; i++)
				{
					upperNames[i] = dt.Columns[i].Caption;
					lowerNames[i] = "_" + dt.Columns[i].Caption;
					dataTypes[i] = dt.Columns[i].DataType.Name;

					string name = upperNames[i];

					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].Length > name.Length && list[j].Substring(0, name.Length) == name)
						{
							addons[i] = list[j].Substring(name.Length);
							break;
						}
					}
				}

			}
			catch { return false; }
			sbContent = new StringBuilder();
			return true;
		}

		private void AddHead()
		{
			AddLine("using System;");
			AddLine("using System.Collections.Generic;");
			AddLine("using System.Text;");
			AddLine("using System.Data;");
			AddLine();
			AddLine("namespace " + NameSpace);
			BeginBrace();
			AddLine("public partial class " + tableName);
			BeginBrace();
		}

		private void AddDefinitions()
		{
			AddLine("DBManager dm = new DBManager();");

			for (int i = 0; i < lowerNames.Length; i++)
			{
				AddLine(string.Format("private {0} {1};", dataTypes[i], lowerNames[i]));
			}
			AddLine();
		}

		private void AddProperties()
		{
			for (int i = 0; i < lowerNames.Length; i++)
			{
				AddLine("/// <summary>");
				AddLine(i == 0 ? "/// " + addons[i] + "\t 注：此属性在数据库中，必需为Int型的主键，同时最好为自增长字段，如果不是，请自行修改代码。" : "/// " + addons[i]);
				AddLine("/// </summary>");
				AddLine(string.Format("public {0} {1}", dataTypes[i], upperNames[i]));
				BeginBrace();
				AddLine("get { return  " + lowerNames[i] + "; }");
				AddLine("set { " + lowerNames[i] + " = value; }");
				EndBrace();
				AddLine();
			}
		}

		private void AddLoadByID()
		{
			AddLine("/// <summary>");
			AddLine("/// 根据当前ID从数据库中读取数据。");
			AddLine("/// </summary>");
			AddLine("/// <param name=\"id\">需要删除的记录的主键。</param>");
			AddLine("public bool LoadByID(int id)");
			BeginBrace();
			AddLine(upperNames[0] + " = id;");
			AddLine("if (IDEqualZero()) return false;");
			#region 生成更新字符串
			StringBuilder sb1 = new StringBuilder();
			sb1.Append("string.Format(\"select * from " + tableName +
				" where " + upperNames[0] + " = " + "{" + "0" + "}\", " + upperNames[0] + ");");

			#endregion

			AddLine("dm.CommandText = " + sb1.ToString());
			AddLine("string[] data = dm.GetRow();");
			AddLine("return LoadFromStrings(data);");
			EndBrace();
			AddLine();
		}

		private void AddLoadFromStrings()
		{
			AddLine("/// <summary>");
			AddLine("/// 从字符串数组中初始化数据， 此数据一定是从数据库中读取的完全数据。");
			AddLine("/// </summary>");
			AddLine("/// <param name=\"data\">从数据库中读取的完全数据。</param>");
			AddLine("public bool LoadFromStrings(string[] data)");
			BeginBrace();
			AddLine("if(data == null || data.Length == 0 || data.Length != " + upperNames.Length + ") return false;");
			for (int i = 0; i < upperNames.Length; i++)
			{
				if (dataTypes[i].IndexOf("String") >= 0)
				{
					AddLine(upperNames[i] + " = data[" + i.ToString() + "];");
				}
				else
				{
					AddLine(upperNames[i] + " = " + dataTypes[i] + ".Parse(data[" + i.ToString() + "]);");
				}
			}
			AddLine("return true;");
			EndBrace();
			AddLine();
		}

		private void AddLoadFromSql()
		{
			AddLine("/// <summary>");
			AddLine("/// 从字符串数组中初始化数据， 此数据一定是从数据库中读取的完全数据。");
			AddLine("/// </summary>");
			AddLine("/// <param name=\"sql\">从数据库中读取的完全数据。</param>");
			AddLine("public static List<" + tableName + "> LoadFromSql(string sql)");
			BeginBrace();
			AddLine("List<" + tableName + "> list = new List<" + tableName + ">();");
			AddLine("DBManager dms = new DBManager();");
			AddLine("dms.CommandText = sql;");
			AddLine("string[][] data = dms.GetStrings();");
			AddLine("for(int i = 0; i < data.Length; i++)");
			BeginBrace();
			AddLine(tableName + " obj = new " + tableName + "();");
			AddLine("if(obj.LoadFromStrings(data[i]))");
			AddLine("\tlist.Add(obj);");
			EndBrace();
			AddLine("return list;");
			EndBrace();
			AddLine();
		}

		private void AddInsert()
		{
			AddLine("/// <summary>");
			AddLine("/// 将当前对象的数据插入到数据库中，同时自动生成新ID保存到ID字段中。");
			AddLine("/// </summary>");
			AddLine("public bool Insert()");
			BeginBrace();
			AddLine("dm.CommandText = GetInsertString();");
			AddLine("if(dm.NonQuery())");
			BeginBrace();
			AddLine(upperNames[0] + " = GetLastID();");
			AddLine("return true;");
			EndBrace();
			AddLine("return false;");
			EndBrace();
			AddLine();
		}

		private void AddInsertString()
		{
			AddLine("/// <summary>");
			AddLine("/// 返回用于Update的SQL字符串。");
			AddLine("/// </summary>");
			AddLine("public string GetInsertString()");
			BeginBrace();
			#region 生成插入字符串
			StringBuilder sb1 = new StringBuilder();
			sb1.Append("string.Format(\"insert into " + tableName + "(");
			for (int i = 1; i < upperNames.Length; i++)
				sb1.Append(upperNames[i] + ", ");
			sb1[sb1.Length - 2] = ' ';

			sb1.Append(") Values(");

			for (int i = 1; i < upperNames.Length; i++)
			{
				if (dataTypes[i].Contains("Date"))
				{
					sb1.Append("#{" + i + "}#, ");
				}
				else if (dataTypes[i].Contains("String"))
				{
					sb1.Append("'{" + i + "}', ");
				}
				else
				{
					sb1.Append("{" + i + "}, ");
				}
			}
			sb1[sb1.Length - 2] = ')';

			sb1.Append("\"");

			for (int i = 0; i < upperNames.Length; i++)
				sb1.Append(", " + upperNames[i]);
			sb1.Append(");");

			#endregion
			AddLine("return " + sb1.ToString());
			EndBrace();
			AddLine();
		}

		private void AddUpdate()
		{
			AddLine("/// <summary>");
			AddLine("/// 根据当前的数据内容，向ID更新数据记录。");
			AddLine("/// </summary>");
			AddLine("public bool Update()");
			BeginBrace();
			AddLine("if (IDEqualZero()) return false;");
			AddLine("dm.CommandText = GetUpdateString();");
			AddLine("if(dm.NonQuery())");
			AddLine("\treturn true;");
			AddLine("return false;");
			EndBrace();
			AddLine();
		}

		private void AddUpdateString()
		{
			AddLine("/// <summary>");
			AddLine("/// 返回用于Update的SQL字符串。");
			AddLine("/// </summary>");
			AddLine("public string GetUpdateString()");
			BeginBrace();
			#region 生成更新字符串
			StringBuilder sb1 = new StringBuilder();
			sb1.Append("string.Format(\"update " + tableName + " Set ");
			for (int i = 1; i < upperNames.Length; i++)
			{
				if (dataTypes[i].Contains("Date"))
				{
					sb1.Append(upperNames[i] + " = " + "#{" + i.ToString() + "}#, ");
				}
				else if (dataTypes[i].Contains("String"))
				{
					sb1.Append(upperNames[i] + " = " + "'{" + i.ToString() + "}', ");
				}
				else
				{
					sb1.Append(upperNames[i] + " = " + "{" + i.ToString() + "}, ");
				}
			}

			sb1[sb1.Length - 2] = ' ';

			sb1.Append("Where " + upperNames[0] + " = " + "{" + "0" + "}");


			sb1.Append("\"");

			for (int i = 0; i < upperNames.Length; i++)
				sb1.Append(", " + upperNames[i]);
			sb1.Append(");");

			#endregion
			AddLine("return " + sb1.ToString());
			EndBrace();
			AddLine();
		}

		private void AddDelete()
		{
			AddLine("/// <summary>");
			AddLine("/// 根据当前ID会自动在数据库中删除此对象，但是在删除后ID不清零。");
			AddLine("/// </summary>");
			AddLine("public bool Delete()");
			BeginBrace();
			AddLine("if (IDEqualZero()) return false;");
			AddLine("dm.CommandText = GetDeleteString();");
			AddLine("if(dm.NonQuery())");
			AddLine("\treturn true;");
			AddLine("return false;");
			EndBrace();
			AddLine();
		}

		private void AddDeleteString()
		{
			AddLine("/// <summary>");
			AddLine("/// 返回用于Update的SQL字符串。");
			AddLine("/// </summary>");
			AddLine("public string GetDeleteString()");
			BeginBrace();
			#region 生成更新字符串
			StringBuilder sb1 = new StringBuilder();
			sb1.Append("string.Format(\"delete from " + tableName +
				" where " + upperNames[0] + " = " + "{" + "0" + "}\"," + upperNames[0] + ");");

			#endregion
			AddLine("return " + sb1.ToString());
			EndBrace();
			AddLine();
		}

		private void AddStaticDelete()
		{
			AddLine("/// <summary>");
			AddLine("/// 根据当前ID会自动在数据库中删除此对象。");
			AddLine("/// </summary>");
			AddLine("/// <param name=\"id\">需要删除的记录的主键。</param>");
			AddLine("public static bool Delete(int id)");
			BeginBrace();
			AddLine("if (id == 0) return false;");
			#region 生成更新字符串
			StringBuilder sb1 = new StringBuilder();
			sb1.Append("string.Format(\"delete from " + tableName +
				" where " + upperNames[0] + " = " + "{" + "0" + "}\", id);");
			#endregion
			AddLine("DBManager dms = new DBManager();");
			AddLine("dms.CommandText = " + sb1.ToString());
			AddLine("return dms.NonQuery();");
			EndBrace();
			AddLine();
		}

		private void AddGetLastID()
		{
			//"select 编号 from admin order by 编号 desc";
			AddLine("/// <summary>");
			AddLine("/// 获得数据库中ID的当前最大值。");
			AddLine("/// </summary>");
			AddLine("public int GetLastID()");
			BeginBrace();
			#region 生成更新字符串
			StringBuilder sb1 = new StringBuilder();
			sb1.Append("\"select " + upperNames[0] + " from " + tableName + " order by " + upperNames[0] + " desc \";");
			#endregion
			AddLine("dm.CommandText = " + sb1.ToString());
			AddLine("object obj = dm.GetObject();");
			AddLine("return obj == null ? 0 : int.Parse(obj.ToString());");
			EndBrace();
			AddLine();
		}

		private void AddToStrings()
		{
			AddLine("/// <summary>");
			AddLine("/// 返回当前对象中所有数据转换成的一维数组。");
			AddLine("/// </summary>");
			AddLine("public string[] ToStrings()");
			BeginBrace();
			//return new string[]{ 0, 1, 2, 3};
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < upperNames.Length; i++)
			{
				if (dataTypes[i].Contains("String"))
				{
					builder.Append(upperNames[i] + ", ");
				}
				else
				{
					builder.Append(upperNames[i] + ".ToString(), ");
				}
			}
			builder.Remove(builder.Length - 2, 2);
			AddLine("return new string[]{" + builder.ToString() + "};");
			EndBrace();
			AddLine();
		}

		private void AddCheckZero()
		{
			AddLine("/// <summary>");
			AddLine("/// 判断当前ID是否为0.");
			AddLine("/// </summary>");
			AddLine("public bool IDEqualZero()");
			BeginBrace();
			AddLine("return " + upperNames[0] + " == 0;");
			EndBrace();
			AddLine();
		}

		private void AddOthers()
		{
			AddLine("/*");
			AddLine();
			for (int i = 0; i < upperNames.Length; i++)
			{
				AddLine("txt" + upperNames[i] + ".Text = name." + upperNames[i] + ";");
			}
			AddLine();
			for (int i = 0; i < upperNames.Length; i++)
			{
				AddLine("name." + upperNames[i] + " = txt" + upperNames[i] + ".Text;");
			}
			AddLine("*/");
			AddLine();
		}

		void AddEnd()
		{
			EndBrace();
			EndBrace();
		}
		#endregion
	}
}
