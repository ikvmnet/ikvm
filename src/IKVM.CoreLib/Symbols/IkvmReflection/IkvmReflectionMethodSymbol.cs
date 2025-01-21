using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using MethodInfo = IKVM.Reflection.MethodInfo;
using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionMethodSymbol : IkvmReflectionMethodBaseSymbol, IMethodSymbol
    {

        readonly MethodInfo _underlyingMethod;

        Type[]? _genericParametersSource;
        IkvmReflectionTypeSymbol?[]? _genericParameters;
        List<(Type[] Arguments, IkvmReflectionMethodSymbol Symbol)>? _genericTypes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="underlyingMethod"></param>
        public IkvmReflectionMethodSymbol(IkvmReflectionSymbolContext context, IkvmReflectionModuleSymbol module, IkvmReflectionTypeSymbol? type, MethodInfo underlyingMethod) :
            base(context, module, type, underlyingMethod)
        {
            _underlyingMethod = underlyingMethod ?? throw new ArgumentNullException(nameof(underlyingMethod));
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionTypeSymbol"/> cached for the generic parameter type.
        /// </summary>
        /// <param name="genericParameterType"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal IkvmReflectionTypeSymbol GetOrCreateGenericParameterSymbol(Type genericParameterType)
        {
            if (genericParameterType is null)
                throw new ArgumentNullException(nameof(genericParameterType));

            Debug.Assert(genericParameterType.DeclaringMethod == _underlyingMethod);

            // initialize tables
            _genericParametersSource ??= _underlyingMethod.GetGenericArguments();
            _genericParameters ??= new IkvmReflectionTypeSymbol?[_genericParametersSource.Length];

            // position of the parameter in the list
            var idx = genericParameterType.GenericParameterPosition;

            // check that our list is long enough to contain the entire table
            if (_genericParameters.Length < idx)
                throw new IndexOutOfRangeException();

            // if not yet created, create, allow multiple instances, but only one is eventually inserted
            ref var rec = ref _genericParameters[idx];
            if (rec == null)
                Interlocked.CompareExchange(ref rec, new IkvmReflectionTypeSymbol(Context, ContainingModule, genericParameterType), null);

            // this should never happen
            if (rec is not IkvmReflectionTypeSymbol sym)
                throw new InvalidOperationException();

            return sym;
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionMethodSymbol"/> cached for the generic parameter type.
        /// </summary>
        /// <param name="genericMethodArguments"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal IkvmReflectionMethodSymbol GetOrCreateGenericTypeSymbol(Type[] genericMethodArguments)
        {
            if (genericMethodArguments is null)
                throw new ArgumentNullException(nameof(genericMethodArguments));

            if (_underlyingMethod.IsGenericMethodDefinition == false)
                throw new InvalidOperationException();

            // initialize the available parameters
            _genericParametersSource ??= _underlyingMethod.GetGenericArguments();
            if (_genericParametersSource.Length != genericMethodArguments.Length)
                throw new InvalidOperationException();

            // initialize generic type map, and lock on it since we're potentially adding items
            _genericTypes ??= [];
            lock (_genericTypes)
            {
                // find existing entry
                foreach (var i in _genericTypes)
                    if (i.Arguments.SequenceEqual(genericMethodArguments))
                        return i.Symbol;

                // generate new symbol
                var sym = new IkvmReflectionMethodSymbol(Context, ContainingModule, ContainingType, _underlyingMethod.MakeGenericMethod(genericMethodArguments));
                _genericTypes.Add((genericMethodArguments, sym));
                return sym;
            }
        }

        internal MethodInfo UnderlyingMethod => _underlyingMethod;

        public IParameterSymbol ReturnParameter => ResolveParameterSymbol(_underlyingMethod.ReturnParameter);

        public ITypeSymbol ReturnType => ResolveTypeSymbol(_underlyingMethod.ReturnType);

        public ICustomAttributeSymbolProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

        public IMethodSymbol GetBaseDefinition()
        {
            return ResolveMethodSymbol(_underlyingMethod.GetBaseDefinition());
        }

        public IMethodSymbol GetGenericMethodDefinition()
        {
            return ResolveMethodSymbol(_underlyingMethod.GetGenericMethodDefinition());
        }

        public IMethodSymbol MakeGenericMethod(params ITypeSymbol[] typeArguments)
        {
            return ResolveMethodSymbol(_underlyingMethod.MakeGenericMethod(UnpackTypeSymbols(typeArguments)));
        }

    }

}