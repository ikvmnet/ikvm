using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class SZArrayTypeSymbol : HasElementSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="elementType"></param>
        public SZArrayTypeSymbol(ISymbolContext context, TypeSymbol elementType) :
            base(context, elementType)
        {

        }

        /// <inheritdoc />
        protected override string NameSuffix => "[]";

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => TypeAttributes.AutoLayout | TypeAttributes.AnsiClass | TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Serializable;

        /// <inheritdoc />
        public sealed override bool IsArray => true;

        /// <inheritdoc />
        public sealed override bool IsSZArray => true;

        /// <inheritdoc />
        public sealed override ITypeSymbol? BaseType => Context.ResolveCoreType("System.Array");

        /// <inheritdoc />
        public sealed override int GetArrayRank()
        {
            return 1;
        }

        /// <inheritdoc />
        public sealed override IImmutableList<IConstructorSymbol> GetDeclaredConstructors()
        {
            return ImmutableList<IConstructorSymbol>.Empty;
        }

        /// <inheritdoc />
        public sealed override IImmutableList<IMethodSymbol> GetDeclaredMethods()
        {
            return ImmutableList<IMethodSymbol>.Empty;
        }

    }

}
