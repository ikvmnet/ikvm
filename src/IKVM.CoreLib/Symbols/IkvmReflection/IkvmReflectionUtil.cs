using System;

using IKVM.Reflection;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    static class IkvmReflectionUtil
    {

#pragma warning disable SYSLIB0017 // Type or member is obsolete
#pragma warning disable SYSLIB0037 // Type or member is obsolete

        /// <summary>
        /// Converts a <see cref="AssemblyName"/> to a <see cref="System.Reflection.AssemblyName"/>.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static System.Reflection.AssemblyName ToAssemblyName(this AssemblyName n)
        {
            return new System.Reflection.AssemblyName()
            {
                Name = n.Name,
                Version = n.Version,
                CultureInfo = n.CultureInfo,
                CultureName = n.CultureName,
                ProcessorArchitecture = (System.Reflection.ProcessorArchitecture)n.ProcessorArchitecture,
                Flags = (System.Reflection.AssemblyNameFlags)n.Flags,
                HashAlgorithm = (System.Configuration.Assemblies.AssemblyHashAlgorithm)n.HashAlgorithm,
                ContentType = (System.Reflection.AssemblyContentType)n.ContentType,
                VersionCompatibility = (System.Configuration.Assemblies.AssemblyVersionCompatibility)n.VersionCompatibility,
            };
        }

#pragma warning restore SYSLIB0037 // Type or member is obsolete
#pragma warning restore SYSLIB0017 // Type or member is obsolete

        /// <summary>
        /// Converts a set of <see cref="AssemblyName"/> to a set of <see cref="System.Reflection.AssemblyName"/>.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static System.Reflection.AssemblyName[] ToAssemblyNames(this AssemblyName[] n)
        {
            if (n.Length == 0)
                return [];

            var a = new System.Reflection.AssemblyName[n.Length];
            for (int i = 0; i < n.Length; i++)
                a[i] = n[i].ToAssemblyName();

            return a;
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

    }

}
