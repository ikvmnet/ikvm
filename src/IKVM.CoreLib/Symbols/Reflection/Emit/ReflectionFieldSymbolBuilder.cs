using System;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionFieldSymbolBuilder : ReflectionSymbolBuilder<IFieldSymbol, ReflectionFieldSymbol>, IFieldSymbolBuilder
    {

        readonly ReflectionModuleSymbolBuilder _containingModuleBuilder;
        readonly ReflectionTypeSymbolBuilder? _containingTypeBuilder;
        readonly FieldBuilder _builder;
        ReflectionFieldSymbol? _symbol;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingModuleBuilder"></param>
        /// <param name="builder"></param>
        public ReflectionFieldSymbolBuilder(ReflectionSymbolContext context, ReflectionModuleSymbolBuilder containingModuleBuilder, FieldBuilder builder) :
            base(context)
        {
            _containingModuleBuilder = containingModuleBuilder ?? throw new ArgumentNullException(nameof(containingModuleBuilder));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingTypeBuilder"></param>
        /// <param name="builder"></param>
        public ReflectionFieldSymbolBuilder(ReflectionSymbolContext context, ReflectionTypeSymbolBuilder containingTypeBuilder, FieldBuilder builder) :
            base(context)
        {
            _containingTypeBuilder = containingTypeBuilder ?? throw new ArgumentNullException(nameof(containingTypeBuilder));
            _containingModuleBuilder = containingTypeBuilder.ContainingModuleBuilder;
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the containing <see cref="ReflectionModuleSymbolBuilder"/>.
        /// </summary>
        internal ReflectionModuleSymbolBuilder ContainingModuleBuilder => _containingModuleBuilder;

        /// <summary>
        /// Gets the containing <see cref="ReflectionTypeSymbolBuilder"/>.
        /// </summary>
        internal ReflectionTypeSymbolBuilder? ContainingTypeBuilder => _containingTypeBuilder;

        /// <summary>
        /// Gets the underlying <see cref="FieldBuilder"/>
        /// </summary>
        internal FieldBuilder ReflectionBuilder => _builder;

        /// <inheritdoc />
        internal override ReflectionFieldSymbol ReflectionSymbol => _symbol ??= Context.GetOrCreateFieldSymbol(_builder);

        /// <inheritdoc />
        public void SetConstant(object? defaultValue)
        {
            _builder.SetConstant(defaultValue);
        }

        /// <inheritdoc />
        public void SetOffset(int iOffset)
        {
            _builder.SetOffset(iOffset);
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
