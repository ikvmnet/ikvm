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
public enum Modifiers : short
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
	Synthetic		= -1			// we use this to record the fact that we created the method/field/property
									// so the member should not be visible through reflection
}

class ClassFile
{
	private ConstantPoolItem[] constantpool;
	private Modifiers access_flags;
	private string name;
	private string supername;
	private string[] interfaces;
	private Field[] fields;
	private Method[] methods;
	private Attribute[] attributes;
	private string sourceFile;
	private bool sourceFileCached;
	private static readonly char[] illegalcharacters = { '<', '>' };

	internal ClassFile(byte[] buf, int offset, int length, string inputClassName)
	{
		try
		{
			BigEndianBinaryReader br = new BigEndianBinaryReader(buf, offset);
			if(br.ReadUInt32() != 0xCAFEBABE)
			{
				throw JavaException.ClassFormatError("Bad magic number");
			}
			int minor = br.ReadUInt16();
			int major = br.ReadUInt16();
			if(major < 45 || major > 48)
			{
				throw JavaException.UnsupportedClassVersionError(major + "." + minor);
			}
			int constantpoolcount = br.ReadUInt16();
			constantpool = new ConstantPoolItem[constantpoolcount];
			for(int i = 1; i < constantpoolcount; i++)
			{
				constantpool[i] = ConstantPoolItem.Read(inputClassName, br);
				// LONG and DOUBLE items take up two slots...
				if(constantpool[i].IsWide)
				{
					i++;
				}
			}
			for(int i = 1; i < constantpoolcount; i++)
			{
				if(constantpool[i] != null)
				{
					try
					{
						constantpool[i].Resolve(this);
					}
					catch(IndexOutOfRangeException)
					{
						throw JavaException.ClassFormatError("{0} (Invalid constant pool item #{1})", inputClassName, i);
					}
					catch(InvalidCastException)
					{
						throw JavaException.ClassFormatError("{0} (Invalid constant pool item #{1})", inputClassName, i);
					}
				}
			}
			access_flags = (Modifiers)br.ReadUInt16();
			// NOTE although the vmspec says (in 4.1) that interfaces must be marked abstract, earlier versions of
			// javac (JDK 1.1) didn't do this, so the VM doesn't enforce this rule
			// NOTE although the vmspec implies (in 4.1) that ACC_SUPER is illegal on interfaces, it doesn't enforce this
			if((IsInterface && IsFinal) || (IsAbstract && IsFinal))
			{
				throw JavaException.ClassFormatError("{0} (Illegal class modifiers 0x{1:X})", inputClassName, access_flags);
			}
			int this_class = br.ReadUInt16();
			try
			{
				name = ((ConstantPoolItemClass)constantpool[this_class]).Name;
			}
			catch(Exception)
			{
				throw JavaException.ClassFormatError("{0} (Class name has bad constant pool index)", inputClassName);
			}
			int super_class = br.ReadUInt16();
			// NOTE for convenience we allow parsing java/lang/Object (which has no super class), so
			// we check for super_class != 0
			if(super_class != 0)
			{
				try
				{
					supername = ((ConstantPoolItemClass)constantpool[super_class]).Name;
				}
				catch(Exception)
				{
					throw JavaException.ClassFormatError("{0} (Bad superclass constant pool index)", inputClassName);
				}
			}
			else
			{
				if(this.Name != "java/lang/Object")
				{
					throw JavaException.ClassFormatError("{0} (Bad superclass index)", Name);
				}
			}
			if(IsInterface && (super_class == 0 || supername != "java/lang/Object"))
			{
				throw JavaException.ClassFormatError("{0} (Interfaces must have java.lang.Object as superclass)", Name);
			}
			int interfaces_count = br.ReadUInt16();
			interfaces = new string[interfaces_count];
			Hashtable interfaceNames = new Hashtable();
			for(int i = 0; i < interfaces_count; i++)
			{
				int index = br.ReadUInt16();
				if(index == 0 || index >= constantpool.Length)
				{
					throw JavaException.ClassFormatError("{0} (Illegal constant pool index)", Name);
				}
				ConstantPoolItemClass cpi = constantpool[index] as ConstantPoolItemClass;
				if(cpi == null)
				{
					throw JavaException.ClassFormatError("{0} (Interface name has bad constant type)", Name);
				}
				interfaces[i] = ((ConstantPoolItemClass)GetConstantPoolItem(index)).Name;
				if(interfaceNames.ContainsKey(interfaces[i]))
				{
					throw JavaException.ClassFormatError("{0} (Repetitive interface name)", Name);
				}
				interfaceNames.Add(interfaces[i], interfaces[i]);
			}
			int fields_count = br.ReadUInt16();
			fields = new Field[fields_count];
			Hashtable fieldNameSigs = new Hashtable();
			for(int i = 0; i < fields_count; i++)
			{
				fields[i] = new Field(this, br);
				string name = fields[i].Name;
				// NOTE It's not in the vmspec (as far as I can tell), but Sun's VM doens't allow names that
				// contain '<' or '>'
				if(name.Length == 0 || name.IndexOfAny(illegalcharacters) != -1)
				{
					throw JavaException.ClassFormatError("{0} (Illegal field name \"{1}\")", Name, name);
				}
				string nameSig = name + fields[i].Signature;
				if(fieldNameSigs.ContainsKey(nameSig))
				{
					throw JavaException.ClassFormatError("{0} (Repetitive field name/signature)", Name);
				}
				fieldNameSigs.Add(nameSig, nameSig);
			}
			int methods_count = br.ReadUInt16();
			methods = new Method[methods_count];
			Hashtable methodNameSigs = new Hashtable();
			for(int i = 0; i < methods_count; i++)
			{
				methods[i] = new Method(this, br);
				string name = methods[i].Name;
				string sig = methods[i].Signature;
				if(name.Length == 0 || (name.IndexOfAny(illegalcharacters) != -1 && name != "<init>" && name != "<clinit>"))
				{
					throw JavaException.ClassFormatError("{0} (Illegal method name \"{1}\")", Name, name);
				}
				if((name == "<init>" || name == "<clinit>") && !sig.EndsWith("V"))
				{
					throw JavaException.ClassFormatError("{0} (Method \"{1}\" has illegal signature \"{2}\")", Name, name, sig);
				}
				string nameSig = name + sig;
				if(methodNameSigs.ContainsKey(nameSig))
				{
					throw JavaException.ClassFormatError("{0} (Repetitive method name/signature)", Name);
				}
				methodNameSigs.Add(nameSig, nameSig);
			}
			int attributes_count = br.ReadUInt16();
			attributes = new Attribute[attributes_count];
			for(int i = 0; i < attributes_count; i++)
			{
				attributes[i] = Attribute.Read(this, br);
			}
			if(br.Position != offset + length)
			{
				if(br.Position > offset + length)
				{
					throw JavaException.ClassFormatError("Truncated class file");
				}
				else
				{
					throw JavaException.ClassFormatError("Extra bytes at the end of the class file");
				}
			}
		}
		catch(IndexOutOfRangeException)
		{
			throw JavaException.ClassFormatError("Truncated class file");
		}
//		catch(Exception)
//		{
//			FileStream fs = File.Create(inputClassName + ".broken");
//			fs.Write(buf, offset, length);
//			fs.Close();
//			throw;
//		}
	}

