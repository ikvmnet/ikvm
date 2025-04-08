using System;
using System.Collections.Immutable;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// A module in a graph of resolved modules.
    /// </summary>
    internal class ResolvedModule
    {

        readonly ModuleConfiguration _configuration;
        readonly ModuleReference _reference;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="reference"></param>
        public ResolvedModule(ModuleConfiguration configuration, ModuleReference reference)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _reference = reference ?? throw new ArgumentNullException(nameof(reference));
        }

        /// <summary>
        /// Returns the configuration that this resolved module is in.
        /// </summary>
        public ModuleConfiguration Configuration => _configuration;

        /// <summary>
        /// Returns the reference to the module's content.
        /// </summary>
        public ModuleReference Reference => _reference;

        /// <summary>
        /// Returns the module descriptor.
        /// </summary>
        public ModuleDescriptor Descriptor => _reference.Descriptor;

        /// <summary>
        /// Returns the module name.
        /// </summary>
        public string Name => _reference.Descriptor.Name;

        /// <summary>
        /// Returns the set of resolved modules that this resolved module reads.
        /// </summary>
        public ImmutableHashSet<ResolvedModule> Reads => _configuration.GetReads(this);

        /// <inheritdoc />
        public override string? ToString()
        {
            return Name;
        }

    }

}
