/*
  Copyright (C) 2002 Jeroen Frijters

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
using System.Collections;

[Flags]
enum Modifiers : short
{
	Public			= 0x0001,
	Private			= 0x0002,
	Protected		= 0x0004,
	Static			= 0x0008,
	Final			= 0x0010,
	Super			= 0x0020,
	Synchronized	= 0x0020,
	Volatile		= 0x0040,
	Transient		= 0x0080,
	Native			= 0x0100,
	Interface		= 0x0200,
	Abstract		= 0x0400,
	Strictfp		= 0x0800,
}

class BigEndianStream
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

	public void WriteUtf8(string str)
	{
		byte[] buf = new byte[str.Length * 3 + 1];
		int j = 0;
		for(int i = 0, e = str.Length; i < e; i++)
		{
			char ch = str[i];
			if ((ch != 0) && (ch <=0x7f))
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

class ConstantPoolItemClass : ConstantPoolItem
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
		if(o != null && o.GetType() == typeof(ConstantPoolItemClass))
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

class ConstantPoolItemUtf8 : ConstantPoolItem
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
		if(o != null && o.GetType() == typeof(ConstantPoolItemUtf8))
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

class ConstantPoolItemInt : ConstantPoolItem
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
		if(o != null && o.GetType() == typeof(ConstantPoolItemInt))
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

class ConstantPoolItemLong : ConstantPoolItem
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
		if(o != null && o.GetType() == typeof(ConstantPoolItemLong))
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

class ConstantPoolItemFloat : ConstantPoolItem
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
		if(o != null && o.GetType() == typeof(ConstantPoolItemFloat))
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

class ConstantPoolItemDouble : ConstantPoolItem
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
		if(o != null && o.GetType() == typeof(ConstantPoolItemDouble))
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

class ConstantPoolItemString : ConstantPoolItem
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
		if(o != null && o.GetType() == typeof(ConstantPoolItemString))
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

class ClassFileAttribute
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

class ConstantValueAttribute : ClassFileAttribute
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

class StringAttribute : ClassFileAttribute
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

class InnerClassesAttribute : ClassFileAttribute
{
	private ClassFileWriter classFile;
	private ArrayList classes = new ArrayList();

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
		foreach(Item i in classes)
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
		if(name != null)
		{
			i.inner_name_index = classFile.AddUtf8(name);
		}
		i.inner_class_access_flags = access;
		classes.Add(i);
	}
}

class ExceptionsAttribute : ClassFileAttribute
{
	private ClassFileWriter classFile;
	private ArrayList classes = new ArrayList();

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
		foreach(ushort idx in classes)
		{
			bes.WriteUInt16(idx);
		}
	}
}

class FieldOrMethod
{
	private Modifiers access_flags;
	private ushort name_index;
	private ushort descriptor_index;
	private ArrayList attribs = new ArrayList();

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
		for(int i = 0; i < attribs.Count; i++)
		{
			((ClassFileAttribute)attribs[i]).Write(bes);
		}
	}
}

class ClassFileWriter
{
	private ArrayList cplist = new ArrayList();
	private Hashtable cphashtable = new Hashtable();
	private ArrayList fields = new ArrayList();
	private ArrayList methods = new ArrayList();
	private ArrayList attribs = new ArrayList();
	private ArrayList interfaces = new ArrayList();
	private Modifiers access_flags;
	private ushort this_class;
	private ushort super_class;

	public ClassFileWriter(Modifiers mods, string name, string super)
	{
		cplist.Add(null);
		access_flags = mods;
		this_class = AddClass(name);
		if(super != null)
		{
			super_class = AddClass(super);
		}
	}

	private ushort Add(ConstantPoolItem cpi)
	{
		object index = cphashtable[cpi];
		if(index == null)
		{
			index = (ushort)cplist.Add(cpi);
			if(cpi is ConstantPoolItemDouble || cpi is ConstantPoolItemLong)
			{
				cplist.Add(null);
			}
			cphashtable[cpi] = index;
		}
		return (ushort)index;
	}

	public ushort AddUtf8(string str)
	{
		return Add(new ConstantPoolItemUtf8(str));
	}

	public ushort AddClass(string classname)
	{
		return Add(new ConstantPoolItemClass(AddUtf8(classname)));
	}

	private ushort AddInt(int i)
	{
		return Add(new ConstantPoolItemInt(i));
	}

	private ushort AddLong(long l)
	{
		return Add(new ConstantPoolItemLong(l));
	}

	private ushort AddFloat(float f)
	{
		return Add(new ConstantPoolItemFloat(f));
	}

	private ushort AddDouble(double d)
	{
		return Add(new ConstantPoolItemDouble(d));
	}

	private ushort AddString(string s)
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
		if(constantValue != null)
		{
			ushort constantValueIndex;
			if(constantValue is sbyte)
			{
				constantValueIndex = AddInt((sbyte)constantValue);
			}
			else if(constantValue is bool)
			{
				constantValueIndex = AddInt((bool)constantValue ? 1 : 0);
			}
			else if(constantValue is short)
			{
				constantValueIndex = AddInt((short)constantValue);
			}
			else if(constantValue is char)
			{
				constantValueIndex = AddInt((char)constantValue);
			}
			else if(constantValue is int)
			{
				constantValueIndex = AddInt((int)constantValue);
			}
			else if(constantValue is uint)
			{
				constantValueIndex = AddInt((int)(uint)constantValue);
			}
			else if(constantValue is long)
			{
				constantValueIndex = AddLong((long)constantValue);
			}
			else if(constantValue is ulong)
			{
				constantValueIndex = AddLong((long)(ulong)constantValue);
			}
			else if(constantValue is float)
			{
				constantValueIndex = AddFloat((float)constantValue);
			}
			else if(constantValue is double)
			{
				constantValueIndex = AddDouble((double)constantValue);
			}
			else if(constantValue is string)
			{
				constantValueIndex = AddString((string)constantValue);
			}
			else
			{
				throw new InvalidOperationException(constantValue.GetType ().FullName);
			}
			field.AddAttribute(new ConstantValueAttribute(AddUtf8("ConstantValue"), constantValueIndex));
		}
		fields.Add(field);
		return field;
	}

	public void AddStringAttribute(string name, string value)
	{
		attribs.Add(new StringAttribute(AddUtf8(name), AddUtf8(value)));
	}

	public void AddAttribute(ClassFileAttribute attrib)
	{
		attribs.Add(attrib);
	}

	public void Write(Stream stream)
	{
		BigEndianStream bes = new BigEndianStream(stream);
		bes.WriteUInt32(0xCAFEBABE);
		bes.WriteUInt16((ushort)3);
		bes.WriteUInt16((ushort)45);
		bes.WriteUInt16((ushort)cplist.Count);
		for(int i = 1; i < cplist.Count; i++)
		{
			ConstantPoolItem cpi = (ConstantPoolItem)cplist[i];
			if(cpi != null)
			{
				cpi.Write(bes);
			}
		}
		bes.WriteUInt16((ushort)access_flags);
		bes.WriteUInt16(this_class);
		bes.WriteUInt16(super_class);
		// interfaces count
		bes.WriteUInt16((ushort)interfaces.Count);
		for(int i = 0; i < interfaces.Count; i++)
		{
			bes.WriteUInt16((ushort)interfaces[i]);
		}
		// fields count
		bes.WriteUInt16((ushort)fields.Count);
		for(int i = 0; i < fields.Count; i++)
		{
			((FieldOrMethod)fields[i]).Write(bes);
		}
		// methods count
		bes.WriteUInt16((ushort)methods.Count);
		for(int i = 0; i < methods.Count; i++)
		{
			((FieldOrMethod)methods[i]).Write(bes);
		}
		// attributes count
		bes.WriteUInt16((ushort)attribs.Count);
		for(int i = 0; i < attribs.Count; i++)
		{
			((ClassFileAttribute)attribs[i]).Write(bes);
		}
	}
}
