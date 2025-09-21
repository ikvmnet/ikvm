using System;

namespace java.util.function
{

    /// <summary>
    /// Implementation of <see cref="DoubleFunction"/> that forwards to a <see cref="Func{Double, TResult}"/>.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class DelegateDoubleFunction<TResult> : DoubleFunction
    {

        readonly Func<double, TResult> func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateDoubleFunction(Func<double, TResult> func)
        {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public object apply(double t)
        {
            return func(t);
        }

    }

}
