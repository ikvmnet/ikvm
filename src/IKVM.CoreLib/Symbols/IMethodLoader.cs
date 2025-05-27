using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a source of information for a <see cref="DefinitionMethodSymbol" />.
    /// </summary>
    interface IMethodLoader
    {

        /// <summary>
        /// Gets whether or not this method is missing.
        /// </summary>
        /// <returns></returns>
        bool GetIsMissing();

        /// <summary>
        /// Gets the module of the method.
        /// </summary>
        /// <returns></returns>
        ModuleSymbol GetModule();

        /// <summary>
        /// Gets the declaring type of the method.
        /// </summary>
        /// <returns></returns>
        TypeSymbol? GetDeclaringType();

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns></returns>
        string GetName();

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <returns></returns>
        MethodAttributes GetAttributes();

        /// <summary>
        /// Gets the calling convention.
        /// </summary>
        /// <returns></returns>
        CallingConventions GetCallingConvention();

        /// <summary>
        /// Gets the method implementation flags.
        /// </summary>
        /// <returns></returns>
        MethodImplAttributes GetMethodImplementationFlags();

        /// <summary>
        /// Gets the generic arguments.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetGenericArguments();

        /// <summary>
        /// Gets the return parameter.
        /// </summary>
        /// <returns></returns>
        ParameterSymbol GetReturnParameter();

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<ParameterSymbol> GetParameters();

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<CustomAttribute> GetCustomAttributes();
    }

}
