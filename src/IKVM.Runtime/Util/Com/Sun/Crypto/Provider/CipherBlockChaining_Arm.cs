#if NET6_0_OR_GREATER

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;

namespace IKVM.Runtime.Util.Com.Sun.Crypto.Provider
{
    /// <summary>
    /// Arm implementations of the CipherBlockChaining intrinsic functions.
    /// </summary>
    static class CipherBlockChaining_Arm
    {

        /// <summary>
        /// Returns <c>true</c> if the current platform is supported by this implementation.
        /// </summary>
        public static bool IsSupported => AESCrypt_Arm.IsSupported;

        /// <summary>
        /// Implementation of com.sun.crypto.provider.CipherBlockChaining.implDecrypt for the Arm platform.
        /// Derived from the OpenJDK C code 'stubGenerator_aarch64.cpp:generate_cipherBlockChaining_decryptAESCrypt'.
        /// Keep the structure of the body of this method as close to the orignal C code as possible to facilitate porting changes.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="key"></param>
        /// <param name="rvec"></param>
        /// <param name="length"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DecryptAESCrypt(ReadOnlySpan<byte> from, Span<byte> to, ReadOnlySpan<int> key, Span<byte> rvec, int length)
        {
            if (length == 0)
            {
                goto finish;
            }

            var keylen = key.Length;

            var v2 = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(rvec));

            var v31 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref key, 16));
            v31 = AdvSimd.ReverseElement8(v31);

            // Initialize Values, due to branching below,
            // these are - for csc - unassigned, when
            // setting the value in the branch.
            Unsafe.SkipInit(out Vector128<int> v17);
            Unsafe.SkipInit(out Vector128<int> v18);
            Unsafe.SkipInit(out Vector128<int> v19);
            Unsafe.SkipInit(out Vector128<int> v20);

            switch (keylen)
            {
                case < 52: goto loadKeys_44;
                case 52: goto loadKeys_52;
            }

            (v17, v18) = Vector128Util.Load2xUnsafe(in RefUtils.Post(ref key, 32));
            v17 = AdvSimd.ReverseElement8(v17);
            v18 = AdvSimd.ReverseElement8(v18);
        loadKeys_52:;
            (v19, v20) = Vector128Util.Load2xUnsafe(in RefUtils.Post(ref key, 32));
            v19 = AdvSimd.ReverseElement8(v19);
            v20 = AdvSimd.ReverseElement8(v20);
        loadKeys_44:;
            var (v21, v22, v23, v24) = Vector128Util.Load4xUnsafe(in RefUtils.Post(ref key, 64));
            v21 = AdvSimd.ReverseElement8(v21);
            v22 = AdvSimd.ReverseElement8(v22);
            v23 = AdvSimd.ReverseElement8(v23);
            v24 = AdvSimd.ReverseElement8(v24);
            var (v25, v26, v27, v28) = Vector128Util.Load4xUnsafe(in RefUtils.Post(ref key, 64));
            v25 = AdvSimd.ReverseElement8(v25);
            v26 = AdvSimd.ReverseElement8(v26);
            v27 = AdvSimd.ReverseElement8(v27);
            v28 = AdvSimd.ReverseElement8(v28);
            var (v29, v30) = Vector128Util.Load2xUnsafe(in MemoryMarshal.GetReference(key));
            v29 = AdvSimd.ReverseElement8(v29);
            v30 = AdvSimd.ReverseElement8(v30);

        aes_loop:;
            var v0 = Vector128Util.LoadUnsafe(in RefUtils.Post(ref from, 16));
            var v1 = AdvSimd.Or(v0, v0);

            switch (keylen)
            {
                case < 52: goto rounds_44;
                case 52: goto rounds_52;
            }

            v0 = Aes.InverseMixColumns(Aes.Decrypt(v0, v17.AsByte()));
            v0 = Aes.InverseMixColumns(Aes.Decrypt(v0, v18.AsByte()));
        rounds_52:;
            v0 = Aes.InverseMixColumns(Aes.Decrypt(v0, v19.AsByte()));
            v0 = Aes.InverseMixColumns(Aes.Decrypt(v0, v20.AsByte()));
        rounds_44:;
            v0 = Aes.InverseMixColumns(Aes.Decrypt(v0, v21.AsByte()));
            v0 = Aes.InverseMixColumns(Aes.Decrypt(v0, v22.AsByte()));
            v0 = Aes.InverseMixColumns(Aes.Decrypt(v0, v23.AsByte()));
            v0 = Aes.InverseMixColumns(Aes.Decrypt(v0, v24.AsByte()));
            v0 = Aes.InverseMixColumns(Aes.Decrypt(v0, v25.AsByte()));
            v0 = Aes.InverseMixColumns(Aes.Decrypt(v0, v26.AsByte()));
            v0 = Aes.InverseMixColumns(Aes.Decrypt(v0, v27.AsByte()));
            v0 = Aes.InverseMixColumns(Aes.Decrypt(v0, v28.AsByte()));
            v0 = Aes.InverseMixColumns(Aes.Decrypt(v0, v29.AsByte()));
            v0 = Aes.Decrypt(v0, v30.AsByte());
            v0 = AdvSimd.Xor(v0, v31.AsByte());
            v0 = AdvSimd.Xor(v0, v2.AsByte());

            v0.StoreUnsafe(ref RefUtils.Post(ref to, 16));
            v2 = AdvSimd.Or(v1, v1);

            if ((length -= 16) != 0)
            {
                goto aes_loop;
            }

            v2.CopyTo(rvec);

        finish:;
        }
    }
}

#endif