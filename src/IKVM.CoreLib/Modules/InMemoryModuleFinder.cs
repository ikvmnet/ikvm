using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// <see cref="IModuleFinder"/> implementation that takes a static list of <see cref="ModuleReference"/>s.
    /// </summary>
    internal class InMemoryModuleFinder : ModuleFinder
    {

        readonly Dictionary<string, ModuleReference> _map;
        readonly ImmutableHashSet<ModuleReference> _all;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="references"></param>
        public InMemoryModuleFinder(ImmutableArray<ModuleReference> references)
        {
            _map = references.ToDictionary(i => i.Descriptor.Name);
            _all = references.ToImmutableHashSet();
        }

        /// <inheritdoc />
        public override ModuleReference? Find(string name)
        {
            return _map.GetValueOrDefault(name);
        }

        /// <inheritdoc />
        public override ImmutableHashSet<ModuleReference> FindAll()
        {
            return _all;
        }

    }

}
