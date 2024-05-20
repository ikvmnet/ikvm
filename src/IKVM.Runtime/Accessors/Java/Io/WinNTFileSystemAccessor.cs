using System;

namespace IKVM.Runtime.Accessors.Java.Io
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.io.WinNTFileSystem' type.
    /// </summary>
    internal sealed class WinNTFileSystemAccessor : Accessor<object>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public WinNTFileSystemAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.io.WinNTFileSystem")
        {

        }

    }

#endif

}
