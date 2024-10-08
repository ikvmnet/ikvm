using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionConstructorSymbolBuilder : ReflectionMethodBaseSymbolBuilder, IReflectionConstructorSymbolBuilder
    {

        readonly ConstructorBuilder _builder;
        ConstructorInfo? _ctor;

        ReflectionILGenerator? _il;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionConstructorSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbolBuilder resolvingModule, IReflectionTypeSymbolBuilder resolvingType, ConstructorBuilder builder) :
            base(context, resolvingModule, resolvingType)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc />
        public ConstructorInfo UnderlyingConstructor => _ctor ?? _builder;

        /// <inheritdoc />
        public ConstructorInfo UnderlyingEmitConstructor => _builder;

        /// <inheritdoc />
        public ConstructorInfo UnderlyingDynamicEmitConstructor => _ctor ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public override MethodBase UnderlyingMethodBase => UnderlyingConstructor;

        /// <inheritdoc />
        public override MethodBase UnderlyingEmitMethodBase => UnderlyingEmitConstructor;

        /// <inheritdoc />
        public ConstructorBuilder UnderlyingConstructorBuilder => _builder;

        #region IConstructorSymbolBuilder

        /// <inheritdoc />
        public override void SetImplementationFlags(MethodImplAttributes attributes)
        {
            UnderlyingConstructorBuilder.SetImplementationFlags(attributes);
        }

        /// <inheritdoc />
        public override IParameterSymbolBuilder DefineParameter(int iSequence, ParameterAttributes attributes, string? strParamName)
        {
            if (iSequence <= 0)
                throw new ArgumentOutOfRangeException(nameof(iSequence));

            return ResolveParameterSymbol(this, UnderlyingConstructorBuilder.DefineParameter(iSequence, attributes, strParamName));
        }

        /// <inheritdoc />
        public override IILGenerator GetILGenerator()
        {
            return _il ??= new ReflectionILGenerator(Context, UnderlyingConstructorBuilder.GetILGenerator());
        }

        /// <inheritdoc />
        public override IILGenerator GetILGenerator(int streamSize)
        {
            return _il ??= new ReflectionILGenerator(Context, UnderlyingConstructorBuilder.GetILGenerator(streamSize));
        }

        /// <inheritdoc />
        public override void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            UnderlyingConstructorBuilder.SetCustomAttribute(((ReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
        }

        /// <inheritdoc />
        public override void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            UnderlyingConstructorBuilder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        #endregion

        #region IConstructorSymbol

        /// <inheritdoc />
        public override bool IsComplete => _ctor != null;

        #endregion

        #region IMethodBaseSymbol

        /// <inheritdoc />
        public override ITypeSymbol[] GetGenericArguments()
        {
            // constructor never has generic arguments
            return [];
        }

        #endregion

        /// <inheritdoc />
        public override void OnComplete()
        {
            _ctor = (ConstructorInfo?)ResolvingModule.UnderlyingModule.ResolveMethod(MetadataToken) ?? throw new InvalidOperationException();
            base.OnComplete();
        }

    }

}
