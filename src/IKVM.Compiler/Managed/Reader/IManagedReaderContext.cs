using System.Collections.Generic;

namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Provides an interface for a managed entity to call back into its reader.
    /// </summary>
    internal interface IManagedReaderContext
    {

        /// <summary>
        /// Loads the data associated with the given assembly handle.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        void LoadAssembly(ManagedAssembly assembly, out ManagedAssemblyData data);

        /// <summary>
        /// Gets a the set of types available within the assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> ResolveTypes(ManagedAssembly assembly);

        /// <summary>
        /// Searches the managed assembly for the specified type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? ResolveType(ManagedAssembly assembly, string typeName);

        /// <summary>
        /// Gets a the set of types available within the assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<ManagedExportedType> ResolveExportedTypes(ManagedAssembly assembly);

        /// <summary>
        /// Searches the managed assembly for the specified exported type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedExportedType? ResolveExportedType(ManagedAssembly assembly, string typeName);

        /// <summary>
        /// Loads the data associated with the given type handle.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadType(ManagedType type, out ManagedTypeData result);

        /// <summary>
        /// Gets a the set of types available within the assembly.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> ResolveNestedTypes(ManagedType type);

        /// <summary>
        /// Gets a the set of types available within the assembly.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? ResolveNestedType(ManagedType type, string typeName);

    }

}
