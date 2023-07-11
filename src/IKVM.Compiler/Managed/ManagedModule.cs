using System.Collections.Generic;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a manged assembly.
    /// </summary>
    internal class ManagedModule
    {

        readonly IManagedModuleContext context;

        internal string name;
        internal FixedValueList1<ManagedCustomAttribute> customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="customAttributes"></param>
        public ManagedModule(IManagedModuleContext context, string name, in FixedValueList1<ManagedCustomAttribute> customAttributes)
        {
            this.context = context;
            this.name = name;
            this.customAttributes = customAttributes;
        }

        /// <summary>
        /// Gets the context responsible for loading this assembly.
        /// </summary>
        public IManagedModuleContext Context => context;

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Gets the set of custom attributes applied to the assembly.
        /// </summary>
        public ManagedModuleCustomAttributeList CustomAttributes => new ManagedModuleCustomAttributeList(this);

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
