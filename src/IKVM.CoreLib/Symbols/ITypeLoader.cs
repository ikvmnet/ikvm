using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a source of information for a type definition.
    /// </summary>
    interface ITypeLoader
    {

        /// <summary>
        /// Gets the module of the property.
        /// </summary>
        /// <returns></returns>
        ModuleSymbol GetModule();

        /// <summary>
        /// Gets whether or not the type is missing.
        /// </summary>
        /// <returns></returns>
        bool GetIsMissing();

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <returns></returns>
        string GetName();

        /// <summary>
        /// Gets the namespace of the type.
        /// </summary>
        /// <returns></returns>
        string? GetNamespace();

        /// <summary>
        /// Loads the attributes of the type.
        /// </summary>
        /// <returns></returns>
        TypeAttributes GetAttributes();

        /// <summary>
        /// Loads the declaring type of the type.
        /// </summary>
        /// <returns></returns>
        TypeSymbol? GetDeclaringType();

        /// <summary>
        /// Loads the base type of the type.
        /// </summary>
        /// <returns></returns>
        TypeSymbol? GetBaseType();

        /// <summary>
        /// Loads the generic parameter attributes of the type.
        /// </summary>
        /// <returns></returns>
        GenericParameterAttributes GetGenericParameterAttributes();

        /// <summary>
        /// Loads the generic parameters of the type.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetGenericArguments();

        /// <summary>
        /// Loads the generic parameter constraints of the type.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetGenericParameterConstraints();

        /// <summary>
        /// Loads the nested types of the type.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetNestedTypes();

        /// <summary>
        /// Loads the interfaces implemented by the type.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetInterfaces();

        /// <summary>
        /// Loads the fields of the type.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<FieldSymbol> GetFields();

        /// <summary>
        /// Loads the methods of the type.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<MethodSymbol> GetMethods();

        /// <summary>
        /// Loads the method implementation mapping of the type.
        /// </summary>
        /// <returns></returns>
        MethodImplementationMapping GetMethodImplementations();

        /// <summary>
        /// Loads the properties of the type.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<PropertySymbol> GetProperties();

        /// <summary>
        /// Loads the events of the type.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<EventSymbol> GetEvents();

        /// <summary>
        /// Loads the optional custom modifiers of the type.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetOptionalCustomModifiers();

        /// <summary>
        /// Loads the required custom modifiers of the type.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetRequiredCustomModifiers();

        /// <summary>
        /// Loads the custom attributes of the type.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<CustomAttribute> GetCustomAttributes();

    }

}
