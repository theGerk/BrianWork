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
			myobj obj = new myobj()
			{
				mystrs = args,
				myints = new int[] { 1, 2, 3, 4, -5, 4, 0 },
				inintint = new int[][][]
				{
					new int[][]
					{
						new int[] {0,1,2 },
						new int[] {3,4,5 },
						new int[] {6,7,8 }
					},
					new int[][]
					{
						new int[] {9,10,11 },
						new int[] {12,13,14 },
						new int[] { }
					},
					new int[][]
					{
						null,
						new int[] {-49, 64, 70 },
						null
					}
				}
			};
			byte[] bytes = Serailizer.SerializeMethods.Serailize(obj);
			Console.WriteLine(bytes.Length);
			foreach(var b in bytes)
			{
				Console.WriteLine(b);
			}
		}
	}

	class myobj
	{
		public string[] mystrs;
		public int[] myints { get; set; }
		public int[][][] inintint;
	}
}
