using System;
using System.Reflection;
using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Metadata
{

    internal sealed class MetadataAssemblyInfo : MetadataEntityInfo, IManagedAssemblyInfo
    {

        readonly AssemblyDefinition assembly;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal MetadataAssemblyInfo(MetadataContext context, AssemblyDefinition assembly) :
            base(context)
        {
            this.assembly = assembly;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public AssemblyName Name => assembly.GetAssemblyName();

    }

}
