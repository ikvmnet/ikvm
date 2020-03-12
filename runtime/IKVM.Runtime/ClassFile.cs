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
using System.IO;
using System.Collections.Generic;
using IKVM.Attributes;

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
		LinkageError,
		// "exceptions" that are wrapped in an IncompatibleClassChangeError
		IllegalAccessException,
		// if an error is added here, it must also be added to MethodAnalyzer.SetHardError()
	}

	[Flags]
	enum ClassFileParseOptions
	{
		None = 0,
		LocalVariableTable = 1,
		LineNumberTable = 2,
		RelaxedClassNameValidation = 4,
		TrustedAnnotations = 8,
		RemoveAssertions = 16,
	}

	sealed class ClassFile
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

		private static class SupportedVersions
		{
			internal static readonly int Minimum = 45;
			internal static readonly int Maximum = Experimental.JDK_9 ? 53 : 52;
		}

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

		internal struct BootstrapMethod
		{
			private ushort bsm_index;
			private ushort[] args;

			internal BootstrapMethod(ushort bsm_index, ushort[] args)
			{
				this.bsm_index = bsm_index;
				this.args = args;
			}

			internal int BootstrapMethodIndex
			{
				get { return bsm_index; }
			}

			internal int ArgumentCount
			{
				get { return args.Length; }
			}

			internal int GetArgument(int index)
			{
				return args[index];
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

		internal enum RefKind
		{
			getField = 1,
			getStatic = 2,
			putField = 3,
			putStatic = 4,
			invokeVirtual = 5,
			invokeStatic = 6,
			invokeSpecial = 7,
			newInvokeSpecial = 8,
			invokeInterface = 9
		}

		internal enum ConstantType
		{
			Integer,
			Long,
			Float,
			Double,
			String,
			Class,
			MethodHandle,
			MethodType,
			LiveObject,		// used by anonymous class constant pool patching
		}

		internal abstract class ConstantPoolItem
		{
			internal virtual void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
			{
			}

			internal virtual void Link(TypeWrapper thisType, LoadMode mode)
			{
			}

			internal virtual ConstantType GetConstantType()
			{
				throw new InvalidOperationException();
			}

			internal virtual void MarkLinkRequired()
			{
			}

			// this is used for sun.reflect.ConstantPool
			// it returns a boxed System.Int32, System.Int64, System.Float, System.Double or a string
			internal virtual object GetRuntimeValue()
			{
				return null;
			}
		}

		internal sealed class ConstantPoolItemClass : ConstantPoolItem, IEquatable<ConstantPoolItemClass>
		{
			private ushort name_index;
			private string name;
			private TypeWrapper typeWrapper;
			private static char[] invalidJava15Characters = { '.', ';', '[', ']' };

			internal ConstantPoolItemClass(BigEndianBinaryReader br)
			{
				name_index = br.ReadUInt16();
			}

			internal ConstantPoolItemClass(string name, TypeWrapper typeWrapper)
			{
				this.name = name;
				this.typeWrapper = typeWrapper;
			}

			internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
			{
				// if the item was patched, we already have a name
				if(name != null)
				{
					return;
				}
				name = classFile.GetConstantPoolUtf8String(utf8_cp, name_index);
				if(name.Length > 0)
				{
					// We don't enforce the strict class name rules in the static compiler, since HotSpot doesn't enforce *any* rules on
					// class names for the system (and boot) class loader. We still need to enforce the 1.5 restrictions, because we
					// rely on those invariants.
#if !STATIC_COMPILER
					if(classFile.MajorVersion < 49 && (options & ClassFileParseOptions.RelaxedClassNameValidation) == 0)
					{
						char prev = name[0];
						if(Char.IsLetter(prev) || prev == '$' || prev == '_' || prev == '[' || prev == '/')
						{
							int skip = 1;
							int end = name.Length;
							if(prev == '[')
							{
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
					else
#endif
					{
						// since 1.5 the restrictions on class names have been greatly reduced
						int start = 0;
						int end = name.Length;
						if(name[0] == '[')
						{
							if(!IsValidFieldSig(name))
							{
								goto barf;
							}
							// the semicolon is only allowed at the end and IsValidFieldSig enforces this,
							// but since invalidJava15Characters contains the semicolon, we decrement end
							// to make the following check against invalidJava15Characters ignore the
							// trailing semicolon.
							if(name[end - 1] == ';')
							{
								end--;
							}
							while(name[start] == '[')
							{
								start++;
							}
						}
						if(name.IndexOfAny(invalidJava15Characters, start, end - start) >= 0)
						{
							goto barf;
						}
						name = String.Intern(name.Replace('/', '.'));
						return;
					}
				}
				barf:
					throw new ClassFormatError("Invalid class name \"{0}\"", name);
			}

			internal override void MarkLinkRequired()
			{
				if(typeWrapper == null)
				{
					typeWrapper = VerifierTypeWrapper.Null;
				}
			}

			internal void LinkSelf(TypeWrapper thisType)
			{
				this.typeWrapper = thisType;
			}

			internal override void Link(TypeWrapper thisType, LoadMode mode)
			{
				if(typeWrapper == VerifierTypeWrapper.Null)
				{
					TypeWrapper tw = thisType.GetClassLoader().LoadClass(name, mode | LoadMode.WarnClassNotFound);
#if !STATIC_COMPILER && !FIRST_PASS
					if(!tw.IsUnloadable)
					{
						try
						{
							thisType.GetClassLoader().CheckPackageAccess(tw, thisType.ClassObject.pd);
						}
						catch(java.lang.SecurityException)
						{
							tw = new UnloadableTypeWrapper(name);
						}
					}
#endif
					typeWrapper = tw;
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

			public sealed override int GetHashCode()
			{
				return name.GetHashCode();
			}

			public bool Equals(ConstantPoolItemClass other)
			{
				return ReferenceEquals(name, other.name);
			}
		}

		private sealed class ConstantPoolItemDouble : ConstantPoolItem
		{
			internal double d;

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

			internal override object GetRuntimeValue()
			{
				return d;
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

			internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
			{
				ConstantPoolItemNameAndType name_and_type = (ConstantPoolItemNameAndType)classFile.GetConstantPoolItem(name_and_type_index);
				clazz = (ConstantPoolItemClass)classFile.GetConstantPoolItem(class_index);
				// if the constant pool items referred to were strings, GetConstantPoolItem returns null
				if(name_and_type == null || clazz == null)
				{
					throw new ClassFormatError("Bad index in constant pool");
				}
				name = String.Intern(classFile.GetConstantPoolUtf8String(utf8_cp, name_and_type.name_index));
				descriptor = classFile.GetConstantPoolUtf8String(utf8_cp, name_and_type.descriptor_index);
				Validate(name, descriptor, classFile.MajorVersion);
				descriptor = String.Intern(descriptor.Replace('/', '.'));
			}

			protected abstract void Validate(string name, string descriptor, int majorVersion);

			internal override void MarkLinkRequired()
			{
				clazz.MarkLinkRequired();
			}

			internal override void Link(TypeWrapper thisType, LoadMode mode)
			{
				clazz.Link(thisType, mode);
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

			internal abstract MemberWrapper GetMember();
		}

		internal sealed class ConstantPoolItemFieldref : ConstantPoolItemFMI
		{
			private FieldWrapper field;
			private TypeWrapper fieldTypeWrapper;

			internal ConstantPoolItemFieldref(BigEndianBinaryReader br) : base(br)
			{
			}

			protected override void Validate(string name, string descriptor, int majorVersion)
			{
				if(!IsValidFieldSig(descriptor))
				{
					throw new ClassFormatError("Invalid field signature \"{0}\"", descriptor);
				}
				if(!IsValidFieldName(name, majorVersion))
				{
					throw new ClassFormatError("Invalid field name \"{0}\"", name);
				}
			}

			internal TypeWrapper GetFieldType()
			{
				return fieldTypeWrapper;
			}

			internal override void Link(TypeWrapper thisType, LoadMode mode)
			{
				base.Link(thisType, mode);
				lock(this)
				{
					if(fieldTypeWrapper != null)
					{
						return;
					}
				}
				FieldWrapper fw = null;
				TypeWrapper wrapper = GetClassType();
				if(wrapper == null)
				{
					return;
				}
				if(!wrapper.IsUnloadable)
				{
					fw = wrapper.GetFieldWrapper(Name, Signature);
					if(fw != null)
					{
						fw.Link(mode);
					}
				}
				ClassLoaderWrapper classLoader = thisType.GetClassLoader();
				TypeWrapper fld = classLoader.FieldTypeWrapperFromSig(this.Signature, mode);
				lock(this)
				{
					if(fieldTypeWrapper == null)
					{
						fieldTypeWrapper = fld;
						field = fw;
					}
				}
			}

			internal FieldWrapper GetField()
			{
				return field;
			}

			internal override MemberWrapper GetMember()
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

			protected override void Validate(string name, string descriptor, int majorVersion)
			{
				if(!IsValidMethodSig(descriptor))
				{
					throw new ClassFormatError("Method {0} has invalid signature {1}", name, descriptor);
				}
				if(!IsValidMethodName(name, majorVersion))
				{
					if(!ReferenceEquals(name, StringConstants.INIT))
					{
						throw new ClassFormatError("Invalid method name \"{0}\"", name);
					}
					if(!descriptor.EndsWith("V"))
					{
						throw new ClassFormatError("Method {0} has invalid signature {1}", name, descriptor);
					}
				}
			}

			internal override void Link(TypeWrapper thisType, LoadMode mode)
			{
				base.Link(thisType, mode);
				lock(this)
				{
					if(argTypeWrappers != null)
					{
						return;
					}
				}
				ClassLoaderWrapper classLoader = thisType.GetClassLoader();
				TypeWrapper[] args = classLoader.ArgTypeWrapperListFromSig(this.Signature, mode);
				TypeWrapper ret = classLoader.RetTypeWrapperFromSig(this.Signature, mode);
				lock(this)
				{
					if(argTypeWrappers == null)
					{
						argTypeWrappers = args;
						retTypeWrapper = ret;
					}
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

			internal override MemberWrapper GetMember()
			{
				return method;
			}
		}

		internal sealed class ConstantPoolItemMethodref : ConstantPoolItemMI
		{
			internal ConstantPoolItemMethodref(BigEndianBinaryReader br) : base(br)
			{
			}

			internal override void Link(TypeWrapper thisType, LoadMode mode)
			{
				base.Link(thisType, mode);
				TypeWrapper wrapper = GetClassType();
				if(wrapper != null && !wrapper.IsUnloadable)
				{
					method = wrapper.GetMethodWrapper(Name, Signature, !ReferenceEquals(Name, StringConstants.INIT));
					if(method != null)
					{
						method.Link(mode);
					}
					if(Name != StringConstants.INIT
						&& !thisType.IsInterface
						&& (!JVM.AllowNonVirtualCalls || (thisType.Modifiers & Modifiers.Super) == Modifiers.Super)
						&& thisType != wrapper
						&& thisType.IsSubTypeOf(wrapper))
					{
						invokespecialMethod = thisType.BaseTypeWrapper.GetMethodWrapper(Name, Signature, true);
						if(invokespecialMethod != null)
						{
							invokespecialMethod.Link(mode);
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

			internal override void Link(TypeWrapper thisType, LoadMode mode)
			{
				base.Link(thisType, mode);
				TypeWrapper wrapper = GetClassType();
				if(wrapper != null)
				{
					if(!wrapper.IsUnloadable)
					{
						method = wrapper.GetInterfaceMethod(Name, Signature);
					}
					if(method == null)
					{
						// NOTE vmspec 5.4.3.4 clearly states that an interfacemethod may also refer to a method in Object
						method = CoreClasses.java.lang.Object.Wrapper.GetMethodWrapper(Name, Signature, false);
					}
					if(method != null)
					{
						method.Link(mode);
					}
				}
			}
		}

		private sealed class ConstantPoolItemFloat : ConstantPoolItem
		{
			internal float v;

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

			internal override object GetRuntimeValue()
			{
				return v;
			}
		}

		private sealed class ConstantPoolItemInteger : ConstantPoolItem
		{
			internal int v;

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

			internal override object GetRuntimeValue()
			{
				return v;
			}
		}

		private sealed class ConstantPoolItemLong : ConstantPoolItem
		{
			internal long l;

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

			internal override object GetRuntimeValue()
			{
				return l;
			}
		}

		private sealed class ConstantPoolItemNameAndType : ConstantPoolItem
		{
			internal ushort name_index;
			internal ushort descriptor_index;

			internal ConstantPoolItemNameAndType(BigEndianBinaryReader br)
			{
				name_index = br.ReadUInt16();
				descriptor_index = br.ReadUInt16();
			}

			internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
			{
				if(classFile.GetConstantPoolUtf8String(utf8_cp, name_index) == null
					|| classFile.GetConstantPoolUtf8String(utf8_cp, descriptor_index) == null)
				{
					throw new ClassFormatError("Illegal constant pool index");
				}
			}
		}

		internal sealed class ConstantPoolItemMethodHandle : ConstantPoolItem
		{
			private byte ref_kind;
			private ushort method_index;
			private ConstantPoolItemFMI cpi;

			internal ConstantPoolItemMethodHandle(BigEndianBinaryReader br)
			{
				ref_kind = br.ReadByte();
				method_index = br.ReadUInt16();
			}

			internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
			{
				switch ((RefKind)ref_kind)
				{
					case RefKind.getField:
					case RefKind.getStatic:
					case RefKind.putField:
					case RefKind.putStatic:
						cpi = classFile.GetConstantPoolItem(method_index) as ConstantPoolItemFieldref;
						break;
					case RefKind.invokeSpecial:
					case RefKind.invokeVirtual:
					case RefKind.invokeStatic:
					case RefKind.newInvokeSpecial:
						cpi = classFile.GetConstantPoolItem(method_index) as ConstantPoolItemMethodref;
						if (cpi == null && classFile.MajorVersion >= 52 && ((RefKind)ref_kind == RefKind.invokeStatic || (RefKind)ref_kind == RefKind.invokeSpecial))
							goto case RefKind.invokeInterface;
						break;
					case RefKind.invokeInterface:
						cpi = classFile.GetConstantPoolItem(method_index) as ConstantPoolItemInterfaceMethodref;
						break;
				}
				if (cpi == null)
				{
					throw new ClassFormatError("Invalid constant pool item MethodHandle");
				}
				if (ReferenceEquals(cpi.Name, StringConstants.INIT) && Kind != RefKind.newInvokeSpecial)
				{
					throw new ClassFormatError("Bad method name");
				}
			}

			internal override void MarkLinkRequired()
			{
				cpi.MarkLinkRequired();
			}

			internal string Class
			{
				get { return cpi.Class; }
			}

			internal string Name
			{
				get { return cpi.Name; }
			}

			internal string Signature
			{
				get { return cpi.Signature; }
			}

			internal ConstantPoolItemFMI MemberConstantPoolItem
			{
				get { return cpi; }
			}

			internal RefKind Kind
			{
				get { return (RefKind)ref_kind; }
			}

			internal MemberWrapper Member
			{
				get { return cpi.GetMember(); }
			}

			internal TypeWrapper GetClassType()
			{
				return cpi.GetClassType();
			}

			internal override void Link(TypeWrapper thisType, LoadMode mode)
			{
				cpi.Link(thisType, mode);
			}

			internal override ConstantType GetConstantType()
			{
				return ConstantType.MethodHandle;
			}
		}

		internal sealed class ConstantPoolItemMethodType : ConstantPoolItem
		{
			private ushort signature_index;
			private string descriptor;
			private TypeWrapper[] argTypeWrappers;
			private TypeWrapper retTypeWrapper;

			internal ConstantPoolItemMethodType(BigEndianBinaryReader br)
			{
				signature_index = br.ReadUInt16();
			}

			internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
			{
				string descriptor = classFile.GetConstantPoolUtf8String(utf8_cp, signature_index);
				if (descriptor == null || !IsValidMethodSig(descriptor))
				{
					throw new ClassFormatError("Invalid MethodType signature");
				}
				this.descriptor = String.Intern(descriptor.Replace('/', '.'));
			}

			internal override void Link(TypeWrapper thisType, LoadMode mode)
			{
				lock (this)
				{
					if (argTypeWrappers != null)
					{
						return;
					}
				}
				ClassLoaderWrapper classLoader = thisType.GetClassLoader();
				TypeWrapper[] args = classLoader.ArgTypeWrapperListFromSig(descriptor, mode);
				TypeWrapper ret = classLoader.RetTypeWrapperFromSig(descriptor, mode);
				lock (this)
				{
					if (argTypeWrappers == null)
					{
						argTypeWrappers = args;
						retTypeWrapper = ret;
					}
				}
			}

			internal string Signature
			{
				get { return descriptor; }
			}

			internal TypeWrapper[] GetArgTypes()
			{
				return argTypeWrappers;
			}

			internal TypeWrapper GetRetType()
			{
				return retTypeWrapper;
			}

			internal override ConstantType GetConstantType()
			{
				return ConstantType.MethodType;
			}
		}

		internal sealed class ConstantPoolItemInvokeDynamic : ConstantPoolItem
		{
			private ushort bootstrap_specifier_index;
			private ushort name_and_type_index;
			private string name;
			private string descriptor;
			private TypeWrapper[] argTypeWrappers;
			private TypeWrapper retTypeWrapper;

			internal ConstantPoolItemInvokeDynamic(BigEndianBinaryReader br)
			{
				bootstrap_specifier_index = br.ReadUInt16();
				name_and_type_index = br.ReadUInt16();
			}

			internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
			{
				ConstantPoolItemNameAndType name_and_type = (ConstantPoolItemNameAndType)classFile.GetConstantPoolItem(name_and_type_index);
				// if the constant pool items referred to were strings, GetConstantPoolItem returns null
				if (name_and_type == null)
				{
					throw new ClassFormatError("Bad index in constant pool");
				}
				name = String.Intern(classFile.GetConstantPoolUtf8String(utf8_cp, name_and_type.name_index));
				descriptor = String.Intern(classFile.GetConstantPoolUtf8String(utf8_cp, name_and_type.descriptor_index).Replace('/', '.'));
			}

			internal override void Link(TypeWrapper thisType, LoadMode mode)
			{
				lock (this)
				{
					if (argTypeWrappers != null)
					{
						return;
					}
				}
				ClassLoaderWrapper classLoader = thisType.GetClassLoader();
				TypeWrapper[] args = classLoader.ArgTypeWrapperListFromSig(descriptor, mode);
				TypeWrapper ret = classLoader.RetTypeWrapperFromSig(descriptor, mode);
				lock (this)
				{
					if (argTypeWrappers == null)
					{
						argTypeWrappers = args;
						retTypeWrapper = ret;
					}
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

			internal string Name
			{
				get { return name; }
			}

			internal string Signature
			{
				get { return descriptor; }
			}

			internal ushort BootstrapMethod
			{
				get { return bootstrap_specifier_index; }
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

			internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
			{
				s = classFile.GetConstantPoolUtf8String(utf8_cp, string_index);
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

		// this is only used to copy strings into "constantpool" when we see a RuntimeVisibleTypeAnnotations attribute,
		// because we need a consistent way of exposing constant pool items to the runtime and that case
		private sealed class ConstantPoolItemUtf8 : ConstantPoolItem
		{
			private readonly string str;

			internal ConstantPoolItemUtf8(string str)
			{
				this.str = str;
			}

			internal override object GetRuntimeValue()
			{
				return str;
			}
		}

		private sealed class ConstantPoolItemLiveObject : ConstantPoolItem
		{
			internal readonly object Value;

			internal ConstantPoolItemLiveObject(object value)
			{
				this.Value = value;
			}

			internal override ConstantType GetConstantType()
			{
				return ConstantType.LiveObject;
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
			NameAndType = 12,
			MethodHandle = 15,
			MethodType = 16,
			InvokeDynamic = 18,
		}

		internal abstract class FieldOrMethod : IEquatable<FieldOrMethod>
		{
			// Note that Modifiers is a ushort, so it combines nicely with the following ushort field
			protected Modifiers access_flags;
			protected ushort flags;
			private string name;
			private string descriptor;
			protected string signature;
			protected object[] annotations;
			protected byte[] runtimeVisibleTypeAnnotations;

			internal FieldOrMethod(ClassFile classFile, string[] utf8_cp, BigEndianBinaryReader br)
			{
				access_flags = (Modifiers)br.ReadUInt16();
				name = String.Intern(classFile.GetConstantPoolUtf8String(utf8_cp, br.ReadUInt16()));
				descriptor = classFile.GetConstantPoolUtf8String(utf8_cp, br.ReadUInt16());
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

			internal bool IsEnum
			{
				get
				{
					return (access_flags & Modifiers.Enum) != 0;
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

			internal byte[] RuntimeVisibleTypeAnnotations
			{
				get
				{
					return runtimeVisibleTypeAnnotations;
				}
			}

			public sealed override int GetHashCode()
			{
				return name.GetHashCode() ^ descriptor.GetHashCode();
			}

			public bool Equals(FieldOrMethod other)
			{
				return ReferenceEquals(name, other.name) && ReferenceEquals(descriptor, other.descriptor);
			}
		}

		internal sealed class Field : FieldOrMethod
		{
			private object constantValue;
			private string[] propertyGetterSetter;

			internal Field(ClassFile classFile, string[] utf8_cp, BigEndianBinaryReader br) : base(classFile, utf8_cp, br)
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
					switch(classFile.GetConstantPoolUtf8String(utf8_cp, br.ReadUInt16()))
					{
						case "Deprecated":
							if(br.ReadUInt32() != 0)
							{
								throw new ClassFormatError("Invalid Deprecated attribute length");
							}
							flags |= FLAG_MASK_DEPRECATED;
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
						case "Signature":
							if(classFile.MajorVersion < 49)
							{
								goto default;
							}
							if(br.ReadUInt32() != 2)
							{
								throw new ClassFormatError("Signature attribute has incorrect length");
							}
							signature = classFile.GetConstantPoolUtf8String(utf8_cp, br.ReadUInt16());
							break;
						case "RuntimeVisibleAnnotations":
							if(classFile.MajorVersion < 49)
							{
								goto default;
							}
							annotations = ReadAnnotations(br, classFile, utf8_cp);
							break;
						case "RuntimeInvisibleAnnotations":
							if(classFile.MajorVersion < 49)
							{
								goto default;
							}
							foreach(object[] annot in ReadAnnotations(br, classFile, utf8_cp))
							{
								if(annot[1].Equals("Likvm/lang/Property;"))
								{
									DecodePropertyAnnotation(classFile, annot);
								}
#if STATIC_COMPILER
								else if(annot[1].Equals("Likvm/lang/Internal;"))
								{
									this.access_flags &= ~Modifiers.AccessMask;
									flags |= FLAG_MASK_INTERNAL;
								}
#endif
							}
							break;
						case "RuntimeVisibleTypeAnnotations":
							if (classFile.MajorVersion < 52)
							{
								goto default;
							}
							classFile.CreateUtf8ConstantPoolItems(utf8_cp);
							runtimeVisibleTypeAnnotations = br.Section(br.ReadUInt32()).ToArray();
							break;
						default:
							br.Skip(br.ReadUInt32());
							break;
					}
				}
			}

			private void DecodePropertyAnnotation(ClassFile classFile, object[] annot)
			{
				if(propertyGetterSetter != null)
				{
					Tracer.Error(Tracer.ClassLoading, "Ignoring duplicate ikvm.lang.Property annotation on {0}.{1}", classFile.Name, this.Name);
					return;
				}
				propertyGetterSetter = new string[2];
				for(int i = 2; i < annot.Length - 1; i += 2)
				{
					string value = annot[i + 1] as string;
					if(value == null)
					{
						propertyGetterSetter = null;
						break;
					}
					if(annot[i].Equals("get") && propertyGetterSetter[0] == null)
					{
						propertyGetterSetter[0] = value;
					}
					else if(annot[i].Equals("set") && propertyGetterSetter[1] == null)
					{
						propertyGetterSetter[1] = value;
					}
					else
					{
						propertyGetterSetter = null;
						break;
					}
				}
				if(propertyGetterSetter == null || propertyGetterSetter[0] == null)
				{
					propertyGetterSetter = null;
					Tracer.Error(Tracer.ClassLoading, "Ignoring malformed ikvm.lang.Property annotation on {0}.{1}", classFile.Name, this.Name);
					return;
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

			internal void PatchConstantValue(object value)
			{
				constantValue = value;
			}

			internal bool IsStaticFinalConstant
			{
				get { return (access_flags & (Modifiers.Final | Modifiers.Static)) == (Modifiers.Final | Modifiers.Static) && constantValue != null; }
			}

			internal bool IsProperty
			{
				get
				{
					return propertyGetterSetter != null;
				}
			}

			internal string PropertyGetter
			{
				get
				{
					return propertyGetterSetter[0];
				}
			}

			internal string PropertySetter
			{
				get
				{
					return propertyGetterSetter[1];
				}
			}
		}

		internal sealed class Method : FieldOrMethod
		{
			private Code code;
			private string[] exceptions;
			private LowFreqData low;
			private MethodParametersEntry[] parameters;

			sealed class LowFreqData
			{
				internal object annotationDefault;
				internal object[][] parameterAnnotations;
#if STATIC_COMPILER
				internal string DllExportName;
				internal int DllExportOrdinal;
				internal string InterlockedCompareAndSetField;
#endif
			}

			internal Method(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options, BigEndianBinaryReader br) : base(classFile, utf8_cp, br)
			{
				// vmspec 4.6 says that all flags, except ACC_STRICT are ignored on <clinit>
				// however, since Java 7 it does need to be marked static
				if(ReferenceEquals(Name, StringConstants.CLINIT) && ReferenceEquals(Signature, StringConstants.SIG_VOID) && (classFile.MajorVersion < 51 || IsStatic))
				{
					access_flags &= Modifiers.Strictfp;
					access_flags |= (Modifiers.Static | Modifiers.Private);
				}
				else
				{
					// LAMESPEC: vmspec 4.6 says that abstract methods can not be strictfp (and this makes sense), but
					// javac (pre 1.5) is broken and marks abstract methods as strictfp (if you put the strictfp on the class)
					if((ReferenceEquals(Name, StringConstants.INIT) && (IsStatic || IsSynchronized || IsFinal || IsAbstract || IsNative))
						|| (IsPrivate && IsPublic) || (IsPrivate && IsProtected) || (IsPublic && IsProtected)
						|| (IsAbstract && (IsFinal || IsNative || IsPrivate || IsStatic || IsSynchronized))
						|| (classFile.IsInterface && classFile.MajorVersion <= 51 && (!IsPublic || IsFinal || IsNative || IsSynchronized || !IsAbstract))
						|| (classFile.IsInterface && classFile.MajorVersion >= 52 && (!(IsPublic || IsPrivate) || IsFinal || IsNative || IsSynchronized)))
					{
						throw new ClassFormatError("Method {0} in class {1} has illegal modifiers: 0x{2:X}", Name, classFile.Name, (int)access_flags);
					}
				}
				int attributes_count = br.ReadUInt16();
				for(int i = 0; i < attributes_count; i++)
				{
					switch(classFile.GetConstantPoolUtf8String(utf8_cp, br.ReadUInt16()))
					{
						case "Deprecated":
							if(br.ReadUInt32() != 0)
							{
								throw new ClassFormatError("Invalid Deprecated attribute length");
							}
							flags |= FLAG_MASK_DEPRECATED;
							break;
						case "Code":
						{
							if(!code.IsEmpty)
							{
								throw new ClassFormatError("{0} (Duplicate Code attribute)", classFile.Name);
							}
							BigEndianBinaryReader rdr = br.Section(br.ReadUInt32());
							code.Read(classFile, utf8_cp, this, rdr, options);
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
						case "Signature":
							if(classFile.MajorVersion < 49)
							{
								goto default;
							}
							if(br.ReadUInt32() != 2)
							{
								throw new ClassFormatError("Signature attribute has incorrect length");
							}
							signature = classFile.GetConstantPoolUtf8String(utf8_cp, br.ReadUInt16());
							break;
						case "RuntimeVisibleAnnotations":
							if(classFile.MajorVersion < 49)
							{
								goto default;
							}
							annotations = ReadAnnotations(br, classFile, utf8_cp);
							if ((options & ClassFileParseOptions.TrustedAnnotations) != 0)
							{
								foreach(object[] annot in annotations)
								{
									switch((string)annot[1])
									{
#if STATIC_COMPILER
										case "Lsun/reflect/CallerSensitive;":
											flags |= FLAG_CALLERSENSITIVE;
											break;
#endif
										case "Ljava/lang/invoke/LambdaForm$Compiled;":
											flags |= FLAG_LAMBDAFORM_COMPILED;
											break;
										case "Ljava/lang/invoke/LambdaForm$Hidden;":
											flags |= FLAG_LAMBDAFORM_HIDDEN;
											break;
										case "Ljava/lang/invoke/ForceInline;":
											flags |= FLAG_FORCEINLINE;
											break;
									}
								}
							}
							break;
						case "RuntimeVisibleParameterAnnotations":
						{
							if(classFile.MajorVersion < 49)
							{
								goto default;
							}
							if(low == null)
							{
								low = new LowFreqData();
							}
							BigEndianBinaryReader rdr = br.Section(br.ReadUInt32());
							byte num_parameters = rdr.ReadByte();
							low.parameterAnnotations = new object[num_parameters][];
							for(int j = 0; j < num_parameters; j++)
							{
								ushort num_annotations = rdr.ReadUInt16();
								low.parameterAnnotations[j] = new object[num_annotations];
								for(int k = 0; k < num_annotations; k++)
								{
									low.parameterAnnotations[j][k] = ReadAnnotation(rdr, classFile, utf8_cp);
								}
							}
							if(!rdr.IsAtEnd)
							{
								throw new ClassFormatError("{0} (RuntimeVisibleParameterAnnotations attribute has wrong length)", classFile.Name);
							}
							break;
						}
						case "AnnotationDefault":
						{
							if(classFile.MajorVersion < 49)
							{
								goto default;
							}
							if(low == null)
							{
								low = new LowFreqData();
							}
							BigEndianBinaryReader rdr = br.Section(br.ReadUInt32());
							low.annotationDefault = ReadAnnotationElementValue(rdr, classFile, utf8_cp);
							if(!rdr.IsAtEnd)
							{
								throw new ClassFormatError("{0} (AnnotationDefault attribute has wrong length)", classFile.Name);
							}
							break;
						}
#if STATIC_COMPILER
						case "RuntimeInvisibleAnnotations":
							if(classFile.MajorVersion < 49)
							{
								goto default;
							}
							foreach(object[] annot in ReadAnnotations(br, classFile, utf8_cp))
							{
								if(annot[1].Equals("Likvm/lang/Internal;"))
								{
									if (classFile.IsInterface)
									{
										StaticCompiler.IssueMessage(Message.InterfaceMethodCantBeInternal, classFile.Name, this.Name, this.Signature);
									}
									else
									{
										this.access_flags &= ~Modifiers.AccessMask;
										flags |= FLAG_MASK_INTERNAL;
									}
								}
								if(annot[1].Equals("Likvm/lang/DllExport;"))
								{
									string name = null;
									int? ordinal = null;
									for (int j = 2; j < annot.Length; j += 2)
									{
										if (annot[j].Equals("name") && annot[j + 1] is string)
										{
											name = (string)annot[j + 1];
										}
										else if (annot[j].Equals("ordinal") && annot[j + 1] is int)
										{
											ordinal = (int)annot[j + 1];
										}
									}
									if (name != null && ordinal != null)
									{
										if (!IsStatic)
										{
											StaticCompiler.IssueMessage(Message.DllExportMustBeStaticMethod, classFile.Name, this.Name, this.Signature);
										}
										else
										{
											if (low == null)
											{
												low = new LowFreqData();
											}
											low.DllExportName = name;
											low.DllExportOrdinal = ordinal.Value;
										}
									}
								}
								if(annot[1].Equals("Likvm/internal/InterlockedCompareAndSet;"))
								{
									string field = null;
									for (int j = 2; j < annot.Length; j += 2)
									{
										if (annot[j].Equals("value") && annot[j + 1] is string)
										{
											field = (string)annot[j + 1];
										}
									}
									if (field != null)
									{
										if (low == null)
										{
											low = new LowFreqData();
										}
										low.InterlockedCompareAndSetField = field;
									}
								}
							}
							break;
#endif
						case "MethodParameters":
						{
							if(classFile.MajorVersion < 52)
							{
								goto default;
							}
							if(parameters != null)
							{
								throw new ClassFormatError("{0} (Duplicate MethodParameters attribute)", classFile.Name);
							}
							parameters = ReadMethodParameters(br, utf8_cp);
							break;
						}
						case "RuntimeVisibleTypeAnnotations":
							if (classFile.MajorVersion < 52)
							{
								goto default;
							}
							classFile.CreateUtf8ConstantPoolItems(utf8_cp);
							runtimeVisibleTypeAnnotations = br.Section(br.ReadUInt32()).ToArray();
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
						throw new ClassFormatError("Code attribute in native or abstract methods in class file " + classFile.Name);
					}
				}
				else
				{
					if(code.IsEmpty)
					{
						if(ReferenceEquals(this.Name, StringConstants.CLINIT))
						{
							code.verifyError = string.Format("Class {0}, method {1} signature {2}: No Code attribute", classFile.Name, this.Name, this.Signature);
							return;
						}
						throw new ClassFormatError("Absent Code attribute in method that is not native or abstract in class file " + classFile.Name);
					}
				}
			}

			private static MethodParametersEntry[] ReadMethodParameters(BigEndianBinaryReader br, string[] utf8_cp)
			{
				uint length = br.ReadUInt32();
				if(length > 0)
				{
					BigEndianBinaryReader rdr = br.Section(length);
					byte parameters_count = rdr.ReadByte();
					if(length == 1 + parameters_count * 4)
					{
						MethodParametersEntry[] parameters = new MethodParametersEntry[parameters_count];
						for(int j = 0; j < parameters_count; j++)
						{
							ushort name = rdr.ReadUInt16();
							if(name >= utf8_cp.Length || (name != 0 && utf8_cp[name] == null))
							{
								return MethodParametersEntry.Malformed;
							}
							parameters[j].name = utf8_cp[name];
							parameters[j].flags = rdr.ReadUInt16();
						}
						return parameters;
					}
				}
				throw new ClassFormatError("Invalid MethodParameters method attribute length " + length + " in class file");
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

			internal bool IsVirtual
			{
				get
				{
					return (access_flags & (Modifiers.Static | Modifiers.Private)) == 0
						&& !IsConstructor;
				}
			}

			// Is this the <clinit>()V method?
			internal bool IsClassInitializer
			{
				get
				{
					return ReferenceEquals(Name, StringConstants.CLINIT) && ReferenceEquals(Signature, StringConstants.SIG_VOID) && IsStatic;
				}
			}

			internal bool IsConstructor
			{
				get
				{
					return ReferenceEquals(Name, StringConstants.INIT);
				}
			}

#if STATIC_COMPILER
			internal bool IsCallerSensitive
			{
				get
				{
					return (flags & FLAG_CALLERSENSITIVE) != 0;
				}
			}
#endif

			internal bool IsLambdaFormCompiled
			{
				get
				{
					return (flags & FLAG_LAMBDAFORM_COMPILED) != 0;
				}
			}

			internal bool IsLambdaFormHidden
			{
				get
				{
					return (flags & FLAG_LAMBDAFORM_HIDDEN) != 0;
				}
			}

			internal bool IsForceInline
			{
				get
				{
					return (flags & FLAG_FORCEINLINE) != 0;
				}
			}

			internal string[] ExceptionsAttribute
			{
				get
				{
					return exceptions;
				}
			}

			internal object[][] ParameterAnnotations
			{
				get
				{
					return low == null ? null : low.parameterAnnotations;
				}
			}

			internal object AnnotationDefault
			{
				get
				{
					return low == null ? null : low.annotationDefault;
				}
			}

#if STATIC_COMPILER
			internal string DllExportName
			{
				get
				{
					return low == null ? null : low.DllExportName;
				}
			}

			internal int DllExportOrdinal
			{
				get
				{
					return low == null ? -1 : low.DllExportOrdinal;
				}
			}

			internal string InterlockedCompareAndSetField
			{
				get
				{
					return low == null ? null : low.InterlockedCompareAndSetField;
				}
			}
#endif

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
				set
				{
					code.instructions = value;
				}
			}

			internal ExceptionTableEntry[] ExceptionTable
			{
				get
				{
					return code.exception_table;
				}
				set
				{
					code.exception_table = value;
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

			internal MethodParametersEntry[] MethodParameters
			{
				get
				{
					return parameters;
				}
			}

			internal bool MalformedMethodParameters
			{
				get
				{
					return parameters == MethodParametersEntry.Malformed;
				}
			}

			internal bool HasJsr
			{
				get
				{
					return code.hasJsr;
				}
			}

			private struct Code
			{
				internal bool hasJsr;
				internal string verifyError;
				internal ushort max_stack;
				internal ushort max_locals;
				internal Instruction[] instructions;
				internal ExceptionTableEntry[] exception_table;
				internal int[] argmap;
				internal LineNumberTableEntry[] lineNumberTable;
				internal LocalVariableTableEntry[] localVariableTable;

				internal void Read(ClassFile classFile, string[] utf8_cp, Method method, BigEndianBinaryReader br, ClassFileParseOptions options)
				{
					max_stack = br.ReadUInt16();
					max_locals = br.ReadUInt16();
					uint code_length = br.ReadUInt32();
					if(code_length == 0 || code_length > 65535)
					{
						throw new ClassFormatError("Invalid method Code length {1} in class file {0}", classFile.Name, code_length);
					}
					Instruction[] instructions = new Instruction[code_length + 1];
					int basePosition = br.Position;
					int instructionIndex = 0;
					try
					{
						BigEndianBinaryReader rdr = br.Section(code_length);
						while(!rdr.IsAtEnd)
						{
							instructions[instructionIndex].Read((ushort)(rdr.Position - basePosition), rdr, classFile);
							hasJsr |= instructions[instructionIndex].NormalizedOpCode == NormalizedByteCode.__jsr;
							instructionIndex++;
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
					// build the pcIndexMap
					int[] pcIndexMap = new int[this.instructions[instructionIndex - 1].PC + 1];
					for(int i = 0; i < pcIndexMap.Length; i++)
					{
						pcIndexMap[i] = -1;
					}
					for(int i = 0; i < instructionIndex - 1; i++)
					{
						pcIndexMap[this.instructions[i].PC] = i;
					}
					// convert branch offsets to indexes
					for(int i = 0; i < instructionIndex - 1; i++)
					{
						switch(this.instructions[i].NormalizedOpCode)
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
								this.instructions[i].SetTargetIndex(pcIndexMap[this.instructions[i].Arg1 + this.instructions[i].PC]);
								break;
							case NormalizedByteCode.__tableswitch:
							case NormalizedByteCode.__lookupswitch:
								this.instructions[i].MapSwitchTargets(pcIndexMap);
								break;
						}
					}
					// read exception table
					ushort exception_table_length = br.ReadUInt16();
					exception_table = new ExceptionTableEntry[exception_table_length];
					for(int i = 0; i < exception_table_length; i++)
					{
						ushort start_pc = br.ReadUInt16();
						ushort end_pc = br.ReadUInt16();
						ushort handler_pc = br.ReadUInt16();
						ushort catch_type = br.ReadUInt16();
						if(start_pc >= end_pc
							|| end_pc > code_length
							|| handler_pc >= code_length
							|| (catch_type != 0 && !classFile.SafeIsConstantPoolClass(catch_type)))
						{
							throw new ClassFormatError("Illegal exception table: {0}.{1}{2}", classFile.Name, method.Name, method.Signature);
						}
						classFile.MarkLinkRequiredConstantPoolItem(catch_type);
						// if start_pc, end_pc or handler_pc is invalid (i.e. doesn't point to the start of an instruction),
						// the index will be -1 and this will be handled by the verifier
						int startIndex = pcIndexMap[start_pc];
						int endIndex;
						if (end_pc == code_length)
						{
							// it is legal for end_pc to point to just after the last instruction,
							// but since there isn't an entry in our pcIndexMap for that, we have
							// a special case for this
							endIndex = instructionIndex - 1;
						}
						else
						{
							endIndex = pcIndexMap[end_pc];
						}
						int handlerIndex = pcIndexMap[handler_pc];
						exception_table[i] = new ExceptionTableEntry(startIndex, endIndex, handlerIndex, catch_type, i);
					}
					ushort attributes_count = br.ReadUInt16();
					for(int i = 0; i < attributes_count; i++)
					{
						switch(classFile.GetConstantPoolUtf8String(utf8_cp, br.ReadUInt16()))
						{
							case "LineNumberTable":
								if((options & ClassFileParseOptions.LineNumberTable) != 0)
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
								else
								{
									br.Skip(br.ReadUInt32());
								}
								break;
							case "LocalVariableTable":
								if((options & ClassFileParseOptions.LocalVariableTable) != 0)
								{
									BigEndianBinaryReader rdr = br.Section(br.ReadUInt32());
									int count = rdr.ReadUInt16();
									localVariableTable = new LocalVariableTableEntry[count];
									for(int j = 0; j < count; j++)
									{
										localVariableTable[j].start_pc = rdr.ReadUInt16();
										localVariableTable[j].length = rdr.ReadUInt16();
										localVariableTable[j].name = classFile.GetConstantPoolUtf8String(utf8_cp, rdr.ReadUInt16());
										localVariableTable[j].descriptor = classFile.GetConstantPoolUtf8String(utf8_cp, rdr.ReadUInt16()).Replace('/', '.');
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
					// build the argmap
					string sig = method.Signature;
					List<int> args = new List<int>();
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
					argmap = args.ToArray();
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
				internal readonly int startIndex;
				internal readonly int endIndex;
				internal readonly int handlerIndex;
				internal readonly ushort catch_type;
				internal readonly int ordinal;
				internal readonly bool isFinally;

				internal ExceptionTableEntry(int startIndex, int endIndex, int handlerIndex, ushort catch_type, int ordinal)
					: this(startIndex, endIndex, handlerIndex, catch_type, ordinal, false)
				{
				}

				internal ExceptionTableEntry(int startIndex, int endIndex, int handlerIndex, ushort catch_type, int ordinal, bool isFinally)
				{
					this.startIndex = startIndex;
					this.endIndex = endIndex;
					this.handlerIndex = handlerIndex;
					this.catch_type = catch_type;
					this.ordinal = ordinal;
					this.isFinally = isFinally;
				}
			}

			[Flags]
			internal enum InstructionFlags : byte
			{
				Reachable = 1,
				Processed = 2,
				BranchTarget = 4,
			}

			internal struct Instruction
			{
				private ushort pc;
				private NormalizedByteCode normopcode;
				private int arg1;
				private short arg2;
				private SwitchEntry[] switch_entries;

				struct SwitchEntry
				{
					internal int value;
					internal int target;
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

				internal int HandlerIndex
				{
					get { return (ushort)arg2; }
				}

				internal int HardErrorMessageId
				{
					get
					{
						return arg1;
					}
				}

				internal void PatchOpCode(NormalizedByteCode bc)
				{
					this.normopcode = bc;
				}

				internal void PatchOpCode(NormalizedByteCode bc, int arg1)
				{
					this.normopcode = bc;
					this.arg1 = arg1;
				}

				internal void PatchOpCode(NormalizedByteCode bc, int arg1, short arg2)
				{
					this.normopcode = bc;
					this.arg1 = arg1;
					this.arg2 = arg2;
				}

				internal void SetPC(int pc)
				{
					this.pc = (ushort)pc;
				}

				internal void SetTargetIndex(int targetIndex)
				{
					this.arg1 = targetIndex;
				}

				internal void SetTermNop(ushort pc)
				{
					// TODO what happens if we already have exactly the maximum number of instructions?
					this.pc = pc;
					this.normopcode = NormalizedByteCode.__nop;
				}

				internal void MapSwitchTargets(int[] pcIndexMap)
				{
					arg1 = pcIndexMap[arg1 + pc];
					for (int i = 0; i < switch_entries.Length; i++)
					{
						switch_entries[i].target = pcIndexMap[switch_entries[i].target + pc];
					}
				}

				internal void Read(ushort pc, BigEndianBinaryReader br, ClassFile classFile)
				{
					this.pc = pc;
					ByteCode bc = (ByteCode)br.ReadByte();
					switch(ByteCodeMetaData.GetMode(bc))
					{
						case ByteCodeMode.Simple:
							break;
						case ByteCodeMode.Constant_1:
							arg1 = br.ReadByte();
							classFile.MarkLinkRequiredConstantPoolItem(arg1);
							break;
						case ByteCodeMode.Local_1:
							arg1 = br.ReadByte();
							break;
						case ByteCodeMode.Constant_2:
							arg1 = br.ReadUInt16();
							classFile.MarkLinkRequiredConstantPoolItem(arg1);
							break;
						case ByteCodeMode.Branch_2:
							arg1 = br.ReadInt16();
							break;
						case ByteCodeMode.Branch_4:
							arg1 = br.ReadInt32();
							break;
						case ByteCodeMode.Constant_2_1_1:
							arg1 = br.ReadUInt16();
							classFile.MarkLinkRequiredConstantPoolItem(arg1);
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
							classFile.MarkLinkRequiredConstantPoolItem(arg1);
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
							for(int i = low; i < high; i++)
							{
								entries[i - low].value = i;
								entries[i - low].target = br.ReadInt32();
							}
							// do the last entry outside the loop, to avoid overflowing "i", if high == int.MaxValue
							entries[high - low].value = high;
							entries[high - low].target = br.ReadInt32();
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
								entries[i].target = br.ReadInt32();
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

				internal int TargetIndex
				{
					get
					{
						return arg1;
					}
					set
					{
						arg1 = value;
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

				internal int DefaultTarget
				{
					get
					{
						return arg1;
					}
					set
					{
						arg1 = value;
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

				internal int GetSwitchTargetIndex(int i)
				{
					return switch_entries[i].target;
				}

				internal void SetSwitchTargets(int[] targets)
				{
					SwitchEntry[] newEntries = (SwitchEntry[])switch_entries.Clone();
					for (int i = 0; i < newEntries.Length; i++)
					{
						newEntries[i].target = targets[i];
					}
					switch_entries = newEntries;
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
