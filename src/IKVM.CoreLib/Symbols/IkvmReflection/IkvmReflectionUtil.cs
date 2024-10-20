using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    static class IkvmReflectionUtil
    {

        /// <summary>
        /// Converts a <see cref="AssemblyName"/> to a <see cref="AssemblyIdentity"/>.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static AssemblyIdentity Pack(this AssemblyName assemblyName)
        {
            var pk = assemblyName.GetPublicKey()?.ToImmutableArray() ?? ImmutableArray<byte>.Empty;
            var hasPublicKey = pk.Length > 0;
            var pkt = assemblyName.GetPublicKeyToken()?.ToImmutableArray() ?? ImmutableArray<byte>.Empty;

            return new AssemblyIdentity(
                assemblyName.Name ?? throw new InvalidOperationException(),
                assemblyName.Version,
                assemblyName.CultureName,
                hasPublicKey ? pk : pkt,
                hasPublicKey,
                (System.Reflection.AssemblyContentType)assemblyName.ContentType,
                (System.Reflection.ProcessorArchitecture)assemblyName.ProcessorArchitecture);
        }

        /// <summary>
        /// Converts a set of <see cref="AssemblyName"/> to a set of <see cref="AssemblyIdentity"/>.
        /// </summary>
        /// <param name="assemblyNames"></param>
        /// <returns></returns>
        public static AssemblyIdentity[] Pack(this AssemblyName[] assemblyNames)
        {
            if (assemblyNames.Length == 0)
                return [];

            var a = new AssemblyIdentity[assemblyNames.Length];
            for (int i = 0; i < assemblyNames.Length; i++)
                a[i] = assemblyNames[i].Pack();

            return a;
        }

        /// <summary>
        /// Converts a <see cref="AssemblyIdentity"/> to a <see cref="AssemblyName"/>.
        /// </summary>
        /// <param name="assemblyNameInfo"></param>
        /// <returns></returns>
        public static AssemblyName Unpack(this AssemblyIdentity assemblyNameInfo)
        {
            AssemblyName assemblyName = new();
            assemblyName.Name = assemblyNameInfo.Name;
            assemblyName.CultureName = assemblyNameInfo.CultureName;
            assemblyName.Version = assemblyNameInfo.Version;
            assemblyName.Flags = (AssemblyNameFlags)assemblyNameInfo.Flags;
            assemblyName.ContentType = (AssemblyContentType)assemblyNameInfo.ContentType;
            assemblyName.ProcessorArchitecture = (ProcessorArchitecture)assemblyNameInfo.ProcessorArchitecture;

            if (assemblyNameInfo.HasPublicKey)
                assemblyName.SetPublicKey(assemblyNameInfo.PublicKey.ToArray());
            else if (assemblyNameInfo.PublicKeyToken.IsDefaultOrEmpty == false)
                assemblyName.SetPublicKeyToken(assemblyNameInfo.PublicKeyToken.ToArray());

            return assemblyName;
        }

        /// <summary>
        /// Converts a set of <see cref="AssemblyIdentity"/> to a set of <see cref="AssemblyName"/>.
        /// </summary>
        /// <param name="assemblyNameInfos"></param>
        /// <returns></returns>
        public static AssemblyName[] Unpack(this AssemblyIdentity[] assemblyNameInfos)
        {
            if (assemblyNameInfos.Length == 0)
                return [];

            var a = new AssemblyName[assemblyNameInfos.Length];
            for (int i = 0; i < assemblyNameInfos.Length; i++)
                a[i] = assemblyNameInfos[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the symbol into their original assembly.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Assembly Unpack(this IAssemblySymbol type)
        {
            if (type is IIkvmReflectionAssemblySymbol symbol)
                return symbol.UnderlyingAssembly;

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Unpacks the symbol into their original type.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static Module Unpack(this IModuleSymbol module)
        {
            if (module is IIkvmReflectionModuleSymbol symbol)
                return symbol.UnderlyingModule;

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static Module[] Unpack(this IModuleSymbol[] modules)
        {
            if (modules.Length == 0)
                return [];

            var a = new Module[modules.Length];
            for (int i = 0; i < modules.Length; i++)
                a[i] = modules[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the symbol into their original type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type Unpack(this ITypeSymbol type)
        {
            if (type is IIkvmReflectionTypeSymbol symbol)
                return symbol.UnderlyingType;

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static Type[] Unpack(this ITypeSymbol[] types)
        {
            if (types.Length == 0)
                return [];

            var a = new Type[types.Length];
            for (int i = 0; i < types.Length; i++)
                a[i] = types[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static Type[][] Unpack(this ITypeSymbol[][] types)
        {
            if (types.Length == 0)
                return [];

            var a = new Type[types.Length][];
            for (int i = 0; i < types.Length; i++)
                a[i] = types[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the symbol into their original type.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static MemberInfo Unpack(this IMemberSymbol member)
        {
            if (member is IIkvmReflectionMemberSymbol symbol)
                return symbol.UnderlyingMember;

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public static MemberInfo[] Unpack(this IMemberSymbol[] members)
        {
            if (members.Length == 0)
                return [];

            var a = new MemberInfo[members.Length];
            for (int i = 0; i < members.Length; i++)
                a[i] = members[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the symbol into their original type.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public static ConstructorInfo Unpack(this IConstructorSymbol ctor)
        {
            if (ctor is IIkvmReflectionConstructorSymbol symbol)
                return symbol.UnderlyingConstructor;

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public static ConstructorInfo[] Unpack(this IConstructorSymbol[] ctor)
        {
            if (ctor.Length == 0)
                return [];

            var a = new ConstructorInfo[ctor.Length];
            for (int i = 0; i < ctor.Length; i++)
                a[i] = ctor[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the symbol into their original type.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public static MethodInfo Unpack(this IMethodSymbol ctor)
        {
            if (ctor is IIkvmReflectionMethodSymbol symbol)
                return symbol.UnderlyingMethod;

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public static MethodInfo[] Unpack(this IMethodSymbol[] ctor)
        {
            if (ctor.Length == 0)
                return [];

            var a = new MethodInfo[ctor.Length];
            for (int i = 0; i < ctor.Length; i++)
                a[i] = ctor[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the symbol into their original type.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static FieldInfo Unpack(this IFieldSymbol field)
        {
            if (field is IIkvmReflectionFieldSymbol symbol)
                return symbol.UnderlyingField;

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static FieldInfo[] Unpack(this IFieldSymbol[] fields)
        {
            if (fields.Length == 0)
                return [];

            var a = new FieldInfo[fields.Length];
            for (int i = 0; i < fields.Length; i++)
                a[i] = fields[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the symbol into their original type.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static PropertyInfo Unpack(this IPropertySymbol property)
        {
            if (property is IIkvmReflectionPropertySymbol symbol)
                return symbol.UnderlyingProperty;

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static PropertyInfo[] Unpack(this IPropertySymbol[] properties)
        {
            if (properties.Length == 0)
                return [];

            var a = new PropertyInfo[properties.Length];
            for (int i = 0; i < properties.Length; i++)
                a[i] = properties[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the parameter modifier.
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static ParameterModifier Unpack(this System.Reflection.ParameterModifier modifier)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Unpacks the parameter modifier.
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static ParameterModifier[] Unpack(this System.Reflection.ParameterModifier[] modifiers)
        {
            if (modifiers.Length == 0)
                return [];

            var a = new ParameterModifier[modifiers.Length];
            for (int i = 0; i < modifiers.Length; i++)
                a[i] = modifiers[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the <see cref="ILocalBuilder"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static LocalBuilder Unpack(this ILocalBuilder builder)
        {
            return ((IkvmReflectionLocalBuilder)builder).UnderlyingLocalBuilder;
        }

        /// <summary>
        /// Unpacks the <see cref="ILabel"/>.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static Label Unpack(this ILabel label)
        {
            return ((IkvmReflectionLabel)label).UnderlyingLabel;
        }

        /// <summary>
        /// Unpacks the <see cref="ILabel"/>s.
        /// </summary>
        /// <param name="labels"></param>
        /// <returns></returns>
        public static Label[] Unpack(this ILabel[] labels)
        {
            var a = new Label[labels.Length];
            for (int i = 0; i < labels.Length; i++)
                a[i] = labels[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the <see cref="CustomAttribute"/>.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static CustomAttributeBuilder Unpack(this CustomAttribute attributes)
        {
            // unpack constructor arg values
            var constructorValues = new object?[attributes.ConstructorArguments.Length];
            for (int i = 0; i < attributes.ConstructorArguments.Length; i++)
                constructorValues[i] = UnpackCustomAttributeValue(attributes.ConstructorArguments[i]);

            // unpack named arguments into sets of properties and fields
            List<PropertyInfo>? namedProperties = null;
            List<object?>? propertyValues = null;
            List<FieldInfo>? namedFields = null;
            List<object?>? fieldValues = null;

            // split named arguments into two sets
            foreach (var i in attributes.NamedArguments)
            {
                var v = UnpackCustomAttributeValue(i.TypedValue.Value);
                if (i.IsField == false)
                {
                    namedProperties ??= new();
                    namedProperties.Add(((IPropertySymbol)i.MemberInfo).Unpack());
                    propertyValues ??= new();
                    propertyValues.Add(v);
                }
                else
                {
                    namedFields ??= new();
                    namedFields.Add(((IFieldSymbol)i.MemberInfo).Unpack());
                    fieldValues ??= new();
                    fieldValues.Add(v);
                }
            }

            return new CustomAttributeBuilder(
                attributes.Constructor.Unpack(),
                constructorValues,
                namedProperties?.ToArray() ?? [],
                propertyValues?.ToArray() ?? [],
                namedFields?.ToArray() ?? [],
                fieldValues?.ToArray() ?? []);
        }

        /// <summary>
        /// Unpacks the <see cref="CustomAttribute"/>s.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static CustomAttributeBuilder[] Unpack(this CustomAttribute[] attributes)
        {
            if (attributes.Length == 0)
                return [];

            var a = new CustomAttributeBuilder[attributes.Length];
            for (int i = 0; i < attributes.Length; i++)
                a[i] = attributes[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the <see cref="CustomAttribute"/>s.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static CustomAttributeBuilder[] Unpack(this ImmutableArray<CustomAttribute> attributes)
        {
            if (attributes.Length == 0)
                return [];

            var a = new CustomAttributeBuilder[attributes.Length];
            for (int i = 0; i < attributes.Length; i++)
                a[i] = attributes[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the attribute value object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static object? UnpackCustomAttributeValue(object? value)
        {
            return value switch
            {
                IReadOnlyList<object> l => UnpackCustomAttributeArrayValue(l),
                ITypeSymbol t => t.Unpack(),
                CustomAttributeTypedArgument a => UnpackCustomAttributeValue(a.Value),
                _ => value,
            };
        }

        /// <summary>
        /// Unpacks the attribute value array.
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        static object?[] UnpackCustomAttributeArrayValue(IReadOnlyList<object> l)
        {
            var a = new object?[l.Count];
            for (int i = 0; i < l.Count; i++)
                a[i] = UnpackCustomAttributeValue(l[i]);

            return a;
        }

        /// <summary>
        /// Returns <c>true</c> if the given type represents a type definition.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTypeDefinition(this Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return type.HasElementType == false && type.IsConstructedGenericType == false && type.IsGenericParameter == false;
        }

        /// <summary>
        /// Returns <c>true</c> if the given method represents a method definition.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool IsMethodDefinition(this MethodBase method)
        {
            if (method is null)
                throw new ArgumentNullException(nameof(method));

            return method.IsGenericMethod == false || method.IsGenericMethodDefinition;
        }

    }

}
