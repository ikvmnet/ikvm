#if NET6_0_OR_GREATER

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;

namespace IKVM.Runtime.Util.Com.Sun.Crypto.Provider
{
    /// <summary>
    /// Arm implementation of the AES intrinsic functions.
    /// </summary>
    [SkipLocalsInit]
    static class AESCrypt_Arm
    {
        /// <summary>
        /// Returns <c>true</c> if the current platform is supported by this implementation.
        /// </summary>
        public static bool IsSupported => Aes.IsSupported && AdvSimd.IsSupported;

        /// <summary>
        /// Implementation of com.sun.crypto.provider.AESCrypt.decryptBlock for the Arm platform.
        /// Derived from the OpenJDK C code 'stubGenerator_aarch64.cpp:generate_aescrypt_decryptBlock'.
        /// Keep the structure of the body of this method as close to the orignal C code as possible to facilitate porting changes.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="key"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DecryptBlock(ReadOnlySpan<byte> from, Span<byte> to, ReadOnlySpan<int> key)
        {
            //ext\openjdk\hotspot\src\cpu\aarch64\vm\stubGenerator_aarch64.cpp:2606-2699
            var keylen = key.Length;

            var v0 = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(from));

            // ld1(…, __ post(key, X)):
            // ld1(v1, v2, …) was added with .NET 9 (AdvSimd.Arm64.Load_xVector128)
            // Unrolled the Load-loop manually for .NET 6+
            // post post-increments the pointer, and returns the previous location.

            var v5 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
            v5 = AdvSimd.ReverseElement8(v5);

            var (v1, v2, v3, v4) = Vector128Util.Load4xUnsafe(in RefUtils.Post(ref key, 64));
            v1 = AdvSimd.ReverseElement8(v1);
            v2 = AdvSimd.ReverseElement8(v2);
            v3 = AdvSimd.ReverseElement8(v3);
            v4 = AdvSimd.ReverseElement8(v4);
            v0 = Aes.Decrypt(v0, v1.AsByte());
            v0 = Aes.InverseMixColumns(v0);
            v0 = Aes.Decrypt(v0, v2.AsByte());
            v0 = Aes.InverseMixColumns(v0);
            v0 = Aes.Decrypt(v0, v3.AsByte());
            v0 = Aes.InverseMixColumns(v0);
            v0 = Aes.Decrypt(v0, v4.AsByte());
            v0 = Aes.InverseMixColumns(v0);

            (v1, v2, v3, v4) = Vector128Util.Load4xUnsafe(in RefUtils.Post(ref key, 64));
            v1 = AdvSimd.ReverseElement8(v1);
            v2 = AdvSimd.ReverseElement8(v2);
            v3 = AdvSimd.ReverseElement8(v3);
            v4 = AdvSimd.ReverseElement8(v4);
            v0 = Aes.Decrypt(v0, v1.AsByte());
            v0 = Aes.InverseMixColumns(v0);
            v0 = Aes.Decrypt(v0, v2.AsByte());
            v0 = Aes.InverseMixColumns(v0);
            v0 = Aes.Decrypt(v0, v3.AsByte());
            v0 = Aes.InverseMixColumns(v0);
            v0 = Aes.Decrypt(v0, v4.AsByte());
            v0 = Aes.InverseMixColumns(v0);

            (v1, v2) = Vector128Util.Load2xUnsafe(in RefUtils.Post(ref key, 32));
            v1 = AdvSimd.ReverseElement8(v1);
            v2 = AdvSimd.ReverseElement8(v2);

            if (keylen == 44)
            {
                goto doLast;
            }

            v0 = Aes.Decrypt(v0, v1.AsByte());
            v0 = Aes.InverseMixColumns(v0);
            v0 = Aes.Decrypt(v0, v2.AsByte());
            v0 = Aes.InverseMixColumns(v0);

            (v1, v2) = Vector128Util.Load2xUnsafe(in RefUtils.Post(ref key, 32));
            v1 = AdvSimd.ReverseElement8(v1);
            v2 = AdvSimd.ReverseElement8(v2);

            if (keylen == 52)
            {
                goto doLast;
            }

            v0 = Aes.Decrypt(v0, v1.AsByte());
            v0 = Aes.InverseMixColumns(v0);
            v0 = Aes.Decrypt(v0, v2.AsByte());
            v0 = Aes.InverseMixColumns(v0);

            (v1, v2) = Vector128Util.Load2xUnsafe(in RefUtils.Post(ref key, 32));
            v1 = AdvSimd.ReverseElement8(v1);
            v2 = AdvSimd.ReverseElement8(v2);

        doLast:;

