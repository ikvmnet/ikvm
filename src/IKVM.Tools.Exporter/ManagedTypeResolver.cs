using System;

using IKVM.Reflection;
using IKVM.Runtime;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Exporter
{

    class ManagedTypeResolver : IManagedTypeResolver
    {

        readonly StaticCompiler compiler;
        readonly Assembly baseAssembly;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="compiler"></param>
        /// <param name="baseAssembly"></param>
        public ManagedTypeResolver(StaticCompiler compiler, Assembly baseAssembly)
        {
            this.compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
            this.baseAssembly = baseAssembly;
        }

        /// <summary>
        /// Attempts to resolve the base Java assembly.
        /// </summary>
        /// <returns></returns>
        public Assembly ResolveBaseAssembly()
        {
            return baseAssembly;
        }

        /// <summary>
        /// Attempts to resolve an assembly from one of the assembly sources.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public Assembly ResolveAssembly(string assemblyName)
        {
            return compiler.Load(assemblyName);
        }

        /// <summary>
        /// Attempts to resolve a type from one of the assembly sources.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Type ResolveCoreType(string typeName)
        {
            foreach (var assembly in compiler.Universe.GetAssemblies())
                if (assembly.GetType(typeName) is Type t)
                    return t;

            return null;
        }

        /// <summary>
        /// Attempts to resolve a type from the IKVM runtime assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Type ResolveRuntimeType(string typeName)
        {
            return compiler.GetRuntimeType(typeName);
        }

    }

}
