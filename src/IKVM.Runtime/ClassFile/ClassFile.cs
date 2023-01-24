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
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;

using IKVM.Attributes;
using IKVM.ByteCode;
using IKVM.ByteCode.Reading;

namespace IKVM.Internal
{

    sealed partial class ClassFile
    {

        const ushort FLAG_MASK_DEPRECATED = 0x100;
        const ushort FLAG_MASK_INTERNAL = 0x200;
        const ushort FLAG_CALLERSENSITIVE = 0x400;
        const ushort FLAG_LAMBDAFORM_COMPILED = 0x800;
        const ushort FLAG_LAMBDAFORM_HIDDEN = 0x1000;
        const ushort FLAG_FORCEINLINE = 0x2000;
        const ushort FLAG_HAS_ASSERTIONS = 0x4000;

        readonly ClassReader reader;

        ConstantPoolItem[] constantpool;
        string[] utf8_cp;

        // Modifiers is a ushort, so the next four fields combine into two 32 bit slots
        Modifiers access_flags;
        ushort flags;
        ConstantPoolItemClass[] interfaces;
        Field[] fields;
        Method[] methods;
        string sourceFile;
#if IMPORTER
        string sourcePath;
#endif
        string ikvmAssembly;
        InnerClass[] innerClasses;
        object[] annotations;
        string signature;
        string[] enclosingMethod;
        BootstrapMethod[] bootstrapMethods;
        TypeAnnotationReaderCollection runtimeVisibleTypeAnnotations;

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
            return GetClassName(ClassReader.Read(bytes.AsMemory(offset, length)), out isstub);
        }

