using System;
using System.Collections.Generic;

namespace java.util
{

    class ListWrapper<T> : CollectionWrapper<T>, IList<T>
    {

        readonly List _list;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="list"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ListWrapper(List list) :
            base(list)
        {
            _list = list ?? throw new ArgumentNullException(nameof(list));
        }

        /// <inheritdoc />
        public T this[int index] 
        {
            get => (T)_list.get(index);
            set => _list.set(index, value);
        }

        /// <inheritdoc />
        public int IndexOf(T item) => _list.indexOf(item);

        /// <inheritdoc />
        public void Insert(int index, T item) => _list.add(index, item);

        /// <inheritdoc />
        public void RemoveAt(int index) => _list.remove(index);

    }

}