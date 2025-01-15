using IKVM.Runtime;
using IKVM.Runtime.Accessors.Com.Sun.Crypto.Provider;
using System;
using System.Runtime.CompilerServices;

namespace IKVM.Java.Externs.com.sun.crypto.provider;

internal static class CipherBlockChaining
{
#if !FIRST_PASS
    private static AESCryptAccessor aesCryptAccessor;

    private static AESCryptAccessor AESCryptAccessor => JVM.Internal.BaseAccessors.Get(ref aesCryptAccessor);
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool DecryptAESCrypt(object self, byte[] cipher, int cipherOffset, int cipherLen, byte[] plain, int plainOffset)
    {
#if !FIRST_PASS && NETCOREAPP3_0_OR_GREATER
        if (plain == cipher || self is not global::com.sun.crypto.provider.CipherBlockChaining
            {
                r: { } r,
                embeddedCipher: global::com.sun.crypto.provider.AESCrypt aes
            })
        {
            return false;
        }

        var k = AESCryptAccessor.K(aes);

        if (X86.AESCrypt.IsSupported)
        {
            X86.CipherBlockChaining.DecryptAESCrypt(cipher.AsSpan(cipherOffset), plain.AsSpan(plainOffset), k, r, cipherLen);
            return true;
        }
#endif
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EncryptAESCrypt(object self, byte[] plain, int plainOffset, int plainLen, byte[] cipher, int cipherOffset)
    {
#if !FIRST_PASS && NETCOREAPP3_0_OR_GREATER
        if (self is not global::com.sun.crypto.provider.CipherBlockChaining
            {
                r: { } r,
                embeddedCipher: global::com.sun.crypto.provider.AESCrypt aes
            })
        {
            return false;
        }

        var k = AESCryptAccessor.K(aes);

        if (X86.AESCrypt.IsSupported)
        {
            X86.CipherBlockChaining.EncryptAESCrypt(plain.AsSpan(plainOffset), cipher.AsSpan(cipherOffset), k, r, plainLen);
            return true;
        }
#endif
        return false;
    }
}