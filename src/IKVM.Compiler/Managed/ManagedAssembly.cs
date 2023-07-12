using System.Collections.Generic;
using System.Reflection;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a manged assembly.
    /// </summary>
    internal sealed class ManagedAssembly
    {

        readonly IManagedAssemblyContext context;
        readonly object handle;
        readonly AssemblyName name;

        internal ManagedAssemblyData data;
        bool load = true;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        public ManagedAssembly(IManagedAssemblyContext context, object handle, AssemblyName name)
        {
            this.context = context;
            this.handle = handle;
            this.name = name;
        }

        /// <summary>
        /// If necessary, loads the assembly.
        /// </summary>
        void LazyLoad()
        {
            // multiple threads may enter load at the same time, but this should be safe
            if (load)
            {
                context.LoadAssembly(this, out data);
                load = false;
            }
        }

        /// <summary>
        /// Gets the assembly context that loaded this managed assembly.
        /// </summary>
        public IManagedAssemblyContext Context => context;

        /// <summary>
        /// Gets the internal handle to the underlying assembly source.
        /// </summary>
        public object Handle => handle;

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        public AssemblyName Name => name;

        /// <summary>
        /// Gets the set of custom attributes applied to the assembly.
        /// </summary>
        public ManagedAssemblyCustomAttributeList CustomAttributes
        {
            get
            {
                LazyLoad();
                return new ManagedAssemblyCustomAttributeList(this);
            }
        }

        /// <summary>
        /// Gets the set of references to other assemblies.
        /// </summary>
        public ManagedAssemblyReferenceList References
        {
            get
            {
                LazyLoad();
                return new ManagedAssemblyReferenceList(this);
            }
        }

        /// <summary>
        /// Gets the types that make up this assembly.
        /// </summary>
        public IEnumerable<ManagedType> ResolveTypes() => context.ResolveTypes(this);

        /// <summary>
        /// Attempts to resolve a type with the specified name from the managed assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public ManagedType? ResolveType(string typeName) => context.ResolveType(this, typeName);

        /// <summary>
        /// Gets the exported types that make up this assembly.
        /// </summary>
        public IEnumerable<ManagedExportedType> ResolveExportedTypes() => context.ResolveExportedTypes(this);

        /// <summary>
        /// Attempts to resolve an exported type with the specified name from the managed assembly.
        /// </summary>
        public ManagedExportedType? ResolveExportedType(string typeName) => context.ResolveExportedType(this, typeName);


    }

}
