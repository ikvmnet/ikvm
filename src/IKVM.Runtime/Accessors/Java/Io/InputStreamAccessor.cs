using System;

namespace IKVM.Runtime.Accessors.Java.Io
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.io.InputStream' type.
    /// </summary>
    internal sealed class InputStreamAccessor : Accessor<object>
    {

        MethodAccessor<Action<object>> close;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public InputStreamAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.io.InputStream")
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