	internal Modifiers Modifiers
	{
		get
		{
			return access_flags;
		}
	}

	internal bool IsAbstract
	{
		get
		{
			return (access_flags & Modifiers.Abstract) != 0;
		}
	}

	internal bool IsFinal
	{
		get
		{
			return (access_flags & Modifiers.Final) != 0;
		}
	}

	internal bool IsPublic
	{
		get
		{
			return (access_flags & Modifiers.Public) != 0;
		}
	}

	internal bool IsInterface
	{
		get
		{
			return (access_flags & Modifiers.Interface) != 0;
		}
	}

	internal bool IsSuper
	{
		get
		{
			return (access_flags & Modifiers.Super) != 0;
		}
	}

	internal ConstantPoolItemFieldref GetFieldref(int index)
	{
		return (ConstantPoolItemFieldref)constantpool[index];
	}

	// NOTE this returns an FMI, because it used for both normal methods and interface methods
	internal ConstantPoolItemFMI GetMethodref(int index)
	{
		return (ConstantPoolItemFMI)constantpool[index];
	}

	private ConstantPoolItem GetConstantPoolItem(int index)
	{
		return constantpool[index];
	}

	internal string GetConstantPoolClass(int index)
	{
		return ((ConstantPoolItemClass)constantpool[index]).Name;
	}

	private string GetConstantPoolString(int index)
	{
		return ((ConstantPoolItemString)constantpool[index]).Value;
	}

	private string GetConstantPoolUtf8(int index)
	{
		return ((ConstantPoolItemUtf8)constantpool[index]).Value;
	}

	internal object GetConstantPoolConstant(int index)
	{
		ConstantPoolItem cpi = constantpool[index];
		if(cpi is ConstantPoolItemDouble)
		{
			return ((ConstantPoolItemDouble)cpi).Value;
		}
		else if(cpi is ConstantPoolItemFloat)
		{
			return ((ConstantPoolItemFloat)cpi).Value;
		}
		else if(cpi is ConstantPoolItemInteger)
		{
			return ((ConstantPoolItemInteger)cpi).Value;
		}
		else if(cpi is ConstantPoolItemLong)
		{
			return ((ConstantPoolItemLong)cpi).Value;
		}
		else if(cpi is ConstantPoolItemString)
		{
			return ((ConstantPoolItemString)cpi).Value;
		}
		else
		{
			return null;
		}
	}

