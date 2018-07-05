using System;
using System.Collections.Generic;
using System.Text;

namespace WindGoes.Database
{
    /// <summary>
    /// 数据库表中的字段信息，包括字段排列号，类型和名称
    /// </summary>
    public struct FieldInfo
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int index;
        /// <summary>
        /// 字段名。
        /// </summary>
        public string name;
        /// <summary>
        /// 数据类型。
        /// </summary>
        public Type type;

        public FieldInfo(int i, string n, Type t)
        {
            index = i;
            name = n;
            type = t;
        }
    }
}
