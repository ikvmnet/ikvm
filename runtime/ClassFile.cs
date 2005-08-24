/*
  Copyright (C) 2002, 2003, 2004, 2005 Jeroen Frijters

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

namespace IKVM.Internal
{
	enum HardError : short
	{
		NoClassDefFoundError,
		IllegalAccessError,
		InstantiationError,
		IncompatibleClassChangeError,
		NoSuchFieldError,
		AbstractMethodError,
		NoSuchMethodError,
		LinkageError
	}

	sealed class ClassFile
	{
		private ConstantPoolItem[] constantpool;
		private string[] utf8_cp;
		private Modifiers access_flags;
		private ushort this_class;
		private ushort super_class;
		private ConstantPoolItemClass[] interfaces;
		private Field[] fields;
		private Method[] methods;
		private string sourceFile;
		private ushort majorVersion;
		private bool deprecated;
		private string ikvmAssembly;
		private InnerClass[] innerClasses;

		private class SupportedVersions
		{
			internal static readonly int Minimum = 45;
			internal static readonly int Maximum = JVM.SafeGetEnvironmentVariable("IKVM_EXPERIMENTAL_JDK_5_0") == null ? 48 : 49;
		}

		// This method parses just enough of the class file to obtain its name, it doesn't
		// validate the class file structure, but it may throw a ClassFormatError if it
		// encounters bogus data
		internal static string GetClassName(byte[] buf, int offset, int length)
		{
			const string inputClassName = "(unknown)";
			BigEndianBinaryReader br = new BigEndianBinaryReader(buf, offset, length);
			if(br.ReadUInt32() != 0xCAFEBABE)
			{
				throw new ClassFormatError("{0} (Bad magic number)", inputClassName);
			}
			int minorVersion = br.ReadUInt16();
			int majorVersion = br.ReadUInt16();
			if(majorVersion < SupportedVersions.Minimum || majorVersion > SupportedVersions.Maximum)
			{
				throw new UnsupportedClassVersionError(inputClassName + " (" + majorVersion + "." + minorVersion + ")");
			}
			int constantpoolcount = br.ReadUInt16();
			int[] cpclass = new int[constantpoolcount];
			string[] utf8_cp = new string[constantpoolcount];
			for(int i = 1; i < constantpoolcount; i++)
			{
				Constant tag = (Constant)br.ReadByte();
				switch(tag)
				{
					case Constant.Class:
						cpclass[i] = br.ReadUInt16();
						break;
					case Constant.Double:
					case Constant.Long:
						br.Skip(8);
						i++;
						break;
					case Constant.Fieldref:
					case Constant.InterfaceMethodref:
					case Constant.Methodref:
					case Constant.NameAndType:
					case Constant.Float:
					case Constant.Integer:
						br.Skip(4);
						break;
					case Constant.String:
						br.Skip(2);
						break;
					case Constant.Utf8:
						utf8_cp[i] = br.ReadString(inputClassName);
						break;
					default:
						throw new ClassFormatError("{0} (Illegal constant pool type 0x{1:X})", inputClassName, tag);
				}
			}
			br.ReadUInt16(); // access_flags
			try
			{
				return String.Intern(utf8_cp[cpclass[br.ReadUInt16()]].Replace('/', '.'));
			}
			catch(Exception x)
			{
				throw new ClassFormatError("{0}: {1}", x.GetType().Name, x.Message);
			}
		}

		internal ClassFile(byte[] buf, int offset, int length, string inputClassName)
		{
			try
			{
				BigEndianBinaryReader br = new BigEndianBinaryReader(buf, offset, length);
				if(br.ReadUInt32() != 0xCAFEBABE)
				{
					throw new ClassFormatError("{0} (Bad magic number)", inputClassName);
				}
				int minorVersion = br.ReadUInt16();
				majorVersion = br.ReadUInt16();
				if(majorVersion < SupportedVersions.Minimum || majorVersion > SupportedVersions.Maximum)
				{
					throw new UnsupportedClassVersionError(inputClassName + " (" + majorVersion + "." + minorVersion + ")");
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
							utf8_cp[i] = br.ReadString(inputClassName);
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
						catch(ClassFormatError x)
						{
							// HACK at this point we don't yet have the class name, so any exceptions throw
							// are missing the class name
							throw new ClassFormatError("{0} ({1})", inputClassName, x.Message);
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
				this_class = br.ReadUInt16();
				ValidateConstantPoolItemClass(inputClassName, this_class);
				super_class = br.ReadUInt16();
				ValidateConstantPoolItemClass(inputClassName, super_class);
				if(IsInterface && (super_class == 0 || this.SuperClass != "java.lang.Object"))
				{
					throw new ClassFormatError("{0} (Interfaces must have java.lang.Object as superclass)", Name);
				}
				// most checks are already done by ConstantPoolItemClass.Resolve, but since it allows
				// array types, we do need to check for that
				if(this.Name[0] == '[')
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
						if(ReferenceEquals(interfaces[j].Name, cpi.Name))
						{
							throw new ClassFormatError("{0} (Repetitive interface name)", Name);
						}
					}
				}
				int fields_count = br.ReadUInt16();
				fields = new Field[fields_count];
				for(int i = 0; i < fields_count; i++)
				{
					fields[i] = new Field(this, br);
					string name = fields[i].Name;
					if(!IsValidIdentifier(name))
					{
						throw new ClassFormatError("{0} (Illegal field name \"{1}\")", Name, name);
					}
					for(int j = 0; j < i; j++)
					{
						if(ReferenceEquals(fields[j].Name, name) && ReferenceEquals(fields[j].Signature, fields[i].Signature))
						{
							throw new ClassFormatError("{0} (Repetitive field name/signature)", Name);
						}
					}
				}
				int methods_count = br.ReadUInt16();
				methods = new Method[methods_count];
				for(int i = 0; i < methods_count; i++)
				{
					methods[i] = new Method(this, br);
					string name = methods[i].Name;
					string sig = methods[i].Signature;
					if(!IsValidIdentifier(name) && name != "<init>" && name != "<clinit>")
					{
						throw new ClassFormatError("{0} (Illegal method name \"{1}\")", Name, name);
					}
					if((name == "<init>" || name == "<clinit>") && !sig.EndsWith("V"))
					{
						throw new ClassFormatError("{0} (Method \"{1}\" has illegal signature \"{2}\")", Name, name, sig);
					}
					for(int j = 0; j < i; j++)
					{
						if(ReferenceEquals(methods[j].Name, name) && ReferenceEquals(methods[j].Signature, sig))
						{
							throw new ClassFormatError("{0} (Repetitive method name/signature)", Name);
						}
					}
				}
				int attributes_count = br.ReadUInt16();
				for(int i = 0; i < attributes_count; i++)
				{
					switch(GetConstantPoolUtf8String(br.ReadUInt16()))
					{
						case "Deprecated":
							deprecated = true;
							br.Skip(br.ReadUInt32());
							break;
						case "SourceFile":
							if(br.ReadUInt32() != 2)
							{
								throw new ClassFormatError("SourceFile attribute has incorrect length");
							}
							sourceFile = GetConstantPoolUtf8String(br.ReadUInt16());
							break;
						case "InnerClasses":
							// Sun totally ignores the length of InnerClasses attribute,
							// so when we're running Fuzz this used to show up as lots of differences,
							// now we do the same.
							BigEndianBinaryReader rdr = br;
							br.ReadUInt32();
							ushort count = rdr.ReadUInt16();
							innerClasses = new InnerClass[count];
							for(int j = 0; j < innerClasses.Length; j++)
							{
								innerClasses[j].innerClass = rdr.ReadUInt16();
								innerClasses[j].outerClass = rdr.ReadUInt16();
								innerClasses[j].name = rdr.ReadUInt16();
								innerClasses[j].accessFlags = (Modifiers)rdr.ReadUInt16();
								if(innerClasses[j].innerClass != 0 && !(GetConstantPoolItem(innerClasses[j].innerClass) is ConstantPoolItemClass))
								{
									throw new ClassFormatError("{0} (inner_class_info_index has bad constant pool index)", this.Name);
								}
								if(innerClasses[j].outerClass != 0 && !(GetConstantPoolItem(innerClasses[j].outerClass) is ConstantPoolItemClass))
								{
									throw new ClassFormatError("{0} (outer_class_info_index has bad constant pool index)", this.Name);
								}
								if(innerClasses[j].name != 0 && utf8_cp[innerClasses[j].name] == null)
								{
									throw new ClassFormatError("{0} (inner class name has bad constant pool index)", this.Name);
								}
								if(innerClasses[j].innerClass == innerClasses[j].outerClass)
								{
									throw new ClassFormatError("{0} (Class is both inner and outer class)", this.Name);
								}
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
				// now that we've constructed the high level objects, the utf8 table isn't needed anymore
				// TODO remove utf8_cp field from ClassFile object
				utf8_cp = null;
				if(br.Position != offset + length)
				{
					throw new ClassFormatError("Extra bytes at the end of the class file");
				}
			}
			catch(OverflowException)
			{
				throw new ClassFormatError("Truncated class file (or section)");
			}
			catch(IndexOutOfRangeException)
			{
				// TODO we should throw more specific errors
				throw new ClassFormatError("Unspecified class file format error");
			}
			//		catch(Exception x)
			//		{
			//			Console.WriteLine(x);
			//			FileStream fs = File.Create(inputClassName + ".broken");
			//			fs.Write(buf, offset, length);
			//			fs.Close();
			//			throw;
			//		}
		}

		private void ValidateConstantPoolItemClass(string classFile, ushort index)
		{
			if(index >= constantpool.Length || !(constantpool[index] is ConstantPoolItemClass))
			{
				throw new ClassFormatError("{0} (Bad constant pool index #{1})", classFile, index);
			}
		}

		private static bool IsValidIdentifier(string name)
		{
			if(name.Length == 0)
			{
				return false;
			}
			if(!Char.IsLetter(name[0]) && name[0] != '$' && name[0] != '_')
			{
				return false;
			}
			for(int i = 1; i < name.Length; i++)
			{
				if(!Char.IsLetterOrDigit(name[i]) && name[i] != '$' && name[i] != '_')
				{
					return false;
				}
			}
			return true;
		}

		private static bool IsValidFieldSig(string sig)
		{
			return IsValidFieldSigImpl(sig, 0, sig.Length);
		}

		private static bool IsValidFieldSigImpl(string sig, int start, int end)
		{
			if(start >= end)
			{
				return false;
			}
			switch(sig[start])
			{
				case 'L':
					// TODO we might consider doing more checking here
					return sig.IndexOf(';', start + 1) == end - 1;
				case '[':
					while(sig[start] == '[')
					{
						start++;
						if(start == end)
						{
							return false;
						}
					}
					return IsValidFieldSigImpl(sig, start, end);
				case 'B':
				case 'Z':
				case 'C':
				case 'S':
				case 'I':
				case 'J':
				case 'F':
				case 'D':
					return start == end - 1;
				default:
					return false;
			}
		}

		private static bool IsValidMethodSig(string sig)
		{
			if(sig.Length < 3 || sig[0] != '(')
			{
				return false;
			}
			int end = sig.IndexOf(')');
			if(end == -1)
			{
				return false;
			}
			if(!sig.EndsWith(")V") && !IsValidFieldSigImpl(sig, end + 1, sig.Length))
			{
				return false;
			}
			for(int i = 1; i < end; i++)
			{
				switch(sig[i])
				{
					case 'B':
					case 'Z':
					case 'C':
					case 'S':
					case 'I':
					case 'J':
					case 'F':
					case 'D':
						break;
					case 'L':
						i = sig.IndexOf(';', i);
						break;
					case '[':
						while(sig[i] == '[')
						{
							i++;
						}
						if("BZCSIJFDL".IndexOf(sig[i]) == -1)
						{
							return false;
						}
						if(sig[i] == 'L')
						{
							i = sig.IndexOf(';', i);
						}
						break;
					default:
						return false;
				}
				if(i == -1 || i >= end)
				{
					return false;
				}
			}
			return true;
		}

		internal int MajorVersion
		{
			get
			{
				return majorVersion;
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

		internal void RemoveUnusedFields()
		{
			ArrayList list = new ArrayList();
			foreach(Field f in fields)
			{
				if(f.IsPrivate && f.IsStatic && f.Name != "serialVersionUID" && !IsReferenced(f))
				{
					// unused, so we skip it
					Tracer.Info(Tracer.Compiler, "Unused field {0}::{1}", this.Name, f.Name);
				}
				else
				{
					list.Add(f);
				}
			}
			fields = (Field[])list.ToArray(typeof(Field));
		}

		private bool IsReferenced(Field fld)
		{
			foreach(ConstantPoolItem cpi in constantpool)
			{
				ConstantPoolItemFieldref fieldref = cpi as ConstantPoolItemFieldref;
				if(fieldref != null && 
					fieldref.Class == this.Name && 
					fieldref.Name == fld.Name && 
					fieldref.Signature == fld.Signature)
				{
					return true;
				}
			}
			return false;
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

		internal string GetConstantPoolUtf8String(int index)
		{
			string s = utf8_cp[index];
			if(s == null)
			{
				if(this_class == 0)
				{
					throw new ClassFormatError("Bad constant pool index #{0}", index);
				}
				else
				{
					throw new ClassFormatError("{0} (Bad constant pool index #{1})", this.Name, index);
				}
			}
			return s;
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
				return GetConstantPoolClass(this_class);
			}
		}

		internal string SuperClass
		{
			get
			{
				return GetConstantPoolClass(super_class);
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

		internal sealed class ConstantPoolItemClass : ConstantPoolItem
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
				name = classFile.GetConstantPoolUtf8String(name_index);
				if(name.Length > 0)
				{
					char prev = name[0];
					if(Char.IsLetter(prev) || prev == '$' || prev == '_' || prev == '[')
					{
						int skip = 1;
						int end = name.Length;
						if(prev == '[')
						{
							// TODO optimize this
							if(!IsValidFieldSig(name))
							{
								goto barf;
							}
							while(name[skip] == '[')
							{
								skip++;
							}
							if(name.EndsWith(";"))
							{
								end--;
							}
						}
						for(int i = skip; i < end; i++)
						{
							char c = name[i];
							if(!Char.IsLetterOrDigit(c) && c != '$' && c != '_' && (c != '/' || prev == '/'))
							{
								goto barf;
							}
							prev = c;
						}
						name = String.Intern(name.Replace('/', '.'));
						return;
					}
				}
				barf:
					throw new ClassFormatError("Invalid class name \"{0}\"", name);
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
			catch(RetargetableJavaException x)
			{
				if(!JVM.IsStaticCompiler && Tracer.ClassLoading.TraceError)
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
					Exception m = IKVM.Runtime.Util.MapException(x.ToJava());
					Tracer.Error(Tracer.ClassLoading, m.ToString() + Environment.NewLine + m.StackTrace);
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

		private sealed class ConstantPoolItemDouble : ConstantPoolItem
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

		internal abstract class ConstantPoolItemFMI : ConstantPoolItem
		{
			private ushort class_index;
			private ushort name_and_type_index;
			private ConstantPoolItemClass clazz;
			private string name;
			private string descriptor;

			internal ConstantPoolItemFMI(BigEndianBinaryReader br)
			{
				class_index = br.ReadUInt16();
				name_and_type_index = br.ReadUInt16();
			}

			internal override void Resolve(ClassFile classFile)
			{
				ConstantPoolItemNameAndType name_and_type = (ConstantPoolItemNameAndType)classFile.GetConstantPoolItem(name_and_type_index);
				clazz = (ConstantPoolItemClass)classFile.GetConstantPoolItem(class_index);
				// if the constant pool items referred to were strings, GetConstantPoolItem returns null
				if(name_and_type == null || clazz == null)
				{
					throw new ClassFormatError("Bad index in constant pool");
				}
				name = String.Intern(classFile.GetConstantPoolUtf8String(name_and_type.name_index));
				descriptor = classFile.GetConstantPoolUtf8String(name_and_type.descriptor_index);
				Validate(name, descriptor);
				descriptor = String.Intern(descriptor.Replace('/', '.'));
			}

			protected abstract void Validate(string name, string descriptor);

			internal override void Link(TypeWrapper thisType, Hashtable classCache)
			{
				clazz.Link(thisType, classCache);
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

		internal sealed class ConstantPoolItemFieldref : ConstantPoolItemFMI
		{
			private FieldWrapper field;
			private TypeWrapper fieldTypeWrapper;

			internal ConstantPoolItemFieldref(BigEndianBinaryReader br) : base(br)
			{
			}

			protected override void Validate(string name, string descriptor)
			{
				if(!IsValidFieldSig(descriptor))
				{
					throw new ClassFormatError("Invalid field signature \"{0}\"", descriptor);
				}
				if(!IsValidIdentifier(name))
				{
					throw new ClassFormatError("Invalid field name \"{0}\"", name);
				}
			}

			internal TypeWrapper GetFieldType()
			{
				return fieldTypeWrapper;
			}

			internal override void Link(TypeWrapper thisType, Hashtable classCache)
			{
				base.Link(thisType, classCache);
				if(fieldTypeWrapper == null)
				{
					ClassLoaderWrapper classLoader = thisType.GetClassLoader();
					fieldTypeWrapper = FieldTypeWrapperFromSig(classLoader, classCache, this.Signature);
					TypeWrapper wrapper = GetClassType();
					if(!wrapper.IsUnloadable)
					{
						field = wrapper.GetFieldWrapper(Name, Signature);
						if(field != null)
						{
							bool ok = false;
							try
							{
								field.Link();
								ok = true;
							}
							finally
							{
								if(!ok)
								{
									fieldTypeWrapper = null;
								}
							}
						}
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
			private TypeWrapper[] argTypeWrappers;
			private TypeWrapper retTypeWrapper;
			protected MethodWrapper method;
			protected MethodWrapper invokespecialMethod;

			internal ConstantPoolItemMI(BigEndianBinaryReader br) : base(br)
			{
			}

			protected override void Validate(string name, string descriptor)
			{
				if(!IsValidMethodSig(descriptor))
				{
					throw new ClassFormatError("Method {0} has invalid signature {1}", name, descriptor);
				}
				if(name == "<init>" || name == "<clinit>")
				{
					if(!descriptor.EndsWith("V"))
					{
						throw new ClassFormatError("Method {0} has invalid signature {1}", name, descriptor);
					}
				}
				else if(!IsValidIdentifier(name))
				{
					throw new ClassFormatError("Invalid method name \"{0}\"", name);
				}
			}

			internal override void Link(TypeWrapper thisType, Hashtable classCache)
			{
				base.Link(thisType, classCache);
				if(argTypeWrappers == null)
				{
					ClassLoaderWrapper classLoader = thisType.GetClassLoader();
					argTypeWrappers = ArgTypeWrapperListFromSig(classLoader, classCache, this.Signature);
					retTypeWrapper = RetTypeWrapperFromSig(classLoader, classCache, this.Signature);
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

			internal MethodWrapper GetMethod()
			{
				return method;
			}

			internal MethodWrapper GetMethodForInvokespecial()
			{
				return invokespecialMethod != null ? invokespecialMethod : method;
			}
		}

		internal sealed class ConstantPoolItemMethodref : ConstantPoolItemMI
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
					method = wrapper.GetMethodWrapper(Name, Signature, Name != "<init>");
					if(method != null)
					{
						method.Link();
					}
					if(Name != "<init>" && 
						(thisType.Modifiers & (Modifiers.Interface | Modifiers.Super)) == Modifiers.Super &&
						thisType != wrapper && thisType.IsSubTypeOf(wrapper))
					{
						invokespecialMethod = thisType.BaseTypeWrapper.GetMethodWrapper(Name, Signature, true);
						if(invokespecialMethod != null)
						{
							invokespecialMethod.Link();
						}
					}
				}
			}
		}

		internal sealed class ConstantPoolItemInterfaceMethodref : ConstantPoolItemMI
		{
			internal ConstantPoolItemInterfaceMethodref(BigEndianBinaryReader br) : base(br)
			{
			}

			private static MethodWrapper GetInterfaceMethod(TypeWrapper wrapper, string name, string sig)
			{
				MethodWrapper method = wrapper.GetMethodWrapper(name, sig, false);
				if(method != null)
				{
					return method;
				}
				TypeWrapper[] interfaces = wrapper.Interfaces;
				for(int i = 0; i < interfaces.Length; i++)
				{
					method = GetInterfaceMethod(interfaces[i], name, sig);
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
					method = GetInterfaceMethod(wrapper, Name, Signature);
					if(method == null)
					{
						// NOTE vmspec 5.4.3.4 clearly states that an interfacemethod may also refer to a method in Object
						method = CoreClasses.java.lang.Object.Wrapper.GetMethodWrapper(Name, Signature, false);
					}
					if(method != null)
					{
						method.Link();
					}
				}
			}
		}

		private sealed class ConstantPoolItemFloat : ConstantPoolItem
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

		private sealed class ConstantPoolItemInteger : ConstantPoolItem
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

		private sealed class ConstantPoolItemLong : ConstantPoolItem
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

		internal sealed class ConstantPoolItemNameAndType : ConstantPoolItem
		{
			internal ushort name_index;
			internal ushort descriptor_index;

			internal ConstantPoolItemNameAndType(BigEndianBinaryReader br)
			{
				name_index = br.ReadUInt16();
				descriptor_index = br.ReadUInt16();
			}
		}

		private sealed class ConstantPoolItemString : ConstantPoolItem
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

		internal abstract class FieldOrMethod
		{
			protected Modifiers access_flags;
			private string name;
			private string descriptor;
			protected bool deprecated;

			internal FieldOrMethod(ClassFile classFile, BigEndianBinaryReader br)
			{
				access_flags = (Modifiers)br.ReadUInt16();
				name = String.Intern(classFile.GetConstantPoolUtf8String(br.ReadUInt16()));
				descriptor = classFile.GetConstantPoolUtf8String(br.ReadUInt16());
				ValidateSig(classFile, descriptor);
				descriptor = String.Intern(descriptor.Replace('/', '.'));
			}

			protected abstract void ValidateSig(ClassFile classFile, string descriptor);

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

		internal sealed class Field : FieldOrMethod
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
							br.Skip(br.ReadUInt32());
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
										constantValue = (byte)classFile.GetConstantPoolConstantInteger(index);
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

			protected override void ValidateSig(ClassFile classFile, string descriptor)
			{
				if(!IsValidFieldSig(descriptor))
				{
					throw new ClassFormatError("{0} (Field \"{1}\" has invalid signature \"{2}\")", classFile.Name, this.Name, descriptor);
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

		internal sealed class Method : FieldOrMethod
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
							br.Skip(br.ReadUInt32());
							break;
						case "Code":
						{
							if(!code.IsEmpty)
							{
								throw new ClassFormatError("{0} (Duplicate Code attribute)", classFile.Name);
							}
							BigEndianBinaryReader rdr = br.Section(br.ReadUInt32());
							code.Read(classFile, this, rdr);
							if(!rdr.IsAtEnd)
							{
								throw new ClassFormatError("{0} (Code attribute has wrong length)", classFile.Name);
							}
							break;
						}
						case "Exceptions":
						{
							if(exceptions != null)
							{
								throw new ClassFormatError("{0} (Duplicate Exceptions attribute)", classFile.Name);
							}
							BigEndianBinaryReader rdr = br.Section(br.ReadUInt32());
							ushort count = rdr.ReadUInt16();
							exceptions = new string[count];
							for(int j = 0; j < count; j++)
							{
								exceptions[j] = classFile.GetConstantPoolClass(rdr.ReadUInt16());
							}
							if(!rdr.IsAtEnd)
							{
								throw new ClassFormatError("{0} (Exceptions attribute has wrong length)", classFile.Name);
							}
							break;
						}
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
						if(this.Name == "<clinit>")
						{
							code.verifyError = string.Format("Class {0}, method {1} signature {2}: No Code attribute", classFile.Name, this.Name, this.Signature);
							return;
						}
						throw new ClassFormatError("Method has no Code attribute");
					}
				}
			}

			protected override void ValidateSig(ClassFile classFile, string descriptor)
			{
				if(!IsValidMethodSig(descriptor))
				{
					throw new ClassFormatError("{0} (Method \"{1}\" has invalid signature \"{2}\")", classFile.Name, this.Name, descriptor);
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

			internal string VerifyError
			{
				get
				{
					return code.verifyError;
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
				internal string verifyError;
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
					if(code_length > 65536)
					{
						throw new ClassFormatError("{0} (Invalid Code length {1})", classFile.Name, code_length);
					}
					Instruction[] instructions = new Instruction[code_length + 1];
					int basePosition = br.Position;
					int instructionIndex = 0;
					try
					{
						BigEndianBinaryReader rdr = br.Section(code_length);
						while(!rdr.IsAtEnd)
						{
							instructions[instructionIndex++].Read((ushort)(rdr.Position - basePosition), rdr);
						}
						// we add an additional nop instruction to make it easier for consumers of the code array
						instructions[instructionIndex++].SetTermNop((ushort)(rdr.Position - basePosition));
					}
					catch(ClassFormatError x)
					{
						// any class format errors in the code block are actually verify errors
						verifyError = x.Message;
					}
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
										if(lineNumberTable[j].start_pc >= code_length)
										{
											throw new ClassFormatError("{0} (LineNumberTable has invalid pc)", classFile.Name);
										}
									}
									if(!rdr.IsAtEnd)
									{
										throw new ClassFormatError("{0} (LineNumberTable attribute has wrong length)", classFile.Name);
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
									// NOTE we're intentionally not checking that we're at the end of the section
									// (optional attributes shouldn't cause ClassFormatError)
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
					if(args.Count > max_locals)
					{
						throw new ClassFormatError("{0} (Arguments can't fit into locals)", classFile.Name);
					}
				}

				internal bool IsEmpty
				{
					get
					{
						return instructions == null;
					}
				}
			}

			internal sealed class ExceptionTableEntry
			{
				internal ushort start_pc;
				internal ushort end_pc;
				internal ushort handler_pc;
				internal ushort catch_type;
				internal int ordinal;
			}

			[Flags]
			internal enum InstructionFlags : byte
			{
				Reachable = 1,
				Processed = 2,
				BranchTarget = 4
			}

			internal struct Instruction
			{
				private ushort pc;
				private NormalizedByteCode normopcode;
				internal InstructionFlags flags;
				private int arg1;
				private short arg2;
				private SwitchEntry[] switch_entries;

				struct SwitchEntry
				{
					internal int value;
					internal int target_offset;
				}

				internal void SetHardError(HardError error, int messageId)
				{
					normopcode = NormalizedByteCode.__static_error;
					arg2 = (short)error;
					arg1 = messageId;
				}

				internal HardError HardError
				{
					get
					{
						return (HardError)arg2;
					}
				}

				internal int HardErrorMessageId
				{
					get
					{
						return arg1;
					}
				}

				internal bool IsReachable
				{
					get
					{
						return (flags & InstructionFlags.Reachable) != 0;
					}
				}

				internal void PatchOpCode(NormalizedByteCode bc)
				{
					this.normopcode = bc;
				}

				internal void SetTermNop(ushort pc)
				{
					// TODO what happens if we already have exactly the maximum number of instructions?
					this.pc = pc;
					this.normopcode = NormalizedByteCode.__nop;
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
							arg2 = br.ReadByte();
							if(br.ReadByte() != 0)
							{
								throw new ClassFormatError("invokeinterface filler must be zero");
							}
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
							if(low > high || high > 16384L + low)
							{
								throw new ClassFormatError("Incorrect tableswitch");
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
							if(count < 0 || count > 16384)
							{
								throw new ClassFormatError("Incorrect lookupswitch");
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
					this.normopcode = ByteCodeMetaData.GetNormalizedByteCode(bc);
					arg1 = ByteCodeMetaData.GetArg(bc, arg1);
				}

				internal int PC
				{
					get
					{
						return pc;
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
}