	internal string Name
	{
		get
		{
			return name;
		}
	}

	internal string PackageName
	{
		get
		{
			int index = name.LastIndexOf('/');
			if(index == -1)
			{
				return "";
			}
			return name.Substring(0, index);
		}
	}

	internal string SuperClass
	{
		get
		{
			return supername;
		}
	}

	internal Field[] Fields
	{
		get
		{
			return fields;
		}
	}

	internal Method[] Methods
	{
		get
		{
			return methods;
		}
	}

	internal string[] Interfaces
	{
		get
		{
			return interfaces;
		}
	}

	private Attribute GetAttribute(string name)
	{
		for(int i = 0; i < attributes.Length; i++)
		{
			if(attributes[i].Name == name)
			{
				return attributes[i];
			}
		}
		return null;
	}

	internal string SourceFileAttribute
	{
		get
		{
			if(!sourceFileCached)
			{
				sourceFileCached = true;
				Attribute attr = GetAttribute("SourceFile");
				if(attr != null)
				{
					sourceFile = ((ConstantPoolItemUtf8)GetConstantPoolItem(attr.Data.ReadUInt16())).Value;
				}
			}
			return sourceFile;
		}
	}

	internal string NetExpTypeAttribute
	{
		get
		{
			Attribute attr = GetAttribute("IK.VM.NET.Type");
			if(attr != null)
			{
				return ((ConstantPoolItemUtf8)GetConstantPoolItem(attr.Data.ReadUInt16())).Value;
			}
			return null;
		}
	}

	internal abstract class ConstantPoolItem
	{
		internal virtual bool IsWide
		{
			get
			{
				return false;
			}
		}

		internal virtual void Resolve(ClassFile classFile)
		{
		}

		internal static ConstantPoolItem Read(string inputClassName, BigEndianBinaryReader br)
		{
			byte tag = br.ReadByte();
			switch((Constant)tag)
			{
				case Constant.Class:
					return new ConstantPoolItemClass(br);
				case Constant.Double:
					return new ConstantPoolItemDouble(br);
				case Constant.Fieldref:
					return new ConstantPoolItemFieldref(br);
				case Constant.Float:
					return new ConstantPoolItemFloat(br);
				case Constant.Integer:
					return new ConstantPoolItemInteger(br);
				case Constant.InterfaceMethodref:
					return new ConstantPoolItemInterfaceMethodref(br);
				case Constant.Long:
					return new ConstantPoolItemLong(br);
				case Constant.Methodref:
					return new ConstantPoolItemMethodref(br);
				case Constant.NameAndType:
					return new ConstantPoolItemNameAndType(br);
				case Constant.String:
					return new ConstantPoolItemString(br);
				case Constant.Utf8:
					return new ConstantPoolItemUtf8(inputClassName, br);
				default:
					throw JavaException.ClassFormatError("{0} (Illegal constant pool type 0x{1:X})", inputClassName, tag);
			}
		}
	}

	private class ConstantPoolItemClass : ConstantPoolItem
	{
		private ushort name_index;
		private string name;

		internal ConstantPoolItemClass(BigEndianBinaryReader br)
		{
			name_index = br.ReadUInt16();
		}

		internal override void Resolve(ClassFile classFile)
		{
			name = ((ConstantPoolItemUtf8)classFile.GetConstantPoolItem(name_index)).Value;;
		}

		internal string Name
		{
			get
			{
				return name;
			}
		}
	}

	private class ConstantPoolItemDouble : ConstantPoolItem
	{
		private double d;

		internal ConstantPoolItemDouble(BigEndianBinaryReader br)
		{
			d = br.ReadDouble();
		}

		internal double Value
		{
			get
			{
				return d;
			}
		}

		internal override bool IsWide
		{
			get
			{
				return true;
			}
		}
	}

	internal class ConstantPoolItemFMI : ConstantPoolItem
	{
		private ushort class_index;
		private ushort name_and_type_index;
		private string name;
		private string signature;
		private string clazz;

		internal ConstantPoolItemFMI(BigEndianBinaryReader br)
		{
			class_index = br.ReadUInt16();
			name_and_type_index = br.ReadUInt16();
		}

