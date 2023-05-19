using System;

namespace IKVM.Runtime.Accessors.Java.Io
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.io.BufferedOutputStream' type.
    /// </summary>
    internal sealed class BufferedOutputStreamAccessor : Accessor<object>
    {

        MethodAccessor<Func<object, object>> init;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public BufferedOutputStreamAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.io.BufferedOutputStream")
        {

        }

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="self"></param>
        public object Init(object self) => GetConstructor(ref init, Resolve("java.io.OutputStream")).Invoker(self);

    }

#endif

}
