using System.Collections.Generic;

namespace java.util
{

    public static class MapExtensions
    {

        /// <summary>
        /// Returns a <see cref="IDictionary{TKey, TValue}"/> implementation for the given <see cref="Map"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="map"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> AsDictionary<TKey, TValue>(this Map map) => map switch
        {
            IDictionary<TKey, TValue> i => i,
            Map i => new MapWrapper<TKey, TValue>(i),
        };

    }

}
