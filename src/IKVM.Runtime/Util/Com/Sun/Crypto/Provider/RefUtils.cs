#if NET6_0_OR_GREATER

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IKVM.Runtime.Util.Com.Sun.Crypto.Provider
{
    unsafe static class RefUtils
    {
        public static ref T Post<T>(ref Span<T> data, nint byteOffset) where T : unmanaged
        {
            ref T element = ref MemoryMarshal.GetReference(data);
            data = MemoryMarshal.CreateSpan(ref Unsafe.AddByteOffset(ref element, byteOffset), data.Length - (int)(byteOffset / sizeof(T)));
            return ref element;
        }

        public static ref T Post<T>(ref ReadOnlySpan<T> data, nint byteOffset) where T : unmanaged
        {
            ref T element = ref MemoryMarshal.GetReference(data);
            data = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AddByteOffset(ref element, byteOffset), data.Length - (int)(byteOffset / sizeof(T)));
            return ref element;
        }
    }
}

#endif