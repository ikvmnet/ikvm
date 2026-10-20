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
using IKVM.ByteCode;
using IKVM.ByteCode.Decoding;
using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Linking
{

    internal sealed partial class Method<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> : FieldOrMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {

        Code<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> code;
        string[] exceptions;
        LowFreqData? low;
        MethodParametersEntry[] parameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="utf8_cp"></param>
        /// <param name="options"></param>
        /// <param name="reader"></param>
        /// <exception cref="ClassFormatException"></exception>
        internal Method(ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile, IKVM.ByteCode.Decoding.Method reader) :
            base(classFile, reader.AccessFlags, reader.Name, reader.Descriptor)
        {
            // vmspec 4.6 says that all flags, except ACC_STRICT are ignored on <clinit>
            // however, since Java 7 it does need to be marked static
            if (ReferenceEquals(Name, StringConstants.CLINIT) && ReferenceEquals(Signature, StringConstants.SIG_VOID) && (classFile.MajorVersion < 51 || IsStatic))
            {
                accessFlags &= ClassFileAccessFlags.Strictfp;
                accessFlags |= (ClassFileAccessFlags.Static | ClassFileAccessFlags.Private);
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
                    throw new ClassFormatException("Method {0} in class {1} has illegal modifiers: 0x{2:X}", Name, classFile.Name, (int)accessFlags);
                }
            }

            for (int i = 0; i < reader.Attributes.Count; i++)
            {
                var attribute = reader.Attributes[i];

                switch (classFile.GetConstantPoolUtf8String(attribute.Name))
                {
                    case AttributeName.Deprecated:
                        var deprecatedAttribute = (DeprecatedAttribute)attribute;
                        flags |= ClassFileFlags.MASK_DEPRECATED;
                        break;
                    case AttributeName.Code:
                        {
                            var codeAttribute = (CodeAttribute)attribute;
                            if (code.IsEmpty == false)
                                throw new ClassFormatException("{0} (Duplicate Code attribute)", classFile.Name);

                            code.Read(classFile, this, codeAttribute);
                            break;
                        }
                    case AttributeName.Exceptions:
                        {
                            if (exceptions != null)
                                throw new ClassFormatException("{0} (Duplicate Exceptions attribute)", classFile.Name);

                            var exceptionsAttribute = (ExceptionsAttribute)attribute;
                            exceptions = new string[exceptionsAttribute.Exceptions.Count];
                            for (int j = 0; j < exceptionsAttribute.Exceptions.Count; j++)
                                exceptions[j] = classFile.GetConstantPoolClass(exceptionsAttribute.Exceptions[j]);

                            break;
                        }
                    case AttributeName.Signature:
                        if (classFile.MajorVersion < 49)
                            goto default;

                        var signatureAttribute = (SignatureAttribute)attribute;
                        signature = classFile.GetConstantPoolUtf8String( signatureAttribute.Signature);
                        break;
                    case AttributeName.RuntimeVisibleAnnotations:
                        if (classFile.MajorVersion < 49)
                            goto default;

                        var runtimeVisibleAnnotationsAttribute = (RuntimeVisibleAnnotationsAttribute)attribute;
                        annotations = ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>.ReadAnnotations(runtimeVisibleAnnotationsAttribute.Annotations, classFile);
                        if ((classFile.Options & ClassFileParseOptions.TrustedAnnotations) != 0)
                        {
                            foreach (object[] annot in annotations)
                            {
                                switch ((string)annot[1])
                                {
                                    case "Lsun/reflect/CallerSensitive;" when ClassFile.Context.IsImporter:
                                        flags |= ClassFileFlags.CALLERSENSITIVE;
                                        break;
                                    case "Ljava/lang/invoke/LambdaForm$Compiled;":
                                        flags |= ClassFileFlags.LAMBDAFORM_COMPILED;
                                        break;
                                    case "Ljava/lang/invoke/LambdaForm$Hidden;":
                                        flags |= ClassFileFlags.LAMBDAFORM_HIDDEN;
                                        break;
                                    case "Ljava/lang/invoke/ForceInline;":
                                        flags |= ClassFileFlags.FORCEINLINE;
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
                                low.parameterAnnotations[j][k] = ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>.ReadAnnotation(parameter.Annotations[k], classFile);
                        }

                        break;
                    case AttributeName.AnnotationDefault:
                        if (classFile.MajorVersion < 49)
                            goto default;

                        var annotationDefaultAttribute = (AnnotationDefaultAttribute)attribute;
                        low ??= new LowFreqData();
                        low.annotationDefault = ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>.ReadAnnotationElementValue(annotationDefaultAttribute.DefaultValue, classFile);

                        break;
                    case AttributeName.RuntimeInvisibleAnnotations when ClassFile.Context.IsImporter:
                        if (classFile.MajorVersion < 49)
                            goto default;

                        var runtimeInvisibleAnnotationsAttribute = (RuntimeInvisibleAnnotationsAttribute)attribute;

                        foreach (object[] annot in ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>.ReadAnnotations(runtimeInvisibleAnnotationsAttribute.Annotations, classFile))
                        {
                            if (annot[1].Equals("Likvm/lang/Internal;"))
                            {
                                if (classFile.IsInterface)
                                {
                                    classFile.Context.Diagnostics.InterfaceMethodCantBeInternal(classFile.Name, Name, Signature);
                                }
                                else
                                {
                                    accessFlags &= ~ClassFileAccessFlags.AccessMask;
                                    flags |= ClassFileFlags.MASK_INTERNAL;
                                }
                            }
                            else if (annot[1].Equals("Likvm/internal/InterlockedCompareAndSet;"))
                            {
                                string? field = null;
                                for (int j = 2; j < annot.Length; j += 2)
                                    if (annot[j].Equals("value") && annot[j + 1] is string f)
                                        field = f;

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
                                    classFile.Context.Diagnostics.ModuleInitializerMethodRequirements(classFile.Name, Name, Signature);
                                }
                                else
                                {
                                    flags |= ClassFileFlags.MODULE_INITIALIZER;
                                }
                            }
                        }

                        break;
                    case AttributeName.MethodParameters:
                        if (classFile.MajorVersion < 52)
                            goto default;

                        if (parameters != null)
                            throw new ClassFormatException("{0} (Duplicate MethodParameters attribute)", classFile.Name);

                        var methodParametersAttribute = (MethodParametersAttribute)attribute;
                        parameters = ReadMethodParameters(methodParametersAttribute.Parameters);

                        break;
                    case AttributeName.RuntimeVisibleTypeAnnotations:
                        if (classFile.MajorVersion < 52)
                            goto default;

                        var runtimeVisibleTypeAnnotationsAttribute = (RuntimeVisibleTypeAnnotationsAttribute)attribute;
                        classFile.Constants.CreateUtf8ConstantPoolItems();
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
                    throw new ClassFormatException("Code attribute in native or abstract methods in class file " + classFile.Name);
                }
            }
            else
            {
                if (code.IsEmpty)
                {
                    if (ReferenceEquals(this.Name, StringConstants.CLINIT))
                    {
                        code._verifyError = string.Format("Class {0}, method {1} signature {2}: No Code attribute", classFile.Name, this.Name, this.Signature);
                        return;
                    }
                    throw new ClassFormatException("Absent Code attribute in method that is not native or abstract in class file " + classFile.Name);
                }
            }
        }

        MethodParametersEntry[] ReadMethodParameters(MethodParameterTable parameters)
        {
            var l = new MethodParametersEntry[parameters.Count];

            for (int i = 0; i < parameters.Count; i++)
            {
                var name = parameters[i].Name;
                if (name.Slot >= ClassFile.Constants.SlotCount || (name.IsNotNil && ClassFile.Constants.GetUtf8(name) == null))
                    return MethodParametersEntry.Malformed;

                l[i].name = ClassFile.Constants.GetUtf8(name);
                l[i].accessFlags = parameters[i].AccessFlags;
            }

            return l;
        }

        protected override void ValidateSig(ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile, string descriptor)
        {
            if (!ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>.IsValidMethodDescriptor(descriptor))
                throw new ClassFormatException("{0} (Method \"{1}\" has invalid signature \"{2}\")", classFile.Name, this.Name, descriptor);
        }

        internal bool IsStrictfp => (accessFlags & ClassFileAccessFlags.Strictfp) != 0;

        internal bool IsVirtual => (accessFlags & (ClassFileAccessFlags.Static | ClassFileAccessFlags.Private)) == 0 && !IsConstructor;

        // Is this the <clinit>()V method?
        internal bool IsClassInitializer => ReferenceEquals(Name, StringConstants.CLINIT) && ReferenceEquals(Signature, StringConstants.SIG_VOID) && IsStatic;

        internal bool IsConstructor => ReferenceEquals(Name, StringConstants.INIT);

        internal bool IsCallerSensitive => (flags & ClassFileFlags.CALLERSENSITIVE) != 0;

        internal bool IsLambdaFormCompiled => (flags & ClassFileFlags.LAMBDAFORM_COMPILED) != 0;

        internal bool IsLambdaFormHidden => (flags & ClassFileFlags.LAMBDAFORM_HIDDEN) != 0;

        internal bool IsForceInline => (flags & ClassFileFlags.FORCEINLINE) != 0;

        internal string[] ExceptionsAttribute => exceptions;

        internal object[][]? ParameterAnnotations => low == null ? null : low.parameterAnnotations;

        internal object? AnnotationDefault => low == null ? null : low.annotationDefault;

        internal string? InterlockedCompareAndSetField => low == null ? null : low.InterlockedCompareAndSetField;

        internal string VerifyError => code._verifyError;

        // maps argument 'slot' (as encoded in the xload/xstore instructions) into the ordinal
        internal int[] ArgMap => code._argmap;

        internal int MaxStack => code._maxStack;

        internal int MaxLocals => code._maxLocals;

        internal Instruction<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>[] Instructions
        {
            get => code._instructions;
            set => code._instructions = value;
        }

        internal ExceptionTableEntry[] ExceptionTable
        {
            get => code._exceptionTable;
            set => code._exceptionTable = value;
        }

        internal LineNumberTable LineNumberTable => code._lineNumberTable;

        internal LocalVariableTable LocalVariableTable => code._localVariableTable;

        internal MethodParametersEntry[] MethodParameters => parameters;

        internal bool MalformedMethodParameters => parameters == MethodParametersEntry.Malformed;

        internal bool HasJsr => code._hasJsr;

    }

}
