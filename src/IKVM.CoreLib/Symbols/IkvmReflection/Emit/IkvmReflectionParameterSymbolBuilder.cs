using System;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    abstract class IkvmReflectionParameterSymbolBuilder : IkvmReflectionSymbolBuilder<IParameterSymbol, IkvmReflectionParameterSymbol>, IParameterSymbolBuilder
    {

        readonly MemberInfo _member;
        readonly ParameterBuilder _builder;
        readonly IkvmReflectionParameterBuilderInfo _info;
        IkvmReflectionParameterSymbol? _symbol;

        object? _constant;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected IkvmReflectionParameterSymbolBuilder(IkvmReflectionSymbolContext context, MemberInfo member, ParameterBuilder builder) :
            base(context)
        {
            _member = member ?? throw new ArgumentNullException(nameof(member));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _info = new IkvmReflectionParameterBuilderInfo(_member, _builder);
        }

        /// <summary>
        /// Gets the wrapped <see cref="ParameterBuilder"/>.
        /// </summary>
        internal ParameterBuilder ReflectionBuilder => _builder;

        /// <inheritdoc />
        internal override IkvmReflectionParameterSymbol ReflectionSymbol => _symbol ??= GetOrCreateSymbol(_info);

        /// <summary>
        /// Gets or creates the <see cref="ReflectionParameterSymbol"/>.
        /// </summary>
        /// <returns></returns>
        protected internal abstract IkvmReflectionParameterSymbol GetOrCreateSymbol(IkvmReflectionParameterBuilderInfo info);

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
            _builder.SetCustomAttribute(((IkvmReflectionCustomAttributeBuilder)customBuilder).ReflectionBuilder);
        }

    }

    class IkvmReflectionParameterSymbolBuilder<TContainingSymbol, TContainingReflectionSymbol, TContainingBuilder> : IkvmReflectionParameterSymbolBuilder
        where TContainingSymbol : IMethodBaseSymbol
        where TContainingReflectionSymbol : IkvmReflectionMethodBaseSymbol, TContainingSymbol
        where TContainingBuilder : IkvmReflectionMethodBaseSymbolBuilder<TContainingSymbol, TContainingReflectionSymbol>
    {

        readonly TContainingBuilder _containingMethodBuilder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingMethodBuilder"></param>
        /// <param name="member"></param>
        /// <param name="builder"></param>
        public IkvmReflectionParameterSymbolBuilder(IkvmReflectionSymbolContext context, TContainingBuilder containingMethodBuilder, MemberInfo member, ParameterBuilder builder) :
            base(context, member, builder)
        {
            _containingMethodBuilder = containingMethodBuilder ?? throw new ArgumentNullException(nameof(containingMethodBuilder));
        }

        /// <summary>
        /// Gets the containing <see cref="IkvmReflectionMethodBaseSymbolBuilder{IMethodBaseSymbol, ReflectionMethodBaseSymbol}"/>.
        /// </summary>
        internal TContainingBuilder ContainingMethodBuilder => _containingMethodBuilder;

        /// <inheritdoc />
        protected internal override IkvmReflectionParameterSymbol GetOrCreateSymbol(IkvmReflectionParameterBuilderInfo info) => ContainingMethodBuilder.ReflectionSymbol.ResolveParameterSymbol(info);

    }

}
