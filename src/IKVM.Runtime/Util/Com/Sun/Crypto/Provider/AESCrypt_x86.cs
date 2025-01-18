#if NETCOREAPP3_0_OR_GREATER

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IKVM.Runtime.Util.Com.Sun.Crypto.Provider
{

    /// <summary>
    /// X86 implementation of the AES intrinsic functions.
    /// </summary>
    static class AESCrypt_x86
    {

        /// <summary>
        /// Returns <c>true</c> if the current platform is supported by this implementation.
        /// </summary>
        public static bool IsSupported => Aes.IsSupported && Ssse3.IsSupported;

        public static ReadOnlySpan<int> KeyShuffleMask => [0x00010203, 0x04050607, 0x08090a0b, 0x0c0d0e0f];

        /// <summary>
        /// Implementation of com.sun.crypto.provider.AESCrypt.decryptBlock for the x86 platform.
        /// Derived from the OpenJDK C code 'stubGenerator_x86_32.cpp:generate_aescrypt_decryptBlock'.
        /// Keep the structure of the body of this method as close to the orignal C code as possible to facilitate porting changes.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="key"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DecryptBlock(ReadOnlySpan<byte> from, Span<byte> to, ReadOnlySpan<int> key)
        {
            // ext\openjdk\hotspot\src\cpu\x86\vm\stubGenerator_x86_32.cpp:3125-3210
            Vector128<byte> xmm_temp1, xmm_temp2, xmm_temp3, xmm_temp4;
            var xmm_key_shuf_mask = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(KeyShuffleMask), 0).AsByte();
            var xmm_result = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(from));

            xmm_temp1 = LoadKey(key, 0x10, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(key, 0x20, xmm_key_shuf_mask);
            xmm_temp3 = LoadKey(key, 0x30, xmm_key_shuf_mask);
            xmm_temp4 = LoadKey(key, 0x40, xmm_key_shuf_mask);

            xmm_result = Sse2.Xor(xmm_result, xmm_temp1);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp2);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp3);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp4);

            xmm_temp1 = LoadKey(key, 0x50, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(key, 0x60, xmm_key_shuf_mask);
            xmm_temp3 = LoadKey(key, 0x70, xmm_key_shuf_mask);
            xmm_temp4 = LoadKey(key, 0x80, xmm_key_shuf_mask);

            xmm_result = Aes.Decrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp2);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp3);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp4);

            xmm_temp1 = LoadKey(key, 0x90, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(key, 0xa0, xmm_key_shuf_mask);
            xmm_temp3 = LoadKey(key, 0x00, xmm_key_shuf_mask);

            if (key.Length == 44)
            {
                goto doLast;
            }

            xmm_result = Aes.Decrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp2);

            xmm_temp1 = LoadKey(key, 0xb0, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(key, 0xc0, xmm_key_shuf_mask);

            if (key.Length == 52)
            {
                goto doLast;
            }

            xmm_result = Aes.Decrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp2);

            xmm_temp1 = LoadKey(key, 0xd0, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(key, 0xe0, xmm_key_shuf_mask);

        doLast:;
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.Decrypt(xmm_result, xmm_temp2);
            xmm_result = Aes.DecryptLast(xmm_result, xmm_temp3);
            xmm_result.CopyTo(to);
        }

        /// <summary>
        /// Implementation of com.sun.crypto.provider.AESCrypt.encryptBlock for the x86 platform.
        /// Derived from the OpenJDK C code 'stubGenerator_x86_32.cpp:generate_aescrypt_encryptBlock'.
        /// Keep the structure of the body of this method as close to the orignal C code as possible to facilitate porting changes.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="key"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EncryptBlock(ReadOnlySpan<byte> from, Span<byte> to, ReadOnlySpan<int> key)
        {
            // ext\openjdk\hotspot\src\cpu\x86\vm\stubGenerator_x86_32.cpp:2189-2277
            Vector128<byte> xmm_temp1, xmm_temp2, xmm_temp3, xmm_temp4;
            var xmm_key_shuf_mask = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(KeyShuffleMask)).AsByte();
            var xmm_result = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(from));

            xmm_temp1 = LoadKey(key, 0x00, xmm_key_shuf_mask);
            xmm_result = Sse2.Xor(xmm_result, xmm_temp1);

            xmm_temp1 = LoadKey(key, 0x10, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(key, 0x20, xmm_key_shuf_mask);
            xmm_temp3 = LoadKey(key, 0x30, xmm_key_shuf_mask);
            xmm_temp4 = LoadKey(key, 0x40, xmm_key_shuf_mask);

            xmm_result = Aes.Encrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp2);
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp3);
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp4);

            xmm_temp1 = LoadKey(key, 0x50, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(key, 0x60, xmm_key_shuf_mask);
            xmm_temp3 = LoadKey(key, 0x70, xmm_key_shuf_mask);
            xmm_temp4 = LoadKey(key, 0x80, xmm_key_shuf_mask);

            xmm_result = Aes.Encrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp2);
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp3);
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp4);

            xmm_temp1 = LoadKey(key, 0x90, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(key, 0xa0, xmm_key_shuf_mask);

            if (key.Length == 44)
            {
                goto doLast;
            }

            xmm_result = Aes.Encrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp2);

            xmm_temp1 = LoadKey(key, 0xb0, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(key, 0xc0, xmm_key_shuf_mask);

            if (key.Length == 52)
            {
                goto doLast;
            }

            xmm_result = Aes.Encrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp2);

            xmm_temp1 = LoadKey(key, 0xd0, xmm_key_shuf_mask);
            xmm_temp2 = LoadKey(key, 0xe0, xmm_key_shuf_mask);

        doLast:;
            xmm_result = Aes.Encrypt(xmm_result, xmm_temp1);
            xmm_result = Aes.EncryptLast(xmm_result, xmm_temp2);
            xmm_result.CopyTo(to);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector128<byte> LoadKey(ReadOnlySpan<int> key, nuint offset, Vector128<byte>? shuffle_mask = null)
        {
            var xmm_temp = Vector128Util.LoadUnsafe(in Unsafe.As<int, byte>(ref MemoryMarshal.GetReference(key)), offset);
            shuffle_mask ??= Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(KeyShuffleMask)).AsByte();

            return Ssse3.Shuffle(xmm_temp, shuffle_mask.Value);
        }

    }

}

#endif
