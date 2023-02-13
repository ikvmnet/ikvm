using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace IKVM.Runtime.Accessors
{

    /// <summary>
    /// Provides runtime access to type accessors.
    /// </summary>
    class Accessors
    {

        /// <summary>
        /// Gets the set of accessors for a given assembly.
        /// </summary>
        /// <param name="accessors"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Accessors Get(ref Accessors accessors, Assembly assembly)
        {
            return AccessorUtil.LazyGet(ref accessors, () => new Accessors(assembly));
        }

        readonly Assembly assembly;
        readonly ConcurrentDictionary<Type, Accessor> accessors = new ConcurrentDictionary<Type, Accessor>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Accessors(Assembly assembly)
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
            return AccessorUtil.LazyGet(ref accessor, () => (TAccessor)accessors.GetOrAdd(typeof(TAccessor), Make));
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