		internal override void Resolve(ClassFile classFile)
		{
			ConstantPoolItemNameAndType nat = (ConstantPoolItemNameAndType)classFile.GetConstantPoolItem(name_and_type_index);
			nat.Resolve(classFile);
			name = nat.Name;
			signature = nat.Type;
			ConstantPoolItemClass cls = (ConstantPoolItemClass)classFile.GetConstantPoolItem(class_index);
			cls.Resolve(classFile);
			clazz = cls.Name;
		}

		internal string Name
		{
			get
			{
				return name;
			}
		}

		internal string Signature
		{
			get
			{
				return signature;
			}
		}

		internal string Class
		{
			get
			{
				return clazz;
			}
		}
	}

	internal class ConstantPoolItemFieldref : ConstantPoolItemFMI
	{
		internal ConstantPoolItemFieldref(BigEndianBinaryReader br) : base(br)
		{
		}
	}

	internal class ConstantPoolItemMethodref : ConstantPoolItemFMI
	{
		internal ConstantPoolItemMethodref(BigEndianBinaryReader br) : base(br)
		{
		}
	}

	internal class ConstantPoolItemInterfaceMethodref : ConstantPoolItemFMI
	{
		internal ConstantPoolItemInterfaceMethodref(BigEndianBinaryReader br) : base(br)
		{
		}
	}

	private class ConstantPoolItemFloat : ConstantPoolItem
	{
		private float v;

		internal ConstantPoolItemFloat(BigEndianBinaryReader br)
		{
			v = br.ReadSingle();
		}

		internal float Value
		{
			get
			{
				return v;
			}
		}
	}

	private class ConstantPoolItemInteger : ConstantPoolItem
	{
		private int v;

		internal ConstantPoolItemInteger(BigEndianBinaryReader br)
		{
			v = br.ReadInt32();
		}

		internal int Value
		{
			get
			{
				return v;
			}
		}
	}

	private class ConstantPoolItemLong : ConstantPoolItem
	{
		private long l;

		internal ConstantPoolItemLong(BigEndianBinaryReader br)
		{
			l = br.ReadInt64();
		}

		internal long Value
		{
			get
			{
				return l;
			}
		}

		internal override bool IsWide
		{
			get
			{
				return true;
			}
		}
	}

	private class ConstantPoolItemNameAndType : ConstantPoolItem
	{
		private ushort name_index;
		private ushort descriptor_index;
		private string name;
		private string descriptor;

		internal ConstantPoolItemNameAndType(BigEndianBinaryReader br)
		{
			name_index = br.ReadUInt16();
			descriptor_index = br.ReadUInt16();
		}

		internal override void Resolve(ClassFile classFile)
		{
			ConstantPoolItemUtf8 nameUtf8 = (ConstantPoolItemUtf8)classFile.GetConstantPoolItem(name_index);
			nameUtf8.Resolve(classFile);
			name = nameUtf8.Value;
			ConstantPoolItemUtf8 descriptorUtf8 = (ConstantPoolItemUtf8)classFile.GetConstantPoolItem(descriptor_index);
			descriptorUtf8.Resolve(classFile);
			descriptor = descriptorUtf8.Value;
		}

		internal string Name
		{
			get
			{
				return name;
			}
		}

		internal string Type
		{
			get
			{
				return descriptor;
			}
		}
	}

	private class ConstantPoolItemString : ConstantPoolItem
	{
		private ushort string_index;
		private string s;

		internal ConstantPoolItemString(BigEndianBinaryReader br)
		{
			string_index = br.ReadUInt16();
		}

		internal override void Resolve(ClassFile classFile)
		{
			ConstantPoolItemUtf8 utf8 = (ConstantPoolItemUtf8)classFile.GetConstantPoolItem(string_index);
			utf8.Resolve(classFile);
			s = utf8.Value;
		}

		internal string Value
		{
			get
			{
				return s;
			}
		}
	}

	private class ConstantPoolItemUtf8 : ConstantPoolItem
	{
		private string s;

		internal ConstantPoolItemUtf8(string inputClassName, BigEndianBinaryReader br)
		{
			try
			{
				s = br.ReadString();
			}
			catch(FormatException)
			{
				throw JavaException.ClassFormatError("{0} (Illegal UTF8 string in constant pool)", inputClassName);
			}
		}

		internal string Value
		{
			get
			{
				return s;
			}
		}
	}

	private enum Constant
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

	internal class Attribute
	{
		private string name;
		private BigEndianBinaryReader data;

		private Attribute()
		{
		}

		internal static Attribute Read(ClassFile classFile, BigEndianBinaryReader br)
		{
			try
			{
				int name_index = br.ReadUInt16();
				string name = classFile.GetConstantPoolUtf8(name_index);
				int attribute_length = br.ReadInt32();
				Attribute a = new Attribute();
				a.name = name;
				a.data = br.Duplicate();
				br.Skip(attribute_length);
				return a;
			}
			catch(InvalidCastException)
			{
			}
			catch(NullReferenceException)
			{
			}
			catch(IndexOutOfRangeException)
			{
			}
			throw JavaException.ClassFormatError("{0} (Attribute name invalid type)", classFile.Name);
		}

