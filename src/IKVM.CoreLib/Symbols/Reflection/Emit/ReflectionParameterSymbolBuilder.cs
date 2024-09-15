using System;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    abstract class ReflectionParameterSymbolBuilder : ReflectionSymbolBuilder<IParameterSymbol, ReflectionParameterSymbol>, IParameterSymbolBuilder
    {
        
        readonly ParameterBuilder _builder;
        readonly ReflectionParameterBuilderInfo _info;
        ReflectionParameterSymbol? _symbol;

        object? _constant;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected ReflectionParameterSymbolBuilder(ReflectionSymbolContext context, ParameterBuilder builder) :
            base(context)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _info = new ReflectionParameterBuilderInfo(_builder);
        }

        /// <summary>
        /// Gets the wrapped <see cref="ParameterBuilder"/>.
        /// </summary>
        internal ParameterBuilder ReflectionBuilder => _builder;

        /// <inheritdoc />
        internal override ReflectionParameterSymbol ReflectionSymbol => _symbol ??= GetOrCreateSymbol(_info);

        /// <summary>
        /// Gets or creates the <see cref="ReflectionParameterSymbol"/>.
        /// </summary>
        /// <returns></returns>
        protected internal abstract ReflectionParameterSymbol GetOrCreateSymbol(ReflectionParameterBuilderInfo info);

        /// <inheritdoc />
        public void SetConstant(object? defaultValue)
        {
            _builder.SetConstant(_constant = defaultValue);
        }

        /// <summary>
        /// Saves the constant value so it can be retrieved as a symbol.
        /// </summary>
        /// <returns></returns>
        internal object? GetConstant()
        {
            return _constant;
        }

        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            _builder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            _builder.SetCustomAttribute(((ReflectionCustomAttributeBuilder)customBuilder).ReflectionBuilder);
        }

    }

    class ReflectionParameterSymbolBuilder<TContainingSymbol, TContainingReflectionSymbol, TContainingBuilder> : ReflectionParameterSymbolBuilder
        where TContainingSymbol : IMethodBaseSymbol
        where TContainingReflectionSymbol : ReflectionMethodBaseSymbol, TContainingSymbol
        where TContainingBuilder : ReflectionMethodBaseSymbolBuilder<TContainingSymbol, TContainingReflectionSymbol>
    {

        readonly TContainingBuilder _containingMethodBuilder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingMethodBuilder"></param>
        /// <param name="builder"></param>
        public ReflectionParameterSymbolBuilder(ReflectionSymbolContext context, TContainingBuilder containingMethodBuilder, ParameterBuilder builder) :
            base(context, builder)
        {
            _containingMethodBuilder = containingMethodBuilder ?? throw new ArgumentNullException(nameof(containingMethodBuilder));
        }

        /// <summary>
        /// Gets the containing <see cref="ReflectionMethodBaseSymbolBuilder{IMethodBaseSymbol, ReflectionMethodBaseSymbol}"/>.
        /// </summary>
        internal TContainingBuilder ContainingMethodBuilder => _containingMethodBuilder;

        /// <inheritdoc />
        protected internal override ReflectionParameterSymbol GetOrCreateSymbol(ReflectionParameterBuilderInfo info) => ContainingMethodBuilder.ReflectionSymbol.ResolveParameterSymbol(info);

    }

}
