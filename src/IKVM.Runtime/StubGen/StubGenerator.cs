using System;
using System.Collections.Generic;
using System.IO;

using IKVM.Attributes;
using IKVM.ByteCode;
using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Decoding;
using IKVM.ByteCode.Encoding;
using IKVM.CoreLib.Symbols;

namespace IKVM.Runtime.StubGen
{

    class StubGenerator
    {

        readonly RuntimeContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public StubGenerator(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Generates a class file stub for the specified <see cref="RuntimeJavaType"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <param name="includeNonPublicTypes"></param>
        /// <param name="includeNonPublicInterfaces"></param>
        /// <param name="includeNonPublicMembers"></param>
        /// <param name="includeParameterNames"></param>
        /// <param name="includeSerialVersionUID"></param>
        internal void Write(Stream stream, RuntimeJavaType type, bool includeNonPublicTypes, bool includeNonPublicInterfaces, bool includeNonPublicMembers, bool includeParameterNames, bool includeSerialVersionUID)
        {
            var name = type.Name.Replace('.', '/');

            string super = null;
            if (type.IsInterface)
                super = "java/lang/Object";
            else if (type.BaseTypeWrapper != null)
                super = type.BaseTypeWrapper.Name.Replace('.', '/');

            var builder = new ClassFileBuilder(new ClassFormatVersion(includeParameterNames ? (ushort)52 : (ushort)49, 0), (AccessFlag)type.Modifiers, name, super);

            AddInterfaces(builder, type, includeNonPublicInterfaces);
            AddMethods(builder, type, includeNonPublicMembers, includeParameterNames);
            AddFields(builder, type, includeNonPublicMembers, includeSerialVersionUID);
            AddInnerClassesAttribute(builder, type, name, includeNonPublicTypes);
            AddSignatureAttribute(builder, type);
            AddAssemblyAttribute(builder, type);
            AddDeprecatedAttribute(builder, type);
            AddRuntimeVisibleAnnotationsAttribute(builder, type);
            AddRuntimeVisibleTypeAnnotationsAttribute(builder, type);

            // serialize final class file
            var blob = new BlobBuilder();
            builder.Serialize(blob);
            blob.WriteContentTo(stream);
        }

        /// <summary>
        /// Adds the Interfaces available to the type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="includeNonPublicInterfaces"></param>
        void AddInterfaces(ClassFileBuilder builder, RuntimeJavaType type, bool includeNonPublicInterfaces)
        {
            foreach (var iface in type.Interfaces)
                if (iface.IsPublic || includeNonPublicInterfaces)
                    builder.AddInterface(iface.Name.Replace('.', '/'));
        }

        /// <summary>
        /// Adds the methods for a type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="includeNonPublicMembers"></param>
        /// <param name="includeParameterNames"></param>
        void AddMethods(ClassFileBuilder builder, RuntimeJavaType type, bool includeNonPublicMembers, bool includeParameterNames)
        {
            foreach (var method in type.GetMethods())
            {
                var accessFlags = (AccessFlag)method.Modifiers;
                var attributes = new AttributeTableBuilder(builder.Constants);

                if (method.IsHideFromReflection == false && (method.IsPublic || method.IsProtected || includeNonPublicMembers))
                {
                    // HACK javac has a bug in com.sun.tools.javac.code.Types.isSignaturePolymorphic() where it assumes that
                    // MethodHandle doesn't have any native methods with an empty argument list
                    // (or at least it throws a NPE when it examines the signature of a method without any parameters when it
                    // accesses argtypes.tail.tail)
                    if (method.Name == "<init>" || (type == context.JavaBase.TypeOfJavaLangInvokeMethodHandle && (method.Modifiers & Modifiers.Native) == 0))
                    {
                        // generate the method code which throws a UnsatisfiedLinkError.
                        var codeBlob = new BlobBuilder();
                        new CodeBuilder(codeBlob)
                            .New(builder.Constants.GetOrAddClass("java/lang/UnsatisfiedLinkError"))
                            .Dup()
                            .LoadConstant(builder.Constants.GetOrAddString("IKVM stubs can only be used on IKVM"))
                            .InvokeSpecial(builder.Constants.GetOrAddMethodref("java/lang/UnsatisfiedLinkError", "<init>", "(Ljava/lang/String;)V"))
                            .Athrow();

                        attributes.Code(3, (ushort)(method.GetParameters().Length * 2 + 1), codeBlob, e => { }, new AttributeTableBuilder(builder.Constants));
                    }
                    else
                    {
                        if ((accessFlags & AccessFlag.Abstract) == 0)
                            accessFlags |= AccessFlag.Native;

                        if (method.IsOptionalAttributeAnnotationValue)
                            attributes.AnnotationDefault(e => EncodeAnnotationDefault(builder, ref e, method.ReturnType));
                    }

                    var methodBase = method.GetMethod();
                    if (methodBase != null)
                    {
                        AddExceptionsAttribute(builder, type, method, attributes, methodBase);
                        AddDeprecatedAttribute(builder, type, method, attributes, methodBase);
                        AddAnnotationDefaultAttribute(builder, type, method, attributes, methodBase);
                        AddMethodParameters(builder, type, method, attributes, includeParameterNames);
                    }

                    AddSignatureAttribute(builder, type, method, attributes);
                    AddRuntimeVisibleAnnotationsAttribute(builder, type, method, attributes);
                    AddRuntimeVisibleTypeAnnotationsAttribute(builder, type, method, attributes);
                    AddRuntimeVisibleParameterAnnotationsAttribute(builder, type, method, attributes);

                    builder.AddMethod(accessFlags, method.Name, method.Signature.Replace('.', '/'), attributes);
                }
            }
        }

        /// <summary>
        /// Adds the fields for a type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="includeNonPublicMembers"></param>
        /// <param name="includeSerialVersionUID"></param>
        void AddFields(ClassFileBuilder builder, RuntimeJavaType type, bool includeNonPublicMembers, bool includeSerialVersionUID)
        {
            var hasSerialVersionUID = false;

            foreach (var field in type.GetFields())
            {
                if (field.IsHideFromReflection == false)
                {
                    var attributes = new AttributeTableBuilder(builder.Constants);

                    var isSerialVersionUID = includeSerialVersionUID && field.IsSerialVersionUID;
                    hasSerialVersionUID |= isSerialVersionUID;

                    if (field.IsPublic || field.IsProtected || isSerialVersionUID || includeNonPublicMembers)
                    {
                        AddConstantValueAttribute(builder, type, field, attributes);
                        AddSignatureAttribute(builder, type, field, attributes);
                        AddDeprecatedAttribute(builder, type, field, attributes);
                        AddRuntimeVisibleAnnotationsAttribute(builder, type, field, attributes);
                        AddRuntimeVisibleTypeAnnotationsAttribute(builder, type, field, attributes);
                        builder.AddField((AccessFlag)field.Modifiers, field.Name, field.Signature.Replace('.', '/'), attributes);
                    }
                }
            }

            // class is serializable but doesn't have an explicit serialVersionUID, so we add the field to record
            // the serialVersionUID as we see it (mainly to make the Japi reports more realistic)
            if (includeSerialVersionUID && hasSerialVersionUID == false && IsSerializable(type))
            {
                var fieldAttributes = new AttributeTableBuilder(builder.Constants);
                fieldAttributes.ConstantValue(SerialVersionUID.Compute(type));
                builder.AddField(AccessFlag.Private | AccessFlag.Static | AccessFlag.Final, "serialVersionUID", "J", fieldAttributes);
            }
        }

        /// <summary>
        /// Adds the RuntimeVisibleTypeAnnotations attribute for a type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        void AddRuntimeVisibleTypeAnnotationsAttribute(ClassFileBuilder builder, RuntimeJavaType type)
        {
            var blob = new BlobBuilder();
            var encoder = new TypeAnnotationTableEncoder(blob);
            if (ImportTypeAnnotations(builder, ref encoder, type, type.GetRawTypeAnnotations()))
                builder.Attributes.Attribute(AttributeName.RuntimeVisibleTypeAnnotations, blob);
        }

        /// <summary>
        /// Adds the RuntimeVisibleAnnotations attribute for a type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        void AddRuntimeVisibleAnnotationsAttribute(ClassFileBuilder builder, RuntimeJavaType type)
        {
            var blob = new BlobBuilder();
            var encoder = new AnnotationTableEncoder(blob);
            if (EncodeAnnotations(builder, ref encoder, type.TypeAsBaseType) ||
                EncodeAnnotationsForType(builder, ref encoder, type))
                builder.Attributes.Attribute(AttributeName.RuntimeVisibleAnnotations, blob);
        }

        /// <summary>
        /// Adds the Deprecated attribute.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        void AddDeprecatedAttribute(ClassFileBuilder builder, RuntimeJavaType type)
        {
            if (type.TypeAsBaseType.IsDefined(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName), false))
                builder.Attributes.Deprecated();
        }

