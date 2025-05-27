using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a source of information for a <see cref="DefinitionFieldSymbol"/>.
    /// </summary>
    interface IFieldLoader
    {

        /// <summary>
        /// Gets whether or not this field is missing.
        /// </summary>
        /// <returns></returns>
        bool GetIsMissing();

        /// <summary>
        /// Gets the declaring module.
        /// </summary>
        /// <returns></returns>
        ModuleSymbol GetModule();

        /// <summary>
        /// Gets the declaring type.
        /// </summary>
        /// <returns></returns>
        TypeSymbol? GetDeclaringType();

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <returns></returns>
        string GetName();

        /// <summary>
        /// Gets the attributes of the field.
        /// </summary>
        /// <returns></returns>
        FieldAttributes GetAttributes();

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        /// <returns></returns>
        TypeSymbol GetFieldType();

        /// <summary>
        /// Gets the constant value of the field.
        /// </summary>
        /// <returns></returns>
        object? GetConstantValue();

        /// <summary>
        /// Loads the optional custom modifiers of the field.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetOptionalCustomModifiers();

        /// <summary>
        /// Loads the required custom modifiers of the field.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetRequiredCustomModifiers();

        /// <summary>
        /// Loads the custom attributes of the field.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<CustomAttribute> GetCustomAttributes();

    }

}
