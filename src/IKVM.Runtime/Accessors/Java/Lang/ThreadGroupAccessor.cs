using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.lang.ThreadGroup' type.
    /// </summary>
    internal sealed class ThreadGroupAccessor : Accessor<object>
    {

        MethodAccessor<Func<object, object, string, object>> init1;
        MethodAccessor<Func<object, string, object>> init2;
        MethodAccessor<Func<object>> init3;
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
        /// Invokes the constructor.
        /// </summary>
        /// <returns></returns>
        public object Init(object unused, object parent, string name) => GetConstructor(ref init1, "(Ljava.lang.Void;Ljava.lang.ThreadGroup;Ljava.lang.String;)V").Invoker(unused, parent, name);

        /// <summary>
        /// Invokes the constructor.
        /// </summary>
        /// <returns></returns>
        public object Init(object parent, string name) => GetConstructor(ref init2, "(Ljava.lang.ThreadGroup;Ljava.lang.String;)V").Invoker(parent, name);

        /// <summary>
        /// Invokes the constructor.
        /// </summary>
        /// <returns></returns>
        public object Init() => GetConstructor(ref init3, "()V").Invoker();

        /// <summary>
        /// Invokes the 'uncaughtException' method.
        /// </summary>
        public void InvokeUncaughtException(object self, object t, object e) => GetMethod(ref uncaughtException, nameof(uncaughtException), "(Ljava.lang.Thread;Ljava.lang.Throwable;)V").Invoker(self, t, e);

    }

#endif

}
