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
using System.Linq;

using IKVM.Attributes;
using IKVM.ByteCode;
using IKVM.ByteCode.Decoding;
using IKVM.CoreLib.Diagnostics;

namespace IKVM.Runtime
{

    sealed partial class ClassFile : IDisposable
    {

        const ushort FLAG_MASK_DEPRECATED = 0x100;
        const ushort FLAG_MASK_INTERNAL = 0x200;
        const ushort FLAG_CALLERSENSITIVE = 0x400;
        const ushort FLAG_LAMBDAFORM_COMPILED = 0x800;
        const ushort FLAG_LAMBDAFORM_HIDDEN = 0x1000;
        const ushort FLAG_FORCEINLINE = 0x2000;
        const ushort FLAG_HAS_ASSERTIONS = 0x4000;
        const ushort FLAG_MODULE_INITIALIZER = 0x8000;

        readonly RuntimeContext context;
        readonly IDiagnosticHandler diagnostics;
        readonly IKVM.ByteCode.Decoding.ClassFile clazz;

        readonly ConstantPoolItem[] constantpool;
        readonly string[] utf8_cp;

        // Modifiers is a ushort, so the next four fields combine into two 32 bit slots
        Modifiers access_flags;
        ushort flags;
        readonly ConstantPoolItemClass[] interfaces;
        readonly Field[] fields;
        readonly Method[] methods;
        readonly string sourceFile;
#if IMPORTER
        string sourcePath;
#endif
        readonly string ikvmAssembly;
        readonly InnerClass[] innerClasses;
        readonly object[] annotations;
        readonly string signature;
        readonly string[] enclosingMethod;
        readonly BootstrapMethod[] bootstrapMethods;
        readonly TypeAnnotationTable runtimeVisibleTypeAnnotations = TypeAnnotationTable.Empty;

#if IMPORTER

        /// <summary>
        /// This method returns the class name, and whether or not the class is an IKVM stub.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="isstub"></param>
        /// <returns></returns>
        internal static string GetClassName(byte[] bytes, int offset, int length, out bool isstub)
        {
            try
            {
                using var clazz = IKVM.ByteCode.Decoding.ClassFile.Read(bytes.AsMemory(offset, length));
                return GetClassName(clazz, out isstub);
            }
            catch (UnsupportedClassVersionException e)
            {
                throw new UnsupportedClassVersionError(e.Message);
            }
            catch (ByteCodeException e)
            {
                throw new ClassFormatError(e.Message);
            }
        }

        /// <summary>
        /// This method returns the class name, and whether or not the class is an IKVM stub.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="isstub"></param>
        /// <returns></returns>
        /// <exception cref="UnsupportedClassVersionError"></exception>
        /// <exception cref="ClassFormatError"></exception>
        static string GetClassName(IKVM.ByteCode.Decoding.ClassFile reader, out bool isstub)
        {
            if (reader.Version < new ClassFormatVersion(45, 3) || reader.Version > 52)
                throw new UnsupportedClassVersionError(reader.Version);

            // this is a terrible way to go about encoding this information
            isstub = reader.Constants.Any(i => i.Kind == ConstantKind.Utf8 && reader.Constants.Get((Utf8ConstantHandle)i).Value == "IKVM.NET.Assembly");
            return string.Intern(reader.Constants.Get(reader.This).Name.Replace('/', '.'));
        }

#endif

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="clazz"></param>
        /// <param name="inputClassName"></param>
        /// <param name="options"></param>
        /// <param name="constantPoolPatches"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UnsupportedClassVersionError"></exception>
        /// <exception cref="ClassFormatError"></exception>
        internal ClassFile(RuntimeContext context, IDiagnosticHandler diagnostics, IKVM.ByteCode.Decoding.ClassFile clazz, string inputClassName, ClassFileParseOptions options, object[] constantPoolPatches)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
            this.clazz = clazz ?? throw new ArgumentNullException(nameof(clazz));