        /// <summary>
        /// Adds the InnerClasses attribute.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="javaType"></param>
        /// <param name="name"></param>
        /// <param name="includeNonPublicTypes"></param>
        void AddInnerClassesAttribute(ClassFileBuilder builder, RuntimeJavaType javaType, string name, bool includeNonPublicTypes)
        {
            var any = false;
            var blob = new BlobBuilder();
            var encoder = new InnerClassTableEncoder(blob);

            if (javaType.DeclaringTypeWrapper != null)
            {
                var innerName = name;
                int idx = name.LastIndexOf('$');
                if (idx >= 0)
                    innerName = innerName.Substring(idx + 1);

                encoder.InnerClass(
                    builder.Constants.GetOrAddClass(name),
                    builder.Constants.GetOrAddClass(javaType.DeclaringTypeWrapper.Name.Replace('.', '/')),
                    builder.Constants.GetOrAddUtf8(innerName),
                    (AccessFlag)javaType.ReflectiveModifiers);

                any = true;
            }

            foreach (var innerType in javaType.InnerClasses)
            {
                if (innerType.IsPublic || includeNonPublicTypes)
                {
                    var namePart = innerType.Name;
                    namePart = namePart.Substring(namePart.LastIndexOf('$') + 1);

                    encoder.InnerClass(
                        builder.Constants.GetOrAddClass(innerType.Name.Replace('.', '/')),
                        builder.Constants.GetOrAddClass(name),
                        builder.Constants.GetOrAddUtf8(namePart),
                        (AccessFlag)innerType.ReflectiveModifiers);

                    any = true;
                }
            }

            if (any)
                builder.Attributes.Attribute(AttributeName.InnerClasses, blob);
        }

        /// <summary>
        /// Adds the Signature attribute for a type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="javaType"></param>
        void AddSignatureAttribute(ClassFileBuilder builder, RuntimeJavaType javaType)
        {
            var signature = javaType.GetGenericSignature();
            if (signature != null)
                builder.Attributes.Signature(signature);
        }

        /// <summary>
        /// Adds the IKVM.NET.Assembly attribute for a type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="javaType"></param>
        void AddAssemblyAttribute(ClassFileBuilder builder, RuntimeJavaType javaType)
        {

            // consists of a U2 constant pointing to the full name of the assembly
            var assemblyAttributeBlob = new BlobBuilder();
            new ClassFormatWriter(assemblyAttributeBlob.ReserveBytes(ClassFormatWriter.U2).GetBytes()).WriteU2(builder.Constants.GetOrAddUtf8(GetAssemblyName(javaType)).Slot);
            builder.Attributes.Encoder.Attribute(builder.Constants.GetOrAddUtf8("IKVM.NET.Assembly"), assemblyAttributeBlob);
        }

