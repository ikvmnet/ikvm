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

using IKVM.ByteCode;
using IKVM.ByteCode.Decoding;
using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Linking
{

    sealed partial class ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> : IDisposable
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {

        /// <summary>
        /// Reads a <see cref="AccessFlag"/> value into a <see cref="ClassFileAccessFlags"/> value.
        /// </summary>
        /// <param name="accessFlags"></param>
        /// <returns></returns>
        internal static ClassFileAccessFlags ReadAccessFlags(AccessFlag accessFlags)
        {
            var f = default(ClassFileAccessFlags);
            if ((accessFlags & AccessFlag.Public) != 0)
                f |= ClassFileAccessFlags.Public;
            if ((accessFlags & AccessFlag.Final) != 0)
                f |= ClassFileAccessFlags.Final;
            if ((accessFlags & AccessFlag.Super) != 0)
                f |= ClassFileAccessFlags.Super;
            if ((accessFlags & AccessFlag.Interface) != 0)
                f |= ClassFileAccessFlags.Interface;
            if ((accessFlags & AccessFlag.Abstract) != 0)
                f |= ClassFileAccessFlags.Abstract;
            if ((accessFlags & AccessFlag.Synthetic) != 0)
                f |= ClassFileAccessFlags.Synthetic;
            if ((accessFlags & AccessFlag.Annotation) != 0)
                f |= ClassFileAccessFlags.Annotation;
            if ((accessFlags & AccessFlag.Enum) != 0)
                f |= ClassFileAccessFlags.Enum;
            if ((accessFlags & AccessFlag.Bridge) != 0)
                f |= ClassFileAccessFlags.Bridge;
            if ((accessFlags & AccessFlag.Native) != 0)
                f |= ClassFileAccessFlags.Native;
            if ((accessFlags & AccessFlag.Private) != 0)
                f |= ClassFileAccessFlags.Private;
            if ((accessFlags & AccessFlag.Protected) != 0)
                f |= ClassFileAccessFlags.Protected;
            if ((accessFlags & AccessFlag.Static) != 0)
                f |= ClassFileAccessFlags.Static;
            if ((accessFlags & AccessFlag.Strict) != 0)
                f |= ClassFileAccessFlags.Strictfp;
            if ((accessFlags & AccessFlag.Synchronized) != 0)
                f |= ClassFileAccessFlags.Synchronized;
            if ((accessFlags & AccessFlag.Transient) != 0)
                f |= ClassFileAccessFlags.Transient;
            if ((accessFlags & AccessFlag.VarArgs) != 0)
                f |= ClassFileAccessFlags.VarArgs;
            if ((accessFlags & AccessFlag.Volatile) != 0)
                f |= ClassFileAccessFlags.Volatile;

            return f;
        }

        /// <summary>
        /// Returns <c>true</c> if the given string is a valid method name given the specified class format version.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsValidMethodName(string name, ClassFormatVersion version)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (name.Length == 0)
                return false;

            for (int i = 0; i < name.Length; i++)
                if (".;[/<>".Contains(name[i]))
                    return false;

            return version >= 49 || IsValidPre49Identifier(name);
        }

        /// <summary>
        /// Returns <c>true</c> if the given string is a valid method name given the specified class format version.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsValidMethodName(ReadOnlySpan<char> name, ClassFormatVersion version)
        {
            if (name.Length == 0)
                return false;

            for (int i = 0; i < name.Length; i++)
                if (".;[/<>".Contains(name[i]))
                    return false;

            return version >= 49 || IsValidPre49Identifier(name);
        }

        /// <summary>
        /// Returns <c>true</c> if the given string is a valid field name given the specified class format version.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsValidFieldName(string name, ClassFormatVersion version)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return IsValidFieldName(name.AsSpan(), version);
        }

        /// <summary>
        /// Returns <c>true</c> if the given string is a valid field name given the specified class format version.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsValidFieldName(ReadOnlySpan<char> name, ClassFormatVersion version)
        {
            if (name.Length == 0)
                return false;

            for (int i = 0; i < name.Length; i++)
                if (".;[/".Contains(name[i]))
                    return false;

            return version >= 49 || IsValidPre49Identifier(name);
        }

        /// <summary>
        /// Returns <c>true</c> if the given string is a valid identifier for pre-49 class files.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsValidPre49Identifier(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (!char.IsLetter(name[0]) && "$_".Contains(name[0]) == false)
                return false;

            for (int i = 1; i < name.Length; i++)
                if (!char.IsLetterOrDigit(name[i]) && "$_".Contains(name[i]) == false)
                    return false;

            return true;
        }

        /// <summary>
        /// Returns <c>true</c> if the given string is a valid identifier for pre-49 class files.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsValidPre49Identifier(ReadOnlySpan<char> name)
        {
            if (!char.IsLetter(name[0]) && "$_".Contains(name[0]) == false)
                return false;

            for (int i = 1; i < name.Length; i++)
                if (!char.IsLetterOrDigit(name[i]) && "$_".Contains(name[i]) == false)
                    return false;

            return true;
        }

        /// <summary>
        /// Returns <c>true</c> if the specified descriptor is a valid field descriptor.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public static bool IsValidFieldDescriptor(string descriptor)
        {
            if (descriptor is null)
                throw new ArgumentNullException(nameof(descriptor));

            return IsValidFieldDescriptor(descriptor.AsSpan());
        }

        /// <summary>
        /// Returns <c>true</c> if the specified descriptor is a valid field descriptor.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public static bool IsValidFieldDescriptor(string descriptor, int start, int end)
        {
            if (descriptor is null)
                throw new ArgumentNullException(nameof(descriptor));

            return IsValidFieldDescriptor(descriptor.AsSpan());
        }

        /// <summary>
        /// Returns <c>true</c> if the specified descriptor is a valid field descriptor.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public static bool IsValidFieldDescriptor(ReadOnlySpan<char> descriptor)
        {
            if (descriptor.IsEmpty)
                return false;

            switch (descriptor[0])
            {
                case 'L':
                    // skip L, next semicolon should be last character
                    descriptor = descriptor.Slice(1);
                    return descriptor.Length >= 2 && descriptor.IndexOf(';') == descriptor.Length - 1;
                case '[':
                    // advance past [ values
                    while (descriptor[0] == '[')
                    {
                        descriptor = descriptor.Slice(1);
                        if (descriptor.IsEmpty)
                            return false;
                    }

                    return IsValidFieldDescriptor(descriptor);
                case 'B':
                case 'Z':
                case 'C':
                case 'S':
                case 'I':
                case 'J':
                case 'F':
                case 'D':
                    // skip char, should be empty
                    descriptor = descriptor.Slice(1);
                    return descriptor.IsEmpty;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if the specified descriptor is a valid method descriptor.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public static bool IsValidMethodDescriptor(string descriptor)
        {
            if (descriptor is null)
                throw new ArgumentNullException(nameof(descriptor));

            return IsValidMethodDescriptor(descriptor.AsSpan());
        }

        /// <summary>
        /// Returns <c>true</c> if the specified descriptor is a valid method descriptor.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public static bool IsValidMethodDescriptor(ReadOnlySpan<char> descriptor)
        {
            if (descriptor.Length < 3 || descriptor[0] != '(')
                return false;

            int end = descriptor.IndexOf(')');
            if (end == -1)
                return false;

            if (!descriptor.EndsWith(")V".AsSpan()) && !IsValidFieldDescriptor(descriptor[(end + 1)..]))
                return false;

            for (int i = 1; i < end; i++)
            {
                switch (descriptor[i])
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
                        var p = descriptor.Slice(i).IndexOf(';');
                        i = p == -1 ? -1 : p + i;
                        break;
                    case '[':
                        while (descriptor[i] == '[')
                            i++;

                        if ("BZCSIJFDL".Contains(descriptor[i]) == false)
                            return false;

                        if (descriptor[i] == 'L')
                        {
                            var o = descriptor.Slice(i).IndexOf(';');
                            i = o == -1 ? -1 : o + i;
                        }

                        break;
                    default:
                        return false;
                }

                if (i == -1 || i >= end)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// This method returns the class name, and whether or not the class is an IKVM stub.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="isstub"></param>
        /// <returns></returns>
        internal static string GetClassName(ILinkingContext<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> context, byte[] bytes, int offset, int length, out bool isstub)
        {
            try
            {
                using var clazz = IKVM.ByteCode.Decoding.ClassFile.Read(bytes.AsMemory(offset, length));
                return GetClassName(clazz, out isstub);
            }
            catch (IKVM.ByteCode.UnsupportedClassVersionException e)
            {
                throw new UnsupportedClassVersionException(e.Version);
            }
            catch (ByteCodeException e)
            {
                throw new ClassFormatException(e.Message);
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
        static string GetClassName(ByteCode.Decoding.ClassFile reader, out bool isstub)
        {
            if (reader.Version < new ClassFormatVersion(45, 3) || reader.Version > 52)
                throw new UnsupportedClassVersionException(reader.Version);

            // this is a terrible way to go about encoding this information
            isstub = reader.Constants.Any(i => i.Kind == ConstantKind.Utf8 && reader.Constants.Get((Utf8ConstantHandle)i).Value == "IKVM.NET.Assembly");
            return string.Intern(reader.Constants.Get(reader.This).Name.Replace('/', '.'));
        }

        readonly ILinkingContext<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> _context;
        readonly ByteCode.Decoding.ClassFile _decoder;
        readonly ClassFileParseOptions _options;
        readonly ConstantPool<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> _constants;

        ClassFileAccessFlags _accessFlags;
        ClassFileFlags _flags;
        readonly ConstantPoolItemClass<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>[] _interfaces;
        readonly Field<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>[] _fields;
        readonly Method<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>[] _methods;
        readonly string? _sourceFile;
        string? _sourcePath;
        readonly string? _ikvmAssembly;
        readonly InnerClass[]? _innerClasses;
        readonly object[]? _annotations;
        readonly string? _signature;
        readonly string?[]? _enclosingMethod;
        readonly BootstrapMethodTable _bootstrapMethods;
        readonly TypeAnnotationTable _runtimeVisibleTypeAnnotations = TypeAnnotationTable.Empty;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="decoder"></param>
        /// <param name="inputClassName"></param>
        /// <param name="options"></param>
        /// <param name="constantPoolPatches"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal ClassFile(ILinkingContext<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> context, ByteCode.Decoding.ClassFile decoder, string inputClassName, ClassFileParseOptions options, object[] constantPoolPatches)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _decoder = decoder ?? throw new ArgumentNullException(nameof(decoder));
            _options = options;

            try
            {
                if (decoder.Version < new ClassFormatVersion(45, 3) || decoder.Version > 52)
                    throw new UnsupportedClassVersionException(decoder.Version);

                // load the constants
                _constants = new ConstantPool<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>(this);
                _constants.Patch(inputClassName, constantPoolPatches);
                _constants.Resolve(inputClassName);

                // read the access flags
                _accessFlags = ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>.ReadAccessFlags(decoder.AccessFlags);

                // NOTE although the vmspec says (in 4.1) that interfaces must be marked abstract, earlier versions of
                // javac (JDK 1.1) didn't do this, so the VM doesn't enforce this rule for older class files.
                // NOTE although the vmspec implies (in 4.1) that ACC_SUPER is illegal on interfaces, it doesn't enforce this
                // for older class files.
                // (See http://bugs.sun.com/bugdatabase/view_bug.do?bug_id=6320322)
                if ((IsInterface && IsFinal) || (IsAbstract && IsFinal) || (decoder.Version >= 49 && IsAnnotation && !IsInterface) || (decoder.Version >= 49 && IsInterface && (!IsAbstract || IsSuper || IsEnum)))
                    throw new ClassFormatException("{0} (Illegal class modifiers 0x{1:X})", inputClassName, _accessFlags);

                ValidateConstantPoolItemClass(inputClassName, decoder.This);
                ValidateConstantPoolItemClass(inputClassName, decoder.Super);

                if (IsInterface && (decoder.Super.IsNil || SuperClass.Name != "java.lang.Object"))
                    throw new ClassFormatException("{0} (Interfaces must have java.lang.Object as superclass)", Name);

                // most checks are already done by ConstantPoolItemClass.Resolve, but since it allows
                // array types, we do need to check for that
                if (Name[0] == '[')
                    throw new ClassFormatException("Bad name");

                _interfaces = new ConstantPoolItemClass<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>[decoder.Interfaces.Count];
                for (int i = 0; i < _interfaces.Length; i++)
                {
                    var handle = decoder.Interfaces[i].Class;
                    if (handle.IsNil || handle.Slot >= _constants.SlotCount)
                        throw new ClassFormatException("{0} (Illegal constant pool index)", Name);

                    var cpi = _constants[handle] as ConstantPoolItemClass<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>;
                    if (cpi == null)
                        throw new ClassFormatException("{0} (Interface name has bad constant type)", Name);

                    _interfaces[i] = cpi;
                }

                CheckDuplicates(_interfaces, "Repetitive interface name");

                _fields = new Field<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>[decoder.Fields.Count];
                for (int i = 0; i < decoder.Fields.Count; i++)
                {
                    _fields[i] = new Field<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>(this, decoder.Fields[i]);
                    var name = _fields[i].Name;

                    if (IsValidFieldName(name, decoder.Version) == false)
                        throw new ClassFormatException("{0} (Illegal field name \"{1}\")", Name, name);
                }

                CheckDuplicates<FieldOrMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>>(_fields, "Repetitive field name/signature");

                _methods = new Method<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>[decoder.Methods.Count];
                for (int i = 0; i < decoder.Methods.Count; i++)
                {
                    _methods[i] = new Method<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>(this, decoder.Methods[i]);
                    var name = _methods[i].Name;
                    var sig = _methods[i].Signature;
                    if (IsValidMethodName(name, decoder.Version) == false)
                    {
                        if (!ReferenceEquals(name, StringConstants.INIT) && !ReferenceEquals(name, StringConstants.CLINIT))
                            throw new ClassFormatException("{0} (Illegal method name \"{1}\")", Name, name);
                        if (!sig.EndsWith("V"))
                            throw new ClassFormatException("{0} (Method \"{1}\" has illegal signature \"{2}\")", Name, name, sig);
                        if ((options & ClassFileParseOptions.RemoveAssertions) != 0 && _methods[i].IsClassInitializer)
                            RemoveAssertionInit(_methods[i]);
                    }
                }

                CheckDuplicates<FieldOrMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>>(_methods, "Repetitive method name/signature");

                for (int i = 0; i < decoder.Attributes.Count; i++)
                {
                    var attribute = decoder.Attributes[i];

                    switch (GetConstantPoolUtf8String(attribute.Name))
                    {
                        case AttributeName.Deprecated:
                            var deprecatedAttribute = (DeprecatedAttribute)attribute;
                            _flags |= ClassFileFlags.MASK_DEPRECATED;
                            break;
                        case AttributeName.SourceFile:
                            var sourceFileAttribute = (SourceFileAttribute)attribute;
                            _sourceFile = GetConstantPoolUtf8String(sourceFileAttribute.SourceFile);
                            break;
                        case AttributeName.InnerClasses:
                            if (MajorVersion < 49)
                                goto default;

                            var innerClassesAttribute = (InnerClassesAttribute)attribute;
                            _innerClasses = new InnerClass[innerClassesAttribute.Table.Count];
                            for (int j = 0; j < _innerClasses.Length; j++)
                            {
                                var item = innerClassesAttribute.Table[j];

                                _innerClasses[j].innerClass = item.Inner;
                                _innerClasses[j].outerClass = item.Outer;
                                _innerClasses[j].name = item.InnerName;
                                _innerClasses[j].accessFlags = ReadAccessFlags(item.InnerAccessFlags);

                                if (_innerClasses[j].innerClass.IsNotNil && GetConstantPoolItem(_innerClasses[j].innerClass) is not ConstantPoolItemClass<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)
                                    throw new ClassFormatException("{0} (inner_class_info_index has bad constant pool index)", Name);

                                if (_innerClasses[j].outerClass.IsNotNil && GetConstantPoolItem(_innerClasses[j].outerClass) is not ConstantPoolItemClass<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)
                                    throw new ClassFormatException("{0} (outer_class_info_index has bad constant pool index)", Name);

                                if (_innerClasses[j].name.IsNotNil && _constants.GetUtf8(_innerClasses[j].name) == null)
                                    throw new ClassFormatException("{0} (inner class name has bad constant pool index)", Name);

                                if (_innerClasses[j].innerClass == _innerClasses[j].outerClass)
                                    throw new ClassFormatException("{0} (Class is both inner and outer class)", Name);

                                if (_innerClasses[j].innerClass.IsNotNil && _innerClasses[j].outerClass.IsNotNil)
                                {
                                    MarkLinkRequiredConstantPoolItem(_innerClasses[j].innerClass);
                                    MarkLinkRequiredConstantPoolItem(_innerClasses[j].outerClass);
                                }
                            }

                            break;
                        case AttributeName.Signature:
                            if (decoder.Version < 49)
                                goto default;

                            var signatureAttribute = (SignatureAttribute)attribute;
                            _signature = GetConstantPoolUtf8String(signatureAttribute.Signature);
                            break;
                        case AttributeName.EnclosingMethod:
                            if (decoder.Version < 49)
                                goto default;

                            var enclosingMethodAttribute = (EnclosingMethodAttribute)attribute;
                            var classHandle = enclosingMethodAttribute.Class;
                            var methodHandle = enclosingMethodAttribute.Method;
                            ValidateConstantPoolItemClass(inputClassName, classHandle);

                            if (methodHandle.IsNil)
                            {
                                _enclosingMethod =
                                [
                                    GetConstantPoolClass(classHandle),
                                    null,
                                    null
                                ];
                            }
                            else
                            {
                                if (GetConstantPoolItem(methodHandle) is not ConstantPoolItemNameAndType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> m)
                                    throw new ClassFormatException("{0} (Bad constant pool index #{1})", inputClassName, methodHandle);

                                _enclosingMethod =
                                [
                                    GetConstantPoolClass(classHandle),
                                    GetConstantPoolUtf8String(m._nameHandle),
                                    GetConstantPoolUtf8String(m._descriptorHandle).Replace('/', '.')
                                ];
                            }

                            break;
                        case AttributeName.RuntimeVisibleAnnotations:
                            if (decoder.Version < 49)
                                goto default;

                            var runtimeVisibleAnnotationsAttribute = (RuntimeVisibleAnnotationsAttribute)attribute;
                            _annotations = ReadAnnotations(runtimeVisibleAnnotationsAttribute.Annotations, this);
                            break;
                        case AttributeName.RuntimeInvisibleAnnotations:
                            if (decoder.Version < 49)
                                goto default;

                            var runtimeInvisibleAnnotationsAttribute = (RuntimeInvisibleAnnotationsAttribute)attribute;
                            foreach (var annot in ReadAnnotations(runtimeInvisibleAnnotationsAttribute.Annotations, this))
                            {
                                if (annot[1].Equals("Likvm/lang/Internal;"))
                                {
                                    _accessFlags &= ~ClassFileAccessFlags.AccessMask;
                                    _flags |= ClassFileFlags.MASK_INTERNAL;
                                }
                            }

                            break;
                        case AttributeName.BootstrapMethods:
                            if (decoder.Version < 51)
                                goto default;

                            var bootstrapMethodsAttribute = (BootstrapMethodsAttribute)attribute;
                            _bootstrapMethods = ReadBootstrapMethods(bootstrapMethodsAttribute.Methods, this);
                            break;
                        case AttributeName.RuntimeVisibleTypeAnnotations:
                            if (decoder.Version < 52)
                                goto default;

                            var _runtimeVisibleTypeAnnotations = (RuntimeVisibleTypeAnnotationsAttribute)attribute;
                            _constants.CreateUtf8ConstantPoolItems();
                            this._runtimeVisibleTypeAnnotations = _runtimeVisibleTypeAnnotations.TypeAnnotations;
                            break;
                        case "IKVM.NET.Assembly":
                            if (attribute.Data.Length != 2)
                                throw new ClassFormatException("IKVM.NET.Assembly attribute has incorrect length");

                            var r = new ClassFormatReader(attribute.Data);
                            if (r.TryReadU2(out var index) == false)
                                throw new ClassFormatException("IKVM.NET.Assembly attribute has incorrect length");

                            _ikvmAssembly = GetConstantPoolUtf8String(new(index));
                            break;
                        default:
                            break;
                    }
                }

                // validate the invokedynamic entries to point into the bootstrapMethods array
                foreach (var handle in _decoder.Constants)
                    if (_constants[handle] != null && _constants[handle] is ConstantPoolItemInvokeDynamic<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> cpi)
                        if (cpi.BootstrapMethod >= _bootstrapMethods.Count)
                            throw new ClassFormatException("Short length on BootstrapMethods in class file");
            }
            catch (OverflowException)
            {
                throw new ClassFormatException("Truncated class file (or section)");
            }
            catch (IndexOutOfRangeException)
            {
                throw new ClassFormatException("Unspecified class file format error");
            }
            catch (ByteCodeException)
            {
                throw new ClassFormatException("Unspecified class file format error");
            }
        }

        /// <summary>
        /// Gets the <see cref="ILinkingContext{TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod}"/> that holds this class file.
        /// </summary>
        internal ILinkingContext<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> Context => _context;

        /// <summary>
        /// Gets the underlying byte code decoder.
        /// </summary>
        internal IKVM.ByteCode.Decoding.ClassFile Decoder => _decoder;

        /// <summary>
        /// Gets the parse options.
        /// </summary>
        internal ClassFileParseOptions Options => _options;

        /// <summary>
        /// Gets the constant pool.
        /// </summary>
        internal ConstantPool<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> Constants => _constants;

        void CheckDuplicates<T>(T[] members, string msg)
            where T : IEquatable<T>
        {
            if (members.Length < 100)
            {
                for (int i = 0; i < members.Length; i++)
                    for (int j = 0; j < i; j++)
                        if (members[i].Equals(members[j]))
                            throw new ClassFormatException("{0} ({1})", Name, msg);
            }
            else
            {
                var hs = new HashSet<T>();
                for (int i = 0; i < members.Length; i++)
                    if (hs.Add(members[i]) == false)
                        throw new ClassFormatException("{0} ({1})", Name, msg);
            }
        }

        internal void MarkLinkRequiredConstantPoolItem(ConstantHandle handle)
        {
            if (handle.Slot > 0 && handle.Slot < Constants.SlotCount && Constants[handle] != null)
                Constants[handle].MarkLinkRequired();
        }

        internal void MarkLinkRequiredConstantPoolItem(int index)
        {
            MarkLinkRequiredConstantPoolItem(new ConstantHandle(ConstantKind.Unknown, checked((ushort)index)));
        }

        static BootstrapMethodTable ReadBootstrapMethods(BootstrapMethodTable bootstrapMethods, ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile)
        {
            foreach (var bootstrapMethod in bootstrapMethods)
            {
                if (bootstrapMethod.Method.Slot >= classFile.Constants.SlotCount || classFile.Constants[bootstrapMethod.Method] is not ConstantPoolItemMethodHandle<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)
                    throw new ClassFormatException("bootstrap_method_index {0} has bad constant type in class file {1}", bootstrapMethod.Method.Slot, classFile.Name);

                classFile.MarkLinkRequiredConstantPoolItem(bootstrapMethod.Method);

                foreach (var argument in bootstrapMethod.Arguments)
                {
                    if (classFile.IsValidConstant(argument) == false)
                        throw new ClassFormatException("argument_index {0} has bad constant type in class file {1}", argument.Slot, classFile.Name);

                    classFile.MarkLinkRequiredConstantPoolItem(argument);
                }
            }

            return bootstrapMethods;
        }

        bool IsValidConstant(ConstantHandle handle)
        {
            if (handle.Slot < _constants.SlotCount && _constants[handle] != null)
            {
                try
                {
                    _ = _constants[handle].ConstantType;
                    return true;
                }
                catch (InvalidOperationException)
                {

                }
            }

            return false;
        }

        internal static object[][] ReadAnnotations(AnnotationTable reader, ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile)
        {
            var annotations = new object[reader.Count][];

            for (int i = 0; i < annotations.Length; i++)
                annotations[i] = ReadAnnotation(reader[i], classFile);

            return annotations;
        }

        internal static object[] ReadAnnotation(Annotation annotation, ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile)
        {
            var l = new object[2 + annotation.Elements.Count * 2];
            l[0] = AnnotationTag.TAG_ANNOTATION;
            l[1] = classFile.GetConstantPoolUtf8String(annotation.Type);
            for (int i = 0; i < annotation.Elements.Count; i++)
            {
                l[2 + i * 2 + 0] = classFile.GetConstantPoolUtf8String(annotation.Elements[i].Name);
                l[2 + i * 2 + 1] = ReadAnnotationElementValue(annotation.Elements[i].Value, classFile);
            }

            return l;
        }

        internal static object ReadAnnotationElementValue(in ElementValue reader, ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile)
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
                        return classFile.GetConstantPoolUtf8String((Utf8ConstantHandle)((ConstantElementValue)reader).Handle);
                    case ElementValueKind.Enum:
                        var _enum = (EnumElementValue)reader;
                        return new object[] {
                            AnnotationTag.TAG_ENUM,
                            classFile.GetConstantPoolUtf8String( _enum.TypeName),
                            classFile.GetConstantPoolUtf8String( _enum.ConstantName)
                        };
                    case ElementValueKind.Class:
                        var _class = (ClassElementValue)reader;
                        return new object[] {
                            AnnotationTag.TAG_CLASS,
                            classFile.GetConstantPoolUtf8String( _class.Class)
                        };
                    case ElementValueKind.Annotation:
                        return ReadAnnotation(((AnnotationElementValue)reader).Annotation, classFile);
                    case ElementValueKind.Array:
                        var _array = (ArrayElementValue)reader;

                        var array = new object[_array.Count + 1];
                        array[0] = AnnotationTag.TAG_ARRAY;
                        for (int i = 0; i < _array.Count; i++)
                            array[i + 1] = ReadAnnotationElementValue(_array[i], classFile);

                        return array;
                    default:
                        throw new ClassFormatException("Invalid tag {0} in annotation element_value", reader.Kind);
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

            return new object[] {
                AnnotationTag.TAG_ERROR,
                "java.lang.IllegalArgumentException",
                "Wrong type at constant pool index"
            };
        }

        void ValidateConstantPoolItemClass(string classFile, ClassConstantHandle handle)
        {
            if (handle.Slot >= _constants.SlotCount || _constants[handle] is not ConstantPoolItemClass<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)
                throw new ClassFormatException("{0} (Bad constant pool index #{1})", classFile, handle.Slot);
        }

        /// <summary>
        /// Gets the major version of the class.
        /// </summary>
        public int MajorVersion => _decoder.Version.Major;

        /// <summary>
        /// Initiates linkage of this class file to the specified java type instance.
        /// </summary>
        /// <param name="thisType"></param>
        /// <param name="mode"></param>
        public void Link(TLinkingType thisType, LoadMode mode)
        {
            // this is not just an optimization, it's required for anonymous classes to be able to refer to themselves
            ((ConstantPoolItemClass<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)_constants[_decoder.This]).LinkSelf(thisType);

            foreach (var handle in _decoder.Constants)
                _constants[handle]?.Link(thisType, mode);
        }

        /// <summary>
        /// Gets the modifiers of the class.
        /// </summary>
        public ClassFileAccessFlags AccessFlags => _accessFlags;

        /// <summary>
        /// Gets whether this class file represents an abstract class.
        /// </summary>
        public bool IsAbstract => (_accessFlags & (ClassFileAccessFlags.Abstract | ClassFileAccessFlags.Interface)) != 0;

        /// <summary>
        /// Gets whether this class file represents a final class.
        /// </summary>
        public bool IsFinal => (_accessFlags & ClassFileAccessFlags.Final) != 0;

        /// <summary>
        /// Gets whether this class file represents a public class.
        /// </summary>
        public bool IsPublic => (_accessFlags & ClassFileAccessFlags.Public) != 0;

        /// <summary>
        /// Gets whether this class file represents an interface.
        /// </summary>
        public bool IsInterface => (_accessFlags & ClassFileAccessFlags.Interface) != 0;

        /// <summary>
        /// Gets whether this class file represents an enum.
        /// </summary>
        public bool IsEnum => (_accessFlags & ClassFileAccessFlags.Enum) != 0;

        /// <summary>
        /// Gets whether this class file represents an annotation.
        /// </summary>
        public bool IsAnnotation => (_accessFlags & ClassFileAccessFlags.Annotation) != 0;

        /// <summary>
        /// Gets whether this class file is a super.
        /// </summary>
        public bool IsSuper => (_accessFlags & ClassFileAccessFlags.Super) != 0;

        /// <summary>
        /// Returns <c>true</c> if the specified class field is referenced within the class file.
        /// </summary>
        /// <param name="fld"></param>
        /// <returns></returns>
        internal bool IsReferenced(Field<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> fld) => _constants.OfType<ConstantPoolItemFieldref<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>>().Any(i => i.Class == Name && i.Name == fld.Name && i.Signature == fld.Signature);

        /// <summary>
        /// Gets the field ref constant for the given handle.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal ConstantPoolItemFieldref<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> GetFieldref(FieldrefConstantHandle handle)
        {
            return (ConstantPoolItemFieldref<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)_constants[handle];
        }

        /// <summary>
        /// Gets the field ref constant for the given handle.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        internal ConstantPoolItemFieldref<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> GetFieldref(ushort slot)
        {
            return GetFieldref(new FieldrefConstantHandle(slot));
        }

        /// <summary>
        /// Version of GetFieldref that does not throw if the handle is invalid. Used by IsSideEffectFreeStaticInitializer.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal ConstantPoolItemFieldref<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>? SafeGetFieldref(ConstantHandle handle)
        {
            if (handle.IsNotNil && handle.Slot < _constants.SlotCount)
                return _constants[handle] as ConstantPoolItemFieldref<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>;

            return null;
        }

        /// <summary>
        /// Version of GetFieldref that does not throw if the handle is invalid. Used by IsSideEffectFreeStaticInitializer.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal ConstantPoolItemFieldref<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>? SafeGetFieldref(ushort index)
        {
            return SafeGetFieldref(new ConstantHandle(ConstantKind.Unknown, index));
        }

        internal ConstantPoolItemMI<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> GetMethodref(MethodrefConstantHandle handle)
        {
            return (ConstantPoolItemMI<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)_constants[handle];
        }

        // NOTE this returns an MI, because it used for both normal methods and interface methods
        internal ConstantPoolItemMI<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> GetMethodref(int handle)
        {
            return GetMethodref(new MethodrefConstantHandle(checked((ushort)handle)));
        }

        /// <summary>
        /// Version of GetMethodref that does not throw if the handle is invalid. Used by IsAccessBridge.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal ConstantPoolItemMI<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>? SafeGetMethodref(ConstantHandle handle)
        {
            if (handle.IsNotNil && handle.Slot < _constants.SlotCount)
                return _constants[handle] as ConstantPoolItemMI<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>;

            return null;
        }

        /// <summary>
        /// Version of GetMethodref that does not throw if the handle is invalid. Used by IsAccessBridge.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        internal ConstantPoolItemMI<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>? SafeGetMethodref(int slot)
        {
            if (slot > ushort.MaxValue || slot < ushort.MinValue)
                return null;

            return SafeGetMethodref(new ConstantHandle(ConstantKind.Unknown, (ushort)slot));
        }

        internal ConstantPoolItemInvokeDynamic<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> GetInvokeDynamic(InvokeDynamicConstantHandle handle)
        {
            return (ConstantPoolItemInvokeDynamic<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)_constants[handle];
        }

        internal ConstantPoolItem<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> GetConstantPoolItem(ConstantHandle handle)
        {
            return _constants[handle];
        }

        internal string GetConstantPoolClass(ClassConstantHandle handle)
        {
            return ((ConstantPoolItemClass<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)_constants[handle]).Name;
        }

        internal bool SafeIsConstantPoolClass(ClassConstantHandle handle)
        {
            if (handle.Slot > 0 && handle.Slot < _constants.SlotCount)
                return _constants[handle] as ConstantPoolItemClass<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> != null;

            return false;
        }

        internal TLinkingType GetConstantPoolClassType(ClassConstantHandle handle)
        {
            return ((ConstantPoolItemClass<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)_constants[handle]).GetClassType();
        }

        internal TLinkingType GetConstantPoolClassType(int slot)
        {
            return GetConstantPoolClassType(new ClassConstantHandle(checked((ushort)slot)));
        }

        internal string GetConstantPoolUtf8String(Utf8ConstantHandle handle)
        {
            var s = _constants.GetUtf8(handle);
            if (s == null)
            {
                if (_decoder.This.IsNil)
                    throw new ClassFormatException("Bad constant pool index #{0}", handle);
                else
                    throw new ClassFormatException("{0} (Bad constant pool index #{1})", Name, handle);
            }

            return s;
        }

        internal ConstantType GetConstantPoolConstantType(ConstantHandle handle)
        {
            return _constants[handle].ConstantType;
        }

        internal ConstantType GetConstantPoolConstantType(int slot)
        {
            return GetConstantPoolConstantType(new ConstantHandle(ConstantKind.Unknown, checked((ushort)slot)));
        }

        internal double GetConstantPoolConstantDouble(DoubleConstantHandle handle)
        {
            return ((ConstantPoolItemDouble<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)_constants[handle]).Value;
        }

        internal float GetConstantPoolConstantFloat(FloatConstantHandle handle)
        {
            return ((ConstantPoolItemFloat<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)_constants[handle]).Value;
        }

        internal int GetConstantPoolConstantInteger(IntegerConstantHandle handle)
        {
            return ((ConstantPoolItemInteger<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)_constants[handle]).Value;
        }

        internal long GetConstantPoolConstantLong(LongConstantHandle handle)
        {
            return ((ConstantPoolItemLong<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)_constants[handle]).Value;
        }

        internal string GetConstantPoolConstantString(StringConstantHandle handle)
        {
            return ((ConstantPoolItemString<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)_constants[handle]).Value;
        }

        internal string GetConstantPoolConstantString(int slot)
        {
            return GetConstantPoolConstantString(new StringConstantHandle(checked((ushort)slot)));
        }

        internal ConstantPoolItemMethodHandle<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> GetConstantPoolConstantMethodHandle(MethodHandleConstantHandle handle)
        {
            return (ConstantPoolItemMethodHandle<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)_constants[handle];
        }

        internal ConstantPoolItemMethodHandle<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> GetConstantPoolConstantMethodHandle(ushort slot)
        {
            return GetConstantPoolConstantMethodHandle(new MethodHandleConstantHandle(slot));
        }

        internal ConstantPoolItemMethodType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> GetConstantPoolConstantMethodType(MethodTypeConstantHandle handle)
        {
            return (ConstantPoolItemMethodType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)_constants[handle];
        }

        internal ConstantPoolItemMethodType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> GetConstantPoolConstantMethodType(ushort slot)
        {
            return GetConstantPoolConstantMethodType(new MethodTypeConstantHandle(slot));
        }

        internal object GetConstantPoolConstantLiveObject(ushort slot)
        {
            return ((ConstantPoolItemLiveObject<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)_constants[new(ConstantKind.Unknown, slot)]).Value;
        }

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        internal string Name => GetConstantPoolClass(_decoder.This);

        /// <summary>
        /// Gets the constant pool item that represents the super class.
        /// </summary>
        internal ConstantPoolItemClass<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> SuperClass => (ConstantPoolItemClass<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)_constants[_decoder.Super];

        /// <summary>
        /// Gets the fields of the class.
        /// </summary>
        internal Field<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>[] Fields => _fields;

        /// <summary>
        /// Gets the methods of the class.
        /// </summary>
        internal Method<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>[] Methods => _methods;

        /// <summary>
        /// Gets the interfaces of the class.
        /// </summary>
        internal ConstantPoolItemClass<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>[] Interfaces => _interfaces;

        internal string? SourceFileAttribute => _sourceFile;

        internal string? SourcePath
        {
            get { return _sourcePath; }
            set { _sourcePath = value; }
        }

        internal object[]? Annotations => _annotations;

        internal string? GenericSignature => _signature;

        internal string?[]? EnclosingMethod => _enclosingMethod;

        internal ref readonly TypeAnnotationTable RuntimeVisibleTypeAnnotations => ref _runtimeVisibleTypeAnnotations;

        /// <summary>
        /// Creates a copy of the runtime values of the constant pool.
        /// </summary>
        /// <returns></returns>
        internal object?[] GetConstantPool()
        {
            var cp = new object?[Constants.SlotCount];

            foreach (var handle in Decoder.Constants)
                if (Constants[handle] != null)
                    cp[handle.Slot] = Constants[handle].GetRuntimeValue();

            return cp;
        }

        internal string? IKVMAssemblyAttribute => _ikvmAssembly;

        internal bool DeprecatedAttribute => (_flags & ClassFileFlags.MASK_DEPRECATED) != 0;

        /// <summary>
        /// Gets whether this class is an internal class.
        /// </summary>
        internal bool IsInternal => (_flags & ClassFileFlags.MASK_INTERNAL) != 0;

        // for use by ikvmc (to implement the -privatepackage option)
        internal void SetInternal()
        {
            _accessFlags &= ~ClassFileAccessFlags.AccessMask;
            _flags |= ClassFileFlags.MASK_INTERNAL;
        }

        internal bool HasAssertions => (_flags & ClassFileFlags.HAS_ASSERTIONS) != 0;

        internal bool HasInitializedFields
        {
            get
            {
                foreach (var f in _fields)
                    if (f.IsStatic && !f.IsFinal && f.ConstantValue != null)
                        return true;

                return false;
            }
        }

        internal BootstrapMethod GetBootstrapMethod(int index)
        {
            return _bootstrapMethods[index];
        }

        internal InnerClass[]? InnerClasses => _innerClasses;

        internal Field<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>? GetField(string name, string sig)
        {
            for (int i = 0; i < _fields.Length; i++)
                if (_fields[i].Name == name && _fields[i].Signature == sig)
                    return _fields[i];

            return null;
        }

        /// <summary>
        /// Removes a call to java.lang.Class.desiredAssertionStatus() and replaces it with a hard coded constant (true).
        /// </summary>
        /// <param name="method"></param>
        void RemoveAssertionInit(Method<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> method)
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
            ConstantPoolItemFieldref<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>? fieldref;
            Field<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> field;
            if (method.Instructions is [
                { NormalizedOpCode: NormalizedOpCode.Ldc },
                { NormalizedOpCode: NormalizedOpCode.InvokeVirtual },
                { NormalizedOpCode: NormalizedOpCode.Ifne },
                { NormalizedOpCode: NormalizedOpCode.Iconst },
                { NormalizedOpCode: NormalizedOpCode.Goto },
                { NormalizedOpCode: NormalizedOpCode.Iconst },
                { NormalizedOpCode: NormalizedOpCode.PutStatic },
                ..] &&
                method.Instructions[0].NormalizedOpCode == NormalizedOpCode.Ldc && SafeIsConstantPoolClass(new ClassConstantHandle(checked((ushort)method.Instructions[0].Arg1))) &&
                method.Instructions[1].NormalizedOpCode == NormalizedOpCode.InvokeVirtual && IsDesiredAssertionStatusMethodref(method.Instructions[1].Arg1) &&
                method.Instructions[2].NormalizedOpCode == NormalizedOpCode.Ifne && method.Instructions[2].TargetIndex == 5 &&
                method.Instructions[3].NormalizedOpCode == NormalizedOpCode.Iconst && method.Instructions[3].Arg1 == 1 &&
                method.Instructions[4].NormalizedOpCode == NormalizedOpCode.Goto && method.Instructions[4].TargetIndex == 6 &&
                method.Instructions[5].NormalizedOpCode == NormalizedOpCode.Iconst && method.Instructions[5].Arg1 == 0 &&
                method.Instructions[6].NormalizedOpCode == NormalizedOpCode.PutStatic && (fieldref = SafeGetFieldref(checked((ushort)method.Instructions[6].Arg1))) != null &&
                fieldref.Class == Name && fieldref.Signature == "Z" &&
                (field = GetField(fieldref.Name, fieldref.Signature)) != null &&
                field.IsStatic && field.IsFinal &&
                !HasBranchIntoRegion(method.Instructions, 7, method.Instructions.Length, 0, 7) &&
                !HasStaticFieldWrite(method.Instructions, 7, method.Instructions.Length, field) &&
                !HasExceptionHandlerInRegion(method.ExceptionTable, 0, 7))
            {
                field.PatchConstantValue(true);
                method.Instructions[0].PatchOpCode(NormalizedOpCode.Goto, 7);
                _flags |= ClassFileFlags.HAS_ASSERTIONS;
            }
        }

        bool IsDesiredAssertionStatusMethodref(int cpi)
        {
            return SafeGetMethodref(cpi) is ConstantPoolItemMethodref<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> { Class: "java.lang.Class", Name: "desiredAssertionStatus", Signature: "()Z" };
        }

        static bool HasBranchIntoRegion(Instruction<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>[] instructions, int checkStart, int checkEnd, int regionStart, int regionEnd)
        {
            for (int i = checkStart; i < checkEnd; i++)
            {
                switch (instructions[i].NormalizedOpCode)
                {
                    case NormalizedOpCode.Ifeq:
                    case NormalizedOpCode.Ifne:
                    case NormalizedOpCode.Iflt:
                    case NormalizedOpCode.Ifge:
                    case NormalizedOpCode.Ifgt:
                    case NormalizedOpCode.Ifle:
                    case NormalizedOpCode.IfIcmpeq:
                    case NormalizedOpCode.IfIcmpne:
                    case NormalizedOpCode.IfIcmplt:
                    case NormalizedOpCode.IfIcmpge:
                    case NormalizedOpCode.IfIcmpgt:
                    case NormalizedOpCode.IfIcmple:
                    case NormalizedOpCode.IfAcmpeq:
                    case NormalizedOpCode.IfAcmpne:
                    case NormalizedOpCode.IfNull:
                    case NormalizedOpCode.IfNonNull:
                    case NormalizedOpCode.Goto:
                    case NormalizedOpCode.Jsr:
                        if (instructions[i].TargetIndex > regionStart && instructions[i].TargetIndex < regionEnd)
                            return true;

                        break;
                    case NormalizedOpCode.TableSwitch:
                    case NormalizedOpCode.LookupSwitch:
                        if (instructions[i].DefaultTarget > regionStart && instructions[i].DefaultTarget < regionEnd)
                            return true;

                        for (int j = 0; j < instructions[i].SwitchEntryCount; j++)
                            if (instructions[i].GetSwitchTargetIndex(j) > regionStart && instructions[i].GetSwitchTargetIndex(j) < regionEnd)
                                return true;

                        break;
                }
            }
            return false;
        }

        bool HasStaticFieldWrite(Instruction<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>[] instructions, int checkStart, int checkEnd, Field<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> field)
        {
            for (int i = checkStart; i < checkEnd; i++)
            {
                if (instructions[i].NormalizedOpCode == NormalizedOpCode.PutStatic)
                {
                    var fieldref = SafeGetFieldref(checked((ushort)instructions[i].Arg1));
                    if (fieldref != null && fieldref.Class == Name && fieldref.Name == field.Name && fieldref.Signature == field.Signature)
                        return true;
                }
            }
            return false;
        }

        static bool HasExceptionHandlerInRegion(ExceptionTableEntry[] entries, int regionStart, int regionEnd)
        {
            for (int i = 0; i < entries.Length; i++)
                if (entries[i].HandlerIndex > regionStart && entries[i].HandlerIndex < regionEnd)
                    return true;

            return false;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _decoder.Dispose();
        }

    }

}
