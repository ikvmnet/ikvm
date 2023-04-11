using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.lang.Thread' type.
    /// </summary>
    internal sealed class ThreadAccessor : Accessor<object>
    {

        FieldAccessor<object> current;
        MethodAccessor<Func<object>> currentThread;

        MethodAccessor<Func<object, object>> init;
        MethodAccessor<Func<object, bool>> isDaemon;
        MethodAccessor<Action<object>> die;
        MethodAccessor<Func<object, object>> getThreadGroup;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public ThreadAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.lang.Thread")
        {

        }

        /// <summary>
        /// Gets the value of the 'current' field.
        /// </summary>
        /// <returns></returns>
        public object GetCurrent() => GetField(ref current, nameof(current)).GetValue();

        /// <summary>
        /// Invokes the 'currentThread' method.
        /// </summary>
        public object InvokeCurrentThread() => GetMethod(ref currentThread, nameof(currentThread), Resolve("java.lang.Thread")).Invoker();

        /// <summary>
        /// Invokes the constructor.
        /// </summary>
        /// <param name="threadGroup"></param>
        /// <returns></returns>
        public object Init(object threadGroup) => GetConstructor(ref init, Resolve("java.lang.ThreadGroup")).Invoker(threadGroup);

        /// <summary>
        /// Invokes the 'isDaemon' method.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public bool InvokeIsDaemon(object self) => GetMethod(ref isDaemon, nameof(isDaemon), typeof(bool)).Invoker(self);

        /// <summary>
        /// Invokes the 'die' method.
        /// </summary>
        public void InvokeDie(object self) => GetMethod(ref die, nameof(die), typeof(void)).Invoker(self);

        /// <summary>
        /// Invokes the 'getThreadGroup' method.
        /// </summary>
        public object InvokeGetThreadGroup(object self) => GetMethod(ref getThreadGroup, nameof(getThreadGroup), Resolve("java.lang.ThreadGroup")).Invoker(self);

    }

#endif

}
