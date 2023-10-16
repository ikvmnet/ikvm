using System;

namespace IKVM.Runtime.Util.Java.Security
{

#if FIRST_PASS == false

    /// <summary>
    /// Implementation of <see cref="global::java.security.PrivilegedAction"/> that invokes a delegate.
    /// </summary>
    class FuncPrivilegedAction<TResult> : global::java.security.PrivilegedAction
    {

        readonly Func<TResult> func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="action"></param>
        public FuncPrivilegedAction(Func<TResult> func)
        {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public object run()
        {
            return func();
        }

    }

#endif

}
