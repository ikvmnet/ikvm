using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    interface IParameterLoader
    {

        /// <summary>
        /// Gets whether the event is missing.
        /// </summary>
        bool GetIsMissing();

        /// <summary>
        /// Gets the member.
        /// </summary>
        /// <returns></returns>
        MemberSymbol GetMember();

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        /// <returns></returns>
        string? GetName();

        /// <summary>
        /// Gets the attributes of the parameter.
        /// </summary>
        /// <returns></returns>
        ParameterAttributes GetAttributes();

        /// <summary>
        /// Gets the type of the parameter.
        /// </summary>
        /// <returns></returns>
        TypeSymbol GetParameterType();

        /// <summary>
        /// Gets the position of the parameter.
        /// </summary>
        /// <returns></returns>
        int GetPosition();

        /// <summary>
        /// Gets the default value of the parameter.
        /// </summary>
        /// <returns></returns>
        object? GetDefaultValue();

        /// <summary>
        /// Gets the optional custom modifiers of the parameter.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetOptionalCustomModifiers();

        /// <summary>
        /// Gets the required custom modifiers of the parameter.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetRequiredCustomModifiers();

        /// <summary>
        /// Loads the custom attributes of the event.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<CustomAttribute> GetCustomAttributes();

    }

}