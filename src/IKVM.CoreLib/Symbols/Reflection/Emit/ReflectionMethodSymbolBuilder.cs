using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionMethodSymbolBuilder : ReflectionMethodBaseSymbolBuilder, IReflectionMethodSymbolBuilder
    {

        MethodBuilder? _builder;
        MethodInfo _method;

        ReflectionILGenerator? _il;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionMethodSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol? resolvingType, MethodBuilder builder) :
            base(context, resolvingModule, resolvingType)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _method = _builder;
        }

        /// <inheritdoc />
        public MethodInfo UnderlyingMethod => _method;

        /// <inheritdoc />
        public override MethodBase UnderlyingMethodBase => UnderlyingMethod;

        /// <inheritdoc />
        public MethodBuilder UnderlyingMethodBuilder => _builder ?? throw new InvalidOperationException();

        #region IMethodSymbolBuilder

        /// <inheritdoc />
        public void SetImplementationFlags(MethodImplAttributes attributes)
        {
            UnderlyingMethodBuilder.SetImplementationFlags(attributes);
        }

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
                a[i] = (IGenericTypeParameterSymbolBuilder)ResolveTypeSymbol(l[i]);

            return a;
        }

        /// <inheritdoc />
        public IParameterSymbolBuilder DefineParameter(int position, ParameterAttributes attributes, string? strParamName)
        {
            return ResolveParameterSymbol(UnderlyingMethodBuilder.DefineParameter(position, attributes, strParamName));
        }

        /// <inheritdoc />
        public IILGenerator GetILGenerator()
        {
            return _il ??= new ReflectionILGenerator(Context, UnderlyingMethodBuilder.GetILGenerator());
        }

        /// <inheritdoc />
        public IILGenerator GetILGenerator(int streamSize)
        {
            return _il ??= new ReflectionILGenerator(Context, UnderlyingMethodBuilder.GetILGenerator(streamSize));
        }

        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            UnderlyingMethodBuilder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            UnderlyingMethodBuilder.SetCustomAttribute(((ReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
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

        /// <inheritdoc />
        public override void OnComplete()
        {
            _method = (MethodInfo?)ResolvingModule.UnderlyingModule.ResolveMethod(MetadataToken) ?? throw new InvalidOperationException();
            _builder = null;
            base.OnComplete();
        }

    }

}
