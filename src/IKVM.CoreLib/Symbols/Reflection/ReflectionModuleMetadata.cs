using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Emit;
using IKVM.CoreLib.Symbols.Reflection.Emit;
using IKVM.CoreLib.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    /// <summary>
    /// Provides the metadata resolution implementation for a module.
    /// </summary>
    struct ReflectionModuleMetadata
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

        readonly IReflectionModuleSymbol _symbol;

        IndexRangeDictionary<IReflectionTypeSymbol> _typeSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _typeLock;

        IndexRangeDictionary<IReflectionMethodBaseSymbol> _methodSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _methodLock;

        IndexRangeDictionary<IReflectionFieldSymbol> _fieldSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _fieldLock;

        IndexRangeDictionary<IReflectionPropertySymbol> _propertySymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _propertyLock;

        IndexRangeDictionary<IReflectionEventSymbol> _eventSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _eventLock;

        IndexRangeDictionary<IReflectionParameterSymbol> _parameterSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _parameterLock;

        IndexRangeDictionary<IReflectionTypeSymbol> _genericParameterSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _genericParameterLock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbol"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionModuleMetadata(IReflectionModuleSymbol symbol)
        {
            _symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionTypeSymbol"/> cached for the module by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (type.Module.GetMetadataTokenSafe() != _symbol.MetadataToken)
                throw new ArgumentException(nameof(type));

            // type is a generic parameter (GenericParam)
            if (type.IsGenericParameter)
                return GetOrCreateGenericTypeParameterSymbol(type);

            // type is not a type definition (TypeDef)
            if (IsTypeDefinition(type) == false)
                return GetOrCreateTypeSymbolForSpecification(type);

            // create lock on demand
            if (_typeLock == null)
                Interlocked.CompareExchange(ref _typeLock, new ReaderWriterLockSlim(), null);

            using (_typeLock.CreateUpgradeableReadLock())
            {
                var row = type.GetMetadataTokenRowNumberSafe();
                if (_typeSymbols[row] == null)
                    using (_typeLock.CreateWriteLock())
                        if (type is TypeBuilder builder)
                            return _typeSymbols[row] ??= new ReflectionTypeSymbolBuilder(_symbol.Context, _symbol, builder);
                        else
                            return _typeSymbols[row] ??= new ReflectionTypeSymbol(_symbol.Context, _symbol, type);
                else
                    return _typeSymbols[row] ?? throw new InvalidOperationException();
            }
        }


        /// <summary>
        /// Gets or creates the <see cref="IReflectionTypeSymbolBuilder"/> cached for the module by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IReflectionTypeSymbolBuilder GetOrCreateTypeSymbol(TypeBuilder type)
        {
            return (IReflectionTypeSymbolBuilder)GetOrCreateTypeSymbol((Type)type);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbol"/> for the specification type: array, pointer, generic, etc.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        IReflectionTypeSymbol GetOrCreateTypeSymbolForSpecification(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (type.Module.GetMetadataTokenSafe() != _symbol.MetadataToken)
                throw new ArgumentException(nameof(type));

            if (type.GetElementType() is { } elementType)
            {
                var elementTypeSymbol = _symbol.ResolveTypeSymbol(elementType)!;

                // handles both SZ arrays and normal arrays
                if (type.IsArray)
                    return (IReflectionTypeSymbol)elementTypeSymbol.MakeArrayType(type.GetArrayRank());

                if (type.IsPointer)
                    return (IReflectionTypeSymbol)elementTypeSymbol.MakePointerType();

                if (type.IsByRef)
                    return (IReflectionTypeSymbol)elementTypeSymbol.MakeByRefType();

                throw new InvalidOperationException();
            }

            if (type.IsGenericType)
            {
                var definitionTypeSymbol = _symbol.GetOrCreateTypeSymbol(type.GetGenericTypeDefinition());
                return (IReflectionTypeSymbol)definitionTypeSymbol.MakeGenericType(definitionTypeSymbol.ResolveTypeSymbols(type.GetGenericArguments())!);
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            if (method is null)
                throw new ArgumentNullException(nameof(method));
            if (method.Module.GetMetadataTokenSafe() != _symbol.MetadataToken)
                throw new ArgumentException(nameof(method));

            // they are methods, but they are associated differently
            if (method is DynamicMethod)
                throw new ArgumentException("Dynamic methods cannot be attached to context.");

            // create lock on demand
            if (_methodLock == null)
                Interlocked.CompareExchange(ref _methodLock, new ReaderWriterLockSlim(), null);

            using (_methodLock.CreateUpgradeableReadLock())
            {
                var row = method.GetMetadataTokenRowNumberSafe();
                if (_methodSymbols[row] == null)
                {
                    using (_methodLock.CreateWriteLock())
                    {
                        if (method is ConstructorInfo c)
                        {
                            if (method is ConstructorBuilder builder)
                                return _methodSymbols[row] ??= new ReflectionConstructorSymbolBuilder(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(c.DeclaringType!), builder);
                            else
                                return _methodSymbols[row] ??= new ReflectionConstructorSymbol(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(c.DeclaringType!), c);
                        }
                        else if (method is MethodInfo m)
                        {
                            if (method is MethodBuilder builder)
                                return _methodSymbols[row] ??= new ReflectionMethodSymbolBuilder(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(method.DeclaringType), builder);
                            else
                                return _methodSymbols[row] ??= new ReflectionMethodSymbol(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(method.DeclaringType), m);
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                }
                else
                {
                    return _methodSymbols[row] ?? throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionConstructorSymbol"/> cached for the type by ctor.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public IReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return (IReflectionConstructorSymbol)GetOrCreateMethodBaseSymbol(ctor);
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionConstructorSymbol"/> cached for the type by ctor.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public IReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            return (IReflectionConstructorSymbolBuilder)GetOrCreateConstructorSymbol((ConstructorInfo)ctor);
        }

        /// <summary>
        /// Gets or creates the <see cref="IMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            return (IReflectionMethodSymbol)GetOrCreateMethodBaseSymbol(method);
        }

        /// <summary>
        /// Gets or creates the <see cref="IMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            return (IReflectionMethodSymbolBuilder)GetOrCreateMethodBaseSymbol((MethodInfo)method);
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionFieldSymbol"/> cached for the type by field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));
            if (field.Module.GetMetadataTokenSafe() != _symbol.MetadataToken)
                throw new ArgumentException(nameof(field));

            // create lock on demand
            if (_fieldLock == null)
                Interlocked.CompareExchange(ref _fieldLock, new ReaderWriterLockSlim(), null);

            using (_fieldLock.CreateUpgradeableReadLock())
            {
                var row = field.GetMetadataTokenRowNumberSafe();
                if (_fieldSymbols[row] == null)
                    using (_fieldLock.CreateWriteLock())
                        if (field is FieldBuilder builder)
                            return _fieldSymbols[row] ??= new ReflectionFieldSymbolBuilder(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(field.DeclaringType), builder);
                        else
                            return _fieldSymbols[row] ??= new ReflectionFieldSymbol(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(field.DeclaringType), field);
                else
                    return _fieldSymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionFieldSymbolBuilder"/> cached for the type by method.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field)
        {
            return (IReflectionFieldSymbolBuilder)GetOrCreateFieldSymbol((FieldInfo)field);
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionPropertySymbol"/> cached for the type by property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));
            if (property.Module.GetMetadataTokenSafe() != _symbol.MetadataToken)
                throw new ArgumentException(nameof(property));

            // create lock on demand
            if (_propertyLock == null)
                Interlocked.CompareExchange(ref _propertyLock, new ReaderWriterLockSlim(), null);

            using (_propertyLock.CreateUpgradeableReadLock())
            {
                var row = property.GetMetadataTokenRowNumberSafe();
                if (_propertySymbols[row] == null)
                    using (_propertyLock.CreateWriteLock())
                        if (property is PropertyBuilder builder)
                            return _propertySymbols[row] ??= new ReflectionPropertySymbolBuilder(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(property.DeclaringType!), builder);
                        else
                            return _propertySymbols[row] ??= new ReflectionPropertySymbol(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(property.DeclaringType!), property);
                else
                    return _propertySymbols[row] ?? throw new InvalidOperationException();
            }
        }


        /// <summary>
        /// Gets or creates the <see cref="IReflectionPropertySymbolBuilder"/> cached for the type by property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            return (IReflectionPropertySymbolBuilder)GetOrCreatePropertySymbol((PropertyInfo)property);
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionEventSymbol"/> cached for the type by event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public IReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));
            if (@event.Module.GetMetadataTokenSafe() != _symbol.MetadataToken)
                throw new ArgumentException(nameof(@event));

            // create lock on demand
            if (_eventLock == null)
                Interlocked.CompareExchange(ref _eventLock, new ReaderWriterLockSlim(), null);

            using (_eventLock.CreateUpgradeableReadLock())
            {
                var row = @event.GetMetadataTokenRowNumberSafe();
                if (_eventSymbols[row] == null)
                    using (_eventLock.CreateWriteLock())
                        return _eventSymbols[row] ??= new ReflectionEventSymbol(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(@event.DeclaringType!), @event);
                else
                    return _eventSymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionEventSymbolBuilder"/> cached for the type by event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public IReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event)
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));
            if (@event.GetModuleBuilder().GetMetadataTokenSafe() != _symbol.MetadataToken)
                throw new ArgumentException(nameof(@event));

            // create lock on demand
            if (_eventLock == null)
                Interlocked.CompareExchange(ref _eventLock, new ReaderWriterLockSlim(), null);

            using (_eventLock.CreateUpgradeableReadLock())
            {
                var row = @event.GetMetadataTokenRowNumberSafe();
                if (_eventSymbols[row] == null)
                    using (_eventLock.CreateWriteLock())
                        return (IReflectionEventSymbolBuilder)(_eventSymbols[row] ??= new ReflectionEventSymbolBuilder(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(@event.GetTypeBuilder()), @event));
                else
                    return (IReflectionEventSymbolBuilder?)_eventSymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionParameterSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter.Member.Module.GetMetadataTokenSafe() != _symbol.MetadataToken)
                throw new ArgumentException(nameof(parameter));

            // create lock on demand
            if (_parameterLock == null)
                Interlocked.CompareExchange(ref _parameterLock, new ReaderWriterLockSlim(), null);

            using (_parameterLock.CreateUpgradeableReadLock())
            {
                var position = parameter.Position;
                if (_parameterSymbols[position] == null)
                    using (_parameterLock.CreateWriteLock())
                        return _parameterSymbols[position] ??= new ReflectionParameterSymbol(_symbol.Context, _symbol, _symbol.ResolveMethodBaseSymbol((MethodBase)parameter.Member), parameter);
                else
                    return _parameterSymbols[position] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionParameterSymbolBuilder"/> cached for the type by method.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter.GetModuleBuilder().GetMetadataTokenSafe() != _symbol.MetadataToken)
                throw new ArgumentException(nameof(parameter));

            // create lock on demand
            if (_parameterLock == null)
                Interlocked.CompareExchange(ref _parameterLock, new ReaderWriterLockSlim(), null);

            using (_parameterLock.CreateUpgradeableReadLock())
            {
                var position = parameter.Position;
                if (_parameterSymbols[position] == null)
                    using (_parameterLock.CreateWriteLock())
                        return (IReflectionParameterSymbolBuilder)(_parameterSymbols[position] ??= new ReflectionParameterSymbolBuilder(_symbol.Context, _symbol, _symbol.ResolveMethodSymbol(parameter.GetMethodBuilder()), parameter));
                else
                    return (IReflectionParameterSymbolBuilder?)_parameterSymbols[position] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionTypeSymbol"/> cached for the module by type.
        /// </summary>
        /// <param name="genericParameterType"></param>
        /// <returns></returns>
        IReflectionTypeSymbol GetOrCreateGenericTypeParameterSymbol(Type genericParameterType)
        {
            if (genericParameterType is null)
                throw new ArgumentNullException(nameof(genericParameterType));
            if (genericParameterType.Module.GetMetadataTokenSafe() != _symbol.MetadataToken)
                throw new ArgumentException(nameof(genericParameterType));

            // create lock on demand
            if (_genericParameterLock == null)
                Interlocked.CompareExchange(ref _genericParameterLock, new ReaderWriterLockSlim(), null);

            using (_genericParameterLock.CreateUpgradeableReadLock())
            {
                var hnd = MetadataTokens.GenericParameterHandle(genericParameterType.GetMetadataTokenSafe());
                var row = MetadataTokens.GetRowNumber(hnd);
                if (_genericParameterSymbols[row] == null)
                    using (_genericParameterLock.CreateWriteLock())
                        if (genericParameterType is GenericTypeParameterBuilder builder)
                            return _genericParameterSymbols[row] ??= new ReflectionGenericTypeParameterSymbolBuilder(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(genericParameterType.DeclaringType), _symbol.ResolveMethodSymbol((MethodInfo?)genericParameterType.DeclaringMethod), builder);
                        else
                            return _genericParameterSymbols[row] ??= new ReflectionGenericTypeParameterSymbol(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(genericParameterType.DeclaringType), _symbol.ResolveMethodSymbol((MethodInfo?)genericParameterType.DeclaringMethod), genericParameterType);
                else
                {
                    return _genericParameterSymbols[row] ?? throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionGenericTypeParameterSymbolBuilder"/> cached for the module.
        /// </summary>
        /// <param name="genericParameterType"></param>
        /// <returns></returns>
        IReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericParameterType)
        {
            return (IReflectionGenericTypeParameterSymbolBuilder)GetOrCreateGenericTypeParameterSymbol((Type)genericParameterType);
        }

    }

}
