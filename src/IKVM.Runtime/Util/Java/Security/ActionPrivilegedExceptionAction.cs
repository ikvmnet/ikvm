using System;

namespace IKVM.Runtime.Util.Java.Security
{

#if FIRST_PASS == false

    /// <summary>
    /// Implementation of <see cref="global::java.security.PrivilegedExceptionAction"/> that invokes a delegate.
    /// </summary>
    class ActionPrivilegedExceptionAction : global::java.security.PrivilegedExceptionAction
    {

        readonly Action action;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="action"></param>
        public ActionPrivilegedExceptionAction(Action action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public object run()
        {
            action();
            return null;
        }

    }

#endif

}
