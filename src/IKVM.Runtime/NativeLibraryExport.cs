using System;

namespace IKVM.Runtime
{

#if FIRST_PASS == false && IMPORTER == FALSE && EXPORTER == false

    /// <summary>
    /// Represents a handle to an export loaded by <see cref="NativeLibrary"/>.
    /// </summary>
    internal class NativeLibraryExport
    {

        readonly NativeLibraryHandle lib;
        readonly nint handle;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="lib"></param>
        /// <param name="handle"></param>
        internal NativeLibraryExport(NativeLibraryHandle lib, IntPtr handle)
        {
            this.lib = lib ?? throw new ArgumentNullException(nameof(lib));
            this.handle = handle;
        }

        /// <summary>
        /// Gets the handle of the library.
        /// </summary>
        public NativeLibraryHandle Library => lib;

        /// <summary>
        /// Gets the handle of the export.
        /// </summary>
        public nint Handle => handle;

    }

#endif

}
