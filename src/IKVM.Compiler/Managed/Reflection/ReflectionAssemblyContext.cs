using System;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Maintains a context for a specific assembly file.
    /// </summary>
    internal class ReflectionContext
    {

        readonly ReflectionContextResolver resolver;
        readonly ReflectionAssembly assembly;
        readonly ConditionalWeakTable<Type, ReflectionType> typeCache = new ConditionalWeakTable<Type, ReflectionType>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="assembly"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionContext(ReflectionContextResolver resolver, Assembly assembly)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.assembly = new ReflectionAssembly(this, assembly);
        }

        /// <summary>
        /// Gets the assembly that defines this context.
        /// </summary>
        public ReflectionAssembly Assembly => assembly;

        /// <summary>
        /// Resolves a context based on the specified assembly name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ReflectionContext Resolve(AssemblyName name) => assembly.Name == name ? this : resolver.Resolve(name);

    }

}
