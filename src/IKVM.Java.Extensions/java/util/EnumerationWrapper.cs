using System;
using System.Collections;
using System.Collections.Generic;

namespace java.util
{

    class EnumerationWrapper<T> : IEnumerator<T>
    {

        readonly Enumeration enumeration;
        T current;
        int position = -1;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="enumeration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public EnumerationWrapper(Enumeration enumeration)
        {
            this.enumeration = enumeration ?? throw new ArgumentNullException(nameof(enumeration));
        }

        /// <summary>
        /// Gets the current element.
        /// </summary>
        public T Current => position > -1 ? current : throw new InvalidOperationException();

        /// <summary>
        /// Gets the current element.
        /// </summary>
        object IEnumerator.Current => current;

        /// <summary>
        /// Moves to the next instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool MoveNext()
        {
            if (enumeration.hasMoreElements())
            {
                current = (T)enumeration.nextElement();
                position++;
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
