using System;
using System.Runtime.CompilerServices;

namespace IKVM.Java.Externs.com.sun.crypto.provider;

internal static class GHASH
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ProcessBlocks(byte[] data, int inOfs, int blocks, long[] st, long[] subH)
    {
#if NETCOREAPP3_0_OR_GREATER
        if (X86.GHASH.IsSupported)
        {
            X86.GHASH.ProcessBlocks(data.AsSpan(inOfs), blocks, st, subH);
            return true;
        }
#endif

        return false;
    }
}
