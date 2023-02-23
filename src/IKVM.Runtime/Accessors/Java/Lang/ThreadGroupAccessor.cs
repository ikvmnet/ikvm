using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

    /// <summary>
    /// Provides runtime access to the 'java.lang.ThreadGroup' type.
    /// </summary>
    internal sealed class ThreadGroupAccessor : Accessor
    {

        ConstructorAccessor<Func<object, object, string, object>> init1;
        ConstructorAccessor<Func<object, string, object>> init2;
        ConstructorAccessor<Func<object>> init3;
        StaticMethodAccessor<Func<string, object>> createRootGroup;
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
        public object Init(object unused, object parent, string name) => GetConstructor(ref init1).Invoker(unused, parent, name);

        /// <summary>
        /// Invokes the constructor.
        /// </summary>
        /// <returns></returns>
        public object Init(object parent, string name) => GetConstructor(ref init2).Invoker(parent, name);

        /// <summary>
        /// Invokes the constructor.
        /// </summary>
        /// <returns></returns>
        public object Init() => GetConstructor(ref init3).Invoker();

        /// <summary>
        /// Inovkes the 'createRootGroup' method.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object InvokeCreateRootGroup(string name) => GetStaticMethod(ref createRootGroup, name);

        /// <summary>
        /// Invokes the 'uncaughtException' method.
        /// </summary>
        public void InvokeUncaughtException(object self, object t, object e) => GetVoidMethod(ref uncaughtException, nameof(uncaughtException)).Invoker(self, t, e);

    }

}
