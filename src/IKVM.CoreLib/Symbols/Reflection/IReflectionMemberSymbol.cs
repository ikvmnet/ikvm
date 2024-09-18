using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionMemberSymbol : IReflectionSymbol, IMemberSymbol
    {

        /// <summary>
        /// Gets the resolving module.
        /// </summary>
        IReflectionModuleSymbol ResolvingModule { get; }

        /// <summary>
        /// Gets the resolving type.
        /// </summary>
        IReflectionTypeSymbol? ResolvingType { get; }

        /// <summary>
        /// Gets the underlying <see cref="MemberInfo"/>.
        /// </summary>
        MemberInfo UnderlyingMember { get; }

    }

}
