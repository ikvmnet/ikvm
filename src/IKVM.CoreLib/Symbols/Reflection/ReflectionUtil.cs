using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.CoreLib.Symbols.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection
{

    static class ReflectionUtil
    {

        /// <summary>
        /// Unpacks the symbol into their original type.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static Module Unpack(this IModuleSymbol module)
        {
            if (module is IReflectionModuleSymbol symbol)
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
            if (type is IReflectionTypeSymbol symbol)
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
            if (member is IReflectionMemberSymbol symbol)
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
            if (ctor is IReflectionConstructorSymbol symbol)
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
            if (ctor is IReflectionMethodSymbol symbol)
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
            if (field is IReflectionFieldSymbol symbol)
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
            if (property is IReflectionPropertySymbol symbol)
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
        /// Unpacks the <see cref="ILocalBuilder"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static LocalBuilder Unpack(this ILocalBuilder builder)
        {
            return ((ReflectionLocalBuilder)builder).UnderlyingLocalBuilder;
        }

        /// <summary>
        /// Unpacks the <see cref="ILabel"/>.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static Label Unpack(this ILabel label)
        {
            return ((ReflectionLabel)label).UnderlyingLabel;
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
        /// Unpacks the <see cref="ICustomAttributeBuilder"/>.
        /// </summary>
        /// <param name="customAttributes"></param>
        /// <returns></returns>
        public static CustomAttributeBuilder Unpack(this ICustomAttributeBuilder customAttributes)
        {
            return ((ReflectionCustomAttributeBuilder)customAttributes).UnderlyingBuilder;
        }

        /// <summary>
        /// Unpacks the <see cref="ICustomAttributeBuilder"/>s.
        /// </summary>
        /// <param name="customAttributes"></param>
        /// <returns></returns>
        public static CustomAttributeBuilder[] Unpack(this ICustomAttributeBuilder[] customAttributes)
        {
            if (customAttributes.Length == 0)
                return [];

            var a = new CustomAttributeBuilder[customAttributes.Length];
            for (int i = 0; i < customAttributes.Length; i++)
                a[i] = customAttributes[i].Unpack();

            return a;
        }

    }

}
