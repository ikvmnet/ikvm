#if NETCOREAPP3_0_OR_GREATER
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IKVM.Java.Externs.com.sun.crypto.provider;

partial class AESCrypt
{
    private static bool IsSupportedX86 => Aes.IsSupported && Ssse3.IsSupported && Sse2.IsSupported;

    private static Vector128<byte> LoadKey_X86(ReadOnlySpan<int> key, int offset, Vector128<byte>? shuffle_mask = null)
    {
#if NET8_0_OR_GREATER
        var xmmdest = Vector128.Create(key[offset..]).AsByte();
        shuffle_mask ??= Vector128.Create(KeyShuffleMask).AsByte();
#else
        var xmmdest = Vector128Polyfill.Create(key).AsByte();
        shuffle_mask ??= Vector128Polyfill.Create(KeyShuffleMask).AsByte();
#endif
        Ssse3.Shuffle(xmmdest, shuffle_mask.Value);
        return xmmdest;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void EncryptBlock_X86(byte[] @in, int inOffset, byte[] @out, int outOffset, int[] K)
    {
        // ext\openjdk\hotspot\src\cpu\x86\vm\stubGenerator_x86_32.cpp:2189-2277

        Vector128<byte> xmm_temp1, xmm_temp2, xmm_temp3, xmm_temp4;
#if NET8_0_OR_GREATER
        var xmm_key_shuf_mask = Vector128.Create(KeyShuffleMask).AsByte();
        var xmm_result = Vector128.Create(@in, inOffset);
#else
        var xmm_key_shuf_mask = Vector128Polyfill.Create(KeyShuffleMask).AsByte();
        var xmm_result = Vector128Polyfill.Create(@in, inOffset);
#endif

        xmm_temp1 = LoadKey_X86(K, 0x00, xmm_key_shuf_mask);
        Sse2.Xor(xmm_result, xmm_temp1);

        xmm_temp1 = LoadKey_X86(K, 0x10, xmm_key_shuf_mask);
        xmm_temp2 = LoadKey_X86(K, 0x20, xmm_key_shuf_mask);
        xmm_temp3 = LoadKey_X86(K, 0x30, xmm_key_shuf_mask);
        xmm_temp4 = LoadKey_X86(K, 0x40, xmm_key_shuf_mask);

        xmm_result = Aes.Encrypt(xmm_result, xmm_temp1);
        xmm_result = Aes.Encrypt(xmm_result, xmm_temp2);
        xmm_result = Aes.Encrypt(xmm_result, xmm_temp3);
        xmm_result = Aes.Encrypt(xmm_result, xmm_temp4);

        xmm_temp1 = LoadKey_X86(K, 0x50, xmm_key_shuf_mask);
        xmm_temp2 = LoadKey_X86(K, 0x60, xmm_key_shuf_mask);
        xmm_temp3 = LoadKey_X86(K, 0x70, xmm_key_shuf_mask);
        xmm_temp4 = LoadKey_X86(K, 0x80, xmm_key_shuf_mask);

        xmm_result = Aes.Encrypt(xmm_result, xmm_temp1);
        xmm_result = Aes.Encrypt(xmm_result, xmm_temp2);
        xmm_result = Aes.Encrypt(xmm_result, xmm_temp3);
        xmm_result = Aes.Encrypt(xmm_result, xmm_temp4);

        xmm_temp1 = LoadKey_X86(K, 0x90, xmm_key_shuf_mask);
        xmm_temp2 = LoadKey_X86(K, 0xa0, xmm_key_shuf_mask);

        /*
         * ext\openjdk\hotspot\src\cpu\x86\vm\stubGenerator_x86_32.cpp:2215,2216
         */
        if (K.Length != 11)
        {
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp2);

            xmm_temp1 = LoadKey_X86(K, 0xb0, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey_X86(K, 0xc0, xmm_key_shuf_mask);

            if (K.Length != 13)
            {
                xmm_result = Aes.Encrypt(xmm_result, xmm_temp1);
                xmm_result = Aes.Encrypt(xmm_result, xmm_temp2);

                xmm_temp1 = LoadKey_X86(K, 0xd0, xmm_key_shuf_mask);
                xmm_temp2 = LoadKey_X86(K, 0xe0, xmm_key_shuf_mask);
            }
        }

        xmm_result = Aes.Encrypt(xmm_result, xmm_temp1);
        xmm_result = Aes.EncryptLast(xmm_result, xmm_temp2);
        _ = xmm_result.TryCopyTo(@out.AsSpan(outOffset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DecryptBlock_X86(byte[] @in, int inOffset, byte[] @out, int outOffset, int[] K)
    {
        // ext\openjdk\hotspot\src\cpu\x86\vm\stubGenerator_x86_32.cpp:3125-3210

        Vector128<byte> xmm_temp1, xmm_temp2, xmm_temp3, xmm_temp4;
#if NET8_0_OR_GREATER
        var xmm_key_shuf_mask = Vector128.Create(KeyShuffleMask).AsByte();
        var xmm_result = Vector128.Create(@in, inOffset);
#else
        var xmm_key_shuf_mask = Vector128Polyfill.Create(KeyShuffleMask).AsByte();
        var xmm_result = Vector128Polyfill.Create(@in, inOffset);
#endif

        xmm_temp1 = LoadKey_X86(K, 0x10, xmm_key_shuf_mask);
        xmm_temp2 = LoadKey_X86(K, 0x20, xmm_key_shuf_mask);
        xmm_temp3 = LoadKey_X86(K, 0x30, xmm_key_shuf_mask);
        xmm_temp4 = LoadKey_X86(K, 0x40, xmm_key_shuf_mask);

        xmm_result = Sse2.Xor(xmm_result, xmm_temp1);
        xmm_result = Aes.Decrypt(xmm_result, xmm_temp2);
        xmm_result = Aes.Decrypt(xmm_result, xmm_temp3);
        xmm_result = Aes.Decrypt(xmm_result, xmm_temp4);

        xmm_temp1 = LoadKey_X86(K, 0x50, xmm_key_shuf_mask);
        xmm_temp2 = LoadKey_X86(K, 0x60, xmm_key_shuf_mask);
        xmm_temp3 = LoadKey_X86(K, 0x70, xmm_key_shuf_mask);
        xmm_temp4 = LoadKey_X86(K, 0x80, xmm_key_shuf_mask);

        xmm_result = Aes.Decrypt(xmm_result, xmm_temp1);
        xmm_result = Aes.Decrypt(xmm_result, xmm_temp2);
        xmm_result = Aes.Decrypt(xmm_result, xmm_temp3);
        xmm_result = Aes.Decrypt(xmm_result, xmm_temp4);

        xmm_temp1 = LoadKey_X86(K, 0x90, xmm_key_shuf_mask);
        xmm_temp2 = LoadKey_X86(K, 0xa0, xmm_key_shuf_mask);
        xmm_temp3 = LoadKey_X86(K, 0x00, xmm_key_shuf_mask);

        if (K.Length != 11)
        {
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp2);

            xmm_temp1 = LoadKey_X86(K, 0xb0, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey_X86(K, 0xc0, xmm_key_shuf_mask);

            if (K.Length != 13)
            {
                xmm_result = Aes.Decrypt(xmm_result, xmm_temp1);
                xmm_result = Aes.Decrypt(xmm_result, xmm_temp2);

                xmm_temp1 = LoadKey_X86(K, 0xd0, xmm_key_shuf_mask);
                xmm_temp2 = LoadKey_X86(K, 0xe0, xmm_key_shuf_mask);
            }
        }

        xmm_result = Aes.Decrypt(xmm_result, xmm_temp1);
        xmm_result = Aes.Decrypt(xmm_result, xmm_temp2);
        xmm_result = Aes.DecryptLast(xmm_result, xmm_temp3);
        _ = xmm_result.TryCopyTo(@out.AsSpan(outOffset));
    }
}
#endif
