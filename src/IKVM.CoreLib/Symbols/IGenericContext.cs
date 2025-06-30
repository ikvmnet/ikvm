using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    interface IGenericContext
    {

        /// <summary>
        /// Gets the generic type argument for the specified index.
        /// </summary>
        TypeSymbol GetGenericTypeArgument(int index);

        /// <summary>
        /// Gets the generic method argument for the specified index.
        /// </summary>
        TypeSymbol GetGenericMethodArgument(int index);

    }

}
