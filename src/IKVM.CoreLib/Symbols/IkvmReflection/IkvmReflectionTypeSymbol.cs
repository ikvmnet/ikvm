using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using IKVM.CoreLib.Symbols.Reflection;
using IKVM.Reflection;

using ConstructorInfo = IKVM.Reflection.ConstructorInfo;
using FieldInfo = IKVM.Reflection.FieldInfo;
using MethodInfo = IKVM.Reflection.MethodInfo;
using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionTypeSymbol : IkvmReflectionMemberSymbol, ITypeSymbol
    {

        const int MAX_CAPACITY = 65536 * 2;

        const BindingFlags DefaultBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        Type _type;

        IkvmReflectionTypeSymbol?[]? _asArray;
        IkvmReflectionTypeSymbol? _asSZArray;
        IkvmReflectionTypeSymbol? _asPointer;
        IkvmReflectionTypeSymbol? _asByRef;
        ConcurrentDictionary<ITypeSymbol[], IkvmReflectionTypeSymbol>? _genericTypeSymbols;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        public IkvmReflectionTypeSymbol(IkvmReflectionSymbolContext context, IkvmReflectionModuleSymbol module, Type type) :
            base(context, module, type)
        {
            Debug.Assert(module.ReflectionObject == type.Module);
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionTypeSymbol"/> cached for the generic parameter type.
        /// </summary>
        /// <param name="genericTypeArguments"></param>
        /// <returns></returns>
        internal IkvmReflectionTypeSymbol GetOrCreateGenericTypeSymbol(ITypeSymbol[] genericTypeArguments)
        {
            if (genericTypeArguments is null)
                throw new ArgumentNullException(nameof(genericTypeArguments));

            if (_type.IsGenericTypeDefinition == false)
                throw new InvalidOperationException();

            if (_genericTypeSymbols == null)
                Interlocked.CompareExchange(ref _genericTypeSymbols, new ConcurrentDictionary<ITypeSymbol[], IkvmReflectionTypeSymbol>(TypeSymbolListEqualityComparer.Instance), null);

            return _genericTypeSymbols.GetOrAdd(
                genericTypeArguments,
                _ => new IkvmReflectionTypeSymbol(Context, ContainingModule, _type.MakeGenericType(genericTypeArguments.Unpack())));
        }

        /// <summary>
        /// Resolves the symbol for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected internal override IkvmReflectionTypeSymbol ResolveTypeSymbol(Type type)
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
        public System.Reflection.TypeAttributes Attributes => (System.Reflection.TypeAttributes)_type.Attributes;

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
        public System.Reflection.GenericParameterAttributes GenericParameterAttributes => (System.Reflection.GenericParameterAttributes)_type.GenericParameterAttributes;

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

        /// <inheritdoc />
        public bool IsSerializable => _type.IsSerializable;

        /// <inheritdoc />
        public bool IsValueType => _type.IsValueType;

        /// <inheritdoc />
        public bool IsVisible => _type.IsVisible;

        /// <inheritdoc />
        public bool IsSignatureType => throw new NotImplementedException();

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
        public IConstructorSymbol? GetConstructor(System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return _type.GetConstructor((BindingFlags)bindingAttr, binder: null, types.Unpack(), modifiers: null) is ConstructorInfo ctor ? ResolveConstructorSymbol(ctor) : null;
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
        public IConstructorSymbol[] GetConstructors(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveConstructorSymbols(_type.GetConstructors((BindingFlags)bindingAttr));
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
        public IEventSymbol? GetEvent(string name, System.Reflection.BindingFlags bindingAttr)
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
        public IEventSymbol[] GetEvents(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveEventSymbols(_type.GetEvents((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name)
        {
            return _type.GetField(name) is FieldInfo f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return _type.GetField(name, (BindingFlags)bindingAttr) is FieldInfo f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields()
        {
            return ResolveFieldSymbols(_type.GetFields());
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveFieldSymbols(_type.GetFields((BindingFlags)bindingAttr));
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
        public IMemberSymbol[] GetMember(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveMemberSymbols(_type.GetMember(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMember(string name, System.Reflection.MemberTypes type, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveMemberSymbols(_type.GetMember(name, (MemberTypes)type, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMembers(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveMemberSymbols(_type.GetMembers((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMembers()
        {
            return ResolveMemberSymbols(_type.GetMembers());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return _type.GetMethod(name, (BindingFlags)bindingAttr) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
        {
            return _type.GetMethod(name, types.Unpack()) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return _type.GetMethod(name, (BindingFlags)bindingAttr, null, types.Unpack(), null) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name)
        {
            return _type.GetMethod(name) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, System.Reflection.CallingConventions callConvention, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            return _type.GetMethod(name, (BindingFlags)bindingAttr, null, (CallingConventions)callConvention, types.Unpack(), modifiers?.Unpack()) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, int genericParameterCount, System.Reflection.BindingFlags bindingAttr, System.Reflection.CallingConventions callConvention, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, int genericParameterCount, System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            return _type.GetMethod(name, (BindingFlags)bindingAttr, null, types.Unpack(), modifiers?.Unpack()) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, int genericParameterCount, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveMethodSymbols(_type.GetMethods((BindingFlags)bindingAttr));
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
        public ITypeSymbol? GetNestedType(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return _type.GetNestedType(name, (BindingFlags)bindingAttr) is Type t ? ResolveTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetNestedTypes()
        {
            return ResolveTypeSymbols(_type.GetNestedTypes());
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetNestedTypes(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveTypeSymbols(_type.GetNestedTypes((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IPropertySymbol[] GetProperties()
        {
            return ResolvePropertySymbols(_type.GetProperties());
        }

        /// <inheritdoc />
        public IPropertySymbol[] GetProperties(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolvePropertySymbols(_type.GetProperties((BindingFlags)bindingAttr));
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
        public IPropertySymbol? GetProperty(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return _type.GetProperty(name, (BindingFlags)bindingAttr) is { } p ? ResolvePropertySymbol(p) : null;
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
                Interlocked.CompareExchange(ref _asSZArray, new IkvmReflectionTypeSymbol(Context, ContainingModule, _type.MakeArrayType()), null);

            return _asSZArray;
        }

        /// <inheritdoc />
        public ITypeSymbol MakeArrayType(int rank)
        {
            if (rank == 1)
                return MakeArrayType();

            if (_asArray == null)
                Interlocked.CompareExchange(ref _asArray, new IkvmReflectionTypeSymbol?[32], null);

            ref var asArray = ref _asArray[rank];
            if (asArray == null)
                Interlocked.CompareExchange(ref asArray, new IkvmReflectionTypeSymbol(Context, ContainingModule, _type.MakeArrayType(rank)), null);

            return asArray;
        }

        /// <inheritdoc />
        public ITypeSymbol MakeByRefType()
        {
            if (_asByRef == null)
                Interlocked.CompareExchange(ref _asByRef, new IkvmReflectionTypeSymbol(Context, ContainingModule, _type.MakeByRefType()), null);

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
                Interlocked.CompareExchange(ref _asPointer, new IkvmReflectionTypeSymbol(Context, ContainingModule, _type.MakePointerType()), null);

            return _asPointer;
        }

        /// <summary>
        /// Sets the reflection type. Used by the builder infrastructure to complete a symbol.
        /// </summary>
        /// <param name="type"></param>
        internal void Complete(Type type)
        {
            ResolveTypeSymbol(_type = type);
            base.Complete(_type);

            ContainingModule.Complete(_type.Module);

            foreach (var i in _type.GetConstructors(DefaultBindingFlags))
                ResolveConstructorSymbol(i).Complete(i);

            foreach (var i in _type.GetMethods(DefaultBindingFlags))
                ResolveMethodSymbol(i).Complete(i);

            foreach (var i in _type.GetFields(DefaultBindingFlags))
                ResolveFieldSymbol(i).Complete(i);

            foreach (var i in _type.GetProperties(DefaultBindingFlags))
                ResolvePropertySymbol(i).Complete(i);

            foreach (var i in _type.GetEvents(DefaultBindingFlags))
                ResolveEventSymbol(i).Complete(i);
        }
    }

}
