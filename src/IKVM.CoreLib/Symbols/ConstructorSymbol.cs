using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class ConstructorSymbol : MethodBaseSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public ConstructorSymbol(SymbolContext context, TypeSymbol declaringType) :
            base(context, declaringType.Module, declaringType)
        {

        }

        /// <inheritdoc />
        public override MemberTypes MemberType => MemberTypes.Constructor;

        /// <inheritdoc />
        public override bool IsGenericMethod => false;

        /// <inheritdoc />
        public override bool IsGenericMethodDefinition => false;

        /// <inheritdoc />
        public override bool ContainsGenericParameters => false;

    }

}