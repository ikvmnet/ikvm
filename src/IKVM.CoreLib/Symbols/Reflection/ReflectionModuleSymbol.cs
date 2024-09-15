using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    /// <summary>
    /// Implementation of <see cref="IModuleSymbol"/> derived from System.Reflection.
    /// </summary>
    class ReflectionModuleSymbol : ReflectionSymbol, IModuleSymbol
    {

        /// <summary>
        /// Returns <c>true</c> if the given <see cref="Type"/> is a TypeDef. That is, not a modified or substituted or generic parameter type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static bool IsTypeDefinition(Type type)
        {
#if NET
            return type.IsTypeDefinition;
#else
            return type.HasElementType == false && type.IsConstructedGenericType == false && type.IsGenericParameter == false;
#endif
        }

        const int MAX_CAPACITY = 65536 * 2;

        const BindingFlags DefaultBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        readonly Module _module;

        IndexRangeDictionary<Type> _typeTable = new(maxCapacity: MAX_CAPACITY);
        IndexRangeDictionary<ReflectionTypeSymbol> _typeSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _typeLock;

        IndexRangeDictionary<MethodBase> _methodTable = new(maxCapacity: MAX_CAPACITY);
        IndexRangeDictionary<ReflectionMethodBaseSymbol> _methodSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _methodLock;

        IndexRangeDictionary<FieldInfo> _fieldTable = new(maxCapacity: MAX_CAPACITY);
        IndexRangeDictionary<ReflectionFieldSymbol> _fieldSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _fieldLock;

        IndexRangeDictionary<PropertyInfo> _propertyTable = new(maxCapacity: MAX_CAPACITY);
        IndexRangeDictionary<ReflectionPropertySymbol> _propertySymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _propertyLock;

        IndexRangeDictionary<EventInfo> _eventTable = new(maxCapacity: MAX_CAPACITY);
        IndexRangeDictionary<ReflectionEventSymbol> _eventSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _eventLock;

        IndexRangeDictionary<ParameterInfo> _parameterTable = new();
        IndexRangeDictionary<ReflectionParameterSymbol> _parameterSymbols = new();
        ReaderWriterLockSlim? _parameterLock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionModuleSymbol(ReflectionSymbolContext context, Module module) :
            base(context)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

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
        internal ReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            Debug.Assert(type.Module == _module);

            // type is not a definition, but is substituted
            if (IsTypeDefinition(type) == false)
                return GetOrCreateTypeSymbolForSpecification(type);

            // create lock on demand
            if (_typeLock == null)
                lock (this)
                    _typeLock ??= new ReaderWriterLockSlim();

            using (_typeLock.CreateUpgradeableReadLock())
            {
                var row = type.GetMetadataTokenRowNumberSafe();
                if (_typeTable[row] != type)
                    using (_typeLock.CreateWriteLock())
                        _typeTable[row] = type;

                if (_typeSymbols[row] == null)
                    using (_typeLock.CreateWriteLock())
                        return _typeSymbols[row] ??= new ReflectionTypeSymbol(Context, this, type);
                else
                    return _typeSymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates a <see cref="ReflectionTypeSymbol"/> for the specification type: array, pointer, etc.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        ReflectionTypeSymbol GetOrCreateTypeSymbolForSpecification(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            Debug.Assert(type.Module == _module);

            if (type.GetElementType() is { } elementType)
            {
                var elementTypeSymbol = ResolveTypeSymbol(elementType);

                // handles both SZ arrays and normal arrays
                if (type.IsArray)
                    return (ReflectionTypeSymbol)elementTypeSymbol.MakeArrayType(type.GetArrayRank());

                if (type.IsPointer)
                    return (ReflectionTypeSymbol)elementTypeSymbol.MakePointerType();

                if (type.IsByRef)
                    return (ReflectionTypeSymbol)elementTypeSymbol.MakeByRefType();

                throw new InvalidOperationException();
            }

            if (type.IsGenericType)
            {
                var definitionTypeSymbol = ResolveTypeSymbol(type.GetGenericTypeDefinition());
                return definitionTypeSymbol.GetOrCreateGenericTypeSymbol(type.GetGenericArguments());
            }

            // generic type parameter
            if (type.IsGenericParameter && type.DeclaringMethod is null && type.DeclaringType is not null)
            {
                var declaringType = ResolveTypeSymbol(type.DeclaringType);
                return declaringType.GetOrCreateGenericParameterSymbol(type);
            }

            // generic method parameter
            if (type.IsGenericParameter && type.DeclaringMethod is not null && type.DeclaringMethod.DeclaringType is not null)
            {
                var declaringMethod = ResolveTypeSymbol(type.DeclaringMethod.DeclaringType);
                return declaringMethod.GetOrCreateGenericParameterSymbol(type);
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionMethodSymbol"/> cached fqor the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal ReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            if (method is null)
                throw new ArgumentNullException(nameof(method));

            Debug.Assert(method.Module == _module);

            // create lock on demand
            if (_methodLock == null)
                lock (this)
                    _methodLock ??= new ReaderWriterLockSlim();

            using (_methodLock.CreateUpgradeableReadLock())
            {
                var row = method.GetMetadataTokenRowNumberSafe();
                if (_methodTable[row] != method)
                    using (_methodLock.CreateWriteLock())
                        _methodTable[row] = method;

                if (_methodSymbols[row] == null)
                    using (_methodLock.CreateWriteLock())
                        if (method is ConstructorInfo c)
                            return _methodSymbols[row] ??= new ReflectionConstructorSymbol(Context, ResolveTypeSymbol(c.DeclaringType ?? throw new InvalidOperationException()), c);
                        else if (method is MethodInfo m)
                            if (method.DeclaringType is { } dt)
                                return _methodSymbols[row] ??= new ReflectionMethodSymbol(Context, ResolveTypeSymbol(dt), m);
                            else
                                return _methodSymbols[row] ??= new ReflectionMethodSymbol(Context, this, m);
                        else
                            throw new InvalidOperationException();
                else
                    return _methodSymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionConstructorSymbol"/> cached for the type by ctor.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal ReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return (ReflectionConstructorSymbol)GetOrCreateMethodBaseSymbol(ctor);
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal ReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            return (ReflectionMethodSymbol)GetOrCreateMethodBaseSymbol(method);
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionFieldSymbol"/> cached for the type by field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal ReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
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
                var row = field.GetMetadataTokenRowNumberSafe();
                if (_fieldTable[row] != field)
                    using (_fieldLock.CreateWriteLock())
                        _fieldTable[row] = field;

                if (_fieldSymbols[row] == null)
                    using (_fieldLock.CreateWriteLock())
                        if (field.DeclaringType is { } dt)
                            return _fieldSymbols[row] ??= new ReflectionFieldSymbol(Context, ResolveTypeSymbol(dt), field);
                        else
                            return _fieldSymbols[row] ??= new ReflectionFieldSymbol(Context, this, field);
                else
                    return _fieldSymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionPropertySymbol"/> cached for the type by property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal ReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
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
                var row = property.GetMetadataTokenRowNumberSafe();
                if (_propertyTable[row] != property)
                    using (_propertyLock.CreateWriteLock())
                        _propertyTable[row] = property;

                if (_propertySymbols[row] == null)
                    using (_propertyLock.CreateWriteLock())
                        return _propertySymbols[row] ??= new ReflectionPropertySymbol(Context, ResolveTypeSymbol(property.DeclaringType!), property);
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
        internal ReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
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
                var row = @event.GetMetadataTokenRowNumberSafe();
                if (_eventTable[row] is not EventInfo i || i != @event)
                    using (_eventLock.CreateWriteLock())
                        _eventTable[row] = @event;

                if (_eventSymbols[row] == null)
                    using (_eventLock.CreateWriteLock())
                        return _eventSymbols[row] ??= new ReflectionEventSymbol(Context, ResolveTypeSymbol(@event.DeclaringType!), @event);
                else
                    return _eventSymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal ReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
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
                        return _parameterSymbols[position] ??= new ReflectionParameterSymbol(Context, ResolveMethodBaseSymbol((MethodBase)parameter.Member), parameter);
                else
                    return _parameterSymbols[position] ?? throw new InvalidOperationException();
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
        public IFieldSymbol? GetField(string name, BindingFlags bindingAttr)
        {
            return _module.GetField(name, bindingAttr) is { } f ? ResolveFieldSymbol(f) : null;
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields(BindingFlags bindingFlags)
        {
            return ResolveFieldSymbols(_module.GetFields(bindingFlags));
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
        public IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, CallingConventions callConvention, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            return _module.GetMethod(name, bindingAttr, null, callConvention, types.Unpack(), modifiers) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods()
        {
            return ResolveMethodSymbols(_module.GetMethods());
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods(BindingFlags bindingFlags)
        {
            return ResolveMethodSymbols(_module.GetMethods(bindingFlags));
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
            return ResolveCustomAttributes(_module.GetCustomAttributesData().Where(i => i.AttributeType == ((ReflectionTypeSymbol)attributeType).ReflectionObject));
        }

        /// <inheritdoc />
        public CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return _module.IsDefined(((ReflectionTypeSymbol)attributeType).ReflectionObject, false);
        }

    }

}
