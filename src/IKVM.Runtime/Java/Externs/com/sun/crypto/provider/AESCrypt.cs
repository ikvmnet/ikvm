using IKVM.Attributes;
using System;

namespace IKVM.Java.Externs.com.sun.crypto.provider;

[HideFromJava]
internal static partial class AESCrypt
{
    public static bool EncryptBlock(byte[] @in, int inOffset, byte[] @out, int outOffset, int[] K)
    {
#if NETCOREAPP3_0_OR_GREATER
        if (X86.IsSupported)
        {
            X86.EncryptBlock(@in, inOffset, @out, outOffset, K);
            return true;
        }
#endif

        return false;
    }

    public static bool DecryptBlock(byte[] @in, int inOffset, byte[] @out, int outOffset, int[] K)
    {
#if NETCOREAPP3_0_OR_GREATER
        if (X86.IsSupported)
        {
            X86.DecryptBlock(@in, inOffset, @out, outOffset, K);
            return true;
        }
#endif

        return false;
    }

#if NETCOREAPP3_0_OR_GREATER
    private static partial class X86
    {
        public static ReadOnlySpan<int> KeyShuffleMask => [0x00010203, 0x04050607, 0x08090a0b, 0x0c0d0e0f];
    }
#endif
}