            try
            {
                if (clazz.Version < new ClassFormatVersion(45, 3) || clazz.Version > 52)
                    throw new UnsupportedClassVersionError(clazz.Version);

                // load a copy of the constant pool using our own custom class hierarchy, reading data from IKVM.ByteCdoe
                constantpool = new ConstantPoolItem[clazz.Constants.SlotCount];
                utf8_cp = new string[clazz.Constants.SlotCount];
                for (ushort i = 1; i < clazz.Constants.SlotCount; i++)
                {
                    switch (clazz.Constants.GetKind(new ConstantHandle(ConstantKind.Unknown, i)))
                    {
                        case ConstantKind.Unknown:
                            // longs and doubles can leave holes in the constant pool
                            break;
                        case ConstantKind.Class:
                            constantpool[i] = new ConstantPoolItemClass(context, clazz.Constants.Read(new ClassConstantHandle(i)));
                            break;
                        case ConstantKind.Double:
                            constantpool[i] = new ConstantPoolItemDouble(context, clazz.Constants.Read(new DoubleConstantHandle(i)));
                            break;
                        case ConstantKind.Fieldref:
                            constantpool[i] = new ConstantPoolItemFieldref(context, clazz.Constants.Read(new FieldrefConstantHandle(i)));
                            break;
                        case ConstantKind.Float:
                            constantpool[i] = new ConstantPoolItemFloat(context, clazz.Constants.Read(new FloatConstantHandle(i)));
                            break;
                        case ConstantKind.Integer:
                            constantpool[i] = new ConstantPoolItemInteger(context, clazz.Constants.Read(new IntegerConstantHandle(i)));
                            break;
                        case ConstantKind.InterfaceMethodref:
                            constantpool[i] = new ConstantPoolItemInterfaceMethodref(context, clazz.Constants.Read(new InterfaceMethodrefConstantHandle(i)));
                            break;
                        case ConstantKind.Long:
                            constantpool[i] = new ConstantPoolItemLong(context, clazz.Constants.Read(new LongConstantHandle(i)));
                            break;
                        case ConstantKind.Methodref:
                            constantpool[i] = new ConstantPoolItemMethodref(context, clazz.Constants.Read(new MethodrefConstantHandle(i)));
                            break;
                        case ConstantKind.NameAndType:
                            constantpool[i] = new ConstantPoolItemNameAndType(context, clazz.Constants.Read(new NameAndTypeConstantHandle(i)));
                            break;
                        case ConstantKind.MethodHandle:
                            if (clazz.Version < 51)
                                goto default;
                            constantpool[i] = new ConstantPoolItemMethodHandle(context, clazz.Constants.Read(new MethodHandleConstantHandle(i)));
                            break;
                        case ConstantKind.MethodType:
                            if (clazz.Version < 51)
                                goto default;
                            constantpool[i] = new ConstantPoolItemMethodType(context, clazz.Constants.Read(new MethodTypeConstantHandle(i)));
                            break;
                        case ConstantKind.InvokeDynamic:
                            if (clazz.Version < 51)
                                goto default;
                            constantpool[i] = new ConstantPoolItemInvokeDynamic(context, clazz.Constants.Read(new InvokeDynamicConstantHandle(i)));
                            break;
                        case ConstantKind.String:
                            constantpool[i] = new ConstantPoolItemString(context, clazz.Constants.Read(new StringConstantHandle(i)));
                            break;
                        case ConstantKind.Utf8:
                            utf8_cp[i] = clazz.Constants.Read(new Utf8ConstantHandle(i)).Value;
                            break;
                        default:
                            throw new ClassFormatError("Unknown constant type.");
                    }
                }

                if (constantPoolPatches != null)
                    PatchConstantPool(constantPoolPatches, utf8_cp, inputClassName);

                for (int i = 1; i < clazz.Constants.SlotCount; i++)
                {
                    if (constantpool[i] != null)
                    {
                        try
                        {
                            constantpool[i].Resolve(this, utf8_cp, options);
                        }
                        catch (ClassFormatError x)
                        {
                            // HACK at this point we don't yet have the class name, so any exceptions throw
                            // are missing the class name
                            throw new ClassFormatError("{0} ({1})", inputClassName, x.Message);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            throw new ClassFormatError("{0} (Invalid constant pool item #{1})", inputClassName, i);
                        }
                        catch (InvalidCastException)
                        {
                            throw new ClassFormatError("{0} (Invalid constant pool item #{1})", inputClassName, i);
                        }
                    }
                }

                access_flags = (Modifiers)clazz.AccessFlags;

                // NOTE although the vmspec says (in 4.1) that interfaces must be marked abstract, earlier versions of
                // javac (JDK 1.1) didn't do this, so the VM doesn't enforce this rule for older class files.
                // NOTE although the vmspec implies (in 4.1) that ACC_SUPER is illegal on interfaces, it doesn't enforce this
                // for older class files.
                // (See http://bugs.sun.com/bugdatabase/view_bug.do?bug_id=6320322)
                if ((IsInterface && IsFinal) || (IsAbstract && IsFinal) || (clazz.Version >= 49 && IsAnnotation && !IsInterface) || (clazz.Version >= 49 && IsInterface && (!IsAbstract || IsSuper || IsEnum)))
                    throw new ClassFormatError("{0} (Illegal class modifiers 0x{1:X})", inputClassName, access_flags);

                ValidateConstantPoolItemClass(inputClassName, clazz.This);
                ValidateConstantPoolItemClass(inputClassName, clazz.Super);

                if (IsInterface && (clazz.Super.IsNil || SuperClass.Name != "java.lang.Object"))
                    throw new ClassFormatError("{0} (Interfaces must have java.lang.Object as superclass)", Name);

                // most checks are already done by ConstantPoolItemClass.Resolve, but since it allows
                // array types, we do need to check for that
                if (Name[0] == '[')
                    throw new ClassFormatError("Bad name");

                interfaces = new ConstantPoolItemClass[clazz.Interfaces.Count];
                for (int i = 0; i < interfaces.Length; i++)
                {
                    var handle = clazz.Interfaces[i].Class;
                    if (handle.IsNil || handle.Slot >= constantpool.Length)
                        throw new ClassFormatError("{0} (Illegal constant pool index)", Name);

                    var cpi = constantpool[handle.Slot] as ConstantPoolItemClass;
                    if (cpi == null)
                        throw new ClassFormatError("{0} (Interface name has bad constant type)", Name);

                    interfaces[i] = cpi;
                }

                CheckDuplicates(interfaces, "Repetitive interface name");

                fields = new Field[clazz.Fields.Count];
                for (int i = 0; i < clazz.Fields.Count; i++)
                {
                    fields[i] = new Field(this, utf8_cp, clazz.Fields[i]);
                    var name = fields[i].Name;

                    if (!IsValidFieldName(name, clazz.Version))
                        throw new ClassFormatError("{0} (Illegal field name \"{1}\")", Name, name);
                }

                CheckDuplicates<FieldOrMethod>(fields, "Repetitive field name/signature");

                methods = new Method[clazz.Methods.Count];
                for (int i = 0; i < clazz.Methods.Count; i++)
                {
                    methods[i] = new Method(this, utf8_cp, options, clazz.Methods[i]);
                    string name = methods[i].Name;
                    string sig = methods[i].Signature;
                    if (!IsValidMethodName(name, clazz.Version))
                    {
                        if (!ReferenceEquals(name, StringConstants.INIT) && !ReferenceEquals(name, StringConstants.CLINIT))
                            throw new ClassFormatError("{0} (Illegal method name \"{1}\")", Name, name);
                        if (!sig.EndsWith("V"))
                            throw new ClassFormatError("{0} (Method \"{1}\" has illegal signature \"{2}\")", Name, name, sig);
                        if ((options & ClassFileParseOptions.RemoveAssertions) != 0 && methods[i].IsClassInitializer)
                            RemoveAssertionInit(methods[i]);
                    }
                }

                CheckDuplicates<FieldOrMethod>(methods, "Repetitive method name/signature");

                for (int i = 0; i < clazz.Attributes.Count; i++)
                {
                    var attribute = clazz.Attributes[i];

                    switch (GetConstantPoolUtf8String(utf8_cp, attribute.Name))
                    {
                        case AttributeName.Deprecated:
                            var deprecatedAttribute = (DeprecatedAttribute)attribute;
                            flags |= FLAG_MASK_DEPRECATED;
                            break;
                        case AttributeName.SourceFile:
                            var sourceFileAttribute = (IKVM.ByteCode.Decoding.SourceFileAttribute)attribute;
                            sourceFile = GetConstantPoolUtf8String(utf8_cp, sourceFileAttribute.SourceFile);
                            break;
                        case AttributeName.InnerClasses:
                            if (MajorVersion < 49)
                                goto default;

                            var innerClassesAttribute = (InnerClassesAttribute)attribute;
                            innerClasses = new InnerClass[innerClassesAttribute.Table.Count];
                            for (int j = 0; j < innerClasses.Length; j++)
                            {
                                var item = innerClassesAttribute.Table[j];

                                innerClasses[j].innerClass = item.Inner;
                                innerClasses[j].outerClass = item.Outer;
                                innerClasses[j].name = item.InnerName;
                                innerClasses[j].accessFlags = (Modifiers)item.InnerAccessFlags;

                                if (innerClasses[j].innerClass.IsNotNil && !(GetConstantPoolItem(innerClasses[j].innerClass) is ConstantPoolItemClass))
                                    throw new ClassFormatError("{0} (inner_class_info_index has bad constant pool index)", this.Name);

                                if (innerClasses[j].outerClass.IsNotNil && !(GetConstantPoolItem(innerClasses[j].outerClass) is ConstantPoolItemClass))
                                    throw new ClassFormatError("{0} (outer_class_info_index has bad constant pool index)", this.Name);

                                if (innerClasses[j].name.IsNotNil && utf8_cp[innerClasses[j].name.Slot] == null)
                                    throw new ClassFormatError("{0} (inner class name has bad constant pool index)", this.Name);

                                if (innerClasses[j].innerClass == innerClasses[j].outerClass)
                                    throw new ClassFormatError("{0} (Class is both inner and outer class)", this.Name);

                                if (innerClasses[j].innerClass.IsNotNil && innerClasses[j].outerClass.IsNotNil)
                                {
                                    MarkLinkRequiredConstantPoolItem(innerClasses[j].innerClass);
                                    MarkLinkRequiredConstantPoolItem(innerClasses[j].outerClass);
                                }
                            }

                            break;
                        case AttributeName.Signature:
                            if (clazz.Version < 49)
                                goto default;

                            var signatureAttribute = (IKVM.ByteCode.Decoding.SignatureAttribute)attribute;
                            signature = GetConstantPoolUtf8String(utf8_cp, signatureAttribute.Signature);
                            break;
                        case AttributeName.EnclosingMethod:
                            if (clazz.Version < 49)
                                goto default;

                            var enclosingMethodAttribute = (IKVM.ByteCode.Decoding.EnclosingMethodAttribute)attribute;
                            var classHandle = enclosingMethodAttribute.Class;
                            var methodHandle = enclosingMethodAttribute.Method;
                            ValidateConstantPoolItemClass(inputClassName, classHandle);

                            if (methodHandle.IsNil)
                            {
                                enclosingMethod =
                                [
                                    GetConstantPoolClass(classHandle),
                                    null,
                                    null
                                ];
                            }
                            else
                            {
                                if (GetConstantPoolItem(methodHandle) is not ConstantPoolItemNameAndType m)
                                    throw new ClassFormatError("{0} (Bad constant pool index #{1})", inputClassName, methodHandle);

                                enclosingMethod = new string[]
                                {
                                    GetConstantPoolClass(classHandle),
                                    GetConstantPoolUtf8String(utf8_cp, m.NameHandle),
                                    GetConstantPoolUtf8String(utf8_cp, m.DescriptorHandle).Replace('/', '.')
                                };
                            }

                            break;
                        case AttributeName.RuntimeVisibleAnnotations:
                            if (clazz.Version < 49)
                                goto default;

                            var runtimeVisibleAnnotationsAttribute = (RuntimeVisibleAnnotationsAttribute)attribute;
                            annotations = ReadAnnotations(runtimeVisibleAnnotationsAttribute.Annotations, this, utf8_cp);
                            break;
#if IMPORTER
                        case AttributeName.RuntimeInvisibleAnnotations:
                            if (clazz.Version < 49)
                                goto default;

                            var runtimeInvisibleAnnotationsAttribute = (RuntimeInvisibleAnnotationsAttribute)attribute;
                            foreach (var annot in ReadAnnotations(runtimeInvisibleAnnotationsAttribute.Annotations, this, utf8_cp))
                            {
                                if (annot[1].Equals("Likvm/lang/Internal;"))
                                {
                                    access_flags &= ~Modifiers.AccessMask;
                                    flags |= FLAG_MASK_INTERNAL;
                                }
                            }

                            break;
#endif
                        case AttributeName.BootstrapMethods:
                            if (clazz.Version < 51)
                                goto default;

                            var bootstrapMethodsAttribute = (BootstrapMethodsAttribute)attribute;
                            bootstrapMethods = ReadBootstrapMethods(bootstrapMethodsAttribute.Methods, this);
                            break;
                        case AttributeName.RuntimeVisibleTypeAnnotations:
                            if (clazz.Version < 52)
                                goto default;

                            var _runtimeVisibleTypeAnnotations = (IKVM.ByteCode.Decoding.RuntimeVisibleTypeAnnotationsAttribute)attribute;
                            CreateUtf8ConstantPoolItems(utf8_cp);
                            runtimeVisibleTypeAnnotations = _runtimeVisibleTypeAnnotations.TypeAnnotations;
                            break;
                        case "IKVM.NET.Assembly":
                            if (attribute.Data.Length != 2)
                                throw new ClassFormatError("IKVM.NET.Assembly attribute has incorrect length");

                            var r = new ClassFormatReader(attribute.Data);
                            if (r.TryReadU2(out var index) == false)
                                throw new ClassFormatError("IKVM.NET.Assembly attribute has incorrect length");

                            ikvmAssembly = GetConstantPoolUtf8String(utf8_cp, new(index));
                            break;
                        default:
                            break;
                    }
                }

                // validate the invokedynamic entries to point into the bootstrapMethods array
                for (int i = 1; i < constantpool.Length; i++)
                    if (constantpool[i] != null && constantpool[i] is ConstantPoolItemInvokeDynamic cpi)
                        if (bootstrapMethods == null || cpi.BootstrapMethod >= bootstrapMethods.Length)
                            throw new ClassFormatError("Short length on BootstrapMethods in class file");
            }
            catch (OverflowException)
            {
                throw new ClassFormatError("Truncated class file (or section)");
            }
            catch (IndexOutOfRangeException)
            {
                throw new ClassFormatError("Unspecified class file format error");
            }
            catch (ByteCodeException)
            {
                throw new ClassFormatError("Unspecified class file format error");
            }
        }

