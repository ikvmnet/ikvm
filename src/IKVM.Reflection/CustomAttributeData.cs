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
using System.Text;

using IKVM.Reflection.Emit;
using IKVM.Reflection.Metadata;
using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    public sealed class CustomAttributeData
    {

        internal static readonly IList<CustomAttributeData> EmptyList = new List<CustomAttributeData>(0).AsReadOnly();

        /*
		 * There are several states a CustomAttributeData object can be in:
		 * 
		 * 1) Unresolved Custom Attribute
		 *    - customAttributeIndex >= 0
		 *    - declSecurityIndex == -1
		 *    - declSecurityBlob == null
		 *    - lazyConstructor = null
		 *    - lazyConstructorArguments = null
		 *    - lazyNamedArguments = null
		 * 
		 * 2) Resolved Custom Attribute
		 *    - customAttributeIndex >= 0
		 *    - declSecurityIndex == -1
		 *    - declSecurityBlob == null
		 *    - lazyConstructor != null
		 *    - lazyConstructorArguments != null
		 *    - lazyNamedArguments != null
		 *    
		 * 3) Pre-resolved Custom Attribute
		 *    - customAttributeIndex = -1
		 *    - declSecurityIndex == -1
		 *    - declSecurityBlob == null
		 *    - lazyConstructor != null
		 *    - lazyConstructorArguments != null
		 *    - lazyNamedArguments != null
		 *    
		 * 4) Pseudo Custom Attribute, .NET 1.x declarative security or result of CustomAttributeBuilder.ToData()
		 *    - customAttributeIndex = -1
		 *    - declSecurityIndex == -1
		 *    - declSecurityBlob == null
		 *    - lazyConstructor != null
		 *    - lazyConstructorArguments != null
		 *    - lazyNamedArguments != null
		 *    
		 * 5) Unresolved declarative security
		 *    - customAttributeIndex = -1
		 *    - declSecurityIndex >= 0
		 *    - declSecurityBlob != null
		 *    - lazyConstructor != null
		 *    - lazyConstructorArguments != null
		 *    - lazyNamedArguments == null
		 * 
		 * 6) Resolved declarative security
		 *    - customAttributeIndex = -1
		 *    - declSecurityIndex >= 0
		 *    - declSecurityBlob == null
		 *    - lazyConstructor != null
		 *    - lazyConstructorArguments != null
		 *    - lazyNamedArguments != null
		 * 
		 */

        readonly Module module;
        readonly int customAttributeIndex;
        readonly int declSecurityIndex;
        readonly byte[] declSecurityBlob;

        ConstructorInfo lazyConstructor;
        IList<CustomAttributeTypedArgument> lazyConstructorArguments;
        IList<CustomAttributeNamedArgument> lazyNamedArguments;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="index"></param>
        internal CustomAttributeData(Module module, int index)
        {
            this.module = module ?? throw new ArgumentNullException(nameof(module));
            this.customAttributeIndex = index;
            this.declSecurityIndex = -1;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="constructor"></param>
        /// <param name="args"></param>
        /// <param name="namedArguments"></param>
        internal CustomAttributeData(Module module, ConstructorInfo constructor, object[] args, List<CustomAttributeNamedArgument> namedArguments) :
            this(module, constructor, WrapConstructorArgs(args, constructor.MethodSignature), namedArguments)
        {

        }

        static List<CustomAttributeTypedArgument> WrapConstructorArgs(object[] args, MethodSignature sig)
        {
            var list = new List<CustomAttributeTypedArgument>();
            for (int i = 0; i < args.Length; i++)
                list.Add(new CustomAttributeTypedArgument(sig.GetParameterType(i), args[i]));

            return list;
        }

        // 4) Pseudo Custom Attribute, .NET 1.x declarative security or result of CustomAttributeBuilder.ToData()

        /// <summary>
        /// Pseudo Custom Attribute for .NET 1.x declarative security or result of CustomAttributeBuilder.ToData()
        /// </summary>
        /// <param name="module"></param>
        /// <param name="constructor"></param>
        /// <param name="constructorArgs"></param>
        /// <param name="namedArguments"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal CustomAttributeData(Module module, ConstructorInfo constructor, List<CustomAttributeTypedArgument> constructorArgs, List<CustomAttributeNamedArgument> namedArguments)
        {
            this.module = module ?? throw new ArgumentNullException(nameof(module));
            this.customAttributeIndex = -1;
            this.declSecurityIndex = -1;
            this.lazyConstructor = constructor;

            lazyConstructorArguments = constructorArgs.AsReadOnly();
            if (namedArguments == null)
                this.lazyNamedArguments = Array.Empty<CustomAttributeNamedArgument>();
            else
                this.lazyNamedArguments = namedArguments.AsReadOnly();
        }

        /// <summary>
        /// Pre-resolved custom attribute.
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="constructor"></param>
        /// <param name="br"></param>
        /// <exception cref="BadImageFormatException"></exception>
        internal CustomAttributeData(Assembly asm, ConstructorInfo constructor, ByteReader br)
        {
            this.module = asm.ManifestModule;
            this.customAttributeIndex = -1;
            this.declSecurityIndex = -1;
            this.lazyConstructor = constructor;
            if (br.Length == 0)
            {
                // it's legal to have an empty blob
                lazyConstructorArguments = Array.Empty<CustomAttributeTypedArgument>();
                lazyNamedArguments = Array.Empty<CustomAttributeNamedArgument>();
            }
            else
            {
                if (br.ReadUInt16() != 1)
                    throw new BadImageFormatException();

                lazyConstructorArguments = ReadConstructorArguments(module, br, constructor);
                lazyNamedArguments = ReadNamedArguments(module, br, br.ReadUInt16(), constructor.DeclaringType, true);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('[');
            sb.Append(Constructor.DeclaringType.FullName);
            sb.Append('(');

            var sep = "";
            var parameters = Constructor.GetParameters();
            var args = ConstructorArguments;

            for (int i = 0; i < parameters.Length; i++)
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

        internal static void ReadDeclarativeSecurity(Module module, int index, List<CustomAttributeData> list)
        {
            var asm = module.Assembly;
            var action = module.DeclSecurityTable.records[index].Action;
            var br = module.GetBlobReader(module.DeclSecurityTable.records[index].PermissionSet);
            if (br.PeekByte() == '.')
            {
                br.ReadByte();
                var count = br.ReadCompressedUInt();
                for (int j = 0; j < count; j++)
                {
                    var type = ReadType(module, br);
                    var constructor = type.GetPseudoCustomAttributeConstructor(module.Universe.System_Security_Permissions_SecurityAction);
                    // LAMESPEC there is an additional length here (probably of the named argument list)
                    var blob = br.ReadBytes(br.ReadCompressedUInt());
                    list.Add(new CustomAttributeData(asm, constructor, action, blob, index));
                }
            }
            else
            {
                // .NET 1.x format (xml)
                var buf = new char[br.Length / 2];
                for (int i = 0; i < buf.Length; i++)
                    buf[i] = br.ReadChar();

                var xml = new string(buf);
                var ctor = module.Universe.System_Security_Permissions_PermissionSetAttribute.GetPseudoCustomAttributeConstructor(module.Universe.System_Security_Permissions_SecurityAction);
                var args = new List<CustomAttributeNamedArgument>();
                args.Add(new CustomAttributeNamedArgument(GetProperty(null, module.Universe.System_Security_Permissions_PermissionSetAttribute, "XML", module.Universe.System_String), new CustomAttributeTypedArgument(module.Universe.System_String, xml)));
                list.Add(new CustomAttributeData(asm.ManifestModule, ctor, new object[] { action }, args));
            }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="constructor"></param>
        /// <param name="securityAction"></param>
        /// <param name="blob"></param>
        /// <param name="index"></param>
        internal CustomAttributeData(Assembly asm, ConstructorInfo constructor, int securityAction, byte[] blob, int index)
        {
            this.module = asm.ManifestModule;
            this.customAttributeIndex = -1;
            this.declSecurityIndex = index;
            this.lazyConstructor = constructor;

            var list = new List<CustomAttributeTypedArgument>();
            list.Add(new CustomAttributeTypedArgument(constructor.Module.Universe.System_Security_Permissions_SecurityAction, securityAction));
            this.lazyConstructorArguments = list.AsReadOnly();
            this.declSecurityBlob = blob;
        }

        static Type ReadFieldOrPropType(Module context, ByteReader br)
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
                Signature.ELEMENT_TYPE_SZARRAY => ReadFieldOrPropType(context, br).MakeArrayType(),
                0x55 => ReadType(context, br),
                0x50 => context.Universe.System_Type,
                0x51 => context.Universe.System_Object,
                _ => throw new BadImageFormatException(),
            };
        }

        static CustomAttributeTypedArgument ReadFixedArg(Module context, ByteReader br, Type type)
        {
            var u = context.Universe;
            if (type == u.System_String)
            {
                return new CustomAttributeTypedArgument(type, br.ReadString());
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
                return new CustomAttributeTypedArgument(type, ReadType(context, br));
            }
            else if (type == u.System_Object)
            {
                return ReadFixedArg(context, br, ReadFieldOrPropType(context, br));
            }
            else if (type.IsArray)
            {
                var length = br.ReadInt32();
                if (length == -1)
                    return new CustomAttributeTypedArgument(type, null);

                var elementType = type.GetElementType();
                var array = new CustomAttributeTypedArgument[length];
                for (int i = 0; i < length; i++)
                    array[i] = ReadFixedArg(context, br, elementType);

                return new CustomAttributeTypedArgument(type, array);
            }
            else if (type.IsEnum)
            {
                return new CustomAttributeTypedArgument(type, ReadFixedArg(context, br, type.GetEnumUnderlyingTypeImpl()).Value);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        static Type ReadType(Module context, ByteReader br)
        {
            var typeName = br.ReadString();
            if (typeName == null)
                return null;

            // there are broken compilers that emit an extra NUL character after the type name
            if (typeName.Length > 0 && typeName[typeName.Length - 1] == 0)
                typeName = typeName.Substring(0, typeName.Length - 1);

            return TypeNameParser.Parse(typeName, true).GetType(context.Universe, context, true, typeName, true, false);
        }

        static IList<CustomAttributeTypedArgument> ReadConstructorArguments(Module context, ByteReader br, ConstructorInfo constructor)
        {
            var sig = constructor.MethodSignature;
            var count = sig.GetParameterCount();
            var list = new List<CustomAttributeTypedArgument>(count);
            for (int i = 0; i < count; i++)
                list.Add(ReadFixedArg(context, br, sig.GetParameterType(i)));

            return list.AsReadOnly();
        }

        static IList<CustomAttributeNamedArgument> ReadNamedArguments(Module context, ByteReader br, int named, Type type, bool required)
        {
            var list = new List<CustomAttributeNamedArgument>(named);
            for (int i = 0; i < named; i++)
            {
                var fieldOrProperty = br.ReadByte();
                var fieldOrPropertyType = ReadFieldOrPropType(context, br);
                if (fieldOrPropertyType.__IsMissing && !required)
                    return null;

                var name = br.ReadString();
                var value = ReadFixedArg(context, br, fieldOrPropertyType);
                var member = fieldOrProperty switch
                {
                    0x53 => (MemberInfo)GetField(context, type, name, fieldOrPropertyType),
                    0x54 => (MemberInfo)GetProperty(context, type, name, fieldOrPropertyType),
                    _ => throw new BadImageFormatException(),
                };

                list.Add(new CustomAttributeNamedArgument(member, value));
            }

            return list.AsReadOnly();
        }

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

        [Obsolete("Use AttributeType property instead.")]
        internal bool __TryReadTypeName(out string ns, out string name)
        {
            if (Constructor.DeclaringType.IsNested)
            {
                ns = null;
                name = null;
                return false;
            }

            var typeName = AttributeType.TypeName;
            ns = typeName.Namespace;
            name = typeName.Name;
            return true;
        }

        public byte[] __GetBlob()
        {
            if (declSecurityBlob != null)
                return (byte[])declSecurityBlob.Clone();
            else if (customAttributeIndex == -1)
                return __ToBuilder().GetBlob(module.Assembly);
            else
                return ((ModuleReader)module).GetBlobCopy(module.CustomAttributeTable.records[customAttributeIndex].Value);
        }

        public int __Parent
        {
            get
            {
                return customAttributeIndex >= 0
                    ? module.CustomAttributeTable.records[customAttributeIndex].Parent
                    : declSecurityIndex >= 0
                        ? module.DeclSecurityTable.records[declSecurityIndex].Parent
                        : 0;
            }
        }

        public Type AttributeType
        {
            get { return Constructor.DeclaringType; }
        }

        public ConstructorInfo Constructor
        {
            get
            {
                if (lazyConstructor == null)
                    lazyConstructor = (ConstructorInfo)module.ResolveMethod(module.CustomAttributeTable.records[customAttributeIndex].Constructor);

                return lazyConstructor;
            }
        }

        public IList<CustomAttributeTypedArgument> ConstructorArguments
        {
            get
            {
                if (lazyConstructorArguments == null)
                    LazyParseArguments(false);

                return lazyConstructorArguments;
            }
        }

        public IList<CustomAttributeNamedArgument> NamedArguments
        {
            get
            {
                if (lazyNamedArguments == null)
                {
                    if (customAttributeIndex >= 0)
                    {
                        // 1) Unresolved Custom Attribute
                        LazyParseArguments(true);
                    }
                    else
                    {
                        // 5) Unresolved declarative security
                        ByteReader br = new ByteReader(declSecurityBlob, 0, declSecurityBlob.Length);
                        // LAMESPEC the count of named arguments is a compressed integer (instead of UInt16 as NumNamed in custom attributes)
                        lazyNamedArguments = ReadNamedArguments(module, br, br.ReadCompressedUInt(), Constructor.DeclaringType, true);
                    }
                }

                return lazyNamedArguments;
            }
        }

        void LazyParseArguments(bool requireNameArguments)
        {
            var br = module.GetBlobReader(module.CustomAttributeTable.records[customAttributeIndex].Value);
            if (br.Length == 0)
            {
                // it's legal to have an empty blob
                lazyConstructorArguments = Array.Empty<CustomAttributeTypedArgument>();
                lazyNamedArguments = Array.Empty<CustomAttributeNamedArgument>();
            }
            else
            {
                if (br.ReadUInt16() != 1)
                    throw new BadImageFormatException();

                lazyConstructorArguments = ReadConstructorArguments(module, br, Constructor);
                lazyNamedArguments = ReadNamedArguments(module, br, br.ReadUInt16(), Constructor.DeclaringType, requireNameArguments);
            }
        }

        public CustomAttributeBuilder __ToBuilder()
        {
            var parameters = Constructor.GetParameters();
            var args = new object[ConstructorArguments.Count];
            for (int i = 0; i < args.Length; i++)
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

        static object RewrapArray(Type type, CustomAttributeTypedArgument arg)
        {
            var list = arg.Value as IList<CustomAttributeTypedArgument>;
            if (list != null)
            {
                var elementType = arg.ArgumentType.GetElementType();
                var arr = new object[list.Count];
                for (int i = 0; i < arr.Length; i++)
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

        public static IList<CustomAttributeData> GetCustomAttributes(MemberInfo member)
        {
            return __GetCustomAttributes(member, null, false);
        }

        public static IList<CustomAttributeData> GetCustomAttributes(Assembly assembly)
        {
            return assembly.GetCustomAttributesData(null);
        }

        public static IList<CustomAttributeData> GetCustomAttributes(Module module)
        {
            return __GetCustomAttributes(module, null, false);
        }

        public static IList<CustomAttributeData> GetCustomAttributes(ParameterInfo parameter)
        {
            return __GetCustomAttributes(parameter, null, false);
        }

        public static IList<CustomAttributeData> __GetCustomAttributes(Assembly assembly, Type attributeType, bool inherit)
        {
            return assembly.GetCustomAttributesData(attributeType);
        }

        public static IList<CustomAttributeData> __GetCustomAttributes(Module module, Type attributeType, bool inherit)
        {
            if (module.__IsMissing)
                throw new MissingModuleException((MissingModule)module);

            return GetCustomAttributesImpl(null, module, 0x00000001, attributeType) ?? EmptyList;
        }

        public static IList<CustomAttributeData> __GetCustomAttributes(ParameterInfo parameter, Type attributeType, bool inherit)
        {
            var module = parameter.Module;
            List<CustomAttributeData> list = null;
            if (module.Universe.ReturnPseudoCustomAttributes)
            {
                if (attributeType == null || attributeType.IsAssignableFrom(parameter.Module.Universe.System_Runtime_InteropServices_MarshalAsAttribute))
                {
                    if (parameter.__TryGetFieldMarshal(out var spec))
                    {
                        list ??= new List<CustomAttributeData>();
                        list.Add(CustomAttributeData.CreateMarshalAsPseudoCustomAttribute(parameter.Module, spec));
                    }
                }
            }

            var token = parameter.MetadataToken;
            if (module is ModuleBuilder mb && mb.IsSaved && ModuleBuilder.IsPseudoToken(token))
                token = mb.ResolvePseudoToken(token);

            return GetCustomAttributesImpl(list, module, token, attributeType) ?? EmptyList;
        }

        public static IList<CustomAttributeData> __GetCustomAttributes(MemberInfo member, Type attributeType, bool inherit)
        {
            // like .NET we we don't return custom attributes for unbaked members
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

        static List<CustomAttributeData> GetCustomAttributesImpl(List<CustomAttributeData> list, MemberInfo member, Type attributeType)
        {
            if (member.Module.Universe.ReturnPseudoCustomAttributes)
            {
                var pseudo = member.GetPseudoCustomAttributes(attributeType);
                if (list == null)
                    list = pseudo;
                else if (pseudo != null)
                    list.AddRange(pseudo);
            }

            return GetCustomAttributesImpl(list, member.Module, member.GetCurrentToken(), attributeType);
        }

        internal static List<CustomAttributeData> GetCustomAttributesImpl(List<CustomAttributeData> list, Module module, int token, Type attributeType)
        {
            foreach (var i in module.CustomAttributeTable.Filter(token))
            {
                if (attributeType == null)
                {
                    list ??= new List<CustomAttributeData>();
                    list.Add(new CustomAttributeData(module, i));
                }
                else
                {
                    if (attributeType.IsAssignableFrom(module.ResolveMethod(module.CustomAttributeTable.records[i].Constructor).DeclaringType))
                    {
                        list ??= new List<CustomAttributeData>();
                        list.Add(new CustomAttributeData(module, i));
                    }
                }
            }

            return list;
        }

        public static IList<CustomAttributeData> __GetCustomAttributes(Type type, Type interfaceType, Type attributeType, bool inherit)
        {
            var module = type.Module;
            foreach (int i in module.InterfaceImplTable.Filter(type.MetadataToken))
                if (module.ResolveType(module.InterfaceImplTable.records[i].Interface, type) == interfaceType)
                    return GetCustomAttributesImpl(null, module, (InterfaceImplTable.Index << 24) | (i + 1), attributeType) ?? EmptyList;

            return EmptyList;
        }

        public static IList<CustomAttributeData> __GetDeclarativeSecurity(Assembly assembly)
        {
            if (assembly.__IsMissing)
                throw new MissingAssemblyException((MissingAssembly)assembly);

            return assembly.ManifestModule.GetDeclarativeSecurity(0x20000001);
        }

        public static IList<CustomAttributeData> __GetDeclarativeSecurity(Type type)
        {
            if ((type.Attributes & TypeAttributes.HasSecurity) != 0)
                return type.Module.GetDeclarativeSecurity(type.MetadataToken);
            else
                return EmptyList;
        }

        public static IList<CustomAttributeData> __GetDeclarativeSecurity(MethodBase method)
        {
            if ((method.Attributes & MethodAttributes.HasSecurity) != 0)
                return method.Module.GetDeclarativeSecurity(method.MetadataToken);
            else
                return EmptyList;
        }

        private static bool IsInheritableAttribute(Type attribute)
        {
            var attributeUsageAttribute = attribute.Module.Universe.System_AttributeUsageAttribute;
            var attr = __GetCustomAttributes(attribute, attributeUsageAttribute, false);
            if (attr.Count != 0)
                foreach (CustomAttributeNamedArgument named in attr[0].NamedArguments)
                    if (named.MemberInfo.Name == "Inherited")
                        return (bool)named.TypedValue.Value;

            return true;
        }

        internal static CustomAttributeData CreateDllImportPseudoCustomAttribute(Module module, ImplMapFlags flags, string entryPoint, string dllName, MethodImplAttributes attr)
        {

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

        internal static CustomAttributeData CreateMarshalAsPseudoCustomAttribute(Module module, FieldMarshal fm)
        {
            var typeofMarshalAs = module.Universe.System_Runtime_InteropServices_MarshalAsAttribute;
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

        static void AddNamedArgument(List<CustomAttributeNamedArgument> list, Type type, string fieldName, string value)
        {
            AddNamedArgument(list, type, fieldName, type.Module.Universe.System_String, value);
        }

        static void AddNamedArgument(List<CustomAttributeNamedArgument> list, Type type, string fieldName, int flags, int flagMask)
        {
            AddNamedArgument(list, type, fieldName, type.Module.Universe.System_Boolean, (flags & flagMask) != 0);
        }

        static void AddNamedArgument(List<CustomAttributeNamedArgument> list, Type attributeType, string fieldName, Type valueType, object value)
        {
            // some fields are not available on the .NET Compact Framework version of DllImportAttribute/MarshalAsAttribute
            var field = attributeType.FindField(fieldName, FieldSignature.Create(valueType, new CustomModifiers()));
            if (field != null)
                list.Add(new CustomAttributeNamedArgument(field, new CustomAttributeTypedArgument(valueType, value)));
        }

        internal static CustomAttributeData CreateFieldOffsetPseudoCustomAttribute(Module module, int offset)
        {
            var type = module.Universe.System_Runtime_InteropServices_FieldOffsetAttribute;
            var constructor = type.GetPseudoCustomAttributeConstructor(module.Universe.System_Int32);
            return new CustomAttributeData(module, constructor, new object[] { offset }, null);
        }

        internal static CustomAttributeData CreatePreserveSigPseudoCustomAttribute(Module module)
        {
            var type = module.Universe.System_Runtime_InteropServices_PreserveSigAttribute;
            var constructor = type.GetPseudoCustomAttributeConstructor();
            return new CustomAttributeData(module, constructor, Array.Empty<object>(), null);
        }

    }

}
