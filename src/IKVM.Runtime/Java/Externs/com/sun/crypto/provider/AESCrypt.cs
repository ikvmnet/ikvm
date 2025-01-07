using System;

namespace IKVM.Java.Externs.com.sun.crypto.provider;

internal static partial class AESCrypt
{
    public static bool EncryptBlock(byte[] @in, int inOffset, byte[] @out, int outOffset, int[] K)
    {
#if NETCOREAPP3_0_OR_GREATER
        if (IsSupportedX86)
        {
            EncryptBlock_X86(@in, inOffset, @out, outOffset, K);
            return true;
        }
#endif

        return false;
    }

#if NETCOREAPP3_0_OR_GREATER
    private static ReadOnlySpan<int> KeyShuffleMask => [0x00010203, 0x04050607, 0x08090a0b, 0x0c0d0e0f];
#endif
}
