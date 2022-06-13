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
using System.Collections.Generic;
using System.IO;

using IKVM.Attributes;

namespace IKVM.StubGen
{

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
