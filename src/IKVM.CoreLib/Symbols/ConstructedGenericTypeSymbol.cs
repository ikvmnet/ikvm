using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a type that is a constructed generic type.
    /// </summary>
    class ConstructedGenericTypeSymbol : TypeSymbol
    {

        readonly TypeSymbol _typeDefinition;
        readonly GenericContext _genericContext;

        ImmutableArray<TypeSymbol> _typeArguments;
        ImmutableArray<TypeSymbol> _interfaces;
        ImmutableArray<FieldSymbol> _fields;
        ImmutableArray<MethodSymbol> _methods;
        ImmutableArray<PropertySymbol> _properties;
        ImmutableArray<EventSymbol> _events;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        MethodImplementationMapping _methodImpl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="typeDefinition"></param>
        /// <param name="genericContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstructedGenericTypeSymbol(SymbolContext context, TypeSymbol typeDefinition, GenericContext genericContext) :
            base(context, typeDefinition.Module)
        {
            _typeDefinition = typeDefinition ?? throw new ArgumentNullException(nameof(typeDefinition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => _typeDefinition.DeclaringType;

        /// <inheritdoc />
        public sealed override string Name => _typeDefinition.Name;

        /// <inheritdoc />
        public sealed override string? Namespace => _typeDefinition.Namespace;

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => _typeDefinition.Attributes;

        /// <inheritdoc />
        public sealed override TypeCode TypeCode => _typeDefinition.TypeCode;

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => _typeDefinition.BaseType?.Specialize(_genericContext);

        /// <inheritdoc />
        public sealed override bool IsTypeDefinition => false;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeDefinition => false;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericType => true;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeParameter => false;

        /// <inheritdoc />
        public sealed override bool IsGenericMethodParameter => false;

        /// <inheritdoc />
        public sealed override bool ContainsGenericParameters => GenericArguments.Any(i => i.ContainsGenericParameters);

        /// <inheritdoc />
        public sealed override int GenericParameterPosition => throw new NotSupportedException();

        /// <inheritdoc />
        public sealed override bool HasElementType => false;

        /// <inheritdoc />
        public sealed override bool IsArray => false;

        /// <inheritdoc />
        public sealed override bool IsSZArray => false;

        /// <inheritdoc />
        public sealed override bool IsByRef => false;

        /// <inheritdoc />
        public sealed override bool IsPointer => false;

        /// <inheritdoc />
        public sealed override bool IsFunctionPointer => false;

        /// <inheritdoc />
        public sealed override bool IsUnmanagedFunctionPointer => false;

        /// <inheritdoc />
        public sealed override MethodSymbol? DeclaringMethod => _typeDefinition.DeclaringMethod;

        /// <inheritdoc />
        public sealed override GenericParameterAttributes GenericParameterAttributes => _typeDefinition.GenericParameterAttributes;

        /// <inheritdoc />
        public sealed override bool IsPrimitive => _typeDefinition.IsPrimitive;

        /// <inheritdoc />
        public sealed override bool IsVisible => base.IsVisible && _typeArguments.All(i => i.IsVisible);

        /// <inheritdoc />
        public sealed override bool IsEnum => _typeDefinition.IsEnum;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool ContainsMissingType => _typeDefinition.ContainsMissingType || GenericArguments.Any(i => i.ContainsMissingType);

        /// <inheritdoc />
        public sealed override TypeSymbol GetEnumUnderlyingType()
        {
            return _typeDefinition.GetEnumUnderlyingType().Specialize(_genericContext);
        }

        /// <inheritdoc />
        public sealed override TypeSymbol? GetElementType()
        {
            return null;
        }

        /// <inheritdoc />
        public sealed override int GetArrayRank()
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public override TypeSymbol GenericTypeDefinition => _typeDefinition;

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GenericArguments => GetGenericArguments();

        ImmutableArray<TypeSymbol> GetGenericArguments()
        {
            if (_typeArguments.IsDefault)
            {
                var l = _typeDefinition.GenericArguments;
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(l[i].Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _typeArguments, b.DrainToImmutable());
            }

            return _typeArguments;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericParameterConstraints => throw new InvalidOperationException();

        /// <inheritdoc />
        internal override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            if (_fields.IsDefault)
            {
                var l = _typeDefinition.GetDeclaredFields();
                var b = ImmutableArray.CreateBuilder<FieldSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new ConstructedGenericFieldSymbol(Context, this, i, _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _fields, b.DrainToImmutable());
            }

            return _fields;
        }

        /// <inheritdoc />
        internal override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            if (_methods.IsDefault)
            {
                var l = _typeDefinition.GetDeclaredMethods();
                var b = ImmutableArray.CreateBuilder<MethodSymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(new ConstructedGenericMethodSymbol(Context, Module, this, l[i], _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _methods, b.DrainToImmutable());
            }

            return _methods;
        }

        /// <inheritdoc />
        internal override MethodImplementationMapping GetMethodImplementations()
        {
            if (_methodImpl.Type == null)
            {
                var m = _typeDefinition.GetMethodImplementations();
                var impl = ImmutableArray.CreateBuilder<MethodSymbol>(m.Implementations.Length);
                var decl = ImmutableArray.CreateBuilder<ImmutableArray<MethodSymbol>>(m.Implementations.Length);
                for (int i = 0; i < m.Implementations.Length; i++)
                {
                    impl.Add(Specialize(m.Implementations[i]) ?? throw new InvalidOperationException());

                    var declBuilder = ImmutableArray.CreateBuilder<MethodSymbol>(m.Declarations[i].Length);
                    for (int j = 0; j < m.Declarations[i].Length; j++)
                        declBuilder.Add(Specialize(m.Declarations[i][j]) ?? throw new InvalidOperationException());

                    decl.Add(declBuilder.DrainToImmutable());
                }

                lock (this)
                    if (_methodImpl.Type == null)
                        _methodImpl = new MethodImplementationMapping(this, impl.DrainToImmutable(), decl.DrainToImmutable());
            }

            return _methodImpl;
        }

        /// <summary>
        /// Searches this type for the constructed method that is constructed from the given definition method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        MethodSymbol Specialize(MethodSymbol method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            if (method.DeclaringType is not { } definitionType)
                throw new InvalidOperationException();

            // find method on specialized type
            return definitionType.Specialize(_genericContext).FindMethod(method.Name, method.Signature.Specialize(_genericContext)) ?? throw new InvalidOperationException();
        }

        /// <inheritdoc />
        internal override ImmutableArray<PropertySymbol> GetDeclaredProperties()
        {
            if (_properties.IsDefault)
            {
                var l = _typeDefinition.GetDeclaredProperties();
                var b = ImmutableArray.CreateBuilder<PropertySymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(new ConstructedGenericPropertySymbol(Context, this, l[i], _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _properties, b.DrainToImmutable());
            }

            return _properties;
        }

        /// <inheritdoc />
        internal override ImmutableArray<EventSymbol> GetDeclaredEvents()
        {
            if (_events == default)
            {
                var l = _typeDefinition.GetDeclaredEvents();
                var b = ImmutableArray.CreateBuilder<EventSymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(new ConstructedGenericEventSymbol(Context, this, l[i], _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _events, b.DrainToImmutable());
            }

            return _events;
        }

        /// <inheritdoc />
        internal override ImmutableArray<TypeSymbol> GetDeclaredNestedTypes()
        {
            return ImmutableArray<TypeSymbol>.Empty;
        }

        /// <inheritdoc />
        public sealed override string? GetEnumName(object value)
        {
            return _typeDefinition.GetEnumName(value);
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<string> GetEnumNames()
        {
            return _typeDefinition.GetEnumNames();
        }

        /// <inheritdoc />
        internal override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
        {
            if (_interfaces == default)
            {
                var l = _typeDefinition.GetDeclaredInterfaces();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(l[i].Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _interfaces, b.DrainToImmutable());
            }

            return _interfaces;
        }

        /// <inheritdoc />
        public sealed override bool IsEnumDefined(object value)
        {
            return _typeDefinition.IsEnumDefined(value);
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _typeDefinition.GetDeclaredCustomAttributes();
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers == default)
            {
                var l = _typeDefinition.GetOptionalCustomModifiers();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(i.Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, b.DrainToImmutable());
            }

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers == default)
            {
                var l = _typeDefinition.GetRequiredCustomModifiers();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(i.Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, b.DrainToImmutable());
            }

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal override TypeSymbol Specialize(GenericContext context)
        {
            if (ContainsGenericParameters == false)
                return this;

            var args = GetGenericArguments();
            for (int i = 0; i < args.Length; i++)
                if (args[i].ContainsGenericParameters)
                    args = args.SetItem(i, args[i].Specialize(context));

            return _typeDefinition.MakeGenericType(args);
        }

    }

}
