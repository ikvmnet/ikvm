using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionMethodSymbol : ReflectionMethodBaseSymbol, IReflectionMethodSymbol
    {

        readonly MethodInfo _method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        public ReflectionMethodSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol module, IReflectionTypeSymbol? type, MethodInfo method) :
            base(context, module, type, method)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
        }

        /// <summary>
        /// Gets the underlying <see cref="MethodInfo"/> wrapped by this symbol.
        /// </summary>
        public MethodInfo UnderlyingMethod => _method;

        #region IMethodSymbol

        /// <inheritdoc />
        public IParameterSymbol ReturnParameter => ResolveParameterSymbol(UnderlyingMethod.ReturnParameter);

        /// <inheritdoc />
        public ITypeSymbol ReturnType => ResolveTypeSymbol(UnderlyingMethod.ReturnType);

        /// <inheritdoc />
        public ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

        /// <inheritdoc />
        public IMethodSymbol GetBaseDefinition()
        {
            return ResolveMethodSymbol(UnderlyingMethod.GetBaseDefinition());
        }

        /// <inheritdoc />
        public IMethodSymbol GetGenericMethodDefinition()
        {
            return ResolveMethodSymbol(UnderlyingMethod.GetGenericMethodDefinition());
        }

        /// <inheritdoc />
        public IMethodSymbol MakeGenericMethod(params ITypeSymbol[] typeArguments)
        {
            return ResolveMethodSymbol(UnderlyingMethod.MakeGenericMethod(typeArguments.Unpack()));
        }

        #endregion

    }

}
