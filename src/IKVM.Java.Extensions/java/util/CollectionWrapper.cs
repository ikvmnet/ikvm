using System;
using System.Collections.Generic;

using java.lang;

namespace java.util
{

    class CollectionWrapper<T> : IterableWrapper<T>, ICollection<T>
    {

        internal readonly Collection _collection;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="collection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CollectionWrapper(Collection collection) : base(collection)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        /// <inheritdoc />
        public int Count => _collection.size();

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public void Add(T item) => _collection.add(item);

        /// <inheritdoc />
        public void Clear() => _collection.clear();

        /// <inheritdoc />
        public bool Contains(T item) => _collection.contains(item);

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var entry in this)
                array[arrayIndex++] = entry;
        }

        /// <inheritdoc />
        public bool Remove(T item) => _collection.remove(item);

    }

}