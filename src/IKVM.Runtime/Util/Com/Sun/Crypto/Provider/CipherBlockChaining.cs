using System;
using System.Runtime.CompilerServices;

using IKVM.Attributes;
using IKVM.Runtime.Accessors.Com.Sun.Crypto.Provider;

namespace IKVM.Runtime.Util.Com.Sun.Crypto.Provider
{

    /// <summary>
    /// Intercepted implementations for 'com.sun.crypto.provider.CipherBlockChaining'.
    /// </summary>
    [HideFromJava]
    internal static class CipherBlockChaining
    {

#if FIRST_PASS == false

        static AESCryptAccessor aesCryptAccessor;

        static AESCryptAccessor AESCryptAccessor => JVM.Internal.BaseAccessors.Get(ref aesCryptAccessor);

#endif

        /// <summary>
        /// Intercepted implementation of com.sun.crypto.provider.CipherBlockChaining.implDecrypt. Makes a determination on
        /// whether a .NET hardware intrinsic implementation is available on the current platform and forwards to that
        /// implementation.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cipher"></param>
        /// <param name="cipherOffset"></param>
        /// <param name="cipherLen"></param>
        /// <param name="plain"></param>
        /// <param name="plainOffset"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DecryptAESCrypt(object self, byte[] cipher, int cipherOffset, int cipherLen, byte[] plain, int plainOffset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
#if NETCOREAPP3_0_OR_GREATER
            if (plain == cipher || self is not global::com.sun.crypto.provider.CipherBlockChaining
            {
                r: { } r,
                embeddedCipher: global::com.sun.crypto.provider.AESCrypt aes
            })
            {
                return -1;
            }

            var k = AESCryptAccessor.K(aes);

            if (CipherBlockChaining_x86.IsSupported)
            {
                return CipherBlockChaining_x86.DecryptAESCrypt(cipher.AsSpan(cipherOffset), plain.AsSpan(plainOffset), k, r, cipherLen);
            }

#if NET6_0_OR_GREATER
            if (CipherBlockChaining_Arm.IsSupported)
            {
                return CipherBlockChaining_Arm.DecryptAESCrypt(cipher.AsSpan(cipherOffset), plain.AsSpan(plainOffset), k, r, cipherLen);
            }
#endif
#endif

            return -1;
#endif
        }

        /// <summary>
        /// Intercepted implementation of com.sun.crypto.provider.CipherBlockChaining.implEncrypt. Makes a determination on
        /// whether a .NET hardware intrinsic implementation is available on the current platform and forwards to that
        /// implementation.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="plain"></param>
        /// <param name="plainOffset"></param>
        /// <param name="plainLen"></param>
        /// <param name="cipher"></param>
        /// <param name="cipherOffset"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int EncryptAESCrypt(object self, byte[] plain, int plainOffset, int plainLen, byte[] cipher, int cipherOffset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
#if NETCOREAPP3_0_OR_GREATER
            if (self is not global::com.sun.crypto.provider.CipherBlockChaining
                {
                    r: { } r,
                    embeddedCipher: global::com.sun.crypto.provider.AESCrypt aes
                })
            {
                return -1;
            }

            var k = AESCryptAccessor.K(aes);

            if (AESCrypt_x86.IsSupported)
            {
                return CipherBlockChaining_x86.EncryptAESCrypt(plain.AsSpan(plainOffset), cipher.AsSpan(cipherOffset), k, r, plainLen);
            }

#if NET6_0_OR_GREATER
            if (CipherBlockChaining_Arm.IsSupported)
            {
                return CipherBlockChaining_Arm.EncryptAESCrypt(plain.AsSpan(plainOffset), cipher.AsSpan(cipherOffset), k, r, plainLen);
            }
#endif
#endif

            return -1;
#endif
        }

    }

}