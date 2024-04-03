using System;
using System.Runtime.InteropServices;

namespace IKVM.Runtime.Extensions
{

    class MemoryMarshalExtensions
    {

        /// <summary>
        /// Gets the length of a pointer to a C string.
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static unsafe int GetIndexOfNull(byte* ptr, int max = int.MaxValue)
        {
            if (ptr is null)
                throw new ArgumentNullException(nameof(ptr));
            if (max < 0)
                throw new ArgumentOutOfRangeException(nameof(max));

            for (int i = 0; i < max; i++)
                if (ptr[i] == 0)
                    return i;

            return -1;
        }

        /// <summary>
        /// Creates a <see cref="ReadOnlySpan{byte}"/> from the given pointer
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static unsafe ReadOnlySpan<byte> CreateReadOnlySpanFromNullTerminated(byte* b)
        {
#if NETFRAMEWORK
            return new ReadOnlySpan<byte>(b, GetIndexOfNull(b));
#else
            return MemoryMarshal.CreateReadOnlySpanFromNullTerminated(b);
#endif
        }

    }

}
