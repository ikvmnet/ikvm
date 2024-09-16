using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Threading;
using IKVM.Reflection;

using Module = IKVM.Reflection.Module;
using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    /// <summary>
    /// Implementation of <see cref="IModuleSymbol"/> derived from System.Reflection.
    /// </summary>
    class IkvmReflectionModuleSymbol : IkvmReflectionSymbol, IModuleSymbol
    {

        /// <summary>
        /// Returns <c>true</c> if the given <see cref="Type"/> is a TypeDef. That is, not a modified or substituted or generic parameter type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static bool IsTypeDefinition(Type type)
        {
            return type.HasElementType == false && type.IsConstructedGenericType == false && type.IsGenericParameter == false;
        }

        const int MAX_CAPACITY = 65536 * 2;

        const BindingFlags DefaultBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        readonly IkvmReflectionAssemblySymbol _containingAssembly;
        Module _module;

        IndexRangeDictionary<Type> _typeTable = new(maxCapacity: MAX_CAPACITY);
        IndexRangeDictionary<IkvmReflectionTypeSymbol> _typeSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _typeLock;

        IndexRangeDictionary<MethodBase> _methodTable = new(maxCapacity: MAX_CAPACITY);
        IndexRangeDictionary<IkvmReflectionMethodBaseSymbol> _methodSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _methodLock;

        IndexRangeDictionary<FieldInfo> _fieldTable = new(maxCapacity: MAX_CAPACITY);
        IndexRangeDictionary<IkvmReflectionFieldSymbol> _fieldSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _fieldLock;

        IndexRangeDictionary<PropertyInfo> _propertyTable = new(maxCapacity: MAX_CAPACITY);
        IndexRangeDictionary<IkvmReflectionPropertySymbol> _propertySymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _propertyLock;

        IndexRangeDictionary<EventInfo> _eventTable = new(maxCapacity: MAX_CAPACITY);
        IndexRangeDictionary<IkvmReflectionEventSymbol> _eventSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _eventLock;

        IndexRangeDictionary<ParameterInfo> _parameterTable = new();
        IndexRangeDictionary<IkvmReflectionParameterSymbol> _parameterSymbols = new();
        ReaderWriterLockSlim? _parameterLock;

        IndexRangeDictionary<IkvmReflectionTypeSymbol> _genericParameterSymbols = new();
        ReaderWriterLockSlim? _genericParameterLock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingAssembly"></param>
        /// <param name="module"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionModuleSymbol(IkvmReflectionSymbolContext context, IkvmReflectionAssemblySymbol containingAssembly, Module module) :
            base(context)
        {
            _containingAssembly = containingAssembly ?? throw new ArgumentNullException(nameof(containingAssembly));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        /// <summary>
        /// Gets the <see cref="IkvmReflectionModuleSymbol" /> which contains the metadata of this member.
        /// </summary>
        internal IkvmReflectionAssemblySymbol ContainingAssembly => _containingAssembly;

        /// <summary>
        /// Gets the wrapped <see cref="Module"/>.
        /// </summary>
        internal Module ReflectionObject => _module;

        /// <summary>
        /// Gets or creates the <see cref="ReflectionTypeSymbol"/> cached for the module by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal IkvmReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            Debug.Assert(type.Module == _module);

            // type is a generic parameter (GenericParam)
            if (type.IsGenericParameter)
                return GetOrCreateGenericParameterSymbol(type);

            // type is not a type definition (TypeDef)
            if (IsTypeDefinition(type) == false)
                return GetOrCreateTypeSymbolForSpecification(type);

            // create lock on demand
            if (_typeLock == null)
                lock (this)
                    _typeLock ??= new ReaderWriterLockSlim();

            using (_typeLock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.TypeDefinitionHandle(type.MetadataToken));
                if (_typeTable[row] != type)
                    using (_typeLock.CreateWriteLock())
                        _typeTable[row] = type;

                if (_typeSymbols[row] == null)
                    using (_typeLock.CreateWriteLock())
                        return _typeSymbols[row] ??= new IkvmReflectionTypeSymbol(Context, this, type);
                else
                    return _typeSymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates a <see cref="IkvmReflectionTypeSymbol"/> for the specification type: array, pointer, etc.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        IkvmReflectionTypeSymbol GetOrCreateTypeSymbolForSpecification(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            Debug.Assert(type.Module == _module);

            if (type.GetElementType() is { } elementType)
            {
                var elementTypeSymbol = ResolveTypeSymbol(elementType);

                // handles both SZ arrays and normal arrays
                if (type.IsArray)
                    return (IkvmReflectionTypeSymbol)elementTypeSymbol.MakeArrayType(type.GetArrayRank());

                if (type.IsPointer)
                    return (IkvmReflectionTypeSymbol)elementTypeSymbol.MakePointerType();

                if (type.IsByRef)
                    return (IkvmReflectionTypeSymbol)elementTypeSymbol.MakeByRefType();

                throw new InvalidOperationException();
            }

            if (type.IsGenericType)
            {
                var definitionTypeSymbol = ResolveTypeSymbol(type.GetGenericTypeDefinition());
                return definitionTypeSymbol.GetOrCreateGenericTypeSymbol(ResolveTypeSymbols(type.GetGenericArguments()));
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionMethodBaseSymbol"/> cached fqor the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal IkvmReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            if (method is null)
                throw new ArgumentNullException(nameof(method));

            Debug.Assert(method.Module.MetadataToken == _module.MetadataToken);

            // create lock on demand
            if (_methodLock == null)
                lock (this)
                    _methodLock ??= new ReaderWriterLockSlim();

            using (_methodLock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(method.MetadataToken));
                if (_methodTable[row] != method)
                    using (_methodLock.CreateWriteLock())
                        _methodTable[row] = method;

                if (_methodSymbols[row] == null)
                    using (_methodLock.CreateWriteLock())
                        if (method is ConstructorInfo c)
                            return _methodSymbols[row] ??= new IkvmReflectionConstructorSymbol(Context, ResolveTypeSymbol(c.DeclaringType ?? throw new InvalidOperationException()), c);
                        else if (method is MethodInfo m)
                            if (method.DeclaringType is { } dt)
                                return _methodSymbols[row] ??= new IkvmReflectionMethodSymbol(Context, ResolveTypeSymbol(dt), m);
                            else
                                return _methodSymbols[row] ??= new IkvmReflectionMethodSymbol(Context, this, m);
                        else
                            throw new InvalidOperationException();
                else
                    return _methodSymbols[row] ?? throw new InvalidOperationException();
            }
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
        internal IkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            return (IkvmReflectionMethodSymbol)GetOrCreateMethodBaseSymbol(method);
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

            Debug.Assert(field.Module == _module);

            // create lock on demand
            if (_fieldLock == null)
                lock (this)
                    _fieldLock ??= new ReaderWriterLockSlim();

            using (_fieldLock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.FieldDefinitionHandle(field.MetadataToken));
                if (_fieldTable[row] != field)
                    using (_fieldLock.CreateWriteLock())
                        _fieldTable[row] = field;

                if (_fieldSymbols[row] == null)
                    using (_fieldLock.CreateWriteLock())
                        if (field.DeclaringType is { } dt)
                            return _fieldSymbols[row] ??= new IkvmReflectionFieldSymbol(Context, ResolveTypeSymbol(dt), field);
                        else
                            return _fieldSymbols[row] ??= new IkvmReflectionFieldSymbol(Context, this, field);
                else
                    return _fieldSymbols[row] ?? throw new InvalidOperationException();
            }
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

            Debug.Assert(property.Module == _module);

            // create lock on demand
            if (_propertyLock == null)
                lock (this)
                    _propertyLock ??= new ReaderWriterLockSlim();

            using (_propertyLock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.PropertyDefinitionHandle(property.MetadataToken));
                if (_propertyTable[row] != property)
                    using (_propertyLock.CreateWriteLock())
                        _propertyTable[row] = property;

                if (_propertySymbols[row] == null)
                    using (_propertyLock.CreateWriteLock())
                        return _propertySymbols[row] ??= new IkvmReflectionPropertySymbol(Context, ResolveTypeSymbol(property.DeclaringType!), property);
                else
                    return _propertySymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionEventSymbol"/> cached for the type by event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal IkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));

            Debug.Assert(@event.Module == _module);

            // create lock on demand
            if (_eventLock == null)
                lock (this)
                    _eventLock ??= new ReaderWriterLockSlim();

            using (_eventLock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.EventDefinitionHandle(@event.MetadataToken));
                if (_eventTable[row] is not EventInfo i || i != @event)
                    using (_eventLock.CreateWriteLock())
                        _eventTable[row] = @event;

                if (_eventSymbols[row] == null)
                    using (_eventLock.CreateWriteLock())
                        return _eventSymbols[row] ??= new IkvmReflectionEventSymbol(Context, ResolveTypeSymbol(@event.DeclaringType!), @event);
                else
                    return _eventSymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal IkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));

            Debug.Assert(parameter.Member.Module == _module);

            // create lock on demand
            if (_parameterLock == null)
                lock (this)
                    _parameterLock ??= new ReaderWriterLockSlim();

            using (_parameterLock.CreateUpgradeableReadLock())
            {
                var position = parameter.Position;
                if (_parameterTable[position] != parameter)
                    using (_parameterLock.CreateWriteLock())
                        _parameterTable[position] = parameter;

                if (_parameterSymbols[position] == null)
                    using (_parameterLock.CreateWriteLock())
                        return _parameterSymbols[position] ??= new IkvmReflectionParameterSymbol(Context, ResolveMethodBaseSymbol((MethodBase)parameter.Member), parameter);
                else
                    return _parameterSymbols[position] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionTypeSymbol"/> cached for the module by type.
        /// </summary>
        /// <param name="genericParameterType"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal IkvmReflectionTypeSymbol GetOrCreateGenericParameterSymbol(Type genericParameterType)
        {
            if (genericParameterType is null)
                throw new ArgumentNullException(nameof(genericParameterType));

            Debug.Assert(genericParameterType.Module == _module);

            // create lock on demand
            if (_genericParameterLock == null)
                Interlocked.CompareExchange(ref _genericParameterLock, new ReaderWriterLockSlim(), null);

            using (_genericParameterLock.CreateUpgradeableReadLock())
            {
                var hnd = MetadataTokens.GenericParameterHandle(genericParameterType.MetadataToken);
                var row = MetadataTokens.GetRowNumber(hnd);
                if (_genericParameterSymbols[row] == null)
                    using (_genericParameterLock.CreateWriteLock())
                        return _genericParameterSymbols[row] ??= new IkvmReflectionTypeSymbol(Context, this, genericParameterType);
                else
                    return _genericParameterSymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <inheritdoc />
        public IAssemblySymbol Assembly => Context.GetOrCreateAssemblySymbol(_module.Assembly);

        /// <inheritdoc />
        public string FullyQualifiedName => _module.FullyQualifiedName;

        /// <inheritdoc />
        public int MetadataToken => _module.MetadataToken;

        /// <inheritdoc />
        public Guid ModuleVersionId => _module.ModuleVersionId;

        /// <inheritdoc />
        public string Name => _module.Name;

        /// <inheritdoc />
        public string ScopeName => _module.ScopeName;

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name)
        {
            return _module.GetField(name) is { } f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return _module.GetField(name, (BindingFlags)bindingAttr) is { } f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields(System.Reflection.BindingFlags bindingFlags)
        {
            return ResolveFieldSymbols(_module.GetFields((BindingFlags)bindingFlags));
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields()
        {
            return ResolveFieldSymbols(_module.GetFields());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name)
        {
            return _module.GetMethod(name) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
        {
            return _module.GetMethod(name, types.Unpack()) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, System.Reflection.CallingConventions callConvention, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods()
        {
            return ResolveMethodSymbols(_module.GetMethods());
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods(System.Reflection.BindingFlags bindingFlags)
        {
            return ResolveMethodSymbols(_module.GetMethods((BindingFlags)bindingFlags));
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string className)
        {
            return _module.GetType(className) is { } t ? ResolveTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string className, bool ignoreCase)
        {
            return _module.GetType(className, ignoreCase) is { } t ? ResolveTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string className, bool throwOnError, bool ignoreCase)
        {
            return _module.GetType(className, throwOnError, ignoreCase) is { } t ? ResolveTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetTypes()
        {
            return ResolveTypeSymbols(_module.GetTypes());
        }

        /// <inheritdoc />
        public bool IsResource()
        {
            return _module.IsResource();
        }

        /// <inheritdoc />
        public IFieldSymbol? ResolveField(int metadataToken)
        {
            return _module.ResolveField(metadataToken) is { } f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IFieldSymbol? ResolveField(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            return _module.ResolveField(metadataToken, genericTypeArguments?.Unpack(), genericMethodArguments?.Unpack()) is { } f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IMemberSymbol? ResolveMember(int metadataToken)
        {
            return _module.ResolveMember(metadataToken) is { } m ? ResolveMemberSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMemberSymbol? ResolveMember(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            return _module.ResolveMember(metadataToken, genericTypeArguments?.Unpack(), genericMethodArguments?.Unpack()) is { } m ? ResolveMemberSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodBaseSymbol? ResolveMethod(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            return _module.ResolveMethod(metadataToken, genericTypeArguments?.Unpack(), genericMethodArguments?.Unpack()) is { } m ? ResolveMethodBaseSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodBaseSymbol? ResolveMethod(int metadataToken)
        {
            return _module.ResolveMethod(metadataToken) is { } m ? ResolveMethodBaseSymbol(m) : null;
        }

        /// <inheritdoc />
        public byte[] ResolveSignature(int metadataToken)
        {
            return _module.ResolveSignature(metadataToken);
        }

        /// <inheritdoc />
        public string ResolveString(int metadataToken)
        {
            return _module.ResolveString(metadataToken);
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveType(int metadataToken)
        {
            return ResolveTypeSymbol(_module.ResolveType(metadataToken));
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveType(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            return ResolveTypeSymbol(_module.ResolveType(metadataToken, genericTypeArguments?.Unpack(), genericMethodArguments?.Unpack()));
        }

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return ResolveCustomAttributes(_module.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return ResolveCustomAttributes(_module.GetCustomAttributesData().Where(i => i.AttributeType == ((IkvmReflectionTypeSymbol)attributeType).ReflectionObject));
        }

        /// <inheritdoc />
        public CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return _module.IsDefined(((IkvmReflectionTypeSymbol)attributeType).ReflectionObject, false);
        }

        /// <summary>
        /// Sets the reflection type. Used by the builder infrastructure to complete a symbol.
        /// </summary>
        /// <param name="module"></param>
        internal void Complete(Module module)
        {
            ResolveModuleSymbol(_module = module);
            ContainingAssembly.Complete(_module.Assembly);
        }

    }

}
