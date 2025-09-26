using System;

namespace java.util.function
{

    /// <summary>
    /// Implementation of <see cref="DoubleSupplier"/> that forwards to a <see cref="Func{double}"/>.
    /// </summary>
    public class DelegateDoubleSupplier : DoubleSupplier
    {

        readonly Func<double> _func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateDoubleSupplier(Func<double> func)
        {
            _func = func ?? throw new ArgumentNullException(nameof(func));
        }

        /// <inheritdoc />
        public double getAsDouble()
        {
            return _func();
        }

    }

}
