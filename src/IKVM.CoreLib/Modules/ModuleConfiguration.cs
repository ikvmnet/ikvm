using System;
using System.Collections.Generic;
using System.Collections.Immutable;

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
        /// </summary>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <param name="roots"></param>
        /// <returns></returns>
        public static ModuleConfiguration Resolve(IModuleFinder before, IModuleFinder after, ImmutableArray<string> roots)
        {
            throw new NotImplementedException();
        }

        readonly ImmutableArray<ModuleConfiguration> _parents;
        readonly Dictionary<ResolvedModule, HashSet<ResolvedModule>> _graph;
        readonly HashSet<ResolvedModule> _modules;
        readonly Dictionary<string, ResolvedModule> _nameToModules;

        readonly string _targetPlatform;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ModuleConfiguration(ImmutableArray<string> parents)
        {
            
        }

        /// <summary>
        /// Returns an unmodifiable set of the resolved modules in this configuration.
        /// </summary>
        public ImmutableHashSet<ResolvedModule> Modules { get; }

        /// <summary>
        /// Finds a resolved module in this configuration.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResolvedModule? FindModule(string name);

    }

}
