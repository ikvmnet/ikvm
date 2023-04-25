using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.lang.SecurityManager' type.
    /// </summary>
    internal sealed class SecurityManagerAccessor : Accessor<object>
    {

        MethodAccessor<Action<object, string>> checkRead;
        MethodAccessor<Action<object, string>> checkWrite;
        MethodAccessor<Action<object, string>> checkDelete;
        MethodAccessor<Action<object, string>> checkExec;
        MethodAccessor<Action<object, string, int>> checkAccept;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public SecurityManagerAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.lang.SecurityManager")
        {

        }

        /// <summary>
        /// Invokes the 'checkRead' method.
        /// </summary>
        public void InvokeCheckRead(object self, string path) => GetMethod(ref checkRead, nameof(checkRead), typeof(void), typeof(string)).Invoker(self, path);

        /// <summary>
        /// Invokes the 'checkWrite' method.
        /// </summary>
        public void InvokeCheckWrite(object self, string path) => GetMethod(ref checkWrite, nameof(checkWrite), typeof(void), typeof(string)).Invoker(self, path);

        /// <summary>
        /// Invokes the 'checkDelete' method.
        /// </summary>
        public void InvokeCheckDelete(object self, string path) => GetMethod(ref checkDelete, nameof(checkDelete), typeof(void), typeof(string)).Invoker(self, path);

        /// <summary>
        /// Invokes the 'checkExec' method.
        /// </summary>
        public void InvokeCheckExec(object self, string path) => GetMethod(ref checkExec, nameof(checkExec), typeof(void), typeof(string)).Invoker(self, path);

        /// <summary>
        /// Invokes the 'checkAccept' method.
        /// </summary>
        public void InvokeCheckAccept(object self, string host, int port) => GetMethod(ref checkAccept, nameof(checkAccept), typeof(void), typeof(string), typeof(int)).Invoker(self, host, port);

    }

#endif

}
