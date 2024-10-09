using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    readonly record struct CustomAttribute(
        ITypeSymbol AttributeType,
        IConstructorSymbol Constructor,
        ImmutableArray<CustomAttributeTypedArgument> ConstructorArguments,
        ImmutableArray<CustomAttributeNamedArgument> NamedArguments);


}
