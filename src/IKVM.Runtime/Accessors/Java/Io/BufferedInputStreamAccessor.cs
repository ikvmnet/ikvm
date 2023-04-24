using System;

namespace IKVM.Runtime.Accessors.Java.Io
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.io.BufferedInputStream' type.
    /// </summary>
    internal sealed class BufferedInputStreamAccessor : Accessor<object>
    {

        MethodAccessor<Func<object, object>> init;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public BufferedInputStreamAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.io.BufferedInputStream")
        {

        }

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="self"></param>
        public object Init(object self) => GetConstructor(ref init, Resolve("java.io.InputStream")).Invoker(self);

    }

#endif

}
