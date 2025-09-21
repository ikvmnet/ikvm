using System;

namespace java.util.function
{

    /// <summary>
    /// Implementation of <see cref="IntFunction"/> that forwards to a <see cref="Func{Int32, TResult}"/>.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class DelegateIntFunction<TResult> : IntFunction
    {

        readonly Func<int, TResult> func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateIntFunction(Func<int, TResult> func)
        {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public object apply(int t)
        {
            return func(t);
        }

    }

}
