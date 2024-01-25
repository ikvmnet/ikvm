using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using IKVM.Tests.Util;

namespace IKVM.Reflection.Tests
{

    /// <summary>
    /// Basic assembly resolver.
    /// </summary>
    class TestAssemblyResolver
    {

        readonly Universe universe;
        readonly string tfm;
        readonly string targetFrameworkIdentifier;
        readonly string targetFrameworkVersion;
        readonly IEnumerable<string> dirs;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="universe"></param>
        /// <param name="tfm"></param>
        /// <param name="targetFrameworkIdentifier"></param>
        /// <param name="targetFrameworkVersion"></param>
        /// <param name="dirs"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TestAssemblyResolver(Universe universe, string tfm, string targetFrameworkIdentifier, string targetFrameworkVersion, IEnumerable<string> dirs = null)
        {
            this.universe = universe ?? throw new ArgumentNullException(nameof(universe));
            this.tfm = tfm ?? throw new ArgumentNullException(nameof(tfm));
            this.targetFrameworkIdentifier = targetFrameworkIdentifier ?? throw new ArgumentNullException(nameof(targetFrameworkIdentifier));
            this.targetFrameworkVersion = targetFrameworkVersion ?? throw new ArgumentNullException(nameof(targetFrameworkVersion));
            this.dirs = dirs ?? Array.Empty<string>();

            universe.AssemblyResolve += (s, a) => UniverseAssemblyResolve(a.Name);
        }

        /// <summary>
        /// Attempts to resolve the named assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Resolve(string name)
        {
            // check configured directories
            foreach (var dir in dirs)
            {
                var p = Path.Combine(dir, name + ".dll");
                if (File.Exists(p))
                    return p;
            }

            // check reference assemblies
            foreach (var d in DotNetSdkUtil.GetPathToReferenceAssemblies(tfm, targetFrameworkIdentifier, targetFrameworkVersion))
            {
                var p = Path.GetExtension(name) == ".dll" ? Path.Combine(d, name) : Path.Combine(d, name + ".dll");
                if (File.Exists(p))
                    return p;
            }

            return null;
        }

        /// <summary>
        /// Resolves and loads an assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Assembly UniverseAssemblyResolve(string name)
        {
            return Resolve(name) is string s ? universe.LoadFile(s) : null;
        }

    }

}
