using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeCodeEnumType = System.SByte;

namespace BrianWork
{
	/// <summary>
	/// Contains work done for serailization, and deserailization.
	/// </summary>
	namespace Serailizer
	{
		enum TypeCode : TypeCodeEnumType
		{
			Null = -1,
			Array,
			Bool,
			Byte,
			SByte,
			Char,
			Int,
			UInt,
			UShort,
			ULong,
			Short,
			Long,
			Decimal,
			Float,
			Double,
			Object,
			String
		}


		/// <summary>
		/// Contains static methods for serailization.
		/// </summary>
		static class SerializeMethods
		{
			public static byte[] Serailize(object input)
			{
				using (MemoryStream stream = new MemoryStream())
				{
					Serailize(input, stream);
					return stream.ToArray();
				}
			}


			/// <summary>
			/// Takes any input and serailizes it, writing it to the stream passed in.
			/// </summary>
			/// <param name="input">object to be serailized</param>
			/// <param name="outputStream">stream to be written to</param>
			public static void Serailize(object input, Stream outputStream)
			{
				using (BinaryWriter writer = new BinaryWriter(outputStream))
				{
					Serailize(input, writer);
				}
			}

			/// <summary>
			/// Serailizes, into binary writer
			/// </summary>
			/// <param name="input">The object being serailized</param>
			/// <param name="writer">The BinaryWriter object</param>
			public static void Serailize(object input, BinaryWriter writer)
			{
				//check if it's null
				if (input == null)
				{
					WriteNull(writer);
				}

				//write if it isn't null;
				else
				{
					WriteType(input.GetType(), writer);

					WriteValue(input, writer);
				}
			}


			/// <summary>
			/// serializes value of an object to the stream
			/// </summary>
			/// <param name="input">the object being serialized</param>
			/// <param name="writer">the binary writer to write with</param>
			private static void WriteValue(object input, BinaryWriter writer)
			{


				Type inputType = input.GetType();

				if (inputType.IsArray)
				{
					//ignoring multi-dimensional array, simply hope everything is a flat array or array of arrays.
					//will come back and fix later
					int arrayLength = (int)inputType.GetProperty("Length").GetValue(input);

					//write length of array
					writer.Write(arrayLength);

					//write value of each element
					foreach (var element in (IEnumerable)input)
					{
						//check if it's a null in the array
						if(element == null)
						{
							WriteNull(writer);
						}
						else
						{
							WriteValue(element, writer);
						}
					}
				}
				else    //non array types
				{
					switch (Type.GetTypeCode(inputType))
					{
						case System.TypeCode.Boolean:
							writer.Write((bool)input);
							break;
						case System.TypeCode.Char:
							writer.Write((char)input);
							break;
						case System.TypeCode.SByte:
							writer.Write((sbyte)input);
							break;
						case System.TypeCode.Byte:
							writer.Write((byte)input);
							break;
						case System.TypeCode.Int16:
							writer.Write((short)input);
							break;
						case System.TypeCode.UInt16:
							writer.Write((ushort)input);
							break;
						case System.TypeCode.Int32:
							writer.Write((int)input);
							break;
						case System.TypeCode.UInt32:
							writer.Write((uint)input);
							break;
						case System.TypeCode.Int64:
							writer.Write((long)input);
							break;
						case System.TypeCode.UInt64:
							writer.Write((ulong)input);
							break;
						case System.TypeCode.Single:
							writer.Write((float)input);
							break;
						case System.TypeCode.Double:
							writer.Write((double)input);
							break;
						case System.TypeCode.Decimal:
							writer.Write((decimal)input);
							break;
						case System.TypeCode.String:
							writer.Write((string)input);
							break;
						default: // object

							//write number of fields + properties
							writer.Write(inputType.GetFields().Length + inputType.GetProperties().Length);

							foreach(var feild in inputType.GetFields())
							{
								//write name
								WriteValue(feild.Name, writer);

								//write value
								Serailize(feild.GetValue(input), writer);
							}

							foreach(var property in inputType.GetProperties())
							{
								//write name
								WriteValue(property.Name, writer);

								//write value
								Serailize(property.GetValue(input), writer);
							}
							break;
					}
				}
			}



			/// <summary>
			/// Writes code saying there is a null and returns the size of the code in bytes.
			/// </summary>
			/// <param name="myBinaryWriter">Binary Writer to the stream</param>
			private static void WriteNull(BinaryWriter myBinaryWriter)
			{
				myBinaryWriter.Write((TypeCodeEnumType)TypeCode.Null);
			}


			/// <summary>
			/// Writes a type into the binary writer, also the embeded types. For an array it writes the array code and then the code for what it's an array of.
			/// </summary>
			/// <param name="type">The type to be serialized</param>
			/// <param name="writer">the Writer to the stream</param>
			private static void WriteType(Type type, BinaryWriter writer)
			{
				if (type.IsArray)
				{
					writer.Write((TypeCodeEnumType)TypeCode.Array);
					//could maybe be turned into loop in later version
					WriteType(type.GetElementType(), writer);
				}
				else
				{
					switch (Type.GetTypeCode(type))
					{
						case System.TypeCode.Boolean:
							writer.Write((TypeCodeEnumType)TypeCode.Bool);
							break;
						case System.TypeCode.Char:
							writer.Write((TypeCodeEnumType)TypeCode.Char);
							break;
						case System.TypeCode.SByte:
							writer.Write((TypeCodeEnumType)TypeCode.SByte);
							break;
						case System.TypeCode.Byte:
							writer.Write((TypeCodeEnumType)TypeCode.Byte);
							break;
						case System.TypeCode.Int16:
							writer.Write((TypeCodeEnumType)TypeCode.Short);
							break;
						case System.TypeCode.UInt16:
							writer.Write((TypeCodeEnumType)TypeCode.UShort);
							break;
						case System.TypeCode.Int32:
							writer.Write((TypeCodeEnumType)TypeCode.Int);
							break;
						case System.TypeCode.UInt32:
							writer.Write((TypeCodeEnumType)TypeCode.UInt);
							break;
						case System.TypeCode.Int64:
							writer.Write((TypeCodeEnumType)TypeCode.Long);
							break;
						case System.TypeCode.UInt64:
							writer.Write((TypeCodeEnumType)TypeCode.ULong);
							break;
						case System.TypeCode.Single:
							writer.Write((TypeCodeEnumType)TypeCode.Float);
							break;
						case System.TypeCode.Double:
							writer.Write((TypeCodeEnumType)TypeCode.Double);
							break;
						case System.TypeCode.Decimal:
							writer.Write((TypeCodeEnumType)TypeCode.Decimal);
							break;
						case System.TypeCode.String:
							writer.Write((TypeCodeEnumType)TypeCode.String);
							break;
						default: // treat as object
							writer.Write((TypeCodeEnumType)TypeCode.Object);
							break;
					}
				}
				

			}
		}
	}
}