        /// <summary>
        /// This method returns the class name, and whether or not the class is an IKVM stub.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="isstub"></param>
        /// <returns></returns>
        /// <exception cref="UnsupportedClassVersionError"></exception>
        /// <exception cref="ClassFormatError"></exception>
        static string GetClassName(ClassReader reader, out bool isstub)
        {
            try
            {
                if (reader.Version < new ClassFormatVersion(45, 3) || reader.Version > 52)
                    throw new UnsupportedClassVersionError(reader.Version);

                // this is a terrible way to go about encoding this information
                isstub = reader.Constants.OfType<Utf8ConstantReader>().Any(i => i.Value == "IKVM.NET.Assembly");

                return string.Intern(reader.This.Name.Value.Replace('/', '.'));
            }
            catch (ByteCodeException e)
            {
                throw new ClassFormatError(e.Message);
            }
        }

#endif

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="inputClassName"></param>
        /// <param name="options"></param>
        /// <param name="constantPoolPatches"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UnsupportedClassVersionError"></exception>
        /// <exception cref="ClassFormatError"></exception>
        internal ClassFile(ClassReader reader, string inputClassName, ClassFileParseOptions options, object[] constantPoolPatches)
        {
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));

            try
            {
                if (reader.Version < new ClassFormatVersion(45, 3) || reader.Version > 52)
                    throw new UnsupportedClassVersionError(reader.Version);

                constantpool = new ConstantPoolItem[reader.Constants.Count];
                utf8_cp = new string[reader.Constants.Count];
                for (int i = 1; i < reader.Constants.Count; i++)
                {
                    switch (reader.Constants[i])
                    {
                        case null:
                            // longs and doubles can leave holes in the constant pool
                            break;
                        case ClassConstantReader clazzConstant:
                            constantpool[i] = new ConstantPoolItemClass(clazzConstant);
                            break;
                        case DoubleConstantReader doubleConstant:
                            constantpool[i] = new ConstantPoolItemDouble(doubleConstant);
                            break;
                        case FieldrefConstantReader fieldRefConstant:
                            constantpool[i] = new ConstantPoolItemFieldref(fieldRefConstant);
                            break;
                        case FloatConstantReader floatConstant:
                            constantpool[i] = new ConstantPoolItemFloat(floatConstant);
                            break;
                        case IntegerConstantReader integerConstant:
                            constantpool[i] = new ConstantPoolItemInteger(integerConstant);
                            break;
                        case InterfaceMethodrefConstantReader interfaceMethodrefConstant:
                            constantpool[i] = new ConstantPoolItemInterfaceMethodref(interfaceMethodrefConstant);
                            break;
                        case LongConstantReader longConstant:
                            constantpool[i] = new ConstantPoolItemLong(longConstant);
                            break;
                        case MethodrefConstantReader methodrefConstantReader:
                            constantpool[i] = new ConstantPoolItemMethodref(methodrefConstantReader);
                            break;
                        case NameAndTypeConstantReader nameAndType:
                            constantpool[i] = new ConstantPoolItemNameAndType(nameAndType);
                            break;
                        case MethodHandleConstantReader methodHandle:
                            if (reader.Version < 51)
                                goto default;
                            constantpool[i] = new ConstantPoolItemMethodHandle(methodHandle);
                            break;
                        case MethodTypeConstantReader methodType:
                            if (reader.Version < 51)
                                goto default;
                            constantpool[i] = new ConstantPoolItemMethodType(methodType);
                            break;
                        case InvokeDynamicConstantReader invokeDynamic:
                            if (reader.Version < 51)
                                goto default;
                            constantpool[i] = new ConstantPoolItemInvokeDynamic(invokeDynamic);
                            break;
                        case StringConstantReader stringConstant:
                            constantpool[i] = new ConstantPoolItemString(stringConstant);
                            break;
                        case Utf8ConstantReader utf8ConstantReader:
                            utf8_cp[i] = utf8ConstantReader.Value;
                            break;
                        default:
                            throw new ClassFormatError("Unknown constant type.");
                    }
                }

                if (constantPoolPatches != null)
                    PatchConstantPool(constantPoolPatches, utf8_cp, inputClassName);

                for (int i = 1; i < reader.Constants.Count; i++)
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

                access_flags = (Modifiers)reader.AccessFlags;

                // NOTE although the vmspec says (in 4.1) that interfaces must be marked abstract, earlier versions of
                // javac (JDK 1.1) didn't do this, so the VM doesn't enforce this rule for older class files.
                // NOTE although the vmspec implies (in 4.1) that ACC_SUPER is illegal on interfaces, it doesn't enforce this
                // for older class files.
                // (See http://bugs.sun.com/bugdatabase/view_bug.do?bug_id=6320322)
                if ((IsInterface && IsFinal) || (IsAbstract && IsFinal) || (reader.Version >= 49 && IsAnnotation && !IsInterface) || (reader.Version >= 49 && IsInterface && (!IsAbstract || IsSuper || IsEnum)))
                    throw new ClassFormatError("{0} (Illegal class modifiers 0x{1:X})", inputClassName, access_flags);

                ValidateConstantPoolItemClass(inputClassName, reader.Record.ThisClassIndex);
                ValidateConstantPoolItemClass(inputClassName, reader.Record.SuperClassIndex);

                if (IsInterface && (reader.Record.SuperClassIndex == 0 || SuperClass.Name != "java.lang.Object"))
                    throw new ClassFormatError("{0} (Interfaces must have java.lang.Object as superclass)", Name);

                // most checks are already done by ConstantPoolItemClass.Resolve, but since it allows
                // array types, we do need to check for that
                if (Name[0] == '[')
                    throw new ClassFormatError("Bad name");

                interfaces = new ConstantPoolItemClass[reader.Interfaces.Count];
                for (int i = 0; i < interfaces.Length; i++)
                {
                    int index = reader.Interfaces[i].Record.ClassIndex;
                    if (index == 0 || index >= constantpool.Length)
                        throw new ClassFormatError("{0} (Illegal constant pool index)", Name);

                    var cpi = constantpool[index] as ConstantPoolItemClass;
                    if (cpi == null)
                        throw new ClassFormatError("{0} (Interface name has bad constant type)", Name);

                    interfaces[i] = cpi;
                }

                CheckDuplicates(interfaces, "Repetitive interface name");

                fields = new Field[reader.Fields.Count];
                for (int i = 0; i < reader.Fields.Count; i++)
                {
                    fields[i] = new Field(this, utf8_cp, reader.Fields[i]);
                    var name = fields[i].Name;

                    if (!IsValidFieldName(name, reader.Version))
                        throw new ClassFormatError("{0} (Illegal field name \"{1}\")", Name, name);
                }

                CheckDuplicates<FieldOrMethod>(fields, "Repetitive field name/signature");

                methods = new Method[reader.Methods.Count];
                for (int i = 0; i < reader.Methods.Count; i++)
                {
                    methods[i] = new Method(this, utf8_cp, options, reader.Methods[i]);
                    string name = methods[i].Name;
                    string sig = methods[i].Signature;
                    if (!IsValidMethodName(name, reader.Version))
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

                for (int i = 0; i < reader.Attributes.Count; i++)
                {
                    var attribute = reader.Attributes[i];

                    switch (GetConstantPoolUtf8String(utf8_cp, attribute.Info.Record.NameIndex))
                    {
                        case "Deprecated":
                            if (attribute is not DeprecatedAttributeReader deprecatedAttribute)
                                throw new ClassFormatError("Invalid Deprecated attribute type.");

                            flags |= FLAG_MASK_DEPRECATED;
                            break;
                        case "SourceFile":
                            if (attribute is not SourceFileAttributeReader sourceFileAttribute)
                                throw new ClassFormatError("Invalid SourceFile attribute type.");

                            sourceFile = GetConstantPoolUtf8String(utf8_cp, sourceFileAttribute.Record.SourceFileIndex);
                            break;
                        case "InnerClasses":
                            if (MajorVersion < 49)
                                goto default;

                            if (attribute is not InnerClassesAttributeReader innerClassesAttribute)
                                throw new ClassFormatError("Invalid InnerClasses attribute type.");

                            innerClasses = new InnerClass[innerClassesAttribute.Items.Count];
                            for (int j = 0; j < innerClasses.Length; j++)
                            {
                                var item = innerClassesAttribute.Items[j];

                                innerClasses[j].innerClass = item.InnerClass?.Index ?? 0;
                                innerClasses[j].outerClass = item.OuterClass?.Index ?? 0;
                                innerClasses[j].name = item.InnerName?.Index ?? 0;
                                innerClasses[j].accessFlags = (Modifiers)item.InnerClassAccessFlags;

                                if (innerClasses[j].innerClass != 0 && !(GetConstantPoolItem(innerClasses[j].innerClass) is ConstantPoolItemClass))
                                    throw new ClassFormatError("{0} (inner_class_info_index has bad constant pool index)", this.Name);

                                if (innerClasses[j].outerClass != 0 && !(GetConstantPoolItem(innerClasses[j].outerClass) is ConstantPoolItemClass))
                                    throw new ClassFormatError("{0} (outer_class_info_index has bad constant pool index)", this.Name);

                                if (innerClasses[j].name != 0 && utf8_cp[innerClasses[j].name] == null)
                                    throw new ClassFormatError("{0} (inner class name has bad constant pool index)", this.Name);

                                if (innerClasses[j].innerClass == innerClasses[j].outerClass)
                                    throw new ClassFormatError("{0} (Class is both inner and outer class)", this.Name);

                                if (innerClasses[j].innerClass != 0 && innerClasses[j].outerClass != 0)
                                {
                                    MarkLinkRequiredConstantPoolItem(innerClasses[j].innerClass);
                                    MarkLinkRequiredConstantPoolItem(innerClasses[j].outerClass);
                                }
                            }

                            break;
                        case "Signature":
                            if (reader.Version < 49)
                                goto default;

                            if (attribute is not SignatureAttributeReader signatureAttribute)
                                throw new ClassFormatError("Invalid Signature attribute type.");

                            signature = GetConstantPoolUtf8String(utf8_cp, signatureAttribute.Record.SignatureIndex);
                            break;
                        case "EnclosingMethod":
                            if (reader.Version < 49)
                                goto default;

                            if (attribute is not EnclosingMethodAttributeReader enclosingMethodAttribute)
                                throw new ClassFormatError("Invalid EnclosingMethod attribute type.");

                            var classIndex = enclosingMethodAttribute.Record.ClassIndex;
                            var methodIndex = enclosingMethodAttribute.Record.MethodIndex;
                            ValidateConstantPoolItemClass(inputClassName, classIndex);

                            if (methodIndex == 0)
                            {
                                enclosingMethod = new string[]
                                {
                                    GetConstantPoolClass(classIndex),
                                    null,
                                    null
                                };
                            }
                            else
                            {
                                if (GetConstantPoolItem(methodIndex) is not ConstantPoolItemNameAndType m)
                                    throw new ClassFormatError("{0} (Bad constant pool index #{1})", inputClassName, methodIndex);

                                enclosingMethod = new string[]
                                {
                                    GetConstantPoolClass(classIndex),
                                    GetConstantPoolUtf8String(utf8_cp, m.nameIndex),
                                    GetConstantPoolUtf8String(utf8_cp, m.descriptorIndex).Replace('/', '.')
                                };
                            }

                            break;
                        case "RuntimeVisibleAnnotations":
                            if (reader.Version < 49)
                                goto default;

                            if (attribute is not RuntimeVisibleAnnotationsAttributeReader runtimeVisibleAnnotationsAttribute)
                                throw new ClassFormatError("Invalid RuntimeVisibleAnnotations attribute type.");

                            annotations = ReadAnnotations(runtimeVisibleAnnotationsAttribute.Annotations, this, utf8_cp);
                            break;
#if IMPORTER
                        case "RuntimeInvisibleAnnotations":
                            if (reader.Version < 49)
                                goto default;

                            if (attribute is not RuntimeInvisibleAnnotationsAttributeReader runtimeInvisibleAnnotationsAttribute)
                                throw new ClassFormatError("Invalid RuntimeInvisibleAnnotations attribute type.");

                            foreach (object[] annot in ReadAnnotations(runtimeInvisibleAnnotationsAttribute.Annotations, this, utf8_cp))
                            {
                                if (annot[1].Equals("Likvm/lang/Internal;"))
                                {
                                    access_flags &= ~Modifiers.AccessMask;
                                    flags |= FLAG_MASK_INTERNAL;
                                }
                            }

                            break;
#endif
                        case "BootstrapMethods":
                            if (reader.Version < 51)
                                goto default;

                            if (attribute is not BootstrapMethodsAttributeReader bootstrapMethodsAttribute)
                                throw new ClassFormatError("Invalid BootstrapMethods attribute type.");

                            bootstrapMethods = ReadBootstrapMethods(bootstrapMethodsAttribute.Methods, this);
                            break;
                        case "RuntimeVisibleTypeAnnotations":
                            if (reader.Version < 52)
                                goto default;

                            if (attribute is not RuntimeVisibleTypeAnnotationsAttributeReader runtimeVisibleTypeAnnotationsAttribute)
                                throw new ClassFormatError("Invalid RuntimeVisibleTypeAnnotations attribute type.");

                            CreateUtf8ConstantPoolItems(utf8_cp);
                            runtimeVisibleTypeAnnotations = runtimeVisibleTypeAnnotationsAttribute.Annotations;
                            break;
                        case "IKVM.NET.Assembly":
                            if (attribute is not UnknownAttributeReader unknownAttributeReader)
                                throw new ClassFormatError("Invalid IKVM.NET.Assembly attribute type.");

                            if (unknownAttributeReader.Record.Data.Length != 2)
                                throw new ClassFormatError("IKVM.NET.Assembly attribute has incorrect length");

                            ikvmAssembly = GetConstantPoolUtf8String(utf8_cp, BinaryPrimitives.ReadInt16BigEndian(unknownAttributeReader.Record.Data));
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
                    constantpool[i] = new ConstantPoolItemUtf8(utf8_cp[i]);
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

        void MarkLinkRequiredConstantPoolItem(int index)
        {
            if (index > 0 && index < constantpool.Length && constantpool[index] != null)
            {
                constantpool[index].MarkLinkRequired();
            }
        }

        static BootstrapMethod[] ReadBootstrapMethods(IReadOnlyList<BootstrapMethodsAttributeMethodReader> methods, ClassFile classFile)
        {
            var bsm = new BootstrapMethod[methods.Count];
            for (int i = 0; i < methods.Count; i++)
            {
                var method = methods[i];

                var bsm_index = method.Record.MethodRefIndex;
                if (bsm_index >= classFile.constantpool.Length || classFile.constantpool[bsm_index] is not ConstantPoolItemMethodHandle)
                    throw new ClassFormatError("bootstrap_method_index {0} has bad constant type in class file {1}", bsm_index, classFile.Name);

                classFile.MarkLinkRequiredConstantPoolItem(bsm_index);

                var argument_count = method.Arguments.Count;
                var args = new ushort[argument_count];
                for (int j = 0; j < args.Length; j++)
                {
                    var argument_index = method.Record.Arguments[j];
                    if (classFile.IsValidConstant(argument_index) == false)
                        throw new ClassFormatError("argument_index {0} has bad constant type in class file {1}", argument_index, classFile.Name);

                    classFile.MarkLinkRequiredConstantPoolItem(argument_index);
                    args[j] = argument_index;
                }

                bsm[i] = new BootstrapMethod(bsm_index, args);
            }

            return bsm;
        }

        bool IsValidConstant(ushort index)
        {
            if (index < constantpool.Length && constantpool[index] != null)
            {
                try
                {
                    constantpool[index].GetConstantType();
                    return true;
                }
                catch (InvalidOperationException)
                {

                }
            }

            return false;
        }

        static object[] ReadAnnotations(AnnotationReaderCollection reader, ClassFile classFile, string[] utf8_cp)
        {
            var annotations = new object[reader.Count];

            for (int i = 0; i < annotations.Length; i++)
                annotations[i] = ReadAnnotation(reader[i], classFile, utf8_cp);

            return annotations;
        }

        static object ReadAnnotation(AnnotationReader reader, ClassFile classFile, string[] utf8_cp)
        {
            var l = new object[2 + reader.Elements.Count * 2];
            l[0] = AnnotationDefaultAttribute.TAG_ANNOTATION;
            l[1] = classFile.GetConstantPoolUtf8String(utf8_cp, reader.Record.TypeIndex);
            for (int i = 0; i < reader.Elements.Count; i++)
            {
                l[2 + i * 2 + 0] = classFile.GetConstantPoolUtf8String(utf8_cp, reader.Record.Elements[i].NameIndex);
                l[2 + i * 2 + 1] = ReadAnnotationElementValue(reader.Elements[i], classFile, utf8_cp);
            }

            return l;
        }

        static object ReadAnnotationElementValue(ElementValueReader reader, ClassFile classFile, string[] utf8_cp)
        {
            try
            {
                switch (reader)
                {
                    case ElementValueConstantReader r when r.Tag == ByteCode.Parsing.ElementValueTag.Boolean:
                        return classFile.GetConstantPoolConstantInteger(r.Value.Index) != 0;
                    case ElementValueConstantReader r when r.Tag == ByteCode.Parsing.ElementValueTag.Byte:
                        return (byte)classFile.GetConstantPoolConstantInteger(r.Value.Index);
                    case ElementValueConstantReader r when r.Tag == ByteCode.Parsing.ElementValueTag.Char:
                        return (char)classFile.GetConstantPoolConstantInteger(r.Value.Index);
                    case ElementValueConstantReader r when r.Tag == ByteCode.Parsing.ElementValueTag.Short:
                        return (short)classFile.GetConstantPoolConstantInteger(r.Value.Index);
                    case ElementValueConstantReader r when r.Tag == ByteCode.Parsing.ElementValueTag.Integer:
                        return classFile.GetConstantPoolConstantInteger(r.Value.Index);
                    case ElementValueConstantReader r when r.Tag == ByteCode.Parsing.ElementValueTag.Float:
                        return classFile.GetConstantPoolConstantFloat(r.Value.Index);
                    case ElementValueConstantReader r when r.Tag == ByteCode.Parsing.ElementValueTag.Long:
                        return classFile.GetConstantPoolConstantLong(r.Value.Index);
                    case ElementValueConstantReader r when r.Tag == ByteCode.Parsing.ElementValueTag.Double:
                        return classFile.GetConstantPoolConstantDouble(r.Value.Index);
                    case ElementValueConstantReader r when r.Tag == ByteCode.Parsing.ElementValueTag.String:
                        return classFile.GetConstantPoolUtf8String(utf8_cp, r.Value.Index);
                    case ElementValueEnumConstantReader r:
                        return new object[] {
                            AnnotationDefaultAttribute.TAG_ENUM,
                            classFile.GetConstantPoolUtf8String(utf8_cp, r.TypeName.Index),
                            classFile.GetConstantPoolUtf8String(utf8_cp, r.ConstantName.Index)
                        };
                    case ElementValueClassReader r:
                        return new object[] {
                            AnnotationDefaultAttribute.TAG_CLASS,
                            classFile.GetConstantPoolUtf8String(utf8_cp, r.Class.Index)
                        };
                    case ElementValueAnnotationReader r:
                        return ReadAnnotation(r.Annotation, classFile, utf8_cp);
                    case ElementValueArrayReader r:
                        var array = new object[r.Values.Count + 1];
                        array[0] = AnnotationDefaultAttribute.TAG_ARRAY;
                        for (int i = 0; i < r.Values.Count; i++)
                            array[i + 1] = ReadAnnotationElementValue(r.Values[i], classFile, utf8_cp);
                        return array;
                    default:
                        throw new ClassFormatError("Invalid tag {0} in annotation element_value", reader.Record.Tag);
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

            return new object[] { AnnotationDefaultAttribute.TAG_ERROR, "java.lang.IllegalArgumentException", "Wrong type at constant pool index" };
        }

        private void ValidateConstantPoolItemClass(string classFile, ushort index)
        {
            if (index >= constantpool.Length || constantpool[index] is not ConstantPoolItemClass)
                throw new ClassFormatError("{0} (Bad constant pool index #{1})", classFile, index);
        }

        private static bool IsValidMethodName(string name, ClassFormatVersion version)
        {
            if (name.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < name.Length; i++)
            {
                if (".;[/<>".IndexOf(name[i]) != -1)
                {
                    return false;
                }
            }
            return version >= 49 || IsValidPre49Identifier(name);
        }

        private static bool IsValidFieldName(string name, ClassFormatVersion version)
        {
            if (name.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < name.Length; i++)
            {
                if (".;[/".IndexOf(name[i]) != -1)
                {
                    return false;
                }
            }
            return version >= 49 || IsValidPre49Identifier(name);
        }

        private static bool IsValidPre49Identifier(string name)
        {
            if (!Char.IsLetter(name[0]) && "$_".IndexOf(name[0]) == -1)
            {
                return false;
            }
            for (int i = 1; i < name.Length; i++)
            {
                if (!Char.IsLetterOrDigit(name[i]) && "$_".IndexOf(name[i]) == -1)
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
            if (start >= end)
            {
                return false;
            }
            switch (sig[start])
            {
                case 'L':
                    return sig.IndexOf(';', start + 1) == end - 1;
                case '[':
                    while (sig[start] == '[')
                    {
                        start++;
                        if (start == end)
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
            if (sig.Length < 3 || sig[0] != '(')
            {
                return false;
            }
            int end = sig.IndexOf(')');
            if (end == -1)
            {
                return false;
            }
            if (!sig.EndsWith(")V") && !IsValidFieldSigImpl(sig, end + 1, sig.Length))
            {
                return false;
            }
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
                        {
                            i++;
                        }
                        if ("BZCSIJFDL".IndexOf(sig[i]) == -1)
                        {
                            return false;
                        }
                        if (sig[i] == 'L')
                        {
                            i = sig.IndexOf(';', i);
                        }
                        break;
                    default:
                        return false;
                }
                if (i == -1 || i >= end)
                {
                    return false;
                }
            }
            return true;
        }

        internal int MajorVersion => reader.Version.Major;

        internal void Link(TypeWrapper thisType, LoadMode mode)
        {
            // this is not just an optimization, it's required for anonymous classes to be able to refer to themselves
            ((ConstantPoolItemClass)constantpool[reader.Record.ThisClassIndex]).LinkSelf(thisType);

            for (int i = 1; i < constantpool.Length; i++)
                if (constantpool[i] != null)
                    constantpool[i].Link(thisType, mode);
        }

        internal Modifiers Modifiers => access_flags;

        internal bool IsAbstract
        {
            get
            {
                // interfaces are implicitly abstract
                return (access_flags & (Modifiers.Abstract | Modifiers.Interface)) != 0;
            }
        }

        internal bool IsFinal => (access_flags & Modifiers.Final) != 0;

        internal bool IsPublic => (access_flags & Modifiers.Public) != 0;

        internal bool IsInterface => (access_flags & Modifiers.Interface) != 0;

        internal bool IsEnum => (access_flags & Modifiers.Enum) != 0;

        internal bool IsAnnotation => (access_flags & Modifiers.Annotation) != 0;

        internal bool IsSuper => (access_flags & Modifiers.Super) != 0;

        internal bool IsReferenced(Field fld) => constantpool.OfType<ConstantPoolItemFieldref>().Any(i => i.Class == Name && i.Name == fld.Name && i.Signature == fld.Signature);

        internal ConstantPoolItemFieldref GetFieldref(int index) => (ConstantPoolItemFieldref)constantpool[index];

        // this won't throw an exception if index is invalid
        // (used by IsSideEffectFreeStaticInitializer)
        internal ConstantPoolItemFieldref SafeGetFieldref(int index)
        {
            if (index > 0 && index < constantpool.Length)
                return constantpool[index] as ConstantPoolItemFieldref;

            return null;
        }

        // NOTE this returns an MI, because it used for both normal methods and interface methods
        internal ConstantPoolItemMI GetMethodref(int index) => (ConstantPoolItemMI)constantpool[index];

        // this won't throw an exception if index is invalid
        // (used by IsAccessBridge)
        internal ConstantPoolItemMI SafeGetMethodref(int index)
        {
            if (index > 0 && index < constantpool.Length)
                return constantpool[index] as ConstantPoolItemMI;

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
            if (index > 0 && index < constantpool.Length)
                return constantpool[index] as ConstantPoolItemClass != null;

            return false;
        }

        internal TypeWrapper GetConstantPoolClassType(int index)
        {
            return ((ConstantPoolItemClass)constantpool[index]).GetClassType();
        }

        private string GetConstantPoolUtf8String(string[] utf8_cp, int index)
        {
            var s = utf8_cp[index];
            if (s == null)
            {
                if (reader.This.Index == 0)
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

        internal string Name => GetConstantPoolClass(reader.Record.ThisClassIndex);

        internal ConstantPoolItemClass SuperClass => (ConstantPoolItemClass)constantpool[reader.Record.SuperClassIndex];

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

        internal IReadOnlyList<TypeAnnotationReader> RuntimeVisibleTypeAnnotations => runtimeVisibleTypeAnnotations;

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
