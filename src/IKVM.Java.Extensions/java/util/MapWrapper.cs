using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using java.lang;

namespace java.util
{

    class MapWrapper<TKey, TValue> : IDictionary<TKey, TValue>
    {

        readonly Map map;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="map"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MapWrapper(Map map)
        {
            this.map = map ?? throw new ArgumentNullException(nameof(map));
        }

        public TValue this[TKey key]
        {
            get => (TValue)map.get(key);
            set => map.put(key, value);
        }

        public ICollection<TKey> Keys => map.keySet().AsCollection<TKey>();

        public ICollection<TValue> Values => map.values().AsCollection<TValue>();

        public int Count => map.size();

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value) => map.put(key, value);

        public void Add(KeyValuePair<TKey, TValue> item) => map.put(item.Key, item.Value);

        public void Clear() => map.clear();

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return map.containsKey(item.Key) && map.get(item.Key).Equals(item.Value);
        }

        public bool ContainsKey(TKey key)
        {
            return map.containsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (var entry in this)
                array[arrayIndex++] = entry;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return map.entrySet().AsEnumerable<Map.Entry>().Select(i => new KeyValuePair<TKey, TValue>((TKey)i.getKey(), (TValue)i.getValue())).GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            if (map.containsKey(key))
            {
                map.remove(key);
                return true;
            }

            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return map.remove(item.Key, item.Value);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (map.containsKey(key))
            {
                value = (TValue) map.get(key);
                return true;
            }

            value = default;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

}