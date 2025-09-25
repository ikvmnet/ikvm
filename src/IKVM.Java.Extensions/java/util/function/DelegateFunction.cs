using System;

namespace java.util.function
{

    /// <summary>
    /// Implementation of <see cref="Function"/> that forwards to a <see cref="Func{T, TResult}"/>.
    /// </summary>
    /// <typeparam name="TArg"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class DelegateFunction<TArg, TResult> : Function
    {

        readonly Func<TArg, TResult> _func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateFunction(Func<TArg, TResult> func)
        {
            this._func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public Function andThen(Function after)
        {
            return Function.__DefaultMethods.andThen(this, after);
        }

        public Function compose(Function before)
        {
            return Function.__DefaultMethods.compose(this, before);
        }

        public object? apply(object t)
        {
            return _func((TArg)t);
        }

    }

    /// <summary>
    /// Implementation of <see cref="Function"/> that forwards to a <see cref="Func{T, TResult}"/>.
    /// </summary>
    /// <typeparam name="TArg"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class DelegateFunction<TArg1, TArg2, TResult> : BiFunction
    {

        readonly Func<TArg1, TArg2, TResult> _func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateFunction(Func<TArg1, TArg2, TResult> func)
        {
            this._func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public BiFunction andThen(Function after)
        {
            return BiFunction.__DefaultMethods.andThen(this, after);
        }

        public object? apply(object t1, object t2)
        {
            return _func((TArg1)t1, (TArg2)t2);
        }

    }

}
