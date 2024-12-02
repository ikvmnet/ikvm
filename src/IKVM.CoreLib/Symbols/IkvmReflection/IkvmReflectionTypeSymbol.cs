
using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionTypeSymbol : DefinitionTypeSymbol
    {

        internal readonly Type _underlyingType;
        readonly ConcurrentDictionary<Type, IkvmReflectionGenericMethodParameterTypeSymbol> _genericMethodParamters = new();

        ImmutableArray<TypeSymbol> _typeArguments;
        ImmutableArray<TypeSymbol> _typeConstraints;
        ImmutableArray<TypeSymbol> _interfaces;
        ImmutableArray<MethodSymbol> _methods;
        ImmutableArray<FieldSymbol> _fields;
        ImmutableArray<PropertySymbol> _properties;
        ImmutableArray<EventSymbol> _events;
        ImmutableArray<TypeSymbol> _nestedTypes;
        ImmutableArray<CustomAttribute> _customAttributes;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        MethodImplementationMapping _methodImpl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="underlyingType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionTypeSymbol(IkvmReflectionSymbolContext context, ModuleSymbol module, Type underlyingType) :
            base(context, module)
        {
            _underlyingType = underlyingType ?? throw new ArgumentNullException(nameof(underlyingType));
        }

        /// <summary>
        /// Gets the <see cref="IkvmReflectionGenericMethodParameterTypeSymbol"/> for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal IkvmReflectionGenericMethodParameterTypeSymbol GetOrCreateGenericMethodParameter(Type type)
        {
            IkvmReflectionMethodSymbol FindMethod()
            {
                var methods = GetDeclaredMethods();
                foreach (var method in methods)
                    if (method is IkvmReflectionMethodSymbol m)
                        if (m._underlyingMethod == type.DeclaringMethod)
                            return m;

                throw new InvalidOperationException();
            }

            var method = FindMethod();
            return _genericMethodParamters.GetOrAdd(type, t => new IkvmReflectionGenericMethodParameterTypeSymbol(Context, method, t));
        }

        /// <summary>
        /// Gets the context that owns this type.
        /// </summary>
        new IkvmReflectionSymbolContext Context => (IkvmReflectionSymbolContext)base.Context;

        /// <summary>
        /// Gets the original underlying source type.
        /// </summary>
        internal Type UnderlyingType => _underlyingType;

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => Context.ResolveTypeSymbol(_underlyingType.DeclaringType);

        /// <inheritdoc />
        public sealed override System.Reflection.TypeAttributes Attributes => (System.Reflection.TypeAttributes)_underlyingType.Attributes;

        /// <inheritdoc />
        public sealed override string Name => _underlyingType.Name;

        /// <inheritdoc />
        public sealed override string? Namespace => _underlyingType.Namespace;

        /// <inheritdoc />
        public sealed override TypeCode TypeCode => Type.GetTypeCode(_underlyingType);

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => Context.ResolveTypeSymbol(_underlyingType.BaseType);

        /// <inheritdoc />
        public sealed override bool ContainsGenericParameters => _underlyingType.ContainsGenericParameters;

        /// <inheritdoc />
        public sealed override System.Reflection.GenericParameterAttributes GenericParameterAttributes => (System.Reflection.GenericParameterAttributes)_underlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        public sealed override bool IsPrimitive => _underlyingType.IsPrimitive;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeDefinition => _underlyingType.IsGenericTypeDefinition;

        /// <inheritdoc />
        public sealed override bool IsEnum => _underlyingType.IsEnum;

        /// <inheritdoc />
        public sealed override bool IsMissing => _underlyingType.__IsMissing;

        /// <inheritdoc />
        public sealed override string? GetEnumName(object value)
        {
            return _underlyingType.GetEnumName(value);
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<string> GetEnumNames()
        {
            return _underlyingType.GetEnumNames().ToImmutableArray();
        }

        /// <inheritdoc />
        public sealed override TypeSymbol GetEnumUnderlyingType()
        {
            return Context.ResolveTypeSymbol(_underlyingType.GetEnumUnderlyingType());
        }

        /// <inheritdoc />
        public sealed override bool IsEnumDefined(object value)
        {
            return _underlyingType.IsEnumDefined(value);
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericArguments => ComputeGenericArguments();

        ImmutableArray<TypeSymbol> ComputeGenericArguments()
        {
            if (_typeArguments.IsDefault)
            {
                var c = _underlyingType.GetGenericArguments();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new IkvmReflectionGenericTypeParameterTypeSymbol(Context, this, i));

                ImmutableInterlocked.InterlockedInitialize(ref _typeArguments, b.DrainToImmutable());
            }

            return _typeArguments;
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GenericParameterConstraints => ComputeGenericParameterConstraints();

        ImmutableArray<TypeSymbol> ComputeGenericParameterConstraints()
        {
            if (_typeConstraints.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _typeConstraints, Context.ResolveTypeSymbols(_underlyingType.GetGenericParameterConstraints()));

            return _typeConstraints;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredNestedTypes()
        {
            if (_nestedTypes.IsDefault)
            {
                var l = _underlyingType.GetNestedTypes((IKVM.Reflection.BindingFlags)DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new IkvmReflectionTypeSymbol(Context, Module, i));

                ImmutableInterlocked.InterlockedInitialize(ref _nestedTypes, b.DrainToImmutable());
            }

            return _nestedTypes;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
        {
            if (_interfaces.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _interfaces, Context.ResolveTypeSymbols(_underlyingType.__GetDeclaredInterfaces()));

            return _interfaces;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            if (_fields.IsDefault)
            {
                var c = _underlyingType.__GetDeclaredFields();
                var b = ImmutableArray.CreateBuilder<FieldSymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new IkvmReflectionFieldSymbol(Context, Module, this, i));

                ImmutableInterlocked.InterlockedInitialize(ref _fields, b.DrainToImmutable());
            }

            return _fields;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            if (_methods.IsDefault)
            {
                var c = _underlyingType.__GetDeclaredMethods();
                var b = ImmutableArray.CreateBuilder<MethodSymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new IkvmReflectionMethodSymbol(Context, (IkvmReflectionModuleSymbol)Module, this, i));

                ImmutableInterlocked.InterlockedInitialize(ref _methods, b.DrainToImmutable());
            }

            return _methods;
        }

        /// <inheritdoc />
        internal override MethodImplementationMapping GetMethodImplementations()
        {
            if (_methodImpl.Type == null)
            {
                var m = _underlyingType.__GetMethodImplMap();
                var impl = ImmutableArray.CreateBuilder<MethodSymbol>(m.MethodBodies.Length);
                var decl = ImmutableArray.CreateBuilder<ImmutableArray<MethodSymbol>>(m.MethodBodies.Length);
                for (int i = 0; i < m.MethodBodies.Length; i++)
                {
                    impl.Add(Context.ResolveMethodSymbol(m.MethodBodies[i]));
                    decl.Add(Context.ResolveMethodSymbols(m.MethodDeclarations[i]));
                }

                lock (this)
                    if (_methodImpl.Type == null)
                        _methodImpl = new MethodImplementationMapping(this, impl.DrainToImmutable(), decl.DrainToImmutable());
            }

            lock (this)
                return _methodImpl;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<PropertySymbol> GetDeclaredProperties()
        {
            if (_properties.IsDefault)
            {
                var c = _underlyingType.__GetDeclaredProperties();
                var b = ImmutableArray.CreateBuilder<PropertySymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new IkvmReflectionPropertySymbol(Context, this, i));

                ImmutableInterlocked.InterlockedInitialize(ref _properties, b.DrainToImmutable());
            }

            return _properties;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<EventSymbol> GetDeclaredEvents()
        {
            if (_events.IsDefault)
            {
                var c = _underlyingType.__GetDeclaredEvents();
                var b = ImmutableArray.CreateBuilder<EventSymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new IkvmReflectionEventSymbol(Context, this, i));

                ImmutableInterlocked.InterlockedInitialize(ref _events, b.DrainToImmutable());
            }

            return _events;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingType.GetCustomAttributesData()));

            return _customAttributes;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, Context.ResolveTypeSymbols(_underlyingType.__GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, Context.ResolveTypeSymbols(_underlyingType.__GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
        }

    }

}
