using IKVM.Attributes;
using System;
using System.Runtime.CompilerServices;

namespace IKVM.Java.Externs.com.sun.crypto.provider;

[HideFromJava]
internal static class AESCrypt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EncryptBlock(byte[] @in, int inOffset, byte[] @out, int outOffset, int[] K)
    {
#if NETCOREAPP3_0_OR_GREATER
        if (X86.AESCrypt.IsSupported)
        {
            X86.AESCrypt.EncryptBlock(@in.AsSpan(inOffset), @out.AsSpan(outOffset), K);
            return true;
        }
#endif

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool DecryptBlock(byte[] @in, int inOffset, byte[] @out, int outOffset, int[] K)
    {
#if NETCOREAPP3_0_OR_GREATER
        if (X86.AESCrypt.IsSupported)
        {
            X86.AESCrypt.DecryptBlock(@in.AsSpan(inOffset), @out.AsSpan(outOffset), K);
            return true;
        }
#endif

        return false;
    }
}
