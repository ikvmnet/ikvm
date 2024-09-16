using System;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionFieldSymbolBuilder : IkvmReflectionSymbolBuilder<IFieldSymbol, IkvmReflectionFieldSymbol>, IFieldSymbolBuilder
    {

        readonly IkvmReflectionModuleSymbolBuilder _containingModuleBuilder;
        readonly IkvmReflectionTypeSymbolBuilder? _containingTypeBuilder;
        readonly FieldBuilder _builder;
        IkvmReflectionFieldSymbol? _symbol;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingModuleBuilder"></param>
        /// <param name="builder"></param>
        public IkvmReflectionFieldSymbolBuilder(IkvmReflectionSymbolContext context, IkvmReflectionModuleSymbolBuilder containingModuleBuilder, FieldBuilder builder) :
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
        public IkvmReflectionFieldSymbolBuilder(IkvmReflectionSymbolContext context, IkvmReflectionTypeSymbolBuilder containingTypeBuilder, FieldBuilder builder) :
            base(context)
        {
            _containingTypeBuilder = containingTypeBuilder ?? throw new ArgumentNullException(nameof(containingTypeBuilder));
            _containingModuleBuilder = containingTypeBuilder.ContainingModuleBuilder;
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the containing <see cref="IkvmReflectionModuleSymbolBuilder"/>.
        /// </summary>
        internal IkvmReflectionModuleSymbolBuilder ContainingModuleBuilder => _containingModuleBuilder;

        /// <summary>
        /// Gets the containing <see cref="IkvmReflectionTypeSymbolBuilder"/>.
        /// </summary>
        internal IkvmReflectionTypeSymbolBuilder? ContainingTypeBuilder => _containingTypeBuilder;

        /// <summary>
        /// Gets the underlying <see cref="FieldBuilder"/>
        /// </summary>
        internal FieldBuilder ReflectionBuilder => _builder;

        /// <inheritdoc />
        internal override IkvmReflectionFieldSymbol ReflectionSymbol => _symbol ??= Context.GetOrCreateFieldSymbol(_builder);

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
            _builder.SetCustomAttribute(((IkvmReflectionCustomAttributeBuilder)customBuilder).ReflectionBuilder);
        }

    }

}
