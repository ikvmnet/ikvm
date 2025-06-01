using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;

using IKVM.Reflection;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionTypeSymbol : DefinitionTypeSymbol
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly Type _underlyingType;
        readonly ConcurrentDictionary<Type, TypeSymbol> _genericMethodParameters = new();

        LazyField<AssemblySymbol> _assembly;
        LazyField<ModuleSymbol> _module;
        LazyField<TypeSymbol?> _declaringType;
        LazyField<TypeSymbol?> _baseType;
        LazyField<TypeSymbol> _enumUnderlyingType;
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
        /// <param name="underlyingType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionTypeSymbol(IkvmReflectionSymbolContext context, Type underlyingType) :
            base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingType = underlyingType ?? throw new ArgumentNullException(nameof(underlyingType));
        }

        /// <summary>
        /// Gets the underlying type.
        /// </summary>
        public Type UnderlyingType => _underlyingType;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override ModuleSymbol Module => _module.IsDefault ? _module.InterlockedInitialize(_context.ResolveModuleSymbol(_underlyingType.Module)) : _module.Value;

        /// <inheritdoc />
        public sealed override TypeSymbol DeclaringType => _declaringType.IsDefault ? _declaringType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingType.DeclaringType)) : _declaringType.Value;

        /// <inheritdoc />
        public sealed override global::System.Reflection.TypeAttributes Attributes => (global::System.Reflection.TypeAttributes)_underlyingType.Attributes;

        /// <inheritdoc />
        public sealed override string Name => _underlyingType.Name;

        /// <inheritdoc />
        public sealed override string? Namespace => _underlyingType.Namespace;

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => _baseType.IsDefault ? _baseType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingType.BaseType)) : _baseType.Value;

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericParameters
        {
            get
            {
                if (_typeArguments.IsDefault)
                {
                    var c = _underlyingType.GetGenericArguments();
                    var b = ImmutableArray.CreateBuilder<TypeSymbol>(c.Length);
                    foreach (var i in c)
                        b.Add(new IkvmReflectionGenericTypeParameterTypeSymbol(_context, i));

                    ImmutableInterlocked.InterlockedInitialize(ref _typeArguments, b.DrainToImmutable());
                }

                return _typeArguments;
            }
        }

        /// <inheritdoc />
        public sealed override global::System.Reflection.GenericParameterAttributes GenericParameterAttributes => (global::System.Reflection.GenericParameterAttributes)_underlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredNestedTypes()
        {
            if (_nestedTypes.IsDefault)
            {
                var l = _underlyingType.GetNestedTypes((BindingFlags)Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new IkvmReflectionTypeSymbol(_context, i));

                ImmutableInterlocked.InterlockedInitialize(ref _nestedTypes, b.DrainToImmutable());
            }

            return _nestedTypes;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
        {
            if (_interfaces.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _interfaces, _context.ResolveTypeSymbols(_underlyingType.__GetDeclaredInterfaces()));

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
                    b.Add(new IkvmReflectionFieldSymbol(_context, i));

                b.Sort(static (x, y) => Comparer<int>.Default.Compare(((IkvmReflectionFieldSymbol)x).UnderlyingField.MetadataToken, ((IkvmReflectionFieldSymbol)y).UnderlyingField.MetadataToken));

                ImmutableInterlocked.InterlockedInitialize(ref _fields, b.DrainToImmutable());
            }

            return _fields;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            if (_methods.IsDefault)
            {
                var m = _underlyingType.__GetDeclaredMethods();
                var b = ImmutableArray.CreateBuilder<MethodSymbol>(m.Length);
                foreach (var i in m)
                    b.Add(new IkvmReflectionMethodSymbol(_context, i));

                b.Sort(static (x, y) => Comparer<int>.Default.Compare(((IkvmReflectionMethodSymbol)x).UnderlyingMethod.MetadataToken, ((IkvmReflectionMethodSymbol)y).UnderlyingMethod.MetadataToken));

                ImmutableInterlocked.InterlockedInitialize(ref _methods, b.DrainToImmutable());
            }

            return _methods;
        }

        /// <inheritdoc />
        internal sealed override MethodImplementationMapping GetMethodImplementations()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<PropertySymbol> GetDeclaredProperties()
        {
            if (_properties.IsDefault)
            {
                var c = _underlyingType.__GetDeclaredProperties();
                var b = ImmutableArray.CreateBuilder<PropertySymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new IkvmReflectionPropertySymbol(_context, i));

                b.Sort(static (x, y) => Comparer<int>.Default.Compare(((IkvmReflectionPropertySymbol)x).UnderlyingProperty.MetadataToken, ((IkvmReflectionPropertySymbol)y).UnderlyingProperty.MetadataToken));

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
                var b = ImmutableArray.CreateBuilder<EventSymbol>();
                foreach (var i in c)
                    b.Add(new IkvmReflectionEventSymbol(_context, i));

                b.Sort(static (x, y) => Comparer<int>.Default.Compare(((IkvmReflectionEventSymbol)x).UnderlyingEvent.MetadataToken, ((IkvmReflectionEventSymbol)y).UnderlyingEvent.MetadataToken));

                ImmutableInterlocked.InterlockedInitialize(ref _events, b.DrainToImmutable());
            }

            return _events;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingType.GetCustomAttributesData()));

            return _customAttributes;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, _context.ResolveTypeSymbols(_underlyingType.__GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, _context.ResolveTypeSymbols(_underlyingType.__GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
        }

        /// <summary>
        /// Gets the <see cref="TypeSymbol"/> for the specified generic method parameter within this type.
        /// </summary>
        /// <remarks>
        /// This method exists to avoid resolving the method itself when resolving type parameters of the method. This
        /// causes recursion.
        /// </remarks>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal TypeSymbol GetOrCreateGenericMethodParameter(Type type)
        {
            foreach (var method in GetMethods())
                if (method is IkvmReflectionMethodSymbol symbol)
                    if (symbol.UnderlyingMethod == type.DeclaringMethod)
                        return _genericMethodParameters.GetOrAdd(type, t => new IkvmReflectionGenericMethodParameterTypeSymbol(_context, type));

            throw new InvalidOperationException();
        }

    }

}
