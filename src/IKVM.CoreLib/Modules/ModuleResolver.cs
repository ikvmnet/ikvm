using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using IKVM.ByteCode;
using IKVM.CoreLib.Collections;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// The resolver used by <see cref="ModuleConfiguration"/> and <see cref="ModuleConfiguration.Resolve(IModuleFinder, IModuleFinder, ImmutableArray{string})"/>.
    /// </summary>
    sealed class ModuleResolver
    {

        readonly IModuleFinder _beforeFinder;
        readonly ImmutableArray<ModuleConfiguration> _parents;
        readonly IModuleFinder _afterFinder;
        readonly string? _targetPlatform;
        readonly Dictionary<string, ModuleReference> _nameToReference = new();

        bool _haveAllAutomaticModules;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="beforeFinder"></param>
        /// <param name="parents"></param>
        /// <param name="afterFinder"></param>
        public ModuleResolver(IModuleFinder beforeFinder, ImmutableArray<ModuleConfiguration> parents, IModuleFinder afterFinder)
        {
            _beforeFinder = beforeFinder;
            _parents = parents;
            _afterFinder = afterFinder;

            foreach (var parent in _parents)
            {
                var value = parent.TargetPlatform;
                if (value is not null)
                {
                    if (_targetPlatform is null)
                    {
                        _targetPlatform = value;
                    }
                    else
                    {
                        if (value != _targetPlatform)
                            throw new ArgumentException($"Parents have conflicting constraints on target platform: {_targetPlatform}, {value}.", nameof(parents));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the constraint on the target platform.
        /// </summary>
        public string? TargetPlatform => _targetPlatform;

        /// <summary>
        /// Resolves the given named modules.
        /// </summary>
        /// <param name="roots"></param>
        /// <returns></returns>
        internal ModuleResolver Resolve(ImmutableArray<string> roots)
        {
            var q = new ArrayDeque<ModuleDescriptor>();

            foreach (var root in roots)
            {
                var mref = FindWithBeforeFinder(root);
                if (mref is null)
                {
                    if (FindInParent(root) is not null)
                        continue;

                    mref = FindWithAfterFinder(root);
                    if (mref is null)
                        throw new FindException($"Module {root} not found.");
                }

                AddFoundModule(mref);
                q.InsertFirst(mref.Descriptor);
            }

            Resolve(q);

            return this;
        }

        /// <summary>
        /// Resolve all modules in the given queue. On completion the queue will be empty and any resolved modules will
        /// be added to <see cref="_nameToReference"/>.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        HashSet<ModuleDescriptor> Resolve(ArrayDeque<ModuleDescriptor> q)
        {
            var resolved = new HashSet<ModuleDescriptor>();

            while (q.IsEmpty == false)
            {
                var descriptor = q.RemoveFirst();
                if (_nameToReference.ContainsKey(descriptor.Name) == false)
                    throw new InvalidOperationException();

                // if the module is an automatic module then all automatic modules need to be resolved
                if (descriptor.IsAutomatic && !_haveAllAutomaticModules)
                {
                    foreach (var mref in AddFoundAutomaticModules())
                        q.InsertLast(mref.Descriptor);

                    _haveAllAutomaticModules = true;
                }

                // process dependences
                foreach (var requires in descriptor.Requires)
                {
                    // only required at compile-time
                    if ((requires.Modifiers & ByteCode.ModuleRequiresFlag.StaticPhase) != 0)
                        continue;

                    var dn = requires.Name;

                    // find dependence
                    var mref = FindWithBeforeFinder(dn);
                    if (mref == null)
                    {
                        if (FindInParent(dn) != null)
                            continue; // dependence is in parent

                        mref = FindWithAfterFinder(dn);
                        if (mref == null)
                            throw new FindException($"Module {dn} not found, required by {descriptor}.");
                    }

                    if (_nameToReference.ContainsKey(dn) == false)
                    {
                        AddFoundModule(mref);
                        q.InsertLast(mref.Descriptor);
                    }

                }

                resolved.Add(descriptor);
            }

            return resolved;
        }

        /// <summary>
        /// Adds all automatic modules that have not already been found to the <see cref="_nameToReference"/> set.
        /// </summary>
        /// <returns></returns>
        HashSet<ModuleReference> AddFoundAutomaticModules()
        {
            var result = new HashSet<ModuleReference>();

            foreach (var mref in FindAll())
            {
                var mn = mref.Descriptor.Name;
                if (mref.Descriptor.IsAutomatic && !_nameToReference.ContainsKey(mn))
                {
                    AddFoundModule(mref);
                    result.Add(mref);
                }
            }

            return result;
        }

        /// <summary>
        /// Add the module to the <see cref="_nameToReference"/> set and check any constraints on the target platform
        /// with the constraints of other modules.
        /// </summary>
        /// <param name="mref"></param>
        void AddFoundModule(ModuleReference mref)
        {
            _nameToReference.Add(mref.Descriptor.Name, mref);
        }

        /// <summary>
        /// Execute post-resolution checks and returns the module graph of resolved modules.
        /// </summary>
        /// <param name="cf"></param>
        /// <returns></returns>
        internal ImmutableDictionary<ResolvedModule, ImmutableHashSet<ResolvedModule>> Finish(ModuleConfiguration cf)
        {
            DetectCycles();
            CheckHashes();
            var graph = MakeGraph(cf);
            CheckExportSuppliers(graph);
            return graph;
        }

        /// <summary>
        /// Checks the given module graph for cycles. For now the implementation is a simple depth first search on the dependecy graph.
        /// </summary>
        void DetectCycles()
        {
            var visited = new HashSet<ModuleDescriptor>();
            var visitedPath = new Stack<ModuleDescriptor>();
            var visitedPathSet = new HashSet<ModuleDescriptor>();

            foreach (var mref in _nameToReference.Values)
                Visit(mref.Descriptor, visited, visitedPath, visitedPathSet);
        }

        /// <summary>
        /// Navigates through the 'requires' graph of the given descriptor until a cycle is detected.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <exception cref="InvalidOperationException"></exception>
        void Visit(ModuleDescriptor descriptor, HashSet<ModuleDescriptor> visited, Stack<ModuleDescriptor> visitedPath, HashSet<ModuleDescriptor> visitedPathSet)
        {
            if (visited.Contains(descriptor) == false)
            {
                var added = visitedPathSet.Add(descriptor);
                if (added == false)
                    throw new InvalidOperationException($"Cycle detected: {CycleToString(descriptor, visitedPath)}.");

                visitedPath.Push(descriptor);

                // visit the requires branch to detect cycles
                foreach (var requires in descriptor.Requires)
                {
                    if (_nameToReference.TryGetValue(requires.Name, out var mref))
                    {
                        var other = mref.Descriptor;
                        if (other != descriptor)
                            Visit(other, visited, visitedPath, visitedPathSet);
                    }
                }

                visitedPathSet.Remove(descriptor);
                visitedPath.Pop();

                visited.Add(descriptor);
            }
        }

        /// <summary>
        /// Returns a string with a list of the modules in a detected cycle.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="visitedPath"></param>
        /// <returns></returns>
        string CycleToString(ModuleDescriptor descriptor, Stack<ModuleDescriptor> visitedPath)
        {
            // existing path with current descriptor appended
            var l = new List<ModuleDescriptor>(visitedPath);
            l.Add(descriptor);
            var p = l.IndexOf(descriptor);

            return string.Join(" -> ", l.Skip(p).Select(i => i.Name));
        }

        /// <summary>
        /// Checks the hashes in the module descriptor to ensure that they match any recorded hashes.
        /// </summary>
        void CheckHashes()
        {
            foreach (var mref in _nameToReference.Values)
            {
                continue; // TODO implement hashes
            }
        }

        /// <summary>
        /// Computes the readability graph for the modules in the given Configuration.
        /// </summary>
        /// <remarks>
        /// The readability graph is created by propagating "requires" through the "requires transitive" edges of the
        /// module dependence graph. So if the module dependence graph has m1 requires m2 && m2 requires transitive m3
        /// then the resulting readability graph will contain m1 reads m2, m1 reads m3, and m2 reads m3.
        /// </remarks>
        /// <param name="cf"></param>
        /// <returns></returns>
        ImmutableDictionary<ResolvedModule, ImmutableHashSet<ResolvedModule>> MakeGraph(ModuleConfiguration cf)
        {
            // the "reads" graph starts as a module dependence graph and is iteratively updated to be the readability graph
            var g1 = ImmutableDictionary.CreateBuilder<ResolvedModule, ImmutableHashSet<ResolvedModule>>();

            // need "requires transitive" from the modules in parent configurations
            // as there may be selected modules that have a dependency on modules in
            // the parent configuration.
            var g2 = _parents
                .SelectMany(i => i.All())
                .Distinct()
                .SelectMany(c => c.Modules
                    .SelectMany(m1 => m1.Descriptor.Requires
                        .Where(r => (r.Modifiers & ModuleRequiresFlag.Transitive) != 0)
                        .Select(r => c.FindModule(r.Name))
                        .Where(m2 => m2 != null)
                        .ToDictionary(m2 => m1, m2 => m2)))
                .GroupBy(i => i.Key)
                .ToDictionary(i => i.Key, i => i.Select(j => j.Value!).ToHashSet()!);

            var nameToResolved = new Dictionary<string, ResolvedModule>(_nameToReference.Count);

            foreach (var mref in _nameToReference.Values)
            {
                var desc = mref.Descriptor;
                var name = mref.Descriptor.Name;

                var m1 = GetOrAddResolvedModule(nameToResolved, name, cf, mref);

                var reads = ImmutableHashSet.CreateBuilder<ResolvedModule>();
                var requiresTransitive = new HashSet<ResolvedModule>();

                foreach (var requires in desc.Requires)
                {
                    var dn = requires.Name;

                    ResolvedModule? m2;
                    if (_nameToReference.TryGetValue(dn, out var mref2))
                    {
                        m2 = GetOrAddResolvedModule(nameToResolved, dn, cf, mref2);
                    }
                    else
                    {
                        m2 = FindInParent(dn);
                        if (m2 is null)
                            continue;

                        if (m2.Descriptor.IsAutomatic)
                            foreach (var d in m2.Reads)
                                if (d.Descriptor.IsAutomatic)
                                    reads.Add(d);
                    }

                    reads.Add(m2);

                    if ((requires.Modifiers & ModuleRequiresFlag.Transitive) != 0)
                        requiresTransitive.Add(m2);
                }

                // automatic modules read all selected modules and all modules in parent configurations
                if (desc.IsAutomatic)
                {
                    foreach (var mref2 in _nameToReference.Values)
                    {
                        var desc2 = mref2.Descriptor;
                        var name2 = desc2.Name;

                        if (name != name2)
                        {
                            var m2 = GetOrAddResolvedModule(nameToResolved, name2, cf, mref2);
                            reads.Add(m2);
                            if (desc2.IsAutomatic)
                                requiresTransitive.Add(m2);
                        }
                    }

                    // reads all module sin parent configurations
                    foreach (var parent in _parents)
                    {
                        foreach (var m in parent.All().SelectMany(i => i.Modules))
                        {
                            reads.Add(m);
                            if (m.Reference.Descriptor.IsAutomatic)
                                requiresTransitive.Add(m);
                        }
                    }
                }

                g1.Add(m1, reads.ToImmutable());
                g2.Add(m1, requiresTransitive!);
            }

            var staging = new List<ResolvedModule>();
            var changed = false;
            do
            {
                changed = false;

                foreach (var e in g1)
                {
                    var m1 = e.Key;

                    // automatic module already reads all selected modules so nothing to propagate
                    if (m1.Descriptor.IsAutomatic == false)
                    {
                        foreach (var m2 in e.Value)
                            if (g2.TryGetValue(m2, out var m2RequiresTransitive))
                                foreach (var m3 in m2RequiresTransitive)
                                    if (e.Value.Contains(m3) == false)
                                        staging.Add(m3);

                        if (staging.Count > 0)
                        {
                            foreach (var i in staging)
                                e.Value.Add(i);

                            staging.Clear();
                            changed = true;
                        }
                    }
                }
            }
            while (changed);

            return g1.ToImmutable();
        }

        /// <summary>
        /// Gets or adds a new <see cref="ResolvedModule"/> by name.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="name"></param>
        /// <param name="cf"></param>
        /// <param name="mref"></param>
        /// <returns></returns>
        ResolvedModule GetOrAddResolvedModule(Dictionary<string, ResolvedModule> map, string name, ModuleConfiguration cf, ModuleReference mref)
        {
            if (map.TryGetValue(name, out var m) == false)
            {
                m = new ResolvedModule(cf, mref);
                map.Add(name, m);
            }

            return m;
        }

        /// <summary>
        /// Checks the readability graph to ensure that:
        /// 
        /// <list type="bullet">
        ///     <item>A module does not read two or more module with the same name. This includes the case where aa
        ///     module reads another module with the same name as itself.</item>
        ///     <item>Two or more modules in the configuration don't export the same package to a module that reads
        ///     both. This includes the case where a module <c>M</c> containing package <c>p</c> to <c>M</c>.</item>
        ///     <item>A module <c>M</c> doesn't declare that it "<c>uses p.S</c>" or "<c>provides p.S with ...</c>"
        ///     but package <c>p</c> is neither in module <c>M</c> nor exported to <c>M</c> by any module that
        ///     <c>M</c> reads.</item>
        /// </list>
        /// </summary>
        /// <param name="graph"></param>
        void CheckExportSuppliers(ImmutableDictionary<ResolvedModule, ImmutableHashSet<ResolvedModule>> graph)
        {
            foreach (var e in graph)
            {
                var desc1 = e.Key.Descriptor;
                var name1 = desc1.Name;

                // the names of the modules that are read (including self)
                var names = new HashSet<string>();
                names.Add(name1);

                // the map of packages that are local or exported to descriptor1
                var packageToExporter = new Dictionary<string, ModuleDescriptor>();

                // local packages
                var packages = desc1.Packages;
                foreach (var pn in packages)
                    packageToExporter.Add(pn, desc1);

                // descriptor1 reads descriptor2
                foreach (var endpoint in e.Value)
                {
                    var desc2 = endpoint.Descriptor;
                    var name2 = desc2.Name;

                    if (desc2 != desc1 && names.Add(name2) == false)
                    {
                        if (name2 == name1)
                            throw new InvalidOperationException($"Module {name1} reads another module named {name1}.");
                        else
                            throw new InvalidOperationException($"Module {name1} reads more than on module named {name2}.");
                    }

                    if (desc2.IsAutomatic)
                    {
                        // automatic modules read self and export all packages
                        if (desc2 != desc1)
                            foreach (var source in desc2.Packages)
                                if (TryAdd(packageToExporter, source, desc2, out var supplier) == false)
                                    throw FailTwoSuppliers(desc1, source, desc2, supplier); // descriptor2 and 'supplier' export source to descriptor1
                    }
                    else
                    {
                        foreach (var export in desc2.Exports)
                        {
                            if (export.IsQualified)
                                if (export.Targets.Contains(desc1.Name) == false)
                                    continue;

                            if (TryAdd(packageToExporter, export.Source, desc2, out var supplier) == false)
                                throw FailTwoSuppliers(desc1, export.Source, desc2, supplier); // descriptor2 and 'supplier' export source to descriptor1
                        }
                    }
                }

                // uses/provides checks not applicable to automatic modules
                if (desc1.IsAutomatic == false)
                {
                    // uses S
                    foreach (var service in desc1.Uses)
                    {
                        var pn = PackageName(service);
                        if (packageToExporter.ContainsKey(pn) == false && RequiresStaticMissingModule(desc1, e.Value) == false)
                            throw new ResolutionException($"Module {desc1.Name} uses {service} but does not read a module that exports {pn} to {desc1.Name}.");
                    }

                    // provides S
                    foreach (var provides in desc1.Provides)
                    {
                        var pn = PackageName(provides.Service);
                        if (packageToExporter.ContainsKey(pn) == false && RequiresStaticMissingModule(desc1, e.Value) == false)
                            throw new ResolutionException($"Module {desc1.Name} provides {provides.Service} but does not read a module that exports {pn} to {desc1.Name}.");
                    }
                }
            }
        }

        /// <summary>
        /// Returns <c>true</c> if a module 'requres static' a module that is not in the readability graph, or reads a
        /// module that 'requires static transitive' a module that is not in the readability graph.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="reads"></param>
        /// <returns></returns>
        bool RequiresStaticMissingModule(ModuleDescriptor descriptor, ImmutableHashSet<ResolvedModule> reads)
        {
            var moduleNames = reads
                .Select(i => i.Name)
                .ToHashSet();

            foreach (var r in descriptor.Requires)
                if ((r.Modifiers & ByteCode.ModuleRequiresFlag.StaticPhase) != 0 && moduleNames.Contains(r.Name) == false)
                    return true;

            foreach (var rm in reads)
                foreach (var r in rm.Descriptor.Requires)
                    if ((r.Modifiers & ModuleRequiresFlag.StaticPhase) != 0 && (r.Modifiers & ModuleRequiresFlag.Transitive) != 0 && moduleNames.Contains(r.Name) == false)
                        return true;

            return false;
        }

        /// <summary>
        /// Find a module of the given name in the parent configuration.
        /// </summary>
        /// <param name="mn"></param>
        /// <returns></returns>
        ResolvedModule? FindInParent(string mn)
        {
            foreach (var parent in _parents)
                if (parent.FindModule(mn) is { } m)
                    return m;

            return null;
        }

        /// <summary>
        /// Invokes the before finder to find the given module.
        /// </summary>
        /// <param name="mn"></param>
        /// <returns></returns>
        ModuleReference? FindWithBeforeFinder(string mn)
        {
            return _beforeFinder.Find(mn);
        }

        /// <summary>
        /// Invokes the after finder to find the given module.
        /// </summary>
        /// <param name="mn"></param>
        /// <returns></returns>
        ModuleReference? FindWithAfterFinder(string mn)
        {
            return _afterFinder.Find(mn);
        }

        /// <summary>
        /// Returns the set of all modules that are observable with the before and after <see cref="IModuleFinder"/>s.
        /// </summary>
        /// <returns></returns>
        ImmutableHashSet<ModuleReference> FindAll()
        {
            var bModules = _beforeFinder.FindAll();
            var aModules = _afterFinder.FindAll();

            if (aModules.IsEmpty)
                return bModules;

            if (bModules.IsEmpty && _parents.IsEmpty)
                return aModules;

            var result = ImmutableHashSet.CreateBuilder<ModuleReference>();
            foreach (var mref in aModules)
                if (_beforeFinder.Find(mref.Descriptor.Name) is null && FindInParent(mref.Descriptor.Name) is null)
                    result.Add(mref);

            return result.ToImmutable();
        }

        /// <summary>
        /// Returns the package name of a class name.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        string PackageName(string className)
        {
            int index = className.LastIndexOf('.');
            return index == -1 ? "" : className.Substring(0, index);
        }

        /// <summary>
        /// Attempts to add the value to the dictionary or returns <c>false</c> and sets the existing value.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="existing"></param>
        /// <returns></returns>
        bool TryAdd<TKey, TValue>(IDictionary<TKey, TValue> self, TKey key, TValue value, [MaybeNullWhen(false)] out TValue existing)
        {
            if (self.TryGetValue(key, out existing))
                return false;

            self.Add(key, value);
            existing = value;
            return true;
        }

        /// <summary>
        /// Fail because a module in the configuration exports the same package to a module that reads both. This
        /// includes the case where a module M containing a package p reads another module that exports p to at least
        /// module M.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="source"></param>
        /// <param name="supplier1"></param>
        /// <param name="supplier2"></param>
        /// <returns></returns>
        Exception FailTwoSuppliers(ModuleDescriptor descriptor, string source, ModuleDescriptor supplier1, ModuleDescriptor supplier2)
        {
            if (supplier2 == descriptor)
            {
                var tmp = supplier1;
                supplier1 = supplier2;
                supplier2 = tmp;
            }

            if (supplier1 == descriptor)
            {
                return new ResolutionException($"Module {descriptor.Name} contains package {source}, module {supplier2.Name} exports package {source} to {descriptor.Name}.");
            }
            else
            {
                return new ResolutionException($"Modules {supplier1.Name} and {supplier2.Name} export package {source} to module {descriptor.Name}.");
            }
        }

    }

}
