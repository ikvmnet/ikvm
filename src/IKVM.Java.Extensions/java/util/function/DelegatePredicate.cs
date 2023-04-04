using System;

namespace java.util.function
{

    /// <summary>
    /// Implements the <see cref="Predicate"/> interface against a delegate.
    /// </summary>
    /// <typeparam name="TArg"></typeparam>
    public class DelegatePredicate<TArg> : Predicate
    {

        readonly Func<TArg, bool> func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="func"></param>
        public DelegatePredicate(Func<TArg, bool> func)
        {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public Predicate and(Predicate other)
        {
            return Predicate.__DefaultMethods.and(this, other);
        }

        public Predicate negate()
        {
            return Predicate.__DefaultMethods.negate(this);
        }

        public Predicate or(Predicate other)
        {
            return Predicate.__DefaultMethods.or(this, other);
        }

        public bool test(object t)
        {
            return func((TArg)t);
        }

    }

}
