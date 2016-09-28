using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrianWork.Serializer;
namespace BrianWork
{
	class Program
	{
		public static void Main(string[] args)
		{
			var myObj = new int[] { 4, 3 };
			Console.WriteLine(DeserializeMethods.Deserialize(SerializeMethods.Serialize(myObj)));
		}
	}
}
