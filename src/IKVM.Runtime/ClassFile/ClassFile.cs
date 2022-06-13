/*
  Copyright (C) 2002-2015 Jeroen Frijters

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

using IKVM.Attributes;

namespace IKVM.Internal
{

    sealed partial class ClassFile
	{

		private ConstantPoolItem[] constantpool;
		// Modifiers is a ushort, so the next four fields combine into two 32 bit slots
		private Modifiers access_flags;
		private ushort this_class;
		private ushort super_class;
		private ushort flags;
		private const ushort FLAG_MASK_MAJORVERSION = 0xFF;
		private const ushort FLAG_MASK_DEPRECATED = 0x100;
		private const ushort FLAG_MASK_INTERNAL = 0x200;
		private const ushort FLAG_CALLERSENSITIVE = 0x400;
		private const ushort FLAG_LAMBDAFORM_COMPILED = 0x800;
		private const ushort FLAG_LAMBDAFORM_HIDDEN = 0x1000;
		private const ushort FLAG_FORCEINLINE = 0x2000;
		private const ushort FLAG_HAS_ASSERTIONS = 0x4000;
		private ConstantPoolItemClass[] interfaces;
		private Field[] fields;
		private Method[] methods;
		private string sourceFile;
#if STATIC_COMPILER
		private string sourcePath;
#endif
		private string ikvmAssembly;
		private InnerClass[] innerClasses;
		private object[] annotations;
		private string signature;
		private string[] enclosingMethod;
		private BootstrapMethod[] bootstrapMethods;
		private byte[] runtimeVisibleTypeAnnotations;

#if STATIC_COMPILER
		// This method parses just enough of the class file to obtain its name and
		// determine if the class is a possible ikvmstub generated stub, it doesn't
		// validate the class file structure, but it may throw a ClassFormatError if it
		// encounters bogus data
		internal static string GetClassName(byte[] buf, int offset, int length, out bool isstub)
		{
			isstub = false;
			BigEndianBinaryReader br = new BigEndianBinaryReader(buf, offset, length);
			if(br.ReadUInt32() != 0xCAFEBABE)
			{
				throw new ClassFormatError("Bad magic number");
			}
			int minorVersion = br.ReadUInt16();
			int majorVersion = br.ReadUInt16();
			if((majorVersion & FLAG_MASK_MAJORVERSION) != majorVersion
				|| majorVersion < SupportedVersions.Minimum
				|| majorVersion > SupportedVersions.Maximum
				|| (majorVersion == SupportedVersions.Minimum && minorVersion < 3)
				|| (majorVersion == SupportedVersions.Maximum && minorVersion != 0))
			{
				throw new UnsupportedClassVersionError(majorVersion + "." + minorVersion);
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
					case Constant.InvokeDynamic:
					case Constant.NameAndType:
					case Constant.Float:
					case Constant.Integer:
						br.Skip(4);
						break;
					case Constant.MethodHandle:
						br.Skip(3);
						break;
					case Constant.String:
					case Constant.MethodType:
						br.Skip(2);
						break;
					case Constant.Utf8:
						isstub |= (utf8_cp[i] = br.ReadString(null, majorVersion)) == "IKVM.NET.Assembly";
						break;
					default:
						throw new ClassFormatError("Illegal constant pool type 0x{0:X}", tag);
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
#endif // STATIC_COMPILER

		internal ClassFile(byte[] buf, int offset, int length, string inputClassName, ClassFileParseOptions options, object[] constantPoolPatches)
		{
			try
			{
				BigEndianBinaryReader br = new BigEndianBinaryReader(buf, offset, length);
				if(br.ReadUInt32() != 0xCAFEBABE)
				{
					throw new ClassFormatError("{0} (Bad magic number)", inputClassName);
				}
				ushort minorVersion = br.ReadUInt16();
				ushort majorVersion = br.ReadUInt16();
				if((majorVersion & FLAG_MASK_MAJORVERSION) != majorVersion
					|| majorVersion < SupportedVersions.Minimum
					|| majorVersion > SupportedVersions.Maximum
					|| (majorVersion == SupportedVersions.Minimum && minorVersion < 3)
					|| (majorVersion == SupportedVersions.Maximum && minorVersion != 0))
				{
					throw new UnsupportedClassVersionError(inputClassName + " (" + majorVersion + "." + minorVersion + ")");
				}
				flags = majorVersion;
				int constantpoolcount = br.ReadUInt16();
				constantpool = new ConstantPoolItem[constantpoolcount];
				string[] utf8_cp = new string[constantpoolcount];
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
						case Constant.MethodHandle:
							if (majorVersion < 51)
								goto default;
							constantpool[i] = new ConstantPoolItemMethodHandle(br);
							break;
						case Constant.MethodType:
							if (majorVersion < 51)
								goto default;
							constantpool[i] = new ConstantPoolItemMethodType(br);
							break;
						case Constant.InvokeDynamic:
							if (majorVersion < 51)
								goto default;
							constantpool[i] = new ConstantPoolItemInvokeDynamic(br);
							break;
						case Constant.String:
							constantpool[i] = new ConstantPoolItemString(br);
							break;
						case Constant.Utf8:
							utf8_cp[i] = br.ReadString(inputClassName, majorVersion);
							break;
						default:
							throw new ClassFormatError("{0} (Illegal constant pool type 0x{1:X})", inputClassName, tag);
					}
				}
				if (constantPoolPatches != null)
				{
					PatchConstantPool(constantPoolPatches, utf8_cp, inputClassName);
				}
				for(int i = 1; i < constantpoolcount; i++)
				{
					if(constantpool[i] != null)
					{
						try
						{
							constantpool[i].Resolve(this, utf8_cp, options);
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
				// javac (JDK 1.1) didn't do this, so the VM doesn't enforce this rule for older class files.
				// NOTE although the vmspec implies (in 4.1) that ACC_SUPER is illegal on interfaces, it doesn't enforce this
				// for older class files.
				// (See http://bugs.sun.com/bugdatabase/view_bug.do?bug_id=6320322)
				if((IsInterface && IsFinal)
					|| (IsAbstract && IsFinal)
					|| (majorVersion >= 49 && IsAnnotation && !IsInterface)
					|| (majorVersion >= 49 && IsInterface && (!IsAbstract || IsSuper || IsEnum)))
				{
					throw new ClassFormatError("{0} (Illegal class modifiers 0x{1:X})", inputClassName, access_flags);
				}
				this_class = br.ReadUInt16();
				ValidateConstantPoolItemClass(inputClassName, this_class);
				super_class = br.ReadUInt16();
				ValidateConstantPoolItemClass(inputClassName, super_class);
				if(IsInterface && (super_class == 0 || this.SuperClass.Name != "java.lang.Object"))
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
				}
				CheckDuplicates(interfaces, "Repetitive interface name");
				int fields_count = br.ReadUInt16();
				fields = new Field[fields_count];
				for(int i = 0; i < fields_count; i++)
				{
					fields[i] = new Field(this, utf8_cp, br);
					string name = fields[i].Name;
					if(!IsValidFieldName(name, majorVersion))
					{
						throw new ClassFormatError("{0} (Illegal field name \"{1}\")", Name, name);
					}
				}
				CheckDuplicates<FieldOrMethod>(fields, "Repetitive field name/signature");
				int methods_count = br.ReadUInt16();
				methods = new Method[methods_count];
				for(int i = 0; i < methods_count; i++)
				{
					methods[i] = new Method(this, utf8_cp, options, br);
					string name = methods[i].Name;
					string sig = methods[i].Signature;
					if(!IsValidMethodName(name, majorVersion))
					{
						if(!ReferenceEquals(name, StringConstants.INIT) && !ReferenceEquals(name, StringConstants.CLINIT))
						{
							throw new ClassFormatError("{0} (Illegal method name \"{1}\")", Name, name);
						}
						if(!sig.EndsWith("V"))
						{
							throw new ClassFormatError("{0} (Method \"{1}\" has illegal signature \"{2}\")", Name, name, sig);
						}
						if((options & ClassFileParseOptions.RemoveAssertions) != 0 && methods[i].IsClassInitializer)
						{
							RemoveAssertionInit(methods[i]);
						}
					}
				}
				CheckDuplicates<FieldOrMethod>(methods, "Repetitive method name/signature");
				int attributes_count = br.ReadUInt16();
				for(int i = 0; i < attributes_count; i++)
				{
					switch(GetConstantPoolUtf8String(utf8_cp, br.ReadUInt16()))
					{
						case "Deprecated":
							if(br.ReadUInt32() != 0)
							{
								throw new ClassFormatError("Invalid Deprecated attribute length");
							}
							flags |= FLAG_MASK_DEPRECATED;
							break;
						case "SourceFile":
							if(br.ReadUInt32() != 2)
							{
								throw new ClassFormatError("SourceFile attribute has incorrect length");
							}
							sourceFile = GetConstantPoolUtf8String(utf8_cp, br.ReadUInt16());
							break;
						case "InnerClasses":
						{
							BigEndianBinaryReader rdr = br;
							uint attribute_length = br.ReadUInt32();
							ushort count = rdr.ReadUInt16();
							if(this.MajorVersion >= 49 && attribute_length != 2 + count * (2 + 2 + 2 + 2))
							{
								throw new ClassFormatError("{0} (InnerClasses attribute has incorrect length)", this.Name);
							}
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
								if(innerClasses[j].innerClass != 0 && innerClasses[j].outerClass != 0)
								{
									MarkLinkRequiredConstantPoolItem(innerClasses[j].innerClass);
									MarkLinkRequiredConstantPoolItem(innerClasses[j].outerClass);
								}
							}
							break;
						}
						case "Signature":
							if(majorVersion < 49)
							{
								goto default;
							}
							if(br.ReadUInt32() != 2)
							{
								throw new ClassFormatError("Signature attribute has incorrect length");
							}
							signature = GetConstantPoolUtf8String(utf8_cp, br.ReadUInt16());
							break;
						case "EnclosingMethod":
							if(majorVersion < 49)
							{
								goto default;
							}
							if(br.ReadUInt32() != 4)
							{
								throw new ClassFormatError("EnclosingMethod attribute has incorrect length");
							}
							else
							{
								ushort class_index = br.ReadUInt16();
								ushort method_index = br.ReadUInt16();
								ValidateConstantPoolItemClass(inputClassName, class_index);
								if(method_index == 0)
								{
									enclosingMethod = new string[] {
										GetConstantPoolClass(class_index),
										null,
										null
																   };
								}
								else
								{
									ConstantPoolItemNameAndType m = GetConstantPoolItem(method_index) as ConstantPoolItemNameAndType;
									if(m == null)
									{
										throw new ClassFormatError("{0} (Bad constant pool index #{1})", inputClassName, method_index);
									}
									enclosingMethod = new string[] {
										GetConstantPoolClass(class_index),
										GetConstantPoolUtf8String(utf8_cp, m.name_index),
										GetConstantPoolUtf8String(utf8_cp, m.descriptor_index).Replace('/', '.')
																   };
								}
							}
							break;
						case "RuntimeVisibleAnnotations":
							if(majorVersion < 49)
							{
								goto default;
							}
							annotations = ReadAnnotations(br, this, utf8_cp);
							break;
#if STATIC_COMPILER
						case "RuntimeInvisibleAnnotations":
							if(majorVersion < 49)
							{
								goto default;
							}
							foreach(object[] annot in ReadAnnotations(br, this, utf8_cp))
							{
								if(annot[1].Equals("Likvm/lang/Internal;"))
								{
									this.access_flags &= ~Modifiers.AccessMask;
									flags |= FLAG_MASK_INTERNAL;
								}
							}
							break;
#endif
						case "BootstrapMethods":
							if(majorVersion < 51)
							{
								goto default;
							}
							bootstrapMethods = ReadBootstrapMethods(br, this);
							break;
						case "RuntimeVisibleTypeAnnotations":
							if(majorVersion < 52)
							{
								goto default;
							}
							CreateUtf8ConstantPoolItems(utf8_cp);
							runtimeVisibleTypeAnnotations = br.Section(br.ReadUInt32()).ToArray();
							break;
						case "IKVM.NET.Assembly":
							if(br.ReadUInt32() != 2)
							{
								throw new ClassFormatError("IKVM.NET.Assembly attribute has incorrect length");
							}
							ikvmAssembly = GetConstantPoolUtf8String(utf8_cp, br.ReadUInt16());
							break;
						default:
							br.Skip(br.ReadUInt32());
							break;
					}
				}
				// validate the invokedynamic entries to point into the bootstrapMethods array
				for(int i = 1; i < constantpoolcount; i++)
				{
					ConstantPoolItemInvokeDynamic cpi;
					if(constantpool[i] != null
						&& (cpi = constantpool[i] as ConstantPoolItemInvokeDynamic) != null)
					{
						if(bootstrapMethods == null || cpi.BootstrapMethod >= bootstrapMethods.Length)
						{
							throw new ClassFormatError("Short length on BootstrapMethods in class file");
						}
					}
				}
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

		private void CreateUtf8ConstantPoolItems(string[] utf8_cp)
		{
			for (int i = 0; i < constantpool.Length; i++)
			{
				if (constantpool[i] == null && utf8_cp[i] != null)
				{
					constantpool[i] = new ConstantPoolItemUtf8(utf8_cp[i]);
				}
			}
		}

		private void CheckDuplicates<T>(T[] members, string msg)
			where T : IEquatable<T>
		{
			if (members.Length < 100)
			{
				for (int i = 0; i < members.Length; i++)
				{
					for (int j = 0; j < i; j++)
					{
						if (members[i].Equals(members[j]))
						{
							throw new ClassFormatError("{0} ({1})", Name, msg);
						}
					}
				}
			}
			else
			{
				Dictionary<T, object> dict = new Dictionary<T, object>();
				for (int i = 0; i < members.Length; i++)
				{
					if (dict.ContainsKey(members[i]))
					{
						throw new ClassFormatError("{0} ({1})", Name, msg);
					}
					dict.Add(members[i], null);
				}
			}
		}

		private void PatchConstantPool(object[] constantPoolPatches, string[] utf8_cp, string inputClassName)
		{
#if !STATIC_COMPILER && !FIRST_PASS
			for (int i = 0; i < constantPoolPatches.Length; i++)
			{
				if (constantPoolPatches[i] != null)
				{
					if (utf8_cp[i] != null)
					{
						if (!(constantPoolPatches[i] is string))
						{
							throw new ClassFormatError("Illegal utf8 patch at {0} in class file {1}", i, inputClassName);
						}
						utf8_cp[i] = (string)constantPoolPatches[i];
					}
					else if (constantpool[i] != null)
					{
						switch (constantpool[i].GetConstantType())
						{
							case ConstantType.String:
								constantpool[i] = new ConstantPoolItemLiveObject(constantPoolPatches[i]);
								break;
							case ConstantType.Class:
								java.lang.Class clazz;
								string name;
								if ((clazz = constantPoolPatches[i] as java.lang.Class) != null)
								{
									TypeWrapper tw = TypeWrapper.FromClass(clazz);
									constantpool[i] = new ConstantPoolItemClass(tw.Name, tw);
								}
								else if ((name = constantPoolPatches[i] as string) != null)
								{
									constantpool[i] = new ConstantPoolItemClass(String.Intern(name.Replace('/', '.')), null);
								}
								else
								{
									throw new ClassFormatError("Illegal class patch at {0} in class file {1}", i, inputClassName);
								}
								break;
							case ConstantType.Integer:
								((ConstantPoolItemInteger)constantpool[i]).v = ((java.lang.Integer)constantPoolPatches[i]).intValue();
								break;
							case ConstantType.Long:
								((ConstantPoolItemLong)constantpool[i]).l = ((java.lang.Long)constantPoolPatches[i]).longValue();
								break;
							case ConstantType.Float:
								((ConstantPoolItemFloat)constantpool[i]).v = ((java.lang.Float)constantPoolPatches[i]).floatValue();
								break;
							case ConstantType.Double:
								((ConstantPoolItemDouble)constantpool[i]).d = ((java.lang.Double)constantPoolPatches[i]).doubleValue();
								break;
							default:
								throw new NotImplementedException("ConstantPoolPatch: " + constantPoolPatches[i]);
						}
					}
				}
			}
#endif
		}

		private void MarkLinkRequiredConstantPoolItem(int index)
		{
			if (index > 0 && index < constantpool.Length && constantpool[index] != null)
			{
				constantpool[index].MarkLinkRequired();
			}
		}

		private static BootstrapMethod[] ReadBootstrapMethods(BigEndianBinaryReader br, ClassFile classFile)
		{
			BigEndianBinaryReader rdr = br.Section(br.ReadUInt32());
			ushort count = rdr.ReadUInt16();
			BootstrapMethod[] bsm = new BootstrapMethod[count];
			for(int i = 0; i < bsm.Length; i++)
			{
				ushort bsm_index = rdr.ReadUInt16();
				if(bsm_index >= classFile.constantpool.Length || !(classFile.constantpool[bsm_index] is ConstantPoolItemMethodHandle))
				{
					throw new ClassFormatError("bootstrap_method_index {0} has bad constant type in class file {1}", bsm_index, classFile.Name);
				}
				classFile.MarkLinkRequiredConstantPoolItem(bsm_index);
				ushort argument_count = rdr.ReadUInt16();
				ushort[] args = new ushort[argument_count];
				for(int j = 0; j < args.Length; j++)
				{
					ushort argument_index = rdr.ReadUInt16();
					if(!classFile.IsValidConstant(argument_index))
					{
						throw new ClassFormatError("argument_index {0} has bad constant type in class file {1}", argument_index, classFile.Name);
					}
					classFile.MarkLinkRequiredConstantPoolItem(argument_index);
					args[j] = argument_index;
				}
				bsm[i] = new BootstrapMethod(bsm_index, args);
			}
			if(!rdr.IsAtEnd)
			{
				throw new ClassFormatError("Bad length on BootstrapMethods in class file {0}", classFile.Name);
			}
			return bsm;
		}

		private bool IsValidConstant(ushort index)
		{
			if(index < constantpool.Length && constantpool[index] != null)
			{
				try
				{
					constantpool[index].GetConstantType();
					return true;
				}
				catch (InvalidOperationException) { }
			}
			return false;
		}

		private static object[] ReadAnnotations(BigEndianBinaryReader br, ClassFile classFile, string[] utf8_cp)
		{
			BigEndianBinaryReader rdr = br.Section(br.ReadUInt32());
			ushort num_annotations = rdr.ReadUInt16();
			object[] annotations = new object[num_annotations];
			for(int i = 0; i < annotations.Length; i++)
			{
				annotations[i] = ReadAnnotation(rdr, classFile, utf8_cp);
			}
			if(!rdr.IsAtEnd)
			{
				throw new ClassFormatError("{0} (RuntimeVisibleAnnotations attribute has wrong length)", classFile.Name);
			}
			return annotations;
		}

		private static object ReadAnnotation(BigEndianBinaryReader rdr, ClassFile classFile, string[] utf8_cp)
		{
			string type = classFile.GetConstantPoolUtf8String(utf8_cp, rdr.ReadUInt16());
			ushort num_element_value_pairs = rdr.ReadUInt16();
			object[] annot = new object[2 + num_element_value_pairs * 2];
			annot[0] = AnnotationDefaultAttribute.TAG_ANNOTATION;
			annot[1] = type;
			for(int i = 0; i < num_element_value_pairs; i++)
			{
				annot[2 + i * 2 + 0] = classFile.GetConstantPoolUtf8String(utf8_cp, rdr.ReadUInt16());
				annot[2 + i * 2 + 1] = ReadAnnotationElementValue(rdr, classFile, utf8_cp);
			}
			return annot;
		}

		private static object ReadAnnotationElementValue(BigEndianBinaryReader rdr, ClassFile classFile, string[] utf8_cp)
		{
			try
			{
				byte tag = rdr.ReadByte();
				switch (tag)
				{
					case (byte)'Z':
						return classFile.GetConstantPoolConstantInteger(rdr.ReadUInt16()) != 0;
					case (byte)'B':
						return (byte)classFile.GetConstantPoolConstantInteger(rdr.ReadUInt16());
					case (byte)'C':
						return (char)classFile.GetConstantPoolConstantInteger(rdr.ReadUInt16());
					case (byte)'S':
						return (short)classFile.GetConstantPoolConstantInteger(rdr.ReadUInt16());
					case (byte)'I':
						return classFile.GetConstantPoolConstantInteger(rdr.ReadUInt16());
					case (byte)'F':
						return classFile.GetConstantPoolConstantFloat(rdr.ReadUInt16());
					case (byte)'J':
						return classFile.GetConstantPoolConstantLong(rdr.ReadUInt16());
					case (byte)'D':
						return classFile.GetConstantPoolConstantDouble(rdr.ReadUInt16());
					case (byte)'s':
						return classFile.GetConstantPoolUtf8String(utf8_cp, rdr.ReadUInt16());
					case (byte)'e':
						{
							ushort type_name_index = rdr.ReadUInt16();
							ushort const_name_index = rdr.ReadUInt16();
							return new object[] {
											AnnotationDefaultAttribute.TAG_ENUM,
											classFile.GetConstantPoolUtf8String(utf8_cp, type_name_index),
											classFile.GetConstantPoolUtf8String(utf8_cp, const_name_index)
										};
						}
					case (byte)'c':
						return new object[] {
											AnnotationDefaultAttribute.TAG_CLASS,
											classFile.GetConstantPoolUtf8String(utf8_cp, rdr.ReadUInt16())
										};
					case (byte)'@':
						return ReadAnnotation(rdr, classFile, utf8_cp);
					case (byte)'[':
						{
							ushort num_values = rdr.ReadUInt16();
							object[] array = new object[num_values + 1];
							array[0] = AnnotationDefaultAttribute.TAG_ARRAY;
							for (int i = 0; i < num_values; i++)
							{
								array[i + 1] = ReadAnnotationElementValue(rdr, classFile, utf8_cp);
							}
							return array;
						}
					default:
						throw new ClassFormatError("Invalid tag {0} in annotation element_value", tag);
				}
			}
			catch (NullReferenceException)
			{
			}
			catch (InvalidCastException)
			{
			}
			catch (IndexOutOfRangeException)
			{
			}
			return new object[] { AnnotationDefaultAttribute.TAG_ERROR, "java.lang.IllegalArgumentException", "Wrong type at constant pool index" };
		}

		private void ValidateConstantPoolItemClass(string classFile, ushort index)
		{
			if(index >= constantpool.Length || !(constantpool[index] is ConstantPoolItemClass))
			{
				throw new ClassFormatError("{0} (Bad constant pool index #{1})", classFile, index);
			}
		}

		private static bool IsValidMethodName(string name, int majorVersion)
		{
			if(name.Length == 0)
			{
				return false;
			}
			for(int i = 0; i < name.Length; i++)
			{
				if(".;[/<>".IndexOf(name[i]) != -1)
				{
					return false;
				}
			}
			return majorVersion >= 49 || IsValidPre49Identifier(name);
		}

		private static bool IsValidFieldName(string name, int majorVersion)
		{
			if(name.Length == 0)
			{
				return false;
			}
			for(int i = 0; i < name.Length; i++)
			{
				if(".;[/".IndexOf(name[i]) != -1)
				{
					return false;
				}
			}
			return majorVersion >= 49 || IsValidPre49Identifier(name);
		}

		private static bool IsValidPre49Identifier(string name)
		{
			if(!Char.IsLetter(name[0]) && "$_".IndexOf(name[0]) == -1)
			{
				return false;
			}
			for(int i = 1; i < name.Length; i++)
			{
				if(!Char.IsLetterOrDigit(name[i]) && "$_".IndexOf(name[i]) == -1)
				{
					return false;
				}
			}
			return true;
		}

		internal static bool IsValidFieldSig(string sig)
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

		internal static bool IsValidMethodSig(string sig)
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
				return flags & FLAG_MASK_MAJORVERSION;
			}
		}

		internal void Link(TypeWrapper thisType, LoadMode mode)
		{
			// this is not just an optimization, it's required for anonymous classes to be able to refer to themselves
			((ConstantPoolItemClass)constantpool[this_class]).LinkSelf(thisType);
			for(int i = 1; i < constantpool.Length; i++)
			{
				if(constantpool[i] != null)
				{
					constantpool[i].Link(thisType, mode);
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

		internal bool IsEnum
		{
			get
			{
				return (access_flags & Modifiers.Enum) != 0;
			}
		}

		internal bool IsAnnotation
		{
			get
			{
				return (access_flags & Modifiers.Annotation) != 0;
			}
		}

		internal bool IsSuper
		{
			get
			{
				return (access_flags & Modifiers.Super) != 0;
			}
		}

		internal bool IsReferenced(Field fld)
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

		// this won't throw an exception if index is invalid
		// (used by IsSideEffectFreeStaticInitializer)
		internal ConstantPoolItemFieldref SafeGetFieldref(int index)
		{
			if(index > 0 && index < constantpool.Length)
			{
				return constantpool[index] as ConstantPoolItemFieldref;
			}
			return null;
		}

		// NOTE this returns an MI, because it used for both normal methods and interface methods
		internal ConstantPoolItemMI GetMethodref(int index)
		{
			return (ConstantPoolItemMI)constantpool[index];
		}

		// this won't throw an exception if index is invalid
		// (used by IsAccessBridge)
		internal ConstantPoolItemMI SafeGetMethodref(int index)
		{
			if (index > 0 && index < constantpool.Length)
			{
				return constantpool[index] as ConstantPoolItemMI;
			}
			return null;
		}

		internal ConstantPoolItemInvokeDynamic GetInvokeDynamic(int index)
		{
			return (ConstantPoolItemInvokeDynamic)constantpool[index];
		}

		private ConstantPoolItem GetConstantPoolItem(int index)
		{
			return constantpool[index];
		}

		internal string GetConstantPoolClass(int index)
		{
			return ((ConstantPoolItemClass)constantpool[index]).Name;
		}

		private bool SafeIsConstantPoolClass(int index)
		{
			if(index > 0 && index < constantpool.Length)
			{
				return constantpool[index] as ConstantPoolItemClass != null;
			}
			return false;
		}

		internal TypeWrapper GetConstantPoolClassType(int index)
		{
			return ((ConstantPoolItemClass)constantpool[index]).GetClassType();
		}

		private string GetConstantPoolUtf8String(string[] utf8_cp, int index)
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

		internal ConstantPoolItemMethodHandle GetConstantPoolConstantMethodHandle(int index)
		{
			return (ConstantPoolItemMethodHandle)constantpool[index];
		}

		internal ConstantPoolItemMethodType GetConstantPoolConstantMethodType(int index)
		{
			return (ConstantPoolItemMethodType)constantpool[index];
		}

		internal object GetConstantPoolConstantLiveObject(int index)
		{
			return ((ConstantPoolItemLiveObject)constantpool[index]).Value;
		}

		internal string Name
		{
			get
			{
				return GetConstantPoolClass(this_class);
			}
		}

		internal ConstantPoolItemClass SuperClass
		{
			get
			{
				return (ConstantPoolItemClass)constantpool[super_class];
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

		internal string SourcePath
		{
#if STATIC_COMPILER
			get { return sourcePath; }
			set { sourcePath = value; }
#else
			get { return sourceFile; }
#endif
		}

		internal object[] Annotations
		{
			get
			{
				return annotations;
			}
		}

		internal string GenericSignature
		{
			get
			{
				return signature;
			}
		}

		internal string[] EnclosingMethod
		{
			get
			{
				return enclosingMethod;
			}
		}

		internal byte[] RuntimeVisibleTypeAnnotations
		{
			get
			{
				return runtimeVisibleTypeAnnotations;
			}
		}

		internal object[] GetConstantPool()
		{
			object[] cp = new object[constantpool.Length];
			for (int i = 1; i < cp.Length; i++)
			{
				if (constantpool[i] != null)
				{
					cp[i] = constantpool[i].GetRuntimeValue();
				}
			}
			return cp;
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
				return (flags & FLAG_MASK_DEPRECATED) != 0;
			}
		}

		internal bool IsInternal
		{
			get
			{
				return (flags & FLAG_MASK_INTERNAL) != 0;
			}
		}

		// for use by ikvmc (to implement the -privatepackage option)
		internal void SetInternal()
		{
			access_flags &= ~Modifiers.AccessMask;
			flags |= FLAG_MASK_INTERNAL;
		}

		internal bool HasAssertions
		{
			get
			{
				return (flags & FLAG_HAS_ASSERTIONS) != 0;
			}
		}

		internal bool HasInitializedFields
		{
			get
			{
				foreach (Field f in fields)
				{
					if (f.IsStatic && !f.IsFinal && f.ConstantValue != null)
					{
						return true;
					}
				}
				return false;
			}
		}

		internal BootstrapMethod GetBootstrapMethod(int index)
		{
			return bootstrapMethods[index];
		}

		internal InnerClass[] InnerClasses
		{
			get
			{
				return innerClasses;
			}
		}

		internal Field GetField(string name, string sig)
		{
			for (int i = 0; i < fields.Length; i++)
			{
				if (fields[i].Name == name && fields[i].Signature == sig)
				{
					return fields[i];
				}
			}
			return null;
		}

		private void RemoveAssertionInit(Method m)
		{
			/* We match the following code sequence:
			 *   0  ldc <class X>
			 *   2  invokevirtual <Method java/lang/Class desiredAssertionStatus()Z>
			 *   5  ifne 12
			 *   8  iconst_1
			 *   9  goto 13
			 *  12  iconst_0
			 *  13  putstatic <Field <this class> boolean <static final field>>
			 */
			ConstantPoolItemFieldref fieldref;
			Field field;
			if (m.Instructions[0].NormalizedOpCode == NormalizedByteCode.__ldc && SafeIsConstantPoolClass(m.Instructions[0].Arg1)
				&& m.Instructions[1].NormalizedOpCode == NormalizedByteCode.__invokevirtual && IsDesiredAssertionStatusMethodref(m.Instructions[1].Arg1)
				&& m.Instructions[2].NormalizedOpCode == NormalizedByteCode.__ifne && m.Instructions[2].TargetIndex == 5
				&& m.Instructions[3].NormalizedOpCode == NormalizedByteCode.__iconst && m.Instructions[3].Arg1 == 1
				&& m.Instructions[4].NormalizedOpCode == NormalizedByteCode.__goto && m.Instructions[4].TargetIndex == 6
				&& m.Instructions[5].NormalizedOpCode == NormalizedByteCode.__iconst && m.Instructions[5].Arg1 == 0
				&& m.Instructions[6].NormalizedOpCode == NormalizedByteCode.__putstatic && (fieldref = SafeGetFieldref(m.Instructions[6].Arg1)) != null
				&& fieldref.Class == Name && fieldref.Signature == "Z"
				&& (field = GetField(fieldref.Name, fieldref.Signature)) != null
				&& field.IsStatic && field.IsFinal
				&& !HasBranchIntoRegion(m.Instructions, 7, m.Instructions.Length, 0, 7)
				&& !HasStaticFieldWrite(m.Instructions, 7, m.Instructions.Length, field)
				&& !HasExceptionHandlerInRegion(m.ExceptionTable, 0, 7))
			{
				field.PatchConstantValue(true);
				m.Instructions[0].PatchOpCode(NormalizedByteCode.__goto, 7);
				flags |= FLAG_HAS_ASSERTIONS;
			}
		}

		private bool IsDesiredAssertionStatusMethodref(int cpi)
		{
			ConstantPoolItemMethodref method = SafeGetMethodref(cpi) as ConstantPoolItemMethodref;
			return method != null
				&& method.Class == "java.lang.Class"
				&& method.Name == "desiredAssertionStatus"
				&& method.Signature == "()Z";
		}

		private static bool HasBranchIntoRegion(Method.Instruction[] instructions, int checkStart, int checkEnd, int regionStart, int regionEnd)
		{
			for (int i = checkStart; i < checkEnd; i++)
			{
				switch (instructions[i].NormalizedOpCode)
				{
					case NormalizedByteCode.__ifeq:
					case NormalizedByteCode.__ifne:
					case NormalizedByteCode.__iflt:
					case NormalizedByteCode.__ifge:
					case NormalizedByteCode.__ifgt:
					case NormalizedByteCode.__ifle:
					case NormalizedByteCode.__if_icmpeq:
					case NormalizedByteCode.__if_icmpne:
					case NormalizedByteCode.__if_icmplt:
					case NormalizedByteCode.__if_icmpge:
					case NormalizedByteCode.__if_icmpgt:
					case NormalizedByteCode.__if_icmple:
					case NormalizedByteCode.__if_acmpeq:
					case NormalizedByteCode.__if_acmpne:
					case NormalizedByteCode.__ifnull:
					case NormalizedByteCode.__ifnonnull:
					case NormalizedByteCode.__goto:
					case NormalizedByteCode.__jsr:
						if (instructions[i].TargetIndex > regionStart && instructions[i].TargetIndex < regionEnd)
						{
							return true;
						}
						break;
					case NormalizedByteCode.__tableswitch:
					case NormalizedByteCode.__lookupswitch:
						if (instructions[i].DefaultTarget > regionStart && instructions[i].DefaultTarget < regionEnd)
						{
							return true;
						}
						for (int j = 0; j < instructions[i].SwitchEntryCount; j++)
						{
							if (instructions[i].GetSwitchTargetIndex(j) > regionStart && instructions[i].GetSwitchTargetIndex(j) < regionEnd)
							{
								return true;
							}
						}
						break;
				}
			}
			return false;
		}

		private bool HasStaticFieldWrite(Method.Instruction[] instructions, int checkStart, int checkEnd, Field field)
		{
			for (int i = checkStart; i < checkEnd; i++)
			{
				if (instructions[i].NormalizedOpCode == NormalizedByteCode.__putstatic)
				{
					ConstantPoolItemFieldref fieldref = SafeGetFieldref(instructions[i].Arg1);
					if (fieldref != null && fieldref.Class == Name && fieldref.Name == field.Name && fieldref.Signature == field.Signature)
					{
						return true;
					}
				}
			}
			return false;
		}

		private static bool HasExceptionHandlerInRegion(Method.ExceptionTableEntry[] entries, int regionStart, int regionEnd)
		{
			for (int i = 0; i < entries.Length; i++)
			{
				if (entries[i].handlerIndex > regionStart && entries[i].handlerIndex < regionEnd)
				{
					return true;
				}
			}
			return false;
		}
	}

}
