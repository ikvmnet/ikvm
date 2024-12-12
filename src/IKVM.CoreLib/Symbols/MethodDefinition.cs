using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class MethodDefinition
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected MethodDefinition(SymbolContext context)
        {

        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <returns></returns>
        public abstract MethodAttributes GetAttributes();

        /// <summary>
        /// Gets the calling convention.
        /// </summary>
        /// <returns></returns>
        public abstract CallingConventions GetCallingConvention();

        /// <summary>
        /// Gets the method implementation flags.
        /// </summary>
        /// <returns></returns>
        public abstract MethodImplAttributes GetMethodImplementationFlags();

        /// <summary>
        /// Gets the generic arguments.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> GetGenericArguments();

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<ParameterSymbol> GetParameters();

        /// <summary>
        /// Gets the return parameter.
        /// </summary>
        /// <returns></returns>
        public abstract ParameterSymbol GetReturnParameter();

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<CustomAttribute> GetCustomAttributes();

    }

}
