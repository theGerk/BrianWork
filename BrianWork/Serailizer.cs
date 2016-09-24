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
		/// Contains static methods for deserailization
		/// </summary>
		//static class DeserializeMethods
		//{
		//	public static ObjectType Deserialize<ObjectType>()
		//}


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
			/// <returns>the number of bytes written</returns>
			public static int Serailize(object input, Stream outputStream)
			{
				
				using (BinaryWriter writer = new BinaryWriter(outputStream))
				{
					return Serailize(input, writer);
				}
			}

			/// <summary>
			/// Serailizes, into binary writer
			/// </summary>
			/// <param name="input">The object being serailized</param>
			/// <param name="writer">The BinaryWriter object</param>
			/// <returns>the number of bytes written</returns>
			public static int Serailize(object input, BinaryWriter writer)
			{
				int bytesWritten = 0;
				//check if it's null
				if (input == null)
				{
					bytesWritten += WriteNull(writer);
				}

				//write if it isn't null;
				else
				{
					bytesWritten += WriteType(input.GetType(), writer);

					bytesWritten += WriteValue(input, writer);
				}
				return bytesWritten;
			}


			/// <summary>
			/// serializes value of an object to the stream
			/// </summary>
			/// <param name="input">the object being serialized</param>
			/// <param name="writer">the binary writer to write with</param>
			/// <returns></returns>
			private static int WriteValue(object input, BinaryWriter writer)
			{
				int bytesWritten = 0;


				Type inputType = input.GetType();

				if (inputType.IsArray)
				{
					//ignoring multi-dimensional array, simply hope everything is a flat array or array of arrays.
					//will come back and fix later
					int arrayLength = (int)inputType.GetProperty("Length").GetValue(input);

					//write length of array
					writer.Write(arrayLength);
					bytesWritten += sizeof(int);

					//write value of each element
					foreach (var element in (IEnumerable)input)
					{
						//check if it's a null in the array
						if(element == null)
						{
							bytesWritten += WriteNull(writer);
						}
						else
						{
							bytesWritten += WriteValue(element, writer);
						}
					}
				}
				else    //non array types
				{
					switch (Type.GetTypeCode(inputType))
					{
						case System.TypeCode.Boolean:
							writer.Write((bool)input);
							bytesWritten += sizeof(bool);
							break;
						case System.TypeCode.Char:
							writer.Write((char)input);
							bytesWritten += sizeof(char);
							break;
						case System.TypeCode.SByte:
							writer.Write((sbyte)input);
							bytesWritten += sizeof(sbyte);
							break;
						case System.TypeCode.Byte:
							writer.Write((byte)input);
							bytesWritten += sizeof(byte);
							break;
						case System.TypeCode.Int16:
							writer.Write((short)input);
							bytesWritten += sizeof(short);
							break;
						case System.TypeCode.UInt16:
							writer.Write((ushort)input);
							bytesWritten += sizeof(ushort);
							break;
						case System.TypeCode.Int32:
							writer.Write((int)input);
							bytesWritten += sizeof(int);
							break;
						case System.TypeCode.UInt32:
							writer.Write((uint)input);
							bytesWritten += sizeof(uint);
							break;
						case System.TypeCode.Int64:
							writer.Write((long)input);
							bytesWritten += sizeof(long);
							break;
						case System.TypeCode.UInt64:
							writer.Write((ulong)input);
							bytesWritten += sizeof(ulong);
							break;
						case System.TypeCode.Single:
							writer.Write((float)input);
							bytesWritten += sizeof(float);
							break;
						case System.TypeCode.Double:
							writer.Write((double)input);
							bytesWritten += sizeof(double);
							break;
						case System.TypeCode.Decimal:
							writer.Write((decimal)input);
							bytesWritten += sizeof(decimal);
							break;
						case System.TypeCode.String:
							//unbox
							string stringVersionOfInput = (string)input;

							//write length of string
							int length = stringVersionOfInput.Length;
							writer.Write(length);
							bytesWritten += sizeof(int);

							//write string's values
							writer.Write(stringVersionOfInput.ToCharArray());
							bytesWritten += length;
							break;
						default: // object

							//write number of fields + properties
							writer.Write(inputType.GetFields().Length + inputType.GetProperties().Length);
							bytesWritten += sizeof(int);

							foreach(var feild in inputType.GetFields())
							{
								//write name
								bytesWritten += WriteValue(feild.Name, writer);

								//write value
								bytesWritten += Serailize(feild.GetValue(input), writer);
							}

							foreach(var property in inputType.GetProperties())
							{
								//write name
								bytesWritten += WriteValue(property.Name, writer);

								//write value
								bytesWritten += Serailize(property.GetValue(input), writer);
							}
							break;
					}
				}
				return bytesWritten;
			}



			/// <summary>
			/// Writes code saying there is a null and returns the size of the code in bytes.
			/// </summary>
			/// <param name="myBinaryWriter">Binary Writer to the stream</param>
			/// <returns>1</returns>
			private static int WriteNull(BinaryWriter myBinaryWriter)
			{
				myBinaryWriter.Write((TypeCodeEnumType)TypeCode.Null);
				return sizeof(TypeCodeEnumType);
			}


			/// <summary>
			/// Writes a type into the binary writer, also the embeded types. For an array it writes the array code and then the code for what it's an array of.
			/// </summary>
			/// <param name="type">The type to be serialized</param>
			/// <param name="writer">the Writer to the stream</param>
			/// <returns>bytes written when writing type</returns>
			private static int WriteType(Type type, BinaryWriter writer)
			{
				int bytesWritten = sizeof(TypeCodeEnumType);
				if (type.IsArray)
				{
					writer.Write((TypeCodeEnumType)TypeCode.Array);
					//could maybe be turned into loop in later version
					bytesWritten += WriteType(type.GetElementType(), writer);
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
				

				return bytesWritten;
			}
		}
	}
}