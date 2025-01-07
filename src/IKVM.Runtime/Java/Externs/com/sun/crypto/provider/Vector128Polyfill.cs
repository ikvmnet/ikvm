#if NETCOREAPP3_0_OR_GREATER && !(NET8_0_OR_GREATER)
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace IKVM.Java.Externs.com.sun.crypto.provider;

internal static class Vector128Polyfill
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<T> Create<T>(ReadOnlySpan<T> values) where T : unmanaged
    {
        if (values.Length < Vector128<T>.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(values));
        }

        return Unsafe.ReadUnaligned<Vector128<T>>(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(values)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyTo<T>(this Vector128<T> vector, Span<T> destination) where T : unmanaged
    {
        if (destination.Length < Vector128<T>.Count)
        {
            throw new ArgumentException(null, nameof(destination));
        }

        Unsafe.WriteUnaligned(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(destination)), vector);
    }
}
#endif
