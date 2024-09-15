using System;
using System.Diagnostics;

namespace IKVM.CoreLib.Collections
{

    /// <summary>
    /// Represents a dictionary that can store int keys mapped to values, where the underlying storage is an array that
    /// holds the minimum number of items for the minimum and maximum key values.
    /// </summary>
    struct IndexRangeDictionary<T>
    {

        int _initialCapacity;
        int _maxCapacity;
        int _minKey;
        T?[]? _items;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IndexRangeDictionary() : this(initialCapacity: 4, maxCapacity: int.MaxValue)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IndexRangeDictionary(int initialCapacity = 4, int maxCapacity = int.MaxValue)
        {
            if (initialCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(initialCapacity));
            if (maxCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(maxCapacity));

            _initialCapacity = initialCapacity;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Gets the capacity of the dictionary.
        /// </summary>
        public readonly int Capacity => _items?.Length ?? 0;

        /// <summary>
        /// Gets or sets the item with the specified key, optionally growing the list to accomidate.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T? this[int key]
        {
            readonly get => Get(key);
            set => Set(key, value);
        }

        /// <summary>
        /// Ensures the list is sized such that it can hold the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void EnsureCapacity(int key)
        {
            // initial state, first item, hold only that
            if (_items == null)
            {
                _minKey = key;
                _items = new T[_initialCapacity];
            }

            // key is less than minKey, grow and shift by difference
            if (key < _minKey)
            {
                // increase length until we encompass key
                var len = _items.Length;
                while (key - _minKey +  len - _items.Length < 0)
                    len *= 2;

                if (len > _maxCapacity || len < 0)
                    throw new InvalidOperationException();

                var end = _items.Length - 1;
                Array.Resize(ref _items, len);
                for (int i = end; i >= 0; i--)
                    _items[i + _minKey - key] = _items[i];
                Array.Clear(_items, 0, end);

                _minKey = key;
            }

            // desired position if after the end of the array, we need to grow, but no copies needed
            if (key - _minKey >= _items.Length)
            {
                // increase length until we encompass key
                var len = _items.Length;
                while (key - _minKey >= len)
                    len *= 2;

                if (len > _maxCapacity || len < 0)
                    throw new InvalidOperationException();

                Array.Resize(ref _items, len);
            }

            Debug.Assert(key - _minKey >= 0);
            Debug.Assert(key - _minKey < _items.Length);
        }

        /// <summary>
        /// Adds a new item to the list.
        /// </summary>
        /// <param name="key"></param>
        readonly T? Get(int key)
        {
            var pos = key - _minKey;
            if (_items == null || pos < 0 || pos >= _items.Length)
                return default;
            else
                return _items[pos];
        }

        /// <summary>
        /// Adds a new item to the list.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(int key, T? value)
        {
            EnsureCapacity(key);
            if (_items == null)
                throw new InvalidOperationException();

            Debug.Assert(key - _minKey >= 0);
            Debug.Assert(key - _minKey < _items.Length);
            _items[key - _minKey] = value;
        }

    }

}
