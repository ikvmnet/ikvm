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

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionConstructorSymbolBuilder(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol resolvingModule, IIkvmReflectionTypeSymbol resolvingType, ConstructorBuilder builder) :
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
        public void SetImplementationFlags(System.Reflection.MethodImplAttributes attributes)
        {
            UnderlyingConstructorBuilder.SetImplementationFlags((MethodImplAttributes)attributes);
        }

        /// <inheritdoc />
        public IParameterSymbolBuilder DefineParameter(int iSequence, System.Reflection.ParameterAttributes attributes, string? strParamName)
        {
            return ResolveParameterSymbol(UnderlyingConstructorBuilder.DefineParameter(iSequence, (ParameterAttributes)attributes, strParamName));
        }

        /// <inheritdoc />
        public IILGenerator GetILGenerator()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IILGenerator GetILGenerator(int streamSize)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            UnderlyingConstructorBuilder.SetCustomAttribute(((IkvmReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            UnderlyingConstructorBuilder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        #endregion

        #region IConstructorSymbol

        /// <inheritdoc />
        public override bool IsComplete => _builder == null;

        #endregion

        /// <inheritdoc />
        public override void OnComplete()
        {
            _ctor = (ConstructorInfo?)ResolvingModule.UnderlyingModule.ResolveMethod(MetadataToken) ?? throw new InvalidOperationException();
            _builder = null;
            base.OnComplete();
        }

    }

}
