using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using java.lang;

namespace java.util
{

    class MapWrapper<TKey, TValue> : IDictionary<TKey, TValue>
    {

        readonly Map _map;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="map"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MapWrapper(Map map)
        {
            this._map = map ?? throw new ArgumentNullException(nameof(map));
        }

        /// <inheritdoc />
        public TValue this[TKey key]
        {
            get => (TValue)_map.get(key);
            set => _map.put(key, value);
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys => _map.keySet().AsCollection<TKey>();

        /// <inheritdoc />
        public ICollection<TValue> Values => _map.values().AsCollection<TValue>();

        /// <inheritdoc />
        public int Count => _map.size();

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public void Add(TKey key, TValue value) => _map.put(key, value);

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, TValue> item) => _map.put(item.Key, item.Value);

        /// <inheritdoc />
        public void Clear() => _map.clear();

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _map.containsKey(item.Key) && _map.get(item.Key).Equals(item.Value);
        }

        /// <inheritdoc />
        public bool ContainsKey(TKey key)
        {
            return _map.containsKey(key);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (var entry in this)
                array[arrayIndex++] = entry;
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _map.entrySet().AsEnumerable<Map.Entry>().Select(i => new KeyValuePair<TKey, TValue>((TKey)i.getKey(), (TValue)i.getValue())).GetEnumerator();
        }

        /// <inheritdoc />
        public bool Remove(TKey key)
        {
            if (_map.containsKey(key))
            {
                _map.remove(key);
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _map.remove(item.Key, item.Value);
        }

        /// <inheritdoc />
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_map.containsKey(key))
            {
                value = (TValue) _map.get(key);
                return true;
            }

            value = default!;
            return false;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

}