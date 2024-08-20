using System.Collections.Generic;

namespace java.util
{

    public static class CollectionExtensions
    {

        /// <summary>
        /// Returns a <see cref="ICollection{T}"/> that wraps the specified <see cref="Collection"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static ICollection<T> AsCollection<T>(this Collection collection) => collection switch
        {
            ICollection<T> c => c,
            List l => l.AsList<T>(),
            Set s => s.AsSet<T>(),
            Collection c => new CollectionWrapper<T>(c),
        };

    }

}
