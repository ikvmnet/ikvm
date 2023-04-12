using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.security.AccessController' type.
    /// </summary>
    internal sealed class AccessControllerAccessor : Accessor<object>
    {

        Type javaSecurityPrivilegedAction;

        MethodAccessor<Func<object, object>> doPrivileged;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public AccessControllerAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.security.AccessController")
        {

        }

        Type JavaSecurityPrivilegedAction => Resolve(ref javaSecurityPrivilegedAction, "java.security.PrivilegedAction");

        /// <summary>
        /// Invokes the 'doPrivledged' method.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public object InvokeDoPrivledged(object action) => GetMethod(ref doPrivileged, nameof(doPrivileged), typeof(object), JavaSecurityPrivilegedAction).Invoker(action);

    }

#endif

}
