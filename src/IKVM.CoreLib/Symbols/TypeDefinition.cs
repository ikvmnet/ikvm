using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a source of type definition information.
    /// </summary>
    abstract class TypeDefinition
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected TypeDefinition(SymbolContext context)
        {

        }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();

        /// <summary>
        /// Gets the namespace of the type.
        /// </summary>
        /// <returns></returns>
        public abstract string GetNamespace();

        /// <summary>
        /// Loads the attributes of the type.
        /// </summary>
        /// <returns></returns>
        public abstract TypeAttributes GetAttributes();

        /// <summary>
        /// Loads the declaring type of the type.
        /// </summary>
        /// <returns></returns>
        public abstract TypeSymbol? GetDeclaringType();

        /// <summary>
        /// Loads the base type of the type.
        /// </summary>
        /// <returns></returns>
        public abstract TypeSymbol? GetBaseType();

        /// <summary>
        /// Loads the generic parameter attributes of the type.
        /// </summary>
        /// <returns></returns>
        public abstract GenericParameterAttributes GetGenericParameterAttributes();

        /// <summary>
        /// Loads the generic parameters of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> GetGenericArguments();

        /// <summary>
        /// Loads the generic parameter constraints of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> GetGenericParameterConstraints();

        /// <summary>
        /// Loads the nested types of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> GetNestedTypes();

        /// <summary>
        /// Loads the interfaces implemented by the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> GetInterfaces();

        /// <summary>
        /// Loads the fields of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<FieldSymbol> GetFields();

        /// <summary>
        /// Loads the methods of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<MethodSymbol> GetMethods();

        /// <summary>
        /// Loads the method implementation mapping of the type.
        /// </summary>
        /// <returns></returns>
        public abstract MethodImplementationMapping GetMethodImplementations();

        /// <summary>
        /// Loads the properties of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<PropertySymbol> GetProperties();

        /// <summary>
        /// Loads the events of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<EventSymbol> GetEvents();

        /// <summary>
        /// Loads the optional custom modifiers of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> GetOptionalCustomModifiers();

        /// <summary>
        /// Loads the required custom modifiers of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> GetRequiredCustomModifiers();

        /// <summary>
        /// Loads the custom attributes of the type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<CustomAttribute> GetCustomAttributes();

        /// <summary>
        /// Reslves the nested type definition given by the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract TypeDefinition? ResolveNestedTypeDef(string name);

        /// <summary>
        /// Resolves the field definition given by the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract FieldDefinition? ResolveFieldDef(string name);

        /// <summary>
        /// Resolves the method definition given by the specified signature.
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public abstract MethodDefinition? ResolveMethodDef(MethodSymbolSignature signature);

        /// <summary>
        /// Resolves the property definition given by the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract PropertyDefinition? ResolvePropertyDef(string name);

        /// <summary>
        /// Resolves the event definition given by the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract EventDefinition? ResolveEventDef(string name);

    }

}
