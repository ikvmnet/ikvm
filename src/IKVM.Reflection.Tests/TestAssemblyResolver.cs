using System;
using System.Collections.Generic;
using System.IO;

using IKVM.Tests.Util;

namespace IKVM.Reflection.Tests
{

    /// <summary>
    /// Resolves assemblies from a set of search directories and the reference assemblies of a target framework.
    /// </summary>
    sealed class TestAssemblyResolver
    {

        readonly Universe universe;
        readonly string tfm;
        readonly string targetFrameworkIdentifier;
        readonly string targetFrameworkVersion;
        readonly IEnumerable<string> dirs;

        /// <summary>
        /// Initializes a new instance, and hooks the universe so it resolves through this instance.
        /// </summary>
        /// <param name="universe"></param>
        /// <param name="tfm"></param>
        /// <param name="targetFrameworkIdentifier"></param>
        /// <param name="targetFrameworkVersion"></param>
        /// <param name="dirs"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TestAssemblyResolver(Universe universe, string tfm, string targetFrameworkIdentifier, string targetFrameworkVersion, IEnumerable<string>? dirs = null)
        {
            this.universe = universe ?? throw new ArgumentNullException(nameof(universe));
            this.tfm = tfm ?? throw new ArgumentNullException(nameof(tfm));
            this.targetFrameworkIdentifier = targetFrameworkIdentifier ?? throw new ArgumentNullException(nameof(targetFrameworkIdentifier));
            this.targetFrameworkVersion = targetFrameworkVersion ?? throw new ArgumentNullException(nameof(targetFrameworkVersion));
            this.dirs = dirs ?? [];

            universe.AssemblyResolve += (s, a) => Load(a.Name);
        }

        /// <summary>
        /// Attempts to locate the file of the named assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string? Resolve(string name)
        {
            var fileName = Path.GetExtension(name) == ".dll" ? name : name + ".dll";

            foreach (var dir in dirs)
                if (Path.Combine(dir, fileName) is var p && File.Exists(p))
                    return p;

            foreach (var dir in DotNetSdkUtil.GetPathToReferenceAssemblies(tfm, targetFrameworkIdentifier, targetFrameworkVersion))
                if (Path.Combine(dir, fileName) is var p && File.Exists(p))
                    return p;

            return null;
        }

        /// <summary>
        /// Resolves and loads the named assembly into the universe.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Assembly? Load(string name)
        {
            return Resolve(name) is string s ? universe.LoadFile(s) : null;
        }

    }

}
