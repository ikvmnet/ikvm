using System;
using System.Collections;
using System.Collections.Generic;

namespace IKVM.ByteCode.Collections
{

    /// <summary>
    /// Describes a read-only list that delegates to a function for each index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class DelegateReadOnlyList<T> : IReadOnlyList<T>
    {

        readonly int count;
        readonly Func<int, T> getter;

        /// <summary>
        /// Initialies a new instance.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="getter"></param>
        public DelegateReadOnlyList(int count, Func<int, T> getter)
        {
            this.count = count;
            this.getter = getter;
        }

        public T this[int index] => getter(index);

        public int Count => count;

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
                yield return getter(i);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

}
