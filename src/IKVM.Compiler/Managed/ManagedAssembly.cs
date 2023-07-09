using System.Collections.Generic;
using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a manged assembly.
    /// </summary>
    public class ManagedAssembly
    {

        readonly IManagedAssemblyContext context;

        internal readonly AssemblyName name;
        internal readonly ReadOnlyFixedValueList1<ManagedCustomAttribute> customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="customAttributes"></param>
        public ManagedAssembly(IManagedAssemblyContext context, AssemblyName name, ReadOnlyFixedValueList1<ManagedCustomAttribute> customAttributes)
        {
            this.context = context;

            this.name = name;
            this.customAttributes = customAttributes;
        }

        /// <summary>
        /// Gets the context responsible for loading this assembly.
        /// </summary>
        public IManagedAssemblyContext Context => context;

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        public AssemblyName Name => name;

        /// <summary>
        /// Gets the set of custom attributes applied to the assembly.
        /// </summary>
        public ManagedAssemblyCustomAttributeList CustomAttributes => new ManagedAssemblyCustomAttributeList(this);

        /// <summary>
        /// Attempts to resolve a type with the specified name from the managed assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public ManagedType? ResolveType(string typeName) => Context.ResolveType(this, typeName);

        /// <summary>
        /// Provides resolution of types within the assembly.
        /// </summary>
        public IEnumerable<ManagedType> ResolveTypes() => Context.ResolveTypes(this);

    }

}
