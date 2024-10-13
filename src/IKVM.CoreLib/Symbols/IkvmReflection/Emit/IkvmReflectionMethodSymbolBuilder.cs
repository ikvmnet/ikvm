using System;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionMethodSymbolBuilder : IkvmReflectionMethodBaseSymbolBuilder, IIkvmReflectionMethodSymbolBuilder
    {

        MethodBuilder? _builder;
        MethodInfo _method;

        IkvmReflectionGenericTypeParameterTable _genericTypeParameterTable;
        IkvmReflectionMethodSpecTable _specTable;

        IkvmReflectionILGenerator? _il;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionMethodSymbolBuilder(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbolBuilder resolvingModule, IIkvmReflectionTypeSymbolBuilder? resolvingType, MethodBuilder builder) :
            base(context, resolvingModule, resolvingType)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _method = _builder;
            _genericTypeParameterTable = new IkvmReflectionGenericTypeParameterTable(context, resolvingModule, this);
            _specTable = new IkvmReflectionMethodSpecTable(context, resolvingModule, resolvingType, this);
        }

        /// <inheritdoc />
        public MethodInfo UnderlyingMethod => _method;

        /// <inheritdoc />
        public override MethodBase UnderlyingMethodBase => UnderlyingMethod;

        /// <inheritdoc />
        public MethodBuilder UnderlyingMethodBuilder => _builder ?? throw new InvalidOperationException();

        #region IIkvmReflectionMethodSymbolBuilder

        /// <inheritdoc />
        public IIkvmReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericTypeParameter)
        {
            return _genericTypeParameterTable.GetOrCreateGenericTypeParameterSymbol(genericTypeParameter);
        }

        #endregion

        #region IIkvmReflectionMethodSymbol

        /// <inheritdoc />
        public IIkvmReflectionTypeSymbol GetOrCreateGenericTypeParameterSymbol(Type genericTypeParameter)
        {
            return _genericTypeParameterTable.GetOrCreateGenericTypeParameterSymbol(genericTypeParameter);
        }

        /// <inheritdoc />
        public IIkvmReflectionMethodSymbol GetOrCreateGenericMethodSymbol(MethodInfo method)
        {
            return _specTable.GetOrCreateGenericMethodSymbol(method.GetGenericArguments());
        }

        #endregion

        #region IIkvmReflectionMethodBaseSymbol

        #endregion

        #region IIkvmReflectionMethodBaseSymbolBuilder

        #endregion

        #region IMethodBaseSymbolBuilder

        /// <inheritdoc />
        public override void SetImplementationFlags(System.Reflection.MethodImplAttributes attributes)
        {
            UnderlyingMethodBuilder.SetImplementationFlags((MethodImplAttributes)attributes);
        }

        /// <inheritdoc />
        public override IParameterSymbolBuilder DefineParameter(int position, System.Reflection.ParameterAttributes attributes, string? strParamName)
        {
            return ResolveParameterSymbol(this, UnderlyingMethodBuilder.DefineParameter(position, (ParameterAttributes)attributes, strParamName));
        }

        /// <inheritdoc />
        public override IILGenerator GetILGenerator()
        {
            return _il ??= new IkvmReflectionILGenerator(Context, UnderlyingMethodBuilder.GetILGenerator());
        }

        /// <inheritdoc />
        public override IILGenerator GetILGenerator(int streamSize)
        {
            return _il ??= new IkvmReflectionILGenerator(Context, UnderlyingMethodBuilder.GetILGenerator(streamSize));
        }

        #endregion

        #region IMethodSymbolBuilder

        /// <inheritdoc />
        public void SetParameters(params ITypeSymbol[] parameterTypes)
        {
            UnderlyingMethodBuilder.SetParameters(parameterTypes.Unpack());
        }

        /// <inheritdoc />
        public void SetReturnType(ITypeSymbol? returnType)
        {
            UnderlyingMethodBuilder.SetReturnType(returnType?.Unpack());
        }

        /// <inheritdoc />
        public void SetSignature(ITypeSymbol? returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers)
        {
            UnderlyingMethodBuilder.SetSignature(returnType?.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack());
        }

        public IGenericTypeParameterSymbolBuilder[] DefineGenericParameters(params string[] names)
        {
            var l = UnderlyingMethodBuilder.DefineGenericParameters(names);
            var a = new IGenericTypeParameterSymbolBuilder[l.Length];
            for (int i = 0; i < l.Length; i++)
                a[i] = ResolveGenericTypeParameterSymbol(l[i]);

            return a;
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
        public override bool IsComplete => _builder == null;

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

        #region ICustomAttributeProviderBuilder

        /// <inheritdoc />
        public override void SetCustomAttribute(CustomAttribute attribute)
        {
            UnderlyingMethodBuilder.SetCustomAttribute(attribute.Unpack());
        }

        #endregion

        /// <inheritdoc />
        public override void OnComplete()
        {
            _builder = null;
            base.OnComplete();
        }
    }

}
