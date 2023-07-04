using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Provides an interface for a <see cref="ManagedType"/> to call back into its loader.
    /// </summary>
    internal class ReflectionTypeContext : IManagedTypeContext
    {

        readonly ReflectionAssemblyContext assembly;
        readonly Type reflectionType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="reflectionType"></param>
        public ReflectionTypeContext(ReflectionAssemblyContext assembly, Type reflectionType)
        {
            this.assembly = assembly;
            this.reflectionType = reflectionType;
        }

        /// <summary>
        /// Resolves the nested types for the given managed type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<ManagedType> ResolveNestedTypes(ManagedType type) => assembly.ResolveNestedTypes(reflectionType);

    }

}
