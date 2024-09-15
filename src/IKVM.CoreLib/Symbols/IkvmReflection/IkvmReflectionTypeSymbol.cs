using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

using IKVM.Reflection;

using ConstructorInfo = IKVM.Reflection.ConstructorInfo;
using EventInfo = IKVM.Reflection.EventInfo;
using FieldInfo = IKVM.Reflection.FieldInfo;
using MethodBase = IKVM.Reflection.MethodBase;
using MethodInfo = IKVM.Reflection.MethodInfo;
using PropertyInfo = IKVM.Reflection.PropertyInfo;
using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionTypeSymbol : IkvmReflectionMemberSymbol, ITypeSymbol
    {

        const BindingFlags DefaultBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        readonly Type _type;

        MethodBase[]? _methodsSource;
        int _methodsBaseRow;
        IkvmReflectionMethodBaseSymbol?[]? _methods;

        FieldInfo[]? _fieldsSource;
        int _fieldsBaseRow;
        IkvmReflectionFieldSymbol?[]? _fields;

        PropertyInfo[]? _propertiesSource;
        int _propertiesBaseRow;
        IkvmReflectionPropertySymbol?[]? _properties;

        EventInfo[]? _eventsSource;
        int _eventsBaseRow;
        IkvmReflectionEventSymbol?[]? _events;

        IkvmReflectionTypeSymbol?[]? _asArray;
        IkvmReflectionTypeSymbol? _asSZArray;
        IkvmReflectionTypeSymbol? _asPointer;
        IkvmReflectionTypeSymbol? _asByRef;

        Type[]? _genericParametersSource;
        IkvmReflectionTypeSymbol?[]? _genericParameters;
        List<(Type[] Arguments, IkvmReflectionTypeSymbol Symbol)>? _genericTypes;
        ReaderWriterLockSlim? _genericTypesLock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        public IkvmReflectionTypeSymbol(IkvmReflectionSymbolContext context, IkvmReflectionModuleSymbol module, Type type) :
            base(context, module, null, type)
        {
            Debug.Assert(module.ReflectionObject == type.Module);
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal IkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            return (IkvmReflectionMethodSymbol)GetOrCreateMethodBaseSymbol(method);
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionConstructorSymbol"/> cached for the type by ctor.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal IkvmReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return (IkvmReflectionConstructorSymbol)GetOrCreateMethodBaseSymbol(ctor);
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal IkvmReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            if (method is null)
                throw new ArgumentNullException(nameof(method));

            Debug.Assert(method.DeclaringType == _type);

            var hnd = MetadataTokens.MethodDefinitionHandle(method.MetadataToken);
            var row = MetadataTokens.GetRowNumber(hnd);

            // initialize source table
            if (_methodsSource == null)
            {
                Interlocked.CompareExchange(ref _methodsSource, _type.GetConstructors(DefaultBindingFlags).Cast<MethodBase>().Concat(_type.GetMethods(DefaultBindingFlags)).OrderBy(i => i.MetadataToken).ToArray(), null);
                _methodsBaseRow = _methodsSource.Length != 0 ? MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(_methodsSource[0].MetadataToken)) : 0;
            }

            // initialize cache table
            if (_methods == null)
                Interlocked.CompareExchange(ref _methods, new IkvmReflectionMethodBaseSymbol?[_methodsSource.Length], null);

            // index of current record is specified row - base
            var idx = row - _methodsBaseRow;
            Debug.Assert(idx >= 0);
            Debug.Assert(idx < _methodsSource.Length);

            // check that our list is long enough to contain the entire table
            if (_methods.Length < idx)
                throw new IndexOutOfRangeException();

            // if not yet created, create, allow multiple instances, but only one is eventually inserted
            ref var rec = ref _methods[idx];
            if (rec == null)
            {
                switch (method)
                {
                    case ConstructorInfo c:
                        Interlocked.CompareExchange(ref rec, new IkvmReflectionConstructorSymbol(Context, ContainingModule, this, c), null);
                        break;
                    case MethodInfo m:
                        Interlocked.CompareExchange(ref rec, new IkvmReflectionMethodSymbol(Context, ContainingModule, this, m), null);
                        break;
                }
            }

            // this should never happen
            if (rec is not IkvmReflectionMethodBaseSymbol sym)
                throw new InvalidOperationException();

            return sym;
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionFieldSymbol"/> cached for the type by field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal IkvmReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));

            Debug.Assert(field.DeclaringType == _type);

            var hnd = MetadataTokens.FieldDefinitionHandle(field.MetadataToken);
            var row = MetadataTokens.GetRowNumber(hnd);

            // initialize source table
            if (_fieldsSource == null)
            {
                Interlocked.CompareExchange(ref _fieldsSource, _type.GetFields(DefaultBindingFlags).OrderBy(i => i.MetadataToken).ToArray(), null);
                _fieldsBaseRow = _fieldsSource.Length != 0 ? MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(_fieldsSource[0].MetadataToken)) : 0;
            }

            // initialize cache table
            if (_fields == null)
                Interlocked.CompareExchange(ref _fields, new IkvmReflectionFieldSymbol?[_fieldsSource.Length], null);

            // index of current field is specified row - base
            var idx = row - _fieldsBaseRow;
            Debug.Assert(idx >= 0);
            Debug.Assert(idx < _fieldsSource.Length);

            // check that our list is long enough to contain the entire table
            if (_fields.Length < idx)
                throw new IndexOutOfRangeException();

            // if not yet created, create, allow multiple instances, but only one is eventually inserted
            ref var rec = ref _fields[idx];
            if (rec == null)
                Interlocked.CompareExchange(ref rec, new IkvmReflectionFieldSymbol(Context, this, field), null);

            // this should never happen
            if (rec is not IkvmReflectionFieldSymbol sym)
                throw new InvalidOperationException();

            return sym;
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionPropertySymbol"/> cached for the type by property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal IkvmReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));

            Debug.Assert(property.DeclaringType == _type);

            var hnd = MetadataTokens.PropertyDefinitionHandle(property.MetadataToken);
            var row = MetadataTokens.GetRowNumber(hnd);

            // initialize source table
            if (_propertiesSource == null)
            {
                Interlocked.CompareExchange(ref _propertiesSource, _type.GetProperties(DefaultBindingFlags).OrderBy(i => i.MetadataToken).ToArray(), null);
                _propertiesBaseRow = _propertiesSource.Length != 0 ? MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(_propertiesSource[0].MetadataToken)) : 0;
            }

            // initialize cache table
            if (_properties == null)
                Interlocked.CompareExchange(ref _properties, new IkvmReflectionPropertySymbol?[_propertiesSource.Length], null);

            // index of current property is specified row - base
            var idx = row - _propertiesBaseRow;
            Debug.Assert(idx >= 0);
            Debug.Assert(idx < _propertiesSource.Length);

            // check that our list is long enough to contain the entire table
            if (_properties.Length < idx)
                throw new IndexOutOfRangeException();

            // if not yet created, create, allow multiple instances, but only one is eventually inserted
            ref var rec = ref _properties[idx];
            if (rec == null)
                Interlocked.CompareExchange(ref rec, new IkvmReflectionPropertySymbol(Context, this, property), null);

            // this should never happen
            if (rec is not IkvmReflectionPropertySymbol sym)
                throw new InvalidOperationException();

            return sym;
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionEventSymbol"/> cached for the type by event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal IkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));

            Debug.Assert(@event.DeclaringType == _type);

            var hnd = MetadataTokens.PropertyDefinitionHandle(@event.MetadataToken);
            var row = MetadataTokens.GetRowNumber(hnd);

            // initialize source events
            if (_eventsSource == null)
            {
                Interlocked.CompareExchange(ref _eventsSource, _type.GetEvents(DefaultBindingFlags).OrderBy(i => i.MetadataToken).ToArray(), null);
                _eventsBaseRow = _eventsSource.Length != 0 ? MetadataTokens.GetRowNumber(MetadataTokens.EventDefinitionHandle(_eventsSource[0].MetadataToken)) : 0;
            }

            // initialize cache table
            if (_events == null)
                Interlocked.CompareExchange(ref _events, new IkvmReflectionEventSymbol?[_eventsSource.Length], null);

            // index of current event is specified row - base
            var idx = row - _eventsBaseRow;
            Debug.Assert(idx >= 0);
            Debug.Assert(idx < _eventsSource.Length);

            // check that our list is long enough to contain the entire table
            if (_events.Length < idx)
                throw new IndexOutOfRangeException();

            // if not yet created, create, allow multiple instances, but only one is eventually inserted
            ref var rec = ref _events[idx];
            if (rec == null)
                Interlocked.CompareExchange(ref rec, new IkvmReflectionEventSymbol(Context, this, @event), null);

            // this should never happen
            if (rec is not IkvmReflectionEventSymbol sym)
                throw new InvalidOperationException();

            return sym;
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionTypeSymbol"/> cached for the generic parameter type.
        /// </summary>
        /// <param name="genericTypeParameterType"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal IkvmReflectionTypeSymbol GetOrCreateGenericParameterSymbol(Type genericTypeParameterType)
        {
            if (genericTypeParameterType is null)
                throw new ArgumentNullException(nameof(genericTypeParameterType));

            Debug.Assert(genericTypeParameterType.DeclaringType == _type);

            // initialize tables
            if (_genericParametersSource == null)
                Interlocked.CompareExchange(ref _genericParametersSource, _type.GetGenericArguments(), null);
            if (_genericParameters == null)
                Interlocked.CompareExchange(ref _genericParameters, new IkvmReflectionTypeSymbol?[_genericParametersSource.Length], null);

            // position of the parameter in the list
            var idx = genericTypeParameterType.GenericParameterPosition;

            // check that our list is long enough to contain the entire table
            if (_genericParameters.Length < idx)
                throw new IndexOutOfRangeException();

            // if not yet created, create, allow multiple instances, but only one is eventually inserted
            ref var rec = ref _genericParameters[idx];
            if (rec == null)
                Interlocked.CompareExchange(ref rec, new IkvmReflectionTypeSymbol(Context, ContainingModule, genericTypeParameterType), null);

            // this should never happen
            if (rec is not IkvmReflectionTypeSymbol sym)
                throw new InvalidOperationException();

            return sym;
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionTypeSymbol"/> cached for the generic parameter type.
        /// </summary>
        /// <param name="genericTypeArguments"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal IkvmReflectionTypeSymbol GetOrCreateGenericTypeSymbol(Type[] genericTypeArguments)
        {
            if (genericTypeArguments is null)
                throw new ArgumentNullException(nameof(genericTypeArguments));

            if (_type.IsGenericTypeDefinition == false)
                throw new InvalidOperationException();

            // initialize the available parameters
            if (_genericParametersSource == null)
                Interlocked.CompareExchange(ref _genericParametersSource, _type.GetGenericArguments(), null);
            if (_genericParametersSource.Length != genericTypeArguments.Length)
                throw new InvalidOperationException();

            // initialize generic type map, and lock on it since we're potentially adding items
            if (_genericTypes == null)
                Interlocked.CompareExchange(ref _genericTypes, [], null);
            if (_genericTypesLock == null)
                Interlocked.CompareExchange(ref _genericTypesLock, new(), null);

            try
            {
                _genericTypesLock.EnterUpgradeableReadLock();

                // find existing entry
                foreach (var i in _genericTypes)
                    if (i.Arguments.SequenceEqual(genericTypeArguments))
                        return i.Symbol;

                try
                {
                    _genericTypesLock.EnterWriteLock();

                    // generate new symbol
                    var sym = new IkvmReflectionTypeSymbol(Context, ContainingModule, _type.MakeGenericType(genericTypeArguments));
                    _genericTypes.Add((genericTypeArguments, sym));
                    return sym;
                }
                finally
                {
                    _genericTypesLock.ExitWriteLock();
                }
            }
            finally
            {
                _genericTypesLock.ExitUpgradeableReadLock();
            }
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
        /// Resolves the symbol for the specified constructor.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        protected internal override IkvmReflectionConstructorSymbol ResolveConstructorSymbol(ConstructorInfo ctor)
        {
            if (ctor.DeclaringType == _type)
                return GetOrCreateConstructorSymbol(ctor);
            else
                return base.ResolveConstructorSymbol(ctor);
        }

        /// <summary>
        /// Resolves the symbol for the specified method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        protected internal override IkvmReflectionMethodSymbol ResolveMethodSymbol(MethodInfo method)
        {
            if (method.DeclaringType == _type)
                return GetOrCreateMethodSymbol(method);
            else
                return base.ResolveMethodSymbol(method);
        }

        /// <summary>
        /// Resolves the symbol for the specified field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected internal override IkvmReflectionFieldSymbol ResolveFieldSymbol(FieldInfo field)
        {
            if (field.DeclaringType == _type)
                return GetOrCreateFieldSymbol(field);
            else
                return base.ResolveFieldSymbol(field);
        }

        /// <summary>
        /// Resolves the symbol for the specified field.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected internal override IkvmReflectionPropertySymbol ResolvePropertySymbol(PropertyInfo property)
        {
            if (property.DeclaringType == _type)
                return GetOrCreatePropertySymbol(property);
            else
                return base.ResolvePropertySymbol(property);
        }

        /// <summary>
        /// Resolves the symbol for the specified event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        protected internal override IkvmReflectionEventSymbol ResolveEventSymbol(EventInfo @event)
        {
            if (@event.DeclaringType == _type)
                return GetOrCreateEventSymbol(@event);
            else
                return base.ResolveEventSymbol(@event);
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
        public System.Reflection.TypeAttributes Attributes => (System.Reflection.TypeAttributes)(int)_type.Attributes;

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
        public System.Reflection.GenericParameterAttributes GenericParameterAttributes => (System.Reflection.GenericParameterAttributes)(int)_type.GenericParameterAttributes;

        /// <inheritdoc />
        public int GenericParameterPosition => _type.GenericParameterPosition;

        /// <inheritdoc />
        public ITypeSymbol[] GenericTypeArguments => ResolveTypeSymbols(_type.GenericTypeArguments);

        /// <inheritdoc />
        public bool HasElementType => _type.HasElementType;

        /// <inheritdoc />
        public bool IsAbstract => _type.IsAbstract;

        /// <inheritdoc />
        public bool IsSZArray => _type.IsSZArray;

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
        public bool IsConstructedGenericType => _type.IsConstructedGenericType;

        /// <inheritdoc />
        public bool IsEnum => _type.IsEnum;

        /// <inheritdoc />
        public bool IsGenericParameter => _type.IsGenericParameter;

        /// <inheritdoc />
        public bool IsGenericType => _type.IsGenericType;

        /// <inheritdoc />
        public bool IsGenericTypeDefinition => _type.IsGenericTypeDefinition;

        /// <inheritdoc />
        public bool IsInterface => _type.IsInterface;

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

        /// <inheritdoc />
        public bool IsFunctionPointer => _type.IsFunctionPointer;

        /// <inheritdoc />
        public bool IsUnmanagedFunctionPointer => _type.IsUnmanagedFunctionPointer;

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

        /// <inheritdoc />
        public bool IsSignatureType => throw new NotImplementedException();

        /// <inheritdoc />
        public bool IsSpecialName => _type.IsSpecialName;

        /// <inheritdoc />
        public TypeCode TypeCode => Type.GetTypeCode(_type);

        /// <inheritdoc />
        public IConstructorSymbol? TypeInitializer => _type.TypeInitializer is ConstructorInfo ctor ? ResolveConstructorSymbol(ctor) : null;

        /// <inheritdoc />
        public override bool IsMissing => _type.__IsMissing;

        /// <inheritdoc />
        public override bool ContainsMissing => _type.__ContainsMissingType;

        /// <inheritdoc />
        public int GetArrayRank()
        {
            return _type.GetArrayRank();
        }

        /// <inheritdoc />
        public IConstructorSymbol? GetConstructor(System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return _type.GetConstructor((BindingFlags)bindingAttr, binder: null, UnpackTypeSymbols(types), modifiers: null) is ConstructorInfo ctor ? ResolveConstructorSymbol(ctor) : null;
        }

        /// <inheritdoc />
        public IConstructorSymbol? GetConstructor(ITypeSymbol[] types)
        {
            return _type.GetConstructor(UnpackTypeSymbols(types)) is ConstructorInfo ctor ? ResolveConstructorSymbol(ctor) : null;
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
        public IEventSymbol? GetEvent(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return _type.GetEvent(name, (BindingFlags)bindingAttr) is { } f ? ResolveEventSymbol(f) : null;
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
                return ResolveTypeSymbols(_type.__GetDeclaredInterfaces());
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
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return _type.GetMethod(name, (BindingFlags)bindingAttr, null, UnpackTypeSymbols(types), null) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
        {
            return _type.GetMethod(name, UnpackTypeSymbols(types)) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name)
        {
            return _type.GetMethod(name) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            if (modifiers != null)
                throw new NotImplementedException();

            return _type.GetMethod(name, UnpackTypeSymbols(types), null) is { } m ? ResolveMethodSymbol(m) : null;
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
        public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            if (modifiers != null)
                throw new NotImplementedException();

            var _returnType = returnType != null ? ((IkvmReflectionTypeSymbol)returnType).ReflectionObject : null;
            return _type.GetProperty(name, _returnType, UnpackTypeSymbols(types), null) is { } p ? ResolvePropertySymbol(p) : null;
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, ITypeSymbol[] types)
        {
            return _type.GetProperty(name, UnpackTypeSymbols(types)) is { } p ? ResolvePropertySymbol(p) : null;
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types)
        {
            var _returnType = returnType != null ? ((IkvmReflectionTypeSymbol)returnType).ReflectionObject : null;
            return _type.GetProperty(name, _returnType, UnpackTypeSymbols(types)) is { } p ? ResolvePropertySymbol(p) : null;
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
            var _returnType = returnType != null ? ((IkvmReflectionTypeSymbol)returnType).ReflectionObject : null;

            return _type.GetProperty(name, _returnType) is { } p ? ResolvePropertySymbol(p) : null;
        }

        /// <inheritdoc />
        public bool IsAssignableFrom(ITypeSymbol? c)
        {
            return _type.IsAssignableFrom(c != null ? ((IkvmReflectionTypeSymbol)c).ReflectionObject : null);
        }

        /// <inheritdoc />
        public bool IsEnumDefined(object value)
        {
            return _type.IsEnumDefined(value);
        }

        /// <inheritdoc />
        public bool IsSubclassOf(ITypeSymbol c)
        {
            return _type.IsSubclassOf(((IkvmReflectionTypeSymbol)c).ReflectionObject);
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

    }

}
