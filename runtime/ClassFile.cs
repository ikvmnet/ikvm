/*
  Copyright (C) 2002, 2003, 2004 Jeroen Frijters

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
using IKVM.Attributes;
using IKVM.Runtime;
using IKVM.Internal;

// MONOBUG mcs 1.0 still has problems resolving properties vs. type names
using __Modifiers = IKVM.Attributes.Modifiers;

class ClassFile
{
	private ConstantPoolItem[] constantpool;
	private string[] utf8_cp;
	private Modifiers access_flags;
	private ConstantPoolItemClass this_cpi;
	private ConstantPoolItemClass super_cpi;
	private ConstantPoolItemClass[] interfaces;
	private Field[] fields;
	private Method[] methods;
	private string sourceFile;
	private ClassFile outerClass;
	private ushort majorVersion;
	private bool deprecated;
	private string ikvmAssembly;
	private InnerClass[] innerClasses;
	private static readonly char[] illegalcharacters = { '<', '>' };

	internal ClassFile(byte[] buf, int offset, int length, string inputClassName, bool allowJavaLangObject)
	{
		try
		{
			BigEndianBinaryReader br = new BigEndianBinaryReader(buf, offset, length);
			if(br.ReadUInt32() != 0xCAFEBABE)
			{
				throw new ClassFormatError("Bad magic number");
			}
			int minorVersion = br.ReadUInt16();
			majorVersion = br.ReadUInt16();
			if(majorVersion < 45 || majorVersion > 48)
			{
				throw new UnsupportedClassVersionError(majorVersion + "." + minorVersion);
			}
			int constantpoolcount = br.ReadUInt16();
			constantpool = new ConstantPoolItem[constantpoolcount];
			utf8_cp = new string[constantpoolcount];
			for(int i = 1; i < constantpoolcount; i++)
			{
				Constant tag = (Constant)br.ReadByte();
				switch(tag)
				{
					case Constant.Class:
						constantpool[i] = new ConstantPoolItemClass(br);
						break;
					case Constant.Double:
						constantpool[i] = new ConstantPoolItemDouble(br);
						i++;
						break;
					case Constant.Fieldref:
						constantpool[i] = new ConstantPoolItemFieldref(br);
						break;
					case Constant.Float:
						constantpool[i] = new ConstantPoolItemFloat(br);
						break;
					case Constant.Integer:
						constantpool[i] = new ConstantPoolItemInteger(br);
						break;
					case Constant.InterfaceMethodref:
						constantpool[i] = new ConstantPoolItemInterfaceMethodref(br);
						break;
					case Constant.Long:
						constantpool[i] = new ConstantPoolItemLong(br);
						i++;
						break;
					case Constant.Methodref:
						constantpool[i] = new ConstantPoolItemMethodref(br);
						break;
					case Constant.NameAndType:
						constantpool[i] = new ConstantPoolItemNameAndType(br);
						break;
					case Constant.String:
						constantpool[i] = new ConstantPoolItemString(br);
						break;
					case Constant.Utf8:
						utf8_cp[i] = br.ReadString();
						break;
					default:
						throw new ClassFormatError("{0} (Illegal constant pool type 0x{1:X})", inputClassName, tag);
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
						throw new ClassFormatError("{0} (Invalid constant pool item #{1})", inputClassName, i);
					}
					catch(InvalidCastException)
					{
						throw new ClassFormatError("{0} (Invalid constant pool item #{1})", inputClassName, i);
					}
				}
			}
			access_flags = (Modifiers)br.ReadUInt16();
			// NOTE although the vmspec says (in 4.1) that interfaces must be marked abstract, earlier versions of
			// javac (JDK 1.1) didn't do this, so the VM doesn't enforce this rule
			// NOTE although the vmspec implies (in 4.1) that ACC_SUPER is illegal on interfaces, it doesn't enforce this
			if((IsInterface && IsFinal) || (IsAbstract && IsFinal))
			{
				throw new ClassFormatError("{0} (Illegal class modifiers 0x{1:X})", inputClassName, access_flags);
			}
			int this_class = br.ReadUInt16();
			try
			{
				this_cpi = (ConstantPoolItemClass)constantpool[this_class];
			}
			catch(Exception)
			{
				throw new ClassFormatError("{0} (Class name has bad constant pool index)", inputClassName);
			}
			int super_class = br.ReadUInt16();
			// NOTE for convenience we allow parsing java/lang/Object (which has no super class), so
			// we check for super_class != 0
			if(super_class != 0)
			{
				try
				{
					super_cpi = (ConstantPoolItemClass)constantpool[super_class];
				}
				catch(Exception)
				{
					throw new ClassFormatError("{0} (Bad superclass constant pool index)", inputClassName);
				}
			}
			else
			{
				if(this.Name != "java.lang.Object" || !allowJavaLangObject)
				{
					throw new ClassFormatError("{0} (Bad superclass index)", Name);
				}
			}
			if(IsInterface && (super_class == 0 || super_cpi.Name != "java.lang.Object"))
			{
				throw new ClassFormatError("{0} (Interfaces must have java.lang.Object as superclass)", Name);
			}
			// TODO are there any more checks we need to do on the class name?
			if(Name.Length == 0 || Name[0] == '[')
			{
				throw new ClassFormatError("Bad name");
			}
			int interfaces_count = br.ReadUInt16();
			interfaces = new ConstantPoolItemClass[interfaces_count];
			for(int i = 0; i < interfaces.Length; i++)
			{
				int index = br.ReadUInt16();
				if(index == 0 || index >= constantpool.Length)
				{
					throw new ClassFormatError("{0} (Illegal constant pool index)", Name);
				}
				ConstantPoolItemClass cpi = constantpool[index] as ConstantPoolItemClass;
				if(cpi == null)
				{
					throw new ClassFormatError("{0} (Interface name has bad constant type)", Name);
				}
				interfaces[i] = cpi;
				for(int j = 0; j < i; j++)
				{
					if(interfaces[j].Name == cpi.Name)
					{
						throw new ClassFormatError("{0} (Repetitive interface name)", Name);
					}
				}
			}
			int fields_count = br.ReadUInt16();
			fields = new Field[fields_count];
			Hashtable fieldNameSigs = new Hashtable();
			for(int i = 0; i < fields_count; i++)
			{
				fields[i] = new Field(this, br);
				string name = fields[i].Name;
				// NOTE It's not in the vmspec (as far as I can tell), but Sun's VM doesn't allow names that
				// contain '<' or '>'
				if(name.Length == 0 || name.IndexOfAny(illegalcharacters) != -1)
				{
					throw new ClassFormatError("{0} (Illegal field name \"{1}\")", Name, name);
				}
				try
				{
					fieldNameSigs.Add(name + fields[i].Signature, null);
				}
				catch(ArgumentException)
				{
					throw new ClassFormatError("{0} (Repetitive field name/signature)", Name);
				}
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
					throw new ClassFormatError("{0} (Illegal method name \"{1}\")", Name, name);
				}
				if((name == "<init>" || name == "<clinit>") && !sig.EndsWith("V"))
				{
					throw new ClassFormatError("{0} (Method \"{1}\" has illegal signature \"{2}\")", Name, name, sig);
				}
				try
				{
					methodNameSigs.Add(name + sig, null);
				}
				catch(ArgumentException)
				{
					throw new ClassFormatError("{0} (Repetitive method name/signature)", Name);
				}
			}
			int attributes_count = br.ReadUInt16();
			for(int i = 0; i < attributes_count; i++)
			{
				switch(GetConstantPoolUtf8String(br.ReadUInt16()))
				{
					case "Deprecated":
						deprecated = true;
						if(br.ReadUInt32() != 0)
						{
							throw new ClassFormatError("Deprecated attribute has non-zero length");
						}
						break;
					case "SourceFile":
						if(br.ReadUInt32() != 2)
						{
							throw new ClassFormatError("SourceFile attribute has incorrect length");
						}
						sourceFile = GetConstantPoolUtf8String(br.ReadUInt16());
						break;
					case "InnerClasses":
						BigEndianBinaryReader rdr = br.Section(br.ReadUInt32());
						ushort count = rdr.ReadUInt16();
						innerClasses = new InnerClass[count];
						for(int j = 0; j < innerClasses.Length; j++)
						{
							innerClasses[j].innerClass = rdr.ReadUInt16();
							innerClasses[j].outerClass = rdr.ReadUInt16();
							innerClasses[j].name = rdr.ReadUInt16();
							innerClasses[j].accessFlags = (Modifiers)rdr.ReadUInt16();
						}
						break;
					case "IKVM.NET.Assembly":
						if(br.ReadUInt32() != 2)
						{
							throw new ClassFormatError("IKVM.NET.Assembly attribute has incorrect length");
						}
						ikvmAssembly = GetConstantPoolUtf8String(br.ReadUInt16());
						break;
					default:
						br.Skip(br.ReadUInt32());
						break;
				}
			}
			if(br.Position != offset + length)
			{
				throw new ClassFormatError("Extra bytes at the end of the class file");
			}
		}
		catch(IndexOutOfRangeException)
		{
			throw new ClassFormatError("Truncated class file");
		}
//		catch(Exception)
//		{
//			FileStream fs = File.Create(inputClassName + ".broken");
//			fs.Write(buf, offset, length);
//			fs.Close();
//			throw;
//		}
	}

	internal int MajorVersion
	{
		get
		{
			return majorVersion;
		}
	}

	// NOTE this property is only used when statically compiling
	// (and it is set by the static compiler's class loader in vm.cs)
	internal ClassFile OuterClass
	{
		get
		{
			return outerClass;
		}
		set
		{
			outerClass = value;
		}
	}

	internal void Link(TypeWrapper thisType, Hashtable classCache)
	{
		for(int i = 1; i < constantpool.Length; i++)
		{
			if(constantpool[i] != null)
			{
				constantpool[i].Link(thisType, classCache);
			}
		}
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
			// interfaces are implicitly abstract
			return (access_flags & (Modifiers.Abstract | Modifiers.Interface)) != 0;
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

	// NOTE this returns an MI, because it used for both normal methods and interface methods
	internal ConstantPoolItemMI GetMethodref(int index)
	{
		return (ConstantPoolItemMI)constantpool[index];
	}

	private ConstantPoolItem GetConstantPoolItem(int index)
	{
		return constantpool[index];
	}

	internal string GetConstantPoolClass(int index)
	{
		return ((ConstantPoolItemClass)constantpool[index]).Name;
	}

	internal TypeWrapper GetConstantPoolClassType(int index)
	{
		return ((ConstantPoolItemClass)constantpool[index]).GetClassType();
	}

	private string GetConstantPoolString(int index)
	{
		return ((ConstantPoolItemString)constantpool[index]).Value;
	}

	internal string GetConstantPoolUtf8String(int index)
	{
		return utf8_cp[index];
	}

	internal ConstantType GetConstantPoolConstantType(int index)
	{
		return constantpool[index].GetConstantType();
	}

	internal double GetConstantPoolConstantDouble(int index)
	{
		return ((ConstantPoolItemDouble)constantpool[index]).Value;
	}

	internal float GetConstantPoolConstantFloat(int index)
	{
		return ((ConstantPoolItemFloat)constantpool[index]).Value;
	}

	internal int GetConstantPoolConstantInteger(int index)
	{
		return ((ConstantPoolItemInteger)constantpool[index]).Value;
	}

	internal long GetConstantPoolConstantLong(int index)
	{
		return ((ConstantPoolItemLong)constantpool[index]).Value;
	}

	internal string GetConstantPoolConstantString(int index)
	{
		return ((ConstantPoolItemString)constantpool[index]).Value;
	}

	internal string Name
	{
		get
		{
			return this_cpi.Name;
		}
	}

	internal string SuperClass
	{
		get
		{
			return super_cpi.Name;
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

	internal ConstantPoolItemClass[] Interfaces
	{
		get
		{
			return interfaces;
		}
	}

	internal string SourceFileAttribute
	{
		get
		{
			return sourceFile;
		}
	}

	internal string IKVMAssemblyAttribute
	{
		get
		{
			return ikvmAssembly;
		}
	}

	internal bool DeprecatedAttribute
	{
		get
		{
			return deprecated;
		}
	}

	internal struct InnerClass
	{
		internal ushort innerClass;		// ConstantPoolItemClass
		internal ushort outerClass;		// ConstantPoolItemClass
		internal ushort name;			// ConstantPoolItemUtf8
		internal Modifiers accessFlags;
	}

	internal InnerClass[] InnerClasses
	{
		get
		{
			return innerClasses;
		}
	}

	internal enum ConstantType
	{
		Integer,
		Long,
		Float,
		Double,
		String,
		Class
	}

	internal abstract class ConstantPoolItem
	{
		internal virtual void Resolve(ClassFile classFile)
		{
		}

		internal virtual void Link(TypeWrapper thisType, Hashtable classCache)
		{
		}

		internal virtual ConstantType GetConstantType()
		{
			throw new InvalidOperationException();
		}
	}

	internal class ConstantPoolItemClass : ConstantPoolItem
	{
		private ushort name_index;
		private string name;
		private TypeWrapper typeWrapper;

		internal ConstantPoolItemClass(BigEndianBinaryReader br)
		{
			name_index = br.ReadUInt16();
		}

		internal override void Resolve(ClassFile classFile)
		{
			name = classFile.GetConstantPoolUtf8String(name_index).Replace('/', '.');
		}

		internal override void Link(TypeWrapper thisType, Hashtable classCache)
		{
			if(typeWrapper == null)
			{
				typeWrapper = LoadClassHelper(thisType.GetClassLoader(), classCache, name);
			}
		}

		internal string Name
		{
			get
			{
				return name;
			}
		}

		internal TypeWrapper GetClassType()
		{
			return typeWrapper;
		}

		internal override ConstantType GetConstantType()
		{
			return ConstantType.Class;
		}
	}

	private static TypeWrapper LoadClassHelper(ClassLoaderWrapper classLoader, Hashtable classCache, string name)
	{
		try
		{
			TypeWrapper wrapper = (TypeWrapper)classCache[name];
			if(wrapper != null)
			{
				return wrapper;
			}
			wrapper = classLoader.LoadClassByDottedNameFast(name);
			if(wrapper == null)
			{
				Tracer.Error(Tracer.ClassLoading, "Class not found: {0}", name);
				wrapper = new UnloadableTypeWrapper(name);
			}
			return wrapper;
		}
		catch(Exception x)
		{
			// TODO it might not be a good idea to catch .NET system exceptions here
			if(Tracer.ClassLoading.TraceError)
			{
				object cl = classLoader.GetJavaClassLoader();
				if(cl != null)
				{
					System.Text.StringBuilder sb = new System.Text.StringBuilder();
					Type type = cl.GetType();
					while(type.FullName != "java.lang.ClassLoader")
					{
						type = type.BaseType;
					}
					System.Reflection.FieldInfo parentField = type.GetField("parent", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
					if(parentField != null)
					{
						string sep = "";
						while(cl != null)
						{
							sb.Append(sep).Append(cl);
							sep = " -> ";
							cl = parentField.GetValue(cl);
						}
					}
					Tracer.Error(Tracer.ClassLoading, "ClassLoader chain: {0}", sb);
				}
				x = IKVM.Runtime.Util.MapException(x);
				Tracer.Error(Tracer.ClassLoading, x.ToString() + Environment.NewLine + x.StackTrace);
			}
			return new UnloadableTypeWrapper(name);
		}
	}

	private static TypeWrapper SigDecoderWrapper(ClassLoaderWrapper classLoader, Hashtable classCache, ref int index, string sig)
	{
		switch(sig[index++])
		{
			case 'B':
				return PrimitiveTypeWrapper.BYTE;
			case 'C':
				return PrimitiveTypeWrapper.CHAR;
			case 'D':
				return PrimitiveTypeWrapper.DOUBLE;
			case 'F':
				return PrimitiveTypeWrapper.FLOAT;
			case 'I':
				return PrimitiveTypeWrapper.INT;
			case 'J':
				return PrimitiveTypeWrapper.LONG;
			case 'L':
			{
				int pos = index;
				index = sig.IndexOf(';', index) + 1;
				return LoadClassHelper(classLoader, classCache, sig.Substring(pos, index - pos - 1));
			}
			case 'S':
				return PrimitiveTypeWrapper.SHORT;
			case 'Z':
				return PrimitiveTypeWrapper.BOOLEAN;
			case 'V':
				return PrimitiveTypeWrapper.VOID;
			case '[':
			{
				// TODO this can be optimized
				string array = "[";
				while(sig[index] == '[')
				{
					index++;
					array += "[";
				}
				switch(sig[index])
				{
					case 'L':
					{
						int pos = index;
						index = sig.IndexOf(';', index) + 1;
						return LoadClassHelper(classLoader, classCache, array + sig.Substring(pos, index - pos));
					}
					case 'B':
					case 'C':
					case 'D':
					case 'F':
					case 'I':
					case 'J':
					case 'S':
					case 'Z':
						return LoadClassHelper(classLoader, classCache, array + sig[index++]);
					default:
						// TODO this should never happen, because ClassFile should validate the descriptors
						throw new InvalidOperationException(sig.Substring(index));
				}
			}
			default:
				// TODO this should never happen, because ClassFile should validate the descriptors
				throw new InvalidOperationException(sig.Substring(index));
		}
	}

	internal static TypeWrapper[] ArgTypeWrapperListFromSig(ClassLoaderWrapper classLoader, Hashtable classCache, string sig)
	{
		if(sig[1] == ')')
		{
			return TypeWrapper.EmptyArray;
		}
		ArrayList list = new ArrayList();
		for(int i = 1; sig[i] != ')';)
		{
			list.Add(SigDecoderWrapper(classLoader, classCache, ref i, sig));
		}
		TypeWrapper[] types = new TypeWrapper[list.Count];
		list.CopyTo(types);
		return types;
	}

	internal static TypeWrapper FieldTypeWrapperFromSig(ClassLoaderWrapper classLoader, Hashtable classCache, string sig)
	{
		int index = 0;
		return SigDecoderWrapper(classLoader, classCache, ref index, sig);
	}

	internal static TypeWrapper RetTypeWrapperFromSig(ClassLoaderWrapper classLoader, Hashtable classCache, string sig)
	{
		int index = sig.IndexOf(')') + 1;
		return SigDecoderWrapper(classLoader, classCache, ref index, sig);
	}

	private class ConstantPoolItemDouble : ConstantPoolItem
	{
		private double d;

		internal ConstantPoolItemDouble(BigEndianBinaryReader br)
		{
			d = br.ReadDouble();
		}

		internal override ConstantType GetConstantType()
		{
			return ConstantType.Double;
		}

		internal double Value
		{
			get
			{
				return d;
			}
		}
	}

	internal class ConstantPoolItemFMI : ConstantPoolItem
	{
		private ushort class_index;
		private ushort name_and_type_index;
		protected ConstantPoolItemNameAndType name_and_type;
		private ConstantPoolItemClass clazz;

		internal ConstantPoolItemFMI(BigEndianBinaryReader br)
		{
			class_index = br.ReadUInt16();
			name_and_type_index = br.ReadUInt16();
		}

		internal override void Resolve(ClassFile classFile)
		{
			name_and_type = (ConstantPoolItemNameAndType)classFile.GetConstantPoolItem(name_and_type_index);
			clazz = (ConstantPoolItemClass)classFile.GetConstantPoolItem(class_index);
		}

		internal override void Link(TypeWrapper thisType, Hashtable classCache)
		{
			name_and_type.Link(thisType, classCache);
			clazz.Link(thisType, classCache);
		}

		internal string Name
		{
			get
			{
				return name_and_type.Name;
			}
		}

		internal string Signature
		{
			get
			{
				return name_and_type.Type;
			}
		}

		internal string Class
		{
			get
			{
				return clazz.Name;
			}
		}

		internal TypeWrapper GetClassType()
		{
			return clazz.GetClassType();
		}
	}

	internal class ConstantPoolItemFieldref : ConstantPoolItemFMI
	{
		private FieldWrapper field;

		internal ConstantPoolItemFieldref(BigEndianBinaryReader br) : base(br)
		{
		}

		internal TypeWrapper GetFieldType()
		{
			return name_and_type.GetFieldType();
		}

		internal override void Link(TypeWrapper thisType, Hashtable classCache)
		{
			base.Link(thisType, classCache);
			TypeWrapper wrapper = GetClassType();
			if(!wrapper.IsUnloadable)
			{
				field = wrapper.GetFieldWrapper(Name, Signature);
				if(field != null)
				{
					field.Link();
				}
			}
		}

		internal FieldWrapper GetField()
		{
			return field;
		}
	}

	internal class ConstantPoolItemMI : ConstantPoolItemFMI
	{
		protected MethodWrapper method;
		protected MethodWrapper invokespecialMethod;

		internal ConstantPoolItemMI(BigEndianBinaryReader br) : base(br)
		{
		}

		internal TypeWrapper[] GetArgTypes()
		{
			return name_and_type.GetArgTypes();
		}

		internal TypeWrapper GetRetType()
		{
			return name_and_type.GetRetType();
		}

		internal MethodWrapper GetMethod()
		{
			return method;
		}

		internal MethodWrapper GetMethodForInvokespecial()
		{
			return invokespecialMethod != null ? invokespecialMethod : method;
		}
	}

	internal class ConstantPoolItemMethodref : ConstantPoolItemMI
	{
		internal ConstantPoolItemMethodref(BigEndianBinaryReader br) : base(br)
		{
		}

		internal override void Link(TypeWrapper thisType, Hashtable classCache)
		{
			base.Link(thisType, classCache);
			TypeWrapper wrapper = GetClassType();
			if(!wrapper.IsUnloadable)
			{
				MethodDescriptor md = new MethodDescriptor(Name, Signature);
				method = wrapper.GetMethodWrapper(md, Name != "<init>");
				if(method != null)
				{
					method.Link();
				}
				if(Name != "<init>" && 
					(thisType.Modifiers & (__Modifiers.Interface | __Modifiers.Super)) == __Modifiers.Super &&
					thisType != wrapper && thisType.IsSubTypeOf(wrapper))
				{
					invokespecialMethod = thisType.BaseTypeWrapper.GetMethodWrapper(md, true);
					if(invokespecialMethod != null)
					{
						invokespecialMethod.Link();
					}
				}
			}
		}
	}

	internal class ConstantPoolItemInterfaceMethodref : ConstantPoolItemMI
	{
		internal ConstantPoolItemInterfaceMethodref(BigEndianBinaryReader br) : base(br)
		{
		}

		private static MethodWrapper GetInterfaceMethod(TypeWrapper wrapper, MethodDescriptor md)
		{
			MethodWrapper method = wrapper.GetMethodWrapper(md, false);
			if(method != null)
			{
				return method;
			}
			TypeWrapper[] interfaces = wrapper.Interfaces;
			for(int i = 0; i < interfaces.Length; i++)
			{
				method = GetInterfaceMethod(interfaces[i], md);
				if(method != null)
				{
					return method;
				}
			}
			return null;
		}

		internal override void Link(TypeWrapper thisType, Hashtable classCache)
		{
			base.Link(thisType, classCache);
			TypeWrapper wrapper = GetClassType();
			if(!wrapper.IsUnloadable)
			{
				MethodDescriptor md = new MethodDescriptor(Name, Signature);
				method = GetInterfaceMethod(wrapper, md);
				if(method == null)
				{
					// NOTE vmspec 5.4.3.4 clearly states that an interfacemethod may also refer to a method in Object
					method = CoreClasses.java.lang.Object.Wrapper.GetMethodWrapper(md, false);
				}
				if(method != null)
				{
					method.Link();
				}
			}
		}
	}

	private class ConstantPoolItemFloat : ConstantPoolItem
	{
		private float v;

		internal ConstantPoolItemFloat(BigEndianBinaryReader br)
		{
			v = br.ReadSingle();
		}

		internal override ConstantType GetConstantType()
		{
			return ConstantType.Float;
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

		internal override ConstantType GetConstantType()
		{
			return ConstantType.Integer;
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

		internal override ConstantType GetConstantType()
		{
			return ConstantType.Long;
		}

		internal long Value
		{
			get
			{
				return l;
			}
		}
	}

	internal class ConstantPoolItemNameAndType : ConstantPoolItem
	{
		private ushort name_index;
		private ushort descriptor_index;
		private string name;
		private string descriptor;
		private TypeWrapper[] argTypeWrappers;
		private TypeWrapper retTypeWrapper;
		private TypeWrapper fieldTypeWrapper;

		internal ConstantPoolItemNameAndType(BigEndianBinaryReader br)
		{
			name_index = br.ReadUInt16();
			descriptor_index = br.ReadUInt16();
		}

		internal override void Resolve(ClassFile classFile)
		{
			name = classFile.GetConstantPoolUtf8String(name_index).Replace('/', '.');
			descriptor = classFile.GetConstantPoolUtf8String(descriptor_index).Replace('/', '.');
		}

		internal override void Link(TypeWrapper thisType, Hashtable classCache)
		{
			if(descriptor[0] == '(')
			{
				if(argTypeWrappers == null)
				{
					ClassLoaderWrapper classLoader = thisType.GetClassLoader();
					argTypeWrappers = ArgTypeWrapperListFromSig(classLoader, classCache, descriptor);
					retTypeWrapper = RetTypeWrapperFromSig(classLoader, classCache, descriptor);
				}
			}
			else
			{
				if(fieldTypeWrapper == null)
				{
					ClassLoaderWrapper classLoader = thisType.GetClassLoader();
					fieldTypeWrapper = FieldTypeWrapperFromSig(classLoader, classCache, descriptor);
				}
			}
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

		internal TypeWrapper[] GetArgTypes()
		{
			return argTypeWrappers;
		}

		internal TypeWrapper GetRetType()
		{
			return retTypeWrapper;
		}

		internal TypeWrapper GetFieldType()
		{
			return fieldTypeWrapper;
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
			s = classFile.GetConstantPoolUtf8String(string_index);
		}

		internal override ConstantType GetConstantType()
		{
			return ConstantType.String;
		}

		internal string Value
		{
			get
			{
				return s;
			}
		}
	}

	internal enum Constant
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

	internal class FieldOrMethod
	{
		protected Modifiers access_flags;
		private string name;
		private string descriptor;
		protected bool deprecated;

		internal FieldOrMethod(ClassFile classFile, BigEndianBinaryReader br)
		{
			access_flags = (Modifiers)br.ReadUInt16();
			name = classFile.GetConstantPoolUtf8String(br.ReadUInt16());
			// TODO validate the descriptor
			descriptor = classFile.GetConstantPoolUtf8String(br.ReadUInt16()).Replace('/', '.');
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
				return descriptor;
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

		internal bool DeprecatedAttribute
		{
			get
			{
				return deprecated;
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
				throw new ClassFormatError("{0} (Illegal field modifiers: 0x{1:X})", classFile.Name, access_flags);
			}
			int attributes_count = br.ReadUInt16();
			for(int i = 0; i < attributes_count; i++)
			{
				switch(classFile.GetConstantPoolUtf8String(br.ReadUInt16()))
				{
					case "Deprecated":
						deprecated = true;
						if(br.ReadUInt32() != 0)
						{
							throw new ClassFormatError("Deprecated attribute has non-zero length");
						}
						break;
					case "ConstantValue":
					{
						if(br.ReadUInt32() != 2)
						{
							throw new ClassFormatError("Invalid ConstantValue attribute length");
						}
						ushort index = br.ReadUInt16();
						try
						{
							switch(Signature)
							{
								case "I":
									constantValue = classFile.GetConstantPoolConstantInteger(index);
									break;
								case "S":
									constantValue = (short)classFile.GetConstantPoolConstantInteger(index);
									break;
								case "B":
									constantValue = (sbyte)classFile.GetConstantPoolConstantInteger(index);
									break;
								case "C":
									constantValue = (char)classFile.GetConstantPoolConstantInteger(index);
									break;
								case "Z":
									constantValue = classFile.GetConstantPoolConstantInteger(index) != 0;
									break;
								case "J":
									constantValue = classFile.GetConstantPoolConstantLong(index);
									break;
								case "F":
									constantValue = classFile.GetConstantPoolConstantFloat(index);
									break;
								case "D":
									constantValue = classFile.GetConstantPoolConstantDouble(index);
									break;
								case "Ljava.lang.String;":
									constantValue = classFile.GetConstantPoolConstantString(index);
									break;
								default:
									throw new ClassFormatError("{0} (Invalid signature for constant)", classFile.Name);
							}
						}
						catch(InvalidCastException)
						{
							throw new ClassFormatError("{0} (Bad index into constant pool)", classFile.Name);
						}
						catch(IndexOutOfRangeException)
						{
							throw new ClassFormatError("{0} (Bad index into constant pool)", classFile.Name);
						}
						catch(InvalidOperationException)
						{
							throw new ClassFormatError("{0} (Bad index into constant pool)", classFile.Name);
						}
						catch(NullReferenceException)
						{
							throw new ClassFormatError("{0} (Bad index into constant pool)", classFile.Name);
						}
						break;
					}
					default:
						br.Skip(br.ReadUInt32());
						break;
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
		private string[] exceptions;

		internal Method(ClassFile classFile, BigEndianBinaryReader br) : base(classFile, br)
		{
			// vmspec 4.6 says that all flags, except ACC_STRICT are ignored on <clinit>
			if(Name == "<clinit>" && Signature == "()V")
			{
				access_flags &= Modifiers.Strictfp;
				access_flags |= (Modifiers.Static | Modifiers.Private);
			}
			else
			{
				// LAMESPEC: vmspec 4.6 says that abstract methods can not be strictfp (and this makes sense), but
				// javac (pre 1.5) is broken and marks abstract methods as strictfp (if you put the strictfp on the class)
				if((Name == "<init>" && (IsStatic || IsSynchronized || IsFinal || IsAbstract || IsNative))
					|| (IsPrivate && IsPublic) || (IsPrivate && IsProtected) || (IsPublic && IsProtected)
					|| (IsAbstract && (IsFinal || IsNative || IsPrivate || IsStatic || IsSynchronized))
					|| (classFile.IsInterface && (!IsPublic || !IsAbstract)))
				{
					throw new ClassFormatError("{0} (Illegal method modifiers: 0x{1:X})", classFile.Name, access_flags);
				}
			}
			int attributes_count = br.ReadUInt16();
			for(int i = 0; i < attributes_count; i++)
			{
				switch(classFile.GetConstantPoolUtf8String(br.ReadUInt16()))
				{
					case "Deprecated":
						deprecated = true;
						if(br.ReadUInt32() != 0)
						{
							throw new ClassFormatError("Deprecated attribute has non-zero length");
						}
						break;
					case "Code":
						if(!code.IsEmpty)
						{
							throw new ClassFormatError("Duplicate Code attribute");
						}
						code.Read(classFile, this, br.Section(br.ReadUInt32()));
						break;
					case "Exceptions":
						if(exceptions != null)
						{
							throw new ClassFormatError("Duplicate Exceptions attribute");
						}
						BigEndianBinaryReader rdr = br.Section(br.ReadUInt32());
						ushort count = rdr.ReadUInt16();
						exceptions = new string[count];
						for(int j = 0; j < count; j++)
						{
							exceptions[j] = classFile.GetConstantPoolClass(rdr.ReadUInt16());
						}
						break;
					default:
						br.Skip(br.ReadUInt32());
						break;
				}
			}
			if(IsAbstract || IsNative)
			{
				if(!code.IsEmpty)
				{
					throw new ClassFormatError("Abstract or native method cannot have a Code attribute");
				}
			}
			else
			{
				if(code.IsEmpty)
				{
					throw new ClassFormatError("Method has no Code attribute");
				}
			}
		}

		internal bool IsStrictfp
		{
			get
			{
				return (access_flags & Modifiers.Strictfp) != 0;
			}
		}

		// Is this the <clinit>()V method?
		internal bool IsClassInitializer
		{
			get
			{
				return Name == "<clinit>" && Signature == "()V";
			}
		}

		internal string[] ExceptionsAttribute
		{
			get
			{
				return exceptions;
			}
		}

		// maps argument 'slot' (as encoded in the xload/xstore instructions) into the ordinal
		internal int[] ArgMap
		{
			get
			{
				return code.argmap;
			}
		}

		internal int MaxStack
		{
			get
			{
				return code.max_stack;
			}
		}

		internal int MaxLocals
		{
			get
			{
				return code.max_locals;
			}
		}

		internal Instruction[] Instructions
		{
			get
			{
				return code.instructions;
			}
		}

		// maps a PC to an index in the Instruction[], invalid PCs return -1
		internal int[] PcIndexMap
		{
			get
			{
				return code.pcIndexMap;
			}
		}

		internal ExceptionTableEntry[] ExceptionTable
		{
			get
			{
				return code.exception_table;
			}
		}

		internal LineNumberTableEntry[] LineNumberTableAttribute
		{
			get
			{
				return code.lineNumberTable;
			}
		}

		internal LocalVariableTableEntry[] LocalVariableTableAttribute
		{
			get
			{
				return code.localVariableTable;
			}
		}

		private struct Code
		{
			internal ushort max_stack;
			internal ushort max_locals;
			internal Instruction[] instructions;
			internal int[] pcIndexMap;
			internal ExceptionTableEntry[] exception_table;
			internal int[] argmap;
			internal LineNumberTableEntry[] lineNumberTable;
			internal LocalVariableTableEntry[] localVariableTable;

			internal void Read(ClassFile classFile, Method method, BigEndianBinaryReader br)
			{
				max_stack = br.ReadUInt16();
				max_locals = br.ReadUInt16();
				uint code_length = br.ReadUInt32();
				Instruction[] instructions = new Instruction[code_length + 1];
				int basePosition = br.Position;
				int instructionIndex = 0;
				while(br.Position - basePosition < code_length)
				{
					instructions[instructionIndex++].Read((ushort)(br.Position - basePosition), br);
				}
				// we add an additional nop instruction to make it easier for consumers of the code array
				instructions[instructionIndex++].SetTermNop((ushort)(br.Position - basePosition));
				this.instructions = new Instruction[instructionIndex];
				Array.Copy(instructions, 0, this.instructions, 0, instructionIndex);
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
				for(int i = 0; i < attributes_count; i++)
				{
					switch(classFile.GetConstantPoolUtf8String(br.ReadUInt16()))
					{
						case "LineNumberTable":
							if(JVM.NoStackTraceInfo)
							{
								br.Skip(br.ReadUInt32());
							}
							else
							{
								BigEndianBinaryReader rdr = br.Section(br.ReadUInt32());
								int count = rdr.ReadUInt16();
								lineNumberTable = new LineNumberTableEntry[count];
								for(int j = 0; j < count; j++)
								{
									lineNumberTable[j].start_pc = rdr.ReadUInt16();
									lineNumberTable[j].line_number = rdr.ReadUInt16();
								}
							}
							break;
						case "LocalVariableTable":
							if(JVM.Debug || JVM.IsStaticCompiler)
							{
								BigEndianBinaryReader rdr = br.Section(br.ReadUInt32());
								int count = rdr.ReadUInt16();
								localVariableTable = new LocalVariableTableEntry[count];
								for(int j = 0; j < count; j++)
								{
									localVariableTable[j].start_pc = rdr.ReadUInt16();
									localVariableTable[j].length = rdr.ReadUInt16();
									localVariableTable[j].name = classFile.GetConstantPoolUtf8String(rdr.ReadUInt16());
									localVariableTable[j].descriptor = classFile.GetConstantPoolUtf8String(rdr.ReadUInt16()).Replace('/', '.');
									localVariableTable[j].index = rdr.ReadUInt16();
								}
							}
							else
							{
								br.Skip(br.ReadUInt32());
							}
							break;
						default:
							br.Skip(br.ReadUInt32());
							break;
					}
				}
				// build the pcIndexMap
				pcIndexMap = new int[this.instructions[instructionIndex - 1].PC + 1];
				for(int i = 0; i < pcIndexMap.Length; i++)
				{
					pcIndexMap[i] = -1;
				}
				for(int i = 0; i < instructionIndex - 1; i++)
				{
					pcIndexMap[this.instructions[i].PC] = i;
				}
				// build the argmap
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

			internal bool IsEmpty
			{
				get
				{
					return instructions == null;
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

		internal struct Instruction
		{
			private ushort pc;
			private ByteCode opcode;
			private NormalizedByteCode normopcode;
			private int arg1;
			private short arg2;
			private SwitchEntry[] switch_entries;

			struct SwitchEntry
			{
				internal int value;
				internal int target_offset;
			}

			internal void SetTermNop(ushort pc)
			{
				// TODO what happens if we already have exactly the maximum number of instructions?
				this.pc = pc;
				this.opcode = ByteCode.__nop;
			}

			internal void Read(ushort pc, BigEndianBinaryReader br)
			{
				this.pc = pc;
				ByteCode bc = (ByteCode)br.ReadByte();
				switch(ByteCodeMetaData.GetMode(bc))
				{
					case ByteCodeMode.Simple:
						break;
					case ByteCodeMode.Constant_1:
					case ByteCodeMode.Local_1:
						arg1 = br.ReadByte();
						break;
					case ByteCodeMode.Constant_2:
						arg1 = br.ReadUInt16();
						break;
					case ByteCodeMode.Branch_2:
						arg1 = br.ReadInt16();
						break;
					case ByteCodeMode.Branch_4:
						arg1 = br.ReadInt32();
						break;
					case ByteCodeMode.Constant_2_1_1:
						arg1 = br.ReadUInt16();
						// TODO validate these
						br.ReadByte();	// count
						br.ReadByte();	// unused
						break;
					case ByteCodeMode.Immediate_1:
						arg1 = br.ReadSByte();
						break;
					case ByteCodeMode.Immediate_2:
						arg1 = br.ReadInt16();
						break;
					case ByteCodeMode.Local_1_Immediate_1:
						arg1 = br.ReadByte();
						arg2 = br.ReadSByte();
						break;
					case ByteCodeMode.Constant_2_Immediate_1:
						arg1 = br.ReadUInt16();
						arg2 = br.ReadSByte();
						break;
					case ByteCodeMode.Tableswitch:
					{
						// skip the padding
						uint p = pc + 1u;
						uint align = ((p + 3) & 0x7ffffffc) - p;
						br.Skip(align);
						int default_offset = br.ReadInt32();
						this.arg1 = default_offset;
						int low = br.ReadInt32();
						int high = br.ReadInt32();
						if(low > high)
						{
							// TODO is this the right exception?
							throw new ClassFormatError("Incorrect tableswitch (low > high)");
						}
						SwitchEntry[] entries = new SwitchEntry[high - low + 1];
						for(int i = low; i <= high; i++)
						{
							entries[i - low].value = i;
							entries[i - low].target_offset = br.ReadInt32();
						}
						this.switch_entries = entries;
						break;
					}
					case ByteCodeMode.Lookupswitch:
					{
						// skip the padding
						uint p = pc + 1u;
						uint align = ((p + 3) & 0x7ffffffc) - p;
						br.Skip(align);
						int default_offset = br.ReadInt32();
						this.arg1 = default_offset;
						int count = br.ReadInt32();
						if(count < 0)
						{
							// TODO is this the right exception?
							throw new ClassFormatError("Incorrect lookupswitch (npairs < 0)");
						}
						SwitchEntry[] entries = new SwitchEntry[count];
						for(int i = 0; i < count; i++)
						{
							entries[i].value = br.ReadInt32();
							entries[i].target_offset = br.ReadInt32();
						}
						this.switch_entries = entries;
						break;
					}
					case ByteCodeMode.WidePrefix:
						bc = (ByteCode)br.ReadByte();
						// NOTE the PC of a wide instruction is actually the PC of the
						// wide prefix, not the following instruction (vmspec 4.9.2)
						switch(ByteCodeMetaData.GetWideMode(bc))
						{
							case ByteCodeModeWide.Local_2:
								arg1 = br.ReadUInt16();
								break;
							case ByteCodeModeWide.Local_2_Immediate_2:
								arg1 = br.ReadUInt16();
								arg2 = br.ReadInt16();
								break;
							default:
								throw new ClassFormatError("Invalid wide prefix on opcode: {0}", bc);
						}
						break;
					default:
						throw new ClassFormatError("Invalid opcode: {0}", bc);
				}
				this.opcode = bc;
				this.normopcode = ByteCodeMetaData.GetNormalizedByteCode(bc);
				arg1 = ByteCodeMetaData.GetArg(opcode, arg1);
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
					return normopcode;
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
					return arg1;
				}
			}

			internal int DefaultOffset
			{
				get
				{
					return arg1;
				}
			}

			internal int SwitchEntryCount
			{
				get
				{
					return switch_entries.Length;
				}
			}

			internal int GetSwitchValue(int i)
			{
				return switch_entries[i].value;
			}

			internal int GetSwitchTargetOffset(int i)
			{
				return switch_entries[i].target_offset;
			}
		}

		internal struct LineNumberTableEntry
		{
			internal ushort start_pc;
			internal ushort line_number;
		}

		internal struct LocalVariableTableEntry
		{
			internal ushort start_pc;
			internal ushort length;
			internal string name;
			internal string descriptor;
			internal ushort index;
		}
	}
}