		internal string Name
		{
			get
			{
				return name;
			}
		}

		internal BigEndianBinaryReader Data
		{
			get
			{
				return data.Duplicate();
			}
		}
	}

	internal class FieldOrMethod
	{
		private ClassFile classFile;
		protected Modifiers access_flags;
		private ushort name_index;
		private ushort descriptor_index;
		private Attribute[] attributes;

		internal FieldOrMethod(ClassFile classFile, BigEndianBinaryReader br)
		{
			this.classFile = classFile;
			access_flags = (Modifiers)br.ReadUInt16();
			// TODO check that name is ConstantPoolItemUtf8
			name_index = br.ReadUInt16();
			// TODO check that descriptor is ConstantPoolItemUtf8 and validate the descriptor
			descriptor_index = br.ReadUInt16();
			int attributes_count = br.ReadUInt16();
			attributes = new Attribute[attributes_count];
			for(int i = 0; i < attributes_count; i++)
			{
				attributes[i] = Attribute.Read(classFile, br);
			}
		}

		internal string Name
		{
			get
			{
				return classFile.GetConstantPoolUtf8(name_index);
			}
		}

		internal string Signature
		{
			get
			{
				return classFile.GetConstantPoolUtf8(descriptor_index);
			}
		}

		internal Modifiers Modifiers
		{
			get
			{
				return (Modifiers)access_flags;
			}
		}

		internal bool IsAbstract
		{
			get
			{
				return (access_flags & Modifiers.Abstract) != 0;
			}
		}

		internal bool IsFinal
		{
			get
			{
				return (access_flags & Modifiers.Final) != 0;
			}
		}

		internal bool IsPublic
		{
			get
			{
				return (access_flags & Modifiers.Public) != 0;
			}
		}

		internal bool IsPrivate
		{
			get
			{
				return (access_flags & Modifiers.Private) != 0;
			}
		}

		internal bool IsProtected
		{
			get
			{
				return (access_flags & Modifiers.Protected) != 0;
			}
		}

		internal bool IsStatic
		{
			get
			{
				return (access_flags & Modifiers.Static) != 0;
			}
		}

		internal bool IsSynchronized
		{
			get
			{
				return (access_flags & Modifiers.Synchronized) != 0;
			}
		}

		internal bool IsVolatile
		{
			get
			{
				return (access_flags & Modifiers.Volatile) != 0;
			}
		}

		internal bool IsTransient
		{
			get
			{
				return (access_flags & Modifiers.Transient) != 0;
			}
		}

		internal bool IsNative
		{
			get
			{
				return (access_flags & Modifiers.Native) != 0;
			}
		}

		protected Attribute GetAttribute(string name)
		{
			foreach(Attribute attr in attributes)
			{
				if(attr.Name == name)
				{
					return attr;
				}
			}
			return null;
		}

		internal ClassFile ClassFile
		{
			get
			{
				return classFile;
			}
		}
	}

	internal class Field : FieldOrMethod
	{
		private object constantValue;

		internal Field(ClassFile classFile, BigEndianBinaryReader br) : base(classFile, br)
		{
			if((IsPrivate && IsPublic) || (IsPrivate && IsProtected) || (IsPublic && IsProtected)
				|| (IsFinal && IsVolatile)
				|| (classFile.IsInterface && (!IsPublic || !IsStatic || !IsFinal || IsTransient)))
			{
				throw JavaException.ClassFormatError("{0} (Illegal field modifiers: 0x{1:X})", classFile.Name, access_flags);
			}
			Attribute attr = GetAttribute("ConstantValue");
			if(attr != null)
			{
				ushort index = attr.Data.ReadUInt16();
				try
				{
					object o = classFile.GetConstantPoolConstant(index);
					if(o == null)
					{
						throw JavaException.ClassFormatError("{0} (Bad index into constant pool)", classFile.Name);
					}
					switch(Signature)
					{
						case "I":
							constantValue = (int)o;
							break;
						case "S":
							constantValue = (short)(int)o;
							break;
						case "B":
							constantValue = (sbyte)(int)o;
							break;
						case "C":
							constantValue = (char)(int)o;
							break;
						case "Z":
							constantValue = ((int)o) != 0;
							break;
						case "J":
							constantValue = (long)o;
							break;
						case "F":
							constantValue = (float)o;
							break;
						case "D":
							constantValue = (double)o;
							break;
						case "Ljava/lang/String;":
							constantValue = (string)o;
							break;
						default:
							throw JavaException.ClassFormatError("{0} (Invalid signature for constant)", classFile.Name);
					}
				}
				catch(InvalidCastException)
				{
					throw JavaException.ClassFormatError("{0} (Bad index into constant pool)", classFile.Name);
				}
				catch(IndexOutOfRangeException)
				{
					throw JavaException.ClassFormatError("{0} (Bad index into constant pool)", classFile.Name);
				}
			}
		}

