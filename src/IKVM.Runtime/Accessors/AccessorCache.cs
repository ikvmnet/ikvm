using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace IKVM.Runtime.Accessors
{

    /// <summary>
    /// Provides runtime access to type accessors.
    /// </summary>
    class AccessorCache
    {

        /// <summary>
        /// Gets the set of accessors for a given assembly.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static AccessorCache Get(ref AccessorCache cache, Assembly assembly)
        {
            return AccessorUtil.LazyGet(ref cache, () => new AccessorCache(assembly));
        }

        readonly Assembly assembly;
        readonly ConcurrentDictionary<Type, Accessor> cache = new ConcurrentDictionary<Type, Accessor>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AccessorCache(Assembly assembly)
        {
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        /// <summary>
        /// Gets the instance of the specified accessor if not already resolved.
        /// </summary>
        /// <typeparam name="TAccessor"></typeparam>
        /// <param name="accessor"></param>
        /// <returns></returns>
        public TAccessor Get<TAccessor>(ref TAccessor accessor)
            where TAccessor : Accessor
        {
            return AccessorUtil.LazyGet(ref accessor, () => (TAccessor)cache.GetOrAdd(typeof(TAccessor), Make));
        }

        /// <summary>
        /// Creates a new accessor instance.
        /// </summary>
        /// <param name="accessorType"></param>
        /// <returns></returns>
        Accessor Make(Type accessorType)
        {
            return (Accessor)Activator.CreateInstance(accessorType, (AccessorTypeResolver)(t => assembly.GetType(t)));
        }

    }

}
