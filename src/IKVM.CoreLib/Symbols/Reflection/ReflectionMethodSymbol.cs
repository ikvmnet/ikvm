using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionMethodSymbol : ReflectionMethodBaseSymbol, IReflectionMethodSymbol
    {

        readonly MethodInfo _method;

        ReflectionGenericTypeParameterTable _genericTypeParameterTable;
        ReflectionMethodSpecTable _specTable;

        ReflectionParameterSpecInfo[]? _specParameters;
        ReflectionParameterSpecInfo? _specReturnParameter;

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
        public virtual MethodInfo UnderlyingMethod => _method;

        /// <inheritdoc />
        public virtual MethodInfo UnderlyingRuntimeMethod => GetUnderlyingRuntimeMethod();

        /// <summary>
        /// Gets the underlying runtime method.
        /// </summary>
        /// <returns></returns>
        MethodInfo GetUnderlyingRuntimeMethod()
        {
            if (ReflectionExtensions.MethodOnTypeBuilderInstantiationType.IsInstanceOfType(_method))
                return GetUnderlyingRuntimeMethodForMethodOnTypeBuilderInstantiation();
            else if (ReflectionExtensions.MethodBuilderInstantiationType.IsInstanceOfType(_method))
                return GetUnderlyingRuntimeMethodForMethodBuilderInstantiationType();
            else
                return _method;
        }

        MethodInfo GetUnderlyingRuntimeMethodForMethodOnTypeBuilderInstantiation()
        {
            return _method;
        }

        MethodInfo GetUnderlyingRuntimeMethodForMethodBuilderInstantiationType()
        {
            return _method;
        }

        /// <inheritdoc />
        public override MethodBase UnderlyingMethodBase => UnderlyingMethod;

        /// <inheritdoc />
        public override MethodBase UnderlyingRuntimeMethodBase => UnderlyingRuntimeMethod;

        #region IReflectionMethodSymbol

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreateGenericTypeParameterSymbol(Type genericTypeParameter)
        {
            return _genericTypeParameterTable.GetOrCreateGenericTypeParameterSymbol(genericTypeParameter);
        }

        /// <inheritdoc />
        public IReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericTypeParameter)
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
        public IParameterSymbol ReturnParameter => GetReturnParameter();

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        /// <returns></returns>
        IParameterSymbol GetReturnParameter()
        {
            if (ReflectionExtensions.MethodOnTypeBuilderInstantiationType.IsInstanceOfType(_method))
                return GetReturnParameterForMethodOnTypeBuilderInstantiation();
            else if (IsComplete)
                return ResolveParameterSymbol(UnderlyingRuntimeMethod.ReturnParameter);
            else
                return ResolveParameterSymbol(UnderlyingMethod.ReturnParameter);
        }

        /// <summary>
        /// Gets the return parameter in the case that this method is a <see cref="MethodOnTypeBuilderInstantiation"/>.
        /// </summary>
        /// <returns></returns>
        IParameterSymbol GetReturnParameterForMethodOnTypeBuilderInstantiation()
        {
            if (_specReturnParameter == null)
                Interlocked.CompareExchange(ref _specReturnParameter, new ReflectionParameterSpecInfo(_method, _method.ReturnParameter, _method.DeclaringType!.GetGenericArguments()), null);

            return ResolveParameterSymbol(_specReturnParameter);
        }

        /// <inheritdoc />
        public ITypeSymbol ReturnType => GetReturnType();

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        /// <returns></returns>
        ITypeSymbol GetReturnType()
        {
            if (ReflectionExtensions.MethodOnTypeBuilderInstantiationType.IsInstanceOfType(_method))
                return GetReturnTypeForMethodOnTypeBuilderInstantiation();
            else
                return ResolveTypeSymbol(UnderlyingMethod.ReturnType);
        }

        /// <summary>
        /// Gets the return type in the case that this method is a <see cref="MethodOnTypeBuilderInstantiation"/>.
        /// </summary>
        /// <returns></returns>
        ITypeSymbol GetReturnTypeForMethodOnTypeBuilderInstantiation()
        {
            return ResolveTypeSymbol(_method.ReturnType.SubstituteGenericTypes(_method.DeclaringType!.GetGenericArguments()));
        }

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
        public override IParameterSymbol[] GetParameters()
        {
            if (ReflectionExtensions.MethodOnTypeBuilderInstantiationType.IsInstanceOfType(_method))
                return GetParametersForMethodOnTypeBuilderInstantiation();
            else
                return base.GetParameters();
        }

        /// <summary>
        /// Gets the parameters in the case that this method is a <see cref="MethodOnTypeBuilderInstantiation"/>.
        /// </summary>
        /// <returns></returns>
        IParameterSymbol[] GetParametersForMethodOnTypeBuilderInstantiation()
        {
            // we cache the spec parameters to avoid creating them repeatidly
            if (_specParameters == null)
            {
                var l = _method.GetParameters();
                var a = new ReflectionParameterSpecInfo[l.Length];
                for (int i = 0; i < l.Length; i++)
                    a[i] = new ReflectionParameterSpecInfo(_method, l[i], _method.DeclaringType!.GetGenericArguments());

                Interlocked.CompareExchange(ref _specParameters, a, null);
            }

            // resolve into symbols
            var symbols = new IParameterSymbol[_specParameters.Length];
            for (int i = 0; i < _specParameters.Length; i++)
                symbols[i] = ResolveParameterSymbol(_specParameters[i]);

            return symbols;
        }

        /// <inheritdoc />
        public IMethodSymbol MakeGenericMethod(params ITypeSymbol[] typeArguments)
        {
            return ResolveMethodSymbol(UnderlyingMethod.MakeGenericMethod(typeArguments.Unpack()));
        }

        #endregion

    }

}
