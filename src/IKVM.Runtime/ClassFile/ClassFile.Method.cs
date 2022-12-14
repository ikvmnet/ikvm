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
using IKVM.Attributes;

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Internal
{

    sealed partial class ClassFile
	{

        internal sealed partial class Method : FieldOrMethod
		{

			private Code code;
			private string[] exceptions;
			private LowFreqData low;
			private MethodParametersEntry[] parameters;

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
#if IMPORTER
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
#if IMPORTER
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

#if IMPORTER
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

#if IMPORTER
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
		}
	}

}
