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
    
    public class Person
    {
        public string Name { get; set; }
        public double Age { get; set; }
        [DoNotSerialize]
        public long SSN { get; set; }
    }
	class Program
	{
        public static void Main(string[] args)
        {
            /*var myObj = new myclass[]
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
		}*/

            byte[] serialized = SerializeMethods.Serialize(new { someString = "Hi NSA!", someInteger = 5, someDouble = 2.8, someArrayOfInts = new int[] { 5, 2, 6, 8, 9 }, someArrayOfDoubles = new double[] { 2.5, 8.9, 2.7 } });
            dynamic obj = DeserializeMethods.Deserialize(serialized);
            object[] egers = obj.someArrayOfDoubles;
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
