using System.Collections.Generic;

namespace java.util
{

    public static class SetExtensions
    {

        /// <summary>
        /// Returns a <see cref="ISet{T}"/> that wraps the specified <see cref="Set"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set"></param>
        /// <returns></returns>
        public static ISet<T> AsSet<T>(this Set set) => set switch
        {
            ISet<T> i => i,
            TreeSet i => new TreeSetWrapper<T>(i),
            Set i => new SetWrapper<T>(i),
        };

    }

}
