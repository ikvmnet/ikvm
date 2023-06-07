using System;
using System.Reflection;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Descrines a module derived from .NET reflection information.
    /// </summary>
    internal sealed class ReflectionModuleInfo : ReflectionEntityInfo, IManagedModuleInfo
    {

        readonly ReflectionAssemblyInfo assembly;
        readonly Module module;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="module"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal ReflectionModuleInfo(ReflectionAssemblyInfo assembly, Module module) :
            base(assembly.Context)
        {
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            this.module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public string Name => module.Name;

    }

}
