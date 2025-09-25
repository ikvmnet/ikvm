using System;
using System.Collections;
using System.Collections.Generic;

namespace java.util
{

    class IteratorWrapper<T> : IEnumerator<T>
    {

        readonly Iterator _iterator;
        T? _current;
        int _position = -1;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="iterator"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IteratorWrapper(Iterator iterator)
        {
            _iterator = iterator ?? throw new ArgumentNullException(nameof(iterator));
        }

        /// <summary>
        /// Gets the current element.
        /// </summary>
        public T Current => _position > -1 ? _current! : throw new InvalidOperationException();

        /// <summary>
        /// Gets the current element.
        /// </summary>
        object? IEnumerator.Current => _current;

        /// <summary>
        /// Moves to the next instance.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (_iterator.hasNext())
            {
                _current = (T)_iterator.next();
                _position++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public void Reset()
        {
            throw new NotSupportedException();
        }

    }

}
