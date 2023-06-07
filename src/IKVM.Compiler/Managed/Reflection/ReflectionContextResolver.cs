using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Defines a set of operations to resolve <see cref="ReflectionContext"/> instances by assembly name.
    /// </summary>
    internal class ReflectionContextResolver
    {

        readonly Func<AssemblyName, ReflectionContext> resolve;
        readonly ConcurrentDictionary<AssemblyName, ReflectionContext> cache = new ConcurrentDictionary<AssemblyName, ReflectionContext>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolve"></param>
        public ReflectionContextResolver(Func<AssemblyName, ReflectionContext> resolve)
        {
            this.resolve = resolve ?? throw new ArgumentNullException(nameof(resolve));
        }

        /// <summary>
        /// Resolves a new metadata context by assembly name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ReflectionContext Resolve(AssemblyName name) => cache.GetOrAdd(name, resolve);

    }

}
