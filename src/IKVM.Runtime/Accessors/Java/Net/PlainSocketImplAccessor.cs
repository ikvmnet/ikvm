using System;

namespace IKVM.Runtime.Accessors.Java.Io
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.io.PlainSocketImpl' type.
    /// </summary>
    internal sealed class PlainSocketImplAccessor : Accessor<object>
    {

        Type ikvmInternalCallerID;

        MethodAccessor<Func<object>> ___getCallerID_;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public PlainSocketImplAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.net.PlainSocketImpl")
        {

        }

        Type IkvmInternalCallerID => Resolve(ref ikvmInternalCallerID, "ikvm.internal.CallerID");

        /// <summary>
        /// Invokes the '__<GetCallerID>' method.
        /// </summary>
        /// <returns></returns>
        public object InvokeGetCallerID() => GetMethod(ref ___getCallerID_, "__<GetCallerID>", IkvmInternalCallerID).Invoker();

    }

#endif

}
