using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    /// <summary>
    /// Provides common operations against a <see cref="IReflectionTypeSymbol"/>.
    /// </summary>
    struct ReflectionTypeImpl
    {

        readonly IReflectionTypeSymbol _symbol;

        ReflectionTypeSymbol?[]? _asArray;
        ReflectionTypeSymbol? _asSZArray;
        ReflectionTypeSymbol? _asPointer;
        ReflectionTypeSymbol? _asByRef;
        ConcurrentDictionary<ITypeSymbol[], ReflectionTypeSymbol>? _genericTypeSymbols;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbol"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionTypeImpl(IReflectionTypeSymbol symbol)
        {
            _symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionTypeSymbol"/> cached for the generic parameter type.
        /// </summary>
        /// <param name="genericTypeArguments"></param>
        /// <returns></returns>
        public IReflectionTypeSymbol GetOrCreateGenericTypeSymbol(ITypeSymbol[] genericTypeArguments)
        {
            if (genericTypeArguments is null)
                throw new ArgumentNullException(nameof(genericTypeArguments));

            if (_symbol.UnderlyingType.IsGenericTypeDefinition == false)
                throw new InvalidOperationException();

            if (_genericTypeSymbols == null)
                Interlocked.CompareExchange(ref _genericTypeSymbols, new ConcurrentDictionary<ITypeSymbol[], ReflectionTypeSymbol>(TypeSymbolListEqualityComparer.Instance), null);

            var __symbol = _symbol;
            return _genericTypeSymbols.GetOrAdd(genericTypeArguments, _ => new ReflectionTypeSymbol(__symbol.Context, __symbol.ResolvingModule, __symbol.UnderlyingType.MakeGenericType(genericTypeArguments.Unpack())));
        }

        /// <inheritdoc />
        public readonly IAssemblySymbol Assembly => _symbol.ResolveAssemblySymbol(_symbol.UnderlyingType.Assembly);

        /// <inheritdoc />
        public readonly string? AssemblyQualifiedName => _symbol.UnderlyingType.AssemblyQualifiedName;

        /// <inheritdoc />
        public readonly TypeAttributes Attributes => _symbol.UnderlyingType.Attributes;

        /// <inheritdoc />
        public readonly ITypeSymbol? BaseType => _symbol.ResolveTypeSymbol(_symbol.UnderlyingType.BaseType);

        /// <inheritdoc />
        public readonly bool ContainsGenericParameters => _symbol.UnderlyingType.ContainsGenericParameters;

        /// <inheritdoc />
        public readonly IMethodBaseSymbol? DeclaringMethod => _symbol.ResolveMethodBaseSymbol(_symbol.UnderlyingType.DeclaringMethod);

        /// <inheritdoc />
        public readonly string? FullName => _symbol.UnderlyingType.FullName;

        /// <inheritdoc />
        public readonly string? Namespace => _symbol.UnderlyingType.Namespace;

        /// <inheritdoc />
        public readonly GenericParameterAttributes GenericParameterAttributes => _symbol.UnderlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        public readonly int GenericParameterPosition => _symbol.UnderlyingType.GenericParameterPosition;

        /// <inheritdoc />
        public readonly ITypeSymbol[] GenericTypeArguments => _symbol.ResolveTypeSymbols(_symbol.UnderlyingType.GenericTypeArguments);

        /// <inheritdoc />
        public readonly bool HasElementType => _symbol.UnderlyingType.HasElementType;

        /// <inheritdoc />
        public readonly TypeCode TypeCode => Type.GetTypeCode(_symbol.UnderlyingType);

        /// <inheritdoc />
        public readonly bool IsAbstract => _symbol.UnderlyingType.IsAbstract;

#if NETFRAMEWORK

        /// <inheritdoc />
        /// <remarks>
        /// There's no API to distinguish an array of rank 1 from a vector,
        /// so we check if the type name ends in [], which indicates it's a vector
        /// (non-vectors will have [*] or [,]).
        /// </remarks>
        public readonly bool IsSZArray => _symbol.UnderlyingType.IsArray && _symbol.UnderlyingType.Name.EndsWith("[]");

#else

        /// <inheritdoc />
        public readonly bool IsSZArray => _symbol.UnderlyingType.IsSZArray;

#endif

        /// <inheritdoc />
        public readonly bool IsArray => _symbol.UnderlyingType.IsArray;

        /// <inheritdoc />
        public readonly bool IsAutoLayout => _symbol.UnderlyingType.IsAutoLayout;

        /// <inheritdoc />
        public readonly bool IsExplicitLayout => _symbol.UnderlyingType.IsExplicitLayout;

        /// <inheritdoc />
        public readonly bool IsByRef => _symbol.UnderlyingType.IsByRef;

        /// <inheritdoc />
        public readonly bool IsClass => _symbol.UnderlyingType.IsClass;

        /// <inheritdoc />
        public readonly bool IsEnum => _symbol.UnderlyingType.IsEnum;

        /// <inheritdoc />
        public readonly bool IsInterface => _symbol.UnderlyingType.IsInterface;

        /// <inheritdoc />
        public readonly bool IsConstructedGenericType => _symbol.UnderlyingType.IsConstructedGenericType;

        /// <inheritdoc />
        public readonly bool IsGenericParameter => _symbol.UnderlyingType.IsGenericParameter;

        /// <inheritdoc />
        public readonly bool IsGenericType => _symbol.UnderlyingType.IsGenericType;

        /// <inheritdoc />
        public readonly bool IsGenericTypeDefinition => _symbol.UnderlyingType.IsGenericTypeDefinition;

        /// <inheritdoc />
        public readonly bool IsLayoutSequential => _symbol.UnderlyingType.IsLayoutSequential;

        /// <inheritdoc />
        public readonly bool IsNested => _symbol.UnderlyingType.IsNested;

        /// <inheritdoc />
        public readonly bool IsNestedAssembly => _symbol.UnderlyingType.IsNestedAssembly;

        /// <inheritdoc />
        public readonly bool IsNestedFamANDAssem => _symbol.UnderlyingType.IsNestedFamANDAssem;

        /// <inheritdoc />
        public readonly bool IsNestedFamORAssem => _symbol.UnderlyingType.IsNestedFamORAssem;

        /// <inheritdoc />
        public readonly bool IsNestedFamily => _symbol.UnderlyingType.IsNestedFamily;

        /// <inheritdoc />
        public readonly bool IsNestedPrivate => _symbol.UnderlyingType.IsNestedPrivate;

        /// <inheritdoc />
        public readonly bool IsNestedPublic => _symbol.UnderlyingType.IsNestedPublic;

        /// <inheritdoc />
        public readonly bool IsNotPublic => _symbol.UnderlyingType.IsNotPublic;

        /// <inheritdoc />
        public readonly bool IsPointer => _symbol.UnderlyingType.IsPointer;

#if NET8_0_OR_GREATER

        /// <inheritdoc />
        public readonly bool IsFunctionPointer => _symbol.UnderlyingType.IsFunctionPointer;

        /// <inheritdoc />
        public readonly bool IsUnmanagedFunctionPointer => _symbol.UnderlyingType.IsUnmanagedFunctionPointer;

#else

        /// <inheritdoc />
        public readonly bool IsFunctionPointer => throw new NotImplementedException();

        /// <inheritdoc />
        public readonly bool IsUnmanagedFunctionPointer => throw new NotImplementedException();

#endif

        /// <inheritdoc />
        public readonly bool IsPrimitive => _symbol.UnderlyingType.IsPrimitive;

        /// <inheritdoc />
        public readonly bool IsPublic => _symbol.UnderlyingType.IsPublic;

        /// <inheritdoc />
        public readonly bool IsSealed => _symbol.UnderlyingType.IsSealed;

#pragma warning disable SYSLIB0050 // Type or member is obsolete
        /// <inheritdoc />
        public readonly bool IsSerializable => _symbol.UnderlyingType.IsSerializable;
#pragma warning restore SYSLIB0050 // Type or member is obsolete

        /// <inheritdoc />
        public readonly bool IsValueType => _symbol.UnderlyingType.IsValueType;

        /// <inheritdoc />
        public readonly bool IsVisible => _symbol.UnderlyingType.IsVisible;

#if NET6_0_OR_GREATER

        /// <inheritdoc />
        public readonly bool IsSignatureType => _symbol.UnderlyingType.IsSignatureType;

#else

        /// <inheritdoc />
        public readonly bool IsSignatureType => throw new NotImplementedException();

#endif

        /// <inheritdoc />
        public readonly bool IsSpecialName => _symbol.UnderlyingType.IsSpecialName;

        /// <inheritdoc />
        public readonly IConstructorSymbol? TypeInitializer => _symbol.ResolveConstructorSymbol(_symbol.UnderlyingType.TypeInitializer);

        /// <inheritdoc />
        public readonly int GetArrayRank()
        {
            return _symbol.UnderlyingType.GetArrayRank();
        }

        /// <inheritdoc />
        public readonly IConstructorSymbol? GetConstructor(BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return _symbol.ResolveConstructorSymbol(_symbol.UnderlyingType.GetConstructor(bindingAttr, binder: null, types.Unpack(), modifiers: null));
        }

        /// <inheritdoc />
        public readonly IConstructorSymbol? GetConstructor(ITypeSymbol[] types)
        {
            return _symbol.ResolveConstructorSymbol(_symbol.UnderlyingType.GetConstructor(types.Unpack()));
        }

        /// <inheritdoc />
        public readonly IConstructorSymbol[] GetConstructors()
        {
            return _symbol.ResolveConstructorSymbols(_symbol.UnderlyingType.GetConstructors());
        }

        /// <inheritdoc />
        public readonly IConstructorSymbol[] GetConstructors(BindingFlags bindingAttr)
        {
            return _symbol.ResolveConstructorSymbols(_symbol.UnderlyingType.GetConstructors(bindingAttr));
        }

        /// <inheritdoc />
        public readonly IMemberSymbol[] GetDefaultMembers()
        {
            return _symbol.ResolveMemberSymbols(_symbol.UnderlyingType.GetDefaultMembers());
        }

        /// <inheritdoc />
        public readonly ITypeSymbol? GetElementType()
        {
            return _symbol.ResolveTypeSymbol(_symbol.UnderlyingType.GetElementType());
        }

        /// <inheritdoc />
        public readonly string? GetEnumName(object value)
        {
            return _symbol.UnderlyingType.GetEnumName(value);
        }

        /// <inheritdoc />
        public readonly string[] GetEnumNames()
        {
            return _symbol.UnderlyingType.GetEnumNames();
        }

        /// <inheritdoc />
        public readonly ITypeSymbol GetEnumUnderlyingType()
        {
            return _symbol.ResolveTypeSymbol(_symbol.UnderlyingType.GetEnumUnderlyingType());
        }

        /// <inheritdoc />
        public readonly Array GetEnumValues()
        {
            return _symbol.UnderlyingType.GetEnumValues();
        }

        /// <inheritdoc />
        public readonly IEventSymbol? GetEvent(string name)
        {
            return _symbol.ResolveEventSymbol(_symbol.UnderlyingType.GetEvent(name));
        }

        /// <inheritdoc />
        public readonly IEventSymbol? GetEvent(string name, BindingFlags bindingAttr)
        {
            return _symbol.ResolveEventSymbol(_symbol.UnderlyingType.GetEvent(name, bindingAttr));
        }

        /// <inheritdoc />
        public readonly IEventSymbol[] GetEvents()
        {
            return _symbol.ResolveEventSymbols(_symbol.UnderlyingType.GetEvents());
        }

        /// <inheritdoc />
        public readonly IEventSymbol[] GetEvents(BindingFlags bindingAttr)
        {
            return _symbol.ResolveEventSymbols(_symbol.UnderlyingType.GetEvents(bindingAttr));
        }

        /// <inheritdoc />
        public readonly IFieldSymbol? GetField(string name)
        {
            return _symbol.ResolveFieldSymbol(_symbol.UnderlyingType.GetField(name));
        }

        /// <inheritdoc />
        public readonly IFieldSymbol? GetField(string name, BindingFlags bindingAttr)
        {
            return _symbol.ResolveFieldSymbol(_symbol.UnderlyingType.GetField(name, bindingAttr));
        }

        /// <inheritdoc />
        public readonly IFieldSymbol[] GetFields()
        {
            return _symbol.ResolveFieldSymbols(_symbol.UnderlyingType.GetFields());
        }

        /// <inheritdoc />
        public readonly IFieldSymbol[] GetFields(BindingFlags bindingAttr)
        {
            return _symbol.ResolveFieldSymbols(_symbol.UnderlyingType.GetFields(bindingAttr));
        }

        /// <inheritdoc />
        public readonly ITypeSymbol[] GetGenericArguments()
        {
            return _symbol.ResolveTypeSymbols(_symbol.UnderlyingType.GetGenericArguments());
        }

        /// <inheritdoc />
        public readonly ITypeSymbol[] GetGenericParameterConstraints()
        {
            return _symbol.ResolveTypeSymbols(_symbol.UnderlyingType.GetGenericParameterConstraints());
        }

        /// <inheritdoc />
        public readonly ITypeSymbol GetGenericTypeDefinition()
        {
            return _symbol.ResolveTypeSymbol(_symbol.UnderlyingType.GetGenericTypeDefinition());
        }

        /// <inheritdoc />
        public readonly ITypeSymbol? GetInterface(string name)
        {
            return _symbol.ResolveTypeSymbol(_symbol.UnderlyingType.GetInterface(name));
        }

        /// <inheritdoc />
        public readonly ITypeSymbol? GetInterface(string name, bool ignoreCase)
        {
            return _symbol.ResolveTypeSymbol(_symbol.UnderlyingType.GetInterface(name, ignoreCase));
        }

        /// <inheritdoc />
        public readonly InterfaceMapping GetInterfaceMap(ITypeSymbol interfaceType)
        {
            return _symbol.ResolveInterfaceMapping(_symbol.UnderlyingType.GetInterfaceMap(interfaceType.Unpack()));
        }

        /// <inheritdoc />
        public readonly ITypeSymbol[] GetInterfaces(bool inherit = true)
        {
            if (inherit)
                return _symbol.ResolveTypeSymbols(_symbol.UnderlyingType.GetInterfaces());
            else
                throw new NotImplementedException();
        }

        /// <inheritdoc />
        public readonly IMemberSymbol[] GetMember(string name)
        {
            return _symbol.ResolveMemberSymbols(_symbol.UnderlyingType.GetMember(name));
        }

        /// <inheritdoc />
        public readonly IMemberSymbol[] GetMember(string name, BindingFlags bindingAttr)
        {
            return _symbol.ResolveMemberSymbols(_symbol.UnderlyingType.GetMember(name, bindingAttr));
        }

        /// <inheritdoc />
        public readonly IMemberSymbol[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
        {
            return _symbol.ResolveMemberSymbols(_symbol.UnderlyingType.GetMember(name, type, bindingAttr));
        }

        /// <inheritdoc />
        public readonly IMemberSymbol[] GetMembers(BindingFlags bindingAttr)
        {
            return _symbol.ResolveMemberSymbols(_symbol.UnderlyingType.GetMembers(bindingAttr));
        }

        /// <inheritdoc />
        public readonly IMemberSymbol[] GetMembers()
        {
            return _symbol.ResolveMemberSymbols(_symbol.UnderlyingType.GetMembers());
        }

        /// <inheritdoc />
        public readonly IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr)
        {
            return _symbol.ResolveMethodSymbol(_symbol.UnderlyingType.GetMethod(name, bindingAttr));
        }

        /// <inheritdoc />
        public readonly IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
        {
            return _symbol.ResolveMethodSymbol(_symbol.UnderlyingType.GetMethod(name, types.Unpack()));
        }

        /// <inheritdoc />
        public readonly IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return _symbol.ResolveMethodSymbol(_symbol.UnderlyingType.GetMethod(name, bindingAttr, null, types.Unpack(), null));
        }

        /// <inheritdoc />
        public readonly IMethodSymbol? GetMethod(string name)
        {
            return _symbol.ResolveMethodSymbol(_symbol.UnderlyingType.GetMethod(name));
        }

        /// <inheritdoc />
        public readonly IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, CallingConventions callConvention, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            return _symbol.ResolveMethodSymbol(_symbol.UnderlyingType.GetMethod(name, bindingAttr, null, callConvention, types.Unpack(), modifiers));
        }

        /// <inheritdoc />
        public readonly IMethodSymbol? GetMethod(string name, int genericParameterCount, BindingFlags bindingAttr, CallingConventions callConvention, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
#if NETFRAMEWORK
            throw new NotImplementedException();
#else
            return _symbol.ResolveMethodSymbol(_symbol.UnderlyingType.GetMethod(name, genericParameterCount, bindingAttr, null, callConvention, types.Unpack(), modifiers));
#endif
        }

        /// <inheritdoc />
        public readonly IMethodSymbol? GetMethod(string name, int genericParameterCount, BindingFlags bindingAttr, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
#if NETFRAMEWORK
            throw new NotImplementedException();
#else
            return _symbol.ResolveMethodSymbol(_symbol.UnderlyingType.GetMethod(name, genericParameterCount, bindingAttr, null, types.Unpack(), modifiers));
#endif
        }

        /// <inheritdoc />
        public readonly IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            return _symbol.ResolveMethodSymbol(_symbol.UnderlyingType.GetMethod(name, bindingAttr, null, types.Unpack(), modifiers));
        }

        /// <inheritdoc />
        public readonly IMethodSymbol? GetMethod(string name, int genericParameterCount, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
#if NETFRAMEWORK
            throw new NotImplementedException();
#else
            return _symbol.ResolveMethodSymbol(_symbol.UnderlyingType.GetMethod(name, genericParameterCount, types.Unpack(), modifiers));
#endif
        }

        /// <inheritdoc />
        public readonly IMethodSymbol[] GetMethods(BindingFlags bindingAttr)
        {
            return _symbol.ResolveMethodSymbols(_symbol.UnderlyingType.GetMethods(bindingAttr));
        }

        /// <inheritdoc />
        public readonly IMethodSymbol[] GetMethods()
        {
            return _symbol.ResolveMethodSymbols(_symbol.UnderlyingType.GetMethods());
        }

        /// <inheritdoc />
        public readonly ITypeSymbol? GetNestedType(string name)
        {
            return _symbol.ResolveTypeSymbol(_symbol.UnderlyingType.GetNestedType(name));
        }

        /// <inheritdoc />
        public readonly ITypeSymbol? GetNestedType(string name, BindingFlags bindingAttr)
        {
            return _symbol.ResolveTypeSymbol(_symbol.UnderlyingType.GetNestedType(name, bindingAttr));
        }

        /// <inheritdoc />
        public readonly ITypeSymbol[] GetNestedTypes()
        {
            return _symbol.ResolveTypeSymbols(_symbol.UnderlyingType.GetNestedTypes());
        }

        /// <inheritdoc />
        public readonly ITypeSymbol[] GetNestedTypes(BindingFlags bindingAttr)
        {
            return _symbol.ResolveTypeSymbols(_symbol.UnderlyingType.GetNestedTypes(bindingAttr));
        }

        /// <inheritdoc />
        public readonly IPropertySymbol[] GetProperties()
        {
            return _symbol.ResolvePropertySymbols(_symbol.UnderlyingType.GetProperties());
        }

        /// <inheritdoc />
        public readonly IPropertySymbol[] GetProperties(BindingFlags bindingAttr)
        {
            return _symbol.ResolvePropertySymbols(_symbol.UnderlyingType.GetProperties(bindingAttr));
        }

        /// <inheritdoc />
        public readonly IPropertySymbol? GetProperty(string name, ITypeSymbol[] types)
        {
            return _symbol.ResolvePropertySymbol(_symbol.UnderlyingType.GetProperty(name, types.Unpack()));
        }

        /// <inheritdoc />
        public readonly IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types)
        {
            return _symbol.ResolvePropertySymbol(_symbol.UnderlyingType.GetProperty(name, returnType?.Unpack(), types.Unpack()));
        }

        /// <inheritdoc />
        public readonly IPropertySymbol? GetProperty(string name, BindingFlags bindingAttr)
        {
            return _symbol.ResolvePropertySymbol(_symbol.UnderlyingType.GetProperty(name, bindingAttr));
        }

        /// <inheritdoc />
        public readonly IPropertySymbol? GetProperty(string name)
        {
            return _symbol.ResolvePropertySymbol(_symbol.UnderlyingType.GetProperty(name));
        }

        /// <inheritdoc />
        public readonly IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType)
        {
            return _symbol.ResolvePropertySymbol(_symbol.UnderlyingType.GetProperty(name, returnType?.Unpack()));
        }

        /// <inheritdoc />
        public readonly bool IsAssignableFrom(ITypeSymbol? c)
        {
            return _symbol.UnderlyingType.IsAssignableFrom(c?.Unpack());
        }

        /// <inheritdoc />
        public readonly bool IsEnumDefined(object value)
        {
            return _symbol.UnderlyingType.IsEnumDefined(value);
        }

        /// <inheritdoc />
        public readonly bool IsSubclassOf(ITypeSymbol c)
        {
            return _symbol.UnderlyingType.IsSubclassOf(c.Unpack());
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol MakeArrayType()
        {
            if (_asSZArray == null)
                Interlocked.CompareExchange(ref _asSZArray, new ReflectionTypeSymbol(_symbol.Context, _symbol.ResolvingModule, _symbol.UnderlyingType.MakeArrayType()), null);

            return _asSZArray;
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol MakeArrayType(int rank)
        {
            if (rank == 1)
                return MakeArrayType();

            if (_asArray == null)
                Interlocked.CompareExchange(ref _asArray, new ReflectionTypeSymbol?[32], null);

            ref var asArray = ref _asArray[rank];
            if (asArray == null)
                Interlocked.CompareExchange(ref asArray, new ReflectionTypeSymbol(_symbol.Context, _symbol.ResolvingModule, _symbol.UnderlyingType.MakeArrayType(rank)), null);

            return asArray;
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol MakeByRefType()
        {
            if (_asByRef == null)
                Interlocked.CompareExchange(ref _asByRef, new ReflectionTypeSymbol(_symbol.Context, _symbol.ResolvingModule, _symbol.UnderlyingType.MakeByRefType()), null);

            return _asByRef;
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments)
        {
            return GetOrCreateGenericTypeSymbol(typeArguments);
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol MakePointerType()
        {
            if (_asPointer == null)
                Interlocked.CompareExchange(ref _asPointer, new ReflectionTypeSymbol(_symbol.Context, _symbol.ResolvingModule, _symbol.UnderlyingType.MakePointerType()), null);

            return _asPointer;
        }

    }

}
