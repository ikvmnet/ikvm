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
        public PointerTypeSymbol(SymbolContext context, TypeSymbol elementType) :
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
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
        {
            return ImmutableArray<TypeSymbol>.Empty;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            return ImmutableArray<MethodSymbol>.Empty;
        }

        /// <inheritdoc />
        internal override MethodImplementationMapping GetMethodImplementations()
        {
            return MethodImplementationMapping.CreateEmpty(this);
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return ImmutableArray<CustomAttribute>.Empty;
        }

        /// <inheritdoc />
        internal override TypeSymbol Specialize(GenericContext context)
        {
            if (ContainsGenericParameters == false)
                return this;

            var elementType = GetElementType() ?? throw new InvalidOperationException();
            return elementType.Specialize(context).MakePointerType();
        }

    }

}