            v0 = Aes.Decrypt(v0, v1.AsByte());
            v0 = Aes.InverseMixColumns(v0);
            v0 = Aes.Decrypt(v0, v2.AsByte());

            v0 = AdvSimd.Xor(v0, v5.AsByte());

            v0.CopyTo(to);
        }

        /// <summary>
        /// Implementation of com.sun.crypto.provider.AESCrypt.encryptBlock for the Arm platform.
        /// Derived from the OpenJDK C code 'stubGenerator_aarch64.cpp:generate_aescrypt_encryptBlock'.
        /// Keep the structure of the body of this method as close to the orignal C code as possible to facilitate porting changes.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="key"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EncryptBlock(ReadOnlySpan<byte> from, Span<byte> to, ReadOnlySpan<int> key)
        {
            //ext\openjdk\hotspot\src\cpu\aarch64\vm\stubGenerator_aarch64.cpp:2505-2597
            var keylen = key.Length;

            var v0 = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(from));

            // ld1(…, __ post(key, X)):
            // ld1(v1, v2, …) was added with .NET 9 (AdvSimd.Arm64.Load_xVector128)
            // After benchmarking Load_xVector128 is still slower than Load4xUnsafe here.
            // post post-increments the pointer, and returns the previous location.

            var (v1, v2, v3, v4) = Vector128Util.Load4xUnsafe(in RefUtils.Post(ref key, 64));
            v1 = AdvSimd.ReverseElement8(v1);
            v2 = AdvSimd.ReverseElement8(v2);
            v3 = AdvSimd.ReverseElement8(v3);
            v4 = AdvSimd.ReverseElement8(v4);
            v0 = Aes.Encrypt(v0, v1.AsByte());
            v0 = Aes.MixColumns(v0);
            v0 = Aes.Encrypt(v0, v2.AsByte());
            v0 = Aes.MixColumns(v0);
            v0 = Aes.Encrypt(v0, v3.AsByte());
            v0 = Aes.MixColumns(v0);
            v0 = Aes.Encrypt(v0, v4.AsByte());
            v0 = Aes.MixColumns(v0);

            (v1, v2, v3, v4) = Vector128Util.Load4xUnsafe(in RefUtils.Post(ref key, 64));
            v1 = AdvSimd.ReverseElement8(v1);
            v2 = AdvSimd.ReverseElement8(v2);
            v3 = AdvSimd.ReverseElement8(v3);
            v4 = AdvSimd.ReverseElement8(v4);
            v0 = Aes.Encrypt(v0, v1.AsByte());
            v0 = Aes.MixColumns(v0);
            v0 = Aes.Encrypt(v0, v2.AsByte());
            v0 = Aes.MixColumns(v0);
            v0 = Aes.Encrypt(v0, v3.AsByte());
            v0 = Aes.MixColumns(v0);
            v0 = Aes.Encrypt(v0, v4.AsByte());
            v0 = Aes.MixColumns(v0);

            (v1, v2) = Vector128Util.Load2xUnsafe(in RefUtils.Post(ref key, 32));
            v1 = AdvSimd.ReverseElement8(v1);
            v2 = AdvSimd.ReverseElement8(v2);

            if (keylen == 44)
            {
                goto doLast;
            }

            v0 = Aes.Encrypt(v0, v1.AsByte());
            v0 = Aes.MixColumns(v0);
            v0 = Aes.Encrypt(v0, v2.AsByte());
            v0 = Aes.MixColumns(v0);

            (v1, v2) = Vector128Util.Load2xUnsafe(in RefUtils.Post(ref key, 32));
            v1 = AdvSimd.ReverseElement8(v1);
            v2 = AdvSimd.ReverseElement8(v2);

            if (keylen == 52)
            {
                goto doLast;
            }

            v0 = Aes.Encrypt(v0, v1.AsByte());
            v0 = Aes.MixColumns(v0);
            v0 = Aes.Encrypt(v0, v2.AsByte());
            v0 = Aes.MixColumns(v0);

            (v1, v2) = Vector128Util.Load2xUnsafe(in RefUtils.Post(ref key, 32));
            v1 = AdvSimd.ReverseElement8(v1);
            v2 = AdvSimd.ReverseElement8(v2);

        doLast:;

            v0 = Aes.Encrypt(v0, v1.AsByte());
            v0 = Aes.MixColumns(v0);
            v0 = Aes.Encrypt(v0, v2.AsByte());

            v1 = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(key));
            v1 = AdvSimd.ReverseElement8(v1);
            v0 = AdvSimd.Xor(v0, v1.AsByte());

            v0.CopyTo(to);
        }
    }
}

#endif