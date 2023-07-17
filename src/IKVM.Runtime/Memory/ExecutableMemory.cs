using System;
using System.Runtime.InteropServices;

namespace IKVM.Runtime.JNI.Memory
{

    /// <summary>
    /// Abstract type for reference to platform specifiec executable memory allocation.
    /// </summary>
    abstract class ExecutableMemory : SafeHandle
    {

        /// <summary>
        /// Casts the memory to a Span.
        /// </summary>
        /// <param name="handle"></param>

        public static unsafe implicit operator Span<byte>(ExecutableMemory handle)
        {
            return new Span<byte>((void*)handle.DangerousGetHandle(), handle.size);
        }

        /// <summary>
        /// Allocates executable memory of the specified size.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static ExecutableMemory Allocate(int size)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return Win32ExecutableVirtualMemory.Allocate(size);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return PosixExecutableVirtualMemory.Allocate(size);
            else
                throw new PlatformNotSupportedException();
        }

        readonly int size;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected ExecutableMemory(IntPtr handle, int size) :
            base(IntPtr.Zero, true)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            this.size = size;
            SetHandle(handle);
        }

        /// <summary>
        /// Gets the size of the allocated region.
        /// </summary>
        public int Size => size;

        /// <summary>
        /// Returns <c>true</c> if the handle is invalid.
        /// </summary>
        public override bool IsInvalid => handle == IntPtr.Zero;

    }

}
