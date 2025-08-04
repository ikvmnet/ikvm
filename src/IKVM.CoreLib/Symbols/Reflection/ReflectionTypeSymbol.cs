using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

using IKVM.CoreLib.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionTypeSymbol : DefinitionTypeSymbol
    {

        readonly ReflectionSymbolContext _context;
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
        public ReflectionTypeSymbol(ReflectionSymbolContext context, Type underlyingType) :
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
        public sealed override TypeAttributes Attributes => _underlyingType.Attributes;

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
                        b.Add(new ReflectionGenericTypeParameterTypeSymbol(_context, i));

                    ImmutableInterlocked.InterlockedInitialize(ref _typeArguments, b.DrainToImmutable());
                }

                return _typeArguments;
            }
        }

        /// <inheritdoc />
        public sealed override GenericParameterAttributes GenericParameterAttributes => _underlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredNestedTypes()
        {
            if (_nestedTypes.IsDefault)
            {
                var l = _underlyingType.GetNestedTypes(Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new ReflectionTypeSymbol(_context, i));

                ImmutableInterlocked.InterlockedInitialize(ref _nestedTypes, b.DrainToImmutable());
            }

            return _nestedTypes;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
        {
            if (_interfaces.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _interfaces, _context.ResolveTypeSymbols(_underlyingType.GetDeclaredInterfaces()));

            return _interfaces;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            if (_fields.IsDefault)
            {
                var c = _underlyingType.GetFields(Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<FieldSymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new ReflectionFieldSymbol(_context, i));

#if NET7_0_OR_GREATER == false
                b.Sort((x, y) => Comparer<int>.Default.Compare(((ReflectionFieldSymbol)x).UnderlyingField.MetadataToken, ((ReflectionFieldSymbol)y).UnderlyingField.MetadataToken));
#endif

                ImmutableInterlocked.InterlockedInitialize(ref _fields, b.DrainToImmutable());
            }

            return _fields;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            if (_methods.IsDefault)
            {
                var c = _underlyingType.GetConstructors(Symbol.DeclaredOnlyLookup);
                var m = _underlyingType.GetMethods(Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<MethodSymbol>(c.Length + m.Length);
                foreach (var i in c)
                    b.Add(new ReflectionMethodSymbol(_context, i));
                foreach (var i in m)
                    b.Add(new ReflectionMethodSymbol(_context, i));

#if NET7_0_OR_GREATER == false
                b.Sort(static (x, y) => Comparer<int>.Default.Compare(((ReflectionMethodSymbol)x).UnderlyingMethod.MetadataToken, ((ReflectionMethodSymbol)y).UnderlyingMethod.MetadataToken));
#endif

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
                var c = _underlyingType.GetProperties(Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<PropertySymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new ReflectionPropertySymbol(_context, i));

#if NET7_0_OR_GREATER == false
                b.Sort(static (x, y) => Comparer<int>.Default.Compare(((ReflectionPropertySymbol)x).UnderlyingProperty.MetadataToken, ((ReflectionPropertySymbol)y).UnderlyingProperty.MetadataToken));
#endif

                ImmutableInterlocked.InterlockedInitialize(ref _properties, b.DrainToImmutable());
            }

            return _properties;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<EventSymbol> GetDeclaredEvents()
        {
            if (_events.IsDefault)
            {
                var c = _underlyingType.GetEvents(Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<EventSymbol>();
                foreach (var i in c)
                    b.Add(new ReflectionEventSymbol(_context, i));

#if NET7_0_OR_GREATER == false
                b.Sort(static (x, y) => Comparer<int>.Default.Compare(((ReflectionEventSymbol)x).UnderlyingEvent.MetadataToken, ((ReflectionEventSymbol)y).UnderlyingEvent.MetadataToken));
#endif

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
#if NET8_0_OR_GREATER
            if (_optionalCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, _context.ResolveTypeSymbols(_underlyingType.GetOptionalCustomModifiers()));

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
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, _context.ResolveTypeSymbols(_underlyingType.GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
#else
            return [];
#endif
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
                if (method is ReflectionMethodSymbol symbol)
                    if (symbol.UnderlyingMethod == type.DeclaringMethod)
                        return _genericMethodParameters.GetOrAdd(type, t => new ReflectionGenericMethodParameterTypeSymbol(_context, type));

            throw new InvalidOperationException();
        }

    }

}
