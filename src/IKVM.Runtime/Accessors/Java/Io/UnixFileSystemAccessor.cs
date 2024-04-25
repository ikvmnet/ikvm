using System;

namespace IKVM.Runtime.Accessors.Java.Io
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.io.UnixFileSystem' type.
    /// </summary>
    internal sealed class UnixFileSystemAccessor : Accessor<object>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public UnixFileSystemAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.io.UnixFileSystem")
        {

        }

    }

#endif

}
