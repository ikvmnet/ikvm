using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using IKVM.CoreLib.Collections;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// A configuration that is the result of resolution or resolution with service binding.
    /// 
    /// A configuration encapsulates the readability graph that is the output of resolution. A readability graph is a
    /// directed graph whose vertices are of type <see cref="ResolvedModule"/> and the edges represent the readability
    /// amongst the modules. <see cref="ModuleConfiguration"/> defines the modules() method to get the set of resolved
    /// modules in the graph. ResolvedModule defines the reads() method to get the set of modules that a resolved
    /// module reads. The modules that are read may be in the same configuration or may be in parent configurations.
    /// 
    /// Configuration defines the resolve method to resolve a collection of root modules, and the resolveAndBind method to do resolution with service binding. There are instance and static variants of both methods. The instance methods create a configuration with the receiver as the parent configuration. The static methods are for more advanced cases where there can be more than one parent configuration.
    /// </summary>
    internal class ModuleConfiguration
    {

        /// <summary>
        /// Resolves a collection of root modules to create a configuration.
        /// 
        /// Each root module is located using the given <paramref name="before"/> module finder. If a module is not
        /// found then it is located in the parent configuration as if by invoking the <see cref="FindModule(string)"/>
        /// method on each parent in iteration order. If not found then the module is located using the given <paramref
        /// name="after"/> module finder. The same search order is used to locate transitive dependences. Root modules
        /// or dependences that are located in a parent configuration are resolved no further and are not included in
        /// the resulting configuration.
        /// 
        /// When all modules have been enumerated then a readability graph is computed, and in conjunction with the
        /// module exports and service use, checked for consistency.
        /// 
        /// 
        /// 
        /// </summary>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <param name="roots"></param>
        /// <returns></returns>
        public static ModuleConfiguration Resolve(IModuleFinder before, ImmutableArray<ModuleConfiguration> parents, IModuleFinder after, ImmutableArray<string> roots)
        {
            if (before is null)
                throw new ArgumentNullException(nameof(before));
            if (after is null)
                throw new ArgumentNullException(nameof(after));

            var resolver = new ModuleResolver(before, parents, after);
            resolver.Resolve(roots);
            return new ModuleConfiguration(parents, resolver);
        }

        readonly ImmutableArray<ModuleConfiguration> _parents;
        readonly ImmutableDictionary<ResolvedModule, ImmutableHashSet<ResolvedModule>> _graph;
        readonly ImmutableHashSet<ResolvedModule> _modules;
        readonly ImmutableDictionary<string, ResolvedModule> _nameToModule;
        readonly string? _targetPlatform;

        ImmutableArray<ModuleConfiguration> _all;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ModuleConfiguration(ImmutableArray<ModuleConfiguration> parents, ModuleResolver resolver)
        {
            var graph = resolver.Finish(this);

            var nameEntries = new KeyValuePair<string, ResolvedModule>[graph.Count];
            var moduleArray = new ResolvedModule[graph.Count];

            int i = 0;
            foreach (var resolvedModule in graph.Keys)
            {
                moduleArray[i] = resolvedModule;
                nameEntries[i] = new KeyValuePair<string, ResolvedModule>(resolvedModule.Name, resolvedModule);
                i++;
            }

            _parents = parents;
            _graph = graph;
            _modules = moduleArray.ToImmutableHashSet();
            _nameToModule = nameEntries.ToImmutableDictionary();
            _targetPlatform = resolver.TargetPlatform;
        }

        /// <summary>
        /// Returns an unmodifiable list of this configuration's parents, in search order.
        /// </summary>
        public ImmutableArray<ModuleConfiguration> Parents => _parents;

        /// <summary>
        /// Returns an unmodifiable set of the resolved modules in this configuration.
        /// </summary>
        public ImmutableHashSet<ResolvedModule> Modules => _modules;

        /// <summary>
        /// Returns the target platform, if available.
        /// </summary>
        public string? TargetPlatform => _targetPlatform;

        /// <summary>
        /// Resolves a collection of root modules, with this configuration as its parent, to create a new
        /// configuration. This method works exactly as specified by the static {@link #resolve(ModuleFinder,List,ModuleFinder,Collection) resolve}
        /// method when invoked with this configuration as the parent. In other words, if this configuration is {@code cf} then this method is equivalent to
        /// invoking:
        /// <c>
        /// ModuleConfiguration.Resolve(before, [cf], after, roots);
        /// }
        /// </c>
        /// </summary>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <param name="roots"></param>
        /// <returns></returns>
        /// <exception cref="FindException"> If resolution fails for any of the observability-related reasons specified by the static <see cref="Resolve(IModuleFinder, IModuleFinder, ImmutableArray{string})"/> method.</exception>
        /// <exception cref="ResolutionException"></exception>
        public ModuleConfiguration Resolve(IModuleFinder before, IModuleFinder after, ImmutableArray<string> roots)
        {
            if (before is null)
                throw new ArgumentNullException(nameof(before));
            if (after is null)
                throw new ArgumentNullException(nameof(after));

            return Resolve(before, [this], after, roots);
        }

        /// <summary>
        /// Finds a resolved module in this configuration.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResolvedModule? FindModule(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (_nameToModule.TryGetValue(name, out var r))
                return r;

            if (Parents.IsEmpty == false)
            {
                var all = All();
                for (int i = 1; i < all.Length; i++)
                    if (all[i]._nameToModule.TryGetValue(name, out var r2))
                        return r2;
            }

            return null;
        }

        /// <summary>
        /// Gets the set of modules that read the specified module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        internal ImmutableHashSet<ResolvedModule> GetReads(ResolvedModule module)
        {
            return _graph.TryGetValue(module, out var result) ? result : [];
        }

        /// <summary>
        /// Returns an ordered set of <see cref="ModuleConfiguration"/>. The first element is this configuration. The
        /// remaining elements are the parent <see cref="ModuleConfiguration"/> instances in DFS order.
        /// </summary>
        /// <returns></returns>
        internal ImmutableArray<ModuleConfiguration> All()
        {
            if (_all.IsDefault)
                _all = CalculateAll();

            return _all;
        }

        /// <summary>
        /// Calculates the value for <see cref="All"/>.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<ModuleConfiguration> CalculateAll()
        {
            var all = ImmutableArray.CreateBuilder<ModuleConfiguration>(1);

            var v = new HashSet<ModuleConfiguration>();
            var s = new ArrayDeque<ModuleConfiguration>();
            v.Add(this);
            s.InsertFirst(this);

            while (s.IsEmpty == false)
            {
                var layer = s.RemoveFirst();
                all.Add(layer);

                // push in reverse order
                for (int i = layer._parents.Length - 1; i >= 0; i--)
                {
                    var parent = layer._parents[i];
                    if (v.Add(parent))
                        s.InsertFirst(parent);
                }
            }

            return all.DrainToImmutable();
        }

        /// <inheritdoc />
        public override string? ToString()
        {
            return string.Join(", ", _modules.Select(i => i.Name));
        }

    }

}
