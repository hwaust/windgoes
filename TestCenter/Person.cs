using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCenter
{
	public class Person : WindGoes.Data.IPropertyManager
	{
		public Person()
		{
			Name = "unknown";
			Age = 18;
		}

		public Person(string n)
		{
			Name = n;
			Age = 18;
		}
		public int Age { get; set; }
		public string Name { get; set; }

		public string ToPString()
		{
			return Age + ":" + Name;
		}
		public void FromPString(string s)
		{
			try
			{
				int p = s.IndexOf(':');
				Age = int.Parse(s.Substring(0, p));
				Name = s.Substring(p + 1);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}

}
