/*
 * 名称：对象属性自动读写类。
 * 
 * 作用：类启动前添加此类，然后调用载入和保存方法即可自动保存属性。
 *
 * 
 * 时间：
 * 2011-6-3   初步建立这个类。 
 * 2011-7-2   添加了段，全名，是否要记录几个属性。
 *                  添加直接读取或写入到指定路径的2个方法。 
 * 2013-5-8	主要变化有两点：
 *					1、添加了对更多的基本数据的支持，
 *					2、通过IPropertyManager接口实现对外部类的支持。
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using WindGoes.IO;
using System.Drawing;

namespace WindGoes.Data
{
	/// <summary>
	/// 用于类的属性的自动保存或读取。依赖于IniAccess类，用于数据读写。
	/// </summary>
	public class PropertyIOManager
	{
		#region 基本属性

		/// <summary>
		/// 目标控件。
		/// </summary>
		public object Target { get; set; }

		/// <summary>
		/// 对应属性的名称。
		/// </summary>
		public string PropertyName { get; set; }

		/// <summary>
		/// 对应属性的名称。
		/// </summary>
		public string TargetName { get; set; }

		/// <summary>
		/// INI段中的全名，在读写时，作为Ini的属性名，如 control1_Width = 25。
		/// </summary>
		public string FullName
		{
			get
			{
				if (string.IsNullOrEmpty(PropertyName))
					return TargetName + "_Default";
				
				return TargetName + "_" + PropertyName;
			}
		}

		/// <summary>
		/// 是否加密。
		/// </summary>
		public bool NeedCrypto { get; set; }


		/// <summary>
		/// 在Ini文件中所对应的字段。
		/// </summary>
		public string Section { get; set; }

		/// <summary>
		/// 是否需要加载。
		/// </summary>
		public bool NeedLoad { get; set; }

		/// <summary>
		/// 是否需要记录。
		/// </summary>
		public bool NeedSave { get; set; }


		#endregion

		#region 构造函数
		/// <summary>
		/// 初始化数据库，包括对象、对象名和属性名，其中字段为对象的类型名称。
		/// </summary>
		/// <param name="target">对象，Object类型。</param>
		/// <param name="targetName">对象的名称，如control1。</param>
		/// <param name="propertyName">属性的名称。</param>
		private void Init(object target, string targetName, string propertyName)
		{
			NeedCrypto = false;
			NeedSave = true;
			NeedLoad = true;
			Target = target;
			TargetName = targetName;
			PropertyName = propertyName;
			Section = target.GetType().Name;
		}

		/// <summary>
		/// 属性管理类，目前支持类型包括: string, int, float, double, DateTime, bool。
		/// </summary>
		/// <param name="t">待保存的目标。</param>
		/// <param name="n">目标的名称。</param>
		/// <param name="p">待保存的目标的属性的名称。</param>
		public PropertyIOManager(object t, string n, string p)
		{
			Init(t, n, p);
		}

		/// <summary>
		/// 属性管理类，目前支持类型包括: string, int, float, double, DateTime, bool。
		/// </summary>
		/// <param name="c">待保存的控件。</param>
		/// <param name="p">待保存的属性名。</param>
		public PropertyIOManager(Control c, string p)
		{
			Init(c, c.Name, p);
		}
		#endregion

		#region 最核心的数据读写方法

		/// <summary>
		/// 读取对应控件的属性的值。
		/// </summary>
		/// <returns></returns>
		public string GetValue()
		{
			string s = string.Empty;

			if (string.IsNullOrEmpty(PropertyName))
			{
				Type t1 = Target.GetType();
				switch (t1.Name)
				{
					case "ListView":
						ListView lv = Target as ListView;
						if (lv != null && lv.Columns != null)
						{
							s = "";
							for (int i = 0; i < lv.Columns.Count; i++)
							{
								s += lv.Columns[i].Width + ",";
							}
						}
						break;
					default:
						break;
				}
				return s;
			}

			try
			{
				PropertyInfo pi = Target.GetType().GetProperty(PropertyName);
				Type type = pi.PropertyType;

				object po = pi.GetValue(Target, null);
				if (po != null && po is IPropertyManager)
				{
					return (po as IPropertyManager).ToPString();
				}

				switch (type.Name)
				{
					case "Point":
						Point p = (Point)pi.GetValue(Target, null);
						s = p.X + ":" + p.Y;
						break;
					case "PointF":
						PointF pf = (PointF)pi.GetValue(Target, null);
						s = pf.X + ":" + pf.Y;
						break;

					case "Size":
						Size sz = (Size)pi.GetValue(Target, null);
						s = sz.Width + ":" + sz.Height;
						break;

					case "SizeF":
						SizeF szf = (SizeF)pi.GetValue(Target, null);
						s = szf.Width + ":" + szf.Height;
						break;

					default:
						s = pi.GetValue(Target, null).ToString();
						break;
				}
			}
			catch { }
			return s;
		}

		/// <summary>
		/// 将值写入至控件的对应属性。
		/// </summary>
		/// <param name="v">默认是字符串型。</param>
		public void SetValue(object v)
		{
			if (v == null || v.ToString().Length == 0)
				return;

			object val = null;
			string[] data = null;
			if (string.IsNullOrEmpty(PropertyName))
			{
				Type t1 = Target.GetType();
				switch (t1.Name)
				{
					case "ListView":
						ListView lv = Target as ListView;
						if (lv != null && lv.Columns != null)
						{
							string[] s = v.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

							for (int i = 0; i < lv.Columns.Count; i++)
							{
								lv.Columns[i].Width = int.Parse(s[i]);
							}
						}
						break;
					default:
						break;
				}
				return;
			}
			try
			{
				//属性对象。
				PropertyInfo pi = Target.GetType().GetProperty(PropertyName);

				//属性的类型。
				Type ptype = pi.PropertyType;

				//接口类型。
				Type itype = ptype.GetInterface("IPropertyManager");

				//判断接口可存在，如果存在，则直接使用接口，否则手工解析。
				if (itype != null)
				{
					object pv = pi.GetValue(Target, null);
					if (pv != null)
					{
						(pv as IPropertyManager).FromPString(v.ToString());
						return;
					}
					//else
					//{
					//    MethodInfo mi = itype.GetMethod("FromPString");
					//    val = mi.Invoke(Activator.CreateInstance(ptype), new object[] { v.ToString() });
					//}
				}
				else
				{
					switch (ptype.Name)
					{
						case "String": val = v; break;
						case "Int32": val = int.Parse(v.ToString()); break;
						case "Single": val = float.Parse(v.ToString()); break;
						case "Double": val = double.Parse(v.ToString()); break;
						case "DateTime": val = DateTime.Parse(v.ToString()); break;
						case "Boolean": val = Boolean.Parse(v.ToString()); break;
						case "Point":
							data = v.ToString().Split(':');
							val = new Point(int.Parse(data[0]), int.Parse(data[1]));
							break;
						case "PointF":
							data = v.ToString().Split(':');
							val = new PointF(float.Parse(data[0]), float.Parse(data[1]));
							break;
						case "Size":
							data = v.ToString().Split(':');
							val = new Size(int.Parse(data[0]), int.Parse(data[1]));
							break;
						case "SizeF":
							data = v.ToString().Split(':');
							val = new SizeF(float.Parse(data[0]), float.Parse(data[1]));
							break;

						default: break;
					}
				}
				pi.SetValue(Target, val, null);
			}
			catch { }
		}

		#endregion

		#region 数据读写方法


		/// <summary>
		/// 保存至Ini文件。
		/// </summary>
		/// <param name="ia"></param>
		public void SaveToIni(IniAccess ia)
		{
			if (NeedSave)
			{
				string s = GetValue();

				if (NeedCrypto)
				{
					s = DESCrypto.Encrypt(s);
				}

				ia.WriteValue(FullName, s);
			}
		}

		/// <summary>
		/// 保存至Ini文件。
		/// </summary>
		/// <param name="path"></param>
		public void SaveToIni(string path)
		{
			IniAccess ia = new IniAccess(path);
			ia.Section = Section;
			SaveToIni(ia);
		}


		/// <summary>
		/// 从ini文件中读取。
		/// </summary>
		/// <param name="ia"></param>
		public void LoadFromIni(IniAccess ia)
		{
			string v = ia.ReadValue(FullName);
			if (NeedCrypto && v != null && v.Length > 0)
			{
				v = DESCrypto.Decrypt(v);
			}
			SetValue(v);
		}

		/// <summary>
		/// 从ini文件中读取。
		/// </summary>
		/// <param name="path"></param>
		public void LoadFromIni(string path)
		{
			IniAccess ia = new IniAccess(path);
			ia.Section = Section;
			LoadFromIni(ia);
		}

		#endregion
	}
}
