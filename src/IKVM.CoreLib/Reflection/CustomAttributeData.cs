/*
  Copyright (C) 2009-2012 Jeroen Frijters

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
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;

using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    /// <summary>
    /// Provides access to custom attribute data for assemblies, modules, types, members and parameters that are loaded into the
    /// reflection-only context.
    /// </summary>
    sealed class CustomAttributeData
    {

        /// <summary>
        /// An empty, read-only list of custom attribute data.
        /// </summary>
        internal static readonly IList<CustomAttributeData> EmptyList = new List<CustomAttributeData>(0).AsReadOnly();

        // there are several states a CustomAttributeData object can be in:
        //
        // 1) Unresolved custom attribute
        //    - _customAttribute is not nil
        //    - _lazyConstructor == null
        //    - _lazyConstructorArguments == null
        //    - _lazyNamedArguments == null
        //
        // 2) Resolved custom attribute
        //    - _customAttribute is not nil
        //    - _lazyConstructor != null
        //    - _lazyConstructorArguments != null
        //    - _lazyNamedArguments != null
        //
        // 3) Pre-resolved custom attribute, pseudo custom attribute or result of CustomAttributeBuilder.ToData()
        //    - _customAttribute is nil
        //    - _lazyConstructor != null
        //    - _lazyConstructorArguments != null
        //    - _lazyNamedArguments != null

        readonly Module _module;
        readonly CustomAttributeHandle _customAttribute;

        ConstructorInfo _lazyConstructor;
        IList<CustomAttributeTypedArgument> _lazyConstructorArguments;
        IList<CustomAttributeNamedArgument> _lazyNamedArguments;

        /// <summary>
        /// Initializes a new instance for a custom attribute row of a module that was read from a file. The constructor
        /// and arguments are resolved on first use.
        /// </summary>
        /// <param name="module">The module that contains the custom attribute.</param>
        /// <param name="handle">The handle of the custom attribute row.</param>
        /// <exception cref="ArgumentNullException"><paramref name="module"/> is <see langword="null"/>.</exception>
        internal CustomAttributeData(ModuleReader module, CustomAttributeHandle handle)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _customAttribute = handle;
        }

        /// <summary>
        /// Initializes a new instance for a pseudo custom attribute, typing each constructor argument by the
        /// corresponding constructor parameter type.
        /// </summary>
        /// <param name="module">The module the attribute applies in.</param>
        /// <param name="constructor">The attribute constructor.</param>
        /// <param name="args">The constructor argument values.</param>
        /// <param name="namedArguments">The named arguments, or <see langword="null"/> if there are none.</param>
        internal CustomAttributeData(Module module, ConstructorInfo constructor, object[] args, List<CustomAttributeNamedArgument> namedArguments) :
            this(module, constructor, WrapConstructorArgs(args, constructor.MethodSignature), namedArguments)
        {
        }

        /// <summary>
        /// Wraps constructor argument values as typed arguments, using the parameter types of <paramref name="sig"/>.
        /// </summary>
        /// <param name="args">The constructor argument values.</param>
        /// <param name="sig">The signature of the constructor.</param>
        /// <returns>The typed constructor arguments.</returns>
        static List<CustomAttributeTypedArgument> WrapConstructorArgs(object[] args, MethodSignature sig)
        {
            var list = new List<CustomAttributeTypedArgument>();
            for (var i = 0; i < args.Length; i++)
                list.Add(new CustomAttributeTypedArgument(sig.GetParameterType(i), args[i]));

            return list;
        }

        /// <summary>
        /// Initializes a new instance for a pseudo custom attribute, .NET 1.x declarative security or the result of
        /// <see cref="CustomAttributeBuilder.ToData"/> (state 4).
        /// </summary>
        /// <param name="module">The module the attribute applies in.</param>
        /// <param name="constructor">The attribute constructor.</param>
        /// <param name="constructorArgs">The typed constructor arguments.</param>
        /// <param name="namedArguments">The named arguments, or <see langword="null"/> if there are none.</param>
        /// <exception cref="ArgumentNullException"><paramref name="module"/> is <see langword="null"/>.</exception>
        internal CustomAttributeData(Module module, ConstructorInfo constructor, List<CustomAttributeTypedArgument> constructorArgs, List<CustomAttributeNamedArgument> namedArguments)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _lazyConstructor = constructor;

            _lazyConstructorArguments = constructorArgs.AsReadOnly();
            if (namedArguments == null)
                _lazyNamedArguments = Array.Empty<CustomAttributeNamedArgument>();
            else
                _lazyNamedArguments = namedArguments.AsReadOnly();
        }

        /// <summary>
        /// Initializes a new instance for a pre-resolved custom attribute, decoding the arguments from a custom attribute
        /// blob immediately (state 3).
        /// </summary>
        /// <param name="asm">The assembly whose manifest module the attribute applies in.</param>
        /// <param name="constructor">The attribute constructor.</param>
        /// <param name="br">A reader over the custom attribute blob.</param>
        /// <exception cref="BadImageFormatException">The blob does not start with the custom attribute prolog.</exception>
        internal CustomAttributeData(Assembly asm, ConstructorInfo constructor, BlobReader br)
        {
            _module = asm.ManifestModule;
            _lazyConstructor = constructor;
            if (br.RemainingBytes == 0)
            {
                // it's legal to have an empty blob
                _lazyConstructorArguments = Array.Empty<CustomAttributeTypedArgument>();
                _lazyNamedArguments = Array.Empty<CustomAttributeNamedArgument>();
            }
            else
            {
                if (br.ReadUInt16() != 1)
                    throw new BadImageFormatException();

                _lazyConstructorArguments = ReadConstructorArguments(_module, ref br, constructor);
                _lazyNamedArguments = ReadNamedArguments(_module, ref br, br.ReadUInt16(), constructor.DeclaringType, true);
            }
        }

        /// <summary>
        /// Returns a string representation of the custom attribute.
        /// </summary>
        /// <returns>A string value that represents the custom attribute, in C# like syntax.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('[');
            sb.Append(Constructor.DeclaringType.FullName);
            sb.Append('(');

            var sep = "";
            var parameters = Constructor.GetParameters();
            var args = ConstructorArguments;

            for (var i = 0; i < parameters.Length; i++)
            {
                sb.Append(sep);
                sep = ", ";
                AppendValue(sb, parameters[i].ParameterType, args[i]);
            }

            foreach (var named in NamedArguments)
            {
                sb.Append(sep);
                sep = ", ";
                sb.Append(named.MemberInfo.Name);
                sb.Append(" = ");
                var fi = named.MemberInfo as FieldInfo;
                var type = fi != null ? fi.FieldType : ((PropertyInfo)named.MemberInfo).PropertyType;
                AppendValue(sb, type, named.TypedValue);
            }
            sb.Append(')');
            sb.Append(']');

            return sb.ToString();
        }

        /// <summary>
        /// Appends the representation of an attribute argument to <paramref name="sb"/>, with a cast when its type differs from
        /// the declared type.
        /// </summary>
        /// <param name="sb">The string builder to append to.</param>
        /// <param name="type">The declared type of the parameter, field or property.</param>
        /// <param name="arg">The argument.</param>
        static void AppendValue(StringBuilder sb, Type type, CustomAttributeTypedArgument arg)
        {
            if (arg.ArgumentType == arg.ArgumentType.Module.Universe.System_String)
            {
                sb.Append('"').Append(arg.Value).Append('"');
            }
            else if (arg.ArgumentType.IsArray)
            {
                var elementType = arg.ArgumentType.GetElementType();
                string elementTypeName;
                if (elementType.IsPrimitive
                    || elementType == type.Module.Universe.System_Object
                    || elementType == type.Module.Universe.System_String
                    || elementType == type.Module.Universe.System_Type)
                {
                    elementTypeName = elementType.Name;
                }
                else
                {
                    elementTypeName = elementType.FullName;
                }
                sb.Append("new ").Append(elementTypeName).Append("[").Append(((Array)arg.Value).Length).Append("] { ");
                var sep = "";
                foreach (var elem in (CustomAttributeTypedArgument[])arg.Value)
                {
                    sb.Append(sep);
                    sep = ", ";
                    AppendValue(sb, elementType, elem);
                }
                sb.Append(" }");
            }
            else
            {
                if (arg.ArgumentType != type || (type.IsEnum && !arg.Value.Equals(0)))
                {
                    sb.Append('(');
                    sb.Append(arg.ArgumentType.FullName);
                    sb.Append(')');
                }

                sb.Append(arg.Value);
            }
        }

        /// <summary>
        /// Reads a FieldOrPropType from a custom attribute blob.
        /// </summary>
        /// <param name="context">The module the blob belongs to.</param>
        /// <param name="br">The reader positioned at the type.</param>
        /// <returns>The type.</returns>
        /// <exception cref="BadImageFormatException">The blob contains an invalid type encoding.</exception>
        static Type ReadFieldOrPropType(Module context, ref BlobReader br)
        {
            return br.ReadByte() switch
            {
                Signature.ELEMENT_TYPE_BOOLEAN => context.Universe.System_Boolean,
                Signature.ELEMENT_TYPE_CHAR => context.Universe.System_Char,
                Signature.ELEMENT_TYPE_I1 => context.Universe.System_SByte,
                Signature.ELEMENT_TYPE_U1 => context.Universe.System_Byte,
                Signature.ELEMENT_TYPE_I2 => context.Universe.System_Int16,
                Signature.ELEMENT_TYPE_U2 => context.Universe.System_UInt16,
                Signature.ELEMENT_TYPE_I4 => context.Universe.System_Int32,
                Signature.ELEMENT_TYPE_U4 => context.Universe.System_UInt32,
                Signature.ELEMENT_TYPE_I8 => context.Universe.System_Int64,
                Signature.ELEMENT_TYPE_U8 => context.Universe.System_UInt64,
                Signature.ELEMENT_TYPE_R4 => context.Universe.System_Single,
                Signature.ELEMENT_TYPE_R8 => context.Universe.System_Double,
                Signature.ELEMENT_TYPE_STRING => context.Universe.System_String,
                Signature.ELEMENT_TYPE_SZARRAY => ReadFieldOrPropType(context, ref br).MakeArrayType(),
                0x55 => ReadType(context, ref br), // enum
                0x50 => context.Universe.System_Type, // System.Type
                0x51 => context.Universe.System_Object, // boxed object
                _ => throw new BadImageFormatException(),
            };
        }

        /// <summary>
        /// Reads a fixed argument of the specified type from a custom attribute blob.
        /// </summary>
        /// <param name="context">The module the blob belongs to.</param>
        /// <param name="br">The reader positioned at the argument.</param>
        /// <param name="type">The type of the argument.</param>
        /// <returns>The argument. Array values are returned as an array of <see cref="CustomAttributeTypedArgument"/>.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="type"/> is not a valid custom attribute argument type.</exception>
        static CustomAttributeTypedArgument ReadFixedArg(Module context, ref BlobReader br, Type type)
        {
            var u = context.Universe;
            if (type == u.System_String)
            {
                return new CustomAttributeTypedArgument(type, br.ReadSerializedString());
            }
            else if (type == u.System_Boolean)
            {
                return new CustomAttributeTypedArgument(type, br.ReadByte() != 0);
            }
            else if (type == u.System_Char)
            {
                return new CustomAttributeTypedArgument(type, br.ReadChar());
            }
            else if (type == u.System_Single)
            {
                return new CustomAttributeTypedArgument(type, br.ReadSingle());
            }
            else if (type == u.System_Double)
            {
                return new CustomAttributeTypedArgument(type, br.ReadDouble());
            }
            else if (type == u.System_SByte)
            {
                return new CustomAttributeTypedArgument(type, br.ReadSByte());
            }
            else if (type == u.System_Int16)
            {
                return new CustomAttributeTypedArgument(type, br.ReadInt16());
            }
            else if (type == u.System_Int32)
            {
                return new CustomAttributeTypedArgument(type, br.ReadInt32());
            }
            else if (type == u.System_Int64)
            {
                return new CustomAttributeTypedArgument(type, br.ReadInt64());
            }
            else if (type == u.System_Byte)
            {
                return new CustomAttributeTypedArgument(type, br.ReadByte());
            }
            else if (type == u.System_UInt16)
            {
                return new CustomAttributeTypedArgument(type, br.ReadUInt16());
            }
            else if (type == u.System_UInt32)
            {
                return new CustomAttributeTypedArgument(type, br.ReadUInt32());
            }
            else if (type == u.System_UInt64)
            {
                return new CustomAttributeTypedArgument(type, br.ReadUInt64());
            }
            else if (type == u.System_Type)
            {
                return new CustomAttributeTypedArgument(type, ReadType(context, ref br));
            }
            else if (type == u.System_Object)
            {
                return ReadFixedArg(context, ref br, ReadFieldOrPropType(context, ref br));
            }
            else if (type.IsArray)
            {
                var length = br.ReadInt32();
                if (length == -1)
                    return new CustomAttributeTypedArgument(type, null);

                var elementType = type.GetElementType();
                var array = new CustomAttributeTypedArgument[length];
                for (var i = 0; i < length; i++)
                    array[i] = ReadFixedArg(context, ref br, elementType);

                return new CustomAttributeTypedArgument(type, array);
            }
            else if (type.IsEnum)
            {
                return new CustomAttributeTypedArgument(type, ReadFixedArg(context, ref br, type.GetEnumUnderlyingTypeImpl()).Value);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Reads a serialized type name from a custom attribute blob and resolves it.
        /// </summary>
        /// <param name="context">The module the blob belongs to.</param>
        /// <param name="br">The reader positioned at the type name.</param>
        /// <returns>The type, or <see langword="null"/> if the serialized string is null.</returns>
        static Type ReadType(Module context, ref BlobReader br)
        {
            var typeName = br.ReadSerializedString();
            if (typeName == null)
                return null;

            // there are broken compilers that emit an extra NUL character after the type name
            if (typeName.Length > 0 && typeName[^1] == 0)
                typeName = typeName[..^1];

            return TypeNameParser.Parse(typeName, true).GetType(context.Universe, context, true, typeName, true, false);
        }

        /// <summary>
        /// Reads the fixed arguments for the parameters of <paramref name="constructor"/> from a custom attribute blob.
        /// </summary>
        /// <param name="context">The module the blob belongs to.</param>
        /// <param name="br">The reader positioned after the prolog.</param>
        /// <param name="constructor">The attribute constructor.</param>
        /// <returns>A read-only list of the constructor arguments.</returns>
        static IList<CustomAttributeTypedArgument> ReadConstructorArguments(Module context, ref BlobReader br, ConstructorInfo constructor)
        {
            var sig = constructor.MethodSignature;
            var count = sig.GetParameterCount();
            var list = new List<CustomAttributeTypedArgument>(count);
            for (var i = 0; i < count; i++)
                list.Add(ReadFixedArg(context, ref br, sig.GetParameterType(i)));

            return list.AsReadOnly();
        }

        /// <summary>
        /// Reads the named arguments from a custom attribute or declarative security blob.
        /// </summary>
        /// <param name="context">The module the blob belongs to.</param>
        /// <param name="br">The reader positioned at the first named argument.</param>
        /// <param name="named">The number of named arguments.</param>
        /// <param name="type">The attribute type that declares the fields and properties.</param>
        /// <param name="required"><see langword="false"/> to give up and return <see langword="null"/> when the type of a named argument is missing.</param>
        /// <returns>A read-only list of the named arguments, or <see langword="null"/> if <paramref name="required"/> is <see langword="false"/> and a type is missing.</returns>
        /// <exception cref="BadImageFormatException">A named argument is neither a field nor a property.</exception>
        static IList<CustomAttributeNamedArgument> ReadNamedArguments(Module context, ref BlobReader br, int named, Type type, bool required)
        {
            var list = new List<CustomAttributeNamedArgument>(named);
            for (var i = 0; i < named; i++)
            {
                var fieldOrProperty = br.ReadByte();
                var fieldOrPropertyType = ReadFieldOrPropType(context, ref br);
                if (fieldOrPropertyType.__IsMissing && !required)
                    return null;

                var name = br.ReadSerializedString();
                var value = ReadFixedArg(context, ref br, fieldOrPropertyType);
                var member = fieldOrProperty switch
                {
                    0x53 => (MemberInfo)GetField(context, type, name, fieldOrPropertyType), // FIELD
                    0x54 => (MemberInfo)GetProperty(context, type, name, fieldOrPropertyType), // PROPERTY
                    _ => throw new BadImageFormatException(),
                };

                list.Add(new CustomAttributeNamedArgument(member, value));
            }

            return list.AsReadOnly();
        }

        /// <summary>
        /// Finds the public instance field that a named argument sets, searching <paramref name="type"/> and its base types.
        /// </summary>
        /// <param name="context">The module the blob belongs to.</param>
        /// <param name="type">The attribute type.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="fieldType">The type of the field.</param>
        /// <returns>The field, or a missing field placed on the first missing base type (or <paramref name="type"/>) if it is not found.</returns>
        static FieldInfo GetField(Module context, Type type, string name, Type fieldType)
        {
            var org = type;
            for (; type != null && !type.__IsMissing; type = type.BaseType)
                foreach (FieldInfo field in type.__GetDeclaredFields())
                    if (field.IsPublic && !field.IsStatic && field.Name == name)
                        return field;

            // if the field is missing, we stick the missing field on the first missing base type
            if (type == null)
                type = org;

            var sig = FieldSignature.Create(fieldType, new CustomModifiers());
            return type.FindField(name, sig) ?? type.Module.Universe.GetMissingFieldOrThrow(context, type, name, sig);
        }

        /// <summary>
        /// Finds the public instance property that a named argument sets, searching <paramref name="type"/> and its base types.
        /// </summary>
        /// <param name="context">The module the blob belongs to.</param>
        /// <param name="type">The attribute type.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <returns>The property, or a missing property placed on the first missing base type (or <paramref name="type"/>) if it is not found.</returns>
        static PropertyInfo GetProperty(Module context, Type type, string name, Type propertyType)
        {
            var org = type;
            for (; type != null && !type.__IsMissing; type = type.BaseType)
                foreach (PropertyInfo property in type.__GetDeclaredProperties())
                    if (property.IsPublic && !property.IsStatic && property.Name == name)
                        return property;

            // if the property is missing, we stick the missing property on the first missing base type
            if (type == null)
                type = org;

            return type.Module.Universe.GetMissingPropertyOrThrow(context, type, name, PropertySignature.Create(CallingConventions.Standard | CallingConventions.HasThis, propertyType, null, new PackedCustomModifiers()));
        }

        /// <summary>
        /// Gets the type of the attribute.
        /// </summary>
        public Type AttributeType => Constructor.DeclaringType;

        /// <summary>
        /// Gets a <see cref="ConstructorInfo"/> object that represents the constructor that would have initialized the custom attribute.
        /// </summary>
        public ConstructorInfo Constructor
        {
            get
            {
                if (_lazyConstructor == null)
                    _lazyConstructor = (ConstructorInfo)_module.ResolveMethod(MetadataTokens.GetToken(((ModuleReader)_module).Metadata.GetCustomAttribute(_customAttribute).Constructor));

                return _lazyConstructor;
            }
        }

        /// <summary>
        /// Gets the list of positional arguments specified for the attribute instance represented by the <see cref="CustomAttributeData"/> object.
        /// </summary>
        public IList<CustomAttributeTypedArgument> ConstructorArguments
        {
            get
            {
                if (_lazyConstructorArguments == null)
                    LazyParseArguments(false);

                return _lazyConstructorArguments;
            }
        }

        /// <summary>
        /// Gets the list of named arguments specified for the attribute instance represented by the <see cref="CustomAttributeData"/> object.
        /// </summary>
        public IList<CustomAttributeNamedArgument> NamedArguments
        {
            get
            {
                // only an unresolved custom attribute has not decoded its named arguments
                if (_lazyNamedArguments == null)
                    LazyParseArguments(true);

                return _lazyNamedArguments;
            }
        }

        /// <summary>
        /// Decodes the constructor and named arguments from the blob of the custom attribute row.
        /// </summary>
        /// <param name="requireNameArguments"><see langword="false"/> to leave the named arguments unresolved when the type of one of them is missing.</param>
        /// <exception cref="BadImageFormatException">The blob does not start with the custom attribute prolog.</exception>
        void LazyParseArguments(bool requireNameArguments)
        {
            var br = _module.GetBlobReader(((ModuleReader)_module).Metadata.GetCustomAttribute(_customAttribute).Value);
            if (br.RemainingBytes == 0)
            {
                // it's legal to have an empty blob
                _lazyConstructorArguments = Array.Empty<CustomAttributeTypedArgument>();
                _lazyNamedArguments = Array.Empty<CustomAttributeNamedArgument>();
            }
            else
            {
                if (br.ReadUInt16() != 1)
                    throw new BadImageFormatException();

                _lazyConstructorArguments = ReadConstructorArguments(_module, ref br, Constructor);
                _lazyNamedArguments = ReadNamedArguments(_module, ref br, br.ReadUInt16(), Constructor.DeclaringType, requireNameArguments);
            }
        }

        /// <summary>
        /// Creates a <see cref="CustomAttributeBuilder"/> that recreates this custom attribute.
        /// </summary>
        /// <returns>The custom attribute builder.</returns>
        public CustomAttributeBuilder __ToBuilder()
        {
            var parameters = Constructor.GetParameters();
            var args = new object[ConstructorArguments.Count];
            for (var i = 0; i < args.Length; i++)
                args[i] = RewrapArray(parameters[i].ParameterType, ConstructorArguments[i]);

            var namedProperties = new List<PropertyInfo>();
            var propertyValues = new List<object>();
            var namedFields = new List<FieldInfo>();
            var fieldValues = new List<object>();

            foreach (var named in NamedArguments)
            {
                var pi = named.MemberInfo as PropertyInfo;
                if (pi != null)
                {
                    namedProperties.Add(pi);
                    propertyValues.Add(RewrapArray(pi.PropertyType, named.TypedValue));
                }
                else
                {
                    var fi = (FieldInfo)named.MemberInfo;
                    namedFields.Add(fi);
                    fieldValues.Add(RewrapArray(fi.FieldType, named.TypedValue));
                }
            }

            return new CustomAttributeBuilder(Constructor, args, namedProperties.ToArray(), propertyValues.ToArray(), namedFields.ToArray(), fieldValues.ToArray());
        }

        /// <summary>
        /// Converts an argument value back to the form <see cref="CustomAttributeBuilder"/> accepts, turning arrays of
        /// <see cref="CustomAttributeTypedArgument"/> into arrays of values.
        /// </summary>
        /// <param name="type">The declared type of the parameter, field or property.</param>
        /// <param name="arg">The argument.</param>
        /// <returns>The argument value; an array value declared as <see cref="object"/> is wrapped with <see cref="CustomAttributeBuilder.__MakeTypedArgument"/> to keep its type.</returns>
        static object RewrapArray(Type type, CustomAttributeTypedArgument arg)
        {
            if (arg.Value is IList<CustomAttributeTypedArgument> list)
            {
                var elementType = arg.ArgumentType.GetElementType();
                var arr = new object[list.Count];
                for (var i = 0; i < arr.Length; i++)
                    arr[i] = RewrapArray(elementType, list[i]);

                if (type == type.Module.Universe.System_Object)
                    return CustomAttributeBuilder.__MakeTypedArgument(arg.ArgumentType, arr);

                return arr;
            }
            else
            {
                return arg.Value;
            }
        }

        /// <summary>
        /// Returns a list of <see cref="CustomAttributeData"/> objects representing data about the attributes that have been applied to the target member.
        /// </summary>
        /// <param name="member">The member whose attribute data is to be retrieved.</param>
        /// <returns>A list of objects that represent data about the attributes that have been applied to the target member.</returns>
        public static IList<CustomAttributeData> GetCustomAttributes(MemberInfo member) => __GetCustomAttributes(member, null, false);

        /// <summary>
        /// Returns a list of <see cref="CustomAttributeData"/> objects representing data about the attributes that have been applied to the target assembly.
        /// </summary>
        /// <param name="assembly">The assembly whose attribute data is to be retrieved.</param>
        /// <returns>A list of objects that represent data about the attributes that have been applied to the target assembly.</returns>
        public static IList<CustomAttributeData> GetCustomAttributes(Assembly assembly) => assembly.GetCustomAttributesData(null);

        /// <summary>
        /// Returns a list of <see cref="CustomAttributeData"/> objects representing data about the attributes that have been applied to the target module.
        /// </summary>
        /// <param name="module">The module whose attribute data is to be retrieved.</param>
        /// <returns>A list of objects that represent data about the attributes that have been applied to the target module.</returns>
        public static IList<CustomAttributeData> GetCustomAttributes(Module module) => __GetCustomAttributes(module, null, false);

        /// <summary>
        /// Returns a list of <see cref="CustomAttributeData"/> objects representing data about the attributes that have been applied to the target parameter.
        /// </summary>
        /// <param name="parameter">The parameter whose attribute data is to be retrieved.</param>
        /// <returns>A list of objects that represent data about the attributes that have been applied to the target parameter.</returns>
        public static IList<CustomAttributeData> GetCustomAttributes(ParameterInfo parameter) => __GetCustomAttributes(parameter, null, false);

        /// <summary>
        /// Returns the custom attributes of the specified type that have been applied to the target assembly.
        /// </summary>
        /// <param name="assembly">The assembly whose attribute data is to be retrieved.</param>
        /// <param name="attributeType">The type of attribute to return, or <see langword="null"/> for all attributes.</param>
        /// <param name="inherit">Ignored for assemblies.</param>
        /// <returns>A list of objects that represent data about the matching attributes.</returns>
        public static IList<CustomAttributeData> __GetCustomAttributes(Assembly assembly, Type attributeType, bool inherit) =>
            assembly.GetCustomAttributesData(attributeType);

        /// <summary>
        /// Returns the custom attributes of the specified type that have been applied to the target module.
        /// </summary>
        /// <param name="module">The module whose attribute data is to be retrieved.</param>
        /// <param name="attributeType">The type of attribute to return, or <see langword="null"/> for all attributes.</param>
        /// <param name="inherit">Ignored for modules.</param>
        /// <returns>A list of objects that represent data about the matching attributes.</returns>
        /// <exception cref="MissingModuleException"><paramref name="module"/> is missing.</exception>
        public static IList<CustomAttributeData> __GetCustomAttributes(Module module, Type attributeType, bool inherit)
        {
            if (module.__IsMissing)
                throw new MissingModuleException((MissingModule)module);

            return GetCustomAttributesImpl(null, module, 0x00000001, attributeType) ?? EmptyList;
        }

        /// <summary>
        /// Returns the custom attributes of the specified type that have been applied to the target parameter, including a
        /// <see cref="System.Runtime.InteropServices.MarshalAsAttribute"/> pseudo custom attribute for marshalling information.
        /// </summary>
        /// <param name="parameter">The parameter whose attribute data is to be retrieved.</param>
        /// <param name="attributeType">The type of attribute to return, or <see langword="null"/> for all attributes.</param>
        /// <param name="inherit">Ignored for parameters.</param>
        /// <returns>A list of objects that represent data about the matching attributes.</returns>
        public static IList<CustomAttributeData> __GetCustomAttributes(ParameterInfo parameter, Type attributeType, bool inherit)
        {
            var module = parameter.Module;
            List<CustomAttributeData> list = null;
            if (attributeType == null || attributeType.IsAssignableFrom(module.Universe.System_Runtime_InteropServices_MarshalAsAttribute))
                if (parameter.__TryGetFieldMarshal(out var spec) && CreateMarshalAsPseudoCustomAttribute(module, spec) is { } pseudo)
                    (list ??= []).Add(pseudo);

            var token = parameter.MetadataToken;
            if (module is ModuleBuilder mb && mb.IsSaved && ModuleBuilder.IsPseudoToken(token))
                token = mb.ResolvePseudoToken(token);

            return GetCustomAttributesImpl(list, module, token, attributeType) ?? EmptyList;
        }

        /// <summary>
        /// Returns the custom attributes of the specified type that have been applied to the target member, including pseudo
        /// custom attributes and, when <paramref name="inherit"/> is <see langword="true"/> and the attribute is inheritable,
        /// the attributes of base types or base method definitions.
        /// </summary>
        /// <param name="member">The member whose attribute data is to be retrieved.</param>
        /// <param name="attributeType">The type of attribute to return, or <see langword="null"/> for all attributes.</param>
        /// <param name="inherit"><see langword="true"/> to also search the inheritance chain of types and methods.</param>
        /// <returns>A list of objects that represent data about the matching attributes.</returns>
        /// <exception cref="NotImplementedException"><paramref name="member"/> has not been baked.</exception>
        public static IList<CustomAttributeData> __GetCustomAttributes(MemberInfo member, Type attributeType, bool inherit)
        {
            // like .NET, we don't return custom attributes for unbaked members
            if (!member.IsBaked)
                throw new NotImplementedException();

            if (!inherit || !IsInheritableAttribute(attributeType))
                return GetCustomAttributesImpl(null, member, attributeType) ?? EmptyList;

            var list = new List<CustomAttributeData>();
            for (; ; )
            {
                GetCustomAttributesImpl(list, member, attributeType);

                var type = member as Type;
                if (type != null)
                {
                    type = type.BaseType;
                    if (type == null)
                        return list;

                    member = type;
                    continue;
                }

                var method = member as MethodInfo;
                if (method != null)
                {
                    var prev = member;
                    method = method.GetBaseDefinition();
                    if (method == null || method == prev)
                        return list;

                    member = method;
                    continue;
                }

                return list;
            }
        }

        /// <summary>
        /// Adds the pseudo custom attributes and custom attributes of the specified type that have been applied to <paramref name="member"/>.
        /// </summary>
        /// <param name="list">The list to add to, or <see langword="null"/> to create one when needed.</param>
        /// <param name="member">The member whose attributes are returned.</param>
        /// <param name="attributeType">The type of attribute to return, or <see langword="null"/> for all attributes.</param>
        /// <returns>The list, or <see langword="null"/> if <paramref name="list"/> was <see langword="null"/> and no attributes were found.</returns>
        static List<CustomAttributeData> GetCustomAttributesImpl(List<CustomAttributeData> list, MemberInfo member, Type attributeType)
        {
            var pseudo = member.GetPseudoCustomAttributes(attributeType);
            if (list == null)
                list = pseudo;
            else if (pseudo != null)
                list.AddRange(pseudo);

            return GetCustomAttributesImpl(list, member.Module, member.GetCurrentToken(), attributeType);
        }

        /// <summary>
        /// Adds the custom attributes of the specified type that are attached to the metadata entity identified by <paramref name="token"/>.
        /// </summary>
        /// <param name="list">The list to add to, or <see langword="null"/> to create one when needed.</param>
        /// <param name="module">The module that defines the entity.</param>
        /// <param name="token">The metadata token of the entity.</param>
        /// <param name="attributeType">The type of attribute to return, or <see langword="null"/> for all attributes.</param>
        /// <returns>The list, or <see langword="null"/> if <paramref name="list"/> was <see langword="null"/> and no attributes were found.</returns>
        internal static List<CustomAttributeData> GetCustomAttributesImpl(List<CustomAttributeData> list, Module module, int token, Type attributeType)
        {
            if (module is ModuleBuilder builder)
                return builder.GetCustomAttributes(list, token, attributeType);

            if (module is not ModuleReader reader || (token & 0xFFFFFF) == 0)
                return list;

            foreach (var h in reader.Metadata.GetCustomAttributes(MetadataTokens.EntityHandle(token)))
            {
                if (attributeType == null || attributeType.IsAssignableFrom(module.ResolveMethod(MetadataTokens.GetToken(reader.Metadata.GetCustomAttribute(h).Constructor)).DeclaringType))
                {
                    list ??= [];
                    list.Add(new CustomAttributeData(reader, h));
                }
            }

            return list;
        }

        /// <summary>
        /// Determines whether an attribute type is inherited, according to the <c>Inherited</c> property of its <see cref="AttributeUsageAttribute"/>.
        /// </summary>
        /// <param name="attribute">The attribute type.</param>
        /// <returns><see langword="true"/> if the attribute is inherited or has no <see cref="AttributeUsageAttribute"/>; otherwise, <see langword="false"/>.</returns>
        static bool IsInheritableAttribute(Type attribute)
        {
            var attributeUsageAttribute = attribute.Module.Universe.System_AttributeUsageAttribute;
            var attr = __GetCustomAttributes(attribute, attributeUsageAttribute, false);
            if (attr.Count != 0)
                foreach (CustomAttributeNamedArgument named in attr[0].NamedArguments)
                    if (named.MemberInfo.Name == "Inherited")
                        return (bool)named.TypedValue.Value;

            return true;
        }

        /// <summary>
        /// Creates a <see cref="System.Runtime.InteropServices.DllImportAttribute"/> pseudo custom attribute from P/Invoke mapping information.
        /// </summary>
        /// <param name="module">The module that defines the method.</param>
        /// <param name="flags">The P/Invoke attributes of the mapping.</param>
        /// <param name="entryPoint">The name of the entry point.</param>
        /// <param name="dllName">The name of the module the entry point is imported from.</param>
        /// <param name="attr">The implementation attributes of the method, used for <c>PreserveSig</c>.</param>
        /// <returns>The pseudo custom attribute, or <see langword="null"/> if the core library has no DllImportAttribute.</returns>
        internal static CustomAttributeData CreateDllImportPseudoCustomAttribute(Module module, ImplMapFlags flags, string entryPoint, string dllName, MethodImplAttributes attr)
        {
            if (module.Universe.System_Runtime_InteropServices_DllImportAttribute == null)
                return null;

            var charSet = (flags & ImplMapFlags.CharSetMask) switch
            {
                ImplMapFlags.CharSetAnsi => System.Runtime.InteropServices.CharSet.Ansi,
                ImplMapFlags.CharSetUnicode => System.Runtime.InteropServices.CharSet.Unicode,
                ImplMapFlags.CharSetAuto => System.Runtime.InteropServices.CharSet.Auto,
                _ => System.Runtime.InteropServices.CharSet.None,
            };

            var callingConvention = (flags & ImplMapFlags.CallConvMask) switch
            {
                ImplMapFlags.CallConvCdecl => System.Runtime.InteropServices.CallingConvention.Cdecl,
                ImplMapFlags.CallConvFastcall => System.Runtime.InteropServices.CallingConvention.FastCall,
                ImplMapFlags.CallConvStdcall => System.Runtime.InteropServices.CallingConvention.StdCall,
                ImplMapFlags.CallConvThiscall => System.Runtime.InteropServices.CallingConvention.ThisCall,
                ImplMapFlags.CallConvWinapi => System.Runtime.InteropServices.CallingConvention.Winapi,
                _ => (System.Runtime.InteropServices.CallingConvention)0,
            };

            var list = new List<CustomAttributeNamedArgument>();
            var type = module.Universe.System_Runtime_InteropServices_DllImportAttribute;
            var constructor = type.GetPseudoCustomAttributeConstructor(module.Universe.System_String);
            AddNamedArgument(list, type, "EntryPoint", entryPoint);
            AddNamedArgument(list, type, "CharSet", module.Universe.System_Runtime_InteropServices_CharSet, (int)charSet);
            AddNamedArgument(list, type, "ExactSpelling", (int)flags, (int)ImplMapFlags.NoMangle);
            AddNamedArgument(list, type, "SetLastError", (int)flags, (int)ImplMapFlags.SupportsLastError);
            AddNamedArgument(list, type, "PreserveSig", (int)attr, (int)MethodImplAttributes.PreserveSig);
            AddNamedArgument(list, type, "CallingConvention", module.Universe.System_Runtime_InteropServices_CallingConvention, (int)callingConvention);
            AddNamedArgument(list, type, "BestFitMapping", (int)flags, (int)ImplMapFlags.BestFitOn);
            AddNamedArgument(list, type, "ThrowOnUnmappableChar", (int)flags, (int)ImplMapFlags.CharMapErrorOn);
            return new CustomAttributeData(module, constructor, new object[] { dllName }, list);
        }

        /// <summary>
        /// Creates a <see cref="System.Runtime.InteropServices.MarshalAsAttribute"/> pseudo custom attribute from field marshalling information.
        /// </summary>
        /// <param name="module">The module that defines the field or parameter.</param>
        /// <param name="fm">The marshalling information.</param>
        /// <returns>The pseudo custom attribute, or <see langword="null"/> if the core library has no MarshalAsAttribute.</returns>
        internal static CustomAttributeData CreateMarshalAsPseudoCustomAttribute(Module module, FieldMarshal fm)
        {
            var typeofMarshalAs = module.Universe.System_Runtime_InteropServices_MarshalAsAttribute;
            if (typeofMarshalAs == null)
                return null;

            var typeofUnmanagedType = module.Universe.System_Runtime_InteropServices_UnmanagedType;
            var typeofVarEnum = module.Universe.System_Runtime_InteropServices_VarEnum;
            var typeofType = module.Universe.System_Type;
            var named = new List<CustomAttributeNamedArgument>();
            AddNamedArgument(named, typeofMarshalAs, "ArraySubType", typeofUnmanagedType, (int)(fm.ArraySubType ?? 0));
            AddNamedArgument(named, typeofMarshalAs, "SizeParamIndex", module.Universe.System_Int16, fm.SizeParamIndex ?? 0);
            AddNamedArgument(named, typeofMarshalAs, "SizeConst", module.Universe.System_Int32, fm.SizeConst ?? 0);
            AddNamedArgument(named, typeofMarshalAs, "IidParameterIndex", module.Universe.System_Int32, fm.IidParameterIndex ?? 0);
            AddNamedArgument(named, typeofMarshalAs, "SafeArraySubType", typeofVarEnum, (int)(fm.SafeArraySubType ?? 0));
            if (fm.SafeArrayUserDefinedSubType != null)
                AddNamedArgument(named, typeofMarshalAs, "SafeArrayUserDefinedSubType", typeofType, fm.SafeArrayUserDefinedSubType);
            if (fm.MarshalType != null)
                AddNamedArgument(named, typeofMarshalAs, "MarshalType", module.Universe.System_String, fm.MarshalType);
            if (fm.MarshalTypeRef != null)
                AddNamedArgument(named, typeofMarshalAs, "MarshalTypeRef", module.Universe.System_Type, fm.MarshalTypeRef);
            if (fm.MarshalCookie != null)
                AddNamedArgument(named, typeofMarshalAs, "MarshalCookie", module.Universe.System_String, fm.MarshalCookie);

            var constructor = typeofMarshalAs.GetPseudoCustomAttributeConstructor(typeofUnmanagedType);
            return new CustomAttributeData(module, constructor, new object[] { (int)fm.UnmanagedType }, named);
        }

        /// <summary>
        /// Adds a named argument for a string field of the attribute type.
        /// </summary>
        /// <param name="list">The list to add to.</param>
        /// <param name="type">The attribute type.</param>
        /// <param name="fieldName">The name of the field.</param>
        /// <param name="value">The value of the field.</param>
        static void AddNamedArgument(List<CustomAttributeNamedArgument> list, Type type, string fieldName, string value) =>
            AddNamedArgument(list, type, fieldName, type.Module.Universe.System_String, value);

        /// <summary>
        /// Adds a named argument for a Boolean field of the attribute type that is <see langword="true"/> when <paramref name="flagMask"/> is set in <paramref name="flags"/>.
        /// </summary>
        /// <param name="list">The list to add to.</param>
        /// <param name="type">The attribute type.</param>
        /// <param name="fieldName">The name of the field.</param>
        /// <param name="flags">The flags to test.</param>
        /// <param name="flagMask">The flag that determines the value of the field.</param>
        static void AddNamedArgument(List<CustomAttributeNamedArgument> list, Type type, string fieldName, int flags, int flagMask) =>
            AddNamedArgument(list, type, fieldName, type.Module.Universe.System_Boolean, (flags & flagMask) != 0);

        /// <summary>
        /// Adds a named argument for a field of the attribute type, if the attribute type has a field with that name and type.
        /// </summary>
        /// <param name="list">The list to add to.</param>
        /// <param name="attributeType">The attribute type.</param>
        /// <param name="fieldName">The name of the field.</param>
        /// <param name="valueType">The type of the field.</param>
        /// <param name="value">The value of the field.</param>
        static void AddNamedArgument(List<CustomAttributeNamedArgument> list, Type attributeType, string fieldName, Type valueType, object value)
        {
            // some fields are not available on the .NET Compact Framework version of DllImportAttribute/MarshalAsAttribute
            var field = attributeType.FindField(fieldName, FieldSignature.Create(valueType, new CustomModifiers()));
            if (field != null)
                list.Add(new CustomAttributeNamedArgument(field, new CustomAttributeTypedArgument(valueType, value)));
        }

        /// <summary>
        /// Creates a <see cref="System.Runtime.InteropServices.FieldOffsetAttribute"/> pseudo custom attribute.
        /// </summary>
        /// <param name="module">The module that defines the field.</param>
        /// <param name="offset">The offset of the field.</param>
        /// <returns>The pseudo custom attribute, or <see langword="null"/> if the core library has no FieldOffsetAttribute.</returns>
        internal static CustomAttributeData CreateFieldOffsetPseudoCustomAttribute(Module module, int offset)
        {
            var type = module.Universe.System_Runtime_InteropServices_FieldOffsetAttribute;
            if (type == null)
                return null;

            var constructor = type.GetPseudoCustomAttributeConstructor(module.Universe.System_Int32);
            return new CustomAttributeData(module, constructor, new object[] { offset }, null);
        }

        /// <summary>
        /// Creates a <see cref="System.Runtime.InteropServices.PreserveSigAttribute"/> pseudo custom attribute.
        /// </summary>
        /// <param name="module">The module that defines the method.</param>
        /// <returns>The pseudo custom attribute, or <see langword="null"/> if the core library has no PreserveSigAttribute.</returns>
        internal static CustomAttributeData CreatePreserveSigPseudoCustomAttribute(Module module)
        {
            var type = module.Universe.System_Runtime_InteropServices_PreserveSigAttribute;
            if (type == null)
                return null;

            var constructor = type.GetPseudoCustomAttributeConstructor();
            return new CustomAttributeData(module, constructor, Array.Empty<object>(), null);
        }

    }

}
