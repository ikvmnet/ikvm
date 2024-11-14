using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionTypeSymbol : DefinitionTypeSymbol
    {

        readonly Type _underlyingType;

        ImmutableArray<TypeSymbol> _typeArguments;
        ImmutableArray<TypeSymbol> _typeConstraints;
        ImmutableArray<TypeSymbol> _interfaces;
        ImmutableArray<ConstructorSymbol> _constructors;
        ImmutableArray<MethodSymbol> _methods;
        ImmutableArray<FieldSymbol> _fields;
        ImmutableArray<PropertySymbol> _properties;
        ImmutableArray<EventSymbol> _events;
        ImmutableArray<TypeSymbol> _nestedTypes;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="underlyingType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionTypeSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol module, Type underlyingType) :
            base(context, module)
        {
            _underlyingType = underlyingType ?? throw new ArgumentNullException(nameof(underlyingType));
        }

        /// <summary>
        /// Gets the context that owns this type.
        /// </summary>
        public new ReflectionSymbolContext Context => (ReflectionSymbolContext)base.Context;

        /// <inheritdoc />
        public override TypeAttributes Attributes => _underlyingType.Attributes;

        /// <inheritdoc />
        public override string? FullName => _underlyingType.FullName;

        /// <inheritdoc />
        public override string? Namespace => _underlyingType.Namespace;

        /// <inheritdoc />
        public override TypeCode TypeCode => Type.GetTypeCode(_underlyingType);

        /// <inheritdoc />
        public override TypeSymbol? BaseType => Context.ResolveTypeSymbol(_underlyingType.BaseType);

        /// <inheritdoc />
        public override bool ContainsGenericParameters => _underlyingType.ContainsGenericParameters;

        /// <inheritdoc />
        public override GenericParameterAttributes GenericParameterAttributes => _underlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        public override bool IsPrimitive => _underlyingType.IsPrimitive;

        /// <inheritdoc />
        public override bool IsGenericTypeDefinition => _underlyingType.IsGenericTypeDefinition;

        /// <inheritdoc />
        public override bool IsEnum => _underlyingType.IsEnum;

        /// <inheritdoc />
        public override string Name => _underlyingType.Name;

        /// <inheritdoc />
        public override string? GetEnumName(object value)
        {
            return _underlyingType.GetEnumName(value);
        }

        /// <inheritdoc />
        public override ImmutableArray<string> GetEnumNames()
        {
            return _underlyingType.GetEnumNames().ToImmutableArray();
        }

        /// <inheritdoc />
        public override TypeSymbol GetEnumUnderlyingType()
        {
            return Context.ResolveTypeSymbol(_underlyingType.GetEnumUnderlyingType());
        }

        /// <inheritdoc />
        public override bool IsEnumDefined(object value)
        {
            return _underlyingType.IsEnumDefined(value);
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetGenericArguments()
        {
            if (_typeArguments == default)
            {
                var c = _underlyingType.GetGenericArguments();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new ReflectionGenericTypeParameterTypeSymbol(Context, this, i));

                ImmutableInterlocked.InterlockedInitialize(ref _typeArguments, b.ToImmutable());
            }

            return _typeArguments;
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetGenericParameterConstraints()
        {
            if (_typeConstraints == default)
                ImmutableInterlocked.InterlockedInitialize(ref _typeArguments, Context.ResolveTypeSymbols(_underlyingType.GetGenericParameterConstraints()));

            return _typeConstraints;
        }

        /// <inheritdoc />
        internal override ImmutableArray<TypeSymbol> GetDeclaredNestedTypes()
        {
            if (_nestedTypes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _nestedTypes, Context.ResolveTypeSymbols(_underlyingType.GetNestedTypes(DeclaredOnlyLookup)));

            return _nestedTypes;
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetInterfaces()
        {
            if (_interfaces == default)
                ImmutableInterlocked.InterlockedInitialize(ref _interfaces, Context.ResolveTypeSymbols(_underlyingType.GetInterfaces()));

            return _interfaces;
        }

        /// <inheritdoc />
        public override InterfaceMapping GetInterfaceMap(TypeSymbol interfaceType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            if (_fields == default)
            {
                var c = _underlyingType.GetFields(DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<FieldSymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new ReflectionFieldSymbol(Context, Module, this, i));

                ImmutableInterlocked.InterlockedInitialize(ref _fields, b.ToImmutable());
            }

            return _fields;
        }

        /// <inheritdoc />
        internal override ImmutableArray<ConstructorSymbol> GetDeclaredConstructors()
        {
            if (_constructors == default)
            {
                var c = _underlyingType.GetConstructors(DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<ConstructorSymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new ReflectionConstructorSymbol(Context, this, i));

                ImmutableInterlocked.InterlockedInitialize(ref _constructors, b.ToImmutable());
            }

            return _constructors;
        }

        /// <inheritdoc />
        internal override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            if (_methods == default)
            {
                var c = _underlyingType.GetMethods(DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<MethodSymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new ReflectionMethodSymbol(Context, (ReflectionModuleSymbol)Module, this, i));

                ImmutableInterlocked.InterlockedInitialize(ref _methods, b.ToImmutable());
            }

            return _methods;
        }

        /// <inheritdoc />
        internal override ImmutableArray<PropertySymbol> GetDeclaredProperties()
        {
            if (_properties == default)
            {
                var c = _underlyingType.GetProperties(DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<PropertySymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new ReflectionPropertySymbol(Context, this, i));

                ImmutableInterlocked.InterlockedInitialize(ref _properties, b.ToImmutable());
            }

            return _properties;
        }

        /// <inheritdoc />
        internal override ImmutableArray<EventSymbol> GetDeclaredEvents()
        {
            if (_events == default)
            {
                var c = _underlyingType.GetEvents(DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<EventSymbol>();
                foreach (var i in c)
                    b.Add(new ReflectionEventSymbol(Context, this, i));

                ImmutableInterlocked.InterlockedInitialize(ref _events, b.ToImmutable());
            }

            return _events;
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingType.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