		internal object ConstantValue
		{
			get
			{
				return constantValue;
			}
		}
	}

	internal class Method : FieldOrMethod
	{
		private Code code;

		internal Method(ClassFile classFile, BigEndianBinaryReader br) : base(classFile, br)
		{
			// vmspec 4.6 says that all flags, except ACC_STRICT are ignored on <clinit>
			if(Name == "<clinit>")
			{
				access_flags &= Modifiers.Strictfp;
				access_flags |= (Modifiers.Static | Modifiers.Private);
			}
			else
			{
				if((Name == "<init>" && (IsStatic || IsSynchronized || IsFinal || IsAbstract))
					|| (IsPrivate && IsPublic) || (IsPrivate && IsProtected) || (IsPublic && IsProtected)
					|| (IsAbstract && (IsFinal || IsNative || IsPrivate || IsStatic || IsStrict || IsSynchronized))
					|| (classFile.IsInterface && (!IsPublic || !IsAbstract)))
				{
					throw JavaException.ClassFormatError("{0} (Illegal method modifiers: 0x{1:X})", classFile.Name, access_flags);
				}
			}
			// TODO if the method is abstract or native it may not have a Code attribute (right?)
			// and if it is not abstract or native, it must have a Code attribute
		}

		internal bool IsStrict
		{
			get
			{
				return (access_flags & Modifiers.Strictfp) != 0;
			}
		}

		internal string[] NetExpSigAttribute
		{
			get
			{
				Attribute attr = GetAttribute("IK.VM.NET.Sig");
				if(attr != null)
				{
					string s = ClassFile.GetConstantPoolUtf8(attr.Data.ReadUInt16());
					if(s.Length == 0)
					{
						return new string[0];
					}
					return s.Split('|');
				}
				return null;
			}
		}

		internal Code CodeAttribute
		{
			get
			{
				if(code == null)
				{
					Attribute attr = GetAttribute("Code");
					if(attr != null)
					{
						code = new Code(this, attr);
					}
				}
				return code;
			}
		}

		internal class Code
		{
			private Method method;
			private ushort max_stack;
			private ushort max_locals;
			private Instruction[] instructions;
			private int[] pcIndexMap;
			private ExceptionTableEntry[] exception_table;
			private Attribute[] codeAttributes;
			private int[] argmap;
			private LineNumberTableEntry[] lineNumberTable;
			private bool lineNumberTableCached;
			private LocalVariableTableEntry[] localVariableTable;
			private bool localVariableTableCached;

			internal Code(Method method, Attribute attr)
			{
				this.method = method;
				BigEndianBinaryReader br = attr.Data;
				max_stack = br.ReadUInt16();
				max_locals = br.ReadUInt16();
				uint code_length = br.ReadUInt32();
				ArrayList instructions = new ArrayList();
				int basePosition = br.Position;
				while(br.Position - basePosition < code_length)
				{
					instructions.Add(Instruction.Read(this, br.Position - basePosition, br));
				}
				// we add an additional nop instruction to make it easier for consumers of the code array
				instructions.Add(new Instruction(this, br.Position - basePosition, ByteCode.__nop));
				this.instructions = (Instruction[])instructions.ToArray(typeof(Instruction));
				ushort exception_table_length = br.ReadUInt16();
				exception_table = new ExceptionTableEntry[exception_table_length];
				for(int i = 0; i < exception_table_length; i++)
				{
					exception_table[i] = new ExceptionTableEntry();
					exception_table[i].start_pc = br.ReadUInt16();
					exception_table[i].end_pc = br.ReadUInt16();
					exception_table[i].handler_pc = br.ReadUInt16();
					exception_table[i].catch_type = br.ReadUInt16();
					exception_table[i].ordinal = i;
				}
				ushort attributes_count = br.ReadUInt16();
				codeAttributes = new Attribute[attributes_count];
				for(int i = 0; i < attributes_count; i++)
				{
					codeAttributes[i] = Attribute.Read(method.ClassFile, br);
				}
				// build the pcIndexMap
				pcIndexMap = new int[this.instructions[this.instructions.Length - 1].PC + 1];
				for(int i = 0; i < pcIndexMap.Length; i++)
				{
					pcIndexMap[i] = -1;
				}
				for(int i = 0; i < this.instructions.Length; i++)
				{
					pcIndexMap[this.instructions[i].PC] = i;
				}
			}

