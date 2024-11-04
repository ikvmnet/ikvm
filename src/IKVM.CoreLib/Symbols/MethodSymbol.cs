using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class MethodSymbol : MethodBaseSymbol
    {

        MethodSpecTable _specTable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        protected MethodSymbol(ISymbolContext context, IModuleSymbol module, TypeSymbol? declaringType) :
            base(context, module, declaringType)
        {
            _specTable = new MethodSpecTable(context, module, declaringType, this);
        }

        /// <inheritdoc />
        public override MemberTypes MemberType => MemberTypes.Method;

        /// <inheritdoc />
        public abstract ParameterSymbol ReturnParameter { get; }

        /// <inheritdoc />
        public abstract TypeSymbol ReturnType { get; }

        /// <inheritdoc />
        public abstract ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

        /// <inheritdoc />
        public abstract MethodSymbol GetBaseDefinition();

        /// <inheritdoc />
        public abstract MethodSymbol GetGenericMethodDefinition();

        /// <inheritdoc />
        public MethodSymbol MakeGenericMethod(IImmutableList<TypeSymbol> typeArguments) => _specTable.GetOrCreateGenericMethodSymbol(typeArguments);

    }

}
