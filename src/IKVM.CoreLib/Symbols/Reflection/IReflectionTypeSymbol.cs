using System;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionTypeSymbol : IReflectionMemberSymbol, ITypeSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="Type"/>.
        /// </summary>
        Type UnderlyingType { get; }

    }

}
