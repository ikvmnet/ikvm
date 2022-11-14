using System;
using System.Collections;
using System.Collections.Generic;

using java.lang;

namespace java.util
{

    class IteratorWrapper<T> : IEnumerator<T>
    {

        readonly Iterator iterator;
        T current;
        int position = -1;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="iterator"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IteratorWrapper(Iterator iterator)
        {
            this.iterator = iterator ?? throw new ArgumentNullException(nameof(iterator));
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
            if (iterator.hasNext())
            {
                current = (T)iterator.next();
                position++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            if (iterator is AutoCloseable c)
                c.Dispose();
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
