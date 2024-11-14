using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Emit
{

    class FieldSymbolBuilder : FieldSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public FieldSymbolBuilder(SymbolContext context, TypeSymbolBuilder declaringType) :
            base(context, declaringType.Module, declaringType)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringModule"></param>
        public FieldSymbolBuilder(SymbolContext context, ModuleSymbolBuilder declaringModule) :
            base(context, declaringModule, null)
        {

        }

        /// <inheritdoc />
        public override FieldAttributes Attributes => throw new NotImplementedException();

        /// <inheritdoc />
        public override TypeSymbol FieldType => throw new NotImplementedException();

        /// <inheritdoc />
        public override string Name => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsMissing => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool ContainsMissing => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsComplete => throw new NotImplementedException();

        /// <inheritdoc />
        public override object? GetRawConstantValue()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}
