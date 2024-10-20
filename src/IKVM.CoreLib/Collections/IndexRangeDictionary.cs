using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace IKVM.CoreLib.Collections
{

    /// <summary>
    /// Represents a dictionary that can store int keys mapped to values, where the underlying storage is an array that
    /// holds the minimum number of items for the minimum and maximum key values.
    /// </summary>
    struct IndexRangeDictionary<T>
    {

        const int ALIGNMENT = 8;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static internal int AlignTowardsInfinity(int i)
        {
            if (i >= 0)
                return (i + (ALIGNMENT - 1)) & -ALIGNMENT;
            else
                return -((-i + (ALIGNMENT - 1)) & -ALIGNMENT);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static internal int AlignTowardsZero(int i)
        {
            if (i >= 0)
                return i - (i % ALIGNMENT);
            else
                return -(-i - (-i % ALIGNMENT));
        }

        int _maxCapacity;
        internal int _minKey = 0;
        internal int _maxKey = 0;
        internal T?[] _items = [];

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IndexRangeDictionary() : this(maxCapacity: int.MaxValue)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IndexRangeDictionary(int maxCapacity = int.MaxValue)
        {
            if (maxCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(maxCapacity));

            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Gets the capacity of the dictionary.
        /// </summary>
        public readonly int Capacity => _items.Length;

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
            // on first hit, set keys to this key (not 0)
            if (_items.Length == 0)
            {
                _minKey = key;
                _maxKey = key;
            }

            // calculate new min and max aligned
            var newMin = Math.Min(_minKey, Math.Min(AlignTowardsZero(key), AlignTowardsInfinity(key)));
            var newMax = Math.Max(_maxKey, Math.Max(AlignTowardsZero(key), AlignTowardsInfinity(key)));

            // calculate desired length
            var len = Math.Max(_items.Length, 8);
            while (len < newMax - newMin + 1)
                len *= 2;

            // calculate amount to shift
            int sft = 0;
            if (newMin < _minKey)
                sft = _minKey - newMin;

            // if we calculated any resize or shift operation, apply
            if (_items.Length != len || sft > 0)
            {
                // we will be copying data either to either existing array or new array
                var src = _items;
                if (_items.Length != len)
                    _items = new T[len];

                // copy source data to destination at shift
                // clear newly exposed positions
                if (src.Length > 0)
                {
                    Array.Copy(src, 0, _items, sft, _maxKey - _minKey + 1);
                    Array.Clear(_items, 0, sft);
                }
            }

            // reset our min and max range
            _minKey = newMin;
            _maxKey = newMax;

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
            if (pos < 0 || pos >= _items.Length)
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
