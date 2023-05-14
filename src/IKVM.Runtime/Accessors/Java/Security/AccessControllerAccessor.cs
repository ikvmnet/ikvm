using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.security.AccessController' type.
    /// </summary>
    internal sealed class AccessControllerAccessor : Accessor<object>
    {

        Type ikvmInternalCallerID;
        Type javaSecurityPrivilegedAction;
        Type javaSecurityAccessControlContext;

        MethodAccessor<Func<object, object>> doPrivileged;
        MethodAccessor<Func<object, object, object>> doPrivileged2;
        MethodAccessor<Func<object, object, object, object>> doPrivileged3;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public AccessControllerAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.security.AccessController")
        {

        }

        Type IkvmInternalCallerID => Resolve(ref ikvmInternalCallerID, "ikvm.internal.CallerID");

        Type JavaSecurityPrivilegedAction => Resolve(ref javaSecurityPrivilegedAction, "java.security.PrivilegedAction");

        Type JavaSecurityAccessControlContext => Resolve(ref javaSecurityAccessControlContext, "java.security.AccessControlContext");

        /// <summary>
        /// Invokes the 'doPrivileged' method.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public object InvokeDoPrivileged(object action) => GetMethod(ref doPrivileged, nameof(doPrivileged), typeof(object), JavaSecurityPrivilegedAction).Invoker(action);

        /// <summary>
        /// Invokes the 'doPrivileged' method.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public object InvokeDoPrivileged(object action, object accessControlContext) => GetMethod(ref doPrivileged2, "doPrivileged", typeof(object), JavaSecurityPrivilegedAction, JavaSecurityAccessControlContext).Invoker(action, accessControlContext);

        /// <summary>
        /// Invokes the 'doPrivileged' method.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public object InvokeDoPrivileged(object action, object accessControlContext, object callerID) => GetMethod(ref doPrivileged3, "doPrivileged", typeof(object), JavaSecurityPrivilegedAction, JavaSecurityAccessControlContext, IkvmInternalCallerID).Invoker(action, accessControlContext, callerID);

    }

#endif

}
