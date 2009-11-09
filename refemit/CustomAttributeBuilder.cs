/*
  Copyright (C) 2008 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Reflection;
using System.IO;
using IKVM.Reflection.Emit.Writer;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace IKVM.Reflection.Emit
{
	public class CustomAttributeBuilder
	{
		private readonly ConstructorInfo con;
		private readonly byte[] blob;
		private readonly object[] constructorArgs;
		private readonly PropertyInfo[] namedProperties;
		private readonly object[] propertyValues;
		private readonly FieldInfo[] namedFields;
		private readonly object[] fieldValues;

		internal CustomAttributeBuilder(ConstructorInfo con, byte[] blob)
		{
			this.con = con;
			this.blob = blob;
			this.constructorArgs = null;
			this.namedProperties = null;
			this.propertyValues = null;
			this.namedFields = null;
			this.fieldValues = null;
		}

		public CustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs)
			: this(con, constructorArgs, null, null, null,null)
		{
		}

		public CustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs, FieldInfo[] namedFields, object[] fieldValues)
			: this(con, constructorArgs, null, null, namedFields, fieldValues)
		{
		}

		public CustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues, FieldInfo[] namedFields, object[] fieldValues)
		{
			this.con = con;
			this.blob = null;
			this.constructorArgs = constructorArgs;
			this.namedProperties = namedProperties;
			this.propertyValues = propertyValues;
			this.namedFields = namedFields;
			this.fieldValues = fieldValues;
		}

		private class BlobWriter
		{
			private readonly ModuleBuilder moduleBuilder;
			private readonly CustomAttributeBuilder cab;
			private readonly MemoryStream str = new MemoryStream();

			internal BlobWriter(ModuleBuilder moduleBuilder, CustomAttributeBuilder cab)
			{
				this.moduleBuilder = moduleBuilder;
				this.cab = cab;
			}

			internal byte[] GetBlob()
			{
				// prolog
				WriteUInt16(1);
				ParameterInfo[] pi = cab.con.GetParameters();
				for (int i = 0; i < pi.Length; i++)
				{
					WriteFixedArg(pi[i].ParameterType, cab.constructorArgs[i]);
				}
				// NumNamed
				int named = 0;
				if (cab.namedFields != null)
				{
					named += cab.namedFields.Length;
				}
				if (cab.namedProperties != null)
				{
					named += cab.namedProperties.Length;
				}
				WriteUInt16((ushort)named);
				if (cab.namedFields != null)
				{
					for (int i = 0; i < cab.namedFields.Length; i++)
					{
						WriteNamedArg(0x53, cab.namedFields[i].FieldType, cab.namedFields[i].Name, cab.fieldValues[i]);
					}
				}
				if (cab.namedProperties != null)
				{
					for (int i = 0; i < cab.namedProperties.Length; i++)
					{
						WriteNamedArg(0x54, cab.namedProperties[i].PropertyType, cab.namedProperties[i].Name, cab.propertyValues[i]);
					}
				}
				return str.ToArray();
			}

			internal void WriteNamedArgForDeclSecurity(ByteBuffer bb)
			{
				// NumNamed
				int named = 0;
				if (cab.namedFields != null)
				{
					named += cab.namedFields.Length;
				}
				if (cab.namedProperties != null)
				{
					named += cab.namedProperties.Length;
				}
				WritePackedLen(named);
				if (cab.namedFields != null)
				{
					for (int i = 0; i < cab.namedFields.Length; i++)
					{
						WriteNamedArg(0x53, cab.namedFields[i].FieldType, cab.namedFields[i].Name, cab.fieldValues[i]);
					}
				}
				if (cab.namedProperties != null)
				{
					for (int i = 0; i < cab.namedProperties.Length; i++)
					{
						WriteNamedArg(0x54, cab.namedProperties[i].PropertyType, cab.namedProperties[i].Name, cab.propertyValues[i]);
					}
				}
				str.Position = 0;
				bb.Write(str);
			}

			private void WriteNamedArg(byte fieldOrProperty, Type type, string name, object value)
			{
				str.WriteByte(fieldOrProperty);
				WriteFieldOrPropType(type);
				WriteString(name);
				WriteFixedArg(type, value);
			}

			private void WriteUInt16(ushort value)
			{
				str.WriteByte((byte)value);
				str.WriteByte((byte)(value >> 8));
			}

			private void WriteInt32(int value)
			{
				str.WriteByte((byte)(value >> 0));
				str.WriteByte((byte)(value >> 8));
				str.WriteByte((byte)(value >> 16));
				str.WriteByte((byte)(value >> 24));
			}

			private void WriteFixedArg(Type type, object value)
			{
				if (type.IsArray)
				{
					if (value == null)
					{
						WriteInt32(-1);
					}
					else
					{
						Array array = (Array)value;
						Type elemType = type.GetElementType();
						WriteInt32(array.Length);
						foreach (object val in array)
						{
							WriteElem(elemType, val);
						}
					}
				}
				else
				{
					WriteElem(type, value);
				}
			}

			private void WriteElem(Type type, object value)
			{
				if (type == typeof(string))
				{
					WriteString((string)value);
				}
				else if (type == typeof(Type))
				{
					WriteTypeName((Type)value);
				}
				else if (type == typeof(object))
				{
					if (value == null)
					{
						type = typeof(string);
					}
					else if (value is Type)
					{
						// value.GetType() would return a RuntimeType, but we don't want to deal with that
						type = typeof(Type);
					}
					else
					{
						type = value.GetType();
					}
					WriteFieldOrPropType(type);
					WriteElem(type, value);
				}
				else if (type.IsArray)
				{
					if (value == null)
					{
						WriteInt32(-1);
					}
					else
					{
						Array array = (Array)value;
						Type elemType = type.GetElementType();
						WriteInt32(array.Length);
						foreach (object val in array)
						{
							WriteElem(elemType, val);
						}
					}
				}
				else if (type.IsEnum)
				{
					if (type is TypeBuilder)
					{
						WriteElem(((TypeBuilder)type).GetEnumUnderlyingType(), value);
					}
					else
					{
						WriteElem(Enum.GetUnderlyingType(type), value);
					}
				}
				else
				{
					switch (Type.GetTypeCode(type))
					{
						case TypeCode.Boolean:
							str.WriteByte((bool)value ? (byte)1 : (byte)0);
							break;
						case TypeCode.Char:
							WriteUInt16((char)value);
							break;
						case TypeCode.SByte:
							str.WriteByte((byte)(sbyte)value);
							break;
						case TypeCode.Byte:
							str.WriteByte((byte)value);
							break;
						case TypeCode.Int16:
							WriteUInt16((ushort)(short)value);
							break;
						case TypeCode.UInt16:
							WriteUInt16((ushort)value);
							break;
						case TypeCode.Int32:
							WriteInt32((int)value);
							break;
						case TypeCode.UInt32:
							WriteInt32((int)(uint)value);
							break;
						case TypeCode.Int64:
							WriteInt64((long)value);
							break;
						case TypeCode.UInt64:
							WriteInt64((long)(ulong)value);
							break;
						case TypeCode.Single:
							WriteSingle((float)value);
							break;
						case TypeCode.Double:
							WriteDouble((double)value);
							break;
						default:
							throw new NotImplementedException();
					}
				}
			}

			private void WriteInt64(long value)
			{
				WriteInt32((int)value);
				WriteInt32((int)(value >> 32));
			}

			private void WriteSingle(float value)
			{
				str.Write(BitConverter.GetBytes(value), 0, 4);
			}

			private void WriteDouble(double value)
			{
				WriteInt64(BitConverter.DoubleToInt64Bits(value));
			}

			private void WriteTypeName(Type type)
			{
				string name = null;
				if (type != null)
				{
					// we could also use just the FullName for mscorlib types (IkvmAssembly.GetAssembly(type) == IkvmAssembly.GetAssembly(typeof(object)))
					if (IkvmAssembly.GetAssembly(type) == moduleBuilder.Assembly)
					{
						name = type.FullName;
					}
					else
					{
						name = type.AssemblyQualifiedName;
					}
				}
				WriteString(name);
			}

			private void WriteString(string val)
			{
				if (val == null)
				{
					str.WriteByte(0xFF);
				}
				else
				{
					byte[] buf = System.Text.Encoding.UTF8.GetBytes(val);
					WritePackedLen(buf.Length);
					str.Write(buf, 0, buf.Length);
				}
			}

			private void WritePackedLen(int len)
			{
				if (len < 0 || len > 0x1FFFFFFF)
					throw new ArgumentOutOfRangeException();

				if (len <= 0x7F)
					str.WriteByte((byte)len);
				else if (len <= 0x3FFF)
				{
					str.WriteByte((byte)(0x80 | (len >> 8)));
					str.WriteByte((byte)len);
				}
				else
				{
					str.WriteByte((byte)(0xC0 | (len >> 24)));
					str.WriteByte((byte)(len >> 16));
					str.WriteByte((byte)(len >> 8));
					str.WriteByte((byte)len);
				}
			}

			private void WriteFieldOrPropType(Type type)
			{
				if (type == typeof(Type))
				{
					str.WriteByte(0x50);
				}
				else if (type == typeof(object))
				{
					str.WriteByte(0x51);
				}
				else if (type.IsArray)
				{
					str.WriteByte(0x1D);
					WriteFieldOrPropType(type.GetElementType());
				}
				else if (type.IsEnum)
				{
					str.WriteByte(0x55);
					WriteTypeName(type);
				}
				else
				{
					switch (Type.GetTypeCode(type))
					{
						case TypeCode.Boolean:
							str.WriteByte(0x02);
							break;
						case TypeCode.Char:
							str.WriteByte(0x03);
							break;
						case TypeCode.SByte:
							str.WriteByte(0x04);
							break;
						case TypeCode.Byte:
							str.WriteByte(0x05);
							break;
						case TypeCode.Int16:
							str.WriteByte(0x06);
							break;
						case TypeCode.UInt16:
							str.WriteByte(0x07);
							break;
						case TypeCode.Int32:
							str.WriteByte(0x08);
							break;
						case TypeCode.UInt32:
							str.WriteByte(0x09);
							break;
						case TypeCode.Int64:
							str.WriteByte(0x0A);
							break;
						case TypeCode.UInt64:
							str.WriteByte(0x0B);
							break;
						case TypeCode.Single:
							str.WriteByte(0x0C);
							break;
						case TypeCode.Double:
							str.WriteByte(0x0D);
							break;
						case TypeCode.String:
							str.WriteByte(0x0E);
							break;
						default:
							throw new NotImplementedException();
					}
				}
			}
		}

		internal bool IsPseudoCustomAttribute
		{
			get
			{
				Type type = con.DeclaringType;
				return type == typeof(AssemblyFlagsAttribute)
					|| type == typeof(AssemblyAlgorithmIdAttribute)
					|| type == typeof(AssemblyVersionAttribute)
					|| type == typeof(AssemblyKeyFileAttribute)
					|| type == typeof(AssemblyKeyNameAttribute)
					|| type == typeof(AssemblyCultureAttribute)
					|| type == typeof(DllImportAttribute)
					|| type == typeof(FieldOffsetAttribute)
					|| type == typeof(InAttribute)
					|| type == typeof(MarshalAsAttribute)
					|| type == typeof(MethodImplAttribute)
					|| type == typeof(OutAttribute)
					|| type == typeof(StructLayoutAttribute)
					|| type == typeof(NonSerializedAttribute)
					|| type == typeof(SerializableAttribute)
					|| type == typeof(OptionalAttribute)
					|| type == typeof(PreserveSigAttribute)
					|| type == typeof(ComImportAttribute)
					|| type == typeof(TypeForwardedToAttribute)
					|| type == typeof(SpecialNameAttribute)
					|| type == typeof(DefaultParameterValueAttribute);
			}
		}

		internal ConstructorInfo Constructor
		{
			get { return con; }
		}

		internal int WriteBlob(ModuleBuilder moduleBuilder)
		{
			BlobWriter bw = new BlobWriter(moduleBuilder, this);
			return moduleBuilder.Blobs.Add(ByteBuffer.Wrap(bw.GetBlob()));
		}

		internal bool IsBlob
		{
			get { return blob != null; }
		}

		internal object GetConstructorArgument(int pos)
		{
			return constructorArgs[pos];
		}

		internal int ConstructorArgumentCount
		{
			get { return constructorArgs == null ? 0 : constructorArgs.Length; }
		}

		internal object GetFieldValue(string name)
		{
			if (namedFields != null)
			{
				for (int i = 0; i < namedFields.Length; i++)
				{
					if (namedFields[i].Name == name)
					{
						return fieldValues[i];
					}
				}
			}
			return null;
		}

		internal void WriteNamedArgumentsForDeclSecurity(ModuleBuilder moduleBuilder, ByteBuffer bb)
		{
			BlobWriter bw = new BlobWriter(moduleBuilder, this);
			bw.WriteNamedArgForDeclSecurity(bb);
		}
	}
}
