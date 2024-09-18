using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionMethodBaseSymbol : IReflectionMemberSymbol, IMethodBaseSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="ConstructorInfo"/>.
        /// </summary>
        MethodBase UnderlyingMethodBase { get; }

    }

}
