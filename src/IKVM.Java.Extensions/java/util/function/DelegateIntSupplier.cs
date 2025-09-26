using System;

namespace java.util.function
{

    /// <summary>
    /// Implementation of <see cref="IntSupplier"/> that forwards to a <see cref="Func{int}"/>.
    /// </summary>
    /// <typeparam name="TArg"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class DelegateIntSupplier : IntSupplier
    {

        readonly Func<int> _func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateIntSupplier(Func<int> func)
        {
            _func = func ?? throw new ArgumentNullException(nameof(func));
        }

        /// <inheritdoc />
        public int getAsInt()
        {
            return _func();
        }

    }

}
