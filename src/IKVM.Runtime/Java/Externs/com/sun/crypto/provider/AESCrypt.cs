using IKVM.Attributes;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IKVM.Java.Externs.com.sun.crypto.provider;

[HideFromJava]
internal static partial class AESCrypt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EncryptBlock(byte[] @in, int inOffset, byte[] @out, int outOffset, int[] K)
    {
#if NETCOREAPP3_0_OR_GREATER
        if (X86.IsSupported)
        {
            X86.EncryptBlock(@in.AsSpan(inOffset), @out.AsSpan(outOffset), K);
            return true;
        }
#endif

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool DecryptBlock(byte[] @in, int inOffset, byte[] @out, int outOffset, int[] K)
    {
#if NETCOREAPP3_0_OR_GREATER
        if (X86.IsSupported)
        {
            X86.DecryptBlock(@in.AsSpan(inOffset), @out.AsSpan(outOffset), K);
            return true;
        }
#endif

        return false;
    }

#if NETCOREAPP3_0_OR_GREATER
    private static partial class X86
    {
        private static ReadOnlySpan<int> KeyShuffleMask => [0x00010203, 0x04050607, 0x08090a0b, 0x0c0d0e0f];
    }
#endif
}
