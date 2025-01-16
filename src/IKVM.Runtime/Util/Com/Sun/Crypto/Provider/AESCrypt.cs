using System;
using System.Runtime.CompilerServices;

using IKVM.Attributes;

namespace IKVM.Runtime.Util.Com.Sun.Crypto.Provider
{

    /// <summary>
    /// 
    /// Intercepted implementations for 'com.sun.crypto.provider.CipherBlockChaining'.
    /// </summary>
    [HideFromJava]
    internal static class AESCrypt
    {

        /// <summary>
        /// Intercepted implementation of com.sun.crypto.provider.AESCrypt.decryptBlock. Makes a determination on
        /// whether a .NET hardware intrinsic implementation is available on the current platform and forwards to that
        /// implementation.
        /// </summary>
        /// <param name="in"></param>
        /// <param name="inOffset"></param>
        /// <param name="out"></param>
        /// <param name="outOffset"></param>
        /// <param name="K"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DecryptBlock(byte[] @in, int inOffset, byte[] @out, int outOffset, int[] K)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
#if NETCOREAPP3_0_OR_GREATER
            if (x86.AESCrypt.IsSupported)
            {
                x86.AESCrypt.DecryptBlock(@in.AsSpan(inOffset), @out.AsSpan(outOffset), K);
                return true;
            }
#endif

            return false;
#endif
        }

        /// <summary>
        /// Intercepted implementation of com.sun.crypto.provider.AESCrypt.encryptBlock. Makes a determination on
        /// whether a .NET hardware intrinsic implementation is available on the current platform and forwards to that
        /// implementation.
        /// </summary>
        /// <param name="in"></param>
        /// <param name="inOffset"></param>
        /// <param name="out"></param>
        /// <param name="outOffset"></param>
        /// <param name="K"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EncryptBlock(byte[] @in, int inOffset, byte[] @out, int outOffset, int[] K)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
#if NETCOREAPP3_0_OR_GREATER
            if (x86.AESCrypt.IsSupported)
            {
                x86.AESCrypt.EncryptBlock(@in.AsSpan(inOffset), @out.AsSpan(outOffset), K);
                return true;
            }
#endif

            return false;
#endif
        }

    }

}