			// maps argument 'slot' (as encoded in the xload/xstore instructions) into the ordinal
			internal int[] ArgMap
			{
				get
				{
					if(argmap == null)
					{
						string sig = method.Signature;
						ArrayList args = new ArrayList();
						int pos = 0;
						if(!method.IsStatic)
						{
							args.Add(pos++);
						}
						for(int i = 1; sig[i] != ')'; i++)
						{
							args.Add(pos++);
							switch(sig[i])
							{
								case 'L':
									i = sig.IndexOf(';', i);
									break;
								case 'D':
								case 'J':
									args.Add(-1);
									break;
								case '[':
								{
									while(sig[i] == '[')
									{
										i++;
									}
									if(sig[i] == 'L')
									{
										i = sig.IndexOf(';', i);
									}
									break;
								}
							}
						}
						argmap = new int[args.Count];
						args.CopyTo(argmap);
					}
					return argmap;
				}
			}

			internal Method Method
			{
				get
				{
					return method;
				}
			}

			internal int MaxStack
			{
				get
				{
					return max_stack;
				}
			}

			internal int MaxLocals
			{
				get
				{
					return max_locals;
				}
			}

			internal Instruction[] Instructions
			{
				get
				{
					return instructions;
				}
			}

			// maps a PC to an index in the Instruction[], invalid PCs return -1
			internal int[] PcIndexMap
			{
				get
				{
					return pcIndexMap;
				}
			}

			internal ExceptionTableEntry[] ExceptionTable
			{
				get
				{
					return exception_table;
				}
			}

			private Attribute GetAttribute(string name)
			{
				foreach(Attribute attr in codeAttributes)
				{
					if(attr.Name == name)
					{
						return attr;
					}
				}
				return null;
			}

			internal LineNumberTableEntry[] LineNumberTableAttribute
			{
				get
				{
					if(!lineNumberTableCached)
					{
						lineNumberTableCached = true;
						Attribute attr = GetAttribute("LineNumberTable");
						if(attr != null)
						{
							BigEndianBinaryReader rdr = attr.Data;
							int count = rdr.ReadUInt16();
							lineNumberTable = new LineNumberTableEntry[count];
							for(int i = 0; i < count; i++)
							{
								lineNumberTable[i].start_pc = rdr.ReadUInt16();
								lineNumberTable[i].line_number = rdr.ReadUInt16();
							}
						}
					}
					return lineNumberTable;
				}
			}

			internal LocalVariableTableEntry[] LocalVariableTableAttribute
			{
				get
				{
					if(!localVariableTableCached)
					{
						localVariableTableCached = true;
						Attribute attr = GetAttribute("LocalVariableTable");
						if(attr != null)
						{
							BigEndianBinaryReader rdr = attr.Data;
							int count = rdr.ReadUInt16();
							localVariableTable = new LocalVariableTableEntry[count];
							for(int i = 0; i < count; i++)
							{
								localVariableTable[i].start_pc = rdr.ReadUInt16();
								localVariableTable[i].length = rdr.ReadUInt16();
								localVariableTable[i].name = method.ClassFile.GetConstantPoolUtf8(rdr.ReadUInt16());
								localVariableTable[i].descriptor = method.ClassFile.GetConstantPoolUtf8(rdr.ReadUInt16());
								localVariableTable[i].index = rdr.ReadUInt16();
							}
						}
					}
					return localVariableTable;
				}
			}
		}

		internal class ExceptionTableEntry
		{
			internal ushort start_pc;
			internal ushort end_pc;
			internal ushort handler_pc;
			internal ushort catch_type;
			internal int ordinal;
		}

		internal class Instruction
		{
			private Method.Code method;
			private int pc;
			private ByteCode opcode;
			private int arg1;
			private int arg2;
			private int default_offset;
			private int[] values;
			private int[] target_offsets;

			internal Instruction(Method.Code method, int pc, ByteCode opcode)
				: this(method, pc, opcode, 0)
			{
			}

			private Instruction(Method.Code method, int pc, ByteCode opcode, int arg1)
				: this(method, pc, opcode, arg1, 0)
			{
			}

			private Instruction(Method.Code method, int pc, ByteCode opcode, int arg1, int arg2)
			{
				this.method = method;
				this.pc = pc;
				this.opcode = opcode;
				this.arg1 = arg1;
				this.arg2 = arg2;
			}

			private Instruction(Method.Code method, int pc, ByteCode opcode, int default_offset, int[] values, int[] target_offsets)
				: this(method, pc, opcode)
			{
				this.default_offset = default_offset;
				this.values = values;
				this.target_offsets = target_offsets;
			}

