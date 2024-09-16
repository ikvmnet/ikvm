using System;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionEventSymbolBuilder : IkvmReflectionSymbolBuilder<IEventSymbol, IkvmReflectionEventSymbol>, IEventSymbolBuilder
    {

        readonly IkvmReflectionTypeSymbolBuilder _containingTypeBuilder;
        readonly EventBuilder _builder;
        IkvmReflectionEventSymbol? _symbol;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingTypeBuilder"></param>
        /// <param name="builder"></param>
        public IkvmReflectionEventSymbolBuilder(IkvmReflectionSymbolContext context, IkvmReflectionTypeSymbolBuilder containingTypeBuilder, EventBuilder builder) :
            base(context)
        {
            _containingTypeBuilder = containingTypeBuilder ?? throw new ArgumentNullException(nameof(containingTypeBuilder));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc />
        internal override IkvmReflectionEventSymbol ReflectionSymbol => _symbol ??= _containingTypeBuilder.ReflectionSymbol.ResolveEventSymbol(_builder);

        /// <inheritdoc />
        public void SetAddOnMethod(IMethodSymbolBuilder mdBuilder)
        {
            _builder.SetAddOnMethod(((IkvmReflectionMethodSymbolBuilder)mdBuilder).ReflectionBuilder);
        }

        /// <inheritdoc />
        public void SetRemoveOnMethod(IMethodSymbolBuilder mdBuilder)
        {
            _builder.SetRemoveOnMethod(((IkvmReflectionMethodSymbolBuilder)mdBuilder).ReflectionBuilder);
        }

        /// <inheritdoc />
        public void SetRaiseMethod(IMethodSymbolBuilder mdBuilder)
        {
            _builder.SetRaiseMethod(((IkvmReflectionMethodSymbolBuilder)mdBuilder).ReflectionBuilder);
        }

        /// <inheritdoc />
        public void AddOtherMethod(IMethodSymbolBuilder mdBuilder)
        {
            _builder.AddOtherMethod(((IkvmReflectionMethodSymbolBuilder)mdBuilder).ReflectionBuilder);
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

}