        /// <summary>
        /// Adds the Exceptions attribute for the method.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="attributes"></param>
        /// <param name="methodBase"></param>
        void AddExceptionsAttribute(ClassFileBuilder builder, RuntimeJavaType type, RuntimeJavaMethod method, AttributeTableBuilder attributes, IMethodBaseSymbol methodBase)
        {
            var throws = context.AttributeHelper.GetThrows(methodBase);
            if (throws == null)
            {
                var throwsArray = method.GetDeclaredExceptions();
                if (throwsArray != null && throwsArray.Length > 0)
                {
                    attributes.Exceptions(e =>
                    {
                        foreach (string ex in throwsArray)
                            e.Class(builder.Constants.GetOrAddClass(ex.Replace('.', '/')));
                    });
                }
            }
            else
            {
                if (throws.classes != null || throws.types != null)
                {
                    attributes.Exceptions(e =>
                    {
                        if (throws.classes != null)
                            foreach (string ex in throws.classes)
                                e.Class(builder.Constants.GetOrAddClass(ex.Replace('.', '/')));

                        if (throws.types != null)
                            foreach (var ex in throws.types)
                                e.Class(builder.Constants.GetOrAddClass(context.ClassLoaderFactory.GetJavaTypeFromType(context.Resolver.GetSymbol(ex)).Name.Replace('.', '/')));
                    });
                }
            }
        }


        /// <summary>
        /// Adds the MethodParameters attribute for a method.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="attributes"></param>
        /// <param name="includeParameterNames"></param>
        void AddMethodParameters(ClassFileBuilder builder, RuntimeJavaType type, RuntimeJavaMethod method, AttributeTableBuilder attributes, bool includeParameterNames)
        {
            if (includeParameterNames)
            {
                var mp = type.GetMethodParameters(method);
                if (mp == MethodParametersEntry.Malformed)
                {
                    attributes.MethodParameters(e => { });
                }
                else if (mp != null)
                {
                    attributes.MethodParameters(e =>
                    {
                        foreach (var i in mp)
                            e.MethodParameter(builder.Constants.GetOrAddUtf8(i.name), i.accessFlags);
                    });
                }
            }
        }

        /// <summary>
        /// Adds the AnnotationDefault attribute for a method.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="attributes"></param>
        /// <param name="methodBase"></param>
        void AddAnnotationDefaultAttribute(ClassFileBuilder builder, RuntimeJavaType type, RuntimeJavaMethod method, AttributeTableBuilder attributes, IMethodBaseSymbol methodBase)
        {
            var attr = GetAnnotationDefault(methodBase);
            if (attr is CustomAttribute ca)
                attributes.AnnotationDefault(e => EncodeAnnotationDefault(builder, ref e, ca.ConstructorArguments[0]));
        }

        /// <summary>
        /// Adds the Signature attribute for a method.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="methodAttributes"></param>
        void AddSignatureAttribute(ClassFileBuilder builder, RuntimeJavaType type, RuntimeJavaMethod method, AttributeTableBuilder methodAttributes)
        {
            var signature = type.GetGenericMethodSignature(method);
            if (signature != null)
                methodAttributes.Signature(signature);
        }

        /// <summary>
        /// Adds the Deprecated attribute for a method.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="attributes"></param>
        /// <param name="methodBase"></param>
        void AddDeprecatedAttribute(ClassFileBuilder builder, RuntimeJavaType type, RuntimeJavaMethod method, AttributeTableBuilder attributes, IMethodBaseSymbol methodBase)
        {
            // HACK the instancehelper methods are marked as Obsolete (to direct people toward the ikvm.extensions methods instead)
            // but in the Java world most of them are not deprecated (and to keep the Japi results clean we need to reflect this)
            // the Java deprecated methods actually have two Obsolete attributes
            if (methodBase.IsDefined(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName), false) && (!methodBase.Name.StartsWith("instancehelper_") || methodBase.DeclaringType.FullName != "java.lang.String" || GetObsoleteCount(methodBase) == 2))
                attributes.Deprecated();
        }

        /// <summary>
        /// Adds the RuntimeVisibleParameterAnnotations attribute for a method.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="attributes"></param>
        void AddRuntimeVisibleParameterAnnotationsAttribute(ClassFileBuilder builder, RuntimeJavaType type, RuntimeJavaMethod method, AttributeTableBuilder attributes)
        {
            var blob = new BlobBuilder();
            var encoder = new ParameterAnnotationTableEncoder(blob);
            if (EncodeParameterAnnotations(builder, ref encoder, method.GetMethod()))
                attributes.Attribute(AttributeName.RuntimeVisibleParameterAnnotations, blob);
        }

        /// <summary>
        /// Adds the RuntimeVisibleTypeAnnotations attribute for a method.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="attributes"></param>
        void AddRuntimeVisibleTypeAnnotationsAttribute(ClassFileBuilder builder, RuntimeJavaType type, RuntimeJavaMethod method, AttributeTableBuilder attributes)
        {
            var blob = new BlobBuilder();
            var encoder = new TypeAnnotationTableEncoder(blob);
            if (ImportTypeAnnotations(builder, ref encoder, type, type.GetMethodRawTypeAnnotations(method)))
                attributes.Attribute(AttributeName.RuntimeVisibleTypeAnnotations, blob);
        }

        /// <summary>
        /// Adds the RuntimeVisibleAnnotations attribute for a method.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="attributes"></param>
        void AddRuntimeVisibleAnnotationsAttribute(ClassFileBuilder builder, RuntimeJavaType type, RuntimeJavaMethod method, AttributeTableBuilder attributes)
        {
            var blob = new BlobBuilder();
            var encoder = new AnnotationTableEncoder(blob);
            if (EncodeAnnotations(builder, ref encoder, method.GetMethod()))
                attributes.Attribute(AttributeName.RuntimeVisibleAnnotations, blob);
        }

