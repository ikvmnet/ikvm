using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

    /// <summary>
    /// Provides runtime access to the 'java.lang.Thread' type.
    /// </summary>
    internal sealed class ThreadAccessor : Accessor
    {

        StaticMethodAccessor<Func<object>> currentThread;
        MethodAccessor<Action<object>> die;
        MethodAccessor<Func<object, object>> getThreadGroup;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public ThreadAccessor(AccessorTypeResolver resolver) :
            base(resolver("java.lang.Thread"))
        {

        }

        /// <summary>
        /// Invokes the 'currentThread' method.
        /// </summary>
        public object InvokeCurrentThread() => GetStaticMethod(ref currentThread, nameof(currentThread)).Invoker();

        /// <summary>
        /// Invokes the 'die' method.
        /// </summary>
        public void InvokeDie(object self) => GetVoidMethod(ref die, nameof(die)).Invoker(self);

        /// <summary>
        /// Invokes the 'getThreadGroup' method.
        /// </summary>
        public object InvokeGetThreadGroup(object self) => GetMethod(ref getThreadGroup, nameof(getThreadGroup)).Invoker(self);

    }

}
