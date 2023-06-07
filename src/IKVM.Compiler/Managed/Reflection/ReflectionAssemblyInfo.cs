using System;
using System.Collections.Generic;
using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reflection
{

    internal class ReflectionAssemblyInfo : ReflectionEntityInfo, IManagedAssemblyInfo
    {

        readonly Assembly assembly;
        readonly ReadOnlyListMap<ReflectionModuleInfo, Module> modules;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal ReflectionAssemblyInfo(ReflectionContext context, Assembly assembly) :
            base(context)
        {
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));

            modules = new ReadOnlyListMap<ReflectionModuleInfo, Module>(assembly.GetModules(), (m, i) => new ReflectionModuleInfo(this, m));
        }

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        public AssemblyName Name => assembly.GetName();

        /// <summary>
        /// Gets the modules of the assembly.
        /// </summary>
        public IReadOnlyList<IManagedModuleInfo> Modules => modules;

    }

}
