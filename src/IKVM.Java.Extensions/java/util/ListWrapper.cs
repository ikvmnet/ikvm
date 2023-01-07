using System;
using System.Collections.Generic;

namespace java.util
{

    class ListWrapper<T> : CollectionWrapper<T>, IList<T>
    {

        readonly global::java.util.List list;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="list"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ListWrapper(global::java.util.List list) :
            base(list)
        {
            this.list = list ?? throw new ArgumentNullException(nameof(list));
        }

        public T this[int index] 
        {
            get => (T)list.get(index);
            set => list.set(index, value); }

        public int IndexOf(T item) => list.indexOf(item);

        public void Insert(int index, T item) => list.add(index, item);

        public void RemoveAt(int index) => list.remove(index);

    }

}