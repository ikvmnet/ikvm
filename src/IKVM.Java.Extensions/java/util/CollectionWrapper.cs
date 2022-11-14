using System;
using System.Collections.Generic;

using java.lang;

namespace java.util
{

    class CollectionWrapper<T> : IterableWrapper<T>, ICollection<T>
    {

        readonly Collection collection;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="collection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CollectionWrapper(Collection collection) : base(collection)
        {
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        public int Count => collection.size();

        public bool IsReadOnly => false;

        public void Add(T item) => collection.add(item);

        public void Clear() => collection.clear();

        public bool Contains(T item) => collection.contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var entry in this)
                array[arrayIndex++] = entry;
        }

        public bool Remove(T item)
        {
            return collection.remove(item);
        }

    }

}