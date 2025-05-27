using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

using IKVM.CoreLib.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionTypeLoader : ITypeLoader
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
        public ReflectionTypeLoader(ReflectionSymbolContext context, Type underlyingType)
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
        public TypeAttributes GetAttributes() => _underlyingType.Attributes;

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
                    b.Add(new DefinitionGenericTypeParameterTypeSymbol(_context, new ReflectionGenericTypeParameterTypeLoader(_context, i)));

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
        public GenericParameterAttributes GetGenericParameterAttributes() => _underlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetNestedTypes()
        {
            if (_nestedTypes.IsDefault)
            {
                var l = _underlyingType.GetNestedTypes(Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new DefinitionTypeSymbol(_context, new ReflectionTypeLoader(_context, i)));

                ImmutableInterlocked.InterlockedInitialize(ref _nestedTypes, b.DrainToImmutable());
            }

            return _nestedTypes;
        }

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetInterfaces()
        {
            if (_interfaces.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _interfaces, _context.ResolveTypeSymbols(_underlyingType.GetDeclaredInterfaces()));

            return _interfaces;
        }

        /// <inheritdoc />
        public ImmutableArray<FieldSymbol> GetFields()
        {
            if (_fields.IsDefault)
            {
                var c = _underlyingType.GetFields(Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<FieldSymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new DefinitionFieldSymbol(_context, new ReflectionFieldLoader(_context, i)));

#if NET7_0_OR_GREATER == false
                b.Sort((x, y) => Comparer<int>.Default.Compare(((ReflectionFieldLoader)((DefinitionFieldSymbol)x).Loader).UnderlyingField.MetadataToken, ((ReflectionFieldLoader)((DefinitionFieldSymbol)y).Loader).UnderlyingField.MetadataToken));
#endif

                ImmutableInterlocked.InterlockedInitialize(ref _fields, b.DrainToImmutable());
            }

            return _fields;
        }

        /// <inheritdoc />
        public ImmutableArray<MethodSymbol> GetMethods()
        {
            if (_methods.IsDefault)
            {
                var c = _underlyingType.GetConstructors(Symbol.DeclaredOnlyLookup);
                var m = _underlyingType.GetMethods(Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<MethodSymbol>(c.Length + m.Length);
                foreach (var i in c)
                    b.Add(new DefinitionMethodSymbol(_context, new ReflectionMethodLoader(_context, i)));
                foreach (var i in m)
                    b.Add(new DefinitionMethodSymbol(_context, new ReflectionMethodLoader(_context, i)));

#if NET7_0_OR_GREATER == false
                b.Sort((x, y) => Comparer<int>.Default.Compare(((ReflectionMethodLoader)((DefinitionMethodSymbol)x).Loader).UnderlyingMethod.MetadataToken, ((ReflectionMethodLoader)((DefinitionMethodSymbol)y).Loader).UnderlyingMethod.MetadataToken));
#endif

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
                var c = _underlyingType.GetProperties(Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<PropertySymbol>(c.Length);
                foreach (var i in c)
                    b.Add(new DefinitionPropertySymbol(_context, new ReflectionPropertyLoader(_context, i)));

#if NET7_0_OR_GREATER == false
                b.Sort((x, y) => Comparer<int>.Default.Compare(((ReflectionPropertyLoader)((DefinitionPropertySymbol)x).Loader).UnderlyingProperty.MetadataToken, ((ReflectionPropertyLoader)((DefinitionPropertySymbol)y).Loader).UnderlyingProperty.MetadataToken));
#endif

                ImmutableInterlocked.InterlockedInitialize(ref _properties, b.DrainToImmutable());
            }

            return _properties;
        }

        /// <inheritdoc />
        public ImmutableArray<EventSymbol> GetEvents()
        {
            if (_events.IsDefault)
            {
                var c = _underlyingType.GetEvents(Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<EventSymbol>();
                foreach (var i in c)
                    b.Add(new DefinitionEventSymbol(_context, new ReflectionEventLoader(_context, i)));

#if NET7_0_OR_GREATER == false
                b.Sort((x, y) => Comparer<int>.Default.Compare(((ReflectionEventLoader)((DefinitionEventSymbol)x).Loader).UnderlyingEvent.MetadataToken, ((ReflectionEventLoader)((DefinitionEventSymbol)y).Loader).UnderlyingEvent.MetadataToken));
#endif

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
#if NET8_0_OR_GREATER
            if (_optionalCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, _context.ResolveTypeSymbols(_underlyingType.GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
#else
            return [];
#endif
        }

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
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
                if (method is DefinitionMethodSymbol symbol && symbol.Loader is ReflectionMethodLoader loader)
                    if (loader.UnderlyingMethod == type.DeclaringMethod)
                        return _genericMethodParameters.GetOrAdd(type, t => new DefinitionGenericMethodParameterTypeSymbol(_context, new ReflectionGenericMethodParameterTypeLoader(_context, type)));

            throw new InvalidOperationException();
        }

    }

}
