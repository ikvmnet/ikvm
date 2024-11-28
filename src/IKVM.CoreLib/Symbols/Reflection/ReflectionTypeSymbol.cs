using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.IkvmReflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionTypeSymbol : DefinitionTypeSymbol
    {

        internal readonly Type _underlyingType;
        readonly ConcurrentDictionary<Type, ReflectionGenericMethodParameterTypeSymbol> _genericMethodParamters = new();

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

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="underlyingType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionTypeSymbol(ReflectionSymbolContext context, ModuleSymbol module, Type underlyingType) :
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
        internal ReflectionGenericMethodParameterTypeSymbol GetOrCreateGenericMethodParameter(Type type)
        {
            ReflectionMethodSymbol FindMethod()
            {
                var methods = GetDeclaredMethods();
                foreach (var method in methods)
                    if (method is ReflectionMethodSymbol m)
                        if (m._underlyingMethod == type.DeclaringMethod)
                            return m;

                throw new InvalidOperationException();
            }

            var method = FindMethod();
            return _genericMethodParamters.GetOrAdd(type, t => new ReflectionGenericMethodParameterTypeSymbol(Context, method, t));
        }

        /// <summary>
        /// Gets the context that owns this type.
        /// </summary>
        new ReflectionSymbolContext Context => (ReflectionSymbolContext)base.Context;

        /// <inheritdoc />
        public sealed override string Name => _underlyingType.Name;

        /// <inheritdoc />
        public sealed override string? Namespace => _underlyingType.Namespace;

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => Context.ResolveTypeSymbol(_underlyingType.DeclaringType);

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => _underlyingType.Attributes;

        /// <inheritdoc />
        public sealed override TypeCode TypeCode => Type.GetTypeCode(_underlyingType);

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => Context.ResolveTypeSymbol(_underlyingType.BaseType);

        /// <inheritdoc />
        public sealed override bool ContainsGenericParameters => _underlyingType.ContainsGenericParameters;

        /// <inheritdoc />
        public sealed override GenericParameterAttributes GenericParameterAttributes => _underlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        public sealed override bool IsPrimitive => _underlyingType.IsPrimitive;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeDefinition => _underlyingType.IsGenericTypeDefinition;

        /// <inheritdoc />
        public sealed override bool IsEnum => _underlyingType.IsEnum;

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
        public override ImmutableArray<TypeSymbol> GenericArguments => ComputeGenericArguments();

        ImmutableArray<TypeSymbol> ComputeGenericArguments()
        {
            if (_typeArguments.IsDefault)
            {
                var c = _underlyingType.GetGenericArguments();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new ReflectionGenericTypeParameterTypeSymbol(Context, this, i));

                ImmutableInterlocked.InterlockedInitialize(ref _typeArguments, b.DrainToImmutable());
            }

            return _typeArguments;
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GenericParameterConstraints => ComputeGenericParameterConstraints();

        ImmutableArray<TypeSymbol> ComputeGenericParameterConstraints()
        {
            if (_typeConstraints.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _typeArguments, Context.ResolveTypeSymbols(_underlyingType.GetGenericParameterConstraints()));

            return _typeConstraints;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredNestedTypes()
        {
            if (_nestedTypes.IsDefault)
            {
                var l = _underlyingType.GetNestedTypes(DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new ReflectionTypeSymbol(Context, Module, i));

                ImmutableInterlocked.InterlockedInitialize(ref _nestedTypes, b.DrainToImmutable());
            }

            return _nestedTypes;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
        {
            if (_interfaces.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _interfaces, Context.ResolveTypeSymbols(_underlyingType.GetDeclaredInterfaces()));

            return _interfaces;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            if (_fields.IsDefault)
            {
                var c = _underlyingType.GetFields(DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<ReflectionFieldSymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new ReflectionFieldSymbol(Context, Module, this, i));

#if NET7_0_OR_GREATER == false
                b.Sort((x, y) => Comparer<int>.Default.Compare(x.UnderlyingField.MetadataToken, y.UnderlyingField.MetadataToken));
#endif

                ImmutableInterlocked.InterlockedInitialize(ref _fields, b.ToImmutable().CastArray<FieldSymbol>());
            }

            return _fields;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            if (_methods.IsDefault)
            {
                var c = _underlyingType.GetConstructors(DeclaredOnlyLookup);
                var m = _underlyingType.GetMethods(DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<ReflectionMethodSymbol>(m.Length);
                foreach (var i in c)
                    b.Add(new ReflectionMethodSymbol(Context, (ReflectionModuleSymbol)Module, this, i));
                foreach (var i in m)
                    b.Add(new ReflectionMethodSymbol(Context, (ReflectionModuleSymbol)Module, this, i));

#if NET7_0_OR_GREATER == false
                b.Sort((x, y) => Comparer<int>.Default.Compare(x.UnderlyingMethod.MetadataToken, y.UnderlyingMethod.MetadataToken));
#endif

                ImmutableInterlocked.InterlockedInitialize(ref _methods, b.ToImmutable().CastArray<MethodSymbol>());
            }

            return _methods;
        }

        /// <inheritdoc />
        internal override MethodImplementationMapping GetMethodImplementations()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<PropertySymbol> GetDeclaredProperties()
        {
            if (_properties.IsDefault)
            {
                var c = _underlyingType.GetProperties(DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<ReflectionPropertySymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new ReflectionPropertySymbol(Context, this, i));

#if NET7_0_OR_GREATER == false
                b.Sort((x, y) => Comparer<int>.Default.Compare(x.UnderlyingProperty.MetadataToken, y.UnderlyingProperty.MetadataToken));
#endif

                ImmutableInterlocked.InterlockedInitialize(ref _properties, b.ToImmutable().CastArray<PropertySymbol>());
            }

            return _properties;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<EventSymbol> GetDeclaredEvents()
        {
            if (_events.IsDefault)
            {
                var c = _underlyingType.GetEvents(DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<ReflectionEventSymbol>();
                foreach (var i in c)
                    b.Add(new ReflectionEventSymbol(Context, this, i));

#if NET7_0_OR_GREATER == false
                b.Sort((x, y) => Comparer<int>.Default.Compare(x.UnderlyingEvent.MetadataToken, y.UnderlyingEvent.MetadataToken));
#endif

                ImmutableInterlocked.InterlockedInitialize(ref _events, b.ToImmutable().CastArray<EventSymbol>());
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
#if NET8_0_OR_GREATER
            if (_optionalCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, Context.ResolveTypeSymbols(_underlyingType.GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
#else
            return [];
#endif
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
#if NET8_0_OR_GREATER
            if (_requiredCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, Context.ResolveTypeSymbols(_underlyingType.GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
#else
            return [];
#endif
        }

    }

}
