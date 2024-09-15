using System;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionPropertySymbolBuilder : ReflectionSymbolBuilder<IPropertySymbol, ReflectionPropertySymbol>, IPropertySymbolBuilder
    {

        readonly ReflectionTypeSymbolBuilder _containingTypeBuilder;
        readonly PropertyBuilder _builder;
        ReflectionPropertySymbol? _symbol;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingTypeBuilder"></param>
        /// <param name="builder"></param>
        public ReflectionPropertySymbolBuilder(ReflectionSymbolContext context, ReflectionTypeSymbolBuilder containingTypeBuilder, PropertyBuilder builder) :
            base(context)
        {
            _containingTypeBuilder = containingTypeBuilder ?? throw new ArgumentNullException(nameof(containingTypeBuilder));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        internal PropertyBuilder ReflectionBuilder => _builder;

        internal override ReflectionPropertySymbol ReflectionSymbol => _symbol ??= _containingTypeBuilder.ReflectionSymbol.ResolvePropertySymbol(_builder);

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
