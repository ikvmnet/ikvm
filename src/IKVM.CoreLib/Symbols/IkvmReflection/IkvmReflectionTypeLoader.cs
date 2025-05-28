using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;

using IKVM.Reflection;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionTypeLoader : ITypeLoader
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
        public IkvmReflectionTypeLoader(IkvmReflectionSymbolContext context, Type underlyingType)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingType = underlyingType ?? throw new ArgumentNullException(nameof(underlyingType));
        }

        /// <summary>
        /// Gets the underlying type.
        /// </summary>
        public Type UnderlyingType => _underlyingType;

        /// <inheritdoc />
        public bool GetIsMissing() => false;

        /// <inheritdoc />
        public AssemblySymbol GetAssembly() => _assembly.IsDefault ? _assembly.InterlockedInitialize(_context.ResolveAssemblySymbol(_underlyingType.Module.Assembly)) : _assembly.Value;

        /// <inheritdoc />
        public ModuleSymbol GetModule() => _module.IsDefault ? _module.InterlockedInitialize(_context.ResolveModuleSymbol(_underlyingType.Module)) : _module.Value;

        /// <inheritdoc />
        public TypeSymbol GetDeclaringType() => _declaringType.IsDefault ? _declaringType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingType.DeclaringType)) : _declaringType.Value;

        /// <inheritdoc />
        public global::System.Reflection.TypeAttributes GetAttributes() => (global::System.Reflection.TypeAttributes)_underlyingType.Attributes;

        /// <inheritdoc />
        public string GetName() => _underlyingType.Name;

        /// <inheritdoc />
        public string? GetNamespace() => _underlyingType.Namespace;

        /// <inheritdoc />
        public TypeSymbol? GetBaseType() => _baseType.IsDefault ? _baseType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingType.BaseType)) : _baseType.Value;

        /// <inheritdoc />
        public string? GetEnumName(object value)
        {
            return _underlyingType.GetEnumName(value);
        }

        /// <inheritdoc />
        public ImmutableArray<string> GetEnumNames()
        {
            return _underlyingType.GetEnumNames().ToImmutableArray();
        }

        /// <inheritdoc />
        public TypeSymbol GetEnumUnderlyingType() => _enumUnderlyingType.IsDefault ? _enumUnderlyingType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingType.GetEnumUnderlyingType())) : _enumUnderlyingType.Value;

        /// <inheritdoc />
        public bool IsEnumDefined(object value) => _underlyingType.IsEnumDefined(value);

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetGenericArguments()
        {
            if (_typeArguments.IsDefault)
            {
                var c = _underlyingType.GetGenericArguments();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new DefinitionGenericTypeParameterTypeSymbol(_context, new IkvmReflectionGenericTypeParameterTypeLoader(_context, i)));

                ImmutableInterlocked.InterlockedInitialize(ref _typeArguments, b.DrainToImmutable());
            }

            return _typeArguments;
        }

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetGenericParameterConstraints()
        {
            if (_typeConstraints.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _typeArguments, _context.ResolveTypeSymbols(_underlyingType.GetGenericParameterConstraints()));

            return _typeConstraints;
        }

        /// <inheritdoc />
        public global::System.Reflection.GenericParameterAttributes GetGenericParameterAttributes() => (global::System.Reflection.GenericParameterAttributes)_underlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetNestedTypes()
        {
            if (_nestedTypes.IsDefault)
            {
                var l = _underlyingType.GetNestedTypes((BindingFlags)Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new DefinitionTypeSymbol(_context, new IkvmReflectionTypeLoader(_context, i)));

                ImmutableInterlocked.InterlockedInitialize(ref _nestedTypes, b.DrainToImmutable());
            }

            return _nestedTypes;
        }

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetInterfaces()
        {
            if (_interfaces.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _interfaces, _context.ResolveTypeSymbols(_underlyingType.__GetDeclaredInterfaces()));

            return _interfaces;
        }

        /// <inheritdoc />
        public ImmutableArray<FieldSymbol> GetFields()
        {
            if (_fields.IsDefault)
            {
                var c = _underlyingType.__GetDeclaredFields();
                var b = ImmutableArray.CreateBuilder<FieldSymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new DefinitionFieldSymbol(_context, new IkvmReflectionFieldLoader(_context, i)));

                b.Sort((x, y) => Comparer<int>.Default.Compare(((IkvmReflectionFieldLoader)((DefinitionFieldSymbol)x).Loader).UnderlyingField.MetadataToken, ((IkvmReflectionFieldLoader)((DefinitionFieldSymbol)y).Loader).UnderlyingField.MetadataToken));

                ImmutableInterlocked.InterlockedInitialize(ref _fields, b.DrainToImmutable());
            }

            return _fields;
        }

        /// <inheritdoc />
        public ImmutableArray<MethodSymbol> GetMethods()
        {
            if (_methods.IsDefault)
            {
                var m = _underlyingType.__GetDeclaredMethods();
                var b = ImmutableArray.CreateBuilder<MethodSymbol>(m.Length);
                foreach (var i in m)
                    b.Add(new DefinitionMethodSymbol(_context, new IkvmReflectionMethodLoader(_context, i)));

                b.Sort((x, y) => Comparer<int>.Default.Compare(((IkvmReflectionMethodLoader)((DefinitionMethodSymbol)x).Loader).UnderlyingMethod.MetadataToken, ((IkvmReflectionMethodLoader)((DefinitionMethodSymbol)y).Loader).UnderlyingMethod.MetadataToken));

                ImmutableInterlocked.InterlockedInitialize(ref _methods, b.DrainToImmutable());
            }

            return _methods;
        }

        /// <inheritdoc />
        public MethodImplementationMapping GetMethodImplementations()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ImmutableArray<PropertySymbol> GetProperties()
        {
            if (_properties.IsDefault)
            {
                var c = _underlyingType.__GetDeclaredProperties();
                var b = ImmutableArray.CreateBuilder<PropertySymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new DefinitionPropertySymbol(_context, new IkvmReflectionPropertyLoader(_context, i)));

                b.Sort((x, y) => Comparer<int>.Default.Compare(((IkvmReflectionPropertyLoader)((DefinitionPropertySymbol)x).Loader).UnderlyingProperty.MetadataToken, ((IkvmReflectionPropertyLoader)((DefinitionPropertySymbol)y).Loader).UnderlyingProperty.MetadataToken));

                ImmutableInterlocked.InterlockedInitialize(ref _properties, b.DrainToImmutable());
            }

            return _properties;
        }

        /// <inheritdoc />
        public ImmutableArray<EventSymbol> GetEvents()
        {
            if (_events.IsDefault)
            {
                var c = _underlyingType.__GetDeclaredEvents();
                var b = ImmutableArray.CreateBuilder<EventSymbol>();
                foreach (var i in c)
                    b.Add(new DefinitionEventSymbol(_context, new IkvmReflectionEventLoader(_context, i)));

                b.Sort((x, y) => Comparer<int>.Default.Compare(((IkvmReflectionEventLoader)((DefinitionEventSymbol)x).Loader).UnderlyingEvent.MetadataToken, ((IkvmReflectionEventLoader)((DefinitionEventSymbol)y).Loader).UnderlyingEvent.MetadataToken));

                ImmutableInterlocked.InterlockedInitialize(ref _events, b.DrainToImmutable());
            }

            return _events;
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttribute> GetCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingType.GetCustomAttributesData()));

            return _customAttributes;
        }

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, _context.ResolveTypeSymbols(_underlyingType.__GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
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
                if (method is DefinitionMethodSymbol symbol && symbol.Loader is IkvmReflectionMethodLoader loader)
                    if (loader.UnderlyingMethod == type.DeclaringMethod)
                        return _genericMethodParameters.GetOrAdd(type, t => new DefinitionGenericMethodParameterTypeSymbol(_context, new IkvmReflectionGenericMethodParameterTypeLoader(_context, type)));

            throw new InvalidOperationException();
        }

    }

}
