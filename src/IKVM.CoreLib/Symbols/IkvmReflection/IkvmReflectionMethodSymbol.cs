using System;

using MethodInfo = IKVM.Reflection.MethodInfo;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionMethodSymbol : IkvmReflectionMethodBaseSymbol, IMethodSymbol
    {

        readonly MethodInfo _method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="method"></param>
        public IkvmReflectionMethodSymbol(IkvmReflectionSymbolContext context, IkvmReflectionModuleSymbol module, MethodInfo method) :
            base(context, module, method)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        public IkvmReflectionMethodSymbol(IkvmReflectionSymbolContext context, IkvmReflectionTypeSymbol type, MethodInfo method) :
            this(context, type.ContainingModule, method)
        {

        }

        /// <summary>
        /// Gets the underlying <see cref="MethodBase"/> wrapped by this symbol.
        /// </summary>
        internal new MethodInfo ReflectionObject => _method;

        /// <inheritdoc />
        public IParameterSymbol ReturnParameter => ResolveParameterSymbol(_method.ReturnParameter);

        /// <inheritdoc />
        public ITypeSymbol ReturnType => ResolveTypeSymbol(_method.ReturnType);

        /// <inheritdoc />
        public ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

        /// <inheritdoc />
        public IMethodSymbol GetBaseDefinition()
        {
            return ResolveMethodSymbol(_method.GetBaseDefinition());
        }

        /// <inheritdoc />
        public IMethodSymbol GetGenericMethodDefinition()
        {
            return ResolveMethodSymbol(_method.GetGenericMethodDefinition());
        }

        /// <inheritdoc />
        public IMethodSymbol MakeGenericMethod(params ITypeSymbol[] typeArguments)
        {
            return ResolveMethodSymbol(_method.MakeGenericMethod(typeArguments.Unpack()));
        }

    }

}