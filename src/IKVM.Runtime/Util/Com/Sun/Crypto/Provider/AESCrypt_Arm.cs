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
            var v0 = Vector128Util.LoadUnsafe(ref MemoryMarshal.GetReference(from));

            var v5 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
            v5 = AdvSimd.ReverseElement8(v5);

            // AdvSimd.Arm64.Load4xVector128 was added with .NET 9
            // Manually do the steps required.
            var v1 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
            var v2 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
            var v3 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
            var v4 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
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

            v1 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
            v2 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
            v3 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
            v4 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
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

            v1 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
            v2 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
            v1 = AdvSimd.ReverseElement8(v1);
            v2 = AdvSimd.ReverseElement8(v2);

            if (key.Length == 44)
            {
                goto doLast;
            }

            v0 = Aes.Decrypt(v0, v1.AsByte());
            v0 = Aes.InverseMixColumns(v0);
            v0 = Aes.Decrypt(v0, v2.AsByte());
            v0 = Aes.InverseMixColumns(v0);

            v1 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
            v2 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
            v1 = AdvSimd.ReverseElement8(v1);
            v2 = AdvSimd.ReverseElement8(v2);

            if (key.Length == 52)
            {
                goto doLast;
            }

            v0 = Aes.Decrypt(v0, v1.AsByte());
            v0 = Aes.InverseMixColumns(v0);
            v0 = Aes.Decrypt(v0, v2.AsByte());
            v0 = Aes.InverseMixColumns(v0);

            v1 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
            v2 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
            v1 = AdvSimd.ReverseElement8(v1);
            v2 = AdvSimd.ReverseElement8(v2);

        doLast:;

            v0 = Aes.Decrypt(v0, v1.AsByte());
            v0 = Aes.InverseMixColumns(v0);
            v0 = Aes.Decrypt(v0, v2.AsByte());

            v0 = AdvSimd.Xor(v0, v5.AsByte());

            v0.CopyTo(to);
        }
    }
}

#endif