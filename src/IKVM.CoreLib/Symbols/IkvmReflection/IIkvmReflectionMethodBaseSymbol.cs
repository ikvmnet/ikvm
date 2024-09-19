using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionMethodBaseSymbol : IIkvmReflectionMemberSymbol, IMethodBaseSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="MethodBase"/>.
        /// </summary>
        MethodBase UnderlyingMethodBase { get; }

    }

}
