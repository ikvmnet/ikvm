using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a source of type definition information.
    /// </summary>
    abstract class DefinitionTypeSymbolSource
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected DefinitionTypeSymbolSource(SymbolContext context)
        {

        }

        /// <summary>
        /// Loads the attributes of the type.
        /// </summary>
        /// <returns></returns>
        public abstract System.Reflection.TypeAttributes LoadAttributes();

        /// <summary>
        /// Loads the name of the type.
        /// </summary>
        /// <returns></returns>
        public abstract string LoadName();

        /// <summary>
        /// Loads the namespace of the type.
        /// </summary>
        /// <returns></returns>
        public abstract string LoadNamespace();

        /// <summary>
        /// Loads the declaring type of the type.
        /// </summary>
        /// <returns></returns>
        public abstract TypeSymbol? LoadDeclaringType();

        /// <summary>
        /// Loads the base type of the type.
        /// </summary>
        /// <returns></returns>
        public abstract TypeSymbol? LoadBaseType();

        /// <summary>
        /// Loads the generic parameters of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> LoadGenericArguments();

        /// <summary>
        /// Loads the generic parameter constraints of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> LoadGenericParameterConstraints();

        /// <summary>
        /// Loads the nested types of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> LoadNestedTypes();

        /// <summary>
        /// Loads the interfaces implemented by the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> LoadInterfaces();

        /// <summary>
        /// Loads the fields of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<FieldSymbol> LoadFields();

        /// <summary>
        /// Loads the methods of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<MethodSymbol> LoadMethods();

        /// <summary>
        /// Loads the method implementation mapping of the type.
        /// </summary>
        /// <returns></returns>
        public abstract MethodImplementationMapping LoadMethodImplementations();

        /// <summary>
        /// Loads the properties of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<PropertySymbol> LoadProperties();

        /// <summary>
        /// Loads the events of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<EventSymbol> LoadEvents();

        /// <summary>
        /// Loads the optional custom modifiers of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> LoadOptionalCustomModifiers();

        /// <summary>
        /// Loads the required custom modifiers of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> LoadRequiredCustomModifiers();

        /// <summary>
        /// Loads the custom attributes of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes();

    }

}
