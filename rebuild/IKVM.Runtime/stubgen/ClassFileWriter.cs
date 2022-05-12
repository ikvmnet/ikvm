/*
  Copyright (C) 2002-2013 Jeroen Frijters

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
using System.IO;
using System.Collections.Generic;
using IKVM.Attributes;

namespace IKVM.StubGen
{
	sealed class BigEndianStream
	{
		private Stream stream;

		public BigEndianStream(Stream stream)
		{
			this.stream = stream;
		}

		public void WriteUInt16(ushort s)
		{
			stream.WriteByte((byte)(s >> 8));
			stream.WriteByte((byte)s);
		}

		public void WriteUInt32(uint u)
		{
			stream.WriteByte((byte)(u >> 24));
			stream.WriteByte((byte)(u >> 16));
			stream.WriteByte((byte)(u >> 8));
			stream.WriteByte((byte)u);
		}

		public void WriteInt64(long l)
		{
			WriteUInt32((uint)(l >> 32));
			WriteUInt32((uint)l);
		}

		public void WriteFloat(float f)
		{
			WriteUInt32(BitConverter.ToUInt32(BitConverter.GetBytes(f), 0));
		}

		public void WriteDouble(double d)
		{
			WriteInt64(BitConverter.ToInt64(BitConverter.GetBytes(d), 0));
		}

		public void WriteByte(byte b)
		{
			stream.WriteByte(b);
		}

		public void WriteBytes(byte[] data)
		{
			stream.Write(data, 0, data.Length);
		}

		public void WriteUtf8(string str)
		{
			byte[] buf = new byte[str.Length * 3 + 1];
			int j = 0;
			for (int i = 0, e = str.Length; i < e; i++)
			{
				char ch = str[i];
				if ((ch != 0) && (ch <= 0x7f))
				{
					buf[j++] = (byte)ch;
				}
				else if (ch <= 0x7FF)
				{
					/* 11 bits or less. */
					byte high_five = (byte)(ch >> 6);
					byte low_six = (byte)(ch & 0x3F);
					buf[j++] = (byte)(high_five | 0xC0); /* 110xxxxx */
					buf[j++] = (byte)(low_six | 0x80);   /* 10xxxxxx */
				}
				else
				{
					/* possibly full 16 bits. */
					byte high_four = (byte)(ch >> 12);
					byte mid_six = (byte)((ch >> 6) & 0x3F);
					byte low_six = (byte)(ch & 0x3f);
					buf[j++] = (byte)(high_four | 0xE0); /* 1110xxxx */
					buf[j++] = (byte)(mid_six | 0x80);   /* 10xxxxxx */
					buf[j++] = (byte)(low_six | 0x80);   /* 10xxxxxx*/
				}
			}
			WriteUInt16((ushort)j);
			stream.Write(buf, 0, j);
		}
	}

	enum Constant
	{
		Utf8 = 1,
		Integer = 3,
		Float = 4,
		Long = 5,
		Double = 6,
		Class = 7,
		String = 8,
		Fieldref = 9,
		Methodref = 10,
		InterfaceMethodref = 11,
		NameAndType = 12
	}

	abstract class ConstantPoolItem
	{
		public abstract void Write(BigEndianStream bes);
	}

	sealed class ConstantPoolItemClass : ConstantPoolItem
	{
		private ushort name_index;

		public ConstantPoolItemClass(ushort name_index)
		{
			this.name_index = name_index;
		}

		public override int GetHashCode()
		{
			return name_index;
		}

		public override bool Equals(object o)
		{
			if (o != null && o.GetType() == typeof(ConstantPoolItemClass))
			{
				return ((ConstantPoolItemClass)o).name_index == name_index;
			}
			return false;
		}

		public override void Write(BigEndianStream bes)
		{
			bes.WriteByte((byte)Constant.Class);
			bes.WriteUInt16(name_index);
		}
	}

	sealed class ConstantPoolItemMethodref : ConstantPoolItem
	{
		private ushort class_index;
		private ushort name_and_type_index;

		public ConstantPoolItemMethodref(ushort class_index, ushort name_and_type_index)
		{
			this.class_index = class_index;
			this.name_and_type_index = name_and_type_index;
		}

		public override int GetHashCode()
		{
			return class_index | (name_and_type_index << 16);
		}

		public override bool Equals(object o)
		{
			if (o != null && o.GetType() == typeof(ConstantPoolItemMethodref))
			{
				ConstantPoolItemMethodref m = (ConstantPoolItemMethodref)o;
				return m.class_index == class_index && m.name_and_type_index == name_and_type_index;
			}
			return false;
		}

		public override void Write(BigEndianStream bes)
		{
			bes.WriteByte((byte)Constant.Methodref);
			bes.WriteUInt16(class_index);
			bes.WriteUInt16(name_and_type_index);
		}
	}

	sealed class ConstantPoolItemNameAndType : ConstantPoolItem
	{
		private ushort name_index;
		private ushort descriptor_index;

		public ConstantPoolItemNameAndType(ushort name_index, ushort descriptor_index)
		{
			this.name_index = name_index;
			this.descriptor_index = descriptor_index;
		}

		public override int GetHashCode()
		{
			return name_index | (descriptor_index << 16);
		}

		public override bool Equals(object o)
		{
			if (o != null && o.GetType() == typeof(ConstantPoolItemNameAndType))
			{
				ConstantPoolItemNameAndType n = (ConstantPoolItemNameAndType)o;
				return n.name_index == name_index && n.descriptor_index == descriptor_index;
			}
			return false;
		}

		public override void Write(BigEndianStream bes)
		{
			bes.WriteByte((byte)Constant.NameAndType);
			bes.WriteUInt16(name_index);
			bes.WriteUInt16(descriptor_index);
		}
	}

	sealed class ConstantPoolItemUtf8 : ConstantPoolItem
	{
		private string str;

		public ConstantPoolItemUtf8(string str)
		{
			this.str = str;
		}

		public override int GetHashCode()
		{
			return str.GetHashCode();
		}

		public override bool Equals(object o)
		{
			if (o != null && o.GetType() == typeof(ConstantPoolItemUtf8))
			{
				return ((ConstantPoolItemUtf8)o).str == str;
			}
			return false;
		}

		public override void Write(BigEndianStream bes)
		{
			bes.WriteByte((byte)Constant.Utf8);
			bes.WriteUtf8(str);
		}
	}

	sealed class ConstantPoolItemInt : ConstantPoolItem
	{
		private int v;

		public ConstantPoolItemInt(int v)
		{
			this.v = v;
		}

		public override int GetHashCode()
		{
			return v;
		}

		public override bool Equals(object o)
		{
			if (o != null && o.GetType() == typeof(ConstantPoolItemInt))
			{
				return ((ConstantPoolItemInt)o).v == v;
			}
			return false;
		}

		public override void Write(BigEndianStream bes)
		{
			bes.WriteByte((byte)Constant.Integer);
			bes.WriteUInt32((uint)v);
		}
	}

	sealed class ConstantPoolItemLong : ConstantPoolItem
	{
		private long v;

		public ConstantPoolItemLong(long v)
		{
			this.v = v;
		}

		public override int GetHashCode()
		{
			return (int)v;
		}

		public override bool Equals(object o)
		{
			if (o != null && o.GetType() == typeof(ConstantPoolItemLong))
			{
				return ((ConstantPoolItemLong)o).v == v;
			}
			return false;
		}

		public override void Write(BigEndianStream bes)
		{
			bes.WriteByte((byte)Constant.Long);
			bes.WriteInt64(v);
		}
	}

	sealed class ConstantPoolItemFloat : ConstantPoolItem
	{
		private float v;

		public ConstantPoolItemFloat(float v)
		{
			this.v = v;
		}

		public override int GetHashCode()
		{
			return BitConverter.ToInt32(BitConverter.GetBytes(v), 0);
		}

		public override bool Equals(object o)
		{
			if (o != null && o.GetType() == typeof(ConstantPoolItemFloat))
			{
				return ((ConstantPoolItemFloat)o).v == v;
			}
			return false;
		}

		public override void Write(BigEndianStream bes)
		{
			bes.WriteByte((byte)Constant.Float);
			bes.WriteFloat(v);
		}
	}

	sealed class ConstantPoolItemDouble : ConstantPoolItem
	{
		private double v;

		public ConstantPoolItemDouble(double v)
		{
			this.v = v;
		}

		public override int GetHashCode()
		{
			long l = BitConverter.DoubleToInt64Bits(v);
			return ((int)l) ^ ((int)(l >> 32));
		}

		public override bool Equals(object o)
		{
			if (o != null && o.GetType() == typeof(ConstantPoolItemDouble))
			{
				return ((ConstantPoolItemDouble)o).v == v;
			}
			return false;
		}

		public override void Write(BigEndianStream bes)
		{
			bes.WriteByte((byte)Constant.Double);
			bes.WriteDouble(v);
		}
	}

	sealed class ConstantPoolItemString : ConstantPoolItem
	{
		private ushort string_index;

		public ConstantPoolItemString(ushort string_index)
		{
			this.string_index = string_index;
		}

		public override int GetHashCode()
		{
			return string_index;
		}

		public override bool Equals(object o)
		{
			if (o != null && o.GetType() == typeof(ConstantPoolItemString))
			{
				return ((ConstantPoolItemString)o).string_index == string_index;
			}
			return false;
		}

		public override void Write(BigEndianStream bes)
		{
			bes.WriteByte((byte)Constant.String);
			bes.WriteUInt16(string_index);
		}
	}

	abstract class ClassFileAttribute
	{
		private ushort name_index;

		public ClassFileAttribute(ushort name_index)
		{
			this.name_index = name_index;
		}

		public virtual void Write(BigEndianStream bes)
		{
			bes.WriteUInt16(name_index);
		}
	}

	sealed class DeprecatedAttribute : ClassFileAttribute
	{
		internal DeprecatedAttribute(ClassFileWriter classFile)
			: base(classFile.AddUtf8("Deprecated"))
		{
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			bes.WriteUInt32(0);
		}
	}

	sealed class ConstantValueAttribute : ClassFileAttribute
	{
		private ushort constant_index;

		public ConstantValueAttribute(ushort name_index, ushort constant_index)
			: base(name_index)
		{
			this.constant_index = constant_index;
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			bes.WriteUInt32(2);
			bes.WriteUInt16(constant_index);
		}
	}

	sealed class StringAttribute : ClassFileAttribute
	{
		private ushort string_index;

		public StringAttribute(ushort name_index, ushort string_index)
			: base(name_index)
		{
			this.string_index = string_index;
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			bes.WriteUInt32(2);
			bes.WriteUInt16(string_index);
		}
	}

	sealed class InnerClassesAttribute : ClassFileAttribute
	{
		private ClassFileWriter classFile;
		private List<Item> classes = new List<Item>();

		public InnerClassesAttribute(ClassFileWriter classFile)
			: base(classFile.AddUtf8("InnerClasses"))
		{
			this.classFile = classFile;
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			bes.WriteUInt32((uint)(2 + 8 * classes.Count));
			bes.WriteUInt16((ushort)classes.Count);
			foreach (Item i in classes)
			{
				bes.WriteUInt16(i.inner_class_info_index);
				bes.WriteUInt16(i.outer_class_info_index);
				bes.WriteUInt16(i.inner_name_index);
				bes.WriteUInt16(i.inner_class_access_flags);
			}
		}

		private class Item
		{
			internal ushort inner_class_info_index;
			internal ushort outer_class_info_index;
			internal ushort inner_name_index;
			internal ushort inner_class_access_flags;
		}

		public void Add(string inner, string outer, string name, ushort access)
		{
			Item i = new Item();
			i.inner_class_info_index = classFile.AddClass(inner);
			i.outer_class_info_index = classFile.AddClass(outer);
			if (name != null)
			{
				i.inner_name_index = classFile.AddUtf8(name);
			}
			i.inner_class_access_flags = access;
			classes.Add(i);
		}
	}

	sealed class ExceptionsAttribute : ClassFileAttribute
	{
		private ClassFileWriter classFile;
		private List<ushort> classes = new List<ushort>();

		internal ExceptionsAttribute(ClassFileWriter classFile)
			: base(classFile.AddUtf8("Exceptions"))
		{
			this.classFile = classFile;
		}

		internal void Add(string exceptionClass)
		{
			classes.Add(classFile.AddClass(exceptionClass));
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			bes.WriteUInt32((uint)(2 + 2 * classes.Count));
			bes.WriteUInt16((ushort)classes.Count);
			foreach (ushort idx in classes)
			{
				bes.WriteUInt16(idx);
			}
		}
	}

	sealed class RuntimeVisibleAnnotationsAttribute : ClassFileAttribute
	{
		private ClassFileWriter classFile;
		private MemoryStream mem;
		private BigEndianStream bes;
		private ushort count;

		internal RuntimeVisibleAnnotationsAttribute(ClassFileWriter classFile)
			: base(classFile.AddUtf8("RuntimeVisibleAnnotations"))
		{
			this.classFile = classFile;
			mem = new MemoryStream();
			bes = new BigEndianStream(mem);
		}

		internal void Add(object[] annot)
		{
			count++;
			bes.WriteUInt16(classFile.AddUtf8((string)annot[1]));
			bes.WriteUInt16((ushort)((annot.Length - 2) / 2));
			for (int i = 2; i < annot.Length; i += 2)
			{
				bes.WriteUInt16(classFile.AddUtf8((string)annot[i]));
				WriteElementValue(bes, annot[i + 1]);
			}
		}

		private static string DecodeTypeName(string typeName)
		{
#if !FIRST_PASS && !STUB_GENERATOR
			int index = typeName.IndexOf(',');
			if (index > 0)
			{
				// HACK if we have an assembly qualified type name we have to resolve it to a Java class name
				// (at the very least we should use the right class loader here)
				try
				{
					typeName = "L" + java.lang.Class.forName(typeName.Substring(1, typeName.Length - 2).Replace('/', '.')).getName().Replace('.', '/') + ";";
				}
				catch { }
			}
#endif
			return typeName;
		}

		private void WriteElementValue(BigEndianStream bes, object val)
		{
			if (val is object[])
			{
				object[] arr = (object[])val;
				if (AnnotationDefaultAttribute.TAG_ENUM.Equals(arr[0]))
				{
					bes.WriteByte(AnnotationDefaultAttribute.TAG_ENUM);
					bes.WriteUInt16(classFile.AddUtf8(DecodeTypeName((string)arr[1])));
					bes.WriteUInt16(classFile.AddUtf8((string)arr[2]));
				}
				else if (AnnotationDefaultAttribute.TAG_ARRAY.Equals(arr[0]))
				{
					bes.WriteByte(AnnotationDefaultAttribute.TAG_ARRAY);
					bes.WriteUInt16((ushort)(arr.Length - 1));
					for (int i = 1; i < arr.Length; i++)
					{
						WriteElementValue(bes, arr[i]);
					}
				}
				else if (AnnotationDefaultAttribute.TAG_CLASS.Equals(arr[0]))
				{
					bes.WriteByte(AnnotationDefaultAttribute.TAG_CLASS);
					bes.WriteUInt16(classFile.AddUtf8(DecodeTypeName((string)arr[1])));
				}
				else if (AnnotationDefaultAttribute.TAG_ANNOTATION.Equals(arr[0]))
				{
					bes.WriteByte(AnnotationDefaultAttribute.TAG_ANNOTATION);
					bes.WriteUInt16(classFile.AddUtf8(DecodeTypeName((string)arr[1])));
					bes.WriteUInt16((ushort)((arr.Length - 2) / 2));
					for (int i = 2; i < arr.Length; i += 2)
					{
						bes.WriteUInt16(classFile.AddUtf8((string)arr[i]));
						WriteElementValue(bes, arr[i + 1]);
					}
				}
			}
			else if (val is bool)
			{
				bes.WriteByte((byte)'Z');
				bes.WriteUInt16(classFile.AddInt((bool)val ? 1 : 0));
			}
			else if (val is byte)
			{
				bes.WriteByte((byte)'B');
				bes.WriteUInt16(classFile.AddInt((byte)val));
			}
			else if (val is char)
			{
				bes.WriteByte((byte)'C');
				bes.WriteUInt16(classFile.AddInt((char)val));
			}
			else if (val is short)
			{
				bes.WriteByte((byte)'S');
				bes.WriteUInt16(classFile.AddInt((short)val));
			}
			else if (val is int)
			{
				bes.WriteByte((byte)'I');
				bes.WriteUInt16(classFile.AddInt((int)val));
			}
			else if (val is long)
			{
				bes.WriteByte((byte)'J');
				bes.WriteUInt16(classFile.AddLong((long)val));
			}
			else if (val is float)
			{
				bes.WriteByte((byte)'F');
				bes.WriteUInt16(classFile.AddFloat((float)val));
			}
			else if (val is double)
			{
				bes.WriteByte((byte)'D');
				bes.WriteUInt16(classFile.AddDouble((double)val));
			}
			else if (val is string)
			{
				bes.WriteByte((byte)'s');
				bes.WriteUInt16(classFile.AddUtf8((string)val));
			}
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			bes.WriteUInt32(Length);
			WriteImpl(bes);
		}

		internal void WriteImpl(BigEndianStream bes)
		{
			bes.WriteUInt16(count);
			foreach (byte b in mem.ToArray())
			{
				bes.WriteByte(b);
			}
		}

		internal uint Length
		{
			get { return (uint)mem.Length + 2; }
		}
	}

	sealed class RuntimeVisibleParameterAnnotationsAttribute : ClassFileAttribute
	{
		private readonly List<RuntimeVisibleAnnotationsAttribute> parameters = new List<RuntimeVisibleAnnotationsAttribute>();

		internal RuntimeVisibleParameterAnnotationsAttribute(ClassFileWriter classFile)
			: base(classFile.AddUtf8("RuntimeVisibleParameterAnnotations"))
		{
		}

		internal void Add(RuntimeVisibleAnnotationsAttribute parameter)
		{
			parameters.Add(parameter);
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			uint length = 1;
			foreach (RuntimeVisibleAnnotationsAttribute attr in parameters)
			{
				length += attr.Length;
			}
			bes.WriteUInt32(length);
			bes.WriteByte((byte)parameters.Count);
			foreach (RuntimeVisibleAnnotationsAttribute attr in parameters)
			{
				attr.WriteImpl(bes);
			}
		}
	}

	sealed class RuntimeVisibleTypeAnnotationsAttribute : ClassFileAttribute
	{
		private readonly byte[] data;

		internal RuntimeVisibleTypeAnnotationsAttribute(ClassFileWriter classFile, byte[] data)
			: base(classFile.AddUtf8("RuntimeVisibleTypeAnnotations"))
		{
			this.data = data;
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			bes.WriteUInt32((uint)data.Length);
			bes.WriteBytes(data);
		}
	}

	sealed class AnnotationDefaultClassFileAttribute : ClassFileAttribute
	{
		private ClassFileWriter classFile;
		private byte[] buf;

		internal AnnotationDefaultClassFileAttribute(ClassFileWriter classFile, byte[] buf)
			: base(classFile.AddUtf8("AnnotationDefault"))
		{
			this.classFile = classFile;
			this.buf = buf;
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			bes.WriteUInt32((uint)(buf.Length));
			foreach (byte b in buf)
			{
				bes.WriteByte(b);
			}
		}
	}

	sealed class FieldOrMethod : IAttributeOwner
	{
		private Modifiers access_flags;
		private ushort name_index;
		private ushort descriptor_index;
		private List<ClassFileAttribute> attribs = new List<ClassFileAttribute>();

		public FieldOrMethod(Modifiers access_flags, ushort name_index, ushort descriptor_index)
		{
			this.access_flags = access_flags;
			this.name_index = name_index;
			this.descriptor_index = descriptor_index;
		}

		public void AddAttribute(ClassFileAttribute attrib)
		{
			attribs.Add(attrib);
		}

		public void Write(BigEndianStream bes)
		{
			bes.WriteUInt16((ushort)access_flags);
			bes.WriteUInt16(name_index);
			bes.WriteUInt16(descriptor_index);
			bes.WriteUInt16((ushort)attribs.Count);
			for (int i = 0; i < attribs.Count; i++)
			{
				attribs[i].Write(bes);
			}
		}
	}

	sealed class CodeAttribute : ClassFileAttribute
	{
		private ClassFileWriter classFile;
		private ushort max_stack;
		private ushort max_locals;
		private byte[] code;

		public CodeAttribute(ClassFileWriter classFile)
			: base(classFile.AddUtf8("Code"))
		{
			this.classFile = classFile;
		}

		public ushort MaxStack
		{
			get { return max_stack; }
			set { max_stack = value; }
		}

		public ushort MaxLocals
		{
			get { return max_locals; }
			set { max_locals = value; }
		}

		public byte[] ByteCode
		{
			get { return code; }
			set { code = value; }
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			bes.WriteUInt32((uint)(2 + 2 + 4 + code.Length + 2 + 2));
			bes.WriteUInt16(max_stack);
			bes.WriteUInt16(max_locals);
			bes.WriteUInt32((uint)code.Length);
			for (int i = 0; i < code.Length; i++)
			{
				bes.WriteByte(code[i]);
			}
			bes.WriteUInt16(0);	// no exceptions
			bes.WriteUInt16(0); // no attributes
		}
	}

	sealed class MethodParametersAttribute : ClassFileAttribute
	{
		private readonly ClassFileWriter classFile;
		private readonly ushort[] names;
		private readonly ushort[] flags;

		internal MethodParametersAttribute(ClassFileWriter classFile, ushort[] names, ushort[] flags)
			: base(classFile.AddUtf8("MethodParameters"))
		{
			this.classFile = classFile;
			this.names = names;
			this.flags = flags;
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			if (flags == null || names == null || flags.Length != names.Length)
			{
				// write a malformed MethodParameters attribute
				bes.WriteUInt32(0);
				return;
			}
			bes.WriteUInt32((uint)(1 + names.Length * 4));
			bes.WriteByte((byte)names.Length);
			for (int i = 0; i < names.Length; i++)
			{
				bes.WriteUInt16(names[i]);
				bes.WriteUInt16(flags[i]);
			}
		}
	}

	interface IAttributeOwner
	{
		void AddAttribute(ClassFileAttribute attrib);
	}

	sealed class ClassFileWriter : IAttributeOwner
	{
		private List<ConstantPoolItem> cplist = new List<ConstantPoolItem>();
		private Dictionary<ConstantPoolItem, ushort> cphashtable = new Dictionary<ConstantPoolItem, ushort>();
		private List<FieldOrMethod> fields = new List<FieldOrMethod>();
		private List<FieldOrMethod> methods = new List<FieldOrMethod>();
		private List<ClassFileAttribute> attribs = new List<ClassFileAttribute>();
		private List<ushort> interfaces = new List<ushort>();
		private Modifiers access_flags;
		private ushort this_class;
		private ushort super_class;
		private ushort minorVersion;
		private ushort majorVersion;

		public ClassFileWriter(Modifiers mods, string name, string super, ushort minorVersion, ushort majorVersion)
		{
			cplist.Add(null);
			access_flags = mods;
			this_class = AddClass(name);
			if (super != null)
			{
				super_class = AddClass(super);
			}
			this.minorVersion = minorVersion;
			this.majorVersion = majorVersion;
		}

		private ushort Add(ConstantPoolItem cpi)
		{
			ushort index;
			if (!cphashtable.TryGetValue(cpi, out index))
			{
				index = (ushort)cplist.Count;
				cplist.Add(cpi);
				if (cpi is ConstantPoolItemDouble || cpi is ConstantPoolItemLong)
				{
					cplist.Add(null);
				}
				cphashtable.Add(cpi, index);
			}
			return index;
		}

		public ushort AddUtf8(string str)
		{
			return Add(new ConstantPoolItemUtf8(str));
		}

		public ushort AddClass(string classname)
		{
			return Add(new ConstantPoolItemClass(AddUtf8(classname)));
		}

		public ushort AddMethodRef(string classname, string methodname, string signature)
		{
			return Add(new ConstantPoolItemMethodref(AddClass(classname), AddNameAndType(methodname, signature)));
		}

		public ushort AddNameAndType(string name, string type)
		{
			return Add(new ConstantPoolItemNameAndType(AddUtf8(name), AddUtf8(type)));
		}

		public ushort AddInt(int i)
		{
			return Add(new ConstantPoolItemInt(i));
		}

		public ushort AddLong(long l)
		{
			return Add(new ConstantPoolItemLong(l));
		}

		public ushort AddFloat(float f)
		{
			return Add(new ConstantPoolItemFloat(f));
		}

		public ushort AddDouble(double d)
		{
			return Add(new ConstantPoolItemDouble(d));
		}

		public ushort AddString(string s)
		{
			return Add(new ConstantPoolItemString(AddUtf8(s)));
		}

		public void AddInterface(string name)
		{
			interfaces.Add(AddClass(name));
		}

		public FieldOrMethod AddMethod(Modifiers access, string name, string signature)
		{
			FieldOrMethod method = new FieldOrMethod(access, AddUtf8(name), AddUtf8(signature));
			methods.Add(method);
			return method;
		}

		public FieldOrMethod AddField(Modifiers access, string name, string signature, object constantValue)
		{
			FieldOrMethod field = new FieldOrMethod(access, AddUtf8(name), AddUtf8(signature));
			if (constantValue != null)
			{
				ushort constantValueIndex;
				if (constantValue is byte)
				{
					constantValueIndex = AddInt((sbyte)(byte)constantValue);
				}
				else if (constantValue is bool)
				{
					constantValueIndex = AddInt((bool)constantValue ? 1 : 0);
				}
				else if (constantValue is short)
				{
					constantValueIndex = AddInt((short)constantValue);
				}
				else if (constantValue is char)
				{
					constantValueIndex = AddInt((char)constantValue);
				}
				else if (constantValue is int)
				{
					constantValueIndex = AddInt((int)constantValue);
				}
				else if (constantValue is long)
				{
					constantValueIndex = AddLong((long)constantValue);
				}
				else if (constantValue is float)
				{
					constantValueIndex = AddFloat((float)constantValue);
				}
				else if (constantValue is double)
				{
					constantValueIndex = AddDouble((double)constantValue);
				}
				else if (constantValue is string)
				{
					constantValueIndex = AddString((string)constantValue);
				}
				else
				{
					throw new InvalidOperationException(constantValue.GetType().FullName);
				}
				field.AddAttribute(new ConstantValueAttribute(AddUtf8("ConstantValue"), constantValueIndex));
			}
			fields.Add(field);
			return field;
		}

		public ClassFileAttribute MakeStringAttribute(string name, string value)
		{
			return new StringAttribute(AddUtf8(name), AddUtf8(value));
		}

		public void AddStringAttribute(string name, string value)
		{
			attribs.Add(MakeStringAttribute(name, value));
		}

		public void AddAttribute(ClassFileAttribute attrib)
		{
			attribs.Add(attrib);
		}

		public void Write(Stream stream)
		{
			BigEndianStream bes = new BigEndianStream(stream);
			bes.WriteUInt32(0xCAFEBABE);
			bes.WriteUInt16(minorVersion);
			bes.WriteUInt16(majorVersion);
			bes.WriteUInt16((ushort)cplist.Count);
			foreach (ConstantPoolItem cpi in cplist)
			{
				if (cpi != null)
				{
					cpi.Write(bes);
				}
			}
			bes.WriteUInt16((ushort)access_flags);
			bes.WriteUInt16(this_class);
			bes.WriteUInt16(super_class);
			// interfaces count
			bes.WriteUInt16((ushort)interfaces.Count);
			for (int i = 0; i < interfaces.Count; i++)
			{
				bes.WriteUInt16(interfaces[i]);
			}
			// fields count
			bes.WriteUInt16((ushort)fields.Count);
			for (int i = 0; i < fields.Count; i++)
			{
				fields[i].Write(bes);
			}
			// methods count
			bes.WriteUInt16((ushort)methods.Count);
			for (int i = 0; i < methods.Count; i++)
			{
				methods[i].Write(bes);
			}
			// attributes count
			bes.WriteUInt16((ushort)attribs.Count);
			for (int i = 0; i < attribs.Count; i++)
			{
				attribs[i].Write(bes);
			}
		}
	}
}
