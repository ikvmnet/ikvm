using System;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionConstructorSymbolBuilder : IkvmReflectionMethodBaseSymbolBuilder, IIkvmReflectionConstructorSymbolBuilder
    {

        ConstructorBuilder? _builder;
        ConstructorInfo _ctor;

        IkvmReflectionILGenerator? _il;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionConstructorSymbolBuilder(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbolBuilder resolvingModule, IIkvmReflectionTypeSymbolBuilder resolvingType, ConstructorBuilder builder) :
            base(context, resolvingModule, resolvingType)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _ctor = _builder;
        }

        /// <inheritdoc />
        public ConstructorInfo UnderlyingConstructor => _ctor;

        /// <inheritdoc />
        public override MethodBase UnderlyingMethodBase => UnderlyingConstructor;

        /// <inheritdoc />
        public ConstructorBuilder UnderlyingConstructorBuilder => _builder ?? throw new InvalidOperationException();

        #region IConstructorSymbolBuilder

        /// <inheritdoc />
        public override void SetImplementationFlags(System.Reflection.MethodImplAttributes attributes)
        {
            UnderlyingConstructorBuilder.SetImplementationFlags((MethodImplAttributes)attributes);
        }

        /// <inheritdoc />
        public override IParameterSymbolBuilder DefineParameter(int iSequence, System.Reflection.ParameterAttributes attributes, string? strParamName)
        {
            if (iSequence <= 0)
                throw new ArgumentOutOfRangeException(nameof(iSequence));

            return ResolveParameterSymbol(this, UnderlyingConstructorBuilder.DefineParameter(iSequence, (ParameterAttributes)attributes, strParamName));
        }

        /// <inheritdoc />
        public override IILGenerator GetILGenerator()
        {
            return _il ??= new IkvmReflectionILGenerator(Context, UnderlyingConstructorBuilder.GetILGenerator());
        }

        /// <inheritdoc />
        public override IILGenerator GetILGenerator(int streamSize)
        {
            return _il ??= new IkvmReflectionILGenerator(Context, UnderlyingConstructorBuilder.GetILGenerator(streamSize));
        }

        #endregion

        #region ICustomAttributeProviderBuilder

        /// <inheritdoc />
        public override void SetCustomAttribute(CustomAttribute attribute)
        {
            UnderlyingConstructorBuilder.SetCustomAttribute(attribute.Unpack());
        }

        #endregion

        #region IConstructorSymbol

        /// <inheritdoc />
        public override bool IsComplete => _builder == null;

        #endregion

        /// <inheritdoc />
        public override void OnComplete()
        {
            _builder = null;
            base.OnComplete();
        }

    }

}
