using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionConstructorSymbolBuilder : ReflectionMethodBaseSymbolBuilder, IReflectionConstructorSymbolBuilder
    {

        ConstructorBuilder? _builder;
        ConstructorInfo _ctor;

        ReflectionILGenerator? _il;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionConstructorSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol resolvingType, ConstructorBuilder builder) :
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
        public void SetImplementationFlags(MethodImplAttributes attributes)
        {
            UnderlyingConstructorBuilder.SetImplementationFlags(attributes);
        }

        /// <inheritdoc />
        public IParameterSymbolBuilder DefineParameter(int iSequence, ParameterAttributes attributes, string? strParamName)
        {
            return ResolveParameterSymbol(UnderlyingConstructorBuilder.DefineParameter(iSequence, attributes, strParamName));
        }

        /// <inheritdoc />
        public IILGenerator GetILGenerator()
        {
            return _il ??= new ReflectionILGenerator(Context, UnderlyingConstructorBuilder.GetILGenerator());
        }

        /// <inheritdoc />
        public IILGenerator GetILGenerator(int streamSize)
        {
            return _il ??= new ReflectionILGenerator(Context, UnderlyingConstructorBuilder.GetILGenerator(streamSize));
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            UnderlyingConstructorBuilder.SetCustomAttribute(((ReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
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
