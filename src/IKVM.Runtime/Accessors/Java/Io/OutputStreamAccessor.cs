using System;

namespace IKVM.Runtime.Accessors.Java.Io
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.io.OutputStream' type.
    /// </summary>
    internal sealed class OutputStreamAccessor : Accessor<object>
    {

        MethodAccessor<Action<object>> close;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public OutputStreamAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.io.OutputStream")
        {

        }

        /// <summary>
        /// Invokes the 'close' method.
        /// </summary>
        /// <param name="self"></param>
        public void InvokeClose(object self) => GetMethod(ref close, nameof(close), typeof(void)).Invoker(self);

    }

#endif

}
