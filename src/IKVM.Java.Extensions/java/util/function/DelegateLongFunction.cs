using System;

namespace java.util.function
{

    /// <summary>
    /// Implementation of <see cref="LongFunction"/> that forwards to a <see cref="Func{Int64, TResult}"/>.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class DelegateLongFunction<TResult> : LongFunction
    {

        readonly Func<long, TResult> func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateLongFunction(Func<long, TResult> func)
        {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public object apply(long t)
        {
            return func(t);
        }

    }

}
