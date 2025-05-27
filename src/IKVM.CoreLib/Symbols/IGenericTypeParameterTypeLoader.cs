using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    interface IGenericTypeParameterTypeLoader
    {

        /// <summary>
        /// Gets whether this type is missing.
        /// </summary>
        /// <returns></returns>
        bool GetIsMissing();

        /// <summary>
        /// Gets the declaring type.
        /// </summary>
        /// <returns></returns>
        TypeSymbol GetDeclaringType();

        /// <summary>
        /// Gets the name of the generic type parameter.
        /// </summary>
        string GetName();

        /// <summary>
        /// Gets the position of the generic type parameter.
        /// </summary>
        int GetGenericParameterPosition();

        /// <summary>
        /// Gets the generic parameter attributes.
        /// </summary>
        /// <returns></returns>
        GenericParameterAttributes GetGenericParameterAttributes();

        /// <summary>
        /// Gets the generic parameter constraints.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetGenericParameterConstraints();

        /// <summary>
        /// Gets the optional custom modifiers.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetOptionalCustomModifiers();

        /// <summary>
        /// Gets the required custom modifiers.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetRequiredCustomModifiers();

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<CustomAttribute> GetCustomAttributes();
    }

}
