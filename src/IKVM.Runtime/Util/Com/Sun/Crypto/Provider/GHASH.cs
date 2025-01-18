using System;
using System.Runtime.CompilerServices;

using IKVM.Attributes;

namespace IKVM.Runtime.Util.Com.Sun.Crypto.Provider
{

    /// <summary>
    /// Intercepted implementations for 'com.sun.crypto.provider.GHASH'.
    /// </summary>
    [HideFromJava]
    internal static class GHASH
    {

        /// <summary>
        /// Intercepted implementation of com.sun.crypto.provider.GHASH.processBlocks. Makes a determination on
        /// whether a .NET hardware intrinsic implementation is available on the current platform and forwards to that
        /// implementation.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="inOfs"></param>
        /// <param name="blocks"></param>
        /// <param name="st"></param>
        /// <param name="subH"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ProcessBlocks(byte[] data, int inOfs, int blocks, long[] st, long[] subH)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
#if NETCOREAPP3_0_OR_GREATER
            if (GHASH_x86.IsSupported)
            {
                GHASH_x86.ProcessBlocks(data.AsSpan(inOfs), blocks, st, subH);
                return true;
            }
#endif

            return false;
#endif
        }

    }

}
