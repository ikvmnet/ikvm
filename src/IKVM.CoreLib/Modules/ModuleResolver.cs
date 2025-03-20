using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKVM.CoreLib.Modules
{

    sealed class ModuleResolver
    {

        readonly IModuleFinder _beforeFinder;
        readonly ImmutableArray<ModuleConfiguration> _parents;
        readonly IModuleFinder _afterFinder;

        readonly Dictionary<string, ModuleReference> _nameToReference = new();

        bool _haveAllAutomaticModules;
        string _targetPlatform;

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
                        {
                            throw new ArgumentException($"Parents have conflicting constraints on target platform: {_targetPlatform}, {value}.", nameof(parents));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resolves the given named modules.
        /// </summary>
        /// <param name="roots"></param>
        /// <returns></returns>
        ModuleResolver Resolve(ImmutableArray<string> roots)
        {
            var q = new Queue<ModuleDescriptor>();

            foreach (var root in roots)
            {
                var mref = FindWithBeforeFinder(root);
                if (mref is null)
                {
                    if (FindInParent(root) is not null)
                        continue;

                    mref = FindWithAfterFinder(root);
                    if (mref is null)
                        continue; // findFail: Module %s not found
                }

                AddFoundModule(mref);
                q.Enqueue(mref.Descriptor);
            }

            Resolve(q);

            return this;
        }

        HashSet<ModuleDescriptor> Resolve(Queue<ModuleDescriptor> q)
        {
            var resolved = new HashSet<ModuleDescriptor>();

            while (q.Count != 0)
            {
                var descriptor = q.Dequeue();
                if (_nameToReference.ContainsKey(descriptor.Name) == false)
                    throw new InvalidOperationException();

                // if the module is an automatic module then all automatic modules need to be resolved
                if (descriptor.IsAutomatic && !_haveAllAutomaticModules)
                {
                    addFoundAutomaticModules().forEach(mref-> {
                        ModuleDescriptor other = mref.descriptor();
                        q.offer(other);
                        if (isTracing())
                        {
                            trace("%s requires %s", descriptor.name(), nameAndInfo(mref));
                        }
                    });

                    haveAllAutomaticModules = true;
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
                        {
                            // dependence is in parent
                            continue;
                        }

                        mref = FindWithAfterFinder(dn);
                        if (mref == null)
                        {
                            findFail("Module %s not found, required by %s", dn, descriptor.name());
                        }
                    }

                    if (isTracing() && !dn.equals("java.base"))
                    {
                        trace("%s requires %s", descriptor.name(), nameAndInfo(mref));
                    }

                    if (_nameToReference.ContainsKey(dn) == false)
                    {
                        AddFoundModule(mref);
                        q.offer(mref.descriptor());
                    }

                }

                resolved.add(descriptor);
            }

            return resolved;
        }

    }

}