        /// <summary>
        /// Adds the ConstantValue attribute for a field.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <param name="attributes"></param>
        /// <exception cref="Exception"></exception>
        void AddConstantValueAttribute(ClassFileBuilder builder, RuntimeJavaType type, RuntimeJavaField field, AttributeTableBuilder attributes)
        {
            if (field.GetField() != null && field.GetField().IsLiteral && (field.FieldTypeWrapper.IsPrimitive || field.FieldTypeWrapper == context.JavaBase.TypeOfJavaLangString))
            {
                var constant = field.GetField().GetRawConstantValue();
                if (field.GetField().FieldType.IsEnum)
                    constant = EnumHelper.GetPrimitiveValue(context, field.GetField().FieldType.GetEnumUnderlyingType(), constant);

                if (constant != null)
                {
                    switch (constant)
                    {
                        case int i:
                            attributes.ConstantValue(i);
                            break;
                        case short s:
                            attributes.ConstantValue(s);
                            break;
                        case char c:
                            attributes.ConstantValue(c);
                            break;
                        case byte b:
                            attributes.ConstantValue((int)(sbyte)b);
                            break;
                        case bool z:
                            attributes.ConstantValue(z);
                            break;
                        case float f:
                            attributes.ConstantValue(f);
                            break;
                        case long j:
                            attributes.ConstantValue(j);
                            break;
                        case double d:
                            attributes.ConstantValue(d);
                            break;
                        case string l:
                            attributes.ConstantValue(l);
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
        }

        /// <summary>
        /// Adds the Signature attribute for a field.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <param name="attributes"></param>
        void AddSignatureAttribute(ClassFileBuilder builder, RuntimeJavaType type, RuntimeJavaField field, AttributeTableBuilder attributes)
        {
            var signature = type.GetGenericFieldSignature(field);
            if (signature != null)
                attributes.Signature(signature);
        }

        /// <summary>
        /// Adds the Deprecated attribute for a field.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <param name="attributes"></param>
        void AddDeprecatedAttribute(ClassFileBuilder builder, RuntimeJavaType type, RuntimeJavaField field, AttributeTableBuilder attributes)
        {
            // .NET ObsoleteAttribute translates to Deprecated attribute
            if (field.GetField() != null && field.GetField().IsDefined(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName), false))
                attributes.Deprecated();
        }

        /// <summary>
        /// Adds the RuntimeVisibleTypeAnnotations attribute for a field.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <param name="attributes"></param>
        void AddRuntimeVisibleTypeAnnotationsAttribute(ClassFileBuilder builder, RuntimeJavaType type, RuntimeJavaField field, AttributeTableBuilder attributes)
        {
            var blob = new BlobBuilder();
            var encoder = new TypeAnnotationTableEncoder(blob);
            if (ImportTypeAnnotations(builder, ref encoder, type, type.GetFieldRawTypeAnnotations(field)))
                attributes.Attribute(AttributeName.RuntimeVisibleTypeAnnotations, blob);
        }

        /// <summary>
        /// Adds the RuntimeVisibleAnnotations attribute for a field.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <param name="attributes"></param>
        void AddRuntimeVisibleAnnotationsAttribute(ClassFileBuilder builder, RuntimeJavaType type, RuntimeJavaField field, AttributeTableBuilder attributes)
        {
            var blob = new BlobBuilder();
            var encoder = new AnnotationTableEncoder(blob);
            if (EncodeAnnotations(builder, ref encoder, field.GetField()))
                attributes.Attribute(AttributeName.RuntimeVisibleAnnotations, blob);
        }

        /// <summary>
        /// Encodes the RuntimeVisibleAnnotations attribute from the specified source.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="encoder"></param>
        /// <param name="source"></param>
        bool EncodeAnnotations(ClassFileBuilder builder, ref AnnotationTableEncoder encoder, IMemberSymbol source)
        {
            var any = false;

#if !FIRST_PASS && !EXPORTER
            if (source != null)
            {
                foreach (var cad in source.GetCustomAttributes())
                {
                    var ann = GetAnnotation(cad);
                    if (ann != null)
                    {
                        EncodeAnnotation(builder, ref encoder, ann);
                        any = true;
                    }
                }
            }
#endif

            return any;
        }

        /// <summary>
        /// Encodes the RuntimeVisibleParameterAnnotations attribute from the specified source.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="encoder"></param>
        /// <param name="source"></param>
        bool EncodeParameterAnnotations(ClassFileBuilder builder, ref ParameterAnnotationTableEncoder encoder, IMethodBaseSymbol source)
        {
            var any = false;

#if !FIRST_PASS && !EXPORTER
            if (source != null)
            {
                var parameters = source.GetParameters();
                if (parameters.Length > 0)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        encoder.ParameterAnnotation(e2 =>
                        {
                            foreach (var cad in parameters[i].GetCustomAttributes())
                            {
                                var ann = GetAnnotation(cad);
                                if (ann != null)
                                {
                                    EncodeAnnotation(builder, ref e2, ann);
                                    any = true;
                                }
                            }
                        });
                    }
                }
            }
#endif

            return any;
        }

        /// <summary>
        /// Encodes the RuntimeVisibleTypeAnnotations attribute from the specified binary source.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="encoder"></param>
        /// <param name="type"></param>
        /// <param name="typeAnnotations"></param>
        /// <exception cref="Exception"></exception>
        bool ImportTypeAnnotations(ClassFileBuilder builder, ref TypeAnnotationTableEncoder encoder, RuntimeJavaType type, byte[] typeAnnotations)
        {
#if !FIRST_PASS && !EXPORTER
            if (typeAnnotations != null)
            {
                var reader = new ClassFormatReader(typeAnnotations);
                if (TypeAnnotationTable.TryRead(ref reader, out var table) == false)
                    throw new Exception();

                table.EncodeTo(new ConstantHandleMap(type.GetConstantPool(), builder.Constants), ref encoder);
                return table.Count > 0;
            }
#endif

            return false;
        }

        /// <summary>
        /// Maps handles from a set of allowed primitive types in a view array to a new pool.
        /// </summary>
        class ConstantHandleMap : IConstantHandleMap
        {

            readonly object[] view;
            readonly IConstantPool pool;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="view"></param>
            /// <param name="pool"></param>
            public ConstantHandleMap(object[] view, IConstantPool pool)
            {
                this.view = view;
                this.pool = pool;
            }

