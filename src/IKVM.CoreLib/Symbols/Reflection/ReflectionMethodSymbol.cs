using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionMethodSymbol : ReflectionMethodBaseSymbol, IReflectionMethodSymbol
    {

        readonly MethodInfo _method;

        ReflectionGenericTypeParameterTable _genericTypeParameterTable;
        ReflectionMethodSpecTable _specTable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        public ReflectionMethodSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol module, IReflectionTypeSymbol? type, MethodInfo method) :
            base(context, module, type)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
            _genericTypeParameterTable = new ReflectionGenericTypeParameterTable(context, module, this);
            _specTable = new ReflectionMethodSpecTable(context, module, type, this);
        }

        /// <inheritdoc />
        public MethodInfo UnderlyingMethod => _method;

        /// <inheritdoc />
        public override MethodBase UnderlyingMethodBase => UnderlyingMethod;

        #region IReflectionMethodSymbol

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreateGenericTypeParameterSymbol(Type genericTypeParameter)
        {
            return _genericTypeParameterTable.GetOrCreateGenericTypeParameterSymbol(genericTypeParameter);
        }

        /// <inheritdoc />
        public IReflectionMethodSymbol GetOrCreateGenericMethodSymbol(MethodInfo method)
        {
            return _specTable.GetOrCreateGenericMethodSymbol(ResolveTypeSymbols(method.GetGenericArguments()));
        }

        #endregion

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
