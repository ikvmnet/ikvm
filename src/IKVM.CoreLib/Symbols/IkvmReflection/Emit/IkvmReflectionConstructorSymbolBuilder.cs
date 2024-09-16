using System;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionConstructorSymbolBuilder : IkvmReflectionMethodBaseSymbolBuilder<IConstructorSymbol, IkvmReflectionConstructorSymbol>, IConstructorSymbolBuilder
    {

        readonly ConstructorBuilder _builder;
        IkvmReflectionConstructorSymbol? _symbol;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingTypeBuilder"></param>
        /// <param name="builder"></param>
        public IkvmReflectionConstructorSymbolBuilder(IkvmReflectionSymbolContext context, IkvmReflectionTypeSymbolBuilder containingTypeBuilder, ConstructorBuilder builder) :
            base(context, containingTypeBuilder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the containing <see cref="IkvmReflectionTypeSymbolBuilder"/>.
        /// </summary>
        protected internal new IkvmReflectionTypeSymbolBuilder ContainingTypeBuilder => base.ContainingTypeBuilder ?? throw new NullReferenceException();

        /// <inheritdoc />
        internal override IkvmReflectionConstructorSymbol ReflectionSymbol => _symbol ??= Context.GetOrCreateConstructorSymbol(_builder);

        /// <inheritdoc />
        public void SetImplementationFlags(System.Reflection.MethodImplAttributes attributes)
        {
            _builder.SetImplementationFlags((MethodImplAttributes)attributes);
        }

        /// <inheritdoc />
        public IParameterSymbolBuilder DefineParameter(int iSequence, System.Reflection.ParameterAttributes attributes, string? strParamName)
        {
            return new IkvmReflectionParameterSymbolBuilder<IConstructorSymbol, IkvmReflectionConstructorSymbol, IkvmReflectionConstructorSymbolBuilder>(Context, this, _builder, _builder.DefineParameter(iSequence, (ParameterAttributes)attributes, strParamName));
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
            _builder.SetCustomAttribute(((IkvmReflectionCustomAttributeBuilder)customBuilder).ReflectionBuilder);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            _builder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

    }

}
