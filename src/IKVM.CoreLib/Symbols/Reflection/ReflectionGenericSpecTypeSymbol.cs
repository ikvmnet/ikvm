using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionGenericSpecTypeSymbol : ReflectionTypeSpecSymbol
    {

        /// <summary>
        /// Returns <c>true</c> if the type or any generic type definition or generic arguments are type builders. That is, is this a TypeBuilderInstantiation instance.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static bool ContainsTypeBuilder(Type type)
        {
            // find deepest element type
            while (type.HasElementType)
                type = type.GetElementType() ?? throw new InvalidOperationException();

            // type definition, or generic type definition, result is whether it is itself a type builder
            if (type.IsGenericType == false || type.IsGenericTypeDefinition)
                return type is TypeBuilder;

            // are arguments of closed generic themselves type builder instantiations?
            foreach (var arg in type.GetGenericArguments())
                if (ContainsTypeBuilder(arg))
                    return true;

            // are we a type builder?
            return type.GetGenericTypeDefinition() is TypeBuilder;
        }

        readonly IReflectionTypeSymbol[] _genericTypeArguments;
        Type? _underlyingType;
        bool? _underlyingTypeContainsTypeBuilder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="elementType"></param>
        /// <param name="genericTypeArguments"></param>
        public ReflectionGenericSpecTypeSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol elementType, IReflectionTypeSymbol[] genericTypeArguments) :
            base(context, resolvingModule, elementType)
        {
            _genericTypeArguments = genericTypeArguments ?? throw new ArgumentNullException(nameof(genericTypeArguments));
        }

        /// <inheritdoc />
        public override Type UnderlyingType => _underlyingType ??= ElementType.UnderlyingType.MakeGenericType(_genericTypeArguments.Select(i => i.UnderlyingType).ToArray());

        /// <inheritdoc />
        public override Type UnderlyingRuntimeType => ElementType.UnderlyingRuntimeType.MakeGenericType(_genericTypeArguments.Select(i => i.UnderlyingRuntimeType).ToArray());

        /// <summary>
        /// Returns <c>true</c> if the underlying type contains a type builder.
        /// </summary>
        /// <returns></returns>
        bool UnderlyingTypeContainsTypeBuilder => _underlyingTypeContainsTypeBuilder ??= ContainsTypeBuilder(UnderlyingType);

        /// <inheritdoc />
        public override IFieldSymbol[] GetFields()
        {
            if (UnderlyingTypeContainsTypeBuilder)
            {
                var l = ElementType.UnderlyingType.GetFields();
                var a = new IFieldSymbol[l.Length];
                for (int i = 0; i < l.Length; i++)
                    a[i] = ResolveFieldSymbol(TypeBuilder.GetField(UnderlyingType, l[i]));

                return a;
            }

            return base.GetFields();
        }

        /// <inheritdoc />
        public override IFieldSymbol[] GetFields(BindingFlags bindingAttr)
        {
            if (UnderlyingTypeContainsTypeBuilder)
            {
                var l = ElementType.UnderlyingType.GetFields(bindingAttr);
                var a = new IFieldSymbol[l.Length];
                for (int i = 0; i < l.Length; i++)
                    a[i] = ResolveFieldSymbol(TypeBuilder.GetField(UnderlyingType, l[i]));

                return a;
            }

            return base.GetFields(bindingAttr);
        }

        /// <inheritdoc />
        public override IFieldSymbol? GetField(string name)
        {
            if (UnderlyingTypeContainsTypeBuilder)
                if (ElementType.UnderlyingType.GetField(name) is { } f)
                    return ResolveFieldSymbol(TypeBuilder.GetField(UnderlyingType, f));

            return base.GetField(name);
        }

        /// <inheritdoc />
        public override IFieldSymbol? GetField(string name, BindingFlags bindingAttr)
        {
            if (UnderlyingTypeContainsTypeBuilder)
                if (ElementType.UnderlyingType.GetField(name, bindingAttr) is { } f)
                    return ResolveFieldSymbol(TypeBuilder.GetField(UnderlyingType, f));

            return base.GetField(name, bindingAttr);
        }

        /// <inheritdoc />
        public override IConstructorSymbol[] GetConstructors()
        {
            if (UnderlyingTypeContainsTypeBuilder)
            {
                var l = ElementType.UnderlyingType.GetConstructors();
                var a = new IConstructorSymbol[l.Length];
                for (int i = 0; i < l.Length; i++)
                    a[i] = ResolveConstructorSymbol(TypeBuilder.GetConstructor(UnderlyingType, l[i]));

                return a;
            }

            return base.GetConstructors();
        }

        /// <inheritdoc />
        public override IConstructorSymbol[] GetConstructors(BindingFlags bindingAttr)
        {
            if (UnderlyingTypeContainsTypeBuilder)
            {
                var l = ElementType.UnderlyingType.GetConstructors(bindingAttr);
                var a = new IConstructorSymbol[l.Length];
                for (int i = 0; i < l.Length; i++)
                    a[i] = ResolveConstructorSymbol(TypeBuilder.GetConstructor(UnderlyingType, l[i]));

                return a;
            }

            return base.GetConstructors(bindingAttr);
        }

        /// <inheritdoc />
        public override IConstructorSymbol? GetConstructor(ITypeSymbol[] types)
        {
            if (UnderlyingTypeContainsTypeBuilder)
                if (ElementType.UnderlyingType.GetConstructor(types.Unpack()) is { } m)
                    return ResolveConstructorSymbol(TypeBuilder.GetConstructor(UnderlyingType, m));

            return base.GetConstructor(types);
        }

        /// <inheritdoc />
        public override IConstructorSymbol? GetConstructor(BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            if (UnderlyingTypeContainsTypeBuilder)
                if (ElementType.UnderlyingType.GetConstructor(bindingAttr, null, types.Unpack(), null) is { } m)
                    return ResolveConstructorSymbol(TypeBuilder.GetConstructor(UnderlyingType, m));

            return base.GetConstructor(bindingAttr, types);
        }

        /// <inheritdoc />
        public override IMethodSymbol[] GetMethods()
        {
            if (UnderlyingTypeContainsTypeBuilder)
            {
                var l = ElementType.UnderlyingType.GetMethods();
                var a = new IMethodSymbol[l.Length];
                for (int i = 0; i < l.Length; i++)
                    a[i] = ResolveMethodSymbol(TypeBuilder.GetMethod(UnderlyingType, l[i]));

                return a;
            }

            return base.GetMethods();
        }

        /// <inheritdoc />
        public override IMethodSymbol[] GetMethods(BindingFlags bindingAttr)
        {
            if (UnderlyingTypeContainsTypeBuilder)
            {
                var l = ElementType.UnderlyingType.GetMethods(bindingAttr);
                var a = new IMethodSymbol[l.Length];
                for (int i = 0; i < l.Length; i++)
                    a[i] = ResolveMethodSymbol(TypeBuilder.GetMethod(UnderlyingType, l[i]));

                return a;
            }

            return base.GetMethods(bindingAttr);
        }

        /// <inheritdoc />
        public override IMethodSymbol? GetMethod(string name)
        {
            if (UnderlyingTypeContainsTypeBuilder)
                if (ElementType.UnderlyingType.GetMethod(name) is { } m)
                    return ResolveMethodSymbol(TypeBuilder.GetMethod(UnderlyingType, m));

            return base.GetMethod(name);
        }

        /// <inheritdoc />
        public override IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr)
        {
            if (UnderlyingTypeContainsTypeBuilder)
                if (ElementType.UnderlyingType.GetMethod(name, bindingAttr) is { } m)
                    return ResolveMethodSymbol(TypeBuilder.GetMethod(UnderlyingType, m));

            return base.GetMethod(name, bindingAttr);
        }

        /// <inheritdoc />
        public override IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
        {
            if (UnderlyingTypeContainsTypeBuilder)
                if (ElementType.UnderlyingType.GetMethod(name, types.Unpack()) is { } m)
                    return ResolveMethodSymbol(TypeBuilder.GetMethod(UnderlyingType, m));

            return base.GetMethod(name, types);
        }

        /// <inheritdoc />
        public override IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            if (UnderlyingTypeContainsTypeBuilder)
                if (ElementType.UnderlyingType.GetMethod(name, bindingAttr, null, types.Unpack(), null) is { } m)
                    return ResolveMethodSymbol(TypeBuilder.GetMethod(UnderlyingType, m));

            return base.GetMethod(name, bindingAttr, types);
        }

        /// <inheritdoc />
        public override IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            if (UnderlyingTypeContainsTypeBuilder)
                if (ElementType.UnderlyingType.GetMethod(name, bindingAttr, null, types.Unpack(), modifiers) is { } m)
                    return ResolveMethodSymbol(TypeBuilder.GetMethod(UnderlyingType, m));

            return base.GetMethod(name, bindingAttr, types, modifiers);
        }

        /// <inheritdoc />
        public override IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, CallingConventions callConvention, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            if (UnderlyingTypeContainsTypeBuilder)
                if (ElementType.UnderlyingType.GetMethod(name, bindingAttr, null, callConvention, types.Unpack(), modifiers) is { } m)
                    return ResolveMethodSymbol(TypeBuilder.GetMethod(UnderlyingType, m));

            return base.GetMethod(name, bindingAttr, callConvention, types, modifiers);
        }

        /// <inheritdoc />
        public override IMethodSymbol? GetMethod(string name, int genericParameterCount, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            if (UnderlyingTypeContainsTypeBuilder)
#if NETFRAMEWORK
                throw new NotSupportedException();
#else
                if (ElementType.UnderlyingType.GetMethod(name, genericParameterCount, types.Unpack(), modifiers) is { } m)
                    return ResolveMethodSymbol(TypeBuilder.GetMethod(UnderlyingType, m));
#endif

            return base.GetMethod(name, genericParameterCount, types, modifiers);
        }

        /// <inheritdoc />
        public override IMethodSymbol? GetMethod(string name, int genericParameterCount, BindingFlags bindingAttr, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            if (UnderlyingTypeContainsTypeBuilder)
#if NETFRAMEWORK
                throw new NotSupportedException();
#else
                if (ElementType.UnderlyingType.GetMethod(name, genericParameterCount, bindingAttr, null, types.Unpack(), modifiers) is { } m)
                    return ResolveMethodSymbol(TypeBuilder.GetMethod(UnderlyingType, m));
#endif

            return base.GetMethod(name, genericParameterCount, bindingAttr, types, modifiers);
        }

        /// <inheritdoc />
        public override IMethodSymbol? GetMethod(string name, int genericParameterCount, BindingFlags bindingAttr, CallingConventions callConvention, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            if (UnderlyingTypeContainsTypeBuilder)
#if NETFRAMEWORK
                throw new NotSupportedException();
#else
                if (ElementType.UnderlyingType.GetMethod(name, genericParameterCount, bindingAttr, null, callConvention, types.Unpack(), modifiers) is { } m)
                    return ResolveMethodSymbol(TypeBuilder.GetMethod(UnderlyingType, m));
#endif

            return base.GetMethod(name, genericParameterCount, bindingAttr, callConvention, types, modifiers);
        }

    }

}
