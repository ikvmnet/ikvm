using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.CoreLib.Threading;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    /// <summary>
    /// Provides the metadata resolution implementation for a module.
    /// </summary>
    struct IkvmReflectionModuleMetadata
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

        readonly IIkvmReflectionModuleSymbol _symbol;

        IndexRangeDictionary<IIkvmReflectionTypeSymbol> _typeSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _typeLock;

        IndexRangeDictionary<IIkvmReflectionMethodBaseSymbol> _methodSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _methodLock;

        IndexRangeDictionary<IIkvmReflectionFieldSymbol> _fieldSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _fieldLock;

        IndexRangeDictionary<IIkvmReflectionPropertySymbol> _propertySymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _propertyLock;

        IndexRangeDictionary<IIkvmReflectionEventSymbol> _eventSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _eventLock;

        IndexRangeDictionary<IIkvmReflectionParameterSymbol> _parameterSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _parameterLock;

        IndexRangeDictionary<IIkvmReflectionTypeSymbol> _genericParameterSymbols = new(maxCapacity: MAX_CAPACITY);
        ReaderWriterLockSlim? _genericParameterLock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbol"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionModuleMetadata(IIkvmReflectionModuleSymbol symbol)
        {
            _symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionTypeSymbol"/> cached for the module by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IIkvmReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (type.Module.MetadataToken != _symbol.MetadataToken)
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
                var row = MetadataTokens.GetRowNumber(MetadataTokens.TypeDefinitionHandle(type.MetadataToken));
                if (_typeSymbols[row] == null)
                    using (_typeLock.CreateWriteLock())
                        if (type is TypeBuilder builder)
                            return _typeSymbols[row] ??= new IkvmReflectionTypeSymbolBuilder(_symbol.Context, _symbol, builder);
                        else
                            return _typeSymbols[row] ??= new IkvmReflectionTypeSymbol(_symbol.Context, _symbol, type);
                else
                    return _typeSymbols[row] ?? throw new InvalidOperationException();
            }
        }


        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionTypeSymbolBuilder"/> cached for the module by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IIkvmReflectionTypeSymbolBuilder GetOrCreateTypeSymbol(TypeBuilder type)
        {
            return (IIkvmReflectionTypeSymbolBuilder)GetOrCreateTypeSymbol((Type)type);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbol"/> for the specification type: array, pointer, generic, etc.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        IIkvmReflectionTypeSymbol GetOrCreateTypeSymbolForSpecification(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (type.Module.MetadataToken != _symbol.MetadataToken)
                throw new ArgumentException(nameof(type));

            if (type.GetElementType() is { } elementType)
            {
                var elementTypeSymbol = _symbol.ResolveTypeSymbol(elementType)!;

                // handles both SZ arrays and normal arrays
                if (type.IsArray)
                    return (IIkvmReflectionTypeSymbol)elementTypeSymbol.MakeArrayType(type.GetArrayRank());

                if (type.IsPointer)
                    return (IIkvmReflectionTypeSymbol)elementTypeSymbol.MakePointerType();

                if (type.IsByRef)
                    return (IIkvmReflectionTypeSymbol)elementTypeSymbol.MakeByRefType();

                throw new InvalidOperationException();
            }

            if (type.IsGenericType)
            {
                var definitionTypeSymbol = _symbol.GetOrCreateTypeSymbol(type.GetGenericTypeDefinition());
                return (IIkvmReflectionTypeSymbol)definitionTypeSymbol.MakeGenericType(definitionTypeSymbol.ResolveTypeSymbols(type.GetGenericArguments())!);
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IIkvmReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            if (method is null)
                throw new ArgumentNullException(nameof(method));
            if (method.Module.MetadataToken != _symbol.MetadataToken)
                throw new ArgumentException(nameof(method));

            // create lock on demand
            if (_methodLock == null)
                Interlocked.CompareExchange(ref _methodLock, new ReaderWriterLockSlim(), null);

            using (_methodLock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.MethodDefinitionHandle(method.MetadataToken));
                if (_methodSymbols[row] == null)
                {
                    using (_methodLock.CreateWriteLock())
                    {
                        if (method is ConstructorInfo c)
                        {
                            if (method is ConstructorBuilder builder)
                                return _methodSymbols[row] ??= new IkvmReflectionConstructorSymbolBuilder(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(c.DeclaringType!), builder);
                            else
                                return _methodSymbols[row] ??= new IkvmReflectionConstructorSymbol(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(c.DeclaringType!), c);
                        }
                        else if (method is MethodInfo m)
                        {
                            if (method is MethodBuilder builder)
                                return _methodSymbols[row] ??= new IkvmReflectionMethodSymbolBuilder(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(method.DeclaringType), builder);
                            else
                                return _methodSymbols[row] ??= new IkvmReflectionMethodSymbol(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(method.DeclaringType), m);
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
        /// Gets or creates the <see cref="IIkvmReflectionConstructorSymbol"/> cached for the type by ctor.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public IIkvmReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return (IIkvmReflectionConstructorSymbol)GetOrCreateMethodBaseSymbol(ctor);
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionConstructorSymbol"/> cached for the type by ctor.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public IIkvmReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            return (IIkvmReflectionConstructorSymbolBuilder)GetOrCreateConstructorSymbol((ConstructorInfo)ctor);
        }

        /// <summary>
        /// Gets or creates the <see cref="IMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IIkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            return (IIkvmReflectionMethodSymbol)GetOrCreateMethodBaseSymbol(method);
        }

        /// <summary>
        /// Gets or creates the <see cref="IMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IIkvmReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            return (IIkvmReflectionMethodSymbolBuilder)GetOrCreateMethodBaseSymbol((MethodInfo)method);
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionFieldSymbol"/> cached for the type by field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IIkvmReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));
            if (field.Module.MetadataToken != _symbol.MetadataToken)
                throw new ArgumentException(nameof(field));

            // create lock on demand
            if (_fieldLock == null)
                Interlocked.CompareExchange(ref _fieldLock, new ReaderWriterLockSlim(), null);

            using (_fieldLock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.FieldDefinitionHandle(field.MetadataToken));
                if (_fieldSymbols[row] == null)
                    using (_fieldLock.CreateWriteLock())
                        if (field is FieldBuilder builder)
                            return _fieldSymbols[row] ??= new IkvmReflectionFieldSymbolBuilder(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(field.DeclaringType), builder);
                        else
                            return _fieldSymbols[row] ??= new IkvmReflectionFieldSymbol(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(field.DeclaringType), field);
                else
                    return _fieldSymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionFieldSymbolBuilder"/> cached for the type by method.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IIkvmReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field)
        {
            return (IIkvmReflectionFieldSymbolBuilder)GetOrCreateFieldSymbol((FieldInfo)field);
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionPropertySymbol"/> cached for the type by property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IIkvmReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));
            if (property.Module.MetadataToken != _symbol.MetadataToken)
                throw new ArgumentException(nameof(property));

            // create lock on demand
            if (_propertyLock == null)
                Interlocked.CompareExchange(ref _propertyLock, new ReaderWriterLockSlim(), null);

            using (_propertyLock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.PropertyDefinitionHandle(property.MetadataToken));
                if (_propertySymbols[row] == null)
                    using (_propertyLock.CreateWriteLock())
                        if (property is PropertyBuilder builder)
                            return _propertySymbols[row] ??= new IkvmReflectionPropertySymbolBuilder(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(property.DeclaringType!), builder);
                        else
                            return _propertySymbols[row] ??= new IkvmReflectionPropertySymbol(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(property.DeclaringType!), property);
                else
                    return _propertySymbols[row] ?? throw new InvalidOperationException();
            }
        }


        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionPropertySymbolBuilder"/> cached for the type by property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IIkvmReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            return (IIkvmReflectionPropertySymbolBuilder)GetOrCreatePropertySymbol((PropertyInfo)property);
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionEventSymbol"/> cached for the type by event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public IIkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));
            if (@event.Module.MetadataToken != _symbol.MetadataToken)
                throw new ArgumentException(nameof(@event));

            // create lock on demand
            if (_eventLock == null)
                Interlocked.CompareExchange(ref _eventLock, new ReaderWriterLockSlim(), null);

            using (_eventLock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.EventDefinitionHandle(@event.MetadataToken));
                if (_eventSymbols[row] == null)
                    using (_eventLock.CreateWriteLock())
                        return _eventSymbols[row] ??= new IkvmReflectionEventSymbol(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(@event.DeclaringType!), @event);
                else
                    return _eventSymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionEventSymbolBuilder"/> cached for the type by event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public IIkvmReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event)
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));
            if (@event.Module.MetadataToken != _symbol.MetadataToken)
                throw new ArgumentException(nameof(@event));

            // create lock on demand
            if (_eventLock == null)
                Interlocked.CompareExchange(ref _eventLock, new ReaderWriterLockSlim(), null);

            using (_eventLock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.EventDefinitionHandle(@event.GetEventToken().Token));
                if (_eventSymbols[row] == null)
                    using (_eventLock.CreateWriteLock())
                        return (IIkvmReflectionEventSymbolBuilder)(_eventSymbols[row] ??= new IkvmReflectionEventSymbolBuilder(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(@event.DeclaringType), @event));
                else
                    return (IIkvmReflectionEventSymbolBuilder?)_eventSymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionParameterSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter.Member.Module.MetadataToken != _symbol.MetadataToken)
                throw new ArgumentException(nameof(parameter));

            // create lock on demand
            if (_parameterLock == null)
                Interlocked.CompareExchange(ref _parameterLock, new ReaderWriterLockSlim(), null);

            using (_parameterLock.CreateUpgradeableReadLock())
            {
                var position = parameter.Position;
                if (_parameterSymbols[position] == null)
                    using (_parameterLock.CreateWriteLock())
                        return _parameterSymbols[position] ??= new IkvmReflectionParameterSymbol(_symbol.Context, _symbol, _symbol.ResolveMethodBaseSymbol((MethodBase)parameter.Member), parameter);
                else
                    return _parameterSymbols[position] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionParameterSymbolBuilder"/> cached for the type by method.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIkvmReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter.Module.MetadataToken != _symbol.MetadataToken)
                throw new ArgumentException(nameof(parameter));

            // create lock on demand
            if (_parameterLock == null)
                Interlocked.CompareExchange(ref _parameterLock, new ReaderWriterLockSlim(), null);

            using (_parameterLock.CreateUpgradeableReadLock())
            {
                var position = parameter.Position;
                if (_parameterSymbols[position] == null)
                    using (_parameterLock.CreateWriteLock())
                        return (IIkvmReflectionParameterSymbolBuilder)(_parameterSymbols[position] ??= new IkvmReflectionParameterSymbolBuilder(_symbol.Context, _symbol, _symbol.ResolveMethodSymbol(parameter.Method), parameter));
                else
                    return (IIkvmReflectionParameterSymbolBuilder?)_parameterSymbols[position] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionTypeSymbol"/> cached for the module by type.
        /// </summary>
        /// <param name="genericParameterType"></param>
        /// <returns></returns>
        IIkvmReflectionTypeSymbol GetOrCreateGenericTypeParameterSymbol(Type genericParameterType)
        {
            if (genericParameterType is null)
                throw new ArgumentNullException(nameof(genericParameterType));
            if (genericParameterType.Module.MetadataToken != _symbol.MetadataToken)
                throw new ArgumentException(nameof(genericParameterType));

            // create lock on demand
            if (_genericParameterLock == null)
                Interlocked.CompareExchange(ref _genericParameterLock, new ReaderWriterLockSlim(), null);

            using (_genericParameterLock.CreateUpgradeableReadLock())
            {
                var hnd = MetadataTokens.GenericParameterHandle(genericParameterType.MetadataToken);
                var row = MetadataTokens.GetRowNumber(hnd);
                if (_genericParameterSymbols[row] == null)
                    using (_genericParameterLock.CreateWriteLock())
                        if (genericParameterType is GenericTypeParameterBuilder builder)
                            return _genericParameterSymbols[row] ??= new IkvmReflectionGenericTypeParameterSymbolBuilder(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(genericParameterType.DeclaringType), _symbol.ResolveMethodSymbol((MethodInfo?)genericParameterType.DeclaringMethod), builder);
                        else
                            return _genericParameterSymbols[row] ??= new IkvmReflectionGenericTypeParameterSymbol(_symbol.Context, _symbol, _symbol.ResolveTypeSymbol(genericParameterType.DeclaringType), _symbol.ResolveMethodSymbol((MethodInfo?)genericParameterType.DeclaringMethod), genericParameterType);
                else
                {
                    return _genericParameterSymbols[row] ?? throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionGenericTypeParameterSymbolBuilder"/> cached for the module.
        /// </summary>
        /// <param name="genericParameterType"></param>
        /// <returns></returns>
        IIkvmReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericParameterType)
        {
            return (IIkvmReflectionGenericTypeParameterSymbolBuilder)GetOrCreateGenericTypeParameterSymbol((Type)genericParameterType);
        }

    }

}