        void CreateUtf8ConstantPoolItems(string[] utf8_cp)
        {
            for (int i = 0; i < constantpool.Length; i++)
                if (constantpool[i] == null && utf8_cp[i] != null)
                    constantpool[i] = new ConstantPoolItemUtf8(context, utf8_cp[i]);
        }

        void CheckDuplicates<T>(T[] members, string msg)
            where T : IEquatable<T>
        {
            if (members.Length < 100)
            {
                for (int i = 0; i < members.Length; i++)
                    for (int j = 0; j < i; j++)
                        if (members[i].Equals(members[j]))
                            throw new ClassFormatError("{0} ({1})", Name, msg);
            }
            else
            {
                var hs = new HashSet<T>();
                for (int i = 0; i < members.Length; i++)
                    if (hs.Add(members[i]) == false)
                        throw new ClassFormatError("{0} ({1})", Name, msg);
            }
        }

        void PatchConstantPool(object[] constantPoolPatches, string[] utf8_cp, string inputClassName)
        {
#if !IMPORTER && !FIRST_PASS
            for (int i = 0; i < constantPoolPatches.Length; i++)
            {
                if (constantPoolPatches[i] != null)
                {
                    if (utf8_cp[i] != null)
                    {
                        if (!(constantPoolPatches[i] is string))
                            throw new ClassFormatError("Illegal utf8 patch at {0} in class file {1}", i, inputClassName);

                        utf8_cp[i] = (string)constantPoolPatches[i];
                    }
                    else if (constantpool[i] != null)
                    {
                        switch (constantpool[i].GetConstantType())
                        {
                            case ConstantType.String:
                                constantpool[i] = new ConstantPoolItemLiveObject(context, constantPoolPatches[i]);
                                break;
                            case ConstantType.Class:
                                java.lang.Class clazz;
                                string name;
                                if ((clazz = constantPoolPatches[i] as java.lang.Class) != null)
                                {
                                    var tw = RuntimeJavaType.FromClass(clazz);
                                    constantpool[i] = new ConstantPoolItemClass(context, tw.Name, tw);
                                }
                                else if ((name = constantPoolPatches[i] as string) != null)
                                {
                                    constantpool[i] = new ConstantPoolItemClass(context, string.Intern(name.Replace('/', '.')), null);
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

        void MarkLinkRequiredConstantPoolItem(ConstantHandle handle)
        {
            if (handle.Slot > 0 && handle.Slot < constantpool.Length && constantpool[handle.Slot] != null)
                constantpool[handle.Slot].MarkLinkRequired();
        }

        void MarkLinkRequiredConstantPoolItem(int index)
        {
            MarkLinkRequiredConstantPoolItem(new ConstantHandle(ConstantKind.Unknown, checked((ushort)index)));
        }

        static BootstrapMethod[] ReadBootstrapMethods(BootstrapMethodTable methods, ClassFile classFile)
        {
            var bsm = new BootstrapMethod[methods.Count];
            for (int i = 0; i < methods.Count; i++)
            {
                var method = methods[i];

                var bsm_index = method.Method;
                if (bsm_index.Slot >= classFile.constantpool.Length || classFile.constantpool[bsm_index.Slot] is not ConstantPoolItemMethodHandle)
                    throw new ClassFormatError("bootstrap_method_index {0} has bad constant type in class file {1}", bsm_index, classFile.Name);

                classFile.MarkLinkRequiredConstantPoolItem(bsm_index);

                var argument_count = method.Arguments.Count;
                var args = new ConstantHandle[argument_count];
                for (int j = 0; j < args.Length; j++)
                {
                    var argument_index = method.Arguments[j];
                    if (classFile.IsValidConstant(argument_index) == false)
                        throw new ClassFormatError("argument_index {0} has bad constant type in class file {1}", argument_index, classFile.Name);

                    classFile.MarkLinkRequiredConstantPoolItem(argument_index);
                    args[j] = argument_index;
                }

                bsm[i] = new BootstrapMethod(bsm_index, args);
            }

            return bsm;
        }

        bool IsValidConstant(ConstantHandle handle)
        {
            if (handle.Slot < constantpool.Length && constantpool[handle.Slot] != null)
            {
                try
                {
                    constantpool[handle.Slot].GetConstantType();
                    return true;
                }
                catch (InvalidOperationException)
                {

                }
            }

            return false;
        }

        static object[][] ReadAnnotations(AnnotationTable reader, ClassFile classFile, string[] utf8_cp)
        {
            var annotations = new object[reader.Count][];

            for (int i = 0; i < annotations.Length; i++)
                annotations[i] = ReadAnnotation(reader[i], classFile, utf8_cp);

            return annotations;
        }

        static object[] ReadAnnotation(IKVM.ByteCode.Decoding.Annotation annotation, ClassFile classFile, string[] utf8_cp)
        {
            var l = new object[2 + annotation.Elements.Count * 2];
            l[0] = IKVM.Attributes.AnnotationDefaultAttribute.TAG_ANNOTATION;
            l[1] = classFile.GetConstantPoolUtf8String(utf8_cp, annotation.Type);
            for (int i = 0; i < annotation.Elements.Count; i++)
            {
                l[2 + i * 2 + 0] = classFile.GetConstantPoolUtf8String(utf8_cp, annotation.Elements[i].Name);
                l[2 + i * 2 + 1] = ReadAnnotationElementValue(annotation.Elements[i].Value, classFile, utf8_cp);
            }

            return l;
        }

        static object ReadAnnotationElementValue(in ElementValue reader, ClassFile classFile, string[] utf8_cp)
        {
            try
            {
                switch (reader.Kind)
                {
                    case ElementValueKind.Boolean:
                        return classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)((ConstantElementValue)reader).Handle) != 0;
                    case ElementValueKind.Byte:
                        return (byte)classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)((ConstantElementValue)reader).Handle);
                    case ElementValueKind.Char:
                        return (char)classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)((ConstantElementValue)reader).Handle);
                    case ElementValueKind.Short:
                        return (short)classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)((ConstantElementValue)reader).Handle);
                    case ElementValueKind.Integer:
                        return classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)((ConstantElementValue)reader).Handle);
                    case ElementValueKind.Float:
                        return classFile.GetConstantPoolConstantFloat((FloatConstantHandle)((ConstantElementValue)reader).Handle);
                    case ElementValueKind.Long:
                        return classFile.GetConstantPoolConstantLong((LongConstantHandle)((ConstantElementValue)reader).Handle);
                    case ElementValueKind.Double:
                        return classFile.GetConstantPoolConstantDouble((DoubleConstantHandle)((ConstantElementValue)reader).Handle);
                    case ElementValueKind.String:
                        return classFile.GetConstantPoolUtf8String(utf8_cp, (Utf8ConstantHandle)((ConstantElementValue)reader).Handle);
                    case ElementValueKind.Enum:
                        var _enum = (EnumElementValue)reader;
                        return new object[] {
                            IKVM.Attributes.AnnotationDefaultAttribute.TAG_ENUM,
                            classFile.GetConstantPoolUtf8String(utf8_cp, _enum.TypeName),
                            classFile.GetConstantPoolUtf8String(utf8_cp, _enum.ConstantName)
                        };
                    case ElementValueKind.Class:
                        var _class = (ClassElementValue)reader;
                        return new object[] {
                            IKVM.Attributes.AnnotationDefaultAttribute.TAG_CLASS,
                            classFile.GetConstantPoolUtf8String(utf8_cp, _class.Class)
                        };
                    case ElementValueKind.Annotation:
                        return ReadAnnotation(((AnnotationElementValue)reader).Annotation, classFile, utf8_cp);
                    case ElementValueKind.Array:
                        var _array = (ArrayElementValue)reader;

                        var array = new object[_array.Count + 1];
                        array[0] = IKVM.Attributes.AnnotationDefaultAttribute.TAG_ARRAY;
                        for (int i = 0; i < _array.Count; i++)
                            array[i + 1] = ReadAnnotationElementValue(_array[i], classFile, utf8_cp);

                        return array;
                    default:
                        throw new ClassFormatError("Invalid tag {0} in annotation element_value", reader.Kind);
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
            catch (ByteCodeException)
            {

            }

            return new object[] { IKVM.Attributes.AnnotationDefaultAttribute.TAG_ERROR, "java.lang.IllegalArgumentException", "Wrong type at constant pool index" };
        }

        void ValidateConstantPoolItemClass(string classFile, ClassConstantHandle handle)
        {
            if (handle.Slot >= constantpool.Length || constantpool[handle.Slot] is not ConstantPoolItemClass)
                throw new ClassFormatError("{0} (Bad constant pool index #{1})", classFile, handle);
        }

        static bool IsValidMethodName(string name, ClassFormatVersion version)
        {
            if (name.Length == 0)
                return false;

            for (int i = 0; i < name.Length; i++)
                if (".;[/<>".IndexOf(name[i]) != -1)
                    return false;

            return version >= 49 || IsValidPre49Identifier(name);
        }

        static bool IsValidFieldName(string name, ClassFormatVersion version)
        {
            if (name.Length == 0)
                return false;

            for (int i = 0; i < name.Length; i++)
                if (".;[/".IndexOf(name[i]) != -1)
                    return false;

            return version >= 49 || IsValidPre49Identifier(name);
        }

        static bool IsValidPre49Identifier(string name)
        {
            if (!char.IsLetter(name[0]) && "$_".IndexOf(name[0]) == -1)
                return false;

            for (int i = 1; i < name.Length; i++)
                if (!char.IsLetterOrDigit(name[i]) && "$_".IndexOf(name[i]) == -1)
                    return false;

            return true;
        }

        internal static bool IsValidFieldSig(string sig)
        {
            return IsValidFieldSigImpl(sig, 0, sig.Length);
        }

        static bool IsValidFieldSigImpl(string sig, int start, int end)
        {
            if (start >= end)
                return false;

            switch (sig[start])
            {
                case 'L':
                    return sig.IndexOf(';', start + 1) == end - 1;
                case '[':
                    while (sig[start] == '[')
                    {
                        start++;
                        if (start == end)
                            return false;
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
            if (sig.Length < 3 || sig[0] != '(')
                return false;

            int end = sig.IndexOf(')');
            if (end == -1)
                return false;

            if (!sig.EndsWith(")V") && !IsValidFieldSigImpl(sig, end + 1, sig.Length))
                return false;

            for (int i = 1; i < end; i++)
            {
                switch (sig[i])
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
                        while (sig[i] == '[')
                            i++;
                        if ("BZCSIJFDL".IndexOf(sig[i]) == -1)
                            return false;
                        if (sig[i] == 'L')
                            i = sig.IndexOf(';', i);
                        break;
                    default:
                        return false;
                }

                if (i == -1 || i >= end)
                    return false;
            }

            return true;
        }

        internal int MajorVersion => clazz.Version.Major;

        internal void Link(RuntimeJavaType thisType, LoadMode mode)
        {
            // this is not just an optimization, it's required for anonymous classes to be able to refer to themselves
            ((ConstantPoolItemClass)constantpool[clazz.This.Slot]).LinkSelf(thisType);

            for (int i = 1; i < constantpool.Length; i++)
                if (constantpool[i] != null)
                    constantpool[i].Link(thisType, mode);
        }

        internal Modifiers Modifiers => access_flags;

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// interfaces are implicitly abstract
        /// </remarks>
        internal bool IsAbstract => (access_flags & (Modifiers.Abstract | Modifiers.Interface)) != 0;

        internal bool IsFinal => (access_flags & Modifiers.Final) != 0;

        internal bool IsPublic => (access_flags & Modifiers.Public) != 0;

        internal bool IsInterface => (access_flags & Modifiers.Interface) != 0;

        internal bool IsEnum => (access_flags & Modifiers.Enum) != 0;

        internal bool IsAnnotation => (access_flags & Modifiers.Annotation) != 0;

        internal bool IsSuper => (access_flags & Modifiers.Super) != 0;

        internal bool IsReferenced(Field fld) => constantpool.OfType<ConstantPoolItemFieldref>().Any(i => i.Class == Name && i.Name == fld.Name && i.Signature == fld.Signature);

        internal ConstantPoolItemFieldref GetFieldref(FieldrefConstantHandle handle)
        {
            return (ConstantPoolItemFieldref)constantpool[handle.Slot];
        }
        internal ConstantPoolItemFieldref GetFieldref(int slot)
        {
            return GetFieldref(new FieldrefConstantHandle(checked((ushort)slot)));
        }

        /// <summary>
        /// Version of GetFieldref that does not throw if the handle is invalid. Used by IsSideEffectFreeStaticInitializer.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal ConstantPoolItemFieldref SafeGetFieldref(ConstantHandle handle)
        {
            if (handle.IsNotNil && handle.Slot < constantpool.Length)
                return constantpool[handle.Slot] as ConstantPoolItemFieldref;

            return null;
        }

        /// <summary>
        /// Version of GetFieldref that does not throw if the handle is invalid. Used by IsSideEffectFreeStaticInitializer.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal ConstantPoolItemFieldref SafeGetFieldref(int index)
        {
            if (index > ushort.MaxValue || index < ushort.MinValue)
                return null;

            return SafeGetFieldref(new ConstantHandle(ConstantKind.Unknown, (ushort)index));
        }

        internal ConstantPoolItemMI GetMethodref(MethodrefConstantHandle handle)
        {
            return (ConstantPoolItemMI)constantpool[handle.Slot];
        }

        // NOTE this returns an MI, because it used for both normal methods and interface methods
        internal ConstantPoolItemMI GetMethodref(int handle)
        {
            return GetMethodref(new MethodrefConstantHandle(checked((ushort)handle)));
        }

        /// <summary>
        /// Version of GetMethodref that does not throw if the handle is invalid. Used by IsAccessBridge.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal ConstantPoolItemMI SafeGetMethodref(ConstantHandle handle)
        {
            if (handle.IsNotNil && handle.Slot < constantpool.Length)
                return constantpool[handle.Slot] as ConstantPoolItemMI;

            return null;
        }

        /// <summary>
        /// Version of GetMethodref that does not throw if the handle is invalid. Used by IsAccessBridge.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        internal ConstantPoolItemMI SafeGetMethodref(int slot)
        {
            if (slot > ushort.MaxValue || slot < ushort.MinValue)
                return null;

            return SafeGetMethodref(new ConstantHandle(ConstantKind.Unknown, (ushort)slot));
        }

        internal ConstantPoolItemInvokeDynamic GetInvokeDynamic(InvokeDynamicConstantHandle handle)
        {
            return (ConstantPoolItemInvokeDynamic)constantpool[handle.Slot];
        }

        private ConstantPoolItem GetConstantPoolItem(ConstantHandle handle)
        {
            return constantpool[handle.Slot];
        }

        internal string GetConstantPoolClass(ClassConstantHandle handle)
        {
            return ((ConstantPoolItemClass)constantpool[handle.Slot]).Name;
        }

        private bool SafeIsConstantPoolClass(ClassConstantHandle handle)
        {
            if (handle.Slot > 0 && handle.Slot < constantpool.Length)
                return constantpool[handle.Slot] as ConstantPoolItemClass != null;

            return false;
        }

        internal RuntimeJavaType GetConstantPoolClassType(ClassConstantHandle handle)
        {
            return ((ConstantPoolItemClass)constantpool[handle.Slot]).GetClassType();
        }

        internal RuntimeJavaType GetConstantPoolClassType(int slot)
        {
            return GetConstantPoolClassType(new ClassConstantHandle(checked((ushort)slot)));
        }
        string GetConstantPoolUtf8String(string[] utf8_cp, Utf8ConstantHandle handle)
        {
            var s = utf8_cp[handle.Slot];
            if (s == null)
            {
                if (clazz.This.IsNil)
                    throw new ClassFormatError("Bad constant pool index #{0}", handle);
                else
                    throw new ClassFormatError("{0} (Bad constant pool index #{1})", Name, handle);
            }

            return s;
        }

        internal ConstantType GetConstantPoolConstantType(ConstantHandle handle)
        {
            return constantpool[handle.Slot].GetConstantType();
        }

        internal ConstantType GetConstantPoolConstantType(int slot)
        {
            return GetConstantPoolConstantType(new ConstantHandle(ConstantKind.Unknown, checked((ushort)slot)));
        }

        internal double GetConstantPoolConstantDouble(DoubleConstantHandle handle)
        {
            return ((ConstantPoolItemDouble)constantpool[handle.Slot]).Value;
        }

        internal float GetConstantPoolConstantFloat(FloatConstantHandle handle)
        {
            return ((ConstantPoolItemFloat)constantpool[handle.Slot]).Value;
        }

        internal int GetConstantPoolConstantInteger(IntegerConstantHandle handle)
        {
            return ((ConstantPoolItemInteger)constantpool[handle.Slot]).Value;
        }

        internal long GetConstantPoolConstantLong(LongConstantHandle handle)
        {
            return ((ConstantPoolItemLong)constantpool[handle.Slot]).Value;
        }

        internal string GetConstantPoolConstantString(StringConstantHandle handle)
        {
            return ((ConstantPoolItemString)constantpool[handle.Slot]).Value;
        }

        internal string GetConstantPoolConstantString(int slot)
        {
            return GetConstantPoolConstantString(new StringConstantHandle(checked((ushort)slot)));
        }

        internal ConstantPoolItemMethodHandle GetConstantPoolConstantMethodHandle(MethodHandleConstantHandle handle)
        {
            return (ConstantPoolItemMethodHandle)constantpool[handle.Slot];
        }

        internal ConstantPoolItemMethodHandle GetConstantPoolConstantMethodHandle(int slot)
        {
            return GetConstantPoolConstantMethodHandle(new MethodHandleConstantHandle(checked((ushort)slot)));
        }

        internal ConstantPoolItemMethodType GetConstantPoolConstantMethodType(MethodTypeConstantHandle handle)
        {
            return (ConstantPoolItemMethodType)constantpool[handle.Slot];
        }

        internal ConstantPoolItemMethodType GetConstantPoolConstantMethodType(int slot)
        {
            return GetConstantPoolConstantMethodType(new MethodTypeConstantHandle(checked((ushort)slot)));
        }

        internal object GetConstantPoolConstantLiveObject(int slot)
        {
            return ((ConstantPoolItemLiveObject)constantpool[slot]).Value;
        }

        internal string Name => GetConstantPoolClass(clazz.This);

        internal ConstantPoolItemClass SuperClass => (ConstantPoolItemClass)constantpool[clazz.Super.Slot];

        internal Field[] Fields => fields;

        internal Method[] Methods => methods;

        internal ConstantPoolItemClass[] Interfaces => interfaces;

        internal string SourceFileAttribute => sourceFile;

        internal string SourcePath
        {
#if IMPORTER
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

        internal string GenericSignature => signature;

        internal string[] EnclosingMethod => enclosingMethod;

        internal ref readonly TypeAnnotationTable RuntimeVisibleTypeAnnotations => ref runtimeVisibleTypeAnnotations;

        internal object[] GetConstantPool()
        {
            var cp = new object[constantpool.Length];
            for (int i = 1; i < cp.Length; i++)
                if (constantpool[i] != null)
                    cp[i] = constantpool[i].GetRuntimeValue();

            return cp;
        }

        internal string IKVMAssemblyAttribute => ikvmAssembly;

        internal bool DeprecatedAttribute => (flags & FLAG_MASK_DEPRECATED) != 0;

        internal bool IsInternal => (flags & FLAG_MASK_INTERNAL) != 0;

        // for use by ikvmc (to implement the -privatepackage option)
        internal void SetInternal()
        {
            access_flags &= ~Modifiers.AccessMask;
            flags |= FLAG_MASK_INTERNAL;
        }

        internal bool HasAssertions => (flags & FLAG_HAS_ASSERTIONS) != 0;

        internal bool HasInitializedFields
        {
            get
            {
                foreach (Field f in fields)
                    if (f.IsStatic && !f.IsFinal && f.ConstantValue != null)
                        return true;

                return false;
            }
        }

        internal BootstrapMethod GetBootstrapMethod(int index)
        {
            return bootstrapMethods[index];
        }

        internal InnerClass[] InnerClasses => innerClasses;

        internal Field GetField(string name, string sig)
        {
            for (int i = 0; i < fields.Length; i++)
                if (fields[i].Name == name && fields[i].Signature == sig)
                    return fields[i];

            return null;
        }

        /// <summary>
        /// Removes a call to java.lang.Class.desiredAssertionStatus() and replaces it with a hard coded constant (true).
        /// </summary>
        /// <param name="method"></param>
        void RemoveAssertionInit(Method method)
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
            if (method.Instructions is [
                { NormalizedOpCode: NormalizedByteCode.__ldc },
                { NormalizedOpCode: NormalizedByteCode.__invokevirtual },
                { NormalizedOpCode: NormalizedByteCode.__ifne },
                { NormalizedOpCode: NormalizedByteCode.__iconst },
                { NormalizedOpCode: NormalizedByteCode.__goto },
                { NormalizedOpCode: NormalizedByteCode.__iconst },
                { NormalizedOpCode: NormalizedByteCode.__putstatic },
                ..] &&
                method.Instructions[0].NormalizedOpCode == NormalizedByteCode.__ldc && SafeIsConstantPoolClass(new ClassConstantHandle(checked((ushort)method.Instructions[0].Arg1))) &&
                method.Instructions[1].NormalizedOpCode == NormalizedByteCode.__invokevirtual && IsDesiredAssertionStatusMethodref(method.Instructions[1].Arg1) &&
                method.Instructions[2].NormalizedOpCode == NormalizedByteCode.__ifne && method.Instructions[2].TargetIndex == 5 &&
                method.Instructions[3].NormalizedOpCode == NormalizedByteCode.__iconst && method.Instructions[3].Arg1 == 1 &&
                method.Instructions[4].NormalizedOpCode == NormalizedByteCode.__goto && method.Instructions[4].TargetIndex == 6 &&
                method.Instructions[5].NormalizedOpCode == NormalizedByteCode.__iconst && method.Instructions[5].Arg1 == 0 &&
                method.Instructions[6].NormalizedOpCode == NormalizedByteCode.__putstatic && (fieldref = SafeGetFieldref(method.Instructions[6].Arg1)) != null &&
                fieldref.Class == Name && fieldref.Signature == "Z" &&
                (field = GetField(fieldref.Name, fieldref.Signature)) != null &&
                field.IsStatic && field.IsFinal &&
                !HasBranchIntoRegion(method.Instructions, 7, method.Instructions.Length, 0, 7) &&
                !HasStaticFieldWrite(method.Instructions, 7, method.Instructions.Length, field) &&
                !HasExceptionHandlerInRegion(method.ExceptionTable, 0, 7))
            {
                field.PatchConstantValue(true);
                method.Instructions[0].PatchOpCode(NormalizedByteCode.__goto, 7);
                flags |= FLAG_HAS_ASSERTIONS;
            }
        }

        bool IsDesiredAssertionStatusMethodref(int cpi)
        {
            return SafeGetMethodref(cpi) is ConstantPoolItemMethodref { Class: "java.lang.Class", Name: "desiredAssertionStatus", Signature: "()Z" };
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

        /// <summary>
        /// Dispses of the instance.
        /// </summary>
        public void Dispose()
        {
            clazz.Dispose();
        }

        /// <summary>
        /// Finalizes the instance.
        /// </summary>
        ~ClassFile()
        {
            Dispose();
        }

    }

}
