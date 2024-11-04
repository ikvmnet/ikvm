using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class PointerTypeSymbol : HasElementSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="elementType"></param>
        public PointerTypeSymbol(ISymbolContext context, TypeSymbol elementType) :
            base(context, elementType)
        {

        }

        /// <inheritdoc />
        protected override string NameSuffix => "*";

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => TypeAttributes.Public;

        /// <inheritdoc />
        public override TypeSymbol? BaseType => null;

        /// <inheritdoc />
        public sealed override bool IsPointer => true;

        /// <inheritdoc />
        public sealed override int GetArrayRank()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public sealed override ImmutableList<ConstructorSymbol> GetDeclaredConstructors()
        {
            return ImmutableList<ConstructorSymbol>.Empty;
        }

        /// <inheritdoc />
        public sealed override ImmutableList<MethodSymbol> GetDeclaredMethods()
        {
            return ImmutableList<MethodSymbol>.Empty;
        }

    }

}
