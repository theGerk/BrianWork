using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrianWork
{
	class Program
	{
		public static void Main(string[] args)
		{
			byte[] bytes = new byte[10];
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				using (BinaryWriter bw = new BinaryWriter(stream))
				{
					bw.Write((sbyte)Serializer.TypeCode.Null);
				}
			}
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				using (BinaryReader br = new BinaryReader(stream))
				{
					Console.WriteLine(Serializer.BinaryReaderExtension.ReadType<Serializer.TypeCode>(br));
				}
			}


			foreach(var a in Array.CreateInstance(null, 10))
			{
				Console.WriteLine(a);
			}
		}
	}
}
