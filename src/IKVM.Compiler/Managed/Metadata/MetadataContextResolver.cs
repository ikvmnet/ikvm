using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Defines a set of operations to resolve <see cref="MetadataContext"/> instances by assembly name.
    /// </summary>
    internal class MetadataContextResolver
    {

        readonly Func<AssemblyName, MetadataReader> resolver;
        readonly ConcurrentDictionary<AssemblyName, MetadataContext> cache = new ConcurrentDictionary<AssemblyName, MetadataContext>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public MetadataContextResolver(Func<AssemblyName, MetadataReader> resolver)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        /// <summary>
        /// Resolves a new metadata context by assembly name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MetadataContext Resolve(AssemblyName name) => cache.GetOrAdd(name, _ => new MetadataContext(this, resolver(_)));

    }

}
