using System;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionEventSymbolBuilder : ReflectionSymbolBuilder<IEventSymbol, ReflectionEventSymbol>, IEventSymbolBuilder
    {

        readonly ReflectionTypeSymbolBuilder _containingTypeBuilder;
        readonly EventBuilder _builder;
        readonly ReflectionEventBuilderInfo _info;
        ReflectionEventSymbol? _symbol;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingTypeBuilder"></param>
        /// <param name="builder"></param>
        public ReflectionEventSymbolBuilder(ReflectionSymbolContext context, ReflectionTypeSymbolBuilder containingTypeBuilder, EventBuilder builder) :
            base(context)
        {
            _containingTypeBuilder = containingTypeBuilder ?? throw new ArgumentNullException(nameof(containingTypeBuilder));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _info = new ReflectionEventBuilderInfo(_builder);
        }

        /// <inheritdoc />
        internal override ReflectionEventSymbol ReflectionSymbol => _symbol ??= _containingTypeBuilder.ReflectionSymbol.ResolveEventSymbol(_info);

        /// <inheritdoc />
        public void SetAddOnMethod(IMethodSymbolBuilder mdBuilder)
        {
            _builder.SetAddOnMethod(((ReflectionMethodSymbolBuilder)mdBuilder).ReflectionBuilder);
        }

        /// <inheritdoc />
        public void SetRemoveOnMethod(IMethodSymbolBuilder mdBuilder)
        {
            _builder.SetRemoveOnMethod(((ReflectionMethodSymbolBuilder)mdBuilder).ReflectionBuilder);
        }

        /// <inheritdoc />
        public void SetRaiseMethod(IMethodSymbolBuilder mdBuilder)
        {
            _builder.SetRaiseMethod(((ReflectionMethodSymbolBuilder)mdBuilder).ReflectionBuilder);
        }

        /// <inheritdoc />
        public void AddOtherMethod(IMethodSymbolBuilder mdBuilder)
        {
            _builder.AddOtherMethod(((ReflectionMethodSymbolBuilder)mdBuilder).ReflectionBuilder);
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

}
