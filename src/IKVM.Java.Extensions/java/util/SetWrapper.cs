using System;
using System.Collections.Generic;
using System.Linq;

namespace java.util
{

    class SetWrapper<T> : CollectionWrapper<T>, ISet<T>
    {

        readonly Set _set;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="set"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SetWrapper(Set set) :
            base(set)
        {
            _set = set ?? throw new ArgumentNullException(nameof(set));
        }

        /// <inheritdoc />
        public void ExceptWith(IEnumerable<T> other)
        {
            if (other is T[] array)
                _set.removeAll(Arrays.asList(array));
            else if (other is Collection c)
                _set.removeAll(c);
            else if (other is CollectionWrapper<T> wrapper)
                _set.removeAll(wrapper._collection);
            else
                _set.removeAll(Arrays.asList(other.ToArray()));
        }

        /// <inheritdoc />
        public void IntersectWith(IEnumerable<T> other)
        {
            if (other is T[] array)
                _set.retainAll(Arrays.asList(array));
            else if (other is Collection c)
                _set.retainAll(c);
            else if (other is CollectionWrapper<T> wrapper)
                _set.retainAll(wrapper._collection);
            else
                _set.retainAll(Arrays.asList(other.ToArray()));
        }

        /// <inheritdoc />
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool Overlaps(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool SetEquals(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void UnionWith(IEnumerable<T> other)
        {
            if (other is T[] array)
                _set.addAll(Arrays.asList(array));
            else if (other is Collection c)
                _set.addAll(c);
            else if (other is CollectionWrapper<T> wrapper)
                _set.addAll(wrapper._collection);
            else
                _set.addAll(Arrays.asList(other.ToArray()));
        }

        /// <inheritdoc />
        bool ISet<T>.Add(T item)
        {
            return _set.add(item);
        }

    }

}