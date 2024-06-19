using System;
using System.Threading;

namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.lang.Thread' type.
    /// </summary>
    internal sealed class ThreadAccessor : Accessor<object>
    {

        Type javaLangThread;
        Type javaLangThreadGroup;

        FieldAccessor<object> current;
        FieldAccessor<object, Thread> nativeThread;
        MethodAccessor<Func<object>> currentThread;
        MethodAccessor<Func<bool>> interrupted;

        MethodAccessor<Func<object, object>> init;
        MethodAccessor<Func<object, bool>> isDaemon;
        MethodAccessor<Func<object, bool>> isInterrupted;
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

        Type JavaLangThread => Resolve(ref javaLangThread, "java.lang.Thread");

        Type JavaLangThreadGroup => Resolve(ref javaLangThreadGroup, "java.lang.ThreadGroup");

        /// <summary>
        /// Gets the value of the 'current' field.
        /// </summary>
        /// <returns></returns>
        public object GetCurrent() => GetField(ref current, nameof(current)).GetValue();

        /// <summary>
        /// Gets the value of the 'nativeThread' field.
        /// </summary>
        /// <returns></returns>
        public Thread GetNativeThread(object self) => GetField(ref nativeThread, nameof(nativeThread)).GetValue(self);

        /// <summary>
        /// Invokes the 'currentThread' method.
        /// </summary>
        public object InvokeCurrentThread() => GetMethod(ref currentThread, nameof(currentThread), JavaLangThread).Invoker();

        /// <summary>
        /// Invokes the 'interrupted' method.
        /// </summary>
        /// <returns></returns>
        public bool InvokeIsInterrupted(object self) => GetMethod(ref isInterrupted, nameof(isInterrupted), typeof(bool)).Invoker(self);

        /// <summary>
        /// Invokes the constructor.
        /// </summary>
        /// <param name="threadGroup"></param>
        /// <returns></returns>
        public object Init(object threadGroup) => GetConstructor(ref init, JavaLangThreadGroup).Invoker(threadGroup);

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
        public object InvokeGetThreadGroup(object self) => GetMethod(ref getThreadGroup, nameof(getThreadGroup), JavaLangThreadGroup).Invoker(self);

        /// <summary>
        /// Invokes the 'interrupted' method.
        /// </summary>
        /// <returns></returns>
        public bool InvokeInterrupted() => GetMethod(ref interrupted, nameof(interrupted), typeof(bool)).Invoker();

    }

#endif

}
