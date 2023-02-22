using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

    /// <summary>
    /// Provides runtime access to the 'java.lang.ThreadGroup' type.
    /// </summary>
    internal sealed class ThreadGroupAccessor : Accessor
    {

        MethodAccessor<Action<object, object, object>> uncaughtException;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public ThreadGroupAccessor(AccessorTypeResolver resolver) :
            base(resolver("java.lang.ThreadGroup"))
        {

        }

        /// <summary>
        /// Invokes the 'die' method.
        /// </summary>
        public void InvokeUncaughtException(object self, object t, object e) => GetVoidMethod(ref uncaughtException, nameof(uncaughtException)).Invoker(self, t, e);

    }

}
