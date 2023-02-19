using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

    /// <summary>
    /// Provides runtime access to the 'java.lang.SecurityManager' type.
    /// </summary>
    internal sealed class SecurityManagerAccessor : Accessor
    {

        MethodAccessor<Action<object, string>> checkRead;
        MethodAccessor<Action<object, string>> checkWrite;
        MethodAccessor<Action<object, string>> checkDelete;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public SecurityManagerAccessor(AccessorTypeResolver resolver) :
            base(resolver("java.lang.SecurityManager"))
        {

        }

        /// <summary>
        /// Invokes the 'checkRead' method.
        /// </summary>
        public void InvokeCheckRead(object self, string path) => GetVoidMethod(ref checkRead, nameof(checkRead)).Invoker(self, path);

        /// <summary>
        /// Invokes the 'checkWrite' method.
        /// </summary>
        public void InvokeCheckWrite(object self, string path) => GetVoidMethod(ref checkWrite, nameof(checkWrite)).Invoker(self, path);

        /// <summary>
        /// Invokes the 'checkDelete' method.
        /// </summary>
        public void InvokeCheckDelete(object self, string path) => GetVoidMethod(ref checkDelete, nameof(checkDelete)).Invoker(self, path);

    }

}
