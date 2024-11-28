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
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
        {
            return ImmutableArray<TypeSymbol>.Empty;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            return ImmutableArray<MethodSymbol>.Empty;
        }

        internal override MethodImplementationMapping GetMethodImplementations()
        {
            return MethodImplementationMapping.CreateEmpty(this);
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return ImmutableArray<CustomAttribute>.Empty;
        }

        /// <inheritdoc />
        internal override TypeSymbol Specialize(GenericContext context)
        {
            if (ContainsGenericParameters == false)
                return this;

            var elementType = GetElementType() ?? throw new InvalidOperationException();
            return elementType.Specialize(context).MakeByRefType();
        }

    }

}
