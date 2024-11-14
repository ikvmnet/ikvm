using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class ByRefTypeSymbol : HasElementSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="elementType"></param>
        public ByRefTypeSymbol(SymbolContext context, TypeSymbol elementType) :
            base(context, elementType)
        {

        }

        /// <inheritdoc />
        protected sealed override string NameSuffix => "&";

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => TypeAttributes.Public;

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => null;

        /// <inheritdoc />
        public sealed override bool IsByRef => true;

        /// <inheritdoc />
        public sealed override int GetArrayRank()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetInterfaces()
        {
            return ImmutableArray<TypeSymbol>.Empty;
        }

        /// <inheritdoc />
        public override InterfaceMapping GetInterfaceMap(TypeSymbol interfaceType)
        {
            return new InterfaceMapping(ImmutableList<MethodSymbol>.Empty, interfaceType, ImmutableList<MethodSymbol>.Empty, this);
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<ConstructorSymbol> GetDeclaredConstructors()
        {
            return ImmutableArray<ConstructorSymbol>.Empty;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            return ImmutableArray<MethodSymbol>.Empty;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}
