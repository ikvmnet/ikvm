using System;

namespace java.util.function
{

    public class DelegateFunction<TArg, TResult> : Function
    {

        readonly Func<TArg, TResult> func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateFunction(Func<TArg, TResult> func)
        {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public Function andThen(Function after)
        {
            return Function.__DefaultMethods.andThen(this, after);
        }

        public Function compose(Function before)
        {
            return Function.__DefaultMethods.compose(this, before);
        }

        public object apply(object t)
        {
            return func((TArg)t);
        }

    }

}
