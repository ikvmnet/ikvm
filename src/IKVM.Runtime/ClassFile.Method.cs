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
using IKVM.ByteCode;
using IKVM.ByteCode.Decoding;
using IKVM.CoreLib.Diagnostics;


#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        internal sealed partial class Method : FieldOrMethod
        {

            Code code;
            string[] exceptions;
            LowFreqData low;
            MethodParametersEntry[] parameters;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="classFile"></param>
            /// <param name="utf8_cp"></param>
            /// <param name="options"></param>
            /// <param name="reader"></param>
            /// <exception cref="ClassFormatError"></exception>
            internal Method(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options, IKVM.ByteCode.Decoding.Method reader) :
                base(classFile, utf8_cp, reader.AccessFlags, reader.Name, reader.Descriptor)
            {
                // vmspec 4.6 says that all flags, except ACC_STRICT are ignored on <clinit>
                // however, since Java 7 it does need to be marked static
                if (ReferenceEquals(Name, StringConstants.CLINIT) && ReferenceEquals(Signature, StringConstants.SIG_VOID) && (classFile.MajorVersion < 51 || IsStatic))
                {
                    accessFlags &= Modifiers.Strictfp;
                    accessFlags |= (Modifiers.Static | Modifiers.Private);
                }
                else
                {
                    // LAMESPEC: vmspec 4.6 says that abstract methods can not be strictfp (and this makes sense), but
                    // javac (pre 1.5) is broken and marks abstract methods as strictfp (if you put the strictfp on the class)
                    if ((ReferenceEquals(Name, StringConstants.INIT) && (IsStatic || IsSynchronized || IsFinal || IsAbstract || IsNative))
                        || (IsPrivate && IsPublic) || (IsPrivate && IsProtected) || (IsPublic && IsProtected)
                        || (IsAbstract && (IsFinal || IsNative || IsPrivate || IsStatic || IsSynchronized))
                        || (classFile.IsInterface && classFile.MajorVersion <= 51 && (!IsPublic || IsFinal || IsNative || IsSynchronized || !IsAbstract))
                        || (classFile.IsInterface && classFile.MajorVersion >= 52 && (!(IsPublic || IsPrivate) || IsFinal || IsNative || IsSynchronized)))
                    {
                        throw new ClassFormatError("Method {0} in class {1} has illegal modifiers: 0x{2:X}", Name, classFile.Name, (int)accessFlags);
                    }
                }

                for (int i = 0; i < reader.Attributes.Count; i++)
                {
                    var attribute = reader.Attributes[i];

                    switch (classFile.GetConstantPoolUtf8String(utf8_cp, attribute.Name))
                    {
                        case AttributeName.Deprecated:
                            var deprecatedAttribute = (DeprecatedAttribute)attribute;
                            flags |= FLAG_MASK_DEPRECATED;
                            break;
                        case AttributeName.Code:
                            {
                                var codeAttribute = (CodeAttribute)attribute;
                                if (code.IsEmpty == false)
                                    throw new ClassFormatError("{0} (Duplicate Code attribute)", classFile.Name);

                                code.Read(classFile, utf8_cp, this, codeAttribute, options);
                                break;
                            }
                        case AttributeName.Exceptions:
                            {
                                if (exceptions != null)
                                    throw new ClassFormatError("{0} (Duplicate Exceptions attribute)", classFile.Name);

                                var exceptionsAttribute = (ExceptionsAttribute)attribute;
                                exceptions = new string[exceptionsAttribute.Exceptions.Count];
                                for (int j = 0; j < exceptionsAttribute.Exceptions.Count; j++)
                                    exceptions[j] = classFile.GetConstantPoolClass(exceptionsAttribute.Exceptions[j]);

                                break;
                            }
                        case AttributeName.Signature:
                            if (classFile.MajorVersion < 49)
                                goto default;

                            var signatureAttribute = (IKVM.ByteCode.Decoding.SignatureAttribute)attribute;
                            signature = classFile.GetConstantPoolUtf8String(utf8_cp, signatureAttribute.Signature);
                            break;
                        case AttributeName.RuntimeVisibleAnnotations:
                            if (classFile.MajorVersion < 49)
                                goto default;

                            var runtimeVisibleAnnotationsAttribute = (RuntimeVisibleAnnotationsAttribute)attribute;
                            annotations = ReadAnnotations(runtimeVisibleAnnotationsAttribute.Annotations, classFile, utf8_cp);
                            if ((options & ClassFileParseOptions.TrustedAnnotations) != 0)
                            {
                                foreach (object[] annot in annotations)
                                {
                                    switch ((string)annot[1])
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
                        case AttributeName.RuntimeVisibleParameterAnnotations:
                            if (classFile.MajorVersion < 49)
                                goto default;

                            var runtimeVisibleParameterAnnotationsAttribute = (RuntimeVisibleParameterAnnotationsAttribute)attribute;
                            low ??= new LowFreqData();
                            low.parameterAnnotations = new object[runtimeVisibleParameterAnnotationsAttribute.ParameterAnnotations.Count][];
                            for (int j = 0; j < runtimeVisibleParameterAnnotationsAttribute.ParameterAnnotations.Count; j++)
                            {
                                var parameter = runtimeVisibleParameterAnnotationsAttribute.ParameterAnnotations[j];
                                low.parameterAnnotations[j] = new object[parameter.Annotations.Count];
                                for (int k = 0; k < parameter.Annotations.Count; k++)
                                    low.parameterAnnotations[j][k] = ReadAnnotation(parameter.Annotations[k], classFile, utf8_cp);
                            }

                            break;
                        case AttributeName.AnnotationDefault:
                            if (classFile.MajorVersion < 49)
                                goto default;

                            var annotationDefaultAttribute = (IKVM.ByteCode.Decoding.AnnotationDefaultAttribute)attribute;
                            low ??= new LowFreqData();
                            low.annotationDefault = ReadAnnotationElementValue(annotationDefaultAttribute.DefaultValue, classFile, utf8_cp);

                            break;
#if IMPORTER
                        case AttributeName.RuntimeInvisibleAnnotations:
                            if (classFile.MajorVersion < 49)
                                goto default;

                            var runtimeInvisibleAnnotationsAttribute = (RuntimeInvisibleAnnotationsAttribute)attribute;

                            foreach (object[] annot in ReadAnnotations(runtimeInvisibleAnnotationsAttribute.Annotations, classFile, utf8_cp))
                            {
                                if (annot[1].Equals("Likvm/lang/Internal;"))
                                {
                                    if (classFile.IsInterface)
                                    {
                                        classFile.diagnostics.InterfaceMethodCantBeInternal(classFile.Name, Name, Signature);
                                    }
                                    else
                                    {
                                        accessFlags &= ~Modifiers.AccessMask;
                                        flags |= FLAG_MASK_INTERNAL;
                                    }
                                }
                                else if (annot[1].Equals("Likvm/internal/InterlockedCompareAndSet;"))
                                {
                                    string field = null;
                                    for (int j = 2; j < annot.Length; j += 2)
                                        if (annot[j].Equals("value") && annot[j + 1] is string)
                                            field = (string)annot[j + 1];

                                    if (field != null)
                                    {
                                        low ??= new LowFreqData();
                                        low.InterlockedCompareAndSetField = field;
                                    }
                                }
                                else if (annot[1].Equals("Likvm/lang/ModuleInitializer;"))
                                {
                                    if (classFile.IsInterface || IsConstructor || IsClassInitializer || IsPrivate || IsStatic == false)
                                    {
                                        classFile.diagnostics.ModuleInitializerMethodRequirements(classFile.Name, Name, Signature);
                                    }
                                    else
                                    {
                                        flags |= FLAG_MODULE_INITIALIZER;
                                    }
                                }
                            }

                            break;
#endif
                        case AttributeName.MethodParameters:
                            if (classFile.MajorVersion < 52)
                                goto default;

                            if (parameters != null)
                                throw new ClassFormatError("{0} (Duplicate MethodParameters attribute)", classFile.Name);

                            var methodParametersAttribute = (IKVM.ByteCode.Decoding.MethodParametersAttribute)attribute;
                            parameters = ReadMethodParameters(methodParametersAttribute.Parameters, utf8_cp);

                            break;
                        case AttributeName.RuntimeVisibleTypeAnnotations:
                            if (classFile.MajorVersion < 52)
                                goto default;

                            var runtimeVisibleTypeAnnotationsAttribute = (IKVM.ByteCode.Decoding.RuntimeVisibleTypeAnnotationsAttribute)attribute;
                            classFile.CreateUtf8ConstantPoolItems(utf8_cp);
                            runtimeVisibleTypeAnnotations = runtimeVisibleTypeAnnotationsAttribute.TypeAnnotations;
                            break;
                        default:
                            break;
                    }
                }
                if (IsAbstract || IsNative)
                {
                    if (!code.IsEmpty)
                    {
                        throw new ClassFormatError("Code attribute in native or abstract methods in class file " + classFile.Name);
                    }
                }
                else
                {
                    if (code.IsEmpty)
                    {
                        if (ReferenceEquals(this.Name, StringConstants.CLINIT))
                        {
                            code.verifyError = string.Format("Class {0}, method {1} signature {2}: No Code attribute", classFile.Name, this.Name, this.Signature);
                            return;
                        }
                        throw new ClassFormatError("Absent Code attribute in method that is not native or abstract in class file " + classFile.Name);
                    }
                }
            }

            private static MethodParametersEntry[] ReadMethodParameters(MethodParameterTable parameters, string[] utf8_cp)
            {
                var l = new MethodParametersEntry[parameters.Count];

                for (int i = 0; i < parameters.Count; i++)
                {
                    var name = parameters[i].Name;
                    if (name.Slot >= utf8_cp.Length || (name.IsNotNil && utf8_cp[name.Slot] == null))
                        return MethodParametersEntry.Malformed;

                    l[i].name = utf8_cp[name.Slot];
                    l[i].accessFlags = parameters[i].AccessFlags;
                }

                return l;
            }

            protected override void ValidateSig(ClassFile classFile, string descriptor)
            {
                if (!IsValidMethodSig(descriptor))
                {
                    throw new ClassFormatError("{0} (Method \"{1}\" has invalid signature \"{2}\")", classFile.Name, this.Name, descriptor);
                }
            }

            internal bool IsStrictfp => (accessFlags & Modifiers.Strictfp) != 0;

            internal bool IsVirtual => (accessFlags & (Modifiers.Static | Modifiers.Private)) == 0 && !IsConstructor;

            // Is this the <clinit>()V method?
            internal bool IsClassInitializer => ReferenceEquals(Name, StringConstants.CLINIT) && ReferenceEquals(Signature, StringConstants.SIG_VOID) && IsStatic;

            internal bool IsConstructor => ReferenceEquals(Name, StringConstants.INIT);

            internal bool IsCallerSensitive => (flags & FLAG_CALLERSENSITIVE) != 0;

            internal bool IsLambdaFormCompiled => (flags & FLAG_LAMBDAFORM_COMPILED) != 0;

            internal bool IsLambdaFormHidden => (flags & FLAG_LAMBDAFORM_HIDDEN) != 0;

            internal bool IsForceInline => (flags & FLAG_FORCEINLINE) != 0;

            internal string[] ExceptionsAttribute => exceptions;

            internal object[][] ParameterAnnotations => low == null ? null : low.parameterAnnotations;

            internal object AnnotationDefault => low == null ? null : low.annotationDefault;

#if IMPORTER

            internal string InterlockedCompareAndSetField => low == null ? null : low.InterlockedCompareAndSetField;

#endif

            internal string VerifyError => code.verifyError;

            // maps argument 'slot' (as encoded in the xload/xstore instructions) into the ordinal
            internal int[] ArgMap => code.argmap;

            internal int MaxStack => code.max_stack;

            internal int MaxLocals => code.max_locals;

            internal Instruction[] Instructions
            {
                get => code.instructions;
                set => code.instructions = value;
            }

            internal ExceptionTableEntry[] ExceptionTable
            {
                get => code.exception_table;
                set => code.exception_table = value;
            }

            internal LineNumberTableEntry[] LineNumberTableAttribute => code.lineNumberTable;

            internal LocalVariableTableEntry[] LocalVariableTableAttribute => code.localVariableTable;

            internal MethodParametersEntry[] MethodParameters => parameters;

            internal bool MalformedMethodParameters => parameters == MethodParametersEntry.Malformed;

            internal bool HasJsr => code.hasJsr;

        }

    }

}
