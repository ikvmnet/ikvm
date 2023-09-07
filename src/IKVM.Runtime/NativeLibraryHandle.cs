using System;
using System.Runtime.InteropServices;

namespace IKVM.Runtime
{

#if FIRST_PASS == false && IMPORTER == FALSE && EXPORTER == false

    /// <summary>
    /// Represents a handle to a module loaded by <see cref="NativeLibrary"/>.
    /// </summary>
    internal class NativeLibraryHandle : SafeHandle
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="handle"></param>
        internal NativeLibraryHandle(IntPtr handle) :
            base(IntPtr.Zero, true)
        {
            SetHandle(handle);
        }

        /// <inheritdoc />
        public override bool IsInvalid => handle == IntPtr.Zero;

        /// <inheritdoc />
        protected override bool ReleaseHandle()
        {
            NativeLibrary.Free(handle);
            return true;
        }

        /// <summary>
        /// Gets a reference to the given export.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public NativeLibraryExport GetExport(string name) => NativeLibrary.GetExport(handle, name) is nint h and not 0 ? new NativeLibraryExport(this, h) : null;

    }

#endif

}