			internal static Instruction Read(Method.Code method, int pc, BigEndianBinaryReader br)
			{
				ByteCode bc = (ByteCode)br.ReadByte();
				ByteCodeMode mode = ByteCodeMetaData.GetMode(bc);
				if(bc == ByteCode.__wide)
				{
					bc = (ByteCode)br.ReadByte();
					// NOTE the PC of a wide instruction is actually the PC of the
					// wide prefix, not the following instruction (vmspec 4.9.2)
					mode = ByteCodeMetaData.GetWideMode(bc);
				}
				switch(mode)
				{
					case ByteCodeMode.Simple:
						return new Instruction(method, pc, bc);
					case ByteCodeMode.Constant_1:
					case ByteCodeMode.Local_1:
						return new Instruction(method, pc, bc, br.ReadByte());
					case ByteCodeMode.Constant_2:
					case ByteCodeMode.Local_2:
						return new Instruction(method, pc, bc, br.ReadUInt16());
					case ByteCodeMode.Branch_2:
						return new Instruction(method, pc, bc, br.ReadInt16());
					case ByteCodeMode.Branch_4:
						return new Instruction(method, pc, bc, br.ReadInt32());
					case ByteCodeMode.Constant_2_1_1:
					{
						Instruction instr = new Instruction(method, pc, bc, br.ReadUInt16());
						// TODO validate these
						br.ReadByte();	// count
						br.ReadByte();	// unused
						return instr;
					}
					case ByteCodeMode.Immediate_1:
						return new Instruction(method, pc, bc, br.ReadSByte());
					case ByteCodeMode.Immediate_2:
						return new Instruction(method, pc, bc, br.ReadInt16());
					case ByteCodeMode.Local_1_Immediate_1:
						return new Instruction(method, pc, bc, br.ReadByte(), br.ReadSByte());
					case ByteCodeMode.Local_2_Immediate_2:
						return new Instruction(method, pc, bc, br.ReadUInt16(), br.ReadInt16());
					case ByteCodeMode.Constant_2_Immediate_1:
						return new Instruction(method, pc, bc, br.ReadUInt16(), br.ReadSByte());
					case ByteCodeMode.Tableswitch:
					{
						// skip the padding
						int p = pc + 1;
						int align = ((p + 3) & 0x7ffffffc) - p;
						for(int i = 0; i < align; i++)
						{
							br.ReadByte();
						}
						int default_offset = br.ReadInt32();
						int low = br.ReadInt32();
						int high = br.ReadInt32();
						int[] values = new int[high - low + 1];
						int[] target_offset = new int[high - low + 1];
						for(int i = low; i <= high; i++)
						{
							values[i - low] = i;
							target_offset[i - low] = br.ReadInt32();
						}
						return new Instruction(method, pc, bc, default_offset, values, target_offset);
					}
					case ByteCodeMode.Lookupswitch:
					{
						// skip the padding
						int p = pc + 1;
						int align = ((p + 3) & 0x7ffffffc) - p;
						for(int i = 0; i < align; i++)
						{
							br.ReadByte();
						}
						int default_offset = br.ReadInt32();
						int count = br.ReadInt32();
						int[] values = new int[count];
						int[] target_offset = new int[count];
						for(int i = 0; i < count; i++)
						{
							values[i] = br.ReadInt32();
							target_offset[i] = br.ReadInt32();
						}
						return new Instruction(method, pc, bc, default_offset, values, target_offset);
					}
					default:
						throw JavaException.ClassFormatError("Invalid opcode: {0}", bc);
				}
			}

			internal int PC
			{
				get
				{
					return pc;
				}
			}

			internal ByteCode OpCode
			{
				get
				{
					return opcode;
				}
			}

			internal NormalizedByteCode NormalizedOpCode
			{
				get
				{
					return ByteCodeMetaData.GetNormalizedByteCode(opcode);
				}
			}

			internal int Arg1
			{
				get
				{
					return arg1;
				}
			}

			internal int Arg2
			{
				get
				{
					return arg2;
				}
			}

			internal int NormalizedArg1
			{
				get
				{
					return ByteCodeMetaData.GetArg(opcode, arg1);
				}
			}

			internal int DefaultOffset
			{
				get
				{
					return default_offset;
				}
			}

			internal int[] Values
			{
				get
				{
					return values;
				}
			}

			internal int[] TargetOffsets
			{
				get
				{
					return target_offsets;
				}
			}

			internal Method.Code MethodCode
			{
				get
				{
					return method;
				}
			}
		}

		internal struct LineNumberTableEntry
		{
			internal int start_pc;
			internal int line_number;
		}

		internal struct LocalVariableTableEntry
		{
			internal int start_pc;
			internal int length;
			internal string name;
			internal string descriptor;
			internal int index;
		}
	}
}
