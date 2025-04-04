using System.Collections.Immutable;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// <see cref="IModuleFinder"/> implementation that returns no modules.
    /// </summary>
    class EmptyModuleFinder : ModuleFinder
    {

        /// <summary>
        /// Gets the default instance of the empty module finder.
        /// </summary>
        public static readonly EmptyModuleFinder Instance = new EmptyModuleFinder();

        /// <inheritdoc />
        public override ModuleReference? Find(string name)
        {
            return null;
        }

        /// <inheritdoc />
        public override ImmutableHashSet<ModuleReference> FindAll()
        {
            return ImmutableHashSet<ModuleReference>.Empty;
        }

    }

}