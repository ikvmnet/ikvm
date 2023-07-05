using System.Collections.Generic;
using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Provides an interface for a <see cref="ManagedType"/> to call back into its loader.
    /// </summary>
    internal class MetadataTypeContext : IManagedTypeContext
    {

        readonly MetadataAssemblyContext assemblyContext;
        readonly MetadataReader reader;
        readonly TypeDefinitionHandle handle;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assemblyContext"></param>
        /// <param name="handle"></param>
        public MetadataTypeContext(MetadataAssemblyContext assemblyContext, MetadataReader reader, TypeDefinitionHandle handle)
        {
            this.assemblyContext = assemblyContext;
            this.reader = reader;
            this.handle = handle;
        }

        /// <summary>
        /// Loads the managed type data.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ManagedTypeData LoadType(ManagedType type) => assemblyContext.LoadType(reader, handle);

    }

}