            public Constant Get(ConstantHandle handle)
            {
                return handle.Kind switch
                {
                    ConstantKind.Utf8 => Get((Utf8ConstantHandle)handle),
                    ConstantKind.Integer => Get((IntegerConstantHandle)handle),
                    ConstantKind.Float => Get((FloatConstantHandle)handle),
                    ConstantKind.Long => Get((LongConstantHandle)handle),
                    ConstantKind.Double => Get((DoubleConstantHandle)handle),
                    ConstantKind.Class => Get((ClassConstantHandle)handle),
                    ConstantKind.String => Get((StringConstantHandle)handle),
                    ConstantKind.Fieldref => Get((FieldrefConstantHandle)handle),
                    ConstantKind.Methodref => Get((MethodrefConstantHandle)handle),
                    ConstantKind.InterfaceMethodref => Get((InterfaceMethodrefConstantHandle)handle),
                    ConstantKind.NameAndType => Get((NameAndTypeConstantHandle)handle),
                    ConstantKind.MethodHandle => Get((MethodHandleConstantHandle)handle),
                    ConstantKind.MethodType => Get((MethodTypeConstantHandle)handle),
                    ConstantKind.Dynamic => Get((DynamicConstantHandle)handle),
                    ConstantKind.InvokeDynamic => Get((InvokeDynamicConstantHandle)handle),
                    ConstantKind.Module => Get((ModuleConstantHandle)handle),
                    ConstantKind.Package => Get((PackageConstantHandle)handle),
                    _ => Get((IntegerConstantHandle)handle),
                };
            }

            public RefConstant Get(RefConstantHandle handle)
            {
                throw new NotImplementedException();
            }

            public Utf8Constant Get(Utf8ConstantHandle handle)
            {
                if (view[handle.Slot] is not string s)
                    throw new Exception();

                return new Utf8Constant(s);
            }

            public IntegerConstant Get(IntegerConstantHandle handle)
            {
                if (view[handle.Slot] is not int i)
                    throw new Exception();

                return new IntegerConstant(i);
            }

            public FloatConstant Get(FloatConstantHandle handle)
            {
                if (view[handle.Slot] is not float f)
                    throw new Exception();

                return new FloatConstant(f);
            }

            public LongConstant Get(LongConstantHandle handle)
            {
                if (view[handle.Slot] is not long j)
                    throw new Exception();

                return new LongConstant(j);
            }

            public DoubleConstant Get(DoubleConstantHandle handle)
            {
                if (view[handle.Slot] is not double d)
                    throw new Exception();

                return new DoubleConstant(d);
            }

            public ClassConstant Get(ClassConstantHandle handle)
            {
                throw new NotImplementedException();
            }

            public StringConstant Get(StringConstantHandle handle)
            {
                if (view[handle.Slot] is not string s)
                    throw new Exception();

                return new StringConstant(s);
            }

            public FieldrefConstant Get(FieldrefConstantHandle handle)
            {
                throw new NotImplementedException();
            }

            public MethodrefConstant Get(MethodrefConstantHandle handle)
            {
                throw new NotImplementedException();
            }

            public InterfaceMethodrefConstant Get(InterfaceMethodrefConstantHandle handle)
            {
                throw new NotImplementedException();
            }

            public NameAndTypeConstant Get(NameAndTypeConstantHandle handle)
            {
                throw new NotImplementedException();
            }

            public MethodHandleConstant Get(MethodHandleConstantHandle handle)
            {
                throw new NotImplementedException();
            }

            public MethodTypeConstant Get(MethodTypeConstantHandle handle)
            {
                throw new NotImplementedException();
            }

            public DynamicConstant Get(DynamicConstantHandle handle)
            {
                throw new NotImplementedException();
            }

            public InvokeDynamicConstant Get(InvokeDynamicConstantHandle handle)
            {
                throw new NotImplementedException();
            }

            public ModuleConstant Get(ModuleConstantHandle handle)
            {
                throw new NotImplementedException();
            }

            public PackageConstant Get(PackageConstantHandle handle)
            {
                throw new NotImplementedException();
            }

