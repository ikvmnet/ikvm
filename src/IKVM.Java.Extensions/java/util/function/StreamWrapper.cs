﻿using System;
using System.Collections;
using System.Collections.Generic;

using java.util.stream;

namespace java.util.function
{

    /// <summary>
    /// Wraps a <see cref="Stream"/> to produce an <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    struct StreamWrapper<T> : IEnumerable<T>
    {

        readonly Stream _stream;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="stream"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public StreamWrapper(Stream stream)
        {
            this._stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        /// <summary>
        /// Gets an enumerator for the stream.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _stream.iterator().AsEnumerator<T>();
        }

        /// <summary>
        /// Gets an enumerator for the stream.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

}
