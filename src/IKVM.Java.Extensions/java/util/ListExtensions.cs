using System.Collections.Generic;

namespace java.util
{

    public static class ListExtensions
    {

        /// <summary>
        /// Returns a <see cref="IList{T}"/> that wraps the specified <see cref="List"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<T> AsList<T>(this List list) => list switch
        {
            IList<T> c => c,
            List l => new ListWrapper<T>(l),
        };

    }

}
