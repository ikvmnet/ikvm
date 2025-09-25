﻿using System;

namespace java.util.function
{


    /// <summary>
    /// Implementation of <see cref="Consumer"/> that sends output to a delegate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateConsumer<T> : Consumer
    {

        readonly Action<T> _action;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="action"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateConsumer(Action<T> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void accept(object t)
        {
            _action((T)t);
        }

        public Consumer andThen(Consumer other)
        {
            return Consumer.__DefaultMethods.andThen(this, other);
        }

    }

}
