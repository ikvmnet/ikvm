using System;
using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Metadata
{

    internal sealed class MetadataModuleInfo : MetadataEntityInfo, IManagedModuleInfo
    {

        readonly MetadataAssemblyInfo assembly;
        readonly ModuleDefinition module;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="module"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal MetadataModuleInfo(MetadataContext context, MetadataAssemblyInfo assembly, ModuleDefinition module) :
            base(context)
        {
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            this.module = module;
        }

        public string Name => Context.Reader.GetString(module.Name);

    }

}
