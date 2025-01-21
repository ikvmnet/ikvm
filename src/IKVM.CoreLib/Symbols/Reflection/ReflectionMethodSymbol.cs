using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionMethodSymbol : ReflectionMethodBaseSymbol, IMethodSymbol
    {

        readonly MethodInfo _underlyingMethod;

        Type[]? _genericParametersSource;
        ReflectionTypeSymbol?[]? _genericParameters;
        List<(Type[] Arguments, ReflectionMethodSymbol Symbol)>? _genericTypes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="underlyingMethod"></param>
        public ReflectionMethodSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol module, ReflectionTypeSymbol? type, MethodInfo underlyingMethod) :
            base(context, module, type, underlyingMethod)
        {
            _underlyingMethod = underlyingMethod ?? throw new ArgumentNullException(nameof(underlyingMethod));
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionTypeSymbol"/> cached for the generic parameter type.
        /// </summary>
        /// <param name="genericParameterType"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal ReflectionTypeSymbol GetOrCreateGenericParameterSymbol(Type genericParameterType)
        {
            if (genericParameterType is null)
                throw new ArgumentNullException(nameof(genericParameterType));

            Debug.Assert(genericParameterType.DeclaringMethod == _underlyingMethod);

            // initialize tables
            _genericParametersSource ??= _underlyingMethod.GetGenericArguments();
            _genericParameters ??= new ReflectionTypeSymbol?[_genericParametersSource.Length];

            // position of the parameter in the list
            var idx = genericParameterType.GenericParameterPosition;

            // check that our list is long enough to contain the entire table
            if (_genericParameters.Length < idx)
                throw new IndexOutOfRangeException();

            // if not yet created, create, allow multiple instances, but only one is eventually inserted
            ref var rec = ref _genericParameters[idx];
            if (rec == null)
                Interlocked.CompareExchange(ref rec, new ReflectionTypeSymbol(Context, ContainingModule, genericParameterType), null);

            // this should never happen
            if (rec is not ReflectionTypeSymbol sym)
                throw new InvalidOperationException();

            return sym;
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionMethodSymbol"/> cached for the generic parameter type.
        /// </summary>
        /// <param name="genericMethodArguments"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal ReflectionMethodSymbol GetOrCreateGenericTypeSymbol(Type[] genericMethodArguments)
        {
            if (genericMethodArguments is null)
                throw new ArgumentNullException(nameof(genericMethodArguments));

            if (_underlyingMethod.IsGenericMethodDefinition == false)
                throw new InvalidOperationException();

            // initialize the available parameters
            if (_genericParametersSource == null)
                Interlocked.CompareExchange(ref _genericParametersSource, _underlyingMethod.GetGenericArguments(), null);
            if (_genericParametersSource.Length != genericMethodArguments.Length)
                throw new InvalidOperationException();

            // initialize generic type map, and lock on it since we're potentially adding items
            if (_genericTypes == null)
                Interlocked.CompareExchange(ref _genericTypes, [], null);

            lock (_genericTypes)
            {
                // find existing entry
                foreach (var i in _genericTypes)
                    if (i.Arguments.SequenceEqual(genericMethodArguments))
                        return i.Symbol;

                // generate new symbol
                var sym = new ReflectionMethodSymbol(Context, ContainingModule, ContainingType, _underlyingMethod.MakeGenericMethod(genericMethodArguments));
                _genericTypes.Add((genericMethodArguments, sym));
                return sym;
            }
        }

        /// <summary>
        /// Gets the underlying <see cref="MethodBase"/> wrapped by this symbol.
        /// </summary>
        internal MethodInfo UnderlyingMethod => _underlyingMethod;

        /// <inheritdoc />
        public IParameterSymbol ReturnParameter => ResolveParameterSymbol(_underlyingMethod.ReturnParameter);

        /// <inheritdoc />
        public ITypeSymbol ReturnType => ResolveTypeSymbol(_underlyingMethod.ReturnType);

        /// <inheritdoc />
        public ICustomAttributeSymbolProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

        /// <inheritdoc />
        public IMethodSymbol GetBaseDefinition()
        {
            return ResolveMethodSymbol(_underlyingMethod.GetBaseDefinition());
        }

        /// <inheritdoc />
        public IMethodSymbol GetGenericMethodDefinition()
        {
            return ResolveMethodSymbol(_underlyingMethod.GetGenericMethodDefinition());
        }

        /// <inheritdoc />
        public IMethodSymbol MakeGenericMethod(params ITypeSymbol[] typeArguments)
        {
            return ResolveMethodSymbol(_underlyingMethod.MakeGenericMethod(UnpackTypeSymbols(typeArguments)));
        }

    }

}