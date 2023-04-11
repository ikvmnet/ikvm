using System;

namespace IKVM.Runtime.Accessors.Java.Nio.File
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.nio.Buffer' type.
    /// </summary>
    internal sealed class DirectoryStreamFilterAccessor : Accessor<object>
    {

        MethodAccessor<Func<object, object, bool>> accept;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public DirectoryStreamFilterAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.nio.file.DirectoryStream+Filter")
        {

        }

        /// <summary>
        /// Invokes the 'accept' method.
        /// </summary>
        public bool InvokeAccept(object self, object entry) => GetMethod(ref accept, nameof(accept), typeof(bool), typeof(object)).Invoker(self, entry);

    }

#endif

}
