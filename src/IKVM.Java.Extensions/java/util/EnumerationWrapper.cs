using System;
using System.Collections;
using System.Collections.Generic;

namespace java.util
{

    class EnumerationWrapper<T> : IEnumerator<T>
    {

        readonly Enumeration _enumeration;
        T? _current;
        int _position = -1;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="enumeration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public EnumerationWrapper(Enumeration enumeration)
        {
            _enumeration = enumeration ?? throw new ArgumentNullException(nameof(enumeration));
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
        /// <exception cref="NotImplementedException"></exception>
        public bool MoveNext()
        {
            if (_enumeration.hasMoreElements())
            {
                _current = (T)_enumeration.nextElement();
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