            public ConstantHandle Map(ConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public Utf8ConstantHandle Map(Utf8ConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public IntegerConstantHandle Map(IntegerConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public FloatConstantHandle Map(FloatConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public LongConstantHandle Map(LongConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public DoubleConstantHandle Map(DoubleConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public ClassConstantHandle Map(ClassConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public StringConstantHandle Map(StringConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public FieldrefConstantHandle Map(FieldrefConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public MethodrefConstantHandle Map(MethodrefConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public InterfaceMethodrefConstantHandle Map(InterfaceMethodrefConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public NameAndTypeConstantHandle Map(NameAndTypeConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public MethodHandleConstantHandle Map(MethodHandleConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public MethodTypeConstantHandle Map(MethodTypeConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public DynamicConstantHandle Map(DynamicConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public InvokeDynamicConstantHandle Map(InvokeDynamicConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public ModuleConstantHandle Map(ModuleConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

            public PackageConstantHandle Map(PackageConstantHandle handle)
            {
                return pool.Import(this, handle);
            }

        }

#if !FIRST_PASS && !EXPORTER

        /// <summary>
        /// Extracts our internal annotation format data from a custom attribute.
        /// </summary>
        /// <param name="cad"></param>
        /// <returns></returns>
        object[] GetAnnotation(CustomAttribute cad)
        {
            // attribute is either a AnnotationAttributeBase or a DynamicAnnotationAttribute with a single object[] in our internal annotation format
            if (cad.ConstructorArguments.Length == 1 && cad.ConstructorArguments[0].ArgumentType == context.Types.Object.MakeArrayType() && (cad.Constructor.DeclaringType.BaseType == context.Resolver.ResolveBaseType(typeof(ikvm.@internal.AnnotationAttributeBase).FullName) || cad.Constructor.DeclaringType == context.Resolver.ResolveRuntimeType(typeof(DynamicAnnotationAttribute).FullName)))
            {
                return UnpackArray((IList<CustomAttributeTypedArgument>)cad.ConstructorArguments[0].Value);
            }

            if (cad.Constructor.DeclaringType.BaseType == context.Resolver.ResolveBaseType(typeof(ikvm.@internal.AnnotationAttributeBase).FullName))
            {
                var annotationType = GetAnnotationInterface(cad);
                if (annotationType != null)
                {
                    // this is a custom attribute annotation applied in a non-Java module
                    var list = new List<object>();
                    list.Add(IKVM.Attributes.AnnotationDefaultAttribute.TAG_ANNOTATION);
                    list.Add("L" + annotationType.Replace('.', '/') + ";");

                    var parameters = cad.Constructor.GetParameters();
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        list.Add(parameters[i].Name);
                        list.Add(GetAnnotationValue(cad.ConstructorArguments[i]));
                    }

                    foreach (var arg in cad.NamedArguments)
                    {
                        list.Add(arg.MemberInfo.Name);
                        list.Add(GetAnnotationValue(arg.TypedValue));
                    }

                    return list.ToArray();
                }
            }

            return null;
        }

        string GetAnnotationInterface(CustomAttribute cad)
        {
            var attr = cad.Constructor.DeclaringType.GetCustomAttribute(context.Resolver.ResolveRuntimeType(typeof(ImplementsAttribute).FullName), false);
            if (attr.HasValue)
            {
                var interfaces = (string[])attr.Value.ConstructorArguments[0].Value;
                if (interfaces.Length == 1)
                    return interfaces[0];
            }

            return null;
        }

        /// <summary>
        /// Extract our internal annotation format from a typed argument.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        object GetAnnotationValue(CustomAttributeTypedArgument arg)
        {
            // argument is directly an enum, so we encode it as a TAG_ENUM
            if (arg.ArgumentType.IsEnum)
            {
                // if GetWrapperFromType returns null, we've got an ikvmc synthesized .NET enum nested inside a Java enum
                var tw = context.ClassLoaderFactory.GetJavaTypeFromType(arg.ArgumentType) ?? context.ClassLoaderFactory.GetJavaTypeFromType(arg.ArgumentType.DeclaringType);
                return new object[] { IKVM.Attributes.AnnotationDefaultAttribute.TAG_ENUM, EncodeTypeName(tw), arg.ArgumentType.GetEnumName(arg.Value) };
            }

            // argument is directly a type, so we encode it as a TAG_CLASS
            if (arg.Value is ITypeSymbol type)
            {
                return new object[] { IKVM.Attributes.AnnotationDefaultAttribute.TAG_CLASS, EncodeTypeName(context.ClassLoaderFactory.GetJavaTypeFromType(type)) };
            }

            // argument is directly an array, so we encode it a a TAG_ARRAY
            if (arg.ArgumentType.IsArray)
            {
                var array = (IList<CustomAttributeTypedArgument>)arg.Value;
                var arr = new object[array.Count + 1];
                arr[0] = IKVM.Attributes.AnnotationDefaultAttribute.TAG_ARRAY;
                for (int i = 0; i < array.Count; i++)
                    arr[i + 1] = GetAnnotationValue(array[i]);

                return arr;

            }

            return arg.Value;
        }

        /// <summary>
        /// Encodes multiple objects in our internal annotation format.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="encoder"></param>
        /// <param name="annotation"></param>
        void EncodeAnnotation(ClassFileBuilder builder, ref AnnotationTableEncoder encoder, object[] annotation)
        {
            encoder.Annotation(e =>
            {
                e.Annotation(builder.Constants.GetOrAddUtf8((string)annotation[1]), e2 =>
                {
                    for (int i = 2; i < annotation.Length; i += 2)
                        e2.Element(builder.Constants.GetOrAddUtf8((string)annotation[i]), e3 => EncodeElementValue(builder, ref e3, annotation[i + 1]));
                });
            });
        }

        void EncodeElementValue(ClassFileBuilder builder, ref ElementValueEncoder encoder, object value)
        {
            if (value is object[] v)
            {
                if (((byte)v[0]) == IKVM.Attributes.AnnotationDefaultAttribute.TAG_ENUM)
                {
                    encoder.Enum(builder.Constants.GetOrAddUtf8(DecodeTypeName((string)v[1])), builder.Constants.GetOrAddUtf8((string)v[2]));
                    return;
                }

                if (((byte)v[0]) == IKVM.Attributes.AnnotationDefaultAttribute.TAG_ARRAY)
                {
                    encoder.Array(e =>
                    {
                        for (int i = 1; i < v.Length; i++)
                            e.ElementValue(e2 => EncodeElementValue(builder, ref e2, v[i]));
                    });

                    return;
                }

                if (((byte)v[0]) == IKVM.Attributes.AnnotationDefaultAttribute.TAG_CLASS)
                {
                    encoder.Class(builder.Constants.GetOrAddUtf8((string)v[1]));
                    return;
                }

                if (((byte)v[0]) == IKVM.Attributes.AnnotationDefaultAttribute.TAG_ANNOTATION)
                {
                    encoder.Annotation(e =>
                    {
                        e.Annotation(builder.Constants.GetOrAddUtf8(DecodeTypeName((string)v[1])), e2 =>
                        {
                            for (int i = 2; i < v.Length; i++)
                                e2.Element(builder.Constants.GetOrAddUtf8((string)v[i]), e3 => EncodeElementValue(builder, ref e3, v[i + 1]));
                        });
                    });

                    return;
                }

                throw new InvalidOperationException();
            }

            if (value is bool z)
            {
                encoder.Boolean(builder.Constants.GetOrAddInteger(z ? 1 : 0));
                return;
            }

            if (value is byte b)
            {
                encoder.Byte(builder.Constants.GetOrAddInteger(unchecked((sbyte)b)));
                return;
            }

            if (value is char c)
            {
                encoder.Char(builder.Constants.GetOrAddInteger(c));
                return;
            }

            if (value is short s)
            {
                encoder.Short(builder.Constants.GetOrAddInteger(s));
                return;
            }

            if (value is int i)
            {
                encoder.Integer(builder.Constants.GetOrAddInteger(i));
                return;
            }

            if (value is long j)
            {
                encoder.Long(builder.Constants.GetOrAddLong(j));
                return;
            }

            if (value is float f)
            {
                encoder.Float(builder.Constants.GetOrAddFloat(f));
                return;
            }

            if (value is double d)
            {
                encoder.Double(builder.Constants.GetOrAddDouble(d));
                return;
            }

            if (value is string S)
            {
                encoder.String(builder.Constants.GetOrAddUtf8(S));
                return;
            }

            throw new InvalidOperationException();
        }

        static string EncodeTypeName(RuntimeJavaType tw)
        {
            return tw.SigName.Replace('.', '/');
        }

        static string DecodeTypeName(string typeName)
        {
#if !FIRST_PASS && !EXPORTER
            int index = typeName.IndexOf(',');
            if (index > 0)
            {
                // HACK if we have an assembly qualified type name we have to resolve it to a Java class name
                // (at the very least we should use the right class loader here)
                try
                {
                    typeName = "L" + java.lang.Class.forName(typeName.Substring(1, typeName.Length - 2).Replace('/', '.')).getName().Replace('.', '/') + ";";
                }
                catch
                {

                }
            }
#endif

            return typeName;
        }

#endif

        object[] UnpackArray(IList<CustomAttributeTypedArgument> list)
        {
            var arr = new object[list.Count];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = list[i].Value is IList<CustomAttributeTypedArgument> l ? UnpackArray(l) : list[i].Value;

            return arr;
        }

        int GetObsoleteCount(IMethodBaseSymbol mb)
        {
            return mb.GetCustomAttributes(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName), false).Length;
        }

        CustomAttribute? GetAnnotationDefault(IMethodBaseSymbol mb)
        {
            return mb.GetCustomAttribute(context.Resolver.ResolveRuntimeType(typeof(Attributes.AnnotationDefaultAttribute).FullName), false);
        }

        string GetAssemblyName(RuntimeJavaType tw)
        {
            var loader = tw.ClassLoader;
            var acl = loader as RuntimeAssemblyClassLoader;
            if (acl != null)
                return acl.GetAssembly(tw).FullName;
            else
                return ((RuntimeGenericClassLoader)loader).GetName();
        }

        bool IsSerializable(RuntimeJavaType tw)
        {
            if (tw.Name == "java.io.Serializable")
                return true;

            while (tw != null)
            {
                foreach (var iface in tw.Interfaces)
                    if (IsSerializable(iface))
                        return true;

                tw = tw.BaseTypeWrapper;
            }

            return false;
        }

        /// <summary>
        /// Encodes a set of fixed annotations for a type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="encoder"></param>
        /// <param name="tw"></param>
        /// <returns></returns>
        bool EncodeAnnotationsForType(ClassFileBuilder builder, ref AnnotationTableEncoder encoder, RuntimeJavaType tw)
        {
            if (tw is RuntimeManagedJavaType.AttributeAnnotationJavaTypeBase attributeAnnotation)
            {
                encoder.Annotation(
                    builder.Constants.GetOrAddUtf8("Ljava/lang/annotation/Retention;"), e => e
                        .Enum(
                            builder.Constants.GetOrAddUtf8("value"),
                            builder.Constants.GetOrAddUtf8("Ljava/lang/annotation/RetentionPolicy;"),
                            builder.Constants.GetOrAddUtf8("RUNTIME")));

                var validOn = attributeAnnotation.AttributeTargets;
                var elementTypes = new List<string>();

                if ((validOn & (AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Assembly)) != 0)
                    elementTypes.Add("TYPE");

                if ((validOn & AttributeTargets.Constructor) != 0)
                    elementTypes.Add("CONSTRUCTOR");

                if ((validOn & AttributeTargets.Field) != 0)
                    elementTypes.Add("FIELD");

                if ((validOn & (AttributeTargets.Method | AttributeTargets.ReturnValue)) != 0)
                    elementTypes.Add("METHOD");

                if ((validOn & AttributeTargets.Parameter) != 0)
                    elementTypes.Add("PARAMETER");

                encoder.Annotation(
                    builder.Constants.GetOrAddUtf8("Ljava/lang/annotation/Target;"), e => e
                        .Element(
                            builder.Constants.GetOrAddUtf8("value"), e2 => e2
                                .Array(e3 =>
                                {
                                    foreach (var elementType in elementTypes)
                                        e3.Enum(builder.Constants.GetOrAddUtf8("Ljava/lang/annotation/ElementType;"), builder.Constants.GetOrAddUtf8(elementType));
                                })));

                if (IsRepeatableAnnotation(tw))
                {
                    encoder.Annotation(
                        builder.Constants.GetOrAddUtf8("Ljava/lang/annotation/Repeatable;"), e => e
                            .Class(
                                builder.Constants.GetOrAddUtf8("value"),
                                builder.Constants.GetOrAddUtf8("L" + (tw.Name + RuntimeManagedJavaType.AttributeAnnotationMultipleSuffix).Replace('.', '/') + ";")));
                }

                return true;
            }

            return false;
        }

        bool IsRepeatableAnnotation(RuntimeJavaType tw)
        {
            foreach (var nested in tw.InnerClasses)
                if (nested.Name == tw.Name + RuntimeManagedJavaType.AttributeAnnotationMultipleSuffix)
                    return true;

            return false;
        }

        /// <summary>
        /// Encodes the annotation default value.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="encoder"></param>
        /// <param name="value"></param>
        void EncodeAnnotationDefault(ClassFileBuilder builder, ref ElementValueEncoder encoder, CustomAttributeTypedArgument value)
        {
            EncodeElementValue(builder, ref encoder, value);
        }

        /// <summary>
        /// Encodes the element_value on the annotation default.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="encoder"></param>
        /// <param name="type"></param>
        /// <exception cref="InvalidOperationException"></exception>
        void EncodeAnnotationDefault(ClassFileBuilder builder, ref ElementValueEncoder encoder, RuntimeJavaType type)
        {
            if (type == context.PrimitiveJavaTypeFactory.BOOLEAN)
            {
                encoder.Boolean(builder.Constants.GetOrAddInteger(0));
            }
            else if (type == context.PrimitiveJavaTypeFactory.BYTE)
            {
                encoder.Byte(builder.Constants.GetOrAddInteger(0));
            }
            else if (type == context.PrimitiveJavaTypeFactory.CHAR)
            {
                encoder.Char(builder.Constants.GetOrAddInteger(0));
            }
            else if (type == context.PrimitiveJavaTypeFactory.SHORT)
            {
                encoder.Short(builder.Constants.GetOrAddInteger(0));
            }
            else if (type == context.PrimitiveJavaTypeFactory.INT)
            {
                encoder.Integer(builder.Constants.GetOrAddInteger(0));
            }
            else if (type == context.PrimitiveJavaTypeFactory.FLOAT)
            {
                encoder.Float(builder.Constants.GetOrAddFloat(0));
            }
            else if (type == context.PrimitiveJavaTypeFactory.LONG)
            {
                encoder.Long(builder.Constants.GetOrAddLong(0));
            }
            else if (type == context.PrimitiveJavaTypeFactory.DOUBLE)
            {
                encoder.Double(builder.Constants.GetOrAddDouble(0));
            }
            else if (type == context.JavaBase.TypeOfJavaLangString)
            {
                encoder.String(builder.Constants.GetOrAddUtf8(""));
            }
            else if ((type.Modifiers & Modifiers.Enum) != 0)
            {
                encoder.Enum(builder.Constants.GetOrAddUtf8("L" + type.Name.Replace('.', '/') + ";"), builder.Constants.GetOrAddUtf8("__unspecified"));
            }
            else if (type == context.JavaBase.TypeOfJavaLangClass)
            {
                encoder.Class(builder.Constants.GetOrAddUtf8("Likvm/internal/__unspecified;"));
            }
            else if (type.IsArray)
            {
                encoder.Array(e => { });
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Encodes a single element value into an element value table from a <see cref="CustomAttributeTypedArgument"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="encoder"></param>
        /// <param name="value"></param>
        void EncodeElementValue(ClassFileBuilder builder, ref ElementValueTableEncoder encoder, CustomAttributeTypedArgument value)
        {
            encoder.ElementValue(e => EncodeElementValue(builder, ref e, value));
        }

        /// <summary>
        /// Encodes a single element value into an elmeent value table from a <see cref="CustomAttributeTypedArgument"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="encoder"></param>
        /// <param name="value"></param>
        void EncodeElementValue(ClassFileBuilder builder, ref ElementValueEncoder encoder, CustomAttributeTypedArgument value)
        {
            // typed argument of bool type holds BOOLEAN
            if (value.ArgumentType == context.Types.Boolean)
            {
                encoder.Boolean(builder.Constants.GetOrAddInteger((bool)value.Value ? 1 : 0));
                return;
            }

            // typed argument of byte type holds BYTE
            if (value.ArgumentType == context.Types.Byte)
            {
                encoder.Byte(builder.Constants.GetOrAddInteger(unchecked((sbyte)(byte)value.Value)));
                return;
            }

            // typed argument of char type holds CHAR
            if (value.ArgumentType == context.Types.Char)
            {
                encoder.Char(builder.Constants.GetOrAddInteger((char)value.Value));
                return;
            }

            // typed argument of short type holds SHORT
            if (value.ArgumentType == context.Types.Int16)
            {
                encoder.Short(builder.Constants.GetOrAddInteger((short)value.Value));
                return;
            }

            // typed argument of int type holds INTEGER
            if (value.ArgumentType == context.Types.Int32)
            {
                encoder.Integer(builder.Constants.GetOrAddInteger((int)value.Value));
                return;
            }

            // typed argument of float type holds SINGLE
            if (value.ArgumentType == context.Types.Single)
            {
                encoder.Float(builder.Constants.GetOrAddFloat((float)value.Value));
                return;
            }

            // typed argument of long type holds LONG
            if (value.ArgumentType == context.Types.Int64)
            {
                encoder.Long(builder.Constants.GetOrAddLong((long)value.Value));
                return;
            }

            // typed argument double type holds DOUBLE
            if (value.ArgumentType == context.Types.Double)
            {
                encoder.Double(builder.Constants.GetOrAddDouble((double)value.Value));
                return;
            }

            // typed argument of string type holds STRING
            if (value.ArgumentType == context.Types.String)
            {
                encoder.String(builder.Constants.GetOrAddUtf8((string)value.Value));
                return;
            }

            // typed argument of array type holds ARRAY, CLASS, ENUM and ANNOTATION
            if (value.ArgumentType == context.Types.Object.MakeArrayType())
            {
                // first argument holds type in tag format
                var data = (IReadOnlyList<CustomAttributeTypedArgument>)value.Value;
                var type = (byte)data[0].Value;

                // tag of ARRAY indicates ARRAY
                if (type == IKVM.Attributes.AnnotationDefaultAttribute.TAG_ARRAY)
                {
                    encoder.Array(e =>
                    {
                        for (int i = 1; i < data.Count; i++)
                            EncodeElementValue(builder, ref e, data[i]);
                    });

                    return;
                }

                // tag of CLASS indicates CLASS
                if (type == IKVM.Attributes.AnnotationDefaultAttribute.TAG_CLASS)
                {
                    encoder.Class(builder.Constants.GetOrAddUtf8((string)data[1].Value));
                    return;
                }

                // tag of ENUM indicates ENUM
                if (type == IKVM.Attributes.AnnotationDefaultAttribute.TAG_ENUM)
                {
                    encoder.Enum(builder.Constants.GetOrAddUtf8((string)data[1].Value), builder.Constants.GetOrAddUtf8((string)data[2].Value));
                    return;
                }

                // tag of ANNOTATION indicates ANNOTATION
                if (type == IKVM.Attributes.AnnotationDefaultAttribute.TAG_ANNOTATION)
                {
                    encoder.Annotation(e =>
                    {
                        e.Annotation(builder.Constants.GetOrAddUtf8((string)data[1].Value), e2 =>
                        {
                            for (int i = 2; i < data.Count; i += 2)
                                e2.Element(builder.Constants.GetOrAddUtf8((string)data[i].Value), e3 => EncodeElementValue(builder, ref e3, data[i + 1]));
                        });
                    });

                    return;
                }

                Warning("Warning: incorrect annotation default element tag: " + type);
                return;
            }

            Warning("Warning: incorrect annotation default element type: " + value.ArgumentType);
            return;
        }

        /// <summary>
        /// Emits a warning message.
        /// </summary>
        /// <param name="message"></param>
        void Warning(string message)
        {
#if EXPORTER
            Console.Error.WriteLine(message);
#endif
        }

    }

}
