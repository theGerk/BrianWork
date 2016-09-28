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
	namespace Serializer
	{
		static class BinaryReaderExtension
		{
			/// <summary>
			/// reads as many primitives or strings as are asked for in count from the binary reader and then returns them in an array.
			/// </summary>
			/// <typeparam name="readType">Type being read</typeparam>
			/// <param name="count">the number of whatever type wer are reading to read</param>
			/// <param name="binaryReader">the binary reader to read from</param>
			/// <returns>an array of what has been read</returns>
			public static readType[] ReadType<readType>(int count, BinaryReader binaryReader)
			{
				readType[] output = new readType[count];
				for (int i = 0; i < count; i++)
				{
					output[i] = ReadType<readType>(binaryReader);
				}
				return output;
			}

			/// <summary>
			/// reads as many primitives or strings as are asked for in count from the binary reader and then returns them in an array.
			/// </summary>
			/// <param name="readType">Type being read</param>
			/// <param name="count">the number of whatever type wer are reading to read</param>
			/// <param name="binaryReader">the binary reader to read from</param>
			/// <returns>an array of what has been read, each boxed into an object</returns>
			public static object[] ReadType(Type readType, int count, BinaryReader binaryReader)
			{
				object[] output = new object[count];
				for(int i= 0; i < count; i++)
				{
					output[i] = ReadType(readType, binaryReader);
				}
				return output;
			}

			/// <summary>
			/// reads a single string or primitive from a binary reader and returns it
			/// </summary>
			/// <typeparam name="readType">the type being read</typeparam>
			/// <param name="binaryReader">the reader from which to read</param>
			/// <returns>the value that has just been read</returns>
			public static readType ReadType<readType>(BinaryReader binaryReader)
			{
				return (readType)ReadType(typeof(readType), binaryReader);
			}

			/// <summary>
			/// reads a single string or primitive from a binary reader and returns it
			/// </summary>
			/// <param name="readType">the type being read</param>
			/// <param name="binaryReader">the reader from which to read</param>
			/// <returns>the value that has just been read, boxed in an object</returns>
			public static object ReadType(Type readType, BinaryReader binaryReader)
			{
				switch (Type.GetTypeCode(readType))
				{
					case System.TypeCode.Boolean:
						return binaryReader.ReadBoolean();
						break;
					case System.TypeCode.Char:
						return binaryReader.ReadChar();
						break;
					case System.TypeCode.SByte:
						return binaryReader.ReadSByte();
						break;
					case System.TypeCode.Byte:
						return binaryReader.ReadByte();
						break;
					case System.TypeCode.Int16:
						return binaryReader.ReadInt16();
						break;
					case System.TypeCode.UInt16:
						return binaryReader.ReadUInt16();
						break;
					case System.TypeCode.Int32:
						return binaryReader.ReadInt32();
						break;
					case System.TypeCode.UInt32:
						return binaryReader.ReadUInt32();
						break;
					case System.TypeCode.Int64:
						return binaryReader.ReadInt64();
						break;
					case System.TypeCode.UInt64:
						return binaryReader.ReadUInt64();
						break;
					case System.TypeCode.Single:
						return binaryReader.ReadSingle();
						break;
					case System.TypeCode.Double:
						return binaryReader.ReadDouble();
						break;
					case System.TypeCode.Decimal:
						return binaryReader.ReadDecimal();
						break;
					case System.TypeCode.String:
						return binaryReader.ReadString();
						break;
					default:
						throw new NotImplementedException();
						break;
				}
			}
		}

		/// <summary>
		/// Code representing each type, a null is the only negitive.
		/// </summary>
		enum TypeCode : TypeCodeEnumType
		{
			Null = -1,
			Array = 0,
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
		static class DeserializeMethods
		{
			public static dynamic getValue(Type type, BinaryReader reader)
			{
				if (type == null)
				{
					return null;
				}
				else
				{
					if (type.IsArray)
					{
						//do array stuff
					}
					else
					{
						switch (Type.GetTypeCode(type))
						{
							case System.TypeCode.Boolean:
								return reader.ReadBoolean();
								break;
							case System.TypeCode.Char:
								return reader.ReadChar();
								break;
							case System.TypeCode.SByte:
								return reader.ReadSByte();
								break;
							case System.TypeCode.Byte:
								return reader.ReadByte();
								break;
							case System.TypeCode.Int16:
								return reader.ReadInt16();
								break;
							case System.TypeCode.UInt16:
								return reader.ReadUInt16();
								break;
							case System.TypeCode.Int32:
								return reader.ReadInt32();
								break;
							case System.TypeCode.UInt32:
								return reader.ReadUInt32();
								break;
							case System.TypeCode.Int64:
								return reader.ReadInt64();
								break;
							case System.TypeCode.UInt64:
								return reader.ReadUInt64();
								break;
							case System.TypeCode.Single:
								return reader.ReadSingle();
								break;
							case System.TypeCode.Double:
								return reader.ReadDouble();
								break;
							case System.TypeCode.Decimal:
								return reader.ReadDecimal();
								break;
							case System.TypeCode.String:
								return reader.ReadString();
								break;
							default:	//treat as object
								break;
						}
					}
				}
			}


			/// <summary>
			/// reads a type from the binary reader
			/// </summary>
			/// <param name="reader">the binary reader</param>
			/// <returns>a type based off the type code that is read</returns>
			public static Type getType(BinaryReader reader)
			{
				switch (BinaryReaderExtension.ReadType<TypeCode>(reader))
				{
					case TypeCode.Null:
						return null;
						break;
					case TypeCode.Array:
						return Array.CreateInstance(getType(reader), 0).GetType();
						break;
					case TypeCode.Bool:
						return typeof(bool);
						break;
					case TypeCode.Byte:
						return typeof(byte);
						break;
					case TypeCode.SByte:
						return typeof(sbyte);
						break;
					case TypeCode.Char:
						return typeof(char);
						break;
					case TypeCode.Int:
						return typeof(int);
						break;
					case TypeCode.UInt:
						return typeof(uint);
						break;
					case TypeCode.UShort:
						return typeof(ushort);
						break;
					case TypeCode.ULong:
						return typeof(ulong);
						break;
					case TypeCode.Short:
						return typeof(short);
						break;
					case TypeCode.Long:
						return typeof(long);
						break;
					case TypeCode.Decimal:
						return typeof(decimal);
						break;
					case TypeCode.Float:
						return typeof(float);
						break;
					case TypeCode.Double:
						return typeof(double);
						break;
					case TypeCode.Object:
						return typeof(object);
						break;
					case TypeCode.String:
						return typeof(string);
						break;
					default:
						throw new Exception("invalid type");
						break;
				}
			}
		}


		/// <summary>
		/// Contains static methods for serailization.
		/// </summary>
		static class SerializeMethods
		{
			public static byte[] Serialize(object input)
			{
				using (MemoryStream stream = new MemoryStream())
				{
					Serialize(input, stream);
					return stream.ToArray();
				}
			}


			/// <summary>
			/// Takes any input and Serializes it, writing it to the stream passed in.
			/// </summary>
			/// <param name="input">object to be Serialized</param>
			/// <param name="outputStream">stream to be written to</param>
			/// <returns>the number of bytes written</returns>
			public static int Serialize(object input, Stream outputStream)
			{
				
				using (BinaryWriter writer = new BinaryWriter(outputStream))
				{
					return Serialize(input, writer);
				}
			}

			/// <summary>
			/// Serializes, into binary writer
			/// </summary>
			/// <param name="input">The object being Serialized</param>
			/// <param name="writer">The BinaryWriter object</param>
			/// <returns>the number of bytes written</returns>
			public static int Serialize(object input, BinaryWriter writer)
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
								bytesWritten += Serialize(feild.GetValue(input), writer);
							}

							foreach(var property in inputType.GetProperties())
							{
								//write name
								bytesWritten += WriteValue(property.Name, writer);

								//write value
								bytesWritten += Serialize(property.GetValue(input), writer);
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