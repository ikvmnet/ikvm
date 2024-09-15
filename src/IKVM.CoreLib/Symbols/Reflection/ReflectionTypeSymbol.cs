using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionTypeSymbol : ReflectionMemberSymbol, ITypeSymbol
    {

        const int MAX_CAPACITY = 65536 * 2;

        const BindingFlags DefaultBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        Type _type;

        ReflectionTypeSymbol?[]? _asArray;
        ReflectionTypeSymbol? _asSZArray;
        ReflectionTypeSymbol? _asPointer;
        ReflectionTypeSymbol? _asByRef;

        Type[]? _genericParameterList;
        ReflectionTypeSymbol?[]? _genericParameterSymbols;

        List<(Type[] Arguments, ReflectionTypeSymbol Symbol)>? _genericTypeSymbols;
        ReaderWriterLockSlim? _genericTypeLock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        public ReflectionTypeSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol module, Type type) :
            base(context, module, type)
        {
            Debug.Assert(module.ReflectionObject == type.Module);
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionTypeSymbol"/> cached for the generic parameter type.
        /// </summary>
        /// <param name="genericTypeParameterType"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal ReflectionTypeSymbol GetOrCreateGenericParameterSymbol(Type genericTypeParameterType)
        {
            if (genericTypeParameterType is null)
                throw new ArgumentNullException(nameof(genericTypeParameterType));

            Debug.Assert(genericTypeParameterType.DeclaringType == _type);

            // initialize tables
            if (_genericParameterList == null)
                Interlocked.CompareExchange(ref _genericParameterList, _type.GetGenericArguments(), null);
            if (_genericParameterSymbols == null)
                Interlocked.CompareExchange(ref _genericParameterSymbols, new ReflectionTypeSymbol?[_genericParameterList.Length], null);

            // position of the parameter in the list
            var idx = genericTypeParameterType.GenericParameterPosition;

            // check that our list is long enough to contain the entire table
            if (_genericParameterSymbols.Length < idx)
                throw new IndexOutOfRangeException();

            // if not yet created, create, allow multiple instances, but only one is eventually inserted
            ref var rec = ref _genericParameterSymbols[idx];
            if (rec == null)
                Interlocked.CompareExchange(ref rec, new ReflectionTypeSymbol(Context, ContainingModule, genericTypeParameterType), null);

            // this should never happen
            if (rec is not ReflectionTypeSymbol sym)
                throw new InvalidOperationException();

            return sym;
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionTypeSymbol"/> cached for the generic parameter type.
        /// </summary>
        /// <param name="genericTypeArguments"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal ReflectionTypeSymbol GetOrCreateGenericTypeSymbol(Type[] genericTypeArguments)
        {
            if (genericTypeArguments is null)
                throw new ArgumentNullException(nameof(genericTypeArguments));

            if (_type.IsGenericTypeDefinition == false)
                throw new InvalidOperationException();

            // initialize the available parameters
            if (_genericParameterList == null)
                Interlocked.CompareExchange(ref _genericParameterList, _type.GetGenericArguments(), null);
            if (_genericParameterList.Length != genericTypeArguments.Length)
                throw new InvalidOperationException();

            // initialize generic type map, and lock on it since we're potentially adding items
            if (_genericTypeSymbols == null)
                Interlocked.CompareExchange(ref _genericTypeSymbols, [], null);
            if (_genericTypeLock == null)
                Interlocked.CompareExchange(ref _genericTypeLock, new(), null);

            try
            {
                _genericTypeLock.EnterUpgradeableReadLock();

                // find existing entry
                foreach (var i in _genericTypeSymbols)
                    if (i.Arguments.SequenceEqual(genericTypeArguments))
                        return i.Symbol;

                try
                {
                    _genericTypeLock.EnterWriteLock();

                    // generate new symbol
                    var sym = new ReflectionTypeSymbol(Context, ContainingModule, _type.MakeGenericType(genericTypeArguments));
                    _genericTypeSymbols.Add((genericTypeArguments, sym));
                    return sym;
                }
                finally
                {
                    _genericTypeLock.ExitWriteLock();
                }
            }
            finally
            {
                _genericTypeLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Resolves the symbol for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected internal override ReflectionTypeSymbol ResolveTypeSymbol(Type type)
        {
            if (type == _type)
                return this;
            else
                return base.ResolveTypeSymbol(type);
        }

        /// <summary>
        /// Gets the wrapped <see cref="Type"/>.
        /// </summary>
        internal new Type ReflectionObject => _type;

        /// <inheritdoc />
        public IAssemblySymbol Assembly => Context.GetOrCreateAssemblySymbol(_type.Assembly);

        /// <inheritdoc />
        public string? AssemblyQualifiedName => _type.AssemblyQualifiedName;

        /// <inheritdoc />
        public TypeAttributes Attributes => _type.Attributes;

        /// <inheritdoc />
        public ITypeSymbol? BaseType => _type.BaseType != null ? ResolveTypeSymbol(_type.BaseType) : null;

        /// <inheritdoc />
        public bool ContainsGenericParameters => _type.ContainsGenericParameters;

        /// <inheritdoc />
        public IMethodBaseSymbol? DeclaringMethod => _type.DeclaringMethod is MethodInfo m ? ResolveMethodSymbol(m) : null;

        /// <inheritdoc />
        public string? FullName => _type.FullName;

        /// <inheritdoc />
        public string? Namespace => _type.Namespace;

        /// <inheritdoc />
        public GenericParameterAttributes GenericParameterAttributes => _type.GenericParameterAttributes;

        /// <inheritdoc />
        public int GenericParameterPosition => _type.GenericParameterPosition;

        /// <inheritdoc />
        public ITypeSymbol[] GenericTypeArguments => ResolveTypeSymbols(_type.GenericTypeArguments);

        /// <inheritdoc />
        public bool HasElementType => _type.HasElementType;

        /// <inheritdoc />
        public TypeCode TypeCode => Type.GetTypeCode(_type);

        /// <inheritdoc />
        public bool IsAbstract => _type.IsAbstract;

#if NETFRAMEWORK

        /// <inheritdoc />
        /// <remarks>
        /// There's no API to distinguish an array of rank 1 from a vector,
        /// so we check if the type name ends in [], which indicates it's a vector
        /// (non-vectors will have [*] or [,]).
        /// </remarks>
        public bool IsSZArray => _type.IsArray && _type.Name.EndsWith("[]");

#else

        /// <inheritdoc />
        public bool IsSZArray => _type.IsSZArray;

#endif

        /// <inheritdoc />
        public bool IsArray => _type.IsArray;

        /// <inheritdoc />
        public bool IsAutoLayout => _type.IsAutoLayout;

        /// <inheritdoc />
        public bool IsExplicitLayout => _type.IsExplicitLayout;

        /// <inheritdoc />
        public bool IsByRef => _type.IsByRef;

        /// <inheritdoc />
        public bool IsClass => _type.IsClass;

        /// <inheritdoc />
        public bool IsEnum => _type.IsEnum;

        /// <inheritdoc />
        public bool IsInterface => _type.IsInterface;

        /// <inheritdoc />
        public bool IsConstructedGenericType => _type.IsConstructedGenericType;

        /// <inheritdoc />
        public bool IsGenericParameter => _type.IsGenericParameter;

        /// <inheritdoc />
        public bool IsGenericType => _type.IsGenericType;

        /// <inheritdoc />
        public bool IsGenericTypeDefinition => _type.IsGenericTypeDefinition;

        /// <inheritdoc />
        public bool IsLayoutSequential => _type.IsLayoutSequential;

        /// <inheritdoc />
        public bool IsNested => _type.IsNested;

        /// <inheritdoc />
        public bool IsNestedAssembly => _type.IsNestedAssembly;

        /// <inheritdoc />
        public bool IsNestedFamANDAssem => _type.IsNestedFamANDAssem;

        /// <inheritdoc />
        public bool IsNestedFamORAssem => _type.IsNestedFamORAssem;

        /// <inheritdoc />
        public bool IsNestedFamily => _type.IsNestedFamily;

        /// <inheritdoc />
        public bool IsNestedPrivate => _type.IsNestedPrivate;

        /// <inheritdoc />
        public bool IsNestedPublic => _type.IsNestedPublic;

        /// <inheritdoc />
        public bool IsNotPublic => _type.IsNotPublic;

        /// <inheritdoc />
        public bool IsPointer => _type.IsPointer;

#if NET8_0_OR_GREATER

        /// <inheritdoc />
        public bool IsFunctionPointer => _type.IsFunctionPointer;

        /// <inheritdoc />
        public bool IsUnmanagedFunctionPointer => _type.IsUnmanagedFunctionPointer;

#else

        /// <inheritdoc />
        public bool IsFunctionPointer => throw new NotImplementedException();

        /// <inheritdoc />
        public bool IsUnmanagedFunctionPointer => throw new NotImplementedException();

#endif

        /// <inheritdoc />
        public bool IsPrimitive => _type.IsPrimitive;

        /// <inheritdoc />
        public bool IsPublic => _type.IsPublic;

        /// <inheritdoc />
        public bool IsSealed => _type.IsSealed;

#pragma warning disable SYSLIB0050 // Type or member is obsolete
        /// <inheritdoc />
        public bool IsSerializable => _type.IsSerializable;
#pragma warning restore SYSLIB0050 // Type or member is obsolete

        /// <inheritdoc />
        public bool IsValueType => _type.IsValueType;

        /// <inheritdoc />
        public bool IsVisible => _type.IsVisible;

#if NET6_0_OR_GREATER

        /// <inheritdoc />
        public bool IsSignatureType => _type.IsSignatureType;

#else

        /// <inheritdoc />
        public bool IsSignatureType => throw new NotImplementedException();

#endif

        /// <inheritdoc />
        public bool IsSpecialName => _type.IsSpecialName;

        /// <inheritdoc />
        public IConstructorSymbol? TypeInitializer => _type.TypeInitializer is ConstructorInfo ctor ? ResolveConstructorSymbol(ctor) : null;

        /// <inheritdoc />
        public int GetArrayRank()
        {
            return _type.GetArrayRank();
        }

        /// <inheritdoc />
        public IConstructorSymbol? GetConstructor(BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return _type.GetConstructor(bindingAttr, binder: null, types.Unpack(), modifiers: null) is ConstructorInfo ctor ? ResolveConstructorSymbol(ctor) : null;
        }

        /// <inheritdoc />
        public IConstructorSymbol? GetConstructor(ITypeSymbol[] types)
        {
            return _type.GetConstructor(types.Unpack()) is ConstructorInfo ctor ? ResolveConstructorSymbol(ctor) : null;
        }

        /// <inheritdoc />
        public IConstructorSymbol[] GetConstructors()
        {
            return ResolveConstructorSymbols(_type.GetConstructors());
        }

        /// <inheritdoc />
        public IConstructorSymbol[] GetConstructors(BindingFlags bindingAttr)
        {
            return ResolveConstructorSymbols(_type.GetConstructors(bindingAttr));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetDefaultMembers()
        {
            return ResolveMemberSymbols(_type.GetDefaultMembers());
        }

        /// <inheritdoc />
        public ITypeSymbol? GetElementType()
        {
            return _type.GetElementType() is Type t ? ResolveTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public string? GetEnumName(object value)
        {
            return _type.GetEnumName(value);
        }

        /// <inheritdoc />
        public string[] GetEnumNames()
        {
            return _type.GetEnumNames();
        }

        /// <inheritdoc />
        public ITypeSymbol GetEnumUnderlyingType()
        {
            return ResolveTypeSymbol(_type.GetEnumUnderlyingType());
        }

        /// <inheritdoc />
        public Array GetEnumValues()
        {
            return _type.GetEnumValues();
        }

        /// <inheritdoc />
        public IEventSymbol? GetEvent(string name, BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEventSymbol? GetEvent(string name)
        {
            return _type.GetEvent(name) is { } f ? ResolveEventSymbol(f) : null;
        }

        /// <inheritdoc />
        public IEventSymbol[] GetEvents()
        {
            return ResolveEventSymbols(_type.GetEvents());
        }

        /// <inheritdoc />
        public IEventSymbol[] GetEvents(BindingFlags bindingAttr)
        {
            return ResolveEventSymbols(_type.GetEvents(bindingAttr));
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name)
        {
            return _type.GetField(name) is FieldInfo f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name, BindingFlags bindingAttr)
        {
            return _type.GetField(name, bindingAttr) is FieldInfo f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields()
        {
            return ResolveFieldSymbols(_type.GetFields());
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields(BindingFlags bindingAttr)
        {
            return ResolveFieldSymbols(_type.GetFields(bindingAttr));
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetGenericArguments()
        {
            return ResolveTypeSymbols(_type.GetGenericArguments());
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetGenericParameterConstraints()
        {
            return ResolveTypeSymbols(_type.GetGenericParameterConstraints());
        }

        /// <inheritdoc />
        public ITypeSymbol GetGenericTypeDefinition()
        {
            return ResolveTypeSymbol(_type.GetGenericTypeDefinition());
        }

        /// <inheritdoc />
        public ITypeSymbol? GetInterface(string name)
        {
            return _type.GetInterface(name) is { } i ? ResolveTypeSymbol(i) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol? GetInterface(string name, bool ignoreCase)
        {
            return _type.GetInterface(name, ignoreCase) is { } i ? ResolveTypeSymbol(i) : null;
        }

        /// <inheritdoc />
        public InterfaceMapping GetInterfaceMap(ITypeSymbol interfaceType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetInterfaces(bool inherit = true)
        {
            if (inherit)
                return ResolveTypeSymbols(_type.GetInterfaces());
            else
                throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMember(string name)
        {
            return ResolveMemberSymbols(_type.GetMember(name));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMember(string name, BindingFlags bindingAttr)
        {
            return ResolveMemberSymbols(_type.GetMember(name, bindingAttr));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
        {
            return ResolveMemberSymbols(_type.GetMember(name, type, bindingAttr));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMembers(BindingFlags bindingAttr)
        {
            return ResolveMemberSymbols(_type.GetMembers(bindingAttr));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMembers()
        {
            return ResolveMemberSymbols(_type.GetMembers());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr)
        {
            return _type.GetMethod(name, bindingAttr) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
        {
            return _type.GetMethod(name, types.Unpack()) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return _type.GetMethod(name, bindingAttr, null, types.Unpack(), null) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name)
        {
            return _type.GetMethod(name) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods(BindingFlags bindingAttr)
        {
            return ResolveMethodSymbols(_type.GetMethods(bindingAttr));
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods()
        {
            return ResolveMethodSymbols(_type.GetMethods());
        }

        /// <inheritdoc />
        public ITypeSymbol? GetNestedType(string name)
        {
            return _type.GetNestedType(name) is Type t ? ResolveTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol? GetNestedType(string name, BindingFlags bindingAttr)
        {
            return _type.GetNestedType(name, bindingAttr) is Type t ? ResolveTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetNestedTypes()
        {
            return ResolveTypeSymbols(_type.GetNestedTypes());
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetNestedTypes(BindingFlags bindingAttr)
        {
            return ResolveTypeSymbols(_type.GetNestedTypes(bindingAttr));
        }

        /// <inheritdoc />
        public IPropertySymbol[] GetProperties()
        {
            return ResolvePropertySymbols(_type.GetProperties());
        }

        /// <inheritdoc />
        public IPropertySymbol[] GetProperties(BindingFlags bindingAttr)
        {
            return ResolvePropertySymbols(_type.GetProperties(bindingAttr));
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, ITypeSymbol[] types)
        {
            return _type.GetProperty(name, types.Unpack()) is { } p ? ResolvePropertySymbol(p) : null;
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types)
        {
            return _type.GetProperty(name, returnType?.Unpack(), types.Unpack()) is { } p ? ResolvePropertySymbol(p) : null;
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, BindingFlags bindingAttr)
        {
            return _type.GetProperty(name, bindingAttr) is { } p ? ResolvePropertySymbol(p) : null;
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name)
        {
            return _type.GetProperty(name) is { } p ? ResolvePropertySymbol(p) : null;
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType)
        {
            return _type.GetProperty(name, returnType?.Unpack()) is { } p ? ResolvePropertySymbol(p) : null;
        }

        /// <inheritdoc />
        public bool IsAssignableFrom(ITypeSymbol? c)
        {
            return _type.IsAssignableFrom(c?.Unpack());
        }

        /// <inheritdoc />
        public bool IsEnumDefined(object value)
        {
            return _type.IsEnumDefined(value);
        }

        /// <inheritdoc />
        public bool IsSubclassOf(ITypeSymbol c)
        {
            return _type.IsSubclassOf(c.Unpack());
        }

        /// <inheritdoc />
        public ITypeSymbol MakeArrayType()
        {
            if (_asSZArray == null)
                Interlocked.CompareExchange(ref _asSZArray, new ReflectionTypeSymbol(Context, ContainingModule, _type.MakeArrayType()), null);

            return _asSZArray;
        }

        /// <inheritdoc />
        public ITypeSymbol MakeArrayType(int rank)
        {
            if (rank == 1)
                return MakeArrayType();

            if (_asArray == null)
                Interlocked.CompareExchange(ref _asArray, new ReflectionTypeSymbol?[32], null);

            ref var asArray = ref _asArray[rank];
            if (asArray == null)
                Interlocked.CompareExchange(ref asArray, new ReflectionTypeSymbol(Context, ContainingModule, _type.MakeArrayType(rank)), null);

            return asArray;
        }

        /// <inheritdoc />
        public ITypeSymbol MakeByRefType()
        {
            if (_asByRef == null)
                Interlocked.CompareExchange(ref _asByRef, new ReflectionTypeSymbol(Context, ContainingModule, _type.MakeByRefType()), null);

            return _asByRef;
        }

        /// <inheritdoc />
        public ITypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ITypeSymbol MakePointerType()
        {
            if (_asPointer == null)
                Interlocked.CompareExchange(ref _asPointer, new ReflectionTypeSymbol(Context, ContainingModule, _type.MakePointerType()), null);

            return _asPointer;
        }

        /// <summary>
        /// Sets the reflection type. Used by the builder infrastructure to complete a symbol.
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="NotImplementedException"></exception>
        internal void FinishType(Type type)
        {
            _type = type;
        }

    }

}
