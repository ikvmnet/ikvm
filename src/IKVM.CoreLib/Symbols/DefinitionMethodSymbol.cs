using System;

namespace IKVM.CoreLib.Symbols
{

    public abstract class DefinitionMethodSymbol : MethodSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected DefinitionMethodSymbol(SymbolContext context) :
            base(context)
        {

        }

        /// <inheritdoc />
        public sealed override bool IsGenericMethodDefinition => GenericParameters.IsEmpty == false;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericMethod => false;

        /// <inheritdoc />
        public sealed override MethodSymbol? BaseDefinition => throw new NotImplementedException();

        /// <inheritdoc />
        public sealed override MethodSymbol? GenericMethodDefinition => IsGenericMethodDefinition ? this : throw new InvalidOperationException();

    }

}
