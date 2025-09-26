using System;

namespace java.util.function
{

    /// <summary>
    /// Implementation of <see cref="LongSupplier"/> that forwards to a <see cref="Func{long}"/>.
    /// </summary>
    public class DelegateLongSupplier : LongSupplier
    {

        readonly Func<long> _func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateLongSupplier(Func<long> func)
        {
            _func = func ?? throw new ArgumentNullException(nameof(func));
        }

        /// <inheritdoc />
        public long getAsLong()
        {
            return _func();
        }

    }

}
