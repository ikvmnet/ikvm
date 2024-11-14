using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Emit
{

    class EventSymbolBuilder : EventSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public EventSymbolBuilder(SymbolContext context, TypeSymbolBuilder declaringType) :
            base(context, declaringType)
        {

        }

        /// <inheritdoc />
        public override EventAttributes Attributes => throw new NotImplementedException();

        /// <inheritdoc />
        public override TypeSymbol? EventHandlerType => throw new NotImplementedException();

        /// <inheritdoc />
        public override string Name => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsMissing => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool ContainsMissing => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsComplete => throw new NotImplementedException();

        /// <inheritdoc />
        public override MethodSymbol? GetAddMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ImmutableArray<MethodSymbol> GetOtherMethods(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override MethodSymbol? GetRaiseMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override MethodSymbol? GetRemoveMethod(bool nonPublic)
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
