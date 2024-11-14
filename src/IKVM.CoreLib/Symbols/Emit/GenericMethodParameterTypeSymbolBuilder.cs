using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Emit
{

    class GenericMethodParameterTypeSymbolBuilder : GenericMethodParameterTypeSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringMethod"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public GenericMethodParameterTypeSymbolBuilder(SymbolContext context, MethodSymbolBuilder declaringMethod) :
            base(context, declaringMethod)
        {

        }

        /// <inheritdoc />
        public override string? FullName => throw new NotImplementedException();

        /// <inheritdoc />
        public override string? Namespace => throw new NotImplementedException();

        /// <inheritdoc />
        public override GenericParameterAttributes GenericParameterAttributes => throw new NotImplementedException();

        /// <inheritdoc />
        public override int GenericParameterPosition => throw new NotImplementedException();

        /// <inheritdoc />
        public override string Name => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsComplete => throw new NotImplementedException();

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetInterfaces()
        {
            throw new NotImplementedException();
        }

    }

}
