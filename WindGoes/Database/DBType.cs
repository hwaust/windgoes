using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindGoes.Database
{
	/// <summary>
	/// 表示数据库的类型，目前支持ACCESS和SQLSERVER
	/// </summary>
	public enum DBType
	{
		/// <summary>
		/// Access类。
		/// </summary>
		Access,
		/// <summary>
		/// Sql Server类
		/// </summary>
		SqlServer
	}

}
