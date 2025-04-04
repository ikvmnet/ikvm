using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// <see cref="IModuleFinder"/> implementation that combines multiple <see cref="IModuleFinder"/>s in order.
    /// </summary>
    class ComposableModuleFinder : ModuleFinder
    {

        readonly ImmutableArray<IModuleFinder> _finders;
        readonly Dictionary<string, ModuleReference> _nameToModule = new();

        ImmutableHashSet<ModuleReference>? _all;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="finders"></param>
        public ComposableModuleFinder(ImmutableArray<IModuleFinder> finders)
        {
            _finders = finders;
        }

        /// <inheritdoc />
        public override ModuleReference? Find(string name)
        {
            // already cached
            if (_nameToModule.TryGetValue(name, out var mref))
                return mref;

            // scan finders
            foreach (var finder in _finders)
                if (finder.Find(name) is { } mref2)
                    return _nameToModule[name] = mref2;

            return null;
        }

        /// <inheritdoc />
        public override ImmutableHashSet<ModuleReference> FindAll()
        {
            if (_all == null)
            {
                // start with existing cache
                var all = ImmutableHashSet.CreateBuilder<ModuleReference>();
                foreach (var mref in _nameToModule.Values)
                    all.Add(mref);

                // add any missing items 
                foreach (var finder in _finders)
                    foreach (var mref in finder.FindAll())
                        if (_nameToModule.TryAdd(mref.Descriptor.Name, mref))
                            all.Add(mref);

                // set result
                Interlocked.CompareExchange(ref _all, all.ToImmutable(), null);
            }

            return _all;
        }

    }

}