using System;

namespace java.util.function
{

    /// <summary>
    /// Implementation of <see cref="Supplier"/> that forwards to a <see cref="Func{TResult}"/>.
    /// </summary>
    /// <typeparam name="TArg"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class DelegateSupplier<T> : Supplier
    {

        readonly Func<T> _func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateSupplier(Func<T> func)
        {
            _func = func ?? throw new ArgumentNullException(nameof(func));
        }

        /// <inheritdoc />
        public object? get()
        {
            return _func();
        }

    }

}
