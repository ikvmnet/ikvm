using System;

namespace IKVM.Runtime.Util.Java.Security
{

#if FIRST_PASS == false

    /// <summary>
    /// Implementation of <see cref="global::java.security.PrivilegedAction"/> that invokes a delegate.
    /// </summary>
    class ActionPrivilegedAction : global::java.security.PrivilegedAction
    {

        public static implicit operator ActionPrivilegedAction(Action action) => new ActionPrivilegedAction(action);

        readonly Action action;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="action"></param>
        public ActionPrivilegedAction(Action action)
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
