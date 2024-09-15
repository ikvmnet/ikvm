using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionConstructorSymbolBuilder : ReflectionMethodBaseSymbolBuilder<IConstructorSymbol, ReflectionConstructorSymbol>, IConstructorSymbolBuilder
    {

        readonly ConstructorBuilder _builder;
        ReflectionConstructorSymbol? _symbol;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingTypeBuilder"></param>
        /// <param name="builder"></param>
        public ReflectionConstructorSymbolBuilder(ReflectionSymbolContext context, ReflectionTypeSymbolBuilder containingTypeBuilder, ConstructorBuilder builder) :
            base(context, containingTypeBuilder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the containing <see cref="ReflectionTypeSymbolBuilder"/>.
        /// </summary>
        protected internal new ReflectionTypeSymbolBuilder ContainingTypeBuilder => base.ContainingTypeBuilder ?? throw new NullReferenceException();

        /// <inheritdoc />
        internal override ReflectionConstructorSymbol ReflectionSymbol => _symbol ??= Context.GetOrCreateConstructorSymbol(_builder);

        /// <inheritdoc />
        public void SetImplementationFlags(MethodImplAttributes attributes)
        {
            _builder.SetImplementationFlags(attributes);
        }

        /// <inheritdoc />
        public IParameterSymbolBuilder DefineParameter(int iSequence, ParameterAttributes attributes, string? strParamName)
        {
            return new ReflectionParameterSymbolBuilder<IConstructorSymbol, ReflectionConstructorSymbol, ReflectionConstructorSymbolBuilder>(Context, this, _builder.DefineParameter(iSequence, attributes, strParamName));
        }

        /// <inheritdoc />
        public ILGenerator GetILGenerator()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ILGenerator GetILGenerator(int streamSize)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            _builder.SetCustomAttribute(((ReflectionCustomAttributeBuilder)customBuilder).ReflectionBuilder);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            _builder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

    }

}
