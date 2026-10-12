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

        readonly Universe _universe;
        readonly string _tfm;
        readonly string _targetFrameworkIdentifier;
        readonly string _targetFrameworkVersion;
        readonly IEnumerable<string> _dirs;

        /// <summary>
        /// Initializes a new instance, and hooks the universe so it resolves through this instance.
        /// </summary>
        /// <param name="universe">The universe to load resolved assemblies into.</param>
        /// <param name="tfm">The target framework moniker whose reference assemblies are searched.</param>
        /// <param name="targetFrameworkIdentifier">The target framework identifier, such as <c>.NETFramework</c>.</param>
        /// <param name="targetFrameworkVersion">The target framework version, such as <c>4.7.2</c>.</param>
        /// <param name="dirs">Directories searched before the reference assemblies, or <see langword="null"/> for none.</param>
        /// <exception cref="ArgumentNullException"><paramref name="universe"/>, <paramref name="tfm"/>, <paramref name="targetFrameworkIdentifier"/> or <paramref name="targetFrameworkVersion"/> is <see langword="null"/>.</exception>
        public TestAssemblyResolver(Universe universe, string tfm, string targetFrameworkIdentifier, string targetFrameworkVersion, IEnumerable<string>? dirs = null)
        {
            _universe = universe ?? throw new ArgumentNullException(nameof(universe));
            _tfm = tfm ?? throw new ArgumentNullException(nameof(tfm));
            _targetFrameworkIdentifier = targetFrameworkIdentifier ?? throw new ArgumentNullException(nameof(targetFrameworkIdentifier));
            _targetFrameworkVersion = targetFrameworkVersion ?? throw new ArgumentNullException(nameof(targetFrameworkVersion));
            _dirs = dirs ?? [];

            universe.AssemblyResolve += (s, a) => Load(new System.Reflection.AssemblyName(a.Name).Name!);
        }

        /// <summary>
        /// Attempts to locate the file of the named assembly, searching the extra directories first and then the reference
        /// assemblies of the target framework.
        /// </summary>
        /// <param name="name">The simple name of the assembly, with or without a <c>.dll</c> extension.</param>
        /// <returns>The path of the assembly file, or <see langword="null"/> if it is not found.</returns>
        public string? Resolve(string name)
        {
            var fileName = Path.GetExtension(name) == ".dll" ? name : name + ".dll";

            foreach (var dir in _dirs)
                if (Path.Combine(dir, fileName) is var p && File.Exists(p))
                    return p;

            foreach (var dir in DotNetSdkUtil.GetPathToReferenceAssemblies(_tfm, _targetFrameworkIdentifier, _targetFrameworkVersion))
                if (Path.Combine(dir, fileName) is var p && File.Exists(p))
                    return p;

            return null;
        }

        /// <summary>
        /// Resolves and loads the named assembly into the universe.
        /// </summary>
        /// <param name="name">The simple name of the assembly, with or without a <c>.dll</c> extension.</param>
        /// <returns>The loaded assembly, or <see langword="null"/> if it is not found.</returns>
        public Assembly? Load(string name)
        {
            return Resolve(name) is string s ? _universe.LoadFile(s) : null;
        }

    }

}
