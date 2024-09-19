using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionMemberSymbol : IIkvmReflectionSymbol, IMemberSymbol
    {

        /// <summary>
        /// Gets the resolving module.
        /// </summary>
        IIkvmReflectionModuleSymbol ResolvingModule { get; }

        /// <summary>
        /// Gets the resolving type.
        /// </summary>
        IIkvmReflectionTypeSymbol? ResolvingType { get; }

        /// <summary>
        /// Gets the underlying <see cref="MemberInfo"/>.
        /// </summary>
        MemberInfo UnderlyingMember { get; }

    }

}
