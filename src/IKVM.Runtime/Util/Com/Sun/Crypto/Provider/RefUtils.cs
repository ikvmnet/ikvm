#if NET6_0_OR_GREATER

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IKVM.Runtime.Util.Com.Sun.Crypto.Provider
{
    unsafe static class RefUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Post<T>(ref Span<T> data, nint byteOffset) where T : struct
        {
            ref T element = ref MemoryMarshal.GetReference(data);
#if NET8_0_OR_GREATER
            data = new(ref Unsafe.AddByteOffset(ref element, byteOffset));
#else
            data = MemoryMarshal.CreateSpan(ref Unsafe.AddByteOffset(ref element, byteOffset), 1);
#endif
            return ref element;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Post<T>(ref ReadOnlySpan<T> data, nint byteOffset) where T : struct
        {
            ref T element = ref MemoryMarshal.GetReference(data);
#if NET8_0_OR_GREATER
            data = new(in Unsafe.AddByteOffset(ref element, byteOffset));
#else
            data = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AddByteOffset(ref element, byteOffset), 1);
#endif
            return ref element;
        }
    }
}

#endif