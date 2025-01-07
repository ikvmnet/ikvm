#if NETCOREAPP3_0_OR_GREATER
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IKVM.Java.Externs.com.sun.crypto.provider;

partial class AESCrypt
{
    partial class X86
    {
        public static bool IsSupported => Aes.IsSupported && Ssse3.IsSupported && Sse2.IsSupported;

        private static Vector128<byte> LoadKey(ReadOnlySpan<int> key, nuint offset, Vector128<byte>? shuffle_mask = null)
        {
#if NET8_0_OR_GREATER
            var xmmdest = Vector128.LoadUnsafe(in Unsafe.As<int, byte>(ref MemoryMarshal.GetReference(key)), offset);
            shuffle_mask ??= Vector128.Create(KeyShuffleMask).AsByte();
#else
            var xmmdest = Vector128Polyfill.LoadUnsafe(in Unsafe.As<int, byte>(ref MemoryMarshal.GetReference(key)), offset);
            shuffle_mask ??= Vector128Polyfill.Create(KeyShuffleMask).AsByte();
#endif

            return Ssse3.Shuffle(xmmdest, shuffle_mask.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EncryptBlock(ReadOnlySpan<byte> @in, Span<byte> @out, ReadOnlySpan<int> K)
        {
            // ext\openjdk\hotspot\src\cpu\x86\vm\stubGenerator_x86_32.cpp:2189-2277
            Vector128<byte> xmm_temp1, xmm_temp2, xmm_temp3, xmm_temp4;
#if NET8_0_OR_GREATER
            var xmm_key_shuf_mask = Vector128.Create(KeyShuffleMask).AsByte();
            var xmm_result = Vector128.Create(@in);
#else
            var xmm_key_shuf_mask = Vector128Polyfill.Create(KeyShuffleMask).AsByte();
            var xmm_result = Vector128Polyfill.Create(@in);
#endif

            xmm_temp1 = LoadKey(K, 0x00, xmm_key_shuf_mask);
            xmm_result = Sse2.Xor(xmm_result, xmm_temp1);

            xmm_temp1 = LoadKey(K, 0x10, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(K, 0x20, xmm_key_shuf_mask);
            xmm_temp3 = LoadKey(K, 0x30, xmm_key_shuf_mask);
            xmm_temp4 = LoadKey(K, 0x40, xmm_key_shuf_mask);

            xmm_result = Aes.Encrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp2);
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp3);
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp4);

            xmm_temp1 = LoadKey(K, 0x50, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(K, 0x60, xmm_key_shuf_mask);
            xmm_temp3 = LoadKey(K, 0x70, xmm_key_shuf_mask);
            xmm_temp4 = LoadKey(K, 0x80, xmm_key_shuf_mask);

            xmm_result = Aes.Encrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp2);
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp3);
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp4);

            xmm_temp1 = LoadKey(K, 0x90, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(K, 0xa0, xmm_key_shuf_mask);

            // ext\openjdk\hotspot\src\cpu\x86\vm\stubGenerator_x86_32.cpp:2215,2216
            if (K.Length != 44)
            {
                xmm_result = Aes.Encrypt(xmm_result, xmm_temp1);
                xmm_result = Aes.Encrypt(xmm_result, xmm_temp2);

                xmm_temp1 = LoadKey(K, 0xb0, xmm_key_shuf_mask);
                xmm_temp2 = LoadKey(K, 0xc0, xmm_key_shuf_mask);

                if (K.Length != 52)
                {
                    xmm_result = Aes.Encrypt(xmm_result, xmm_temp1);
                    xmm_result = Aes.Encrypt(xmm_result, xmm_temp2);

                    xmm_temp1 = LoadKey(K, 0xd0, xmm_key_shuf_mask);
                    xmm_temp2 = LoadKey(K, 0xe0, xmm_key_shuf_mask);
                }
            }

            xmm_result = Aes.Encrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.EncryptLast(xmm_result, xmm_temp2);
            xmm_result.CopyTo(@out);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DecryptBlock(ReadOnlySpan<byte> @in, Span<byte> @out, ReadOnlySpan<int> K)
        {
            // ext\openjdk\hotspot\src\cpu\x86\vm\stubGenerator_x86_32.cpp:3125-3210
            Vector128<byte> xmm_temp1, xmm_temp2, xmm_temp3, xmm_temp4;
#if NET8_0_OR_GREATER
            var xmm_key_shuf_mask = Vector128.Create(KeyShuffleMask).AsByte();
            var xmm_result = Vector128.Create(@in);
#else
            var xmm_key_shuf_mask = Vector128Polyfill.Create(KeyShuffleMask).AsByte();
            var xmm_result = Vector128Polyfill.Create(@in);
#endif

            xmm_temp1 = LoadKey(K, 0x10, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(K, 0x20, xmm_key_shuf_mask);
            xmm_temp3 = LoadKey(K, 0x30, xmm_key_shuf_mask);
            xmm_temp4 = LoadKey(K, 0x40, xmm_key_shuf_mask);

            xmm_result = Sse2.Xor(xmm_result, xmm_temp1);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp2);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp3);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp4);

            xmm_temp1 = LoadKey(K, 0x50, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(K, 0x60, xmm_key_shuf_mask);
            xmm_temp3 = LoadKey(K, 0x70, xmm_key_shuf_mask);
            xmm_temp4 = LoadKey(K, 0x80, xmm_key_shuf_mask);

            xmm_result = Aes.Decrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp2);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp3);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp4);

            xmm_temp1 = LoadKey(K, 0x90, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(K, 0xa0, xmm_key_shuf_mask);
            xmm_temp3 = LoadKey(K, 0x00, xmm_key_shuf_mask);

            if (K.Length != 44)
            {
                xmm_result = Aes.Decrypt(xmm_result, xmm_temp1);
                xmm_result = Aes.Decrypt(xmm_result, xmm_temp2);

                xmm_temp1 = LoadKey(K, 0xb0, xmm_key_shuf_mask);
                xmm_temp2 = LoadKey(K, 0xc0, xmm_key_shuf_mask);

                if (K.Length != 52)
                {
                    xmm_result = Aes.Decrypt(xmm_result, xmm_temp1);
                    xmm_result = Aes.Decrypt(xmm_result, xmm_temp2);

                    xmm_temp1 = LoadKey(K, 0xd0, xmm_key_shuf_mask);
                    xmm_temp2 = LoadKey(K, 0xe0, xmm_key_shuf_mask);
                }
            }

            xmm_result = Aes.Decrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp2);
            xmm_result = Aes.DecryptLast(xmm_result, xmm_temp3);
            xmm_result.CopyTo(@out);
        }
    }
}
#endif
