using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCenter
{
	class Data
	{
	}

	public class MyClass : IClone
	{
		public string name { get; set; }
		public MyClass() { name = "Jack"; }

		public object Clone()
		{
			return "hello";
		}

		public string GetName()
		{
			return name;
		}
	}

	public class Mc : MyClass, IClone
	{
		public object Clone()
		{
			return base.Clone() + "jack";
		}

		public string GetName()
		{
			return "hello, " + name;
		}
	}



	public interface IClone
	{
		object Clone();
	}

}
