using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrianWork
{
	using Serializer;
	class Program
	{
		public static void Main(string[] args)
		{
			var myObj = new myclass[]
			{
				null,
				new myclass()
				{
					blah = 93,
					hello = "lalalal",
					dec = null,
					obj = new myclass()
					{
						hello = "hello",
						blah = 39,
						dec = new decimal[]
						{
							3.4M, 0.0000001M
						},
						obj = null
					}
				}
			};

			var mystep = SerializeMethods.Serialize(myObj);
			foreach (var s in mystep) Console.WriteLine(s);
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(DeserializeMethods.Deserialize(mystep)));
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(myObj));
		}
	}

	class myclass
	{
		public int blah;
		public string hello;
		public decimal[] dec;
		public object obj;
	}
